<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="MilitaryUnitPositions.aspx.cs" 
   Inherits="PMIS.HealthSafety.ContentPages.MilitaryUnitPositions"
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
                location.href = "MilitaryUnitPositions.aspx?MilitaryUnitId=" + militaryUnitId;
            }
            else//TODO: Re-select the old mil unit
            {
            }

            
        }
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
     
    <asp:HiddenField ID="hfMilitaryUnitId" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <div style="height: 20px"></div>
    <center style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Класификация</span>
                </td>                
            </tr>
            <tr>
                <td style="padding-bottom: 20px;">
                   <span id="lblHeaderSubTitle" runat="server" class="HeaderText" style="font-size: 1.15em;" >на дейностите за целите на оценка на риска</span>
                </td>
            </tr>
            <tr>
                <td>
                   <table style="margin: 0 auto;">
                     <tr>
                        <td>
                           <span id="lblMilitaryUnit" runat="server" class="InputLabel"></span>
                        </td>
                        <td style="text-align: left;">
                           <mus:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                               DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" 
                               OnEndOfSelection="OnEndOfSelection('musMilitaryUnit');" />
                           <asp:HiddenField runat="server" ID="hdnOldMilitaryUnitID" />
                        </td>
                     </tr>
                   </table>
                </td>
            </tr>
            <tr style="height: 20px;"></tr>
            <tr>
                <td>
                    <div id="divMilitaryUnitPositionsTable" runat="server"></div>
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
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Затвори</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>
    
    <div id="SubdivisionLightBox" class="SubdivisionLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfSubdivisionID" runat="server" />
    <center>
        <table width="90%" style="text-align:center;">
            <colgroup style="width:30%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Подразделение/обект</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblSubdivisionName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtSubdivisionName" type="text" class="RequiredInputField" style="width: 290px" maxlength="500"/>                
                </td>
            </tr>           
            <tr>
                <td colspan="2"> 
                    <span id="spanSubdivisionLightBoxMessage" class="ErrorText" style="display: none;">
                    </span> &nbsp;
               </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="margin: 0 auto;">
                       <tr>
                          <td>
                             <div id="btnSaveSubdivisionLightBox" style="display: inline;" onclick="SaveSubdivision();" class="Button"><i></i><div id="btnSaveSubdivisionLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseSubdivisionLightBox" style="display: inline;" onclick="HideSubdivisionLightBox();" class="Button"><i></i><div id="btnCloseSubdivisionLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </center>            
</div>

