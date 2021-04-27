<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PMIS.Reserve.ContentPages.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <div style="margin: 0 auto; width: 800px">
      <div class="HomePageHeader" runat="server" id="divHumanResources">
         Човешки ресурси
      </div>
      <div class="HomePageItem" runat="server" id="divAddNewReservist">
         <span onclick="HomePageItemClick('AddEditReservist.aspx?fh=1');" class="HomePageItemLink">Създаване на нов запис</span>
      </div>
      <div class="HomePageItem" runat="server" id="divManageReservists">
         <span onclick="HomePageItemClick('ManageReservists.aspx?fh=1');" class="HomePageItemLink">Списък на водените на отчет</span>
      </div>
    
      <div class="HomePageItem" runat="server" id="divImportPersonsData">
         <span onclick="HomePageItemClick('ImportPersonsData.aspx?fh=1');" class="HomePageItemLink">Импорт на данни</span>
      </div>
      <div class="HomePageItem" runat="server" id="divGroupTakingDown">
         <span onclick="HomePageItemClick('GroupTakingDown.aspx?fh=1');" class="HomePageItemLink">Групово снемане от отчет</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divTechnics">
         Техника
      </div>
      <div runat="server" id="pnlTechnicsLinks">
      </div>      
      <div class="HomePageHeader" runat="server" id="divEquipment">
         Комплектоване
      </div>
      <div class="HomePageItem" runat="server" id="divAddEditEquipmentReservistsRequest">
         <span onclick="HomePageItemClick('AddEditEquipmentReservistsRequest.aspx?fh=1');" class="HomePageItemLink">Заявка за комплектоване с резервисти</span>
      </div>
      <div class="HomePageItem" runat="server" id="divManageEquipmentReservistsRequests">
         <span onclick="HomePageItemClick('ManageEquipmentReservistsRequests.aspx?fh=1');" class="HomePageItemLink">Въведени заявки за комплектоване с резервисти</span>
      </div>
      <div class="HomePageItem" runat="server" id="divManageEquipmentReservistsRequestsFulfilment">
         <span onclick="HomePageItemClick('ManageEquipmentReservistsRequestsFulfilment.aspx?fh=1');" class="HomePageItemLink">Изпълнение на заявки за комплектоване с резервисти</span>
      </div>
      <div class="HomePageItem" runat="server" id="divAddEditEquipmentTechnicsRequest">
         <span onclick="HomePageItemClick('AddEditEquipmentTechnicsRequest.aspx?fh=1');" class="HomePageItemLink">Заявка за комплектоване с техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divManageEquipmentTechnicsRequests">
         <span onclick="HomePageItemClick('ManageEquipmentTechnicsRequests.aspx?fh=1');" class="HomePageItemLink">Въведени заявки за комплектоване с техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divManageEquipmentTechnicsRequestsFulfilment">
         <span onclick="HomePageItemClick('ManageEquipmentTechnicsRequestsFulfilment.aspx?fh=1');" class="HomePageItemLink">Изпълнение на заявки за комплектоване с техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divFulfilReservistsMilCommand">
         <span onclick="HomePageItemClick('FulfilReservistsMilCommand.aspx?fh=1');" class="HomePageItemLink">Комплектоване на команда с резервисти</span>
      </div>
      <div class="HomePageItem" runat="server" id="divFulfilTechnicsMilCommand">
         <span onclick="HomePageItemClick('FulfilTechnicsMilCommand.aspx?fh=1');" class="HomePageItemLink">Комплектоване на команда с техника</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divPostpone">
         Отсрочване
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeRes">
         <span onclick="HomePageItemClick('PostponeRes.aspx?fh=1');" class="HomePageItemLink">Заявки за отсрочване на запасни</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeTech">
         <span onclick="HomePageItemClick('PostponeTech.aspx?fh=1');" class="HomePageItemLink">Заявки за отсрочване на техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeReportRes">
         <span onclick="HomePageItemClick('ReportPostponeRes.aspx?fh=1');" class="HomePageItemLink">Протокол за изпълнение отсрочването на запасни</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeReportTech">
         <span onclick="HomePageItemClick('ReportPostponeTech.aspx?fh=1');" class="HomePageItemLink">Протокол за изпълнение отсрочването на техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeReportResByAdministration">
         <span onclick="HomePageItemClick('ReportPostponeResByAdministration.aspx?fh=1');" class="HomePageItemLink">Отчет за изпълнение отсрочването на запасни</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPostponeReportTechByAdministration">
         <span onclick="HomePageItemClick('ReportPostponeTechByAdministration.aspx?fh=1');" class="HomePageItemLink">Отчет за изпълнение отсрочването на техника</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divReports">
         Справки
      </div>
      <div class="HomePageItem" runat="server" id="divReportA33v2">
         <span onclick="HomePageItemClick('ReportA33v2.aspx?fh=1');" class="HomePageItemLink">Сведение-анализ за състоянието на ресурсите от резерва</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportA31">
         <span onclick="HomePageItemClick('ReportA31.aspx?fh=1');" class="HomePageItemLink">Сведение за планираните за доставяне запасни и техника – запас</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportSV1">
         <span onclick="HomePageItemClick('ReportSV1.aspx?fh=1');" class="HomePageItemLink">Отчетна ведомост за състоянието на ресурсите</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportNormativeTechnics">
         <span onclick="HomePageItemClick('ReportNormativeTechnics.aspx?fh=1');" class="HomePageItemLink">Отчетна ведомост за състоянието на техниката</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportListReservistsFromCommand">
         <span onclick="HomePageItemClick('ReportListReservistsFromCommand.aspx?fh=1');" class="HomePageItemLink">Списък на хората с МН от команда</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportAnalyzeCommand">
         <span onclick="HomePageItemClick('ReportAnalyzeCommand.aspx?fh=1');" class="HomePageItemLink">Сведение-анализ за комплектуването на команда</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportAnalyzeResFulfilment">
         <span onclick="HomePageItemClick('ReportAnalyzeResFulfilment.aspx?fh=1');" class="HomePageItemLink">Анализ на комплектуването</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportListTechnicsFromCommand">
         <span onclick="HomePageItemClick('ReportListTechnicsFromCommand.aspx?fh=1');" class="HomePageItemLink">Списък на техниката с МН от команда</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportAnalyzeTechCommand">
         <span onclick="HomePageItemClick('ReportAnalyzeTechCommand.aspx?fh=1');" class="HomePageItemLink">Сведение-анализ за комплектуването на команда с техника-запас</span>
      </div>
      <div runat="server" id="divTechnicsTypesReportLinks"></div>      
      <div class="HomePageItem" runat="server" id="divReportListReservistsWithAppointments">
         <span onclick="HomePageItemClick('ReportListReservistsWithAppointments.aspx?fh=1');" class="HomePageItemLink">Списък на хората с МН по определена заявка</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportListTechnicsWithAppointments">
         <span onclick="HomePageItemClick('ReportListTechnicsWithAppointments.aspx?fh=1');" class="HomePageItemLink">Списък на техниката с МН по определена заявка</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportA33">
         <span onclick="HomePageItemClick('ReportA33.aspx?fh=1');" class="HomePageItemLink">Справка A33</span>
      </div>
      <div class="HomePageItem" runat="server" id="divReportStaffPositionsList">
         <span onclick="HomePageItemClick('ReportStaffPositionsList.aspx?fh=1');" class="HomePageItemLink">Щатно-длъжностен списък</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divPrint">
         Печат
      </div>
      <div class="HomePageItem" runat="server" id="divPrintReservists">
         <span onclick="HomePageItemClick('PrintReservists.aspx?fh=1');" class="HomePageItemLink">Документи за хора</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPrintTechnics">
         <span onclick="HomePageItemClick('PrintTechnics.aspx?fh=1');" class="HomePageItemLink">Документи за техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divPrintPostponeReservists">
         <span onclick="HomePageItemClick('PrintPostponeReservists.aspx?fh=1');" class="HomePageItemLink">Отсрочки на хора</span>
      </div>
      <div class="HomePageItem" runat="server" id="div1">
         <span onclick="HomePageItemClick('PrintPostponeTechnics.aspx?fh=1');" class="HomePageItemLink">Отсрочки на техника</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divListMaint">
          Класификатори
      </div>
      <div class="HomePageItem" runat="server" id="divMilitaryReadinessNames">
          <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=RES_MilitaryReadinessNames');" class="HomePageItemLink">Готовност</span>
      </div>
      <div class="HomePageItem" runat="server" id="divTechnicsCategories">
          <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=RES_TechnicsCategoryNames');" class="HomePageItemLink">Категории техника</span>
      </div>
      <div class="HomePageItem" runat="server" id="divCompanies">
          <span onclick="HomePageItemClick('ManageCompanies.aspx');" class="HomePageItemLink">Фирми</span>
      </div>
      <div class="HomePageItem" runat="server" id="divTechParams">
          <span onclick="HomePageItemClick('TechParams.aspx');" class="HomePageItemLink">Параметри на техниката</span>
      </div>
     
   </div>
</div>

<div style="height: 20px;">
</div>

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
