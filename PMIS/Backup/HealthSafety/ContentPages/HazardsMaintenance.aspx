<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="HazardsMaintenance.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.HazardsMaintenance" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfFromHome" runat="server" />
<asp:HiddenField ID="hfRiskFactorTypeID" runat="server" />
<asp:HiddenField ID="hfRiskFactorID" runat="server" />

<div style="height: 20px;">
</div>

<center>
        <div style="min-height: 400px; width: 100%; text-align: center;" runat="server" id="MainDiv">
            <asp:Label runat="server" ID="lblHeader" CssClass="HeaderText" ></asp:Label>
            <div style="text-align: center; padding-top: 5px;">
                <span id="lblRiskFactorTypeName" runat="server" class="HeaderText" style="font-size: 1.3em;"></span>
            </div>
            <div style="text-align: center; padding-top: 3px;">
                <span id="lblRiskFactorName" runat="server" class="HeaderText" style="font-size: 1.2em;"></span>
            </div>

            <div style="height: 15px;"></div>
            
            <div id="divHazardsTable" runat="server"></div>                        
            
            <div style="height: 10px;"></div>
            
            <asp:Label runat="server" ID="lblStatus"></asp:Label> 
            
            <div style="height: 10px;"></div>
                        
            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
        </div>                 
</center>    

<div id="HazardLightBox" class="HazardLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfHazardID" runat="server" />
    <center>
        <table width="90%" style="text-align:center;">
            <colgroup style="width:16%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Потенциална опасност</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblHazardName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtHazardName" type="text" class="RequiredInputField" style="width: 480px" maxlength="500"/>                
                </td>
            </tr>            
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblSeq" class="InputLabel">Подредба:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtSeq" type="text" class="RequiredInputField" style="width: 50px"/>                
                </td>        
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
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveHazard();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideHazardLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
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

<asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshHazards_Click" CssClass="HiddenButton" />    

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 
<script type="text/javascript">

    // Display light box with hazard properties (for editing or adding new)
    function ShowHazardLightBox(hazardID, hazardName, seq)
    {
        document.getElementById("<%= hfHazardID.ClientID %>").value = hazardID; // setting hazard ID(0 - if new hazard)
      
        // initialize value
        document.getElementById("txtHazardName").value = hazardName;
        document.getElementById("txtSeq").value = seq;

        // clean message label in the light box and hide it            
        document.getElementById("spanLightBoxMessage").style.display = "none";
        document.getElementById("spanLightBoxMessage").innerHTML = "";
      
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("HazardLightBox").style.display = "";
        CenterLightBox("HazardLightBox");
    }

    // Close the light box and refresh risk factor types table
    function HideHazardLightBox()
   {
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("HazardLightBox").style.display = "none";
   }

   // Validate risk factor properties in the light box and generates appropriate error messages, if needed
   function ValidateHazard()
   {
      var res = true;
      
      var spanLightBoxMessage = document.getElementById("spanLightBoxMessage");
      spanLightBoxMessage.innerHTML = "";

      if (TrimString(document.getElementById("txtHazardName").value) == "")
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

   // Saves Hazard through ajax request, if light box values are valid, or displays generated error messages
   function SaveHazard()
   {
       if (ValidateHazard())
      {
          var url = "HazardsMaintenance.aspx?AjaxMethod=JSSaveHazard";

          var params = "RiskFactorTypeID=" + document.getElementById("<%= hfRiskFactorTypeID.ClientID %>").value +
                       "&RiskFactorID=" + document.getElementById("<%= hfRiskFactorID.ClientID %>").value +
                       "&HazardID=" + document.getElementById("<%= hfHazardID.ClientID %>").value +
                       "&Seq=" + TrimString(document.getElementById("txtSeq").value) +
                       "&HazardName=" + custEncodeURI(TrimString(document.getElementById("txtHazardName").value));

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
	        	if (document.getElementById("<%= hfHazardID.ClientID %>").value == "0")
	        	    action = "ADDED";
	        	else
	        	    action = "EDITED";
	        	document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;

	        	HideHazardLightBox();	        	
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

   // Delete Hazard type through ajax request
   function DeleteHazard(hazardID, hazardName)
   {
       YesNoDialog('Желаете ли да изтриете потенциалната опасност "' + hazardName + '"?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "HazardsMaintenance.aspx?AjaxMethod=JSDeleteHazard";
            var params = "";
            params += "RiskFactorTypeID=" + document.getElementById("<%= hfRiskFactorTypeID.ClientID %>").value;
            params += "&RiskFactorID=" + document.getElementById("<%= hfRiskFactorID.ClientID %>").value;
            params += "&HazardID=" + hazardID;
            
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
 
 </asp:Content>
