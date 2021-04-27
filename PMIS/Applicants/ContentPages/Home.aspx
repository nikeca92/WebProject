<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="PMIS.Applicants.ContentPages.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <div style="margin: 0 auto; width: 800px">
                    <div class="HomePageHeader" runat="server" id="divVacancyAnnounces">
                        Конкурси
                    </div>
                    <div class="HomePageItem" runat="server" id="divManageVacancyAnnounces">
                        <span onclick="HomePageItemClick('ManageVacancyAnnounces.aspx');" class="HomePageItemLink">
                            Списък на обявените конкурси</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divAddEditVacancyAnnounce">
                        <span onclick="HomePageItemClick('AddEditVacancyAnnounce.aspx?fh=1');" class="HomePageItemLink">
                            Нов обявен конкурс</span>
                    </div>
                    <div class="HomePageHeader" runat="server" id="divApplicants">
                        Кандидати
                    </div>
                    <div class="HomePageItem" runat="server" id="divManageApplicants">
                        <span runat="server" id="lnkApplicants_Search" onclick="HomePageItemClick('ManageApplicants.aspx');"
                            class="HomePageItemLink">Списък на кандидатите по обявен конкурс</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divAddEditApplicant">
                        <span onclick="HomePageItemClick('AddApplicant_SelectPerson.aspx?fh=1');" class="HomePageItemLink">
                            Нов кандидат</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divApplicantsAllowance">
                        <span onclick="HomePageItemClick('ApplicantsAllowance.aspx?fh=1');" class="HomePageItemLink">
                            Допускане на кандидати</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divApplicantsExams">
                        <span onclick="HomePageItemClick('ApplicantsExams.aspx?fh=1');" class="HomePageItemLink">
                            Изпити на кандидати</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divApplicantsRanking">
                        <span onclick="HomePageItemClick('ApplicantsRanking.aspx?fh=1');" class="HomePageItemLink">
                            Класиране на кандидати</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divApplicantsNomination">
                        <span onclick="HomePageItemClick('ApplicantsNomination.aspx?fh=1');" class="HomePageItemLink">
                            Назначаване на кандидати</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divManagePotencialApplicants">
                        <span runat="server" id="Span1" onclick="HomePageItemClick('ManagePotencialApplicants.aspx?fh=1');"
                            class="HomePageItemLink">Списък на потенциалните кандидати</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divAddEditPotencialApplicant">
                        <span onclick="HomePageItemClick('AddPotencialApplicant_SelectPerson.aspx?fh=1');"
                            class="HomePageItemLink">Нов потенциален кандидат</span>
                    </div>
                    
                    
                    <div class="HomePageHeader" runat="server" id="divCadets">
                        Курсанти
                    </div>
                    <div class="HomePageItem" runat="server" id="divManageCadets">
                        <span onclick="HomePageItemClick('ManageCadets.aspx');" class="HomePageItemLink">Списък
                            на курсантите</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divAddEditCadet">
                        <span onclick="HomePageItemClick('AddCadet_SelectPerson.aspx?fh=1');" class="HomePageItemLink">
                            Нов курсант</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divCadetsRanking">
                        <span onclick="HomePageItemClick('CadetsRanking.aspx?fh=1');" class="HomePageItemLink">
                            Класиране на курсанти</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divManageMilitarySchoolSpecializations">
                        <span id="Span2" runat="server" onclick="HomePageItemClick('ManageMilitarySchoolSpecializations.aspx');"
                            class="HomePageItemLink">Специалности и специализации за военните училища</span>
                    </div>
                    
                    
                    
                    <div class="HomePageHeader" runat="server" id="divReports">
                        Справки
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportDetailed">
                        <span onclick="HomePageItemClick('ReportVacAnnApplDetailed.aspx?fh=1');" class="HomePageItemLink">
                            Детайлна справка за кандидатите по обявен конкурс</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divListParticipate">
                        <span onclick="HomePageItemClick('ReportVacAnnApplListParticipate.aspx?fh=1');" class="HomePageItemLink">
                            Списък на кандидатите участвали в конкурс</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divListRanking">
                        <span onclick="HomePageItemClick('ReportVacAnnApplListRanking.aspx?fh=1');" class="HomePageItemLink">
                            Списък на кандидатите класирани в конкурс</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divListNominated">
                        <span onclick="HomePageItemClick('ReportVacAnnApplListNominated.aspx?fh=1');" class="HomePageItemLink">
                            Списък на кандидатите определени за назначаване</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportCadet">
                        <span onclick="HomePageItemClick('ReportsCadet.aspx?fh=1');" class="HomePageItemLink">
                            Справка кандидат - курсанти</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportRatedApplicantsSummary">
                        <span onclick="HomePageItemClick('ReportRatedApplicantsSummary.aspx?fh=1');" class="HomePageItemLink">
                            Сведение за класираните кандидати</span>
                    </div>
                    
                    <div class="HomePageItem" runat="server" id="divReportVacancyAnnounceApplicants">
                        <span onclick="HomePageItemClick('ReportVacancyAnnounceApplicants.aspx?fh=1');"
                              class="HomePageItemLink">
                            Кандидати за военна служба по обявен конкурс
                        </span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportDocumentsApplied">
                        <span onclick="HomePageItemClick('ReportDocumentsApplied.aspx?fh=1');"
                              class="HomePageItemLink">
                            Сведение за подалите документи за военна служба
                        </span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportDocumentsSent">
                        <span onclick="HomePageItemClick('ReportDocumentsSent.aspx?fh=1');"
                              class="HomePageItemLink">
                            Сведение за изпратените документи за военна служба
                        </span>
                    </div>
                    
                    <div class="HomePageHeader" runat="server" id="divListMaint">
                        Класификатори
                    </div>
                    <div class="HomePageItem" runat="server" id="divDocuments">
                        <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=APPL_Documents');" class="HomePageItemLink">Документи</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divExams">
                        <span onclick="HomePageItemClick('Maintenance.aspx?MaintKey=APPL_Exams');" class="HomePageItemLink">Изпити</span>
                    </div>
                </div>
            </div>
            <div style="height: 20px;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
