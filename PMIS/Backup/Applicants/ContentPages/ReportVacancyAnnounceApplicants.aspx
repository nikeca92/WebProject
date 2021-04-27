<%@ Page Title="Кандидати за военна служба по обявен конкурс" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MasterPage.Master"
    CodeBehind="ReportVacancyAnnounceApplicants.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ReportVacancyAnnounceApplicants" %>

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
        var selectedVacancyAnnounceId = "-1";
        
        function ExcelExport(){
                var hfVacancyAnnounceId = document.getElementById("<%= ddVacancyAnnounces.ClientID %>").value;
                var hfResponsibleMilitaryUnitID = document.getElementById("<%= ddResponsibleMilitaryUnits.ClientID %>").value;
               
                var url = "";
                var pageName = "PrintReportVacancyAnnounceApplicants"
                var param = "";

                url = "../PrintContentPages/" + pageName + ".aspx?VacancyAnnounceID=" + hfVacancyAnnounceId
                      + "&ResponsibleMilitaryUnitID=" + hfResponsibleMilitaryUnitID;

                window.location = url;           
            }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <center>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Кандидати за военна служба по обявен конкурс</span>
                </div>
                <div style="height: 20px;"></div>
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
                                    <td style="vertical-align: top; text-align: right; width: 200px;">
                                        <asp:Label runat="server" ID="lblVacancyAnnounce" CssClass="InputLabel">Конкурс обявен със заповед №:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddVacancyAnnounces" OnSelectedIndexChanged="ddVacancyAnnounces_Changed" AutoPostBack="True" 
                                                          CssClass="InputField" Width="210px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 140px;">
                                        <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Отговорно поделение:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddResponsibleMilitaryUnits" CssClass="InputField" Width="210px">
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
                                            <asp:LinkButton ID="btnExcel" runat="server" CssClass="Button" OnClientClick="ExcelExport(); return false;"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                                            <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                            <div style="padding-left: 30px; display: inline"></div> 
                                        </center>
                                    </td>
                                </tr>
                            </table>
                         </div>
                    </div>
                </div>
                <div style="height: 30px;"></div>              
                <div style="text-align: center;">
                    <div runat="server" id="pnlReportsGrid" style="text-align: center;"></div>
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
            </center>
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hfVacancyAnnounceId" runat="server" />
            <asp:HiddenField ID="hfResponsibleMilitaryUnitId" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
