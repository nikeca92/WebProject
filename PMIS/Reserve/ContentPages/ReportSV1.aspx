<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportSV1.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportSV1" 
         Title="Отчетна ведомост за състоянието на ресурсите" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>
<script src="../Scripts/PickList.js" type="text/javascript"></script>

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
	width: 1350px;
}

.ShadowContainer
{
    margin: 0 auto;
	width: 1350px;
}

#SubShadowContainer
{
	margin: 0 auto;
	width: 1350px;
    min-width: 1350px;
}

</style>

<script type="text/javascript">
function CreateReportForm() 
{
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnMilitaryForceSortId = document.getElementById("<%= hdnMilitaryForceSortId.ClientID %>").value;
    var hdnMilitaryReportSpecialityId = document.getElementById("<%= hdnMilitaryReportSpecialityId.ClientID %>").value;

    var hdnPostCode = document.getElementById("<%= hdnPostCode.ClientID %>").value;
    var hdnRegionId = document.getElementById("<%= hdnRegionId.ClientID %>").value;
    var hdnMunicipalityId = document.getElementById("<%= hdnMunicipalityId.ClientID %>").value;
    var hdnCityId = document.getElementById("<%= hdnCityId.ClientID %>").value;
    var hdnDistrictId = document.getElementById("<%= hdnDistrictId.ClientID %>").value;
    var hdnAddress = document.getElementById("<%= hdnAddress.ClientID %>").value;

    var reportForm = document.createElement("form");
    reportForm.target = "_blank";
    reportForm.method = "POST";
    reportForm.action = "../PrintContentPages/PrintReportSV1.aspx";

    var militaryDepartmentIdInput = document.createElement("input");
    militaryDepartmentIdInput.type = "text";
    militaryDepartmentIdInput.name = "MilitaryDepartmentId";
    militaryDepartmentIdInput.value = hdnMilitaryDepartmentId;
    reportForm.appendChild(militaryDepartmentIdInput);

    var militaryForceSortIdInput = document.createElement("input");
    militaryForceSortIdInput.type = "text";
    militaryForceSortIdInput.name = "MilitaryForceSortId";
    militaryForceSortIdInput.value = hdnMilitaryForceSortId;
    reportForm.appendChild(militaryForceSortIdInput);

    var militaryReportSpecialityIdInput = document.createElement("input");
    militaryReportSpecialityIdInput.type = "text";
    militaryReportSpecialityIdInput.name = "MilitaryReportSpecialityId";
    militaryReportSpecialityIdInput.value = hdnMilitaryReportSpecialityId;
    reportForm.appendChild(militaryReportSpecialityIdInput);

    var postCodeInput = document.createElement("input");
    postCodeInput.type = "text";
    postCodeInput.name = "PostCode";
    postCodeInput.value = hdnPostCode;
    reportForm.appendChild(postCodeInput);

    var regionIdInput = document.createElement("input");
    regionIdInput.type = "text";
    regionIdInput.name = "RegionId";
    regionIdInput.value = hdnRegionId;
    reportForm.appendChild(regionIdInput);

    var municipalityIdInput = document.createElement("input");
    municipalityIdInput.type = "text";
    municipalityIdInput.name = "MunicipalityId";
    municipalityIdInput.value = hdnMunicipalityId;
    reportForm.appendChild(municipalityIdInput);

    var cityIdInput = document.createElement("input");
    cityIdInput.type = "text";
    cityIdInput.name = "CityId";
    cityIdInput.value = hdnCityId;
    reportForm.appendChild(cityIdInput);

    var districtIdInput = document.createElement("input");
    districtIdInput.type = "text";
    districtIdInput.name = "DistrictId";
    districtIdInput.value = hdnDistrictId;
    reportForm.appendChild(districtIdInput);

    var addressInput = document.createElement("input");
    addressInput.type = "text";
    addressInput.name = "Address";
    addressInput.value = hdnAddress;
    reportForm.appendChild(addressInput);
    
    return reportForm;
}

function ShowPrintReport()
{
    var reportForm = CreateReportForm();
    document.body.appendChild(reportForm);

    reportForm.submit();

    document.body.removeChild(reportForm);
}

function ExportReport() 
{
    var reportForm = CreateReportForm();
    reportForm.target = "_self";
    
    var exportInput = document.createElement("input");
    exportInput.type = "text";
    exportInput.name = "Export";
    exportInput.value = true;
    reportForm.appendChild(exportInput);

    document.body.appendChild(reportForm);

    reportForm.submit();

    document.body.removeChild(reportForm);
}

