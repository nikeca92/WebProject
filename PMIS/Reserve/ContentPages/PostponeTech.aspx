<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PostponeTech.aspx.cs" Inherits="PMIS.Reserve.ContentPages.PostponeTechPage" %>

<%@ Register Assembly="ItemSelector" TagPrefix="is" Namespace="ItemSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
.PostponeTechCompanyLightBox
{
    width: 880px;
    background-color: #EEEEEE;
    border: solid 1px #000000;
    position: fixed;
    top: 120px;
    left: 25%;
    z-index: 1000;
    padding-top: 10px;
}

.ItemsTableHeaderBoldCell
{
    border: solid 1px #888888;
    color: #0B4489; 
    font-weight: bold; 
    text-align: center;
    color: #0B4489;
}

.ItemsTableHeaderCell
{
    border: solid 1px #888888;
    color: #0B4489; 
    font-weight: normal;
    text-align: center;
    color: #0B4489;
}

.ItemsTableDataCell
{
    border: solid 1px #888888;
    text-align: center;
}

.ItemsTableDataCell input
{
    width: 35px;
    text-align: center;
}

.CopyPostponeTechLightBox
{
    width: 650px;
    background-color: #EEEEEE;
    border: solid 1px #000000;
    position: fixed;
    top: 120px;
    left: 25%;
    min-height: 150px;
    z-index: 1000;
    padding-top: 10px;
}

