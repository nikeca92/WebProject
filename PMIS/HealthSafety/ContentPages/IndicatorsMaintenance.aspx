<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="IndicatorsMaintenance.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.IndicatorsMaintenance" 
         Title="Управление на потребителските роли" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>

<script type="text/javascript">
    function SortTableBy(sort)
    {
        if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
        {
            sort = sort + 100;
        }
        
        document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;                           
        document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();               
    }

    // Display light box with indicator properties (for editing or adding new)
    function ShowIndicatorLightBox(indicatorId, indicatorName)
    {            
      document.getElementById("<%= hfIndicatorId.ClientID %>").value = indicatorId; // setting indicator ID(0 - if new indicator)
      
      // initialize value
      document.getElementById("txtIndicatorName").value = indicatorName;

      // clean message label in the light box and hide it            
      document.getElementById("spanLightBoxMessage").style.display = "none";
      document.getElementById("spanLightBoxMessage").innerHTML = "";
      
      //shows the light box and "disable" rest of the page
      document.getElementById("HidePage").style.display = "";
      document.getElementById("IndicatorLightBox").style.display = "";
      CenterLightBox("IndicatorLightBox");
    }
   
   // Close the light box and refresh indicators table
   function HideIndicatorLightBox()
   {
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("IndicatorLightBox").style.display = "none";
   }
   
   // Show indicator light box to add new indicator
   function AddIndicator()
   {
      ShowIndicatorLightBox(0, "");
   }
   
   // Validate indicator properties in the light box and generates appropriate error messages, if needed
   function ValidateIndicator()
   {
      var res = true;
      
      var spanLightBoxMessage = document.getElementById("spanLightBoxMessage");
      spanLightBoxMessage.innerHTML = "";
      
      if (TrimString(document.getElementById("txtIndicatorName").value) == "")
      {
          spanLightBoxMessage.innerHTML += GetErrorMessageMandatory("Наименование") + "<br />";
        res = false;
      }
      
      return res;
   }
   
   // Saves indicator through ajax request, if light box values are valid, or displays generated error messages
   function SaveIndicator()
   {
      if(ValidateIndicator())
      {
          var url = "IndicatorsMaintenance.aspx?AjaxMethod=JSSaveIndicator";    

          var params = "IndicatorID=" + document.getElementById("<%= hfIndicatorId.ClientID %>").value +
                       "&IndicatorTypeID=" + document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value +
                       "&IndicatorName=" + custEncodeURI(TrimString(document.getElementById("txtIndicatorName").value));               	                          

          function response_handler(xml)
          {			
          
            var hideDialog = true;
            var resultMsg = xmlValue(xml, "response");
	        if (resultMsg != "OK")
	        {
                var lightBoxMessage = document.getElementById("spanLightBoxMessage");
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
	            hideDialog = false;
	            lightBoxMessage.innerHTML = resultMsg;
	        }	        
	        
	        if (hideDialog)
	        {	
	        	var action;
	        	if (document.getElementById("<%= hfIndicatorId.ClientID %>").value == "0")
	        	    action = "ADDED";
	        	else
	        	    action = "EDITED";
	        	document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;
	        	
	        	HideIndicatorLightBox();	        	
	         }
	      }

	      var myAJAX = new AJAX(url, true, params, response_handler);
	      myAJAX.Call();
      }
      else
      {
        document.getElementById("spanLightBoxMessage").style.display = "";
      }
   }
   
   // Delete ndicator type through ajax request
   function DeleteIndicator(indicatorId, indicatorName)
   {
        YesNoDialog('Желаете ли да изтриете показателя "' + indicatorName + '"?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "IndicatorsMaintenance.aspx?AjaxMethod=JSDeleteIndicator";
                var params = "";
                params += "IndicatorID=" + indicatorId +
                          "&IndicatorTypeID=" + document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value;
                
                function response_handler(xml)
                {			    	                
	                if(xmlValue(xml, "response") == "OK")
	                {
	                    document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "DELETED";
	                    document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
	                }
	            }

	            var myAJAX = new AJAX(url, true, params, response_handler);
	            myAJAX.Call();  
        }
   }

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfIndicatorTypeID" runat="server" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Показатели на елемент на специфичните условия на труд</span>
</div>
<div style="text-align: center;">
   <span id="lblIndicatorTypeName" runat="server" class="HeaderText"></span>
</div>

<div style="height: 40px;">&nbsp;</div>

<div style="text-align: center;">
    <div style="width: 580px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">              
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClientClick="AddIndicator(); return false;"><i></i><div style="width:90px; padding-left:5px;">Нов показател</div><b></b></asp:LinkButton>
       
       <span style="padding: 10px">&nbsp;</span>
       
       
       <div style="display: inline; position: relative; top: -16px;">
          <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png" AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" />                        
          <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png" AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" />                        
          <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
          <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png" AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" />                        
          <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png" AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" />            
          <span style="padding: 30px">&nbsp;</span>
          <span style="text-align: right;">Отиди на страница</span>
          <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
          <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png" AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" /> 
       </div>
    </div>
</div>

<div style="text-align: center;">
    <div style="width: 600px; margin: 0 auto;">
       <div runat="server" id="pnlIndicatorsGrid" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 10px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div id="IndicatorLightBox" class="IndicatorLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfIndicatorId" runat="server" />
    <center>
        <table width="80%" style="text-align:center;">
            <colgroup style="width:50%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Показател</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblIndicatorName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtIndicatorName" type="text" class="RequiredInputField" style="width: 220px" maxlength="500"/>                
                </td>
            </tr>
            <tr style="height: 30px">
                <td colspan="2"> 
                    <span id="spanLightBoxMessage" class="ErrorText" style="display: none;">
                    </span> &nbsp;
               </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="margin: 0 auto;">
                       <tr>
                          <td>
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveIndicator();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideIndicatorLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </center>            
</div>

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshIndicators_Click" CssClass="HiddenButton" />    

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
