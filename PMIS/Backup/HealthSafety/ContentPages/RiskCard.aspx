<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="RiskCard.aspx.cs" 
   Inherits="PMIS.HealthSafety.ContentPages.RiskCard"
   ValidateRequest="false" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="mus" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.isDivMainClassRequired
{
    font-family: Verdana;
}

.isDivMainClassRequired input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 150px;
}

</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
    function OnEndOfSelection(obj_id)
    {
        if (obj_id == "musMilitaryUnit")
        {
            var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
            var oldMilitaryUnitId = document.getElementById("<%= hdnOldMilitaryUnitID.ClientID %>").value;

            if (militaryUnitId != "" &&
                militaryUnitId != optionChooseOneValue &&
                militaryUnitId != oldMilitaryUnitId)
            {
                document.getElementById("<%= btnSelectedMilitaryUnit.ClientID %>").click();
            }
        }
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
     
     <asp:HiddenField ID="hfSelectedPositionId" runat="server" />
     
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <div style="height: 20px"></div>
    <center style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Карта</span>
                </td>                
            </tr>
            <tr>
                <td style="padding-bottom: 20px;">
                   <span id="lblHeaderSubTitle" runat="server" class="HeaderText" style="font-size: 1.15em;" >за оценка на риска за здравето и безопасността на работещите</span>
                </td>
            </tr>
            <tr>
                <td>
                   <table style="margin: 0 auto;">
                     <tr>
                        <td style="text-align: right;">
                           <span id="lblMilitaryUnit" runat="server" class="InputLabel"></span>
                        </td>
                        <td style="text-align: left;">
                           <mus:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                               DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" 
                               OnEndOfSelection="OnEndOfSelection('musMilitaryUnit');" />
                            <asp:HiddenField runat="server" ID="hdnOldMilitaryUnitID" />
                           <asp:Button ID="btnSelectedMilitaryUnit" runat="server" OnClick="btnSelectedMilitaryUnit_Click" CssClass="HiddenButton" /> 
                        </td>
                     </tr>
                     <tr>
                        <td style="text-align: right;">
                           <asp:Label ID="lblSubdivision" runat="server" CssClass="InputLabel" Text="Подразделение/обект:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                           <asp:DropDownList ID="ddSubdivisions" runat="server" CssClass="RequiredInputField" Width="280px" OnSelectedIndexChanged="ddSubdivisions_Changed" AutoPostBack="true"></asp:DropDownList>
                        </td>
                     </tr>
                     <tr>
                        <td style="text-align: right;">
                           <asp:Label ID="lblPosition" runat="server" CssClass="InputLabel" Text="Длъжност:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                           <asp:DropDownList ID="ddPositions" runat="server" CssClass="RequiredInputField" Width="280px" OnSelectedIndexChanged="ddPositions_Changed" AutoPostBack="true"></asp:DropDownList>
                        </td>
                     </tr>
                   </table>
                </td>
            </tr>
            <tr style="height: 20px;"></tr>
            <tr>
                <td>
                    <div id="divRiskCardItemsTable" runat="server"></div>
                </td>
            </tr>
            <tr style="height: 18px;">
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnPrintRiskCard" runat="server" CssClass="Button" OnClientClick="ShowPrintRiskCard(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <div style="padding-left: 45px; display: inline"></div>
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Затвори</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>

    <div id="RiskCardItemLightBox" class="RiskCardItemLightBox" style="display: none; text-align: center;">
        <img border='0' src='../Images/close.png' onclick="javascript:HideRiskCardItemLightBox();" style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
        <asp:HiddenField ID="hfRiskFactorTypeId" runat="server" />
        <asp:HiddenField ID="hfRiskFactorId" runat="server" />
        <asp:HiddenField ID="hfHazardId" runat="server" />
        <asp:HiddenField ID="hfProbabilityId" runat="server" />
        <asp:HiddenField ID="hfExposureId" runat="server" />
        <asp:HiddenField ID="hfEffectWeightId" runat="server" />
        <center>
            <table width="100%" style="text-align:center; min-height: 230px;">
                <tr>
                    <td align="center">
                        <span id="lblRiskFactorTypeName" class="HeaderText" style="text-align: center;"></span>
                    </td>
                </tr>
                <tr id="trRiskFactorName">
                    <td align="left" style="padding: 0 15px; font-size: 0.95em; color: #0B4489; font-weight: bold; width: 700px;">
                        <span id="lblRiskFactorName"></span>
                    </td>
                </tr>
                <tr id="trHazardName">
                    <td align="left" style="padding: 0 15px; font-size: 0.9em; color: #0B4489; font-weight: bold; width: 700px;">
                        <span id="lblHazardName"></span>
                    </td>
                </tr>
                <tr id="trHazards">
                    <td>
                        <table style="margin: 0 auto;">
                            <tr>
                                <td>
                                    <div id="divRiskFactors"></div>    
                                </td>
                            </tr>
                            <tr style="height: 15px"></tr>
                            <tr>
                                <td>
                                    <div id="divHazards"></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <table style="margin: 0 auto;">
                                       <tr>
                                          <td>
                                             <div id="btnCloseRiskCardItemLightBox1" style="display: inline;" onclick="HideRiskCardItemLightBox();" class="Button"><i></i><div id="btnCloseRiskCardItemLightBox1Text" style="width:70px;">Отказ</div><b></b></div>
                                          </td>
                                       </tr>
                                    </table>                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>   
                <tr id="trCoefficients">
                    <td style="text-align: center;">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <div id="divCoefficients"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="spanRiskCardItemBoxMessage" class="ErrorText"></span>
                                </td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
        </center>            
    </div>

    <div id="OtherHazardLightBox" class="OtherHazardLightBox" style="display: none; text-align: center;">
        <img border='0' src='../Images/close.png' onclick="javascript:HideOtherHazardLightBox();" style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
        <asp:HiddenField ID="hfRiskCardItemId" runat="server" />
        <center>
            <table width="100%" style="text-align:center;">
                <tr>
                    <td align="center">
                        <span id="spanOtherHazardTitle" class="HeaderText" style="text-align: center;">Друга опасност</span>
                    </td>
                </tr>
                <tr style="height: 15px"></tr>
                <tr>
                    <td align="center">
                        <textarea id="txtEditOtherHazard" class="RequiredInputField" style="width: 375px; height: 100px;"></textarea>
                    </td>
                </tr>
                <tr style="height: 15px"></tr>
                <tr>
                    <td align="left" style="padding-left: 10px;">
                        <div id="btnSaveOtherHazardLightBox" style="display: inline;" onclick="SaveOtherHazardLightBox();" class="Button"><i></i><div id="btnSaveOtherHazardLightBoxText" style="width:70px;">Запис</div><b></b></div>
                    </td>
                </tr>
            </table>
        </center>            
    </div>

    <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshSubdivisions_Click" CssClass="HiddenButton" /> 
    
    <asp:HiddenField ID="hdnRefreshReason" runat="server" />
    
    <input type="hidden" id="CanLeave" value="true" />
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
     var hfSelectedPositionIdClientID = "<%= hfSelectedPositionId.ClientID %>";
     var hfRiskFactorTypeIdClientID = "<%= hfRiskFactorTypeId.ClientID %>";
     var hfRiskFactorIdClientID = "<%= hfRiskFactorId.ClientID %>";
     var hfHazardIdClientID = "<%= hfHazardId.ClientID %>";
     var hfProbabilityIdClientID = "<%= hfProbabilityId.ClientID %>";
     var hfExposureIdClientID = "<%= hfExposureId.ClientID %>";
     var hfEffectWeightIdClientID = "<%= hfEffectWeightId.ClientID %>";

     var hfRiskCardItemIdClientID = "<%= hfRiskCardItemId.ClientID %>";
 
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
   Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

   function EndRequestHandlerPage(sender, args) 
   {
       
   }

   function PageLoad()
   {

   }

   function ShowPrintRiskCard()
   {
       var hfPositionId = document.getElementById(hfSelectedPositionIdClientID).value;

       var url = "";
       var pageName = "PrintRiskCard"
       var param = "";

       url = "../PrintContentPages/" + pageName + ".aspx?PositionID=" + hfPositionId;

       var uplPopup = window.open(url, pageName, param);

       if (uplPopup != null)
           uplPopup.focus();
   }

   // Shows the light box and "disable" rest of the page
   function ShowRiskCardItemLightBox()
   {
       document.getElementById("HidePage").style.display = "";
       document.getElementById("RiskCardItemLightBox").style.display = "";

       CenterLightBox("RiskCardItemLightBox");

       //Strange behavior: Use this code to re-load the content of these labels. Without this trick sometimes when going on Step 2 (this light-box) these labels were overlapped (probably when loading different text-sizes and when the loaded one have a longer text)
       document.getElementById("lblHazardName").innerHTML = document.getElementById("lblHazardName").innerHTML;
       document.getElementById("lblRiskFactorTypeName").innerHTML = document.getElementById("lblRiskFactorTypeName").innerHTML;
       document.getElementById("lblRiskFactorName").innerHTML = document.getElementById("lblRiskFactorName").innerHTML;
   }

   // Display light box with indicator type properties (for editing or adding new)
    function ShowRiskCardItemLightBoxContent(riskFactorTypeId, riskFactorTypeName, isAddMode)
    {
        document.getElementById(hfRiskFactorTypeIdClientID).value = riskFactorTypeId;

        // initialize value
        document.getElementById("lblRiskFactorTypeName").innerHTML = riskFactorTypeName;

        if (isAddMode)
        {
            document.getElementById("trHazards").style.display = "";
            document.getElementById("trCoefficients").style.display = "none";
            GetRiskFactorsHtml();
        }
        else
        {
            document.getElementById("trHazards").style.display = "none";
            document.getElementById("trCoefficients").style.display = "";
        }

        // clean message label in the light box and hide it
        document.getElementById("spanRiskCardItemBoxMessage").style.display = "none";
        document.getElementById("spanRiskCardItemBoxMessage").innerHTML = "";
    }

   // Close the light box and refresh Risk Card Items table
    function HideRiskCardItemLightBox()
    {
        document.getElementById(hfRiskFactorTypeIdClientID).value = "";
        document.getElementById(hfRiskFactorIdClientID).value = "";
        document.getElementById(hfHazardIdClientID).value = "";
        document.getElementById(hfProbabilityIdClientID).value = "";
        document.getElementById(hfExposureIdClientID).value = "";
        document.getElementById(hfEffectWeightIdClientID).value = "";

       document.getElementById("HidePage").style.display = "none";
       document.getElementById("RiskCardItemLightBox").style.display = "none";

       document.getElementById("divRiskFactors").innerHTML = "";
       document.getElementById("divHazards").innerHTML = "";
       document.getElementById("divCoefficients").innerHTML = "";
       document.getElementById("lblRiskFactorName").innerHTML = "";
       document.getElementById("lblHazardName").innerHTML = "";
   }

   // Get and show all available risk factors related to contextual risk factor type
   function GetRiskFactorsHtml()
   {
       var url = "RiskCard.aspx?AjaxMethod=JSGetRiskFactorsHtml";
       var params = "";
       params += "&RiskFactorTypeId=" + document.getElementById(hfRiskFactorTypeIdClientID).value;

       function response_handler(xml)
       {
           document.getElementById("divRiskFactors").innerHTML = xmlNodeText(xml.childNodes[0]);
           ShowRiskCardItemLightBox();
       }

       var myAJAX = new AJAX(url, true, params, response_handler);
       myAJAX.Call();
   }

   function RiskFactorsDDChange(ddRiskFactors)
   {
       var selectedRiskFactorId = GetSelectedItemId(ddRiskFactors);
       document.getElementById(hfRiskFactorIdClientID).value = selectedRiskFactorId;
       if (selectedRiskFactorId != -1)
       {
           GetHazardsHtml();
       }
       else
       {
           document.getElementById("divHazards").innerHTML = "";
       }
   }

   // Get and show all available hazards related to selected risk factor
   function GetHazardsHtml()
   {
       var url = "RiskCard.aspx?AjaxMethod=JSGetHazardsHtml";
       var params = "";
       params += "&PositionId=" + document.getElementById(hfSelectedPositionIdClientID).value;
       params += "&RiskFactorId=" + document.getElementById(hfRiskFactorIdClientID).value;

       function response_handler(xml)
       {
           document.getElementById("divHazards").innerHTML = xmlNodeText(xml.childNodes[0]);
           CenterLightBox("RiskCardItemLightBox");
       }

       var myAJAX = new AJAX(url, true, params, response_handler);
       myAJAX.Call();
   }

   // Set the selected hazard id to hidden field and go to coefficient view
   function HazardRowSelected(hazardId)
   {
       document.getElementById(hfHazardIdClientID).value = hazardId;
       GoToCoefficientsView(true, 0);
   }

   var isFactorNameLoaded = false;

   // Get selected risk factor name
   function GetFactorName()
   {
       var url = "RiskCard.aspx?AjaxMethod=JSGetFactorName";
       var params = "";
       params += "&RiskFactorId=" + document.getElementById(hfRiskFactorIdClientID).value;

       function response_handler(xml)
       {
           document.getElementById("lblRiskFactorName").innerHTML = xmlNodeText(xml.childNodes[0]);
           isFactorNameLoaded = true;

           if (isFactorNameLoaded && isHazardNameLoaded)
           {
               isFactorNameLoaded = false;
               isHazardNameLoaded = false;
               SwitchToCoefficientsView();
           }
       }

       var myAJAX = new AJAX(url, true, params, response_handler);
       myAJAX.Call();
   }

   var isHazardNameLoaded = false;

   // Get selected hazard name
   function GetHazardName(hasHazardId)
   {
       var riskCardItemId = document.getElementById(hfRiskCardItemIdClientID).value;
       
       if ((riskCardItemId == "" || riskCardItemId == 0) && !hasHazardId)
       {
           document.getElementById("lblHazardName").innerHTML = ReplaceNewLinesWithBRs(document.getElementById("txtOtherHazard").value);
           
           isHazardNameLoaded = true;

           if (isFactorNameLoaded && isHazardNameLoaded)
           {
               isFactorNameLoaded = false;
               isHazardNameLoaded = false;
               SwitchToCoefficientsView();
           }
       }
       else
       {
           var url = "RiskCard.aspx?AjaxMethod=JSGetHazardName";
           var params = "";
           params += "&RiskCardItemId=" + riskCardItemId;
           params += "&HazardId=" + document.getElementById(hfHazardIdClientID).value;

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }

       function response_handler(xml)
       {
           document.getElementById("lblHazardName").innerHTML = xmlNodeText(xml.childNodes[0]);
           isHazardNameLoaded = true;

           if (isFactorNameLoaded && isHazardNameLoaded)
           {
               isFactorNameLoaded = false;
               isHazardNameLoaded = false;
               SwitchToCoefficientsView();
           }
       }
   }

   function BeforeGoToCoefficientsView()
   {
       var otherHazard = document.getElementById("txtOtherHazard").value;
       if (otherHazard != null && TrimString(otherHazard) != "")
       {
           GoToCoefficientsView(false, 0);
       }
   }

   // Get and show all available coefficients
   function GoToCoefficientsView(hasHazardId, riskCardItemId)
   {
       document.getElementById("lblRiskFactorName").style.display = "none";
       document.getElementById("lblHazardName").style.display = "none";
       document.getElementById(hfRiskCardItemIdClientID).value = riskCardItemId;
              
       if (!hasHazardId)
           document.getElementById(hfHazardIdClientID).value = "";
   
       var url = "RiskCard.aspx?AjaxMethod=JSGetCoefficientsHtml";
       var params = "";
       params += "&RiskCardItemId=" + riskCardItemId;
       
       function response_handler(xml)
       {
           var contentHTML = xmlValue(xml, "contentHTML");

           document.getElementById("divCoefficients").innerHTML = contentHTML;
           RefreshUIItems(xml);

           GetFactorName();
           GetHazardName(hasHazardId);

           var ddProbabilities = document.getElementById("ddProbabilities");
           if (ddProbabilities != null)
               document.getElementById(hfProbabilityIdClientID).value = GetSelectedItemId(ddProbabilities);

           var ddExposures = document.getElementById("ddExposures");
           if (ddExposures != null)
               document.getElementById(hfExposureIdClientID).value = GetSelectedItemId(ddExposures);

           var ddEffectWeights = document.getElementById("ddEffectWeights");
           if (ddEffectWeights != null)
               document.getElementById(hfEffectWeightIdClientID).value = GetSelectedItemId(ddEffectWeights);
       }

       var myAJAX = new AJAX(url, true, params, response_handler);
       myAJAX.Call();
   }

   function SwitchToCoefficientsView()
   {
       document.getElementById("lblRiskFactorName").style.display = "";
       document.getElementById("lblHazardName").style.display = "";
   
       document.getElementById("trHazards").style.display = "none";
       document.getElementById("trCoefficients").style.display = "";

       ShowRiskCardItemLightBox();
   }

   function ShowCoefficientsView(hazardId, riskCardItemId, riskFactorId, riskFactorTypeName)
   {
       document.getElementById(hfHazardIdClientID).value = hazardId;
       document.getElementById(hfRiskFactorIdClientID).value = riskFactorId;
       document.getElementById("lblRiskFactorTypeName").innerHTML = riskFactorTypeName;

       GoToCoefficientsView(hazardId != 0, riskCardItemId);
   }

   function ProbabilitiesDDChange(ddProbabilities)
   {
       document.getElementById("lblCoefficientsMessage").innerHTML = "";
       var selectedProbabilityId = GetSelectedItemId(ddProbabilities);
       if (selectedProbabilityId != -1)
       {
           var selectedExposureId = GetSelectedItemId(document.getElementById("ddExposures"));
           var selectedEffectWeightId = GetSelectedItemId(document.getElementById("ddEffectWeights"));

           if (selectedExposureId != -1 && selectedEffectWeightId != -1)
           {
               document.getElementById(hfProbabilityIdClientID).value = selectedProbabilityId;
               document.getElementById(hfExposureIdClientID).value = selectedExposureId;
               document.getElementById(hfEffectWeightIdClientID).value = selectedEffectWeightId;

               CalculateHazardValue(selectedProbabilityId, selectedExposureId, selectedEffectWeightId);
               document.getElementById("btnSaveRiskCardItemLightBox").className = "Button";
           }
           else
           {
               document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
           }
       }
       else
       {
           document.getElementById("lblHazardValue").innerHTML = "";
           document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
       }
   }

   function ExposuresDDChange(ddExposures)
   {
       document.getElementById("lblCoefficientsMessage").innerHTML = "";
       var selectedExposureId = GetSelectedItemId(ddExposures);
       if (selectedExposureId != -1)
       {
           var selectedProbabilityId = GetSelectedItemId(document.getElementById("ddProbabilities"));
           var selectedEffectWeightId = GetSelectedItemId(document.getElementById("ddEffectWeights"));

           if (selectedProbabilityId != -1 && selectedEffectWeightId != -1)
           {
               document.getElementById(hfProbabilityIdClientID).value = selectedProbabilityId;
               document.getElementById(hfExposureIdClientID).value = selectedExposureId;
               document.getElementById(hfEffectWeightIdClientID).value = selectedEffectWeightId;

               CalculateHazardValue(selectedProbabilityId, selectedExposureId, selectedEffectWeightId);
               document.getElementById("btnSaveRiskCardItemLightBox").className = "Button";
           }
           else
           {
               document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
           }
       }
       else
       {
           document.getElementById("lblHazardValue").innerHTML = "";
           document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
       }
   }

   function EffectWeightsDDChange(ddEffectWeights)
   {
       document.getElementById("lblCoefficientsMessage").innerHTML = "";
       var selectedEffectWeightId = GetSelectedItemId(ddEffectWeights);
       if (selectedEffectWeightId != -1)
       {
           var selectedProbabilityId = GetSelectedItemId(document.getElementById("ddProbabilities"));
           var selectedExposureId = GetSelectedItemId(document.getElementById("ddExposures"));

           if (selectedProbabilityId != -1 && selectedExposureId != -1)
           {
               document.getElementById(hfProbabilityIdClientID).value = selectedProbabilityId;
               document.getElementById(hfExposureIdClientID).value = selectedExposureId;
               document.getElementById(hfEffectWeightIdClientID).value = selectedEffectWeightId;

               CalculateHazardValue(selectedProbabilityId, selectedExposureId, selectedEffectWeightId);
               document.getElementById("btnSaveRiskCardItemLightBox").className = "Button";
           }
           else
           {
               document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
           }
       }
       else
       {
           document.getElementById("lblHazardValue").innerHTML = "";
           document.getElementById("btnSaveRiskCardItemLightBox").className = "DisabledButton";
       }
   }

   // Calculate hazard value
   function CalculateHazardValue(probabilityId, exposureId, effectWeightId)
   {
       var url = "RiskCard.aspx?AjaxMethod=JSCalculateHazardValue";
       var params = "";
       params += "&ProbabilityId=" + probabilityId;
       params += "&ExposureId=" + exposureId;
       params += "&EffectWeightId=" + effectWeightId;

       function response_handler(xml)
       {
           if (xmlNodeText(xml.childNodes[0]) != "")
           {
               document.getElementById("lblHazardValue").innerHTML = xmlNodeText(xml.childNodes[0]);
               document.getElementById("btnSaveRiskCardItemLightBox").style.display = "";
           }
           else
               document.getElementById("btnSaveRiskCardItemLightBox").style.display = "none";
       }

       var myAJAX = new AJAX(url, true, params, response_handler);
       myAJAX.Call();
   }

   // Save risk card item
   function SaveRiskCardItem(riskCardItemId)
   {
       if (ValidateSaveRiskCardItem())
       {
           var positionId = document.getElementById(hfSelectedPositionIdClientID).value;
           var riskFactorTypeId = document.getElementById(hfRiskFactorTypeIdClientID).value;
           var riskFactorId = document.getElementById(hfRiskFactorIdClientID).value;
           var hazardId = document.getElementById(hfHazardIdClientID).value;
           var probabilityId = document.getElementById(hfProbabilityIdClientID).value;
           var exposureId = document.getElementById(hfExposureIdClientID).value;
           var effectWeightId = document.getElementById(hfEffectWeightIdClientID).value;

           var otherHazard = "";

           var txtOtherHazard = document.getElementById("txtOtherHazard");
           if (txtOtherHazard != null)
               otherHazard = txtOtherHazard.value;

           var url = "RiskCard.aspx?AjaxMethod=JSSaveRiskCardItem";
           var params = "";

           params += "&RiskCardItemId=" + riskCardItemId;
           params += "&PositionId=" + positionId;
           params += "&RiskFactorTypeId=" + riskFactorTypeId;
           params += "&RiskFactorId=" + riskFactorId;
           params += "&HazardId=" + hazardId;
           params += "&ProbabilityId=" + probabilityId;
           params += "&ExposureId=" + exposureId;
           params += "&EffectWeightId=" + effectWeightId;
           params += "&OtherHazard=" + otherHazard;

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }

       function response_handler(xml)
       {
           HideRiskCardItemLightBox();
           document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
       }
   }

   //Client validation of coefficients view light-box
   function ValidateSaveRiskCardItem()
   {
       var res = true;

       var lblMessage = document.getElementById("lblCoefficientsMessage");
       lblMessage.innerHTML = "";

       var notValidFields = new Array();

       //Mandatory Filed to validate according from DB table

       var ddProbabilities = document.getElementById("ddProbabilities");
       var ddExposures = document.getElementById("ddExposures");
       var ddEffectWeights = document.getElementById("ddEffectWeights");

       if (ddProbabilities.value == optionChooseOneValue)
       {
           res = false;
           if (ddProbabilities.disabled == true || ddProbabilities.style.display == "none")
               notValidFields.push("Вероятност(B)");
           else
               lblMessage.innerHTML += GetErrorMessageMandatory("Вероятност(B)") + "<br />";
       }

       if (ddExposures.value == optionChooseOneValue)
       {
           res = false;

           if (ddExposures.disabled == true || ddExposures.style.display == "none")
               notValidFields.push("Експозиция(E)");
           else
               lblMessage.innerHTML += GetErrorMessageMandatory("Експозиция(E)") + "<br />";
       }

       if (ddEffectWeights.value == optionChooseOneValue)
       {
           res = false;

           if (ddEffectWeights.disabled == true || ddEffectWeights.style.display == "none")
               notValidFields.push("Тежест(T)");
           else
               lblMessage.innerHTML += GetErrorMessageMandatory("Тежест(T)") + "<br />";
       }

       var notValidFieldsCount = notValidFields.length;

       if (notValidFieldsCount > 0)
       {
           var noRightsMessage = GetErrorMessageNoRights(notValidFields);
           lblMessage.innerHTML += "<br />" + noRightsMessage;
       }

       if (res)
       {
           lblMessage.className = "SuccessText";
       }
       else
       {
           lblMessage.className = "ErrorText";
           lblMessage.style.display = "";
       }

       return res;
   }

   // Delete risk card item
   function DeleteRiskCardItem(riskCardItemId)
   {
       YesNoDialog('Желаете ли да изтриете опасността?', ConfirmYes, null);

       function ConfirmYes()
       {
           var url = "RiskCard.aspx?AjaxMethod=JSDeleteRiskCardItem";
           var params = "";

           params += "&RiskCardItemId=" + riskCardItemId;

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }

       function response_handler(xml)
       {
           document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
       }
   }

   // Display light box for edit other hazard
   function ShowOtherHazardLightBox(riskCardItemId, otherHazard)
   {
       document.getElementById(hfRiskCardItemIdClientID).value = riskCardItemId;
       document.getElementById("txtEditOtherHazard").value = ReplaceBRsWithNewLines(otherHazard);

       //shows the light box and "disable" rest of the page
       document.getElementById("HidePage").style.display = "";
       document.getElementById("OtherHazardLightBox").style.display = "";
       CenterLightBox("OtherHazardLightBox");
   }

   // Close the light box and refresh Risk Card Items table
   function HideOtherHazardLightBox()
   {
       document.getElementById(hfRiskCardItemIdClientID).value = "";
       document.getElementById("txtEditOtherHazard").value = "";
       
       document.getElementById("HidePage").style.display = "none";
       document.getElementById("OtherHazardLightBox").style.display = "none";
   }

   // Update other hazard
   function SaveOtherHazardLightBox()
   {
       var riskCardItemId = document.getElementById(hfRiskCardItemIdClientID).value;
       var otherHazard = document.getElementById("txtEditOtherHazard").value;

       if (otherHazard != null && TrimString(otherHazard) != "")
       {
           var url = "RiskCard.aspx?AjaxMethod=JSSaveOtherHazard";
           var params = "";

           params += "&RiskCardItemId=" + riskCardItemId;
           params += "&OtherHazard=" + otherHazard;

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();   
       }

       function response_handler(xml)
       {
           HideOtherHazardLightBox();
           document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
       }
   }
   
 </script>
   
</asp:Content>

