<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PasswordPolicy.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.PasswordPolicyPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <div style="height: 20px"></div>
    <center>
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Пароли</span>
                </td>                
            </tr>
            <tr style="height: 20px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="width: 750px;">
                        <tr>
                            <td style="text-align: right; width: 30%;">
                               <asp:CheckBox runat="server" ID="chkAllowBlankSpace" AutoPostBack="True" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblAllowBlankSpace" runat="server" CssClass="InputLabel" Text="Разрешава се интервал"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkCaseSensitivity" AutoPostBack="True" OnCheckedChanged="chkCaseSensitivity_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblCaseSensitivity" runat="server" CssClass="InputLabel" Text="Изискват се и малки и главни букви"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="rowCaseInsensitive">
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkLetterChars" AutoPostBack="True" OnCheckedChanged="chkLetterChars_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblLetterChars" runat="server" CssClass="InputLabel" Text="Изискват се "></asp:Label>
                               <asp:TextBox runat="server" ID="txtLetterChars" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblLetterChars2" runat="server" CssClass="InputLabel" Text=" букви"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="rowCaseSensitive1">
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkLowerCaseChars" AutoPostBack="True" OnCheckedChanged="chkLowerCaseChars_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblLowerCaseChars" runat="server" CssClass="InputLabel" Text="Изискват се "></asp:Label>
                               <asp:TextBox runat="server" ID="txtLowerCaseChars" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblLowerCaseChars2" runat="server" CssClass="InputLabel" Text=" малки букви"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="rowCaseSensitive2">
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkUpperCaseChars" AutoPostBack="True" OnCheckedChanged="chkUpperCaseChars_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblUpperCaseChars" runat="server" CssClass="InputLabel" Text="Изискват се "></asp:Label>
                               <asp:TextBox runat="server" ID="txtUpperCaseChars" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblUpperCaseChars2" runat="server" CssClass="InputLabel" Text=" главни букви"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkNumericChars" AutoPostBack="True" OnCheckedChanged="chkNumericChars_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblNumericChars" runat="server" CssClass="InputLabel" Text="Изискват се "></asp:Label>
                               <asp:TextBox runat="server" ID="txtNumericChars" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblNumericChars2" runat="server" CssClass="InputLabel" Text=" цифри"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkSpecialChars" AutoPostBack="True" OnCheckedChanged="chkSpecialChars_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblSpecialChars" runat="server" CssClass="InputLabel" Text="Изискват се "></asp:Label>
                               <asp:TextBox runat="server" ID="txtSpecialChars" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblSpecialChars2" runat="server" CssClass="InputLabel" Text=" специални символа"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkMinLenght" AutoPostBack="True" OnCheckedChanged="chkMinLenght_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblMinLenght" runat="server" CssClass="InputLabel" Text="Минимална дължина - "></asp:Label>
                               <asp:TextBox runat="server" ID="txtMinLenght" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblMinLenght2" runat="server" CssClass="InputLabel" Text=" символа"></asp:Label>
                            </td>
                        </tr>
                        <tr><td style="height: 10px;"></td></tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkExpiresAfterDays" AutoPostBack="True" OnCheckedChanged="chkExpiresAfterDays_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblExpiresAfterDays" runat="server" CssClass="InputLabel" Text="Паролата изтича след "></asp:Label>
                               <asp:TextBox runat="server" ID="txtExpiresAfterDays" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblExpiresAfterDays2" runat="server" CssClass="InputLabel" Text=" дни"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkCannotReusePrevPasswords" AutoPostBack="True" OnCheckedChanged="chkCannotReusePrevPasswords_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblCannotReusePrevPasswords" runat="server" CssClass="InputLabel" Text="Не могат да се преизползват последните "></asp:Label>
                               <asp:TextBox runat="server" ID="txtCannotReusePrevPasswords" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblCannotReusePrevPasswords2" runat="server" CssClass="InputLabel" Text=" пароли"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:CheckBox runat="server" ID="chkBlockUserAfterFailedLogins" AutoPostBack="True" OnCheckedChanged="chkBlockUserAfterFailedLogins_Click" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblBlockUserAfterFailedLogins" runat="server" CssClass="InputLabel" Text="Потребителят се блокира след "></asp:Label>
                               <asp:TextBox runat="server" ID="txtBlockUserAfterFailedLogins" CssClass="RequiredInputField" Width="20"></asp:TextBox>
                               <asp:Label ID="lblBlockUserAfterFailedLogins2" runat="server" CssClass="InputLabel" Text=" неуспешни опита за достъп"></asp:Label>
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