</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CompanySelector.js'></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <center style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: center; padding-top: 20px; padding-bottom: 10px;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Безусловно и условно отсрочване на техника-запас</span>
                </td>                
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="border: solid 1px #cdc9c9; width: 900px;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblPostponeYear" runat="server" CssClass="InputLabel" Text="За година:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddPostponeYear" runat="server" CssClass="RequiredInputField" Width="80px" OnSelectedIndexChanged="ddPostponeYear_Change" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblMilitaryDepartment" runat="server" CssClass="InputLabel" Text="Военно окръжие:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddMilitaryDepartment" runat="server" CssClass="RequiredInputField" Width="240px" OnSelectedIndexChanged="ddMilitaryDepartment_Change" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td style="width: 300px; padding-top: 6px;">
                                     <asp:LinkButton ID="btnNewPostponeTechCompany" runat="server" CssClass="Button" OnClientClick="ShowPostponeTechCompanyLightBox(0, 0); return false;"><i></i><div style="width:120px; padding-left:5px;">Нова позиция</div><b></b></asp:LinkButton>
                                     <asp:LinkButton ID="btnCopy" runat="server" CssClass="Button" OnClientClick="ShowCopyPostponeTechLightBox(); return false;"><i></i><div style="width:80px; padding-left:5px;">Копиране</div><b></b></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <div style="text-align: center;">
                        <div style="width: 900px; margin: 0 auto;">
                           <div runat="server" id="divPostponeTechCompanies" style="text-align: center;"></div>
                           <asp:HiddenField runat="server" ID="hdnPostponeTechCompaniesCnt" Value="0" />
                        </div>
                    </div>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:HiddenField ID="hfMsg" runat="server" />
                    <asp:Label ID="lblPostponeTechCompanyMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
        </table>
    </center>
    
    <div id="PostponeTechCompanyLightBox" class="PostponeTechCompanyLightBox" style="display: none; text-align: center;">
        <asp:HiddenField ID="hfPostponeTechCompanyID" runat="server" />
        <center>
            <table style="width: 90%;">
                <tr>
                    <td colspan="4" style="text-align: left;">
                        <span id="lblWorkPlaceTitle" style="color: #0B4489; font-weight: bold; font-size: 1.2em; width: 100%; position: relative; top: -5px;">Месторабота</span>
                    </td>                                
                </tr>
                <tr>
                    <td style="text-align: left; width: 25%;">
                        <span id="lblCompanyName" class="InputLabel">Име на фирмата:</span>
                    </td>
                    <td style="text-align: left; width: 15%;">
                        <span id="lblBulstat" style="padding-right: 5px;" class="InputLabel"><%= PMIS.Common.CommonFunctions.GetLabelText("UnifiedIdentityCode") %> :</span>
                    </td>
                    <td style="text-align: left; width: 25%;">
                        <span id="lblOwnershipType" class="InputLabel">Вид собственост:</span>
                    </td>
                    <td style="text-align: left;  width: 35%;">
                        <span id="lblAdministration" class="InputLabel">Министерство/ведомство:</span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; vertical-align: top;">
                        <div id="lblCompanyNameValue" class="ReadOnlyValue" style="background-color: #FFFFCC;">&nbsp;</div>
                        <input id="btnSelectCompany"
                               type="button"
                               value="Търсене на фирма"
                               class="OpenCompanySelectorButton" 
                               onclick='companySelector.showDialog("companySelectorForWorkplace", CompanySelector_OnSelectedCompany);' />
                        <input type="hidden" id="hdnCompanyID" />
                        <input type="hidden" id="hdnCompanyIDOld" />
                    </td>
                    <td style="text-align: left; vertical-align: top;">
                        <span id="lblUnifiedIdentityCodeValue" class="ReadOnlyValue"></span>
                    </td>
                    <td style="text-align: left; vertical-align: top;">
                        <span id="lblOwnershipTypeValue" class="ReadOnlyValue"></span>
                    </td>
                    <td style="text-align: left; vertical-align: top;">
                        <span id="lblAdministrationValue" class="ReadOnlyValue"></span>
                    </td>
                </tr>
            </table>
            <div style="min-height: 15px;"></div>
            <table style="width: 90%; border-collapse: collapse;">
                <tr>
                    <td style="text-align: left; vertical-align: middle;">
                        <table style="border-collapse: collapse;">
                           <colgroup>
                              <col style="width: 50px;">
                              <col style="width: 500px;">
                              <col style="width: 105px;">
                              <col style="width: 105px;">
                           </colgroup>
                           <tr>
                              <td class="ItemsTableHeaderBoldCell">
                                 <span>№ по ред</span>
                              </td>
                              <td class="ItemsTableHeaderBoldCell">
                                 <span>Тип на техниката</span>
                              </td>
                              <td class="ItemsTableHeaderBoldCell">
                                 <span>Предл. за безусловно отсрочване</span>
                              </td>
                              <td class="ItemsTableHeaderBoldCell">
                                 <span>Предл. за условно отсрочване</span>
                              </td>
                           </tr>
                           <tr>
                              <td class="ItemsTableHeaderCell">
                                 <span>1</span>
                              </td>
                              <td class="ItemsTableHeaderCell">
                                 <span>2</span>
                              </td>
                              <td class="ItemsTableHeaderCell">
                                 <span>3</span>
                              </td>
                              <td class="ItemsTableHeaderCell">
                                 <span>4</span>
                              </td>
                           </tr> 
                        </table>
                        <div id="pnlPostponeTechItems" style="max-height: 480px; width: 780px; overflow-y: auto;"></div>
                        <table style="border-collapse: collapse;">
                           <colgroup>
                              <col style="width: 50px;">
                              <col style="width: 500px;">
                              <col style="width: 105px;">
                              <col style="width: 105px;">
                           </colgroup>
                           <tr>
                              <td class="ItemsTableDataCell"></td>
                              <td class="ItemsTableDataCell" style="text-align: left; font-weight: bold; font-style: italic;">Всичко</td>
                              <td class="ItemsTableDataCell" style="font-weight: bold;"><span id="lblTotalPostponeAbsolutely"></span></td>
                              <td class="ItemsTableDataCell" style="font-weight: bold;"><span id="lblTotalPostponeConditioned"></span></td>
                           </tr>
                        </table>
                        <input type="hidden" id="hdnItemsCnt" value="" />
                        <input type="hidden" id="hdnTechnicsTypesCnt" value="" />
                    </td>                                
                </tr>
                <tr>
                    <td style="padding-top: 6px;">
                        <div style="height: 40px; overflow-y: auto;">
                            <span id="spanLightBoxMessage" class="ErrorText"></span>
                        </div>
                   </td>
               </tr>
                <tr>
                    <td style="text-align: center;">
                        <table style="margin: 0 auto;">
                           <tr>
                              <td>
                                 <div id="btnSaveLightBox" style="display: inline;" onclick="SavePostponeTechCompany();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                                 <div id="btnCloseLightBox" style="display: inline;" onclick="HidePostponeTechCompanyLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                              </td>
                           </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
        </center>
    </div>
    
    <div id="CopyPostponeTechLightBox" class="CopyPostponeTechLightBox" style="display: none; text-align: center;">
        <center>
            <table style="width: 90%;">
                <tr style="height: 30px">
                    <td colspan="4" style="text-align: center;">
                        <span id="lblCopyPostponeTechLightBoxTitle" style="color: #0B4489; font-weight: bold; font-size: 1.2em; width: 100%; position: relative; top: -5px;">Копиране на отсрочване</span>
                    </td>                                
                </tr>
                <tr>
                    <td style="text-align: right;"><asp:Label ID="lblPostponeYearLightBox" runat="server" CssClass="InputLabel" Text="За година:"></asp:Label></td>
                    <td style="text-align: left;"><asp:DropDownList ID="ddPostponeYearLightBox" runat="server" CssClass="RequiredInputField" Width="120px"></asp:DropDownList></td>
                    <td style="text-align: right;"><asp:Label ID="lblMilitaryDepartmentLightBox" runat="server" CssClass="InputLabel" Text="Военно окръжие:"></asp:Label></td>
                    <td style="text-align: left;"><asp:DropDownList ID="ddMilitaryDepartmentLightBox" runat="server" CssClass="RequiredInputField" Width="240px"></asp:DropDownList></td>
                </tr>
                <tr style="height: 30px">
                    <td colspan="4"> 
                        <span id="spanCopyLightBoxMessage" class="ErrorText" style="display: none;">
                        </span> &nbsp;
                   </td>
               </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <table style="margin: 0 auto;">
                           <tr>
                              <td>
                                 <div id="btnSaveCopyLightBox" style="display: inline;" onclick="CopyPostponeTech();" class="Button"><i></i><div id="Div2" style="width:70px;">Запис</div><b></b></div>
                                 <div id="btnCloseCopyLightBox" style="display: inline;" onclick="HideCopyPostponeTechLightBox();" class="Button"><i></i><div id="Div4" style="width:70px;">Затвори</div><b></b></div>                                  
                              </td>
                           </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
        </center>
    </div>
    
    <asp:HiddenField ID="hdnSortBy" runat="server" />
   
    <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshPostponeTechCompanies_Click" CssClass="HiddenButton" />        
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
    <asp:HiddenField ID="hdnLocationHash" runat="server" />
    
    <input type="hidden" id="CanLeave" value="true" />
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
      
   function PageLoad() {   
       hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
   }

   function SortTableBy(sort)
   {
       if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
       {
           sort = sort + 100;
       }

       document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
       document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();
   }
   
   var initRecommendDialogHeight = 0;
   
   // Display light box with items properties (for editing or adding new)
   function ShowPostponeTechCompanyLightBox(postponeTechCompanyID, companyId)
   {
       var url = "PostponeTech.aspx?AjaxMethod=JSGetPostponeTechCompany";
       var params = "PostponeTechCompanyID=" + postponeTechCompanyID;
       params += "&CompanyID=" + companyId;

       function response_handler(xml) {
          var compid = xmlValue(xml, "CompanyID");
          document.getElementById("hdnCompanyID").value = compid;
          document.getElementById("hdnCompanyIDOld").value = compid;
          if (xmlValue(xml, "CompanyName"))
              document.getElementById("lblCompanyNameValue").innerHTML = xmlValue(xml, "CompanyName");
          else
              document.getElementById("lblCompanyNameValue").innerHTML = "&nbsp;";
          document.getElementById("lblUnifiedIdentityCodeValue").innerHTML = xmlValue(xml, "UnifiedIdentityCode");
          document.getElementById("lblOwnershipTypeValue").innerHTML = xmlValue(xml, "OwnershipType");
          document.getElementById("lblAdministrationValue").innerHTML = xmlValue(xml, "Administration");

          var postponeTechItems = xml.getElementsByTagName("PostponeTechItems")[0].getElementsByTagName("PostponeTechItem");

          var tblItemsHTML = '<table style="border-collapse: collapse;">' + 
                             '<colgroup>' +
                             '   <col style="width: 50px;">' +
                             '   <col style="width: 500px;">' +
                             '   <col style="width: 105px;">' +
                             '   <col style="width: 105px;">' +
                             '</colgroup>' +
                             '';

          var prevTechnicsTypeID = 0;
          var techTypeIdx = 0;
          var techSubTypeIdx = 0;

          for (var i = 0; i < postponeTechItems.length; i++) {
              var idx = (i + 1);
              techSubTypeIdx++;

              var PostponeTechItemID = xmlValue(postponeTechItems[i], "PostponeTechItemID");
              var TechnicsTypeName = xmlValue(postponeTechItems[i], "TechnicsTypeName");
              var TechnicsTypeID = xmlValue(postponeTechItems[i], "TechnicsTypeID");
              var TechnicsSubTypeName = xmlValue(postponeTechItems[i], "TechnicsSubTypeName");
              var TechnicsSubTypeID = xmlValue(postponeTechItems[i], "TechnicsSubTypeID");
              var PostponeConditioned = xmlValue(postponeTechItems[i], "PostponeConditioned");
              var PostponeAbsolutely = xmlValue(postponeTechItems[i], "PostponeAbsolutely");

              if (TechnicsTypeID != prevTechnicsTypeID) {
                  techTypeIdx++;
              
                  tblItemsHTML += '<tr>' +
                                  '   <td class="ItemsTableDataCell" style="font-weight: bold;">' + Romanize(techTypeIdx) + '</td>' +
                                  '   <td class="ItemsTableDataCell" style="text-align: left; font-weight: bold;">' +
                                  '      <span id="lblTechnicsTypeName' + techTypeIdx + '">' + TechnicsTypeName + '</span>' +
                                  '   </td>' +
                                  '   <td class="ItemsTableDataCell" style="font-weight: bold;">' +
                                  '      <span id="lblTechTypePostponeAbsolutely' + techTypeIdx + '"></span>' +
                                  '   </td>' +
                                  '   <td class="ItemsTableDataCell" style="font-weight: bold;">' +
                                  '      <span id="lblTechTypePostponeConditioned' + techTypeIdx + '"></span>' +
                                  '   </td>' +
                                  '</tr>';

                  prevTechnicsTypeID = TechnicsTypeID;
                  techSubTypeIdx = 1;
              }

              tblItemsHTML += '<tr>' +
                              '   <td class="ItemsTableDataCell">' + techSubTypeIdx + '</td>' +
                              '   <td class="ItemsTableDataCell" style="text-align: left;">' +
                              '      <span id="lblTechnicsSubTypeName' + idx + '">' + TechnicsSubTypeName + '</span>' +
                              '      <input type="hidden" id="hdnPostponeTechItemID' + idx + '" value="' + PostponeTechItemID + '" />' +
                              '      <input type="hidden" id="hdnTechnicsSubTypeID' + idx + '" value="' + TechnicsSubTypeID + '" />' +
                              '      <input type="hidden" id="hdnTechnicsTypeIndex' + idx + '" value="' + techTypeIdx + '" />' +
                              '   </td>' +
                              '   <td class="ItemsTableDataCell">' +
                              '      <input id="txtPostponeAbsolutely' + idx + '" type="text" class="InputField" value="' + PostponeAbsolutely + '" onblur="RecalcPostponeTotalsLightbox();" />' + 
                              '   </td>' +
                              '   <td class="ItemsTableDataCell">' +
                              '      <input id="txtPostponeConditioned' + idx + '" type="text" class="InputField" value="' + PostponeConditioned + '" onblur="RecalcPostponeTotalsLightbox();" />' + 
                              '   </td>' +
                              '</tr>';
          };

          document.getElementById("hdnItemsCnt").value = postponeTechItems.length;
          document.getElementById("hdnTechnicsTypesCnt").value = techTypeIdx;

          document.getElementById("pnlPostponeTechItems").innerHTML = tblItemsHTML;

          RecalcPostponeTotalsLightbox();

          ShowTheLightBox();
      }

      var myAJAX = new AJAX(url, true, params, response_handler);
      myAJAX.Call();

      document.getElementById("<%= hfPostponeTechCompanyID.ClientID %>").value = postponeTechCompanyID; // setting ID (0 - if new)
   }

   function ShowTheLightBox() {
       // clean message label in the light box
       document.getElementById("spanLightBoxMessage").innerHTML = "";

       //shows the light box and "disable" rest of the page
       document.getElementById("HidePage").style.display = "";
       document.getElementById("PostponeTechCompanyLightBox").style.display = "";
       CenterLightBox("PostponeTechCompanyLightBox");

       initRecommendDialogHeight = document.getElementById("PostponeTechCompanyLightBox").offsetHeight;
   }
   
   // Close the light box and refresh table
   function HidePostponeTechCompanyLightBox()
   {
      document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("PostponeTechCompanyLightBox").style.display = "none";
  }
   
   // Validate properties in the light box and generates appropriate error messages, if needed
  function ValidatePostponeTechCompany() {
      var res = true;
      var lightBox = document.getElementById('PostponeTechCompanyLightBox');
      var lightBoxMessage = document.getElementById("spanLightBoxMessage");

      var notValidFieldsCount = 0;
      lightBoxMessage.innerHTML = "";

      var companyId = document.getElementById("hdnCompanyID").value;

      if (companyId == "" || parseInt(companyId) <= 0) {
          res = false;
          lightBoxMessage.innerHTML += GetErrorMessageMandatory("Име на фирмата") + "</br>";
          notValidFieldsCount++;
      }

      var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);
      for (var i = 1; i <= itemsCnt; i++) {
          var TechnicsSubTypeName = document.getElementById("lblTechnicsSubTypeName" + i).innerHTML;
          var TechnicsTypeIdx = document.getElementById("hdnTechnicsTypeIndex" + i).value;
          var TechnicsTypeName = document.getElementById("lblTechnicsTypeName" + TechnicsTypeIdx).innerHTML;

          if (document.getElementById("txtPostponeAbsolutely" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtPostponeAbsolutely" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(TechnicsTypeName + ' - ' + TechnicsSubTypeName + " - безусл. отсрочване") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtPostponeConditioned" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtPostponeConditioned" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(TechnicsTypeName + ' - ' + TechnicsSubTypeName + " - условно отсрочване") + "</br>";
                  notValidFieldsCount++;
              }
          }
      }

      /*
      if (notValidFieldsCount > 0) {
          lightBox.style.height = initRecommendDialogHeight + notValidFieldsCount * 10 + 5 + "px";
      }
      else {
          lightBox.style.height = initRecommendDialogHeight + "px";
      }
      */

      return res;
   }
   
   // Saves item through ajax request, if light box values are valid, or displays generated error messages
   function SavePostponeTechCompany() {
       if (ValidatePostponeTechCompany()) {
          var compid = document.getElementById("hdnCompanyID").value;

          var url = "PostponeTech.aspx?AjaxMethod=JSSavePostponeTechCompany";
          var params = "PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value +
                       "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value +
                       "&PostponeTechCompanyID=" + document.getElementById("<%= hfPostponeTechCompanyID.ClientID %>").value +
                       "&CompanyID=" + custEncodeURI(TrimString(compid));

          var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);
          params += "&ItemsCnt=" + itemsCnt;

          for (var i = 1; i <= itemsCnt; i++) {
              params += "&PostponeTechItemID" + i + "=" + document.getElementById("hdnPostponeTechItemID" + i).value +
                        "&TechnicsSubTypeID" + i + "=" + document.getElementById("hdnTechnicsSubTypeID" + i).value +
                        "&PostponeConditioned" + i + "=" + document.getElementById("txtPostponeConditioned" + i).value +
                        "&PostponeAbsolutely" + i + "=" + document.getElementById("txtPostponeAbsolutely" + i).value;
          }
          
          function response_handler(xml) {
              var hideDialog = true;
              var resultMsg = xmlValue(xml, "response");
              if (resultMsg != "OK" && resultMsg != "ERROR") {
                  var lightBoxMessage = document.getElementById("spanLightBoxMessage");
                  lightBoxMessage.innerHTML = "";
                  hideDialog = false;
                  lightBoxMessage.innerHTML = resultMsg;
              }
              else if (resultMsg != "OK")
                  document.getElementById("<%= hfMsg.ClientID %>").value = "FailPostponeTechCompanySave";
              else
                  document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessPostponeTechCompanySave";

              if (hideDialog)
                  HidePostponeTechCompanyLightBox();
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
   }
   
   // Delete item through ajax request
   function DeletePostponeTechCompany(postponeTechCompanyID)
   {
       YesNoDialog('Желаете ли да изтриете позицията?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "PostponeTech.aspx?AjaxMethod=JSDeletePostponeTechCompany";
            var params = "PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value +
                         "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value +
                         "&PostponeTechCompanyID=" + postponeTechCompanyID;
                         
            function response_handler(xml)
            {			    	                
                if(xmlValue(xml, "response") != "OK")
                    document.getElementById("<%= hfMsg.ClientID %>").value = "FailPostponeTechCompanyDelete";
                else
                {
                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessPostponeTechCompanyDelete";
                    document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();	                          
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
    }

    function CompanySelector_OnSelectedCompany(companyId) {
        var companyIdOld = document.getElementById("hdnCompanyIDOld").value;

        if (companyId != companyIdOld) {
            document.getElementById("hdnCompanyID").value = companyId;
            document.getElementById("hdnCompanyIDOld").value = companyId;
            
            var url = "PostponeTech.aspx?AjaxMethod=JSCheckForDataPerCompany";
            var params = "";
            params += "compid=" + companyId;
            params += "&PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value;
            params += "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value;
            var myAJAX = new AJAX(url, true, params, isCompanySelected_Callback);
            myAJAX.Call();
        }
        
        function isCompanySelected_Callback(xml) {
            var response = xml.getElementsByTagName("response")[0];
            var postponeTechCompanyID = xmlValue(response, "postponeTechCompanyID");

            if (postponeTechCompanyID != "0" && postponeTechCompanyID != document.getElementById("<%= hfPostponeTechCompanyID.ClientID %>").value) {
                ShowPostponeTechCompanyLightBox(postponeTechCompanyID, companyId);
            }
            else {
                ShowPostponeTechCompanyLightBox(0, companyId);
            }
        }
    }

    function RecalcPostponeTotalsLightbox() {
        var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);

        var totalPostponeConditioned = 0;
        var totalPostponeAbsolutely = 0;

        var technicsTypePostponeConditioned = [];
        var technicsTypePostponeAbsolutely = [];

        for (var i = 1; i <= itemsCnt; i++) {
            var strPostponeConditioned = document.getElementById("txtPostponeConditioned" + i).value;
            var strPostponeAbsolutely = document.getElementById("txtPostponeAbsolutely" + i).value;
            var techTypeIdx = document.getElementById("hdnTechnicsTypeIndex" + i).value;

            var postponeConditioned = (isNaN(parseInt(strPostponeConditioned)) ? 0 : parseInt(strPostponeConditioned));
            var postponeAbsolutely = (isNaN(parseInt(strPostponeAbsolutely)) ? 0 : parseInt(strPostponeAbsolutely));

            totalPostponeConditioned += postponeConditioned;
            totalPostponeAbsolutely += postponeAbsolutely;

            if (technicsTypePostponeConditioned[techTypeIdx] === undefined) {
                technicsTypePostponeConditioned[techTypeIdx] = 0;
            }

            if (technicsTypePostponeAbsolutely[techTypeIdx] === undefined) {
                technicsTypePostponeAbsolutely[techTypeIdx] = 0;
            }

            technicsTypePostponeConditioned[techTypeIdx] += postponeConditioned;
            technicsTypePostponeAbsolutely[techTypeIdx] += postponeAbsolutely;
        }

        document.getElementById("lblTotalPostponeConditioned").innerHTML = totalPostponeConditioned;
        document.getElementById("lblTotalPostponeAbsolutely").innerHTML = totalPostponeAbsolutely;

        for (techTypeIdx in technicsTypePostponeConditioned)
            document.getElementById("lblTechTypePostponeConditioned" + techTypeIdx).innerHTML = technicsTypePostponeConditioned[techTypeIdx];

        for (techTypeIdx in technicsTypePostponeAbsolutely)
            document.getElementById("lblTechTypePostponeAbsolutely" + techTypeIdx).innerHTML = technicsTypePostponeAbsolutely[techTypeIdx];
    }

    function ShowCopyPostponeTechLightBox() {
        //clean old values
        document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").selectedIndex = 0;
        document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").selectedIndex = 0;

        // clean message label in the light box and hide it            
        document.getElementById("spanCopyLightBoxMessage").style.display = "none";
        document.getElementById("spanCopyLightBoxMessage").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("CopyPostponeTechLightBox").style.display = "";
        CenterLightBox("CopyPostponeTechLightBox");

        initRecommendDialogHeight = document.getElementById("CopyPostponeTechLightBox").offsetHeight;
    }

    function HideCopyPostponeTechLightBox() {
        document.getElementById("HidePage").style.display = "none";
        document.getElementById("CopyPostponeTechLightBox").style.display = "none";
    }

    function CopyPostponeTech() {
        if (ValidateCopyPostponeTech()) {
            var url = "PostponeTech.aspx?AjaxMethod=JSIsThereAnyCompanyData";
            var params = "";
            params += "MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").value;
            params += "&PostponeYear=" + document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").value;
            var myAJAX = new AJAX(url, true, params, JSIsThereAnyCompanyData_Callback);
            myAJAX.Call();
        }

        function JSIsThereAnyCompanyData_Callback(xml) {
            if (xmlValue(xml, "response") != "OK") {
                alert("There was a server problem!");
            }
            else {
                if (xmlValue(xml, "result") != "OK") {
                    YesNoDialog('В системата вече има данни за избраната година и ВО. Тези данни ще бъдат презаписани. Желаете ли да продължите?', DoCopy, null);
                }
                else {
                    DoCopy();
                }
            }
        }

        function DoCopy() {
            var url = "PostponeTech.aspx?AjaxMethod=JSCopyPostponeTech";
            var params = "";
            params += "MilitaryDepartmentID_Old=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value;
            params += "&PostponeYear_Old=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value;
            params += "&MilitaryDepartmentID_New=" + document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").value;
            params += "&PostponeYear_New=" + document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").value;
            
            function response_handler(xml) {
                if (xmlValue(xml, "response") != "OK") {
                    alert("There was a server problem!");
                }
                else {
                    document.getElementById("<%= ddPostponeYear.ClientID %>").value = document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").value;
                    document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value = document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").value;
                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCopyPostponeTech";
                    document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();

                    HideCopyPostponeTechLightBox();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
    }

    function ValidateCopyPostponeTech() {
        var res = true;
        var lightBoxMessage = document.getElementById("spanCopyLightBoxMessage");

        lightBoxMessage.innerHTML = "";

        if (document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").value == optionChooseOneValue) {
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("За година") + "</br>";
            res = false;
        }

        if (document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").value == optionChooseOneValue) {
            lightBoxMessage.innerHTML += GetErrorMessageMandatory("Военно окръжие") + "</br>";
            res = false;
        }

        if (res) {
            if (document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").value == document.getElementById("<%= ddPostponeYear.ClientID %>").value &&
                document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").value == document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value) {
                lightBoxMessage.innerHTML += "Избраните година и ВО трябва да бъдат различни от началните";
                res = false;
                }
        }

        if (!res)
            lightBoxMessage.style.display = "";

        return res;
    }
 </script>
</asp:Content>

