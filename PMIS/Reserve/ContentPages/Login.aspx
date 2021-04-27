<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PMIS.Reserve.ContentPages.Login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

   <div style="height: 60px"></div>
   <table style="width: 100%;">
      <tr>
         <td colspan="2" class="HeaderText">
            Вход в системата
         </td>
      </tr>
      <tr style="height: 15px;"></tr>
      <tr>
         <td class="InputLabel" style="width: 50%;">
            Потребителско име:
         </td>
         <td>

            <asp:TextBox runat="server" ID="txtUsername" CssClass="InputField"></asp:TextBox>

         </td>
      </tr>
      <tr>
         <td class="InputLabel">Парола за достъп:</td>
         <td>
             <asp:TextBox runat="server" ID="txtPassword" CssClass="InputField" TextMode="Password"></asp:TextBox>
         </td>
      </tr>
      <tr style="height: 7px;"></tr>
      <tr>
         <td colspan="2" class="ErrorText">
            <asp:Label runat="server" ID="lblMessage"></asp:Label>
         </td>
      </tr>
      <tr style="height: 8px;"></tr>
      <tr>
         <td colspan="2" style="text-align: center;">
            <asp:LinkButton ID="btnLogin" runat="server" CssClass="Button" OnClick="btnLogin_Click"><i></i><div style="width:70px; padding-left:5px;">Вход</div><b></b></asp:LinkButton>
         </td>
      </tr>
   </table>
   
   <input type="hidden" id="Login" value="true" />
   <input type="hidden" id="CanLeave" value="true" />
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

   Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

   function EndRequestHandlerPage(sender, args) {
      PageLoad();
   }

   function PageLoad() {
      document.getElementById("<% Response.Write(txtUsername.ClientID); %>").focus();
   }
   
   //Use this function to be able to login in the system by typing the credentials and pressing Enter (i.e. w/o using the mouse)
   function FormKeyDown(event, btnLoginClientId)
   {
       if(event.which || event.keyCode)
       {
          if ((event.which == 13) || (event.keyCode == 13)) 
          {
             if (typeof(document.getElementById(btnLoginClientId).click) == 'function') 
                document.getElementById(btnLoginClientId).click(); 
             else 
             {
                var href = document.getElementById(btnLoginClientId).href.replace('javascript:', '');
                
                if(href.substring(href.length - 1) == "#")
                {
                   //This is from FixLinks() from the logic related that checks for unsaved changes
                   var doonclick = document.getElementById(btnLoginClientId).getAttribute("doonclick");
                   
                   if(doonclick != null && doonclick != "")
                      eval(doonclick);
                }
                else
                {
                   eval(document.getElementById(btnLoginClientId).href.replace('javascript:', '')); 
                }
             }
                
             return false;
          }
       } 
       else 
       {
          return true
       };
   }
 </script>
   
</asp:Content>

