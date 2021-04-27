<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="PMIS.Reserve.ContentPages.MaintenancePage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<center>
        <div style="min-height: 400px; width: 100%; text-align: center;" runat="server" id="MainDiv">
            <asp:Label runat="server" ID="lblHeader" CssClass="HeaderText" ></asp:Label>

            <div style="height: 15px;"></div>
            
            <div style="text-align: center;">
                <div style="width: 570px; margin: 0 auto; text-align: left; vertical-align: top; height: 30px;">
                   <div style="display: inline;">
                      <asp:Button runat="server" ID="btnFirstHelper" OnClick="btnFirst_Click" style="display: none;" />
                      <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png" AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" OnClientClick="CustomCheckFormSave(function(){ResetAlawaysAsk(); document.getElementById(btnFirstHelperClientID).click();}); return false;" />
                      <asp:Button runat="server" ID="btnPrevHelper" OnClick="btnPrev_Click" style="display: none;" />
                      <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png" AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" OnClientClick="CustomCheckFormSave(function(){ResetAlawaysAsk(); document.getElementById(btnPrevHelperClientID).click();}); return false;" />
                      <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
                      <asp:Button runat="server" ID="btnNextHelper" OnClick="btnNext_Click" style="display: none;" />
                      <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png" AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" OnClientClick="CustomCheckFormSave(function(){ResetAlawaysAsk(); document.getElementById(btnNextHelperClientID).click();}); return false;" />
                      <asp:Button runat="server" ID="btnLastHelper" OnClick="btnLast_Click" style="display: none;" />
                      <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png" AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" OnClientClick="CustomCheckFormSave(function(){ResetAlawaysAsk(); document.getElementById(btnLastHelperClientID).click();}); return false;" />            
                      <span style="padding: 30px">&nbsp;</span>
                      <span style="text-align: right;">Отиди на страница</span>
                      <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px" UnsavedCheckSkipMe="true"></asp:TextBox>
                      <asp:Button runat="server" ID="btnGotoHelper" OnClick="btnGoto_Click" style="display: none;" />
                      <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png" AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" OnClientClick="CustomCheckFormSave(function(){ResetAlawaysAsk(); document.getElementById(btnGotoHelperClientID).click();}); return false;" /> 
                   </div>
                </div>
            </div>
            
            <asp:Table runat="server" ID="tblMaintTable" CssClass="CommonHeaderTable"></asp:Table>
            
            <div style="height: 10px;"></div>
            
            <asp:Label runat="server" ID="lblStatus"  ></asp:Label> 
            
            <div style="height: 10px;"></div>
            
            <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="TableGrid_Save_Click" OnClientClick="ResetAlawaysAsk();"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton> &nbsp;
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="Button" OnClick="btnCancel_Click" CheckForChanges="true" ><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
        </div>
        
        <div style="min-height: 400px; width: 100%; text-align: center;" runat="server" id="MessageDiv" visible="false">
            <asp:Label runat="server" ID="lblMessage" CssClass="PageHeader"></asp:Label>
        </div>                
    </center>
    
    <asp:HiddenField runat="server" ID="UniqueWindowID" Value="" />
    <asp:HiddenField ID="hdnPageIdx" runat="server" Value="1" />
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
<div style="height: 20px;">
</div>

</ContentTemplate>
 </asp:UpdatePanel>
 
<script type="text/javascript">
var btnFirstHelperClientID = "<%= btnFirstHelper.ClientID %>";
var btnPrevHelperClientID = "<%= btnPrevHelper.ClientID %>";
var btnNextHelperClientID = "<%= btnNextHelper.ClientID %>";
var btnLastHelperClientID = "<%= btnLastHelper.ClientID %>";
var btnGotoHelperClientID = "<%= btnGotoHelper.ClientID %>";

function ShowButtons()
{
   for(var i = 0; i < document.getElementsByTagName("input").length; i++)
   {
      el = document.getElementsByTagName("input")[i];		           	           		           		               		           		       

      if(el.getAttribute("showonload"))
      {		              		           
          if(el.getAttribute("showonload") == "true")
             el.style.display = "";
      }
   }

   for(var i = 0; i < document.getElementsByTagName("submit").length; i++)
   {
      el = document.getElementsByTagName("submit")[i];		           	           		           		               		           		       

      if(el.getAttribute("showonload"))
      {		              		           
         if(el.getAttribute("showonload") == "true")
            el.style.display = "";
      }
   }		        		        
}		 		 

function EndRequestHandlerPage(sender, args)
{
   if (args.get_error() == undefined)
      ShowButtons();
   else
      alert("There was an error" + args.get_error().message);
}

function load() 
{
    hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);
    ShowButtons();
}

window.addEventListener?window.addEventListener("load",load,false):window.attachEvent("onload",load);

</script>
 
 </asp:Content>
