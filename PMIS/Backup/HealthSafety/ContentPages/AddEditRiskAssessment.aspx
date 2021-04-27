<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditRiskAssessment.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditRiskAssessment" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.isDivMainClass
{
    font-family: Verdana;
    width: 170px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 142px;
}

</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>

<script type="text/javascript">
   
    window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
      
   function PageLoad() {   
       hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>'; 
   }   
   
   var initRecommendDialogHeight = 0;
   
   // Display light box with recommendation properties (for editing or adding new)
   function ShowRecommendationLightBox(recommendationId)
   {   
      if(recommendationId != 0) // gets current values if editing recommendation
      {
          var url = "AddEditRiskAssessment.aspx?AjaxMethod=JSGetRecommendation";    

          var params = "RecommendationID=" + recommendationId;

          function response_handler(xml)
          {		
            document.getElementById("<%= txtRecommendationText.ClientID %>").value = xmlValue(xml, "RecommendationText");
            document.getElementById("<%= txtDueDate.ClientID %>").value = xmlValue(xml, "DueDate");
            document.getElementById("<%= txtExecutionDate.ClientID %>").value = xmlValue(xml, "ExecutionDate");
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
      else // cleaning old values, if adding new recommendation
      {
        document.getElementById("<%= txtRecommendationText.ClientID %>").value = "";
        document.getElementById("<%= txtDueDate.ClientID %>").value = "";
        document.getElementById("<%= txtExecutionDate.ClientID %>").value = "";
      }
                 
      document.getElementById("<%= hfRecommendationID.ClientID %>").value = recommendationId; // setting recommendation ID(0 - if new recommendation)

      // clean message label in the light box and hide it            
      document.getElementById("spanLightBoxMessage").style.display = "none";
      document.getElementById("spanLightBoxMessage").innerHTML = "";
      
      //shows the light box and "disable" rest of the page
      document.getElementById("HidePage").style.display = "";
      document.getElementById("RecommendationLightBox").style.display = "";
      CenterLightBox("RecommendationLightBox");
      
      var lightBox = document.getElementById('RecommendationLightBox');
      initRecommendDialogHeight = lightBox.offsetHeight;
   }
   
   // Close the light box and refresh recommendations table
   function HideRecommendationLightBox()
   {
      document.getElementById("<%= txtRecommendationText.ClientID %>").value = "";
      document.getElementById("<%= txtDueDate.ClientID %>").value = "";
      document.getElementById("<%= txtExecutionDate.ClientID %>").value = "";
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("RecommendationLightBox").style.display = "none";
   }
   
   // Validate recommendation properties in the light box and generates appropriate error messages, if needed
   function ValidateRecommendation()
   {
      var res = true;
      var lightBox = document.getElementById('RecommendationLightBox');
      var lightBoxMessage = document.getElementById("spanLightBoxMessage");
      lightBoxMessage.innerHTML = "";
      
      var notValidFields = new Array();
      
      var recommendation = document.getElementById("<%= txtRecommendationText.ClientID %>");
      var dueDate = document.getElementById("<%= txtDueDate.ClientID %>");
      var executionDate = document.getElementById("<%= txtExecutionDate.ClientID %>");
      
      if (TrimString(recommendation.value) == "")
      {
        res = false;
        
        if (recommendation.disabled == true || recommendation.style.display == "none")
            notValidFields.push("Текст за препоръка");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Текст на препоръка") + "<br />";
      }
      
      if (TrimString(dueDate.value) == "")
      {
        res = false;
        
        if (dueDate.disabled == true || dueDate.style.display == "none")
            notValidFields.push("Дата за краен срок");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Дата за краен срок") + "<br />";
      }
      else if (!IsValidDate(dueDate.value))
      {
        res = false;
        lightBoxMessage.innerHTML += GetErrorMessageDate("Дата за краен срок") + "<br />";
      }
      
      if (res && TrimString(executionDate.value) != "" && !IsValidDate(executionDate.value))
      {
        res = false;
        lightBoxMessage.innerHTML += GetErrorMessageDate("Дата на изпълнение") + "<br />";
      }
      
        var notValidFieldsCount = notValidFields.length;
        if (notValidFieldsCount > 0)
        {
            lightBox.style.height = initRecommendDialogHeight + notValidFieldsCount * 20 + 5 + "px";
        }
        else
        {
            lightBox.style.height = initRecommendDialogHeight + "px";
        }
        
        var fieldsStr = '"' + notValidFields.join(", ") + '"';
        
        if(notValidFieldsCount > 0)
        {
           var noRightsMessage = GetErrorMessageNoRights(notValidFields);       
           lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
        }
      
      if (res)
        ForceNoChanges();
      
      return res;
   }
   
   // Saves recommendation through ajax request, if light box values are valid, or displays generated error messages
   function SaveRecommendation()
   {
      if(ValidateRecommendation())
      {
          var url = "AddEditRiskAssessment.aspx?AjaxMethod=JSSaveRecommendation";    

          var params = "RiskAssessmentID=" + document.getElementById("<%= hfRiskAssessmentID.ClientID %>").value +
                       "&RecommendationID=" + document.getElementById("<%= hfRecommendationID.ClientID %>").value +
                       "&RecommendationText=" + document.getElementById("<%= txtRecommendationText.ClientID %>").value +
                       "&DueDate=" + document.getElementById("<%= txtDueDate.ClientID %>").value + 
                       "&ExecutionDate=" + document.getElementById("<%= txtExecutionDate.ClientID %>").value;                           	                          

          function response_handler(xml)
          {            
            var hideDialog = true;
            var resultMsg = xmlValue(xml, "response");
	        if (resultMsg != "OK" && resultMsg != "ERROR")
	        {
                var lightBoxMessage = document.getElementById("spanLightBoxMessage");
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
	            hideDialog = false;
	            lightBoxMessage.innerHTML = resultMsg;
	        }
	        else if(resultMsg != "OK")
	            document.getElementById("<%= hfMsg.ClientID %>").value = "FailRecommendationSave";
	        else
	            document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessRecommendationSave";
	        
	        if (hideDialog)
	            HideRecommendationLightBox();	            	         
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
      else
      {
        document.getElementById("spanLightBoxMessage").style.display = "";
      }
   }
   
   // Delete recommendation through ajax request
   function DeleteRecommendation(recommendationId)
   {
         YesNoDialog('Желаете ли да изтриете препоръката?', ConfirmYes, null);
   
         function ConfirmYes()
         {
            var url = "AddEditRiskAssessment.aspx?AjaxMethod=JSDeleteRecommendation&RiskAssessmentID=" + document.getElementById("<%= hfRiskAssessmentID.ClientID %>").value;
                var params = "";
                params += "RecommendationID=" + recommendationId;
                
                function response_handler(xml)
                {			    	                
	                if(xmlValue(xml, "response") != "OK")
	                    document.getElementById("<%= hfMsg.ClientID %>").value = "FailRecommendationDelete";
	                else
	                {
	                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessRecommendationDelete";
	                    document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();	                          
	                }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
        }
   }
   
function ShowPrintRiskAssessment()
{
    var hfRisAssessmentId = document.getElementById("<%= hfRiskAssessmentID.ClientID %>").value;

    var url = "";
    var pageName = "PrintRiskAssessment"
    var param = "";
    
    url = "../PrintContentPages/" + pageName + ".aspx?RiskAssessmentID=" + hfRisAssessmentId;
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}
   
 </script>

<div id="jsMilitaryUnitSelectorDiv" runat="server">
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfRiskAssessmentID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
    <div style="height: 20px"></div>
    <center style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Добавяне на оценка на риска</span>
                </td>                
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="border: solid 1px #cdc9c9; padding: 5px;">
                        <tr style="min-height: 17px;">
                            <td align="right" style="width: 140px;">
                                <asp:Label ID="lblPrepariationDate" runat="server" Text="Дата на изготвяне:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 180px;">
                                <asp:TextBox ID="txtPreparationDate" runat="server" MaxLength="10" Width="80px" CssClass="RequiredInputField"></asp:TextBox>
                            </td>
                            <td align="right" style="width: 150px;">
                                <asp:Label ID="lblRegNumber" runat="server" Text="Регистрационен №:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 150px;">
                                <asp:TextBox ID="txtRegNumber" runat="server" Width="143px" MaxLength="200" CssClass="RequiredInputField"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="min-height: 17px;">
                            <td align="right" style="width: 140px;">
                                <asp:Label ID="lblMilitForceTypeType" runat="server" Text="Вид ВС:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 180px;">
                                <asp:DropDownList ID="ddlMilitaryForceTypes" runat="server" CssClass="InputField"></asp:DropDownList>
                            </td>
                            <td align="right" style="width: 150px;">
                                <asp:Label ID="lblMilitaryUnit" runat="server"  CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 150px;">
                                <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit"
                                    DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                            </td>
                        </tr>                      
                    </table>
                    </center>
                </td>
            </tr>
            <tr style="min-height: 17px;"><td>&nbsp;</td></tr>
            <tr style="min-height: 100px;">
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" style="min-width: 640px;">
                                <asp:Label ID="lblComments" runat="server" Text="Коментари:" CssClass="InputLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="min-width: 640px;">
                                <asp:TextBox ID="txtComments" TextMode="MultiLine" Width="640px" Height="100px" runat="server" CssClass="RequiredInputField"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr> 
            <tr style="min-height: 18px;">
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnPrintRiskAssessment" runat="server" CssClass="Button" OnClientClick="ShowPrintRiskAssessment(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td style="text-align: center;">
                    <table style="margin: 0 auto;">
                        <tr style="height: 35px;">
                            <td style="text-align: left;">
                                <asp:LinkButton ID="btnNewRecommendation" runat="server" CssClass="Button" OnClientClick="ShowRecommendationLightBox(0); return false;"><i></i><div style="width:100px; padding-left:5px;">Нова препоръка</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divRecommendations" runat="server"></div>
                            </td>                    
                        </tr>                    
                    </table>
                </td>
            </tr>            
            <tr style="min-height: 17px;">
                <td>
                    <asp:HiddenField ID="hfMsg" runat="server" />
                    <asp:Label ID="lblRecommendationMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>
    
    <div id="RecommendationLightBox" class="RecommendationLightBox" style="display: none; text-align: center;">
        <asp:HiddenField ID="hfRecommendationID" runat="server" />
        <asp:HiddenField ID="hfIsLightBoxDisable" runat="server" />
        <center>
            <table style="text-align:center;">
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Label ID="lblRecommendationTitle" runat="server" Text="Добавяне/Редактиране на препоръка <br />за отстраняване на риска" CssClass="HeaderText"></asp:Label>
                </td>
            </tr>
            <tr style="height: 17px"></tr>
            <tr style="min-height: 140px;">
                <td align="center" colspan="2" style="width: 380px;">
                    <span style="float: left; padding-left: 18px;"><asp:Label ID="lblRecommendationText" runat="server" Text="Препоръка:" CssClass="InputLabel"></asp:Label></span><br />
                    <asp:TextBox ID="txtRecommendationText" runat="server" TextMode="MultiLine" Width="380px" Height="130px" CssClass="RequiredInputField"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="right" style="width: 190px;">
                    <asp:Label ID="lblDueDate" runat="server" Text="Срок:" CssClass="RequiredInputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 190px;">
                    <asp:TextBox ID="txtDueDate" runat="server" MaxLength="10" CssClass="InputField"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="right" style="width: 190px;">
                    <asp:Label ID="lblExecutionDate" runat="server" Text="Дата на изпълнение:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 190px;">
                    <asp:TextBox ID="txtExecutionDate" MaxLength="10" runat="server" CssClass="InputField"></asp:TextBox>
                </td>
            </tr>             
            <tr style="height: 30px">
                <td colspan="2" style="width: 380px;"> 
                    <span id="spanLightBoxMessage" class="ErrorText" style="display: none;">
                    </span> &nbsp;
               </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="margin: 0 auto;">
                       <tr>
                          <td>
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveRecommendation();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideRecommendationLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
       </table>
        </center>
    </div>
   
    <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshRecommendations_Click" CssClass="HiddenButton" />
    
    <asp:HiddenField ID="hdnLocationHash" runat="server" />

 </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
