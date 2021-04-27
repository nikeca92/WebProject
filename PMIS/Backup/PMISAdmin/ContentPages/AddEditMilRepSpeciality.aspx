<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditMilRepSpeciality.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.AddEditMilRepSpeciality" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfMilReportSpecialityID" runat="server" />
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
                               <asp:Label ID="lblMilRepSpecialityTypes" runat="server" CssClass="InputLabel" Text="Тип:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddMilRepSpecialityTypes" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblMilReportingSpecialityCode" runat="server" CssClass="InputLabel" Text="Код:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMilReportingSpecialityCode" runat="server" CssClass="RequiredInputField" Width="80" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblMilReportingSpecialityName" runat="server" CssClass="InputLabel" Text="Име:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMilReportingSpecialityName" runat="server" CssClass="RequiredInputField" Width="450" MaxLength="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblMilitaryForceSort" runat="server" CssClass="InputLabel" Text="Род войски:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddMilitaryForceSort" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               <asp:CheckBox runat="server" ID="chkActive" Checked="true" />
                            </td>
                            <td style="text-align: left;">
                               <asp:Label ID="lblActive" runat="server" CssClass="InputLabel" Text="Активна"></asp:Label>
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

