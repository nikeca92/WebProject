<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditTrainingHistory.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditTrainingHistory" 
         Title="История на обученията" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>

<script type="text/javascript">
window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

function EndRequestHandlerPage(sender, args) 
{
    SetClientTextAreaMaxLength("txtTrainingDesc", "500");
    SetClientTextAreaMaxLength("txtLegalRef", "2000");
}
      
function PageLoad() 
{
    SetClientTextAreaMaxLength("txtTrainingDesc", "500");
    SetClientTextAreaMaxLength("txtLegalRef", "2000");
}   
   
function SortTableBy(sort)
{
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
    {
        sort = sort + 100;
    }
        
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;                           
    document.getElementById("<%= btnRefresh.ClientID %>").click();               
}

// reset all controls in lightbox to be visible and enabled
function EnableAllLightBoxControls()
{
    var lblDate = document.getElementById("lblTrainingDate");
    var lblYear = document.getElementById("lblTrainingYear");
    var lblDesc = document.getElementById("lblTrainingDesc");
    var lblLegalRef = document.getElementById("lblLegalRef");
    
    var txtDate = document.getElementById("<%= txtTrainingDate.ClientID %>");
    var txtYear = document.getElementById("txtTrainingYear");
    var txtDesc = document.getElementById("txtTrainingDesc");
    var txtLegalRef = document.getElementById("txtLegalRef");
    
    lblDate.disabled = false;
    lblDate.style.display = "";
    lblYear.disabled = false;
    lblYear.style.display = "";
    lblDesc.disabled = false;
    lblDesc.style.display = "";
    lblLegalRef.disabled = false;
    lblLegalRef.style.display = "";
    
    txtDate.disabled = false;
    txtDate.style.display = "";
    txtYear.disabled = false;
    txtYear.style.display = "";
    txtDesc.disabled = false;
    txtDesc.style.display = "";
    txtLegalRef.disabled = false;
    txtLegalRef.style.display = "";
}
    
var initRecommendDialogHeight = 0;

