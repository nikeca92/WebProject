<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageReservists.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ManageReservists" 
         Title="Списък на водените на военен отчет" %>

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
    
</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">

window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

function EndRequestHandlerPage(sender, args)
{
    WriteBenchmarkLog("Клиент 'Списък на водените на военен отчет': UpdatePanel (Ajax) заявката е изпълнена");
}

//Call this when the page is loaded
function PageLoad()
{
    WriteBenchmarkLog("Клиент 'Списък на водените на военен отчет': Екранът е зареден");
}

function WriteBenchmarkLog(str)
{
    if (benchmarkLog.toLowerCase() != "true")
        return;

    var url = "ManageReservists.aspx?AjaxMethod=JSWriteBenchmarkLog";

    var params = "Message=" + custEncodeURI(str);

    function response_handler(xml)
    {
        var resultMsg = xmlValue(xml, "response");
        if (resultMsg != "OK")
        {
            alert("Възникна проблем при записа на времето за изпълнение");
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

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

// Redirect to AddEditReservist page
function ChooseReservist(reservistID)
{
    JSRedirect("AddEditReservist.aspx?ReservistId=" + reservistID);
}

function ShowPrintAllReservists() {
    var hdnFirstAndSurName = document.getElementById("<%= hdnFirstAndSurName.ClientID %>").value;
    var hdnFamilyName = document.getElementById("<%= hdnFamilyName.ClientID %>").value;
    var hdnInitials = document.getElementById("<%= hdnInitials.ClientID %>").value;
    var hdnIdentNumber = document.getElementById("<%= hdnIdentNumber.ClientID %>").value;
    var hdnMilitaryCategoryId = document.getElementById("<%= hdnMilitaryCategoryId.ClientID %>").value;
    var hdnMilitaryRankId = document.getElementById("<%= hdnMilitaryRankId.ClientID %>").value;
    var hdnMilitaryReportStatusId = document.getElementById("<%= hdnMilitaryReportStatusId.ClientID %>").value;
    var hdnMilitaryCommand = document.getElementById("<%= hdnMilitaryCommand.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnPosition = document.getElementById("<%= hdnPosition.ClientID %>").value;
    var hdnMilAppointedRepSpecTypeId = document.getElementById("<%= hdnMilAppointedRepSpecTypeId.ClientID %>").value;
    var hdnMilAppointedRepSpecId = document.getElementById("<%= hdnMilAppointedRepSpecId.ClientID %>").value;
    var hdnMilRepSpecTypeId = document.getElementById("<%= hdnMilRepSpecTypeId.ClientID %>").value;
    var hdnMilRepSpecId = document.getElementById("<%= hdnMilRepSpecId.ClientID %>").value;
    var hdnPositionTitleId = document.getElementById("<%= hdnPositionTitleId.ClientID %>").value;
    var hdnIsPrimaryPositionTitle = document.getElementById("<%= hdnIsPrimaryPositionTitle.ClientID %>").value;
    var hdnAdministrationId = document.getElementById("<%= hdnAdministrationId.ClientID %>").value;
    var hdnLanguageId = document.getElementById("<%= hdnLanguageId.ClientID %>").value;
    var hdnEducationId = document.getElementById("<%= hdnEducationId.ClientID %>").value;
    var hdnCivilSpecialityId = document.getElementById("<%= hdnCivilSpecialityId.ClientID %>").value;
    var hdnIsPermAddress = document.getElementById("<%= hdnIsPermAddress.ClientID %>").value;
    var hdnPostCode = document.getElementById("<%= hdnPostCode.ClientID %>").value;
    var hdnRegionId = document.getElementById("<%= hdnRegionId.ClientID %>").value;
    var hdnMunicipalityId = document.getElementById("<%= hdnMunicipalityId.ClientID %>").value;
    var hdnCityId = document.getElementById("<%= hdnCityId.ClientID %>").value;
    var hdnDistrictId = document.getElementById("<%= hdnDistrictId.ClientID %>").value;
    var hdnAddress = document.getElementById("<%= hdnAddress.ClientID %>").value;
    var hdnWorkCompany_UnifiedIdentityCode = document.getElementById("<%= hdnWorkCompany_UnifiedIdentityCode.ClientID %>").value;
    var hdnWorkCompany_Name = document.getElementById("<%= hdnWorkCompany_Name.ClientID %>").value;
    var hdnHasBeenOnMission = document.getElementById("<%= hdnHasBeenOnMission.ClientID %>").value;
    var hdnAppointmentIsDelivered = document.getElementById("<%= hdnAppointmentIsDelivered.ClientID %>").value;
    var hdnSuitableForMobAppointment = document.getElementById("<%=hdnSuitableForMobAppointment.ClientID %>").value;
    var hdnReadiness = document.getElementById("<%= hdnReadiness.ClientID %>").value;
    var hdnProfessionId = document.getElementById("<%= hdnProfessionId.ClientID %>").value;
    var hdnSpecialityId = document.getElementById("<%= hdnSpecialityId.ClientID %>").value;
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAllReservists"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?FirstAndSurName=" + hdnFirstAndSurName
                + "&FamilyName=" + hdnFamilyName
                + "&Initials=" + hdnInitials
                + "&IdentNumber=" + hdnIdentNumber
                + "&MilitaryCategoryId=" + hdnMilitaryCategoryId
                + "&MilitaryRankId=" + hdnMilitaryRankId
                + "&MilitaryReportStatusId=" + hdnMilitaryReportStatusId
                + "&MilitaryCommand=" + hdnMilitaryCommand
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&Position=" + hdnPosition
                + "&MilAppointedRepSpecTypeId=" + hdnMilAppointedRepSpecTypeId
                + "&MilAppointedRepSpecId=" + hdnMilAppointedRepSpecId
                + "&MilRepSpecTypeId=" + hdnMilRepSpecTypeId
                + "&MilRepSpecId=" + hdnMilRepSpecId
                + "&PositionTitleId=" + hdnPositionTitleId
                + "&IsPrimaryPositionTitle=" + hdnIsPrimaryPositionTitle
                + "&AdministrationId=" + hdnAdministrationId
                + "&LanguageId=" + hdnLanguageId
                + "&EducationId=" + hdnEducationId
                + "&CivilSpecialityId=" + hdnCivilSpecialityId
                + "&IsPermAddress=" + hdnIsPermAddress
                + "&PostCode=" + hdnPostCode
                + "&RegionId=" + hdnRegionId
                + "&MunicipalityId=" + hdnMunicipalityId
                + "&CityId=" + hdnCityId
                + "&DistrictId=" + hdnDistrictId
                + "&Address=" + hdnAddress
                + "&WorkCompany_UnifiedIdentityCode=" + hdnWorkCompany_UnifiedIdentityCode
                + "&WorkCompany_Name=" + hdnWorkCompany_Name
                + "&HasBeenOnMission=" + hdnHasBeenOnMission
                + "&AppointmentIsDelivered=" + hdnAppointmentIsDelivered
                + "&hdnSuitableForMobAppointment=" + hdnSuitableForMobAppointment
                + "&Readiness=" + hdnReadiness
                + "&ProfessionId=" + hdnProfessionId
                + "&SpecialityId=" + hdnSpecialityId       
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function ExportAllReservists() {
    var hdnFirstAndSurName = document.getElementById("<%= hdnFirstAndSurName.ClientID %>").value;
    var hdnFamilyName = document.getElementById("<%= hdnFamilyName.ClientID %>").value;
    var hdnInitials = document.getElementById("<%= hdnInitials.ClientID %>").value;
    var hdnIdentNumber = document.getElementById("<%= hdnIdentNumber.ClientID %>").value;
    var hdnMilitaryCategoryId = document.getElementById("<%= hdnMilitaryCategoryId.ClientID %>").value;
    var hdnMilitaryRankId = document.getElementById("<%= hdnMilitaryRankId.ClientID %>").value;
    var hdnMilitaryReportStatusId = document.getElementById("<%= hdnMilitaryReportStatusId.ClientID %>").value;
    var hdnMilitaryCommand = document.getElementById("<%= hdnMilitaryCommand.ClientID %>").value;
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnPosition = document.getElementById("<%= hdnPosition.ClientID %>").value;
    var hdnMilAppointedRepSpecTypeId = document.getElementById("<%= hdnMilAppointedRepSpecTypeId.ClientID %>").value;
    var hdnMilAppointedRepSpecId = document.getElementById("<%= hdnMilAppointedRepSpecId.ClientID %>").value;
    var hdnMilRepSpecTypeId = document.getElementById("<%= hdnMilRepSpecTypeId.ClientID %>").value;
    var hdnMilRepSpecId = document.getElementById("<%= hdnMilRepSpecId.ClientID %>").value;
    var hdnPositionTitleId = document.getElementById("<%= hdnPositionTitleId.ClientID %>").value;
    var hdnIsPrimaryPositionTitle = document.getElementById("<%= hdnIsPrimaryPositionTitle.ClientID %>").value;
    var hdnAdministrationId = document.getElementById("<%= hdnAdministrationId.ClientID %>").value;
    var hdnLanguageId = document.getElementById("<%= hdnLanguageId.ClientID %>").value;
    var hdnEducationId = document.getElementById("<%= hdnEducationId.ClientID %>").value;
    var hdnCivilSpecialityId = document.getElementById("<%= hdnCivilSpecialityId.ClientID %>").value;
    var hdnIsPermAddress = document.getElementById("<%= hdnIsPermAddress.ClientID %>").value;
    var hdnPostCode = document.getElementById("<%= hdnPostCode.ClientID %>").value;
    var hdnRegionId = document.getElementById("<%= hdnRegionId.ClientID %>").value;
    var hdnMunicipalityId = document.getElementById("<%= hdnMunicipalityId.ClientID %>").value;
    var hdnCityId = document.getElementById("<%= hdnCityId.ClientID %>").value;
    var hdnDistrictId = document.getElementById("<%= hdnDistrictId.ClientID %>").value;
    var hdnAddress = document.getElementById("<%= hdnAddress.ClientID %>").value;
    var hdnWorkCompany_UnifiedIdentityCode = document.getElementById("<%= hdnWorkCompany_UnifiedIdentityCode.ClientID %>").value;
    var hdnWorkCompany_Name = document.getElementById("<%= hdnWorkCompany_Name.ClientID %>").value;
    var hdnHasBeenOnMission = document.getElementById("<%= hdnHasBeenOnMission.ClientID %>").value;
    var hdnAppointmentIsDelivered = document.getElementById("<%= hdnAppointmentIsDelivered.ClientID %>").value;
    var hdnSuitableForMobAppointment = document.getElementById("<%=hdnSuitableForMobAppointment.ClientID %>").value;
    var hdnReadiness = document.getElementById("<%= hdnReadiness.ClientID %>").value;
    var hdnProfessionId = document.getElementById("<%= hdnProfessionId.ClientID %>").value;
    var hdnSpecialityId = document.getElementById("<%= hdnSpecialityId.ClientID %>").value;
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAllReservists"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?FirstAndSurName=" + hdnFirstAndSurName
                + "&FamilyName=" + hdnFamilyName
                + "&Initials=" + hdnInitials
                + "&IdentNumber=" + hdnIdentNumber
                + "&MilitaryCategoryId=" + hdnMilitaryCategoryId
                + "&MilitaryRankId=" + hdnMilitaryRankId
                + "&MilitaryReportStatusId=" + hdnMilitaryReportStatusId
                + "&MilitaryCommand=" + hdnMilitaryCommand
                + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&Position=" + hdnPosition
                + "&MilAppointedRepSpecTypeId=" + hdnMilAppointedRepSpecTypeId
                + "&MilAppointedRepSpecId=" + hdnMilAppointedRepSpecId
                + "&MilRepSpecTypeId=" + hdnMilRepSpecTypeId
                + "&MilRepSpecId=" + hdnMilRepSpecId
                + "&PositionTitleId=" + hdnPositionTitleId
                + "&IsPrimaryPositionTitle=" + hdnIsPrimaryPositionTitle
                + "&AdministrationId=" + hdnAdministrationId
                + "&LanguageId=" + hdnLanguageId
                + "&EducationId=" + hdnEducationId
                + "&CivilSpecialityId=" + hdnCivilSpecialityId
                + "&IsPermAddress=" + hdnIsPermAddress
                + "&PostCode=" + hdnPostCode
                + "&RegionId=" + hdnRegionId
                + "&MunicipalityId=" + hdnMunicipalityId
                + "&CityId=" + hdnCityId
                + "&DistrictId=" + hdnDistrictId
                + "&Address=" + hdnAddress
                + "&WorkCompany_UnifiedIdentityCode=" + hdnWorkCompany_UnifiedIdentityCode
                + "&WorkCompany_Name=" + hdnWorkCompany_Name
                + "&HasBeenOnMission=" + hdnHasBeenOnMission
                + "&AppointmentIsDelivered=" + hdnAppointmentIsDelivered
                + "&hdnSuitableForMobAppointment=" + hdnSuitableForMobAppointment
                + "&Readiness=" + hdnReadiness
                + "&ProfessionId=" + hdnProfessionId
                + "&SpecialityId=" + hdnSpecialityId
                + "&SortBy=" + hdnSortBy
                
                +"&Export=true";

    window.location = url;
}

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />
<asp:HiddenField ID="hfMilitaryDepartmentID" runat="server" />
<asp:HiddenField ID="hfMilitaryCommandID" runat="server" />
<asp:HiddenField ID="hfRequestCommandPositionID" runat="server" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на водените на военен отчет</span>
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
                   <asp:Label runat="server" ID="lblFirstAndSurName" CssClass="InputLabel">Име и презиме:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 350px;">
                   <asp:TextBox runat="server" ID="txtFirstAndSurName" CssClass="InputField" Width="195px"></asp:TextBox>
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                </td>
                <td style="vertical-align: top; text-align: right; width: 200px;">
                   <asp:Label runat="server" ID="lblIdentNumber" CssClass="InputLabel">ЕГН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtIdentNumber" CssClass="InputField" Width="100px"></asp:TextBox> 
                </td>
             </tr>
             <tr style="height: 25px;">
                 <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblFamilyName" CssClass="InputLabel">Фамилия:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 350px;">
                   <asp:TextBox runat="server" ID="txtFamilyName" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                <td style="vertical-align:middle; text-align: right; width: 200px;">
                    <asp:Label runat="server" ID="lblInitials" CssClass="InputLabel">Инициали:</asp:Label>
                </td>
                <td style="vertical-align: bottom; text-align: left;">
                    <asp:TextBox runat="server" ID="txtInitials" CssClass="InputField" Width="30px"></asp:TextBox>
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
                   <asp:Label runat="server" ID="lblMilitaryReportStatus" CssClass="InputLabel">Състояние по отчета:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryReportStatus" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryCommand" CssClass="InputLabel">Команда:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                    <asp:TextBox runat="server" ID="txtMilitaryCommand" CssClass="InputField" Width="100px"></asp:TextBox>                    
                </td>
             </tr>
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">На отчет в:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryDepartment" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblPosition" CssClass="InputLabel">Длъжност:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                    <asp:TextBox runat="server" ID="txtPosition" CssClass="InputField" Width="100px"></asp:TextBox>                    
                </td>
             </tr>
             
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAppointedMilRepSpecType" CssClass="InputLabel">Назначен на тип ВОС:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddAppointedMilRepSpecType" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddAppointedMilRepSpecType_Changed"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAppointedMilRepSpec" CssClass="InputLabel">Назначен на ВОС:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddAppointedMilRepSpec" CssClass="InputField" Width="200px"></asp:DropDownList>
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
             
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblPositionTitle" CssClass="InputLabel">Подходяща длъжност:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddPositionTitle" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>            
             </tr>
             <tr style="height: 25px;">
                <td style="vertical-align: bottom; text-align: right;">
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:CheckBox runat="server" ID="chkIsPrimaryPositionTitle" CssClass="InputField" Text="Основна длъжност" />
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
                   <asp:Label runat="server" ID="lblLanguage" CssClass="InputLabel">Чужд език:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddLanguage" CssClass="InputField" Width="200px"></asp:DropDownList>
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
                   <asp:Label runat="server" ID="lblCivilSpeciality" CssClass="InputLabel">Гражданска специалност:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddCivilSpeciality" CssClass="InputField" Width="200px"></asp:DropDownList>
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
                <td style="vertical-align: middle; text-align: right;">
                   <asp:Checkbox runat="server" ID="chkHasBeenOnMission" />
                </td>
                <td style="text-align: left; vertical-align: middle;">
                   <asp:Label runat="server" ID="lblHasBeenOnMission" CssClass="InputLabel">Бил на мисия</asp:Label>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAppointmentIsDelivered" CssClass="InputLabel">Връчено МН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddAppointmentIsDelivered" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             
             <tr style="height: 25px;">    
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblReadiness" CssClass="InputLabel">Вид резерв:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddReadiness" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="suitableForMobAppointmentLabel" CssClass="InputLabel">Подходящ за МН:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="suitableForMobAppointmentDropDownList" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 25px;">
                <td style="vertical-align: top; text-align: right;">
                   <asp:Label runat="server" ID="lblProfession" CssClass="InputLabel">Професия:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:DropDownList runat="server" ID="ddProfession" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddProfession_Changed"></asp:DropDownList>
                </td>
                <td style="vertical-align: middle; text-align: right;">
                  <asp:Label runat="server" ID="lblSpeciality" CssClass="InputLabel">Специалност:</asp:Label>
                </td>                
                <td style="vertical-align: top; text-align: left;" >                   
                   <asp:DropDownList runat="server" ID="ddSpeciality" CssClass="InputField" Width="200px"></asp:DropDownList>
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
                                <asp:ListItem Value="1" Text="Настоящ адрес" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="2" Text="Постоянен адрес"></asp:ListItem>                        
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
                   
             <tr>
                <td colspan="4" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr style="height: 25px;">
                <td colspan="4" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                    <div style="padding-left: 30px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnPrintAllReservists" runat="server" CssClass="Button" OnClientClick="ShowPrintAllReservists(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnExportAllReservists" runat="server" CssClass="Button" OnClientClick="ExportAllReservists(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                </td>
             </tr>             
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;" runat="server" id="pnlPaging" visible="false">
    <div style="width: 670px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:80px; padding-left:5px;">Нов запис</div><b></b></asp:LinkButton>
       
       <span style="padding: 10px">&nbsp;</span>
    
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

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<asp:HiddenField ID="hdnFirstAndSurName" runat="server" />
<asp:HiddenField ID="hdnFamilyName" runat="server" />
<asp:HiddenField ID="hdnInitials" runat="server" />
<asp:HiddenField ID="hdnIdentNumber" runat="server" />
<asp:HiddenField ID="hdnMilitaryCategoryId" runat="server" />
<asp:HiddenField ID="hdnMilitaryRankId" runat="server" />
<asp:HiddenField ID="hdnMilitaryReportStatusId" runat="server" />
<asp:HiddenField ID="hdnMilitaryCommand" runat="server" />
<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnPosition" runat="server" />
<asp:HiddenField ID="hdnMilAppointedRepSpecTypeId" runat="server" />
<asp:HiddenField ID="hdnMilAppointedRepSpecId" runat="server" />
<asp:HiddenField ID="hdnMilRepSpecTypeId" runat="server" />
<asp:HiddenField ID="hdnMilRepSpecId" runat="server" />
<asp:HiddenField ID="hdnPositionTitleId" runat="server" />
<asp:HiddenField ID="hdnIsPrimaryPositionTitle" runat="server" />
<asp:HiddenField ID="hdnAdministrationId" runat="server" />
<asp:HiddenField ID="hdnLanguageId" runat="server" />
<asp:HiddenField ID="hdnEducationId" runat="server" />
<asp:HiddenField ID="hdnCivilSpecialityId" runat="server" />
<asp:HiddenField ID="hdnIsPermAddress" runat="server" />
<asp:HiddenField ID="hdnPostCode" runat="server" />
<asp:HiddenField ID="hdnRegionId" runat="server" />
<asp:HiddenField ID="hdnMunicipalityId" runat="server" />
<asp:HiddenField ID="hdnCityId" runat="server" />
<asp:HiddenField ID="hdnDistrictId" runat="server" />
<asp:HiddenField ID="hdnAddress" runat="server" />
<asp:HiddenField ID="hdnWorkCompany_UnifiedIdentityCode" runat="server" />
<asp:HiddenField ID="hdnWorkCompany_Name" runat="server" />
<asp:HiddenField ID="hdnHasBeenOnMission" runat="server" />
<asp:HiddenField ID="hdnAppointmentIsDelivered" runat="server" />
<asp:HiddenField ID="hdnSuitableForMobAppointment" runat="server" />
<asp:HiddenField ID="hdnReadiness" runat="server" />
<asp:HiddenField ID="hdnProfessionId" runat="server" />
<asp:HiddenField ID="hdnSpecialityId" runat="server" />
<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
