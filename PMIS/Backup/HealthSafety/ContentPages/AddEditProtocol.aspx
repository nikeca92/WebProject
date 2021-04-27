<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditProtocol.aspx.cs" 
   Inherits="PMIS.HealthSafety.ContentPages.AddEditProtocol"
   ValidateRequest="false" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="mus" Namespace="MilitaryUnitSelector" %>
<%@ Register Assembly="ItemSelector" TagPrefix="is" Namespace="ItemSelector" %>

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
    function OnBeforeListEvent(obj_id)
    {
        if (obj_id == "isWorkingPlace")
        {
            var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
            
            document.getElementById("<%= isWorkingPlace.ClientID %>").setAttribute("Parameters", "MilitaryUnitID=" + militaryUnitId);
        }
    }

    function OnEndOfSelection(obj_id)
    {
        if (obj_id == "isWorkingPlace")
        {
            var workingPlaceId = ItemSelectorUtil.GetSelectedValue("<%= isWorkingPlace.ClientID %>");
            var workingPlace = ItemSelectorUtil.GetSelectedText("<%= isWorkingPlace.ClientID %>");
            
            var indicatorNew = document.getElementById("newWorkingPlace");

            if ((workingPlaceId == "-1" || workingPlaceId == "") &&
                workingPlace != "")
            {
                indicatorNew.style.display = "";
            }
            else
            {
                indicatorNew.style.display = "none";
            }
        }
    }

    function OnBeforeFullListEvent(obj_id)
    {
        if (obj_id == "isWorkingPlace")
        {
            var militaryUnitText = MilitaryUnitSelectorUtil.GetSelectedText("<%= musMilitaryUnit.ClientID %>");

            ItemSelectorUtil.SetProperty("<%= isWorkingPlace.ClientID %>", "DivFullListTitle", "Избор на място на измерване за <br /> " + militaryUnitText);
        }
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
     
    <asp:HiddenField ID="hfProtocolID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <div style="height: 20px"></div>
    <center style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Добавяне на протокол от извършвани измервания</span>
                </td>                
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="border: solid 1px #cdc9c9; width: 650px;">
                        <tr>
                            <td style="text-align: right; min-width: 150px;"><asp:Label ID="lblProtocolNum" runat="server" CssClass="InputLabel" Text="Протокол №:"></asp:Label>&nbsp;<asp:TextBox ID="txtProtocolNum" runat="server" CssClass="RequiredInputField" MaxLength="100"></asp:TextBox></td>
                            <td style="text-align: left; min-width: 110px;"><asp:Label ID="lblProtocolDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>&nbsp;<asp:TextBox ID="txtProtocolDate" runat="server" CssClass="RequiredInputField" Width="70px" MaxLength="10"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 56px;"><asp:Label ID="lblProtocolType" runat="server" CssClass="InputLabel" Text="от измерване:"></asp:Label>&nbsp;<asp:DropDownList ID="ddProtocolType" runat="server" CssClass="InputField" Width="480px"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="width: 650px;  margin: 0 auto;">
                                <colgroup width="150px"></colgroup>
                                <colgroup width="150px"></colgroup>
                                <colgroup width="150px"></colgroup>
                                <colgroup width="150px"></colgroup>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <mus:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                                 DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                                            <asp:HiddenField runat="server" ID="hdnMilitaryUnitID" />
                                        </td>
                                        <td style="text-align: right; vertical-align: top;" rowspan="2">
                                            <asp:Label ID="lblAddress" runat="server" CssClass="InputLabel" Text="Адрес:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" rowspan="2">
                                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblObject" runat="server" CssClass="InputLabel" Text="Обект:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtObject" runat="server" CssClass="InputField" MaxLength="250"></asp:TextBox>
                                        </td>                                      
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblRequesting" runat="server" CssClass="InputLabel" Text="Заявител:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtRequesting" runat="server" CssClass="InputField" MaxLength="250"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right; vertical-align: top;" rowspan="2">
                                            <asp:Label ID="lblUsedEquipments" runat="server" CssClass="InputLabel" Text="Използвана апаратура:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" rowspan="2">
                                            <asp:TextBox ID="txtUsedEquipments" runat="server" TextMode="MultiLine" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMeasurementDate" runat="server" CssClass="InputLabel" Text="Дата:"></asp:Label>                    
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtMeasurementDate" runat="server" CssClass="InputField" Width="70px" MaxLength="10"></asp:TextBox>
                                        </td>                                      
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNormativeDocument" runat="server" CssClass="InputLabel" Text="Нормативен документ:"></asp:Label>                    
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtNormativeDocument" runat="server" CssClass="InputField" MaxLength="250"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right; vertical-align: top;" rowspan="2">
                                            <asp:Label ID="lblPeoplePresent" runat="server" CssClass="InputLabel" Text="Присъствали на измерванията:"></asp:Label>                                            
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" rowspan="2">
                                            <asp:TextBox ID="txtPeoplePresent" runat="server" TextMode="MultiLine" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMeasurementMethod" runat="server" CssClass="InputLabel" Text="Метод на измерване:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtMeasurementMethod" runat="server" CssClass="InputField" MaxLength="250"></asp:TextBox>
                                        </td>                                      
                                    </tr>
                                </table>                                
                            </td>
                        </tr>                       
                    </table>
                    </center>
                </td>
            </tr>
            <tr style="height: 18px;">
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnPrintProtocol" runat="server" CssClass="Button" OnClientClick="ShowPrintProtocol(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <table style="margin: 0 auto;">
                        <tr style="height: 35px;">
                            <td style="text-align: left;">
                                <asp:LinkButton ID="btnNewProtocolItem" runat="server" CssClass="Button" OnClientClick="ShowProtocolItemLightBox(0); return false;"><i></i><div style="width:100px; padding-left:5px;">Ново измерване</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divProtocolItems" runat="server"></div>
                            </td>                    
                        </tr>                    
                    </table>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:HiddenField ID="hfMsg" runat="server" />
                    <asp:Label ID="lblProtocolItemMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>
    
    <div id="ProtocolItemLightBox" class="LightBox" style="display: none; text-align: center; width: 530px; height: auto;">
        <asp:HiddenField ID="hfProtocolItemID" runat="server" />
        <center>
            <table width="95%" style="text-align:center;">
            <colgroup style="width:40%"></colgroup>
            <colgroup style="width:60%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="HeaderText" style="text-align: center;">Измерване</span><br />
                    <span class="HeaderText" style="text-align: center; font-size: 1.2em;" id="lblMilitaryUnitSubHeader"></span>
                </td>
            </tr>
            <tr style="height: 15px"></tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblWorkingPlace" class="InputLabel" style="position: relative; top: -5px;">Място на измерване:</span>                    
                </td>
                <td style="text-align: left; position: relative;">
                    <is:ItemSelector ID="isWorkingPlace" runat="server" DataSourceWebPage="DataSource.aspx" 
                        DataSourceKey="WorkingPlacesPerMilUnit" ResultMaxCount="40" DivListCss="isDivListClass" 
                        PageCount="20" DivFullListCss="isDivFullListClass" DivMainCss="isDivMainClassRequired_ItemSelector" 
                        OnlyListValues="false" 
                        OnBeforeList="OnBeforeListEvent('isWorkingPlace');" 
                        OnEndOfSelection="OnEndOfSelection('isWorkingPlace');" 
                        OnBeforeFullList="OnBeforeFullListEvent('isWorkingPlace');" 
                        UnsavedCheckSkipMe="true" ></is:ItemSelector>
                     <div id="newWorkingPlace" style="display: none; width: 35px; position: absolute; top: 0px; right: 20px; font-size: 0.8em;">
                        <img src="../Images/new.png" alt="Ново място на измерване" title="Ново място на измерване" />
                     </div>
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblWorkingPeople" class="InputLabel">Брой хора:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtWorkingPeople" type="text" class="InputField" style="width: 50px;"/>
                </td>
            </tr>
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblMeasure" class="InputLabel">Измервана величина:</span>                    
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddMeasure" runat="server" CssClass="InputField" Width="240px" AutoPostBack="false" OnChange="ddMeasure_Changed();"></asp:DropDownList>
                </td>
            </tr>
             <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span id="lblMeasured" class="InputLabel">Измерена стойност:</span>                    
                </td>
                <td style="text-align: left;">
                    <input id="txtMeasured" type="text" class="InputField" style="width: 50px;"/>
                </td>
            </tr>   
            <tr style="min-height: 17px">
                <td style="text-align: right;">
                    <span class="InputLabel">Гранична стойност:</span>                    
                </td>
                <td style="text-align: left;">
                    <span id="txtThreshold" class="InputField" style="width: 50px;">&nbsp;</span>                      
                </td>
            </tr>   
            <tr style="min-height: 17px">
                <td style="text-align: right; vertical-align: top;">
                    <span id="lblOther" class="InputLabel">Допълнителна информация:</span>                    
                </td>
                <td style="text-align: left;">
                    <textarea id="txtOther" rows="3" class="InputField" style="width: 230px;"></textarea> 
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
                             <div id="btnSaveLightBox" style="display: inline;" onclick="SaveProtocolItem();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                             <div id="btnCloseLightBox" style="display: inline;" onclick="HideProtocolItemLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
       </table>
        </center>
    </div>
    
    <div id="MilitaryUnitSelectionLightBox" class="LightBox" style="display: none; text-align: center; width: 530px; height: 100px;">
        <center>
            <table width="95%" style="text-align:center;">
            <colgroup style="width:100%"></colgroup>
            <tr style="height: 15px"></tr>
            <tr>
                <td colspan="2" align="center">
                    <span class="RegularText" style="text-align: center;" runat="server" id="lblMilitaryUnitSelectionLightBox"></span>
                    <span class="RegularText" style="text-align: center;" runat="server" id="lblMilitaryUnitSelectionLightBox2"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="margin: 0 auto;">
                       <tr>
                          <td>
                             <div id="btnOK" style="display: inline;" onclick="HideMilitaryUnitSelectionLightBox();" class="Button"><i></i><div id="Div6" style="width:70px;">ОК</div><b></b></div>                                  
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
       </table>
        </center>
    </div>
   
    <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshProtocolItems_Click" CssClass="HiddenButton" />        
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
    <asp:HiddenField ID="hdnLocationHash" runat="server" />

 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
   Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

   function EndRequestHandlerPage(sender, args) 
   {
       SetClientTextAreaMaxLength("txtOther", "500");
   }
      
   function PageLoad() {   
       hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>'; 
       
       SetClientTextAreaMaxLength("txtOther", "500");
   }   
   
   // Set corresponding to choosen measure threshold value by ajax request
   function ddMeasure_Changed()
   {    
      var url = "AddEditProtocol.aspx?AjaxMethod=JSGetThreshold";    

      var params = "MeasureID=" + document.getElementById("<%= ddMeasure.ClientID %>").value;

      function response_handler(xml)
      {				                
        if(xmlValue(xml, "response") == "ERROR")
            alert("There was a server problem!");
        else
           document.getElementById("txtThreshold").innerHTML = xmlValue(xml, "response");
      }

      var myAJAX = new AJAX(url, true, params, response_handler);
      myAJAX.Call();            
   }
   
   var initRecommendDialogHeight = 0;
   
   // Display light box with protocol items properties (for editing or adding new)
   function ShowProtocolItemLightBox(protocolItemID)
   {
       var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
       var oldMilitaryUnitId = document.getElementById("<%= hdnMilitaryUnitID.ClientID %>").value;

       if (militaryUnitId == "-1" ||
          militaryUnitId == "")
       {
           ShowMilitaryUnitSelectionLightBox(1);
           return;
       }

       if (militaryUnitId != oldMilitaryUnitId)
       {
           ShowMilitaryUnitSelectionLightBox(2);
           return;
       }
   
      if(protocolItemID != 0) // gets current values if editing protocol item
      {      
          var url = "AddEditProtocol.aspx?AjaxMethod=JSGetProtocolItem";    

          var params = "ProtocolItemID=" + protocolItemID;

          function response_handler(xml)
          {
              var workingPlaceId = xmlValue(xml, "WorkingPlaceId");
              var workingPlace = xmlValue(xml, "WorkingPlace");

              ItemSelectorUtil.SetSelectedValue("<%= isWorkingPlace.ClientID %>", workingPlaceId);
              ItemSelectorUtil.SetSelectedText("<%= isWorkingPlace.ClientID %>", workingPlace);
            		
              document.getElementById("txtWorkingPeople").value = xmlValue(xml, "WorkingPeople");
              document.getElementById("<%= ddMeasure.ClientID %>").value = xmlValue(xml, "MeasureID");
              document.getElementById("txtMeasured").value = xmlValue(xml, "Measured");
              document.getElementById("txtThreshold").innerHTML = xmlValue(xml, "Threshold");
              document.getElementById("txtOther").value = xmlValue(xml, "Other");

              showLightBox();
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
      else // cleaning old values, if adding new protocol item
      {
          ItemSelectorUtil.SetSelectedValue("<%= isWorkingPlace.ClientID %>", "-1");
          ItemSelectorUtil.SetSelectedText("<%= isWorkingPlace.ClientID %>", "");
          
          document.getElementById("txtWorkingPeople").value = "";
          document.getElementById("<%= ddMeasure.ClientID %>").selectedIndex = 0;
          document.getElementById("txtMeasured").value = "";
          document.getElementById("txtThreshold").innerHTML = "";
          document.getElementById("txtOther").value = "";

          ddMeasure_Changed(); // simulating change of measure drop down in order to get initial threshold value

          showLightBox();
      }

      function showLightBox()
      {
          document.getElementById("<%= hfProtocolItemID.ClientID %>").value = protocolItemID; // setting protocol item ID(0 - if new protocol item)

          document.getElementById("lblMilitaryUnitSubHeader").innerHTML = MilitaryUnitSelectorUtil.GetSelectedText("<%= musMilitaryUnit.ClientID %>");

          // clean message label in the light box and hide it            
          document.getElementById("spanLightBoxMessage").style.display = "none";
          document.getElementById("spanLightBoxMessage").innerHTML = "";

          //shows the light box and "disable" rest of the page
          document.getElementById("HidePage").style.display = "";
          document.getElementById("ProtocolItemLightBox").style.display = "";
          CenterLightBox("ProtocolItemLightBox");

          initRecommendDialogHeight = document.getElementById("ProtocolItemLightBox").offsetHeight;
      }
   }
   
   // Close the light box and refresh protocol items table
   function HideProtocolItemLightBox()
   {
      document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("ProtocolItemLightBox").style.display = "none";
  }

  function ShowMilitaryUnitSelectionLightBox(caseNumber)
  {
      //shows the light box and "disable" rest of the page
      if (caseNumber == 1)
      {
          document.getElementById("<%= lblMilitaryUnitSelectionLightBox.ClientID %>").style.display = "";
          document.getElementById("<%= lblMilitaryUnitSelectionLightBox2.ClientID %>").style.display = "none";
      }
      else if (caseNumber == 2)
      {
          document.getElementById("<%= lblMilitaryUnitSelectionLightBox.ClientID %>").style.display = "none";
          document.getElementById("<%= lblMilitaryUnitSelectionLightBox2.ClientID %>").style.display = "";
      }
      
      document.getElementById("HidePage").style.display = "";
      document.getElementById("MilitaryUnitSelectionLightBox").style.display = "";
      CenterLightBox("MilitaryUnitSelectionLightBox");
  }

  function HideMilitaryUnitSelectionLightBox()
  {
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("MilitaryUnitSelectionLightBox").style.display = "none";
  }
   
   // Validate protocol item properties in the light box and generates appropriate error messages, if needed
   function ValidateProtocolItem()
   {
      var res = true;
      var lightBox = document.getElementById('ProtocolItemLightBox');
      var lightBoxMessage = document.getElementById("spanLightBoxMessage");
      lightBoxMessage.innerHTML = "";
      
      var notValidFields = new Array();

      var workingPlace = ItemSelectorUtil.GetSelectedText("<%= isWorkingPlace.ClientID %>");
      var workingPeople = document.getElementById("txtWorkingPeople");
      var measured = document.getElementById("txtMeasured"); 
      
      if (TrimString(workingPlace) == "")
      {
        res = false;

        if (ItemSelectorUtil.IsHidden("<%= isWorkingPlace.ClientID %>") ||
            ItemSelectorUtil.IsDisabled("<%= isWorkingPlace.ClientID %>"))
            notValidFields.push("Място на измерване");
        else
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Място на измерване") + "<br />";
      }
      
      if (TrimString(workingPeople.value) != "" && !isInt(TrimString(workingPeople.value)))
      {
        res = false;

        lightBoxMessage.innerHTML += GetErrorMessageNumber("Брой хора") + "<br />";
      }
      
      if(TrimString(measured.value) != "" && !isDecimal(TrimString(measured.value)))
      {
        res = false;

        lightBoxMessage.innerHTML += GetErrorMessageNumber("Измерена стойност") + "<br />";
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
      
      
      if(notValidFieldsCount > 0)
      {
         var noRightsMessage = GetErrorMessageNoRights(notValidFields);       
         lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
      }
      
      return res;
   }
   
   // Saves protocol item through ajax request, if light box values are valid, or displays generated error messages
   function SaveProtocolItem()
   {
      if(ValidateProtocolItem())
      {
          var url = "AddEditProtocol.aspx?AjaxMethod=JSSaveProtocolItem";    

          var params = "ProtocolID=" + document.getElementById("<%= hfProtocolID.ClientID %>").value +
                       "&ProtocolItemID=" + document.getElementById("<%= hfProtocolItemID.ClientID %>").value +
                       "&WorkingPlace=" + custEncodeURI(TrimString(ItemSelectorUtil.GetSelectedText("<%= isWorkingPlace.ClientID %>"))) +
                       "&WorkingPlaceID=" + custEncodeURI(TrimString(ItemSelectorUtil.GetSelectedValue("<%= isWorkingPlace.ClientID %>"))) +
                       "&MilitaryUnitID=" + custEncodeURI(TrimString(MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>"))) +
                       "&WorkingPeople=" + TrimString(document.getElementById("txtWorkingPeople").value) +
                       "&MeasureID=" + custEncodeURI(document.getElementById("<%= ddMeasure.ClientID %>").value) + 
                       "&Measured="  + TrimString(document.getElementById("txtMeasured").value) +
                       "&Threshold=" + document.getElementById("txtThreshold").innerHTML +
                       "&Other=" + custEncodeURI(TrimString(document.getElementById("txtOther").value));
          
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
	            document.getElementById("<%= hfMsg.ClientID %>").value = "FailProtocolItemSave";
	        else
	            document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessProtocolItemSave";
	        
	        if (hideDialog)
	            HideProtocolItemLightBox();	                          	         
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
      else
      {
        document.getElementById("spanLightBoxMessage").style.display = "";
      }
   }
   
   // Delete protocol item through ajax request
   function DeleteProtocolItem(protocoltemID)
   {
       YesNoDialog('Желаете ли да изтриете измерването?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "AddEditProtocol.aspx?AjaxMethod=JSDeleteProtocolItem&ProtocolID=" + document.getElementById("<%= hfProtocolID.ClientID %>").value;
                var params = "";
                params += "ProtocolItemID=" + protocoltemID;
                
                function response_handler(xml)
                {			    	                
	                if(xmlValue(xml, "response") != "OK")
	                    document.getElementById("<%= hfMsg.ClientID %>").value = "FailProtocolItemDelete";
	                else
	                {
	                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessProtocolItemDelete";
	                    document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();	                          
	                }
	            }

	            var myAJAX = new AJAX(url, true, params, response_handler);
	            myAJAX.Call();
        }
   }
   
function ShowPrintProtocol()
{
    var hfProtocolID = document.getElementById("<%= hfProtocolID.ClientID %>").value;

    var url = "";
    var pageName = "PrintProtocol"
    var param = "";
    
    url = "../PrintContentPages/" + pageName + ".aspx?ProtocolID=" + hfProtocolID;
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}
   
 </script>
   
</asp:Content>