<div id="PositionLightBox" class="PositionLightBox" style="display: none; text-align: center;">
    <asp:HiddenField ID="hfPositionID" runat="server" />
    <asp:HiddenField ID="hfPositionSubdivisionID" runat="server" />
    <center>
        <table width="90%" style="text-align:center;">
            <colgroup style="width:35%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Длъжност</span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblPositionName" class="InputLabel">Наименование:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtPositionName" type="text" class="RequiredInputField" style="width: 220px" maxlength="500"/>                
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblActivities" class="InputLabel">Извършвани дейности:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtActivities" type="text" class="RequiredInputField" style="width: 380px" maxlength="500"/>                
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblTotalPersonsCnt" class="InputLabel">Общ брой лица:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtTotalPersonsCnt" type="text" class="RequiredInputField" style="width: 50px"/>                
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblFemaleCnt" class="InputLabel">Жени:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtFemaleCnt" type="text" class="RequiredInputField" style="width: 50px"/>                
                </td>
            </tr>
            <tr style="height: 30px">
                <td colspan="2"> 
                    <span id="spanPositionLightBoxMessage" class="ErrorText" style="display: none;">
                    </span> &nbsp;
               </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="margin: 0 auto;">
                       <tr>
                          <td>
                             <div id="btnSavePositionLightBox" style="display: inline;" onclick="SavePosition();" class="Button"><i></i><div id="btnSavePositionLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnClosePositionLightBox" style="display: inline;" onclick="HidePositionLightBox();" class="Button"><i></i><div id="btnClosePositionLightBoxText" style="width:70px;">Затвори</div><b></b></div>
                          </td>
                       </tr>
                    </table>                    
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
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
   Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

   function EndRequestHandlerPage(sender, args) 
   {
       
   }

   function PageLoad()
   {

   }

   function GoToRiskCard(positionId)
   {
       window.open('RiskCard.aspx?PositionId=' + positionId, '_self', false);
   }

   // Display light box with indicator type properties (for editing or adding new)
   function ShowSubdivisionLightBox(subdivisionID, subdivisionName)
   {
       document.getElementById("<%= hfSubdivisionID.ClientID %>").value = subdivisionID; // setting Subdivision ID(0 - if new Subdivision)

       // initialize value
       document.getElementById("txtSubdivisionName").value = subdivisionName;

       // clean message label in the light box and hide it
       document.getElementById("spanSubdivisionLightBoxMessage").style.display = "none";
       document.getElementById("spanSubdivisionLightBoxMessage").innerHTML = "";

       //shows the light box and "disable" rest of the page
       document.getElementById("HidePage").style.display = "";
       document.getElementById("SubdivisionLightBox").style.display = "";
       CenterLightBox("SubdivisionLightBox");
   }

   // Close the light box and refresh Subdivisions table
   function HideSubdivisionLightBox()
   {
       document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
       document.getElementById("HidePage").style.display = "none";
       document.getElementById("SubdivisionLightBox").style.display = "none";
   }

   // Validate Subdivision properties in the light box and generates appropriate error messages, if needed
   function ValidateSubdivision()
   {
       var res = true;

       var spanLightBoxMessage = document.getElementById("spanSubdivisionLightBoxMessage");
       spanLightBoxMessage.innerHTML = "";

       if (TrimString(document.getElementById("txtSubdivisionName").value) == "")
       {
           spanLightBoxMessage.innerHTML += GetErrorMessageMandatory("Наименование") + "<br />";
           res = false;
       }      

       return res;
   }

   // Saves indicator type through ajax request, if light box values are valid, or displays generated error messages
   function SaveSubdivision()
   {
       if (ValidateSubdivision())
       {
           var url = "MilitaryUnitPositions.aspx?AjaxMethod=JSSaveSubdivision";

           var params = "SubdivisionID=" + document.getElementById("<%= hfSubdivisionID.ClientID %>").value +
                       "&SubdivisionName=" + custEncodeURI(TrimString(document.getElementById("txtSubdivisionName").value)) +
                       "&MilitaryUnitId=" + MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");

           function response_handler(xml)
           {

               var hideDialog = true;
               var resultMsg = xmlValue(xml, "response");
               if (resultMsg != "OK")
               {
                   var lightBoxMessage = document.getElementById("spanSubdivisionLightBoxMessage");
                   lightBoxMessage.innerHTML = "";
                   lightBoxMessage.style.display = "";
                   hideDialog = false;
                   lightBoxMessage.innerHTML = resultMsg;
               }

               if (hideDialog)
               {
                   var action;
                   if (document.getElementById("<%= hfSubdivisionID.ClientID %>").value == "0")
                       action = "SUBDIVISION_ADDED";
                   else
                       action = "SUBDIVISION_EDITED";
                   document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;

                   HideSubdivisionLightBox();
               }
           }

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }
       else
       {
           document.getElementById("spanSubdivisionLightBoxMessage").style.display = "";
       }
   }

   // Delete ndicator type through ajax request
   function DeleteSubdivision(subdivisionID, subdivisionName)
   {
       YesNoDialog('Желаете ли да изтриете подразделението/обекта "' + subdivisionName + '"?', ConfirmYes, null);

       function ConfirmYes()
       {
           var url = "MilitaryUnitPositions.aspx?AjaxMethod=JSDeleteSubdivision";
           var params = "";
           params += "SubdivisionID=" + subdivisionID;
           
           function response_handler(xml)
           {
               if (xmlValue(xml, "response") == "OK")
               {
                   document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "SUBDIVISION_DELETED";
                   document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
               }
           }

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }
   }

   // Display light box with Position properties (for editing or adding new)
   function ShowPositionLightBox(subdivisionID, positionID, positionName, activites, totalPersonsCnt, femaleCnt)
   {
       document.getElementById("<%= hfPositionID.ClientID %>").value = positionID; // setting indicator type ID(0 - if new indicator type)
       document.getElementById("<%= hfPositionSubdivisionID.ClientID %>").value = subdivisionID; // setting indicator type ID(0 - if new indicator type)

       // initialize value
       document.getElementById("txtPositionName").value = positionName;
       document.getElementById("txtActivities").value = activites;
       document.getElementById("txtTotalPersonsCnt").value = totalPersonsCnt;
       document.getElementById("txtFemaleCnt").value = femaleCnt;

       // clean message label in the light box and hide it
       document.getElementById("spanPositionLightBoxMessage").style.display = "none";
       document.getElementById("spanPositionLightBoxMessage").innerHTML = "";

       //shows the light box and "disable" rest of the page
       document.getElementById("HidePage").style.display = "";
       document.getElementById("PositionLightBox").style.display = "";
       CenterLightBox("PositionLightBox");
   }

   // Close the light box and refresh table
   function HidePositionLightBox()
   {
       document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
       document.getElementById("HidePage").style.display = "none";
       document.getElementById("PositionLightBox").style.display = "none";
   }

   // Validate Position properties in the light box and generates appropriate error messages, if needed
   function ValidatePosition()
   {
       var res = true;

       var spanLightBoxMessage = document.getElementById("spanPositionLightBoxMessage");
       spanLightBoxMessage.innerHTML = "";

       if (TrimString(document.getElementById("txtPositionName").value) == "")
       {
           spanLightBoxMessage.innerHTML += GetErrorMessageMandatory("Наименование") + "<br />";
           res = false;
       }

       if (!isInt(TrimString(document.getElementById("txtTotalPersonsCnt").value)))
       {
           spanLightBoxMessage.innerHTML += GetErrorMessageNumber("Общ брой лица") + "<br />";
           res = false;
       }

       if (!isInt(TrimString(document.getElementById("txtFemaleCnt").value)))
       {
           spanLightBoxMessage.innerHTML += GetErrorMessageNumber("Жени") + "<br />";
           res = false;
       }

       return res;
   }

   // Saves indicator type through ajax request, if light box values are valid, or displays generated error messages
   function SavePosition()
   {
       if (ValidatePosition())
       {
           var url = "MilitaryUnitPositions.aspx?AjaxMethod=JSSavePosition";

           var params = "PositionID=" + document.getElementById("<%= hfPositionID.ClientID %>").value +
                        "&SubdivisionID=" + document.getElementById("<%= hfPositionSubdivisionID.ClientID %>").value +
                       "&PositionName=" + custEncodeURI(TrimString(document.getElementById("txtPositionName").value)) +
                       "&Activities=" + custEncodeURI(TrimString(document.getElementById("txtActivities").value)) +
                       "&TotalPersonsCnt=" + TrimString(document.getElementById("txtTotalPersonsCnt").value) +
                       "&FemaleCnt=" + TrimString(document.getElementById("txtFemaleCnt").value);

           function response_handler(xml)
           {

               var hideDialog = true;
               var resultMsg = xmlValue(xml, "response");
               if (resultMsg != "OK")
               {
                   var lightBoxMessage = document.getElementById("spanPositionLightBoxMessage");
                   lightBoxMessage.innerHTML = "";
                   lightBoxMessage.style.display = "";
                   hideDialog = false;
                   lightBoxMessage.innerHTML = resultMsg;
               }

               if (hideDialog)
               {
                   var action;
                   if (document.getElementById("<%= hfPositionID.ClientID %>").value == "0")
                       action = "POSITION_ADDED";
                   else
                       action = "POSITION_EDITED";
                   document.getElementById("<%=hdnRefreshReason.ClientID %>").value = action;

                   HidePositionLightBox();
               }
           }

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }
       else
       {
           document.getElementById("PositionLightBox").style.height = "270px";
           document.getElementById("spanPositionLightBoxMessage").style.display = "";
       }
   }

   // Delete Position through ajax request
   function DeletePosition(subdivisionID, positionID, positionName)
   {
       YesNoDialog('Желаете ли да изтриете длъжността "' + positionName + ' и <i>картата за оценка на риска</i> за към нея"?', ConfirmYes, null);

       function ConfirmYes()
       {
           var url = "MilitaryUnitPositions.aspx?AjaxMethod=JSDeletePosition";
           var params = "";
           params += "PositionID=" + positionID;
           params += "&SubdivisionID=" + subdivisionID;
           
           function response_handler(xml)
           {
               if (xmlValue(xml, "response") == "OK")
               {
                   document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "POSITION_DELETED";
                   document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
               }
           }

           var myAJAX = new AJAX(url, true, params, response_handler);
           myAJAX.Call();
       }
   }
 </script>
   
</asp:Content>

