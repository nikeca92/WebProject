﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageCadets.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ManageCadets" Title="Списък на курсантите" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript">

        function SortTableBy(sort)
        {
            if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
            {
                sort = sort + 100;
            }

            document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
            document.getElementById("<%= btnRefresh.ClientID %>").click();
        }

        function EditCadet(cadetId)
        {
            JSRedirect("EditCadet.aspx?CadetId=" + cadetId + "&PageFrom=1");
        }

        function DeleteCadet(cadetId)
        {
            YesNoDialog("Желаете ли да изтриете курсанта?", ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "ManageCadets.aspx?AjaxMethod=JSDeleteCadet";
                var params = "";
                params += "CadetId=" + cadetId;
                
                function response_handler(xml)
                {
                    if (xmlValue(xml, "response") != "OK")
                    {
                        alert("Има проблеми на сървъра!");
                    }
                    else
                    {
                        document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "DELETED";
                        document.getElementById("<%=btnRefresh.ClientID %>").click();
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }
        
        function ShowPrintAllCadets()
        {
            var hfMilitarySchoolId = document.getElementById("<%= hfMilitarySchoolId.ClientID %>").value;
            var hfMilitaryDepartmentId = document.getElementById("<%= hfMilitaryDepartmentId.ClientID %>").value;
            var hfSchoolYear = document.getElementById("<%= hfSchoolYear.ClientID %>").value;
            var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;
            var hdnIdentityNumber = document.getElementById("<%= txtIdentNumber.ClientID %>").value

            var url = "";
            var pageName = "PrintAllCadets"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?MilitarySchoolID=" + hfMilitarySchoolId
                + "&MilitaryDepartmentID=" + hfMilitaryDepartmentId
                + "&SchoolYear=" + hfSchoolYear
                + "&IdentityNumber=" + hdnIdentityNumber
                + "&SortBy=" + hdnSortBy;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" id="CanLeave" value="true" />
            <center>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на курсантите</span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 800px; margin: 0 auto;">
                        <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <tr style="height: 10px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 80px;">
                                        <asp:Label runat="server" ID="lblMilitarySchool" CssClass="InputLabel">Военно училище:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlMilitarySchools" CssClass="InputField" 
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlMilitarySchools_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 120px;">
                                        <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Място на регистрация:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlMilitaryDepartments" Width="200px" CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 80px;">
                                        <asp:Label runat="server" ID="lblSchoolYear" CssClass="InputLabel">Учебна година:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlSchoolYears" CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 80px;">
                                        <asp:Label runat="server" ID="lblIdentNumber" CssClass="InputLabel">ЕГН:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:TextBox runat="server" ID="txtIdentNumber" CssClass="InputField" Width="196px" style="margin-right: 30px"/>
                                    </td>
                                    <td colspan="2">&nbsp;</td>
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
                                            <asp:LinkButton ID="btnPrintAllCadets" runat="server" CssClass="Button" OnClientClick="ShowPrintAllCadets(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="width: 760px; margin: 0 auto; height: 35px; padding-left: 20px">
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:90px; padding-left:5px;">Нов курсант</div><b></b></asp:LinkButton>
                    <span style="padding-left: 100px"></span>
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
                    <div runat="server" id="pnlCadetsGrid" style="text-align: center;">
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
            <asp:HiddenField ID="hfMilitaryDepartmentId" runat="server" />
            <asp:HiddenField ID="hfSchoolYear" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