// Display light box with training properties (for editing or adding new)
function ShowTrainingLightBox(trainingID)
{
    EnableAllLightBoxControls();
    
    if(trainingID != 0) // gets current values if editing training
    {
        // set controls to be disabled and hidden, according to user role rights
        document.getElementById('<%= ((HiddenField)Master.FindControl("hdnDisabledClientControls")).ClientID %>').value = document.getElementById("<%= hdnEditDisabledControls.ClientID %>").value;
        document.getElementById('<%= ((HiddenField)Master.FindControl("hdnHiddenClientControls")).ClientID %>').value = document.getElementById("<%= hdnEditHiddenControls.ClientID %>").value;
              
        var url = "AddEditTrainingHistory.aspx?AjaxMethod=JSGetTraining";    

        var params = "TrainingID=" + trainingID;

        function response_handler(xml)
        {		
            document.getElementById("<%= txtTrainingDate.ClientID %>").value = xmlValue(xml, "TrainingDate");
            document.getElementById("txtTrainingYear").value = xmlValue(xml, "TrainingYear");            
            document.getElementById("txtTrainingDesc").value = xmlValue(xml, "TrainingDesc");
            document.getElementById("txtLegalRef").value = xmlValue(xml, "LegalRef");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else // cleaning old values, if adding new training
    {
        // set controls to be disabled and hidden, according to user role rights
        document.getElementById('<%= ((HiddenField)Master.FindControl("hdnDisabledClientControls")).ClientID %>').value = document.getElementById("<%= hdnAddDisabledControls.ClientID %>").value;
        document.getElementById('<%= ((HiddenField)Master.FindControl("hdnHiddenClientControls")).ClientID %>').value = document.getElementById("<%= hdnAddHiddenControls.ClientID %>").value;
    
        document.getElementById("<%= txtTrainingDate.ClientID %>").value = "";
        document.getElementById("txtTrainingYear").value = "";
        document.getElementById("txtTrainingDesc").value = "";
        document.getElementById("txtLegalRef").value = "";
    }
    
    // force disabling and hiding client controls in the lightbox
    CheckDisabledClientControls();
    CheckHiddenClientControls();

    // ensure that date textbox has DatePicker when the control was first hidden, and then show on client
    if (document.getElementById("<%= txtTrainingDate.ClientID %>").style.display == "")
    {
        document.getElementById("<%= txtTrainingDate.ClientID %>").className = "RequiredInputField <%= DateCssClass %>";
        RefreshDatePickers();
    }
                 
    document.getElementById("<%= hfTrainingID.ClientID %>").value = trainingID; // setting training ID(0 - if new training)

    // clean message label in the light box and hide it            
    document.getElementById("spanLightBoxMessage").style.display = "none";
    document.getElementById("spanLightBoxMessage").innerHTML = "";
      
    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("TrainingsLightBox").style.display = "";
    CenterLightBox("TrainingsLightBox");
      
    initRecommendDialogHeight = document.getElementById("TrainingsLightBox").offsetHeight;
}
   
// Close the light box and refresh trainings table
function HideTrainingsLightBox()
{
    document.getElementById("<%=btnRefresh.ClientID %>").click();
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("TrainingsLightBox").style.display = "none";
}
   
// Validate training properties in the light box and generates appropriate error messages, if needed
function ValidateTraining()
{
    var res = true;
    var lightBox = document.getElementById('TrainingsLightBox');
    var lightBoxMessage = document.getElementById("spanLightBoxMessage");
    lightBoxMessage.innerHTML = "";
      
    var notValidFields = new Array();      
      
    var trainingDate = document.getElementById("<%= txtTrainingDate.ClientID %>");
    var trainingYear = document.getElementById("txtTrainingYear");
    var trainingDesc = document.getElementById("txtTrainingDesc"); 
    var legalRef = document.getElementById("txtLegalRef"); 
      
    if (TrimString(trainingDate.value) == "")
    {
        res = false;
        
        if (trainingDate.disabled == true || trainingDate.style.display == "none")
            notValidFields.push("Дата");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
     else if (!IsValidDate(trainingDate.value))
      {
        res = false;
        lightBoxMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
      }
    
    if (TrimString(trainingYear.value) == "")
    {
        res = false;
        
        if (trainingYear.disabled == true || trainingYear.style.display == "none")
            notValidFields.push("Година на обучение");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Година на обучение") + "<br />";
    }
    else
    if (!isInt(TrimString(trainingYear.value)))
    {
        res = false;

        lightBoxMessage.innerHTML += GetErrorMessageNumber("Година на обучение") + "<br />";
    }
      
    if (TrimString(trainingDesc.value) == "")
    {
        res = false;
        
        if (trainingDesc.disabled == true || trainingDesc.style.display == "none")
            notValidFields.push("Обучение");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Обучение") + "<br />";
    }
      
    var notValidFieldsCount = notValidFields.length;
    if (notValidFieldsCount > 0)
    {
        lightBox.style.height = initRecommendDialogHeight + notValidFieldsCount * 20 + 15 + "px";
    }
    else
    {
        lightBox.style.height = initRecommendDialogHeight + "px";
    }
      
      
    if(notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);       
        lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
    }
      
    return res;
}
   
// Saves training through ajax request, if light box values are valid, or displays generated error messages
function SaveTraining()
{
    if(ValidateTraining())
    {
        var url = "AddEditTrainingHistory.aspx?AjaxMethod=JSSaveTraining";    

        var params = "PersonID=" + document.getElementById("<%= hfPersonID.ClientID %>").value +
                     "&TrainingID=" + document.getElementById("<%= hfTrainingID.ClientID %>").value +
                     "&TrainingDate=" + TrimString(document.getElementById("<%= txtTrainingDate.ClientID %>").value) +
                     "&TrainingYear=" + TrimString(document.getElementById("txtTrainingYear").value) +
                     "&TrainingDesc=" + custEncodeURI(TrimString(document.getElementById("txtTrainingDesc").value)) +                                         
                     "&LegalRef=" + custEncodeURI(TrimString(document.getElementById("txtLegalRef").value));                           	                          

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
	            document.getElementById("<%= hfMsg.ClientID %>").value = "FailTrainingSave";
	        else
	            document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessTrainingSave";
	        
	        if (hideDialog)
	            HideTrainingsLightBox();	                          	         
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        document.getElementById("spanLightBoxMessage").style.display = "";
    }
}

// Delete training through ajax request
function DeleteTraining(trainingID)
{
    YesNoDialog('Желаете ли да изтриете обучението?', ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditTrainingHistory.aspx?AjaxMethod=JSDeleteTraining";
        var params = "";
        params += "PersonID=" + document.getElementById("<%= hfPersonID.ClientID %>").value +
                  "&TrainingID=" + trainingID;
                        
        function response_handler(xml)
        {			    	                
	        if(xmlValue(xml, "response") != "OK")
	            document.getElementById("<%= hfMsg.ClientID %>").value = "FailTrainingDelete";
	        else
	        {
	            document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessTrainingDelete";
	            document.getElementById("<%=btnRefresh.ClientID %>").click();
	        }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ShowPrintTrainingHistory()
{
    var hfPersonID = document.getElementById("<%= hfPersonID.ClientID %>").value;

    var url = "";
    var pageName = "PrintTrainingHistory"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?PersonID=" + hfPersonID;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
 <asp:HiddenField ID="hfFromAddEditCommittee" runat="server" />
 
<asp:HiddenField ID="hfPersonID" runat="server" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">История на обученията</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 600px; margin: 0 auto;">
       <fieldset style="width: 600px; padding-bottom: 0px;">
       <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 80px;">
                   <asp:Label runat="server" ID="lblIdentNumber" CssClass="InputLabel">EГН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;" colspan="2">
                   <asp:Label runat="server" ID="txtIdentNumber" CssClass="InputField"></asp:Label>
                   <asp:Label runat="server" ID="lblName" CssClass="InputLabel">Име:</asp:Label>
                   <asp:Label runat="server" ID="txtName" CssClass="InputField"></asp:Label>
                   
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   
                   <asp:Label runat="server" ID="lblRank" CssClass="InputLabel">Звание:</asp:Label>
                   <asp:Label runat="server" ID="txtRank" CssClass="InputField"></asp:Label>
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
          </table>
        </fieldset>
    </div>
</div>

<asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_Click" />

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 580px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">              
       <div style="display: inline; position: relative; top: -16px;">
          <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClientClick="ShowTrainingLightBox(0); return false;"><i></i><div style="width:90px; padding-left:5px;">Ново обучение</div><b></b></asp:LinkButton>
       
          <span style="padding: 10px">&nbsp;</span>
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
       <div runat="server" id="pnlTrainingsGrid" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:HiddenField ID="hfMsg" runat="server" />
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 10px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
   <asp:LinkButton ID="btnPrintTrainingHistory" runat="server" CssClass="Button" OnClientClick="ShowPrintTrainingHistory(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
</div>

<div id="TrainingsLightBox" class="TrainingsLightBox" style="display: none; text-align: center; height: auto;">
        <asp:HiddenField ID="hfTrainingID" runat="server" />
        <center>
            <table width="80%" style="text-align:center;">
            <colgroup style="width:30%"></colgroup>
            <colgroup style="width:70%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Обучение</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblTrainingDate" class="InputLabel">Дата:</span>
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtTrainingDate" runat="server" CssClass="RequiredInputField" Width="70px" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblTrainingYear" class="InputLabel">Година на обучение:</span>
                </td>
                <td style="text-align: left;">
                    <input id="txtTrainingYear" type="text" class="RequiredInputField" style="width: 50px;"/>
                </td>
            </tr> 
            <tr style="min-height: 17px">
                <td style="text-align: right; vertical-align: top;">
                    <span id="lblTrainingDesc" class="InputLabel">Обучение:</span>
                </td>
                <td style="text-align: left;">
                    <textarea id="txtTrainingDesc" rows="3" class="RequiredInputField" style="width: 350px;"></textarea> 
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right; vertical-align: top;">
                    <span id="lblLegalRef" class="InputLabel">Наредба:</span>                    
                </td>
                <td style="text-align: left;">
                    <textarea id="txtLegalRef" rows="3" class="InputField" style="width: 350px;"></textarea> 
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
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveTraining();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideTrainingsLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
       </table>
        </center>
    </div>
    
    <asp:HiddenField ID="hdnAddDisabledControls" runat="server" />
    <asp:HiddenField ID="hdnAddHiddenControls" runat="server" />
    <asp:HiddenField ID="hdnEditDisabledControls" runat="server" />
    <asp:HiddenField ID="hdnEditHiddenControls" runat="server" />

<div style="height: 20px;"></div>

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
