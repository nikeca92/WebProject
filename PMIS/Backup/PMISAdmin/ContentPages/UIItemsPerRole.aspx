<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="UIItemsPerRole.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.UIItemsPerRole" 
    Title="Дефиниране на права и достъп според потребителската роля" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfSpecificRoleID" runat="server" />
    <div style="height: 20px"></div>
    <center>
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Дефиниране на права и достъп според потребителската роля</span>
                </td>                
            </tr>
            <tr style="height: 20px;">
               <td></td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <div class="InfoArea">Този екран служи за дефиниране на права и достъп за различните потребителски роли в системата</div>
                </td>                
            </tr>
            <tr style="height: 20px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="width: 550px;">
                        </tr>
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Модул:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddModules" CssClass="InputField" Width="200" OnSelectedIndexChanged="ddModules_Change" AutoPostBack="true" 
                                    CheckForChanges="true" UnsavedCheckSkipMe="true"></asp:DropDownList>
                            </td>
                        </tr>
                        </tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddRoles" CssClass="InputField" Width="200" OnSelectedIndexChanged="ddRoles_Change" AutoPostBack="true" 
                                    CheckForChanges="true" UnsavedCheckSkipMe="true"></asp:DropDownList>
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
               <td style="text-align: center;">
                  <table style="margin: 0 auto;">
                     <tr>
                        <td style="vertical-align: top;">
                            <div style="text-align: left; width: 400px;">
                                <asp:TreeView ID="treeUIItems" runat="server" ShowLines="true"
                                     OnSelectedNodeChanged="treeUIItems_SelectionChanged"
                                     SelectedNodeStyle-BackColor="#FFFFCC" 
                                     EnableClientScript="false">
                                </asp:TreeView><!-- Set EnableClientScript to False to get only the visible part of the tree iterated by the UpdatePanel logic; http://blogs.msdn.com/b/carloc/archive/2008/04/14/very-slow-treeview-in-updatepanel.aspx -->
                            </div>
                        </td>
                        <td style="vertical-align: top;">
                            <div style="text-align: left; width: 450px;">
                                <table style="width: 100%;">
                                   <tr>
                                      <td style="vertical-align: top; text-align: right; width: 25%;">
                                         <asp:Label runat="server" ID="lblUIItemNameLabel" Text="Елемент:" CssClass="InputLabel"></asp:Label>
                                      </td>
                                      <td>
                                         <asp:Label runat="server" ID="lblUIItemName" CssClass="ReadOnlyValue"></asp:Label>
                                      </td>
                                   </tr>
                                   <tr runat="server" id="rowAccessLevel">
                                      <td style="vertical-align: top; text-align: right;">
                                         <asp:Label runat="server" ID="lblUIAccessLevel" Text="Състояние:" CssClass="InputLabel"></asp:Label>
                                      </td>
                                      <td>
                                         <asp:RadioButton runat="server" ID="radEnabled" GroupName="AccessLevel" /> <br />
                                         <asp:RadioButton runat="server" ID="radDisabled" GroupName="AccessLevel" /> <br />
                                         <asp:RadioButton runat="server" ID="radHidden" GroupName="AccessLevel" />
                                      </td>
                                   </tr>
                                   <tr runat="server" id="rowAccessOnlyOwnData">
                                      <td></td>
                                      <td style="padding-top: 5px;">
                                         <asp:CheckBox runat="server" ID="chkAccessOnlyOwnData" Text="Достъп само до създадените от него записи"/>
                                      </td>
                                   </tr>
                                </table>
                            </div>
                        </td>
                     </tr>
                  </table>
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
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="height: 12px;">
                <td>
                    &nbsp;
                </td>
            </tr>          
        </table>
    </center>

<asp:HiddenField runat="server" ID="hdnUIItemID" />
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

