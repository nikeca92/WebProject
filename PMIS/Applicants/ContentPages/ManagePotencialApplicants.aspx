<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ManagePotencialApplicants.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ManagePotencialApplicants"
    Title="Списък на потенциалните кандидати" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/PickList.js"></script>

    <script type="text/javascript">
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);        

        var hdnDrvLicCategoriesClientID = "<%= hdnDrvLicCategories.ClientID %>";
        var hfDrivingLicenseClientID = "<%= hfDrivingLicense.ClientID %>";
        var hdnServiceTypesClientID = "<%= hdnServiceTypes.ClientID %>";
        var hfServiceTypesClientID = "<%= hfServiceTypes.ClientID %>";
        
        //Call this function when the page is loaded
        function PageLoad()
        {
           
            LoadPickLists();
        }

        function EndRequestHandlerPage(sender, args)
        {
            LoadPickLists();
        }

        function LoadPickLists()
        {
            var configPickListDrvLicCategories =
                {
                    width: 175,
                    allLabel: "<Всички>"
                }

            categories = document.getElementById(hdnDrvLicCategoriesClientID).value;
            categories = eval(categories);
            PickListUtil.AddPickList("pickListDrvLicCategories", categories, "tdPickListDrvLicCategories", configPickListDrvLicCategories);

            if (TrimString(document.getElementById(hfDrivingLicenseClientID).value) != "")
            {
                PickListUtil.SetSelection("pickListDrvLicCategories", document.getElementById(hfDrivingLicenseClientID).value);
            }
           
            serviceTypes = document.getElementById(hdnServiceTypesClientID).value;
            serviceTypes = eval(serviceTypes);
            PickListUtil.AddPickList("pickListServiceTypes", serviceTypes, "tdPickListServiceTypes", configPickListDrvLicCategories);

            if (TrimString(document.getElementById(hfServiceTypesClientID).value) != "") {
                PickListUtil.SetSelection("pickListServiceTypes", document.getElementById(hfServiceTypesClientID).value);
            }
        }

        function SortTableBy(sort)
        {
            if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
            {
                sort = sort + 100;
            }

            document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
            document.getElementById("<%= btnRefresh.ClientID %>").click();
        }

        function EditApplicant(potencialApplicantId)
        {
            params = "";
            params += "PotencialApplicantId=" + potencialApplicantId;
            params += "&IsRegistered=OK";
            params += "&PageFrom=1";
            JSRedirect("AddPotencialApplicant_PersonDetails.aspx?" + params);
        }

        function DeleteApplicant(potencialApplicantId)
        {
            YesNoDialog("Желаете ли да изтриете кандидата?", ConfirmYes, null);
            
            function ConfirmYes()
            {
                var url = "ManagePotencialApplicants.aspx?AjaxMethod=JSDeletePotencialApplicant";
                var params = "";
                params += "PotencialApplicantId=" + potencialApplicantId;
                
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

        function ShowPrintAllApplicants()
        {
            var hfMilitaryDepartmentId = document.getElementById("<%= hfMilitaryDepartmentId.ClientID %>").value;
            var hfDrivingLicense = document.getElementById("<%= hfDrivingLicense.ClientID %>").value; //hdnDrvLicCategories
            var hfComment = document.getElementById("<%= hfComment.ClientID %>").value; 
            var hfServiceTypes = document.getElementById("<%= hfServiceTypes.ClientID %>").value; 
            var hdnLastApperianceFrom = document.getElementById("<%= hdnLastApperianceFrom.ClientID %>").value;
            var hdnLastApperianceTo = document.getElementById("<%= hdnLastApperianceTo.ClientID %>").value;
            var hdnIdentityNumber = document.getElementById("<%= txtIdentNumber.ClientID %>").value;
            
            var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

            var url = "";
            var pageName = "PrintAllPotencialApplicants"
            var param = "";
            url = "../PrintContentPages/" + pageName + ".aspx?" +
                 "&MilitaryDepartmentID=" + hfMilitaryDepartmentId +
                 "&DrivingLicense=" + hfDrivingLicense +
                 "&Comment=" + hfComment +
                 "&ServiceType=" + hfServiceTypes +
                 "&LastApperianceFrom=" + hdnLastApperianceFrom +
                 "&LastApperianceTo=" + hdnLastApperianceTo +
                 "&IdentityNumber=" + hdnIdentityNumber + 
                 "&SortBy=" + hdnSortBy;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }

        function SetPickListSelection()
        {
            document.getElementById(hfDrivingLicenseClientID).value = PickListUtil.GetSelectedValues("pickListDrvLicCategories");
            document.getElementById(hfServiceTypesClientID).value = PickListUtil.GetSelectedValues("pickListServiceTypes");
        }

    </script>

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
	    width: 1100px;
    }

    #SubShadowContainer
    {
	    width: 1102px;
    }

    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" id="CanLeave" value="true" />
            <center>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на потенциалните кандидати</span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 800px; margin: 0 auto;">                        
                        <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <tr style="height: 10px;"></tr>
                                <tr>
                                    <%--<td style="vertical-align: top; text-align: right; width: 80px;">
                                        <asp:Label runat="server" ID="lblVacancyAnnounce" CssClass="InputLabel">Заповед №:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlVacancyAnnounces" CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>--%>
                                    <td style="vertical-align: top; text-align: right; width: 150px;">
                                        <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Място на регистрация:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top; width: 250px;">
                                        <asp:DropDownList runat="server" ID="ddlMilitaryDepartments" Width="250px" CssClass="InputField">
                                        </asp:DropDownList>
                                    </td>                                    
                                    <td style="vertical-align: top; text-align: right; width: 130px;">
                                        <asp:Label runat="server" ID="lblDrvLicCategories" CssClass="InputLabel"> Шофьорска книжка:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top; width: 200px;">
                                        <div id="tdPickListDrvLicCategories" style="display: inline-table; margin: 0px;"></div>
                                    </td>                                                    
                                </tr>
                                <tr style="height: 25px;">
                                    <td style="vertical-align: top; text-align: right;">
                                        <asp:Label runat="server" ID="lblComment" CssClass="InputLabel">Коментар:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:TextBox ID="txtComment" runat="server" CssClass="InputField" Width="240px"></asp:TextBox>
                                    </td>
                                    <td style="vertical-align: top; text-align: right;">
                                        <asp:Label runat="server" ID="lblServiceType" CssClass="InputLabel">Вид служба:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top; width: 200px;">
                                        <div id="tdPickListServiceTypes" style="display: inline-table; margin: 0px;"></div>
                                    </td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td style="vertical-align: top; text-align: left; padding-left:10px;" colspan="2">
                                        <div id="divLastApperiance">
                                            <asp:Label runat="server" ID="lblLastApperianceFrom" CssClass="InputLabel">Дата на последно явяване от:</asp:Label>
                                            <asp:TextBox ID="txtLastApperianceFrom" runat="server" CssClass="InputField" Width="75px"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblLastApperianceTo" CssClass="InputLabel" style="padding-left:7px;">до:</asp:Label>
                                            <asp:TextBox ID="txtLastApperianceTo" runat="server" CssClass="InputField" Width="75px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td style="vertical-align: top; text-align: right; width: 80px;">
                                        <asp:Label runat="server" ID="lblIdentNumber" CssClass="InputLabel">ЕГН:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:TextBox runat="server" ID="txtIdentNumber" CssClass="InputField" Width="175px" style="margin-right: 30px"/>
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
                                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClientClick="SetPickListSelection();" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                            <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                            <div style="padding-left: 30px; display: inline">
                                            </div>
                                            <asp:LinkButton ID="btnPrintAllApplicants" runat="server" CssClass="Button" OnClientClick="ShowPrintAllApplicants(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
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
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:170px; padding-left:5px;">Нов потенциален кандидат</div><b></b></asp:LinkButton>
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
                    <div runat="server" id="pnlApplicantsGrid" style="text-align: center;">
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
            <%-- <asp:HiddenField ID="hfVacancyAnnounceId" runat="server" />--%>
            <asp:HiddenField ID="hfMilitaryDepartmentId" runat="server" />
            <asp:HiddenField ID="hfComment" runat="server" />
            <asp:HiddenField ID="hfDrivingLicense" runat="server" />
            <asp:HiddenField ID="hdnDrvLicCategories" runat="server"/>
            <asp:HiddenField ID="hdnServiceTypes" runat="server" />
            <asp:HiddenField ID="hfServiceTypes" runat="server" />
            <asp:HiddenField ID="hdnLastApperianceFrom" runat="server" />
            <asp:HiddenField ID="hdnLastApperianceTo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
