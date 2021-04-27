<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <div style="margin: 0 auto; width: 800px">
      <div class="HomePageHeader" runat="server" id="divProtocols">
         Протоколи
      </div>
      <div class="HomePageItem" runat="server" id="divManageProtocols">
         <span runat="server" id="lnkProtocols_Search" onclick="HomePageItemClick('ManageProtocols.aspx');" class="HomePageItemLink">Търсене на протоколи от извършени измервания</span>
      </div>
      <div class="HomePageItem" runat="server" id="divAddProtocol">
         <span onclick="HomePageItemClick('AddEditProtocol.aspx?fh=1');" class="HomePageItemLink">Нов протокол от извършени измервания</span>
      </div>
      
      <div class="HomePageHeader" runat="server" id="divWCondCards">
         Карти
      </div>
      <div class="HomePageItem" runat="server" id="divManageWCondCards">
         <span onclick="HomePageItemClick('ManageWorkplaceConditionsCards.aspx?fh=1');" class="HomePageItemLink">Търсене на карти за комплексно оценяване на специфичните условия на труд и рискове за живота и здравето</span>
      </div>
      <div class="HomePageItem" runat="server" id="divAddWCondCard">
         <span onclick="HomePageItemClick('AddEditWorkplaceConditionsCard.aspx?fh=1');" class="HomePageItemLink">Нова карта за комплексно оценяване на специфичните условия на труд и рискове за живота и здравето</span>
      </div>
      
      <div id="divRiskAssessments" class="HomePageHeader" runat="server">
         Оценки на риска
      </div>
      <div id="divMilitaryUnitPositions" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('MilitaryUnitPositions.aspx');" class="HomePageItemLink">Класификация на дейностите</span>
      </div>
      <div id="divRiskCard" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('RiskCard.aspx');" class="HomePageItemLink">Карта за оценка на риска</span>
      </div>
      <div id="divManageRiskAssessments" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('ManageRiskAssessments.aspx');" class="HomePageItemLink">Търсене на оценки на риска</span>
      </div>
      <div id="divAddRiskAssessment" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('AddEditRiskAssessment.aspx?fh=1');" class="HomePageItemLink">Новa оценка на риска</span>
      </div>
      
      <div id="divCommittees" class="HomePageHeader" runat="server">
         Комитети и групи
      </div>
      <div id="divManageCommittees" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('ManageCommittees.aspx?fh=1');" class="HomePageItemLink">Търсене на комитети и групи по условията на труд</span>
      </div>
      <div id="divAddCommittee" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('AddEditCommittee.aspx?fh=1');" class="HomePageItemLink">Нов комитет или група по условията на труд</span>
      </div>
      <div id="divManageTrainingHistory" class="HomePageItem" runat="server">
         <span onclick="HomePageItemClick('ManageTrainingHistory.aspx?fh=1');" class="HomePageItemLink">История на обученията</span>
      </div>
      
      <div class="HomePageHeader" runat="server" id="divWorkAccidents">
         Трудови злополуки
      </div>
      <div class="HomePageItem" id="divManageDeclarationOfAccident" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('ManageDeclarationOfAccident.aspx');">Търсене на декларации за трудови злополуки</span>
      </div>
      <div class="HomePageItem" id="divAddDeclarationOfAccident" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('AddEditDeclarationOfAccident.aspx?fh=1');">Нова декларация за трудова злополука</span>
      </div>
      <div class="HomePageItem" id="divManageInvestigationProtocol" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('ManageInvestigationProtocol.aspx');" >Търсене на протоколи за резултатите от разследване на злополука</span>
      </div>
      <div class="HomePageItem" id="divAddInvestigationProtocol" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('AddEditInvestigationProtocol.aspx?fh=1');">Нов протокол за резултатите от разследване на злополука</span>
      </div>
      <div id="divManageUnsafeWCondNotices" class="HomePageItem" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('ManageUnsafeWorkingConditionsNotices.aspx');">Търсене на сведения за заболявания и наранявания свързани с работата</span>
      </div>
      <div id="divAddUnsafeWCondNotices" class="HomePageItem" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('AddEditUnsafeWorkingConditionsNotice.aspx?fh=1');">Ново сведение за заболявания и наранявания свързани с работата</span>
      </div>
      
      <div class="HomePageHeader" id="divLists" runat="server">
         Класификатори
      </div>
      <div class="HomePageItem" id="divListsProtocolTypes" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_ProtocolType');">Измервания</span>
      </div>
      <div class="HomePageItem" id="divListsMeasures" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_Measures');">Измервани величини</span>
      </div>
      <!-- This would be edited via the new GTable lightbox maintenance directly from the screen
      <div class="HomePageItem">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_DegreeOfDanger');" >Степени на опасност</span>
      </div>
      -->
      <div id="divIndicatorTypeMaintenance" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('IndicatorTypesMaintenance.aspx?fh=1');" >Елементи на специфичните условия на труд</span>
      </div>
      <div id="divWorkingPlacesMaintenance" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('ManageWorkingPlaces.aspx?fh=1');" >Място на измерване</span>
      </div>
      <div id="divListsProbabilities" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_Probabilities');" >Вероятност</span>
      </div>
      <div id="divListsExposure" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_Exposure');" >Експозиция</span>
      </div>
      <div id="divListsEffectWeight" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_EffectWeight');" >Тежест (последици)</span>
      </div>
      <div id="divListsRiskRank" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('Maintenance.aspx?MaintKey=HS_RiskRank');" >Риск</span>
      </div>
      <div id="divRiskFactorTypeMaintenance" class="HomePageItem" runat="server">
         <span class="HomePageItemLink"  onclick="HomePageItemClick('RiskFactorTypesMaintenance.aspx?fh=1');" >Потенциални опасности</span>
      </div>
   </div>
</div>

<div style="height: 20px;">
</div>

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
