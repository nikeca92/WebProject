<%@ Page Title="Справка кандидат - курсанти" Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="ReportsCadet.aspx.cs" Inherits="PMIS.Applicants.ContentPages.VacAnnApplCadetReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        var selectedMilitarySchoolId = "-1";
        var selectedMilitarySchoolYearId = "-1";
        var selectedMilitarySubjectId = "-1";
        var selectedMilitarySpecializationId = "-1";
        var selectedStatusId = "-1";

        function SortTableBy(sort)
        {
            if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
            {
                sort = sort + 100;
            }

            document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
            document.getElementById("<%= btnRefresh.ClientID %>").click();
        }

        function ddlChange(ddlObject, objectId)
        {
            var selectedItem = GetSelectedItem(ddlObject);

            if (selectedItem == "-1")
            {
                //clear value for hfMilitarySpecializationId
                document.getElementById("<%=hfMilitarySubjectId.ClientID %>").value = "-1";
                document.getElementById("<%=hfMilitarySpecializationId.ClientID %>").value = "-1";
            }


            if (objectId == "ddlMilitarySchool")
            {
                document.getElementById("<%=hfMilitarySchoolId.ClientID %>").value = selectedItem;
            }
            else
            {
                document.getElementById("<%=hfMilitarySchoolYear.ClientID %>").value = selectedItem;
            }

            var url = "ReportsCadet.aspx?AjaxMethod=JSGetDataForSubjectAndSpeziality";
            var params = "";
            params += "&militarySchoolId=" + document.getElementById("<%=hfMilitarySchoolId.ClientID %>").value;
            params += "&militarySchoolYear=" + document.getElementById("<%=hfMilitarySchoolYear.ClientID %>").value;
            
            function response_handlerDLL(xml)
            {

                var HtmlSubjectDiv;
                var HtmlSpecializationDiv;
                var Html = xmlNodeText(xml);

                var index = Html.indexOf("<divSpezialitis>");

                HtmlSubjectDiv = Html.slice(0, index);
                HtmlSpecializationDiv = Html.slice(index);

                document.getElementById("<%=divSubject.ClientID %>").innerHTML = HtmlSubjectDiv;

                document.getElementById("<%=divSpecialization.ClientID %>").innerHTML = HtmlSpecializationDiv;
            }

            var myAJAX = new AJAX(url, true, params, response_handlerDLL);
            myAJAX.Call();
        }

        function ddMilitarySchoolSubjectsChange(ddlObject)
        {
            var selectedItem = GetSelectedItem(ddlObject);

            if (selectedItem == "-1")
            {
                //clear value for hfMilitarySpecializationId
                document.getElementById("<%=hfMilitarySpecializationId.ClientID %>").value = "-1";
            }

            document.getElementById("<%=hfMilitarySubjectId.ClientID %>").value = selectedItem;

            var url = "ReportsCadet.aspx?AjaxMethod=JSGetDataForSpezialion";
            var params = "";
            params += "&militarySchoolId=" + document.getElementById("<%=hfMilitarySchoolId.ClientID %>").value;
            params += "&militarySchoolYear=" + document.getElementById("<%=hfMilitarySchoolYear.ClientID %>").value;
            params += "&militarySubjectId=" + document.getElementById("<%=hfMilitarySubjectId.ClientID %>").value;
            
            function response_handlerDLL(xml)
            {
                document.getElementById("<%=divSpecialization.ClientID %>").innerHTML = xml.text;
            }

            var myAJAX = new AJAX(url, true, params, response_handlerDLL);
            myAJAX.Call();
        }

        function ddMilitarySchoolSpezialitisChange(ddlObject)
        {
            var selectedItem = GetSelectedItem(ddlObject);
            document.getElementById("<%=hfMilitarySpecializationId.ClientID %>").value = selectedItem;
        }

        function ddlStatusChange(ddlObject)
        {
            var selectedItem = GetSelectedItem(ddlObject);
            document.getElementById("<%=hfStatusId.ClientID %>").value = selectedItem;
        }

        function GetSelectedItem(ddlObject)
        {
            var selectedItem;
            var cheked = false;
            var i = 0;
            do
            {
                if (ddlObject[i].selected)
                {
                    cheked = true;
                    selectedItem = ddlObject[i].value
                }
                i++;
            } while (!cheked)

            return selectedItem;
        }
        
        function ShowPrintReportCadets()
        {
            var hfMilitarySchoolId = document.getElementById("<%= hfMilitarySchoolId.ClientID %>").value;
            var hfMilitarySchoolYear = document.getElementById("<%= hfMilitarySchoolYear.ClientID %>").value;
            var hfMilitarySubjectId = document.getElementById("<%= hfMilitarySubjectId.ClientID %>").value;
            var hfMilitarySpecializationId = document.getElementById("<%= hfMilitarySpecializationId.ClientID %>").value;
            var hfStatusId = document.getElementById("<%= hfStatusId.ClientID %>").value;
            var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

            var url = "";
            var pageName = "PrintReportCadets"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?MilitarySchoolID=" + hfMilitarySchoolId
                + "&MilitarySchoolYear=" + hfMilitarySchoolYear
                + "&MilitarySubjectID=" + hfMilitarySubjectId
                + "&MilitarySpecID=" + hfMilitarySpecializationId
                + "&StatusID=" + hfStatusId
                + "&SortBy=" + hdnSortBy;

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
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Справка кандидат - курсанти
                    </span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 850px; margin: 0 auto;">
                        <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <colgroup>
                                    <col width="130px" />
                                    <col width="300px" />
                                    <col width="100px" />
                                    <col width="250px" />
                                    <tr style="height: 15px;">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: right;">
                                            <asp:Label ID="lblVacancyAnnounce" runat="server" CssClass="InputLabel">Военно училище:</asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <asp:DropDownList ID="ddlMilitarySchool" runat="server" CssClass="InputField" onchange="ddlChange(this, 'ddlMilitarySchool')"
                                                Width="350px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="vertical-align: top; text-align: right; width: 140px;">
                                            <asp:Label ID="lblMilitaryDepartment" runat="server" CssClass="InputLabel">Учебна година:</asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <asp:DropDownList ID="ddlMilitarySchoolYear" runat="server" CssClass="InputField"
                                                onchange="ddlChange(this, 'ddlMilitarySchoolYear')" Width="110px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px;">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: right;">
                                            <asp:Label ID="Label1" runat="server" CssClass="InputLabel">Специалност:</asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <div id="divSubject" runat="server">
                                            </div>
                                        </td>
                                        <td style="vertical-align: top; text-align: right;">
                                            <asp:Label ID="Label2" runat="server" CssClass="InputLabel">Специализация:</asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <div id="divSpecialization" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 15px;">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: right;">
                                            <asp:Label ID="Label3" runat="server" CssClass="InputLabel">Статус:</asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <%-- <select id="ddlStatus" onchange="ddlStatusChange(this)" style="width: 110px;">
                                            <option selected="selected" value="-1" label="<Всички>"></option>
                                            <option value="1">Приети</option>
                                            <option value="0">Неприети</option>
                                        </select>--%>
                                            <asp:DropDownList ID="ddlStatus" onchange="ddlStatusChange(this)" runat="server">
                                                <asp:ListItem Selected="True" Text="&lt;Всички&gt;" Value="-1"></asp:ListItem>
                                                <asp:ListItem Value="1">Приети</asp:ListItem>
                                                <asp:ListItem Value="0">Неприети</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" style="vertical-align: top; text-align: right;">
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
                                                <asp:LinkButton ID="btnPrintAllCadets" runat="server" CssClass="Button" OnClientClick="ShowPrintReportCadets(); return false;"><i></i><div 
                                                style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                                            </center>
                                        </td>
                                    </tr>
                                </colgroup>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 20px;">
                </div>
                <div id="divPagingItems" runat="server" style="width: 760px; margin: 0 auto; height: 35px; padding-left: 20px">
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
            <asp:HiddenField ID="hfMilitarySchoolId" runat="server" />
            <asp:HiddenField ID="hfMilitarySchoolYear" runat="server" />
            <asp:HiddenField ID="hfMilitarySubjectId" runat="server" />
            <asp:HiddenField ID="hfMilitarySpecializationId" runat="server" />
            <asp:HiddenField ID="hfStatusId" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
