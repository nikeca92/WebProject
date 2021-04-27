<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="GroupTakingDown.aspx.cs" Inherits="PMIS.Reserve.ContentPages.GroupTakingDown" 
         Title="Групово снемане от отчет" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
    
.SelectionItem
{
    
}

.SelectionItem:hover
{
    cursor: pointer;
    background-color: #8D98B6;
    color: #FFFFFF;    
}

.SelectMilRepSpecLightBox
{
	min-width: 580px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 200px;
	left: 32%;	
	min-height: 200px;
	z-index: 1000;
	padding-top: 10px;
}
 
</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
//Function that sorts the table by a specific column
function SortTableBy(sort)
{
    //If sorting by the same column them set the direction to be DESC
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
    {
        sort = sort + 100;
    }
    
    //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
    document.getElementById("<%= btnRefresh.ClientID %>").click();
}

function ShowPrintGroupTakingDown()
{
    var hdnGender = document.getElementById("<%= hdnGender.ClientID %>").value;
    var hdnAge = document.getElementById("<%= hdnAge.ClientID %>").value;
    var todate = document.getElementById("<%= txtToDate.ClientID %>").value;
    var hdnMilitaryCategoryId = document.getElementById("<%= hdnMilitaryCategoryId.ClientID %>").value;
    var hdnMilitaryRankId = document.getElementById("<%= hdnMilitaryRankId.ClientID %>").value;
    var hdnAdministration = document.getElementById("<%= hdnAdministration.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnMilRepSpecTypeId = document.getElementById("<%= hdnMilRepSpecTypeId.ClientID %>").value;
    var hdnMilRepSpecId = document.getElementById("<%= hdnMilRepSpecId.ClientID %>").value;
    var chkIsPrimaryMilRepSpec = document.getElementById("<%= chkIsPrimaryMilRepSpec.ClientID %>").checked ? "1" : "0";
    
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintGroupTakingDown"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?GenderID=" + hdnGender
                + "&Age=" + hdnAge
                + "&todate=" + todate
                + "&MilitaryCategoryId=" + hdnMilitaryCategoryId
                + "&MilitaryRankId=" + hdnMilitaryRankId
                + "&AdministrationID=" + hdnAdministration
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&MilRepSpecTypeId=" + hdnMilRepSpecTypeId
                + "&MilRepSpecId=" + hdnMilRepSpecId
                + "&IsPrimaryMilRepSpec=" + chkIsPrimaryMilRepSpec
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function ShowPrintCheckMobileAppointment() {
    var hdnGender = document.getElementById("<%= hdnGender.ClientID %>").value;
    var hdnAge = document.getElementById("<%= hdnAge.ClientID %>").value;
    var todate = document.getElementById("<%= txtToDate.ClientID %>").value;
    var hdnMilitaryCategoryId = document.getElementById("<%= hdnMilitaryCategoryId.ClientID %>").value;
    var hdnMilitaryRankId = document.getElementById("<%= hdnMilitaryRankId.ClientID %>").value;
    var hdnAdministration = document.getElementById("<%= hdnAdministration.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnMilRepSpecTypeId = document.getElementById("<%= hdnMilRepSpecTypeId.ClientID %>").value;
    var hdnMilRepSpecId = document.getElementById("<%= hdnMilRepSpecId.ClientID %>").value;
    var chkIsPrimaryMilRepSpec = document.getElementById("<%= chkIsPrimaryMilRepSpec.ClientID %>").checked ? "1" : "0";

    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintCheckMobileAppointment"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?GenderID=" + hdnGender
                + "&Age=" + hdnAge
                + "&todate=" + todate
                + "&MilitaryCategoryId=" + hdnMilitaryCategoryId
                + "&MilitaryRankId=" + hdnMilitaryRankId
                + "&AdministrationID=" + hdnAdministration
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&MilRepSpecTypeId=" + hdnMilRepSpecTypeId
                + "&MilRepSpecId=" + hdnMilRepSpecId
                + "&IsPrimaryMilRepSpec=" + chkIsPrimaryMilRepSpec
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function btnGroupTakeDown_ClientClick()
{
    YesNoDialog("Сигурни ли сте, че желаете да снемете от отчет избраните " + document.getElementById("hdnReservistsCount").value + " човека?", ConfirmYes, null);

    function ConfirmYes()
    {
        document.getElementById("<%= btnGroupTakeDownSrv.ClientID %>").click();
    }
}

function ShowConfirmGroupDownLightBox() {
    document.getElementById("lblDialogCaption").innerHTML = "Изберете дата, към която избраните <b>" + document.getElementById("hdnReservistsCount").value + " човека </b> да бъдат <br/>снети от отчет и натиснете бутона <Снемане от военен отчет>."
    document.getElementById("HidePage").style.display = "";
    document.getElementById("dlgConfirmGroupDown").style.display = "";
    CenterLightBox("dlgConfirmGroupDown");
}

function HideConfirmGroupDownLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("dlgConfirmGroupDown").style.display = "none";
}

</script>

<div id="dlgConfirmGroupDown" class="SelectMilRepSpecLightBox" style="padding: 10px; display: none; text-align: center;">
    <img border='0' src='../Images/close.png' onclick="javascript:HideConfirmGroupDownLightBox();" style="cursor: pointer; float: right;" alt='Затвори' title='Затвори'/><br/>
        
    <div style="height: 15px;"></div>    
        
    <div>
       <span id="lblDialogCaption" class="InputLabel">Изберете дата, към която избраните хора да бъдат снети от отчет.</span>
    </div>
    
    <div style="height: 30px;"></div>
    <div>
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right;">
                    Дата на снемане от отчет:
                </td>
                <td style="text-align: left;">
                    <asp:TextBox runat="server" ID="txtDownDate" CssClass="InputField" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    Заповед №:
                </td>
                <td style="text-align: left;">
                    <asp:TextBox runat="server" ID="txtOrderNumber" CssClass="InputField" Width="240px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    Дата на заповед:
                </td>
                <td style="text-align: left;">
                    <asp:TextBox runat="server" ID="txtOrderDate" CssClass="InputField" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    Подписана от:
                </td>
                <td style="text-align: left;">
                    <asp:TextBox runat="server" ID="txtOrderSignedBy" CssClass="InputField" Width="240px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 30px;"></div>        
    <div style="width: 300px; margin: 0 auto;">
        <table style="width: 100%;">
           <tr>
              <td>
                 <div id="btnConfirmGorupDown" style="display: inline;" onclick="document.getElementById('<%= btnGroupTakeDownSrv.ClientID %>').click();HideConfirmGroupDownLightBox();" class="Button">
                      <i></i>
                      <div id="btnConfirmGorupDownText" style="width: 160px;">Снемане от военен отчет</div>
                      <b></b>
                 </div>
              </td>
              <td>
                 <div>&nbsp;</div>
              </td>
              <td>
                 <div id="btnCancelGroupDown" style="display: inline;" onclick="HideConfirmGroupDownLightBox();"
                      class="Button">
                      <i></i>
                      <div id="btnCancelGroupDownText" style="width: 70px;">Отказ</div>
                      <b></b>
                 </div>
              </td>
           </tr>
        </table>
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Групово снемане от отчет</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td>
                    <table>
                         <tr style="height: 25px;">
                            <td style="vertical-align: bottom; text-align: right; width: 150px;">
                               <asp:Label runat="server" ID="lblGender" CssClass="InputLabel">Пол:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;  width: 250px;">
                               <asp:DropDownList runat="server" ID="ddGender" CssClass="InputField" Width="90px"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: bottom; text-align: right; width: 130px;">
                               <asp:Label runat="server" ID="lblAge" CssClass="InputLabel">Възраст:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:TextBox runat="server" ID="txtAge" CssClass="InputField" Width="30px"></asp:TextBox> 
                               <asp:Label runat="server" ID="lblToDate" CssClass="InputLabel">Към дата:</asp:Label>
                               <asp:TextBox runat="server" ID="txtToDate" CssClass="InputField" Width="80px"></asp:TextBox>
                            </td>
                         </tr>
                         <tr style="height: 25px;">
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMilitaryCategory" CssClass="InputLabel">Категория:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMilitaryCategory" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryCategory_Changed"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMilitaryRank" CssClass="InputLabel">Военно звание:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMilitaryRank" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                         </tr>
                         <tr style="height: 25px;">
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblAdministration" CssClass="InputLabel">Работил/служил в:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddAdministration" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">На отчет в:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMilitaryDepartment" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                         </tr>
                         <tr style="height: 25px;">
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMilRepSpecType" CssClass="InputLabel">Тип ВОС:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMilRepSpecType" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMilRepSpecType_Changed"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMilRepSpec" CssClass="InputLabel">ВОС:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMilRepSpec" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>            
                         </tr>
                         <tr style="height: 25px;">
                            <td style="vertical-align: bottom; text-align: right;">
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:CheckBox runat="server" ID="chkIsPrimaryMilRepSpec" CssClass="InputField" Text="Основна ВОС" />
                            </td>            
                         </tr>
                    </table>
                </td>
             </tr> 
             <tr>
                <td style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr style="height: 25px;">
                <td style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                    <div style="padding-left: 30px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnPrintGroupTakingDown" runat="server" CssClass="Button" OnClientClick="ShowPrintGroupTakingDown(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnCheckMobileAppointment" runat="server" CssClass="Button" OnClientClick="ShowPrintCheckMobileAppointment(); return false;"><i></i><div style="width:110px; padding-left:5px;">Проверка за МН</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnGroupTakeDown" runat="server" CssClass="Button" OnClientClick="ShowConfirmGroupDownLightBox(); return false;"><i></i><div style="width:160px; padding-left:5px;">Снемане от военен отчет</div><b></b></asp:LinkButton>
                    <asp:Button ID="btnGroupTakeDownSrv" runat="server" style="display: none;" OnClick="btnGroupTakeDown_Click" />
                </td>
             </tr>             
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;" runat="server" id="pnlPaging" visible="false">
    <div style="width: 600px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">          
       <div style="display: inline; position: relative;">
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

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;">
    <div style="width: 950px; margin: 0 auto;">
       <div runat="server" id="pnlDataGrid" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 10px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnAge" runat="server" />
<asp:HiddenField ID="hdnGender" runat="server" />
<asp:HiddenField ID="hdnMilitaryCategoryId" runat="server" />
<asp:HiddenField ID="hdnMilitaryRankId" runat="server" />
<asp:HiddenField ID="hdnAdministration" runat="server" />
<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnMilRepSpecTypeId" runat="server" />
<asp:HiddenField ID="hdnMilRepSpecId" runat="server" />


<input type="hidden" id="CanLeave" value="true" />

<input type="hidden" id="hdnReservistsCount" value="<%= reservistsCount.ToString() %>" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
