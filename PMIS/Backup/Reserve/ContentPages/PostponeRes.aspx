<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PostponeRes.aspx.cs" Inherits="PMIS.Reserve.ContentPages.PostponeResPage" %>

<%@ Register Assembly="ItemSelector" TagPrefix="is" Namespace="ItemSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
.PageContentArea
{
	border: solid 1px #AAAAAA;
	background-color: #FFFFFF;
	position: relative;
	top: -1px;
	left: -1px;
	min-height: 400px;
	text-align: left;
	width: 1150px;
}

.ShadowContainer
{
    margin: 0 auto;
	width: 1150px;
}

#SubShadowContainer
{
	margin: 0 auto;
	width: 1150px;
    min-width: 1150px;
}
    
.PostponeResCompanyLightBox
{
    width: 880px;
    background-color: #EEEEEE;
    border: solid 1px #000000;
    position: fixed;
    top: 120px;
    left: 25%;
    min-height: 480px;
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

.CopyPostponeResLightBox
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
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Безусловно и условно отсрочване на запасни, назначени на работа</span>
                </td>                
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="border: solid 1px #cdc9c9; width: 1100px;">
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
                                <td style="width: 500px; padding-top: 6px;">
                                     <asp:LinkButton ID="btnNewPostponeResCompany" runat="server" CssClass="Button" OnClientClick="ShowPostponeResCompanyLightBox(0, 0); return false;"><i></i><div style="width:120px; padding-left:5px;">Нова позиция</div><b></b></asp:LinkButton>
                                     <asp:LinkButton ID="btnCopy" runat="server" CssClass="Button" OnClientClick="ShowCopyPostponeResLightBox(); return false;"><i></i><div style="width:80px; padding-left:5px;">Копиране</div><b></b></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <div style="text-align: center;">
                        <div style="width: 1100px; margin: 0 auto;">
                           <div runat="server" id="divPostponeResCompanies" style="text-align: center;"></div>
                           <asp:HiddenField runat="server" ID="hdnPostponeResCompaniesCnt" Value="0" />
                        </div>
                    </div>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:HiddenField ID="hfMsg" runat="server" />
                    <asp:Label ID="lblPostponeResCompanyMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
        </table>
    </center>
    
    <div id="PostponeResCompanyLightBox" class="PostponeResCompanyLightBox" style="display: none; text-align: center;">
        <asp:HiddenField ID="hfPostponeResCompanyID" runat="server" />
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
                <tr>
                    <td colspan="4">
                        <span id="lblEmployeesCnt" class="InputLabel">Обща численост на персонала:</span>
                        <input type="text" id="txtEmployeesCnt" class="InputField" style="width: 50px;" />
                    </td>
                </tr>
            </table>
            <div style="min-height: 15px;"></div>
            <table style="width: 90%; border-collapse: collapse;">
                <tr>
                    <td style="text-align: left; vertical-align: middle;">
                        <div id="pnlPostponeResItems"></div>
                    </td>                                
                </tr>
                <tr style="height: 36px">
                    <td style="padding-top: 6px;"> 
                        <span id="spanLightBoxMessage" class="ErrorText">
                        </span>&nbsp;
                   </td>
               </tr>
                <tr>
                    <td style="text-align: center;">
                        <table style="margin: 0 auto;">
                           <tr>
                              <td>
                                 <div id="btnSaveLightBox" style="display: inline;" onclick="SavePostponeResCompany();" class="Button"><i></i><div id="btnSaveLightBoxText" style="width:70px;">Запис</div><b></b></div>
                                 <div id="btnCloseLightBox" style="display: inline;" onclick="HidePostponeResCompanyLightBox();" class="Button"><i></i><div id="btnCloseLightBoxText" style="width:70px;">Затвори</div><b></b></div>                                  
                              </td>
                           </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
        </center>
    </div>
    
    <div id="CopyPostponeResLightBox" class="CopyPostponeResLightBox" style="display: none; text-align: center;">
        <center>
            <table style="width: 90%;">
                <tr style="height: 30px">
                    <td colspan="4" style="text-align: center;">
                        <span id="lblCopyPostponeResLightBoxTitle" style="color: #0B4489; font-weight: bold; font-size: 1.2em; width: 100%; position: relative; top: -5px;">Копиране на отсрочване</span>
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
                                 <div id="btnSaveCopyLightBox" style="display: inline;" onclick="CopyPostponeRes();" class="Button"><i></i><div id="Div2" style="width:70px;">Запис</div><b></b></div>
                                 <div id="btnCloseCopyLightBox" style="display: inline;" onclick="HideCopyPostponeResLightBox();" class="Button"><i></i><div id="Div4" style="width:70px;">Затвори</div><b></b></div>                                  
                              </td>
                           </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
        </center>
    </div>
    
    <asp:HiddenField ID="hdnSortBy" runat="server" />
   
    <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshPostponeResCompanies_Click" CssClass="HiddenButton" />        
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
   function ShowPostponeResCompanyLightBox(postponeResCompanyID, companyId)
   {
      var url = "PostponeRes.aspx?AjaxMethod=JSGetPostponeResCompany";
      var params = "PostponeResCompanyID=" + postponeResCompanyID;
      params += "&CompanyID=" + companyId;

      function response_handler(xml) {
          var compid = xmlValue(xml, "CompanyID");
          document.getElementById("hdnCompanyID").value = compid;
          document.getElementById("hdnCompanyIDOld").value = compid;
          if(xmlValue(xml, "CompanyName"))
              document.getElementById("lblCompanyNameValue").innerHTML = xmlValue(xml, "CompanyName");
          else
              document.getElementById("lblCompanyNameValue").innerHTML = "&nbsp;";
          document.getElementById("lblUnifiedIdentityCodeValue").innerHTML = xmlValue(xml, "UnifiedIdentityCode");
          document.getElementById("lblOwnershipTypeValue").innerHTML = xmlValue(xml, "OwnershipType");
          document.getElementById("lblAdministrationValue").innerHTML = xmlValue(xml, "Administration");
          document.getElementById("txtEmployeesCnt").value = xmlValue(xml, "EmployeesCnt");

          var postponeResItems = xml.getElementsByTagName("PostponeResItems")[0].getElementsByTagName("PostponeResItem");

          var tblItemsHTML = '<table style="border-collapse: collapse;">' + 
                             '<colgroup>' +
                             '   <col style="width: 20px;">' +
                             '   <col style="width: 100px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '   <col style="width: 65px;">' +
                             '</colgroup>' +
                             '   <tr>' +
                             '      <td rowspan="4" class="ItemsTableHeaderBoldCell">' +
                             '         <span>№ по ред</span>' +
                             '      </td>' +
                             '      <td rowspan="4" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Клас по НКПД</span>' +
                             '      </td>' +
                             '      <td colspan="10" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Брой на предложените за безусловно и условно отсрочване запасни</span>' +
                             '      </td>' +
                             '   </tr>' +
                             '   <tr>' +
                             '      <td rowspan="2" colspan="2" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Общ брой</span>' +
                             '      </td>' +
                             '      <td colspan="8" class="ItemsTableHeaderBoldCell">' +
                             '         <span>От тях:</span>' +
                             '      </td>' +
                             '   </tr>' +
                             '   <tr>' +
                             '      <td colspan="2" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Офицери</span>' +
                             '      </td>' +
                             '      <td colspan="2" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Офицерски кандидати</span>' +
                             '      </td>' +
                             '      <td colspan="2" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Сержанти / Старшини</span>' +
                             '      </td>' +
                             '      <td colspan="2" class="ItemsTableHeaderBoldCell">' +
                             '         <span>Войници / матроси</span>' +
                             '      </td>' +
                             '   </tr>' +
                             '   <tr>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>безусл. отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>условно отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>безусл. отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>условно отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>безусл. отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>условно отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>безусл. отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>условно отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>безусл. отср.</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>условно отср.</span>' +
                             '      </td>' +
                             '   </tr>' +
                             '   <tr>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>1</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>2</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>3</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>4</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>5</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>6</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>7</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>8</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>9</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>10</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>11</span>' +
                             '      </td>' +
                             '      <td class="ItemsTableHeaderCell">' +
                             '         <span>12</span>' +
                             '      </td>' +
                             '   </tr>' +
                             '';

          for (var i = 0; i < postponeResItems.length; i++) {
              var idx = (i + 1);

              var PostponeResItemID = xmlValue(postponeResItems[i], "PostponeResItemID");
              var NKPDNickname = xmlValue(postponeResItems[i], "NKPDNickname");
              var NKPDID = xmlValue(postponeResItems[i], "NKPDID");
              var OfficersConditioned = xmlValue(postponeResItems[i], "OfficersConditioned");
              var OfficersAbsolutely = xmlValue(postponeResItems[i], "OfficersAbsolutely");
              var OfCandConditioned = xmlValue(postponeResItems[i], "OfCandConditioned");
              var OfCandAbsolutely = xmlValue(postponeResItems[i], "OfCandAbsolutely");
              var SergeantsConditioned = xmlValue(postponeResItems[i], "SergeantsConditioned");
              var SergeantsAbsolutely = xmlValue(postponeResItems[i], "SergeantsAbsolutely");
              var SoldiersConditioned = xmlValue(postponeResItems[i], "SoldiersConditioned");
              var SoldiersAbsolutely = xmlValue(postponeResItems[i], "SoldiersAbsolutely");

              tblItemsHTML += '<tr>' +
                              '   <td class="ItemsTableDataCell">' + idx + '</td>' +
                              '   <td class="ItemsTableDataCell" style="text-align: left; font-weight: bold;">' +
                              '     <span id="lblNKPDNickname' + idx + '">' + NKPDNickname + '</span>' +
                              '     <input type="hidden" id="hdnPostponeResItemID' + idx + '" value="' + PostponeResItemID + '" />' +
                              '     <input type="hidden" id="hdnNKPDID' + idx + '" value="' + NKPDID + '" />' +
                              '   </td>' +
                              '   <td class="ItemsTableDataCell"><span id="lblItemTotalAbsolutely' + idx + '"></span></td>' +
                              '   <td class="ItemsTableDataCell"><span id="lblItemTotalConditioned' + idx + '"></span></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtOfficersAbsolutely' + idx + '" type="text" class="InputField" value="' + OfficersAbsolutely + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtOfficersConditioned' + idx + '" type="text" class="InputField" value="' + OfficersConditioned + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtOfCandAbsolutely' + idx + '" type="text" class="InputField" value="' + OfCandAbsolutely + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtOfCandConditioned' + idx + '" type="text" class="InputField" value="' + OfCandConditioned + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtSergeantsAbsolutely' + idx + '" type="text" class="InputField" value="' + SergeantsAbsolutely + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtSergeantsConditioned' + idx + '" type="text" class="InputField" value="' + SergeantsConditioned + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtSoldiersAbsolutely' + idx + '" type="text" class="InputField" value="' + SoldiersAbsolutely + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '   <td class="ItemsTableDataCell"><input id="txtSoldiersConditioned' + idx + '" type="text" class="InputField" value="' + SoldiersConditioned + '" onblur="RecalcPostponeTotalsLightbox();" /></td>' +
                              '</tr>';
          };

          tblItemsHTML += '<tr>' +
                          '   <td class="ItemsTableDataCell"></td>' +
                          '   <td class="ItemsTableDataCell" style="text-align: left; font-weight: bold; font-style: italic;">Всичко</td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalAbsolutely"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalConditioned"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalOfficersAbsolutely"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalOfficersConditioned"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalOfCandAbsolutely"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalOfCandConditioned"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalSergeantsAbsolutely"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalSergeantsConditioned"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalSoldiersAbsolutely"></span></td>' +
                          '   <td class="ItemsTableDataCell"><span id="lblTotalSoldiersConditioned"></span></td>' +
                          '</tr>' +
                          '</table>' +
                          '<input type="hidden" id="hdnItemsCnt" value="' + postponeResItems.length + '" />';

          document.getElementById("pnlPostponeResItems").innerHTML = tblItemsHTML;

          RecalcPostponeTotalsLightbox();

          ShowTheLightBox();
      }

      var myAJAX = new AJAX(url, true, params, response_handler);
      myAJAX.Call();

      document.getElementById("<%= hfPostponeResCompanyID.ClientID %>").value = postponeResCompanyID; // setting ID (0 - if new)
   }

   function ShowTheLightBox() {
       // clean message label in the light box
       document.getElementById("spanLightBoxMessage").innerHTML = "";

       //shows the light box and "disable" rest of the page
       document.getElementById("HidePage").style.display = "";
       document.getElementById("PostponeResCompanyLightBox").style.display = "";
       CenterLightBox("PostponeResCompanyLightBox");

       initRecommendDialogHeight = document.getElementById("PostponeResCompanyLightBox").offsetHeight;
   }
   
   // Close the light box and refresh table
   function HidePostponeResCompanyLightBox()
   {
      document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();
      document.getElementById("HidePage").style.display = "none";
      document.getElementById("PostponeResCompanyLightBox").style.display = "none";
  }
   
   // Validate properties in the light box and generates appropriate error messages, if needed
   function ValidatePostponeResCompany() {
      var res = true;
      var lightBox = document.getElementById('PostponeResCompanyLightBox');
      var lightBoxMessage = document.getElementById("spanLightBoxMessage");

      var notValidFieldsCount = 0;
      lightBoxMessage.innerHTML = "";

      var companyId = document.getElementById("hdnCompanyID").value;

      if (companyId == "" || parseInt(companyId) <= 0) {
          res = false;
          lightBoxMessage.innerHTML += GetErrorMessageMandatory("Име на фирмата") + "</br>";
          notValidFieldsCount++;
      }

      if (document.getElementById("txtEmployeesCnt").value.Trim() != "") {
          if (!isInt(document.getElementById("txtEmployeesCnt").value)) {
              res = false;
              lightBoxMessage.innerHTML += GetErrorMessageNumber("Обща численост на персонала") + "</br>";
              notValidFieldsCount++;
          }
      }

      var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);
      for (var i = 1; i <= itemsCnt; i++) {
          var NKPDNickname = document.getElementById("lblNKPDNickname" + i).innerHTML;

          if (document.getElementById("txtOfficersAbsolutely" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtOfficersAbsolutely" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Офицери - безусловно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtOfficersConditioned" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtOfficersConditioned" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Офицери - условно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtOfCandAbsolutely" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtOfCandAbsolutely" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Офицерски кандидати - безусловно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtOfCandConditioned" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtOfCandConditioned" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Офицерски кандидати - условно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtSergeantsAbsolutely" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtSergeantsAbsolutely" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Сержанти / Старшини - безусловно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtSergeantsConditioned" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtSergeantsConditioned" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Сержанти / Старшини - условно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtSoldiersAbsolutely" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtSoldiersAbsolutely" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Войници / матроси - безусловно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }

          if (document.getElementById("txtSoldiersConditioned" + i).value.Trim() != "") {
              if (!isInt(document.getElementById("txtSoldiersConditioned" + i).value)) {
                  res = false;
                  lightBoxMessage.innerHTML += GetErrorMessageNumber(NKPDNickname + " - Войници / матроси - условно отср.") + "</br>";
                  notValidFieldsCount++;
              }
          }
      }

      if (notValidFieldsCount > 0) {
          lightBox.style.height = initRecommendDialogHeight + notValidFieldsCount * 10 + 5 + "px";
      }
      else {
          lightBox.style.height = initRecommendDialogHeight + "px";
      }

      return res;
   }
   
   // Saves item through ajax request, if light box values are valid, or displays generated error messages
   function SavePostponeResCompany() {
       if (ValidatePostponeResCompany()) {
          var compid = document.getElementById("hdnCompanyID").value;
          
          var url = "PostponeRes.aspx?AjaxMethod=JSSavePostponeResCompany";
          var params = "PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value +
                       "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value + 
                       "&PostponeResCompanyID=" + document.getElementById("<%= hfPostponeResCompanyID.ClientID %>").value +
                       "&CompanyID=" + custEncodeURI(TrimString(compid)) +
                       "&EmployeesCnt=" + custEncodeURI(TrimString(document.getElementById("txtEmployeesCnt").value));

          var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);
          params += "&ItemsCnt=" + itemsCnt;

          for (var i = 1; i <= itemsCnt; i++) {
             params += "&PostponeResItemID" + i + "=" + document.getElementById("hdnPostponeResItemID" + i).value +
                       "&NKPDID" + i + "=" + document.getElementById("hdnNKPDID" + i).value +
                       "&OfficersConditioned" + i + "=" + document.getElementById("txtOfficersConditioned" + i).value +
                       "&OfficersAbsolutely" + i + "=" + document.getElementById("txtOfficersAbsolutely" + i).value +
                       "&OfCandConditioned" + i + "=" + document.getElementById("txtOfCandConditioned" + i).value +
                       "&OfCandAbsolutely" + i + "=" + document.getElementById("txtOfCandAbsolutely" + i).value +
                       "&SergeantsConditioned" + i + "=" + document.getElementById("txtSergeantsConditioned" + i).value +
                       "&SergeantsAbsolutely" + i + "=" + document.getElementById("txtSergeantsAbsolutely" + i).value +
                       "&SoldiersConditioned" + i + "=" + document.getElementById("txtSoldiersConditioned" + i).value +
                       "&SoldiersAbsolutely" + i + "=" + document.getElementById("txtSoldiersAbsolutely" + i).value;
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
                  document.getElementById("<%= hfMsg.ClientID %>").value = "FailPostponeResCompanySave";
              else
                  document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessPostponeResCompanySave";

              if (hideDialog)
                  HidePostponeResCompanyLightBox();
          }

          var myAJAX = new AJAX(url, true, params, response_handler);
          myAJAX.Call();
      }
   }
   
   // Delete item through ajax request
   function DeletePostponeResCompany(postponeResCompanyID)
   {
       YesNoDialog('Желаете ли да изтриете позицията?', ConfirmYes, null);
        
        function ConfirmYes()
        {
            var url = "PostponeRes.aspx?AjaxMethod=JSDeletePostponeResCompany";
            var params = "PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value +
                         "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value +
                         "&PostponeResCompanyID=" + postponeResCompanyID;
                         
            function response_handler(xml)
            {			    	                
                if(xmlValue(xml, "response") != "OK")
                    document.getElementById("<%= hfMsg.ClientID %>").value = "FailPostponeResCompanyDelete";
                else
                {
                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessPostponeResCompanyDelete";
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
            
            var url = "PostponeRes.aspx?AjaxMethod=JSCheckForDataPerCompany";
            var params = "";
            params += "compid=" + companyId;
            params += "&PostponeYear=" + document.getElementById("<%= ddPostponeYear.ClientID %>").value;
            params += "&MilitaryDepartmentID=" + document.getElementById("<%= ddMilitaryDepartment.ClientID %>").value;
            var myAJAX = new AJAX(url, true, params, isCompanySelected_Callback);
            myAJAX.Call();
        }
        
        function isCompanySelected_Callback(xml) {
            var response = xml.getElementsByTagName("response")[0];
            var postponeResCompanyID = xmlValue(response, "postponeResCompanyID");

            if (postponeResCompanyID != "0" && postponeResCompanyID != document.getElementById("<%= hfPostponeResCompanyID.ClientID %>").value) {
                ShowPostponeResCompanyLightBox(postponeResCompanyID, companyId);
            }
            else {
                ShowPostponeResCompanyLightBox(0, companyId);
            }
        }
    }

    function RecalcPostponeTotalsLightbox() {
        var itemsCnt = parseInt(document.getElementById("hdnItemsCnt").value);

        var totalConditioned = 0;
        var totalAbsolutely = 0;

        var totalOfficersConditioned = 0;
        var totalOfficersAbsolutely = 0;
        var totalOfCandConditioned = 0;
        var totalOfCandAbsolutely = 0;
        var totalSergeantsConditioned = 0;
        var totalSergeantsAbsolutely = 0;
        var totalSoldiersConditioned = 0;
        var totalSoldiersAbsolutely = 0;

        for (var i = 1; i <= itemsCnt; i++) {
            var strOfficersConditioned = document.getElementById("txtOfficersConditioned" + i).value;
            var strOfficersAbsolutely = document.getElementById("txtOfficersAbsolutely" + i).value;
            var strOfCandConditioned = document.getElementById("txtOfCandConditioned" + i).value;
            var strOfCandAbsolutely = document.getElementById("txtOfCandAbsolutely" + i).value;
            var strSergeantsConditioned = document.getElementById("txtSergeantsConditioned" + i).value;
            var strSergeantsAbsolutely = document.getElementById("txtSergeantsAbsolutely" + i).value;
            var strSoldiersConditioned = document.getElementById("txtSoldiersConditioned" + i).value;
            var strSoldiersAbsolutely = document.getElementById("txtSoldiersAbsolutely" + i).value;
        
            var officersConditioned = (isNaN(parseInt(strOfficersConditioned)) ? 0 : parseInt(strOfficersConditioned));
            var officersAbsolutely = (isNaN(parseInt(strOfficersAbsolutely)) ? 0 : parseInt(strOfficersAbsolutely));
            var ofCandConditioned = (isNaN(parseInt(strOfCandConditioned)) ? 0 : parseInt(strOfCandConditioned));
            var ofCandAbsolutely = (isNaN(parseInt(strOfCandAbsolutely)) ? 0 : parseInt(strOfCandAbsolutely));
            var sergeantsConditioned = (isNaN(parseInt(strSergeantsConditioned)) ? 0 : parseInt(strSergeantsConditioned));
            var sergeantsAbsolutely = (isNaN(parseInt(strSergeantsAbsolutely)) ? 0 : parseInt(strSergeantsAbsolutely));
            var soldiersConditioned = (isNaN(parseInt(strSoldiersConditioned)) ? 0 : parseInt(strSoldiersConditioned));
            var soldiersAbsolutely = (isNaN(parseInt(strSoldiersAbsolutely)) ? 0 : parseInt(strSoldiersAbsolutely));

            totalOfficersConditioned += officersConditioned;
            totalOfficersAbsolutely += officersAbsolutely;
            totalOfCandConditioned += ofCandConditioned;
            totalOfCandAbsolutely += ofCandAbsolutely;
            totalSergeantsConditioned += sergeantsConditioned;
            totalSergeantsAbsolutely += sergeantsAbsolutely;
            totalSoldiersConditioned += soldiersConditioned;
            totalSoldiersAbsolutely += soldiersAbsolutely;

            var itemTotalConditioned = 0;
            var itemTotalAbsolutely = 0;

            itemTotalConditioned = officersConditioned + ofCandConditioned + sergeantsConditioned + soldiersConditioned;
            itemTotalAbsolutely = officersAbsolutely + ofCandAbsolutely + sergeantsAbsolutely + soldiersAbsolutely;

            totalConditioned += itemTotalConditioned;
            totalAbsolutely += itemTotalAbsolutely;

            document.getElementById("lblItemTotalConditioned" + i).innerHTML = itemTotalConditioned;
            document.getElementById("lblItemTotalAbsolutely" + i).innerHTML = itemTotalAbsolutely;
        }

        document.getElementById("lblTotalConditioned").innerHTML = totalConditioned;
        document.getElementById("lblTotalAbsolutely").innerHTML = totalAbsolutely;

        document.getElementById("lblTotalOfficersConditioned").innerHTML = totalOfficersConditioned;
        document.getElementById("lblTotalOfficersAbsolutely").innerHTML = totalOfficersAbsolutely;
        document.getElementById("lblTotalOfCandConditioned").innerHTML = totalOfCandConditioned;
        document.getElementById("lblTotalOfCandAbsolutely").innerHTML = totalOfCandAbsolutely;
        document.getElementById("lblTotalSergeantsConditioned").innerHTML = totalSergeantsConditioned;
        document.getElementById("lblTotalSergeantsAbsolutely").innerHTML = totalSergeantsAbsolutely;
        document.getElementById("lblTotalSoldiersConditioned").innerHTML = totalSoldiersConditioned;
        document.getElementById("lblTotalSoldiersAbsolutely").innerHTML = totalSoldiersAbsolutely;
    }

    function ShowCopyPostponeResLightBox() {
        //clean old values
        document.getElementById("<%= ddPostponeYearLightBox.ClientID %>").selectedIndex = 0;
        document.getElementById("<%= ddMilitaryDepartmentLightBox.ClientID %>").selectedIndex = 0;

        // clean message label in the light box and hide it            
        document.getElementById("spanCopyLightBoxMessage").style.display = "none";
        document.getElementById("spanCopyLightBoxMessage").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("CopyPostponeResLightBox").style.display = "";
        CenterLightBox("CopyPostponeResLightBox");

        initRecommendDialogHeight = document.getElementById("CopyPostponeResLightBox").offsetHeight;
    }

    function HideCopyPostponeResLightBox() {
        document.getElementById("HidePage").style.display = "none";
        document.getElementById("CopyPostponeResLightBox").style.display = "none";
    }

    function CopyPostponeRes() {
        if (ValidateCopyPostponeRes()) {
            var url = "PostponeRes.aspx?AjaxMethod=JSIsThereAnyCompanyData";
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
            var url = "PostponeRes.aspx?AjaxMethod=JSCopyPostponeRes";
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
                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCopyPostponeRes";
                    document.getElementById("<%= hdnBtnRefreshTable.ClientID %>").click();
                
                    HideCopyPostponeResLightBox();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
    }

    function ValidateCopyPostponeRes() {
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

