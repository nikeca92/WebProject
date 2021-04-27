<%@ Page Title="Сведение за класираните кандидати" Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="ReportRatedApplicantsSummary.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ReportRatedApplicantsSummary" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
	width: 1170px;
}

.ShadowContainer
{
    margin: 0 auto;
	width: 1170px;
}

#SubShadowContainer
{
	margin: 0 auto;
	width: 1170px;
    min-width: 1170px;
}

</style>

 <script src="../Scripts/Ajax.js" type="text/javascript"></script>
 
 <script type="text/javascript" language="javascript">
     function ShowPrintReportRatedApplicantsSummary()
        {
            var hfVacancyAnnounceId = document.getElementById("<%= hfVacancyAnnounceId.ClientID %>").value;
            var hfMilitaryUnitId = document.getElementById("<%= hfMilitaryUnitId.ClientID %>").value;
            var hfPosition = document.getElementById("<%= hfPosition.ClientID %>").value;
            var hfMilitaryDepartmentId = document.getElementById("<%= hfMilitaryDepartmentId.ClientID %>").value;
            var hfStatus = document.getElementById("<%= hfStatus.ClientID %>").value;

            var url = "";
            var pageName = "PrintReportRatedApplicantsSummary"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?VacancyAnnounceID=" + hfVacancyAnnounceId
                  + "&MilitaryUnitID=" + hfMilitaryUnitId
                  + "&Position=" + hfPosition
                  + "&MilitaryDepartmentID=" + hfMilitaryDepartmentId
                  + "&Status=" + hfStatus;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }
 
 </script>
 
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <center>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Сведение за класираните кандидати</span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 750px; margin: 0 auto;">
                         <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <tr style="height: 10px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 85px;">
                                        <asp:Label runat="server" ID="lblVacancyAnnounce" CssClass="InputLabel">Заповед №:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlVacancyAnnounces" CssClass="InputField" Width="210px" AutoPostBack="true" OnSelectedIndexChanged="ddlVacancyAnnounces_Changed" >
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 140px;">
                                        <asp:Label runat="server" ID="lblMilitaryUnit" CssClass="InputLabel"><%= MilitaryUnitLabel %>:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlMilitaryUnit" CssClass="InputField" Width="240px" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right;">
                                        <asp:Label runat="server" ID="lblPosition" CssClass="InputLabel">Длъжност:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlPosition" CssClass="InputField" Width="210px" >
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right;">
                                        <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Място на регистрация:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlMilitaryDepartments" Width="240px" CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right;">
                                        <asp:Label runat="server" ID="lblStatus" CssClass="InputLabel">Състояние:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="InputField" Width="210px" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top: 15px;">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="width: 100%;">
                                        <center>
                                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                            <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                            <div style="padding-left: 30px; display: inline">
                                            </div>
                                            <asp:LinkButton ID="btnPrintReportRatedApplicantsSummary" runat="server" CssClass="Button" OnClientClick="ShowPrintReportRatedApplicantsSummary(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 30px;">
                </div>
                <div runat="server" id="divPagingtems" style="width: 760px; margin: 0 auto; height: 35px; padding-left: 20px">
                    <span style="padding-left: 30px"></span>
                    <div style="display: inline; position: relative; top: -16px;">
                        <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png"
                            AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" />
                        <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png"
                            AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" />
                        <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
                        <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png"
                            AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" />
                        <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png"
                            AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" />
                        <span style="padding-left: 90px">&nbsp;</span> <span style="text-align: right;">Отиди
                            на страница</span>
                        <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
                        <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png"
                            AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" />
                    </div>
                </div>
                <div style="text-align: center;">
                    <div runat="server" id="pnlReportsGrid" style="text-align: center;">
                    </div>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </div>
                <div style="height: 20px;">
                </div>
            </center>
            
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hfVacancyAnnounceId" runat="server" />
            <asp:HiddenField ID="hfMilitaryUnitId" runat="server" />
            <asp:HiddenField ID="hfPosition" runat="server" />
            <asp:HiddenField ID="hfMilitaryDepartmentId" runat="server" />
            <asp:HiddenField ID="hfStatus" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
            
        </ContentTemplate>
    </asp:UpdatePanel>
 
</asp:Content>
