<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportTechnics_AVIATION_EQUIP.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportTechnics_AVIATION_EQUIP" 
         Title="Списък на техниката на военен отчет" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<style type="text/css">

.ShadowContainer
{
    width: 1000px;
}

</style>

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

function ShowPrintReportAllAviationEquips() {
    var hdnTechnicsTypeKey = document.getElementById("<%= hdnTechnicsTypeKey.ClientID %>").value;
    var hdnInventoryNumber = document.getElementById("<%= hdnInventoryNumber.ClientID %>").value;
    var hdnTechnicsCategoryId = document.getElementById("<%= hdnTechnicsCategoryId.ClientID %>").value;
    var hdnAviationAirKindId = document.getElementById("<%= hdnAviationAirKindId.ClientID %>").value;
    var hdnAviationAirTypeId = document.getElementById("<%= hdnAviationAirTypeId.ClientID %>").value;

    var hdnAviationAirModelName = document.getElementById("<%= hdnAviationAirModelName.ClientID %>").value;
    
    var hdnMilitaryReportStatusId = document.getElementById("<%= hdnMilitaryReportStatusId.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnOwnershipNumber = document.getElementById("<%= hdnOwnershipNumber.ClientID %>").value;
    var hdnOwnershipName = document.getElementById("<%= hdnOwnershipName.ClientID %>").value;
    var hdnIsOwnershipAddress = document.getElementById("<%= hdnIsOwnershipAddress.ClientID %>").value;
    var hdnPostCode = document.getElementById("<%= hdnPostCode.ClientID %>").value;
    var hdnRegionId = document.getElementById("<%= hdnRegionId.ClientID %>").value;
    var hdnMunicipalityId = document.getElementById("<%= hdnMunicipalityId.ClientID %>").value;
    var hdnCityId = document.getElementById("<%= hdnCityId.ClientID %>").value;
    var hdnDistrictId = document.getElementById("<%= hdnDistrictId.ClientID %>").value;
    var hdnAddress = document.getElementById("<%= hdnAddress.ClientID %>").value;
    var hdnNormativeTechnicsId = document.getElementById("<%= hdnNormativeTechnicsId.ClientID %>").value;
    var hdnAppointmentIsDelivered = document.getElementById("<%= hdnAppointmentIsDelivered.ClientID %>").value;
    var hdnReadiness = document.getElementById("<%= hdnReadiness.ClientID %>").value;


    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportAllAviationEquips"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?TechnicsTypeKey=" + hdnTechnicsTypeKey
                + "&InventoryNumber=" + hdnInventoryNumber
                + "&TechnicsCategoryId=" + hdnTechnicsCategoryId
                + "&AviationAirKindId=" + hdnAviationAirKindId
                + "&AviationAirTypeId=" + hdnAviationAirTypeId
                
//                + "&AviationAirModelId=" + hdnAviationAirModelId

                + "&AviationAirModelName=" + hdnAviationAirModelName

                + "&MilitaryReportStatusId=" + hdnMilitaryReportStatusId
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&OwnershipNumber=" + hdnOwnershipNumber
                + "&OwnershipName=" + hdnOwnershipName
                + "&IsOwnershipAddress=" + hdnIsOwnershipAddress
                + "&PostCode=" + hdnPostCode
                + "&RegionId=" + hdnRegionId
                + "&MunicipalityId=" + hdnMunicipalityId
                + "&CityId=" + hdnCityId
                + "&DistrictId=" + hdnDistrictId
                + "&Address=" + hdnAddress
                + "&NormativeTechnicsId=" + hdnNormativeTechnicsId
                + "&AppointmentIsDelivered=" + hdnAppointmentIsDelivered
                + "&Readiness=" + hdnReadiness
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function ExportReport() {
    var hdnTechnicsTypeKey = document.getElementById("<%= hdnTechnicsTypeKey.ClientID %>").value;
    var hdnInventoryNumber = document.getElementById("<%= hdnInventoryNumber.ClientID %>").value;
    var hdnTechnicsCategoryId = document.getElementById("<%= hdnTechnicsCategoryId.ClientID %>").value;
    var hdnAviationAirKindId = document.getElementById("<%= hdnAviationAirKindId.ClientID %>").value;
    var hdnAviationAirTypeId = document.getElementById("<%= hdnAviationAirTypeId.ClientID %>").value;

    var hdnAviationAirModelName = document.getElementById("<%= hdnAviationAirModelName.ClientID %>").value;

    var hdnMilitaryReportStatusId = document.getElementById("<%= hdnMilitaryReportStatusId.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnOwnershipNumber = document.getElementById("<%= hdnOwnershipNumber.ClientID %>").value;
    var hdnOwnershipName = document.getElementById("<%= hdnOwnershipName.ClientID %>").value;
    var hdnIsOwnershipAddress = document.getElementById("<%= hdnIsOwnershipAddress.ClientID %>").value;
    var hdnPostCode = document.getElementById("<%= hdnPostCode.ClientID %>").value;
    var hdnRegionId = document.getElementById("<%= hdnRegionId.ClientID %>").value;
    var hdnMunicipalityId = document.getElementById("<%= hdnMunicipalityId.ClientID %>").value;
    var hdnCityId = document.getElementById("<%= hdnCityId.ClientID %>").value;
    var hdnDistrictId = document.getElementById("<%= hdnDistrictId.ClientID %>").value;
    var hdnAddress = document.getElementById("<%= hdnAddress.ClientID %>").value;
    var hdnNormativeTechnicsId = document.getElementById("<%= hdnNormativeTechnicsId.ClientID %>").value;
    var hdnAppointmentIsDelivered = document.getElementById("<%= hdnAppointmentIsDelivered.ClientID %>").value;
    var hdnReadiness = document.getElementById("<%= hdnReadiness.ClientID %>").value;


    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportAllAviationEquips"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?TechnicsTypeKey=" + hdnTechnicsTypeKey
                + "&InventoryNumber=" + hdnInventoryNumber
                + "&TechnicsCategoryId=" + hdnTechnicsCategoryId
                + "&AviationAirKindId=" + hdnAviationAirKindId
                + "&AviationAirTypeId=" + hdnAviationAirTypeId

    //                + "&AviationAirModelId=" + hdnAviationAirModelId

                + "&AviationAirModelName=" + hdnAviationAirModelName

                + "&MilitaryReportStatusId=" + hdnMilitaryReportStatusId
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&OwnershipNumber=" + hdnOwnershipNumber
                + "&OwnershipName=" + hdnOwnershipName
                + "&IsOwnershipAddress=" + hdnIsOwnershipAddress
                + "&PostCode=" + hdnPostCode
                + "&RegionId=" + hdnRegionId
                + "&MunicipalityId=" + hdnMunicipalityId
                + "&CityId=" + hdnCityId
                + "&DistrictId=" + hdnDistrictId
                + "&Address=" + hdnAddress
                + "&NormativeTechnicsId=" + hdnNormativeTechnicsId
                + "&AppointmentIsDelivered=" + hdnAppointmentIsDelivered
                + "&Readiness=" + hdnReadiness
                + "&SortBy=" + hdnSortBy + "&Export=true";

    window.location = url;
}

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
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
                <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblAirInvNumber" CssClass="InputLabel">Инвентарен номер:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <asp:TextBox runat="server" ID="txtAirInvNumber" CssClass="InputField" Width="255px"></asp:TextBox>
                </td>
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblTechnicsCategory" CssClass="InputLabel">Категория:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:DropDownList runat="server" ID="ddTechnicsCategory" CssClass="InputField" Width="300px"></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 25px;">      
                <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblAviationAirKind" CssClass="InputLabel">Вид:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <asp:DropDownList runat="server" ID="ddAviationAirKind" CssClass="InputField" Width="260px"></asp:DropDownList>
                </td>         
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblAviationAirType" CssClass="InputLabel">Тип:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:DropDownList runat="server" ID="ddAviationAirType" CssClass="InputField" Width="300px"></asp:DropDownList>
                </td>                 
             </tr>   
             <tr style="height: 25px;">      
                <td style="vertical-align: top; text-align: right;">
                   <asp:Label runat="server" ID="lblAviationAirModel" CssClass="InputLabel">Модел:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <%--<asp:DropDownList runat="server" ID="ddAviationAirModel" CssClass="InputField" Width="260px"></asp:DropDownList>--%>
                   <asp:TextBox runat="server" ID="txtAviationAirModelName" CssClass="InputField" Width="255px"></asp:TextBox>
                </td>                      
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblMilitaryReportStatus" CssClass="InputLabel">Състояние по отчета:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <asp:DropDownList runat="server" ID="ddMilitaryReportStatus" CssClass="InputField" Width="260px" ></asp:DropDownList>
                </td>
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">На отчет в:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:DropDownList runat="server" ID="ddMilitaryDepartment" CssClass="InputField" Width="300px"></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="Label1" CssClass="InputLabel" title="Нормативна категория">Нормативна к-я:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;" colspan="3">
                   <asp:DropDownList runat="server" ID="ddNormativeTechnics" CssClass="InputField" Width="735px" ></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 25px;">    
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAppointmentIsDelivered" CssClass="InputLabel">Връчено МН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddAppointmentIsDelivered" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblReadiness" CssClass="InputLabel">Вид резерв:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddReadiness" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 18px;">               
                 <td style="vertical-align: top; text-align: left; width: 150px; padding-left: 70px; padding-top: 15px;" colspan="4">
                   <asp:Label runat="server" ID="lblOwnership" CssClass="InputLabel" Font-Bold="true">Собственик</asp:Label>
                </td>
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: bottom; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblOwnershipNumber" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom; width: 200px;">
                   <asp:TextBox runat="server" ID="txtOwnershipNumber" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                 <td style="vertical-align: bottom; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblOwnershipName" CssClass="InputLabel">Трите имена /<br />Име на фирмата:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom; width: 300px;">
                   <asp:TextBox runat="server" ID="txtOwnershipName" CssClass="InputField" Width="295px"></asp:TextBox>
                </td>
             </tr>
             
             <tr style="height: 25px;">             
             </tr>             
             
             <tr style="height: 25px;">
                <td colspan="4">
                    <table>
                        <tr style="height: 25px;">
                            <td colspan="6" style="vertical-align: bottom; text-align: center;">
                                <asp:RadioButtonList ID="rblAddress" runat="server" CssClass="InputField" Width="400px" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text="Адрес на собственик" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="2" Text="Адрес по местодомуване"></asp:ListItem>                        
                            </asp:RadioButtonList>
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
                               <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMuniciplaity_Changed"></asp:DropDownList>
                            </td>                           
                            <td style="vertical-align: bottom; text-align: right;">
                                <asp:Label runat="server" ID="lblCity" CssClass="InputLabel" Width="140px">Населено място:</asp:Label>
                            </td>
                            <td style="vertical-align: bottom; text-align: left;">
                              <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="200px"  AutoPostBack="true" OnSelectedIndexChanged="ddCity_Changed"></asp:DropDownList>              
                            </td>
                           
                        </tr>
                        <tr style="height: 25px;">
                           <td style="vertical-align: top; text-align: right;">
                               <asp:Label runat="server" ID="lblAddress" CssClass="InputLabel">Aдрес:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;" colspan="3" rowspan="2">
                               <asp:TextBox ID="txtAddress" runat="server" CssClass="InputField" TextMode="MultiLine" Rows="3" Width="500px" ></asp:TextBox>
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                                <asp:Label runat="server" ID="lblDistrict" CssClass="InputLabel">Район:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:DropDownList runat="server" ID="ddDistrict" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                         </tr>
                         <tr style="height: 25px;">    
                             <td colspan="4"></td>          
                               
                                 <td style="text-align: right; vertical-align: bottom;">
                                    <asp:Label runat="server" ID="lblPostCode" CssClass="InputLabel">Пощенски код:</asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: bottom;">
                                    <asp:TextBox runat="server" ID="txtPostCode" CssClass="InputField" Width="50px"></asp:TextBox>
                                </td>
                        </tr>
                    </table>
                </td>
             </tr>
             
             <tr style="height: 25px;">             
             </tr> 
             
             <tr>
                <td colspan="4" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                    <div style="padding-left: 30px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnPrintReportAllAviationEquips" runat="server" CssClass="Button" OnClientClick="ShowPrintReportAllAviationEquips(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                </td>
             </tr>
             <tr>
                <td colspan="2" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div id="divNavigation" runat="server" style="text-align: center;">
    <div style="width: 870px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
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

<asp:HiddenField ID="hdnTechnicsTypeKey" runat="server" />
<asp:HiddenField ID="hdnInventoryNumber" runat="server" />
<asp:HiddenField ID="hdnTechnicsCategoryId" runat="server" />
<asp:HiddenField ID="hdnAviationAirKindId" runat="server" />
<asp:HiddenField ID="hdnAviationAirTypeId" runat="server" />

<%--<asp:HiddenField ID="hdnAviationAirModelId" runat="server" />--%>

<asp:HiddenField ID="hdnAviationAirModelName" runat="server" />

<asp:HiddenField ID="hdnMilitaryReportStatusId" runat="server" />
<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnOwnershipNumber" runat="server" />
<asp:HiddenField ID="hdnOwnershipName" runat="server" />
<asp:HiddenField ID="hdnIsOwnershipAddress" runat="server" />
<asp:HiddenField ID="hdnPostCode" runat="server" />
<asp:HiddenField ID="hdnRegionId" runat="server" />
<asp:HiddenField ID="hdnMunicipalityId" runat="server" />
<asp:HiddenField ID="hdnCityId" runat="server" />
<asp:HiddenField ID="hdnDistrictId" runat="server" />
<asp:HiddenField ID="hdnAddress" runat="server" />
<asp:HiddenField ID="hdnNormativeTechnicsId" runat="server" />
<asp:HiddenField ID="hdnAppointmentIsDelivered" runat="server" />
<asp:HiddenField ID="hdnReadiness" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
