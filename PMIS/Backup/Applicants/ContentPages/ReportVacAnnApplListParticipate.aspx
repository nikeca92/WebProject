<%@ Page Title="Списък на кандидатите участвали в конкурс" Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="ReportVacAnnApplListParticipate.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ReportVacAnnApplListParticipate" %>

<%--<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        var selectedVacancyAnnounceId = "-1";
        // var selectedResponseMilitaryId = "-1";

        function SortTableBy(sort)
        {
            if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
            {
                sort = sort + 100;
            }

            document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
            document.getElementById("<%= btnRefresh.ClientID %>").click();
        }

        function ddlChange(ddlObject)
        {
            var cheked = false;

            //use functon in common.js
            ClearSelectList(document.getElementById("ddMilitaryUnitsForVacAnn"));

            var i = 0;
            do
            {
                if (ddlObject[i].selected)
                {
                    cheked = true;
                    selectedVacancyAnnounceId = ddlObject[i].value
                }
                i++;
            } while (!cheked)

            if (selectedVacancyAnnounceId != "-1")
            {
                document.getElementById("<%=hfVacancyAnnounceId.ClientID %>").value = selectedVacancyAnnounceId;

                var url = "ReportVacAnnApplListParticipate.aspx?AjaxMethod=JSGetListMilitaryUnitsForVacAnn";
                var params = "";
                params += "&vacancyAnnounceId=" + selectedVacancyAnnounceId;
                
                function response_handlerDLL(xml)
                {
                    var Html = xmlNodeText(xml);
                    document.getElementById("<%=divMilitaryUnitsForVacAnn.ClientID %>").innerHTML = Html;

                    //Set new Id on hfResponsibleMilitaryUnitId
                    var ddlObject = document.getElementById("ddMilitaryUnitsForVacAnn");
                    ddMilitaryUnitsForVacAnnChange(ddlObject)
                }

                var myAJAX = new AJAX(url, true, params, response_handlerDLL);
                myAJAX.Call();
            }
            else
            {
                selectedResponseMilitaryId = "-1";
            }
        }
        function ddMilitaryUnitsForVacAnnChange(ddlObject)
        {
            var cheked = false

            var i = 0;
            do
            {
                if (ddlObject[i].selected)
                {
                    cheked = true;
                    selectedResponseMilitaryId = ddlObject[i].value
                }
                i++;
            } while (!cheked)

            //Set New Id
            document.getElementById("<%=hfResponsibleMilitaryUnitId.ClientID %>").value = selectedResponseMilitaryId;
        }

        function ValidateFilterData()
        {
            if (selectedVacancyAnnounceId == "-1")
            {
                if (document.getElementById("<%=divPagingItems.ClientID %>") != undefined)
                {
                    document.getElementById("<%=divPagingItems.ClientID %>").innerHTML = "<span>Няма намерени резултати</span>";
                    document.getElementById("<%=pnlReportsGrid.ClientID %>").style.visibility = "hidden";
                }
                else
                {
                    document.getElementById("<%=pnlReportsGrid.ClientID %>").innerHTML = "<span>Няма намерени резултати</span>";
                }
                
                if (document.getElementById("<%=btnPrintReportVacAnnApplListParticipate.ClientID %>") != undefined)
                {
                    document.getElementById("<%=btnPrintReportVacAnnApplListParticipate.ClientID %>").style.visibility = "hidden";

                }

                return false;
            }
            else
            {
                return true;
            }
        }

        function ShowPrintReportVacAnnApplListParticipate()
        {
            var hfVacancyAnnounceId = document.getElementById("<%= hfVacancyAnnounceId.ClientID %>").value;
            var hfApplicantStatusId = document.getElementById("<%= hfApplicantStatustId.ClientID %>").value;
            var hfResponsibleMilitaryUnitId = document.getElementById("<%= hfResponsibleMilitaryUnitId.ClientID %>").value;
            var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

            var url = "";
            var pageName = "PrintReportVacAnnApplListParticipate"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?VacancyAnnounceID=" + hfVacancyAnnounceId
                + "&ApplicantStatusID=" + hfApplicantStatusId + "&ResponsibleMilitaryUnitID=" + hfResponsibleMilitaryUnitId + "&SortBy=" + hdnSortBy;

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
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на кандидатите участвали
                        в конкурс</span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 850px; margin: 0 auto;">
                        <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <tr style="height: 10px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 170px;">
                                        <asp:Label runat="server" ID="lblVacancyAnnounce" CssClass="InputLabel">Конкурс обявен със заповед №:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlVacancyAnnounces" onchange="ddlChange(this)"
                                            CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 120px;">
                                        <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Отговорно поделение:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <%-- <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx"
                                            DataSourceKey="MilitaryUnit" DivMainCss="isDivMainClass" DivListCss="isDivListClass"
                                            DivFullListCss="isDivFullListClass" />--%>
                                        <div runat="server" id="divMilitaryUnitsForVacAnn">
                                        </div>
                                    </td>
                                </tr>
                                <tr style="height: 15px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 170px;">
                                        <asp:Label runat="server" ID="Label1" CssClass="InputLabel">Статус на кандидата:</asp:Label>
                                    </td>
                                    <td colspan="3" style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlApplStatus" Width="200px" CssClass="InputField">
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
                                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"
                                                OnClientClick="return ValidateFilterData()"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                            <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                            <div style="padding-left: 30px; display: inline">
                                            </div>
                                            <asp:LinkButton ID="btnPrintReportVacAnnApplListParticipate" runat="server" CssClass="Button"
                                                OnClientClick="ShowPrintReportVacAnnApplListParticipate(); return false;" Visible="false"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 20px;">
                </div>
                <div id="divPagingItems" runat="server" style="width: 760px; margin: 0 auto; height: 35px;
                    padding-left: 20px">
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
            <asp:HiddenField ID="hdnSortBy" runat="server" />
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hfVacancyAnnounceId" runat="server" />
            <asp:HiddenField ID="hfApplicantStatustId" runat="server" />
            <asp:HiddenField ID="hfResponsibleMilitaryUnitId" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
