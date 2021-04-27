<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <div style="margin: 0 auto; width: 800px">
      <div class="HomePageHeader" runat="server" id="divAudtTrail">
         Одит
      </div>
      <div class="HomePageItem" runat="server" id="contAudit_Search">
         <span runat="server" id="lnkAuditTrail_Search" onclick="HomePageItemClick('AuditTrail.aspx');" class="HomePageItemLink">Одитни записи</span>
      </div>
      <div class="HomePageItem" runat="server" id="contAutit_LoginLog">
         <span runat="server" id="lnkAuditTrail_LoginLog" onclick="HomePageItemClick('LoginLog.aspx');" class="HomePageItemLink">Потребителски сесии</span>
      </div>
      <div class="HomePageItem" runat="server" id="contAutit_FailedLogins">
         <span runat="server" id="lnkAuditTrail_FailedLogins" onclick="HomePageItemClick('FailedLogins.aspx');" class="HomePageItemLink">Неуспешни опити за достъп</span>
      </div>
      <div class="HomePageHeader" runat="server" id="divSecurity">
         Потребители и сигурност
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_ManageRoles">
         <span runat="server" id="lnkUsers_ManageRoles" onclick="HomePageItemClick('ManageRoles.aspx');" class="HomePageItemLink">Управление на потребителските роли</span>
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_AddRole">
         <span runat="server" id="lnkUsers_AddRole" onclick="HomePageItemClick('AddEditRole.aspx?fh=1');" class="HomePageItemLink">Нова потребителска роля</span>
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_UIItemsPerRole">
         <span runat="server" id="lnkUsers_UIItemsPerRole" onclick="HomePageItemClick('UIItemsPerRole.aspx?fh=1');" class="HomePageItemLink">Дефиниране на права и достъп според потребителската роля</span>
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_ManageUsers">
         <span runat="server" id="lnkUsers_ManageUsers" onclick="HomePageItemClick('ManageUsers.aspx');" class="HomePageItemLink">Управление на потребителите</span>
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_AddUser">
         <span runat="server" id="lnkUsers_AddUser" onclick="HomePageItemClick('AddEditUser.aspx?fh=1');" class="HomePageItemLink">Нов потребител</span>
      </div>
      <div class="HomePageItem" runat="server" id="contUsers_PasswordPolicy">
         <span runat="server" id="lnkUsers_PasswordPolicy" onclick="HomePageItemClick('PasswordPolicy.aspx?fh=1');" class="HomePageItemLink">Пароли</span>
      </div>
      <div class="HomePageHeader" id="divLists" runat="server">
         Класификатори
      </div>
      <div class="HomePageItem" id="divListsMilitaryForceTypes" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_MilitaryForceType');">Видове войски</span>
      </div>
      <div class="HomePageItem" id="divListsMilitaryDepartments" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_MilitaryDepartment');">Военни окръжия</span>
      </div>
      <div class="HomePageItem" id="divListsAdministrations" runat="server">
         <span class="HomePageItemLink" onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_Administrations');">Ведомства</span>
      </div>
      <div class="HomePageItem" runat="server" id="divDrivLicenseCat">
         <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_DrivingLicenseCategories');" class="HomePageItemLink">Шоф. книжка - категории</span>
      </div>
      <div class="HomePageItem" runat="server" id="divMilRepSpecialities">
         <span onclick="HomePageItemClick('ManageMilRepSpecialities.aspx');" class="HomePageItemLink">ВОС</span>
      </div>
      <div class="HomePageItem" runat="server" id="divListsPositionTitles">
         <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_PositionTitles');" class="HomePageItemLink">Длъжности</span>
      </div>
      <div class="HomePageItem" runat="server" id="divMedRibrics">
         <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_MedRubrics');" class="HomePageItemLink">Медицински рубрики</span>
      </div>
      <div class="HomePageItem" runat="server" id="divListsMilitaryStructures">
         <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_MilitaryStructures');" class="HomePageItemLink">Структури</span>
      </div>
      <div class="HomePageItem" runat="server" id="divMilitaryForceSorts">
         <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=ADM_MilitaryForceSorts');" class="HomePageItemLink">Родове войски</span>
      </div>
   </div>
</div>

<div style="height: 20px;">
</div>

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
