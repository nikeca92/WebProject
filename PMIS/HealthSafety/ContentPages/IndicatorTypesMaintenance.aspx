<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="IndicatorTypesMaintenance.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.IndicatorTypesMaintenance" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfFromHome" runat="server" />

<div style="height: 20px;">
</div>

<center>
        <div style="min-height: 400px; width: 100%; text-align: center;" runat="server" id="MainDiv">
            <asp:Label runat="server" ID="lblHeader" CssClass="HeaderText" ></asp:Label>

            <div style="height: 15px;"></div>
            
            <div id="divIndicatorTypesTable" runat="server"></div>                        
            
            <div style="height: 10px;"></div>
            
            <asp:Label runat="server" ID="lblStatus"></asp:Label> 
            
            <div style="height: 10px;"></div>
                        
            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
        </div>                 
</center>    

<div id="IndicatorTypeLightBox" class="IndicatorTypeLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfIndicatorTypeID" runat="server" />
    <center>
        <table width="80%" style="text-align:center;">
            <colgroup style="width:50%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Елемент</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblIndicatorTypeName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtIndicatorTypeName" type="text" class="RequiredInputField" style="width: 220px" maxlength="500"/>                
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblSeq" class="InputLabel">Подредба:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtSeq" type="text" class="RequiredInputField" style="width: 50px"/>                
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
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveIndicatorType();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideIndicatorTypeLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </center>            
</div>
    
<div style="height: 20px;">
</div>

<asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshIndicatorTypes_Click" CssClass="HiddenButton" />    

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 
<script type="text/javascript">    
    
    // Display light box with indicator type properties (for editing or adding new)
    function ShowIndicatorTypeLightBox(indicatorTypeID, indicatorTypeName, seq)
    {            
      document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value = indicatorTypeID; // setting indicator type ID(0 - if new indicator type)
      
      // initialize value
      document.getElementById("txtIndicatorTypeName").value = indicatorTypeName;
      document.getElementById("txtSeq").value = seq;

      // clean message label in the light box and hide it            
      document.getElementById("spanLightBoxMessage").style.display = "none";
      document.getElementById("spanLightBoxMessage").innerHTML = "";
      
      //shows the light box and "disable" rest of the page
      document.getElementById("HidePage").style.display = "";
      document.getElementById("IndicatorTypeLightBox").style.display = "";
      CenterLightBox("IndicatorTypeLightBox");
    }
   
   // Close the light box and refresh indicator types table
   function HideIndicatorTypeLightBox()
   {
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("IndicatorTypeLightBox").style.display = "none";
   }
   
   // Validate indicator type properties in the light box and generates appropriate error messages, if needed
   function ValidateIndicatorType()
   {
      var res = true;
      
      var spanLightBoxMessage = document.getElementById("spanLightBoxMessage");
      spanLightBoxMessage.innerHTML = "";
      
      if (TrimString(document.getElementById("txtIndicatorTypeName").value) == "")
      {
          spanLightBoxMessage.innerHTML += GetErrorMessageMandatory("Наименование") + "<br />";
        res = false;
      }
      
      if (TrimString(document.getElementById("txtSeq").value) == "")
      {
          spanLightBoxMessage.innerHTML += GetErrorMessageMandatory("Подредба") + "<br />";
        res = false;
      }
      else if (!isInt(TrimString(document.getElementById("txtSeq").value)))
      {
          spanLightBoxMessage.innerHTML += GetErrorMessageNumber("Подредба") + "<br />";
        res = false;
      }      
      
      return res;
   }
   
   // Saves indicator type through ajax request, if light box values are valid, or displays generated error messages
   function SaveIndicatorType()
   {
      if(ValidateIndicatorType())
      {
          var url = "IndicatorTypesMaintenance.aspx?AjaxMethod=JSSaveIndicatorType";    

          var params = "IndicatorTypeID=" + document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value +
                       "&Seq=" + TrimString(document.getElementById("txtSeq").value) +
                       "&IndicatorTypeName=" + custEncodeURI(TrimString(document.getElementById("txtIndicatorTypeName").value));

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
	        	if (document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value == "0")
	        	    action = "ADDED";
	        	else
	        	    action = "EDITED";
	        	document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;
	        	
	        	HideIndicatorTypeLightBox();	        	
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
   function DeleteIndicatorType(indicatorTypeID, indicatorTypeName)
   {
        YesNoDialog('Желаете ли да изтриете елемента "' + indicatorTypeName + '"?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "IndicatorTypesMaintenance.aspx?AjaxMethod=JSDeleteIndicatorType";
                var params = "";
                params += "IndicatorTypeID=" + indicatorTypeID;
                
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
   
   function EditIndicators(indicatorTypeId)
   {
       JSRedirect("IndicatorsMaintenance.aspx?IndicatorTypeID=" + indicatorTypeId);   
   }

</script>
 
 </asp:Content>
