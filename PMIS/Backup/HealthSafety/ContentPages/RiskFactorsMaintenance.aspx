<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="RiskFactorsMaintenance.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.RiskFactorsMaintenance" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfFromHome" runat="server" />
<asp:HiddenField ID="hfRiskFactorTypeID" runat="server" />

<div style="height: 20px;">
</div>

<center>
        <div style="min-height: 400px; width: 100%; text-align: center;" runat="server" id="MainDiv">
            <asp:Label runat="server" ID="lblHeader" CssClass="HeaderText" ></asp:Label>
            <div style="text-align: center; padding-top: 5px;">
                <span id="lblRiskFactorTypeName" runat="server" class="HeaderText" style="font-size: 1.3em;"></span>
            </div>

            <div style="height: 15px;"></div>
            
            <div id="divRiskFactorsTable" runat="server"></div>                        
            
            <div style="height: 10px;"></div>
            
            <asp:Label runat="server" ID="lblStatus"></asp:Label> 
            
            <div style="height: 10px;"></div>
                        
            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
        </div>                 
</center>    

<div id="RiskFactorLightBox" class="RiskFactorLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfRiskFactorID" runat="server" />
    <center>
        <table width="90%" style="text-align:center;">
            <colgroup style="width:30%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Фактор</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblRiskFactorName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtRiskFactorName" type="text" class="RequiredInputField" style="width: 320px" maxlength="500"/>                
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
            <tr style="min-height: 17px">  
                 <td style="text-align: right;">
                    <input id="chkAllowAddManually" type="checkbox" class="InputField" style="width: 15px"/>
                </td>              
                <td style="text-align: left;">                    
                    <label for="chkAllowAddManually" style="width: 250px;"><span class="InputLabel" style="width: 250px;">Позволява ръчно добавяне на опасности</span></label>                
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
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveRiskFactor();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideRiskFactorLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
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

<asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshRiskFactors_Click" CssClass="HiddenButton" />    

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 
<script type="text/javascript">

    // Display light box with risk factor properties (for editing or adding new)
    function ShowRiskFactorLightBox(riskFactorID, riskFactorName, seq, allowAddManually)
    {
        document.getElementById("<%= hfRiskFactorID.ClientID %>").value = riskFactorID; // setting risk factor ID(0 - if new risk factor)
      
        // initialize value
        document.getElementById("txtRiskFactorName").value = riskFactorName;
        document.getElementById("txtSeq").value = seq;
        document.getElementById("chkAllowAddManually").checked = allowAddManually == 1;

        // clean message label in the light box and hide it            
        document.getElementById("spanLightBoxMessage").style.display = "none";
        document.getElementById("spanLightBoxMessage").innerHTML = "";
      
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("RiskFactorLightBox").style.display = "";
        CenterLightBox("RiskFactorLightBox");
    }

    // Close the light box and refresh risk factor types table
    function HideRiskFactorLightBox()
   {
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("RiskFactorLightBox").style.display = "none";
   }

   // Validate risk factor properties in the light box and generates appropriate error messages, if needed
   function ValidateRiskFactor()
   {
      var res = true;
      
      var spanLightBoxMessage = document.getElementById("spanLightBoxMessage");
      spanLightBoxMessage.innerHTML = "";

      if (TrimString(document.getElementById("txtRiskFactorName").value) == "")
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

   // Saves risk factor through ajax request, if light box values are valid, or displays generated error messages
   function SaveRiskFactor()
   {
       if (ValidateRiskFactor())
      {
          var url = "RiskFactorsMaintenance.aspx?AjaxMethod=JSSaveRiskFactor";

          var params = "RiskFactorTypeID=" + document.getElementById("<%= hfRiskFactorTypeID.ClientID %>").value +
                       "&RiskFactorID=" + document.getElementById("<%= hfRiskFactorID.ClientID %>").value +
                       "&Seq=" + TrimString(document.getElementById("txtSeq").value) +
                       "&RiskFactorName=" + custEncodeURI(TrimString(document.getElementById("txtRiskFactorName").value)) +
                       "&AllowAddManually=" + document.getElementById("chkAllowAddManually").checked;

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
	        	if (document.getElementById("<%= hfRiskFactorID.ClientID %>").value == "0")
	        	    action = "ADDED";
	        	else
	        	    action = "EDITED";
	        	document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;

	        	HideRiskFactorLightBox();	        	
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

   // Delete risk factor type through ajax request
   function DeleteRiskFactor(riskFactorID, riskFactorName)
   {
       YesNoDialog('Желаете ли да изтриете фактора "' + riskFactorName + '"?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "RiskFactorsMaintenance.aspx?AjaxMethod=JSDeleteRiskFactor";
            var params = "";
            params += "RiskFactorTypeID=" + document.getElementById("<%= hfRiskFactorTypeID.ClientID %>").value;
            params += "&RiskFactorID=" + riskFactorID;
            
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

   function EditHazards(riskFactorTypeId, riskFactorId)
   {
       JSRedirect("HazardsMaintenance.aspx?RiskFactorTypeID=" + riskFactorTypeId + "&RiskFactorID=" + riskFactorId);   
   }

</script>
 
 </asp:Content>
