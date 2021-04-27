<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchReservists.aspx.cs" Inherits="PMIS.Reserve.ContentPages.SearchReservists" 
         Title="Търсене на резервисти" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
.HiddenPnl{
    display:none;
}

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
    window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

    function EndRequestHandlerPage(sender, args) {
        WriteBenchmarkLog("Клиент 'Търсене на резервисти за попълване на заявка': UpdatePanel (Ajax) заявката е изпълнена");
    }

    //Call this when the page is loaded
    function PageLoad() {
        WriteBenchmarkLog("Клиент 'Търсене на резервисти за попълване на заявка': Екранът е зареден");
    }

    function WriteBenchmarkLog(str) {
        if (benchmarkLog.toLowerCase() != "true")
            return;

        var url = "SearchReservists.aspx?AjaxMethod=JSWriteBenchmarkLog";

        var params = "Message=" + custEncodeURI(str);

        function response_handler(xml) {
            var resultMsg = xmlValue(xml, "response");
            if (resultMsg != "OK") {
                alert("Възникна проблем при записа на времето за изпълнение");
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }


    //Function that sorts the table by a specific column
    function SortTableBy(sort) {
        //If sorting by the same column them set the direction to be DESC
        if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort) {
            sort = sort + 100;
        }

        //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
        document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
        document.getElementById("<%= btnRefresh.ClientID %>").click();
    }

    // Save FillReservistsRequest by AJAX call
    function ChooseReservist() {
        //YesNoDialog('Желаете ли избраният резервист да бъде използван за попълване на заявката и да му бъде издадено МН?', ConfirmYes, null);

        var reservistID = document.getElementById("hdnReservistID").value;

        //function ConfirmYes() {
        var url = "SearchReservists.aspx?AjaxMethod=JSChooseReservist";
        var params = "";
        params += "ReservistID=" + reservistID;
        params += "&RequestCommandPositionID=" + document.getElementById("<%= hfRequestCommandPositionID.ClientID %>").value;
        params += "&MilitaryDepartmentID=" + document.getElementById("<%= hfMilitaryDepartmentID.ClientID %>").value;
        params += "&Readiness=" + document.getElementById("<%= hfReadiness.ClientID %>").value;
        params += "&MilReportSpecialityID=" + document.getElementById("<%= ddSelectMilRepSpec.ClientID %>").value;
        params += "&NeedCourse=" + (document.getElementById("<%= cbNeedCourse.ClientID %>").checked ? "1" : "0");
        params += "&IsSuitableForMobAppoitmentDropDownList=" + (document.getElementById("<%= isSuitableForMobAppoitmentDropDownList.ClientID %>").checked ? "1" : "0");

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                document.getElementById("<%= btnBack.ClientID %>").click();
            }
            else {
                var message = xmlValue(xml, "message");
                document.getElementById("lblChooseReservistMessage").innerHTML = message;
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
        //}
    }

    // Open in new window AddEditReservist page in read only mode
    function PreviewReservist(reservistID) {
        var url = "";
        var pageName = "AddEditReservist"
        var param = "";

        url = "../ContentPages/" + pageName + ".aspx?ReservistId=" + reservistID + "&Preview=1";

        var uplPopup = window.open(url, pageName, param);

        if (uplPopup != null)
            uplPopup.focus();
    }

    function ShowSelectMilRepSpecLightBox(reservistId) {
        document.getElementById("hdnReservistID").value = reservistId;

        document.getElementById("HidePage").style.display = "";
        document.getElementById("divSelectMilRepSpec").style.display = "";
        CenterLightBox("divSelectMilRepSpec");
    }

    function HideSelectMilRepSpecLightBox() {
        document.getElementById("HidePage").style.display = "none";
        document.getElementById("divSelectMilRepSpec").style.display = "none";
    }

    function ClearFilter() {
        document.getElementById("<%= txtFirstAndSurName.ClientID %>").value = "";
        document.getElementById("<%= txtFamilyName.ClientID %>").value = "";
        document.getElementById("<%= txtInitials.ClientID %>").value = "";
        document.getElementById("<%= txtIdentNumber.ClientID %>").value = "";
        document.getElementById("<%= txtMobilAppointmentPosition.ClientID %>").value = "";
        document.getElementById("<%= txtWorkPosition.ClientID %>").value = "";
        document.getElementById("<%= ddMilRepSpecType.ClientID %>").value = "-1";
        document.getElementById("<%= isSuitableForMobAppoitmentDropDownList.ClientID %>").value = "-1";

        var dd = document.getElementById("<%= ddMilRepSpec.ClientID %>");
        while (dd.options.length > 0) {
            dd.remove(0);
        }

        document.getElementById("<%= ddPositionTitle.ClientID %>").value = "-1";
        document.getElementById("<%= chkIsPrimaryPositionTitle.ClientID %>").checked = true;
        document.getElementById("<%= ddMilitaryRank.ClientID %>").value = "-1";
        document.getElementById("<%= ddCivilSpeciality.ClientID %>").value = "-1";
        document.getElementById("<%= txtAge.ClientID %>").value = "";
        document.getElementById("<%= ddLanguage.ClientID %>").value = "-1";
        document.getElementById("<%= ddRegion.ClientID %>").value = "-1";
        
        //ddMuniciplaity.Items.Clear();
        dd = document.getElementById("<%= ddMuniciplaity.ClientID %>");
        while (dd.options.length > 0) {
            dd.remove(0);
        } 
        
        //ddCity.Items.Clear();
        dd = document.getElementById("<%= ddCity.ClientID %>");
        while (dd.options.length > 0) {
            dd.remove(0);
        } 
        
        document.getElementById("<%= ddEducation.ClientID %>").value = "-1";
        document.getElementById("<%= ddMilitaryTraining.ClientID %>").value = "-1";

        document.getElementById("<%= pnlPaging.ClientID %>").className = "HiddenPnl";
        document.getElementById("<%= pnlDataGrid.ClientID %>").innerHTML = "";
        document.getElementById("<%= pnlSearchHint.ClientID %>").className = "";
    }
</script>


<asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />
<asp:HiddenField ID="hfMilitaryDepartmentID" runat="server" />
<asp:HiddenField ID="hfMilitaryCommandID" runat="server" />
<asp:HiddenField ID="hfRequestCommandPositionID" runat="server" />
<asp:HiddenField ID="hfReadiness" runat="server" />
<asp:HiddenField ID="hfFromFulfilByCommand" runat="server" />

<div id="divSelectMilRepSpec" class="SelectMilRepSpecLightBox" style="padding: 10px;
    display: none; text-align: center;">
    <img border='0' src='../Images/close.png' onclick="javascript:HideSelectMilRepSpecLightBox();"
        style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
        
    <div style="height: 15px;"></div>    
        
    <div>
       <span class="HeaderText">Избраният резервист ще бъде използван за попълване на заявката. <div style="height: 10px;"></div> Изберете ВОС на която да бъде назначен.</span>
    </div>
    
    <div style="height: 15px;"></div>    
    
    <table style="width: 100%;">
       <tr>
          <td style="text-align: right; width: 80px;">
             <asp:Label runat="server" ID="lblSelectMilRepSpec" CssClass="InputLabel">ВОС:</asp:Label>
          </td>
          <td style="text-align: left;">
             <asp:DropDownList runat="server" ID="ddSelectMilRepSpec" CssClass="InputField" Width="525px"></asp:DropDownList>
          </td>
       </tr>
       <tr>
          <td colspan='2' style='padding-left: 45px; text-align: left;'>            
            <asp:CheckBox runat="server" ID="cbNeedCourse" CssClass="InputField" Text="Нуждае се от курс" Checked="false" />
          </td>
       </tr>
    </table>   
    
    <div style="height: 10px;"></div>
    <div>
        <span id="lblChooseReservistMessage" class="ErrorText"></span>
    </div>
    <div style="height: 10px;"></div>
    
    <div style="width: 250px; margin: 0 auto;">
        <table style="width: 100%;">
           <tr>
              <td>
                 <div id="btnChooseReservist" style="display: inline;" onclick="ChooseReservist();"
                      class="Button">
                      <i></i>
                      <div id="btnChooseReservistText" style="width: 110px;">
                          Издаване на МН</div>
                      <b></b>
                 </div>
              </td>
              <td>
                 <div>&nbsp;</div>
              </td>
              <td>
                 <div id="btnCancelChooseReservist" style="display: inline;" onclick="HideSelectMilRepSpecLightBox();"
                      class="Button">
                      <i></i>
                      <div id="btnCancelChooseReservistText" style="width: 70px;">
                          Отказ</div>
                      <b></b>
                 </div>
              </td>
           </tr>
        </table>
    </div>
    
    <input type="hidden" id="hdnReservistID" />
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Търсене на резервисти за попълване на заявка №</span>
   <br/>
   <span class="HeaderText" style='padding-top: 8px;'><span id="lblSubHeaderTitle" runat="server" style="font-size: 0.93em;"></span></span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr style="height: 25px;">
                <td style="vertical-align: top; text-align: right; width: 160px;">
                   <asp:Label runat="server" ID="lblFirstAndSurName" CssClass="InputLabel">Име и презиме:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:TextBox runat="server" ID="txtFirstAndSurName" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                <td style="vertical-align: top; text-align: right; width: 200px;">
                   <asp:Label runat="server" ID="lblIdentNumber" CssClass="InputLabel">ЕГН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtIdentNumber" CssClass="InputField" Width="100px"></asp:TextBox> 
                </td>
             </tr>
             <tr style="height: 25px;">
                <td style="vertical-align: top; text-align: right;">
                     <asp:Label runat="server" ID="lblFamilyName" CssClass="InputLabel">Фамилия:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtFamilyName" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                <td style="vertical-align: top; text-align: right;">
                    <asp:Label runat="server" ID="lblInitials" CssClass="InputLabel">Инициали:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtInitials" CssClass="InputField" Width="30px"></asp:TextBox>
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
                   <asp:Label runat="server" ID="lblMobilAppointmentPosition" CssClass="InputLabel">Бил с МН на длъжност:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:TextBox runat="server" ID="txtMobilAppointmentPosition" CssClass="InputField" Width="100px"></asp:TextBox>                   
                </td>
             </tr>
             
              <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                    <asp:Label runat="server" ID="lblMilRepSpec" CssClass="InputLabel">ВОС:</asp:Label>
                </td>
                <td colspan="3" style="text-align: left; vertical-align: bottom;">
                    <asp:UpdatePanel ID="upMilRepSpec" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddMilRepSpec" CssClass="InputField" Width="745px"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddMilRepSpecType" EventName="SelectedIndexChanged" />
                            
                        </Triggers>
                    </asp:UpdatePanel>                      
                </td>               
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                </td>
                <td colspan="3" style="text-align: left; vertical-align: bottom;">
                   <asp:CheckBox runat="server" ID="chkIsPrimaryMilRepSpec" CssClass="InputField" Text="Основна ВОС" Checked="true" />
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblPositionTitle" CssClass="InputLabel">Подходяща длъжност:</asp:Label>
                </td>
                <td colspan="1" style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddPositionTitle" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="isSuitableForMobAppoitmentLabel" CssClass="InputLabel">Подходящ за МН:</asp:Label>
                </td>
                <td colspan="1" style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="isSuitableForMobAppoitmentDropDownList" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>            
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                </td>
                <td colspan="3" style="text-align: left; vertical-align: bottom;">
                   <asp:CheckBox runat="server" ID="chkIsPrimaryPositionTitle" CssClass="InputField" Text="Основна длъжност" Checked="true" />
                </td>
             </tr>
                          
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryRank" CssClass="InputLabel">Военно звание:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:UpdatePanel ID="upMilitaryRank" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddMilitaryRank" CssClass="InputField" Width="200px"></asp:DropDownList>
                            <asp:CheckBox runat="server" ID="chkShowAllMilitaryRanks" CssClass="InputField" Text="Всички" OnCheckedChanged="chkShowAllMilitaryRanks_Check" AutoPostBack="true" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkShowAllMilitaryRanks" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>                      
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblCivilSpeciality" CssClass="InputLabel">Гражданска специалност:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddCivilSpeciality" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAge" CssClass="InputLabel">Възраст до:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:TextBox runat="server" ID="txtAge" CssClass="InputField" Width="100px"></asp:TextBox> 
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblLanguage" CssClass="InputLabel">Чужд език:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddLanguage" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             
              <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblRegion" CssClass="InputLabel">Област:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                  <asp:DropDownList runat="server" ID="ddRegion" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddRegion_Changed"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMuniciplaity" CssClass="InputLabel">Община:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                    <asp:UpdatePanel ID="upMuniciplaity" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMuniciplaity_Changed"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddRegion" EventName="SelectedIndexChanged" />
                            
                        </Triggers>
                        
                    </asp:UpdatePanel>    
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblCity" CssClass="InputLabel">Населено място:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                    <asp:UpdatePanel ID="upCity" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="200px"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddRegion" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddMuniciplaity" EventName="SelectedIndexChanged" />
                            
                        </Triggers>
                 </asp:UpdatePanel>
                   
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblWorkPosition" CssClass="InputLabel">По длъжност в месторабота:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:TextBox runat="server" ID="txtWorkPosition" CssClass="InputField" Width="195px"></asp:TextBox> 
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblWork" CssClass="InputLabel">Месторабота</asp:Label>
                   <asp:Label runat="server" ID="lblWorkCompany_UnifiedIdentityCode" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:TextBox ID="txtWorkCompany_UnifiedIdentityCode" runat="server" CssClass="InputField" Width="195px" ></asp:TextBox>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblWorkCompany_Name" CssClass="InputLabel">Име на фирмата:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:TextBox ID="txtWorkCompany_Name" runat="server" CssClass="InputField" Width="195px" ></asp:TextBox>
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblEducation" CssClass="InputLabel">Образование:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddEducation" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryTraining" CssClass="InputLabel">Военна подготовка:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryTraining" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             
             <tr>
                <td colspan="4" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr style="height: 25px;">
                <td colspan="4" style="padding-top: 10px;">
                   <div style="width:210px; height:32px; margin:0 auto;">    
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>                        
                                </td>
                                <td>
                                    <div id="_btnRemove" style="display: inline;" onclick="ClearFilter();" class="Button">
                                        <i></i>
                                        <div id="btnRemoveText" style="width: 70px;">Изчисти</div>
                                        <b></b>
                                    </div>
                                </td>
                            </tr>                        
                        </table>                   
                   </div>
                </td>
             </tr>             
          </table>
        </div>
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="height: 20px;"></div>
        <div style="text-align: center;" runat="server" id="pnlPaging" visible="true" class="HiddenPnl">
            <div style="width: 670px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">              
               <div style="display: inline; position: relative; top: -16px;">
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
        <asp:HiddenField ID="hdnSortBy" runat="server" />
        <asp:HiddenField ID="hdnPageIdx" runat="server" />
        <asp:HiddenField ID="hdnRefreshReason" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnFirst" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPrev" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnNext" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnLast" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnGoto" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Отказ</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<input type="hidden" id="CanLeave" value="true" />

<div id="VehicleModelLightBox" class="VehicleModelLightBox" style="display: none; text-align: center;">
        <input type='hidden' id='hdnVehicleModelId' name='hdnVehicleModelId' />
        <table style='text-align:center;'>
            <tr style='height: 15px'><td colspan="2"></td></tr>
            <tr>
                <td colspan='2' align='center'>
                    <span id='lblVehicleModelBoxTitle' class='SmallHeaderText'></span>
                </td>
            </tr>   
            <tr style='height: 17px'><td colspan="2"></td></tr>
            <tr style='min-height: 17px;'>
                <td align='right' style='width: 130px;'>
                    <span id='lblVehicleMakeLightBox' class='InputLabel'>Марка:</span>
                </td>
                <td align='left' style='min-width: 220px;'>
                    <span id='lblVehicleMakeValue' class='ReadOnlyValue'></span>
                </td>
            </tr>             
            <tr style='min-height: 17px;'>
                <td align='right' style='width: 130px;'>
                    <span id='lblVehicleModelName' class='InputLabel'>Модел автомобил:</span>
                </td>
                <td align='left' style='min-width: 220px;'>
                    <input type='text' id='txtVehicleModelName' class='RequiredInputField' style='width: 200px;' maxlength='300' />
                </td>
            </tr>             
            <tr style='height: 30px'>
                <td colspan='2'> 
                    <span id='lblVehicleModelLightBoxMessage' class='ErrorText' style='display: none;'></span>
               </td>
            </tr>
            <tr>
                <td colspan='2' style='text-align: center;'>
                    <table style='margin: 0 auto;'>
                       <tr>
                          <td style='text-align: center;'>
                             <div id='btnSaveVehicleModel' style='display: inline;' onclick='SaveVehicleModel();' class='Button'><i></i><div id='btnSaveVehicleModelText' style='width:70px;'>Запис</div><b></b></div>
                             <div id='btnCloseVehicleModelBox' style='display: inline;' onclick='HideVehicleModelLightBox();' class='Button'><i></i><div id='btnCloseVehickleModelBoxText' style='width:70px;'>Затвори</div><b></b></div>
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </div>

 </asp:Content>