window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

//Call this function when the page is loaded
function PageLoad()
{
    LoadPickLists();

    if (document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value != "")
        PickListUtil.SetSelection("pickListMilitaryDepartments", document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value);

    if (document.getElementById("<%= hdnMilitaryReportSpecialitiesSelected.ClientID %>").value != "")
        PickListUtil.SetSelection("pickListMilitaryReportSpecialities", document.getElementById("<%= hdnMilitaryReportSpecialitiesSelected.ClientID %>").value);   
}

function EndRequestHandlerPage(sender, args) {
    ReLoadPickLists();
}

function LoadPickLists()
{
    var configPickListMilitaryDepartments =
    {
        width: 275,
        allLabel: "<Всички>"
    }

    militaryDepartments = document.getElementById("<%= hdnMilitaryDepartmentJson.ClientID %>").value;
    militaryDepartments = eval(militaryDepartments);
    PickListUtil.AddPickList("pickListMilitaryDepartments", militaryDepartments, "tdPickListMilitaryDepartments", configPickListMilitaryDepartments);

    var configPickListMilitaryReportSpecialities =
    {
        width: 275,
        allLabel: "<Всички>"
    }

    militaryReportSpecialities = document.getElementById("<%= hdnMilitaryReportSpecialitiesJson.ClientID %>").value;
    militaryReportSpecialities = eval(militaryReportSpecialities);
    PickListUtil.AddPickList("pickListMilitaryReportSpecialities", militaryReportSpecialities, "tdPickListMilitaryReportSpecialities", configPickListMilitaryReportSpecialities);
}

function ReLoadPickLists()
{
    var configPickListMilitaryReportSpecialities =
    {
        width: 275,
        allLabel: "<Всички>"
    }

    militaryReportSpecialities = document.getElementById("<%= hdnMilitaryReportSpecialitiesJson.ClientID %>").value;
    militaryReportSpecialities = eval(militaryReportSpecialities);
    PickListUtil.ReloadPickList("pickListMilitaryReportSpecialities", militaryReportSpecialities, configPickListMilitaryReportSpecialities);

    militaryReportSpecialitiesSelected = document.getElementById("<%= hdnMilitaryReportSpecialitiesSelected.ClientID %>").value;

    PickListUtil.SetSelection("pickListMilitaryReportSpecialities", militaryReportSpecialitiesSelected);

    //refresh_Val("pickListMilitaryReportSpecialities");
}

function SetPickListsSelection()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryDepartments");
    document.getElementById("<%= hdnMilitaryReportSpecialitiesSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryReportSpecialities");
}

function btnClear_Click()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = "";
    PickListUtil.ClearSelection("pickListMilitaryDepartments");
    document.getElementById("<%= ddMilitaryForceSort.ClientID %>").value = optionAllValue;
    document.getElementById("<%= hdnMilitaryReportSpecialitiesSelected.ClientID %>").value = "";
    PickListUtil.ClearSelection("pickListMilitaryReportSpecialities");

    document.getElementById("<%= ddRegion.ClientID %>").value = "-1";
    ClearSelectList(document.getElementById("<%= ddMuniciplaity.ClientID %>"), true);
    ClearSelectList(document.getElementById("<%= ddCity.ClientID %>"), true);
    ClearSelectList(document.getElementById("<%= ddDistrict.ClientID %>"), true);
    document.getElementById("<%= txtPostCode.ClientID %>").value = "";
    document.getElementById("<%= txtAddress.ClientID %>").value = "";
}

function OnDdlMilitaryForceSortSelectedIndexChange() 
{
    PickListUtil.ClearSelection("pickListMilitaryReportSpecialities");
}

</script>


<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Отчетна ведомост за състоянието на ресурсите</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td colspan="4"></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">                                      
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Военно окръжие:</asp:Label>
                   <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentJson" />
                   <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentSelected" />
                </td>
                <td style="text-align: left; vertical-align: top; width: 280px;">
                   <div id="tdPickListMilitaryDepartments"></div>
                </td>                
                <td style="vertical-align: middle; text-align: right; width: 90px;">                                      
                   <asp:Label runat="server" ID="lblMilitaryForceSort" CssClass="InputLabel">Род войски:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: middle;">
                   <asp:DropDownList runat="server" ID="ddMilitaryForceSort" CssClass="InputField" 
                        Width="200px"  onselectedindexchanged="ddMilitaryForceSort_SelectedIndexChanged" AutoPostBack="true"
                        onChange="javascript:OnDdlMilitaryForceSortSelectedIndexChange();"></asp:DropDownList>
                </td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">                                      
                   <asp:Label runat="server" ID="lblMilitaryReportSpeciality" CssClass="InputLabel">ВОС:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 280px;">
                   <div id="tdPickListMilitaryReportSpecialities"></div>
                </td>
                <td colspan="2" style="text-align: left; vertical-align: top; width: 280px;"></td>
               
             </tr>
             <tr style="height: 10px;">
                 <td colspan="4"></td>
             </tr>
             <tr>
                 <td colspan="5" >
                     <table width="100%" style="border-collapse:collapse; vertical-align: middle;">
                        <tr style="height: 25px;">
                            <td colspan="6" style="text-align: left;">Постоянен адрес</td>  
                        </tr>
                        <tr style="height: 25px;">
                            <td style="vertical-align: middle; text-align: right;">
                                <asp:Label runat="server" ID="lblRegion" CssClass="InputLabel">Област:</asp:Label> 
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:DropDownList runat="server" ID="ddRegion" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddRegion_Changed"></asp:DropDownList>                  
                            </td>
                            <td style="vertical-align: middle; text-align: right;">
                                <asp:Label runat="server" ID="lblMuniciplaity" CssClass="InputLabel">Община:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMuniciplaity_Changed"></asp:DropDownList>
                            </td>                           
                            <td style="vertical-align: middle; text-align: right;">
                               <asp:Label runat="server" ID="lblCity" CssClass="InputLabel" Width="140px">Населено място:</asp:Label> 
                            </td>
                            <td style="vertical-align: bottom; text-align: left;">
                               <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddCity_Changed"></asp:DropDownList>             
                            </td>
                        </tr>
                        <tr style="height: 25px;">
                           <td style="vertical-align: top; text-align: right;">
                               <asp:Label runat="server" ID="lblAddress" CssClass="InputLabel">Aдрес:</asp:Label> 
                           </td>
                           <td style="text-align: left; vertical-align: bottom;" colspan="3" rowspan="2">
                               <asp:TextBox ID="txtAddress" runat="server" CssClass="InputField" TextMode="MultiLine" Rows="3" Width="485px" ></asp:TextBox>
                           </td>
                           <td style="vertical-align: middle; text-align: right;">
                               <asp:Label runat="server" ID="lblDistrict" CssClass="InputLabel">Район:</asp:Label>
                           </td>
                           <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddDistrict" CssClass="InputField" Width="200px"></asp:DropDownList> 
                           </td>
                         </tr>
                         <tr style="height: 25px;">    
                            <td colspan="4"></td>          
                            <td style="text-align: right; vertical-align: middle;">
                                <asp:Label runat="server" ID="lblPostCode" CssClass="InputLabel">Пощенски код:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:TextBox runat="server" ID="txtPostCode" CssClass="InputField" Width="50px"></asp:TextBox>
                            </td>
                         </tr>
                     </table>
                 </td>
             </tr>
             
             
             
             <tr style="height: 10px;">
                 <td colspan="4"></td>
             </tr>
             <tr >
                <td colspan="4" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="4" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" OnClientClick="btnClear_Click();" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline">
                        </div>
                        <asp:LinkButton ID="btnPrintReport" runat="server" CssClass="Button" OnClientClick="ShowPrintReport(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                    </center>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<Triggers>
   <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="ddMilitaryForceSort" EventName="SelectedIndexChanged" />
</Triggers>
<ContentTemplate>

    <asp:HiddenField runat="server" ID="hdnMilitaryReportSpecialitiesJson" />
    <asp:HiddenField runat="server" ID="hdnMilitaryReportSpecialitiesSelected" />

<div style="height: 30px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;" runat="server" id="pnlPaging" visible="false">
    <div style="width: 620px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">              
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
    <div style="width: 1300px; margin: 0 auto;">
       <div runat="server" id="pnlReportGrid" style="text-align: center;"></div>
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

<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnMilitaryForceSortId" runat="server" />
<asp:HiddenField ID="hdnMilitaryReportSpecialityId" runat="server" />

<asp:HiddenField ID="hdnRegionId" runat="server" />
<asp:HiddenField ID="hdnMunicipalityId" runat="server" />
<asp:HiddenField ID="hdnCityId" runat="server" />
<asp:HiddenField ID="hdnDistrictId" runat="server" />
<asp:HiddenField ID="hdnAddress" runat="server" />
<asp:HiddenField ID="hdnPostCode" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
