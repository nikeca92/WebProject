<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ManageDeclarationOfAccident.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.ManageDeclarationOfAccident"
    Title="Управление на декларациите за злополука" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script src="../Scripts/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        var ValidationMessage;

        function SortTableBy(sort)
        {
            if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
            {
                sort = sort + 100;
            }
            document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;

            document.getElementById("<%= hdnPageIdx.ClientID %>").value = 1; //when sort -> goto to 1st page 

            document.getElementById("<%= btnRefresh.ClientID %>").click();    //Server refresh page           
        }

        function EditDeclarationAcc(declarationId)
        {
            JSRedirect("AddEditDeclarationOfAccident.aspx?DeclarationId=" + declarationId);
        }

        function DeleteDeclarationAcc(declarationId)
        {
            YesNoDialog("Желаете ли да изтриете декларацията?", ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "ManageDeclarationOfAccident.aspx?AjaxMethod=JSDeleteDeclarationAcc";
                var params = "";
                params += "DeclarationId=" + declarationId;
                
                function response_handler(xml)
                {
                    if (xmlValue(xml, "response") != "OK")
                    {
                        alert("Има проблем на сървъра!");
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

        function ClearHiddenFields()
        {
            document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "";
            document.getElementById("<%=lblGridMessage.ClientID %>").innerHTML = "";
           
            if (ValidateUiData())
            {
                return true;
            }
            else
            {
                //Show Validation Message
                document.getElementById("<%=lblMessageValidation.ClientID %>").innerHTML = ValidationMessage
                document.getElementById("<%=lblMessageValidation.ClientID %>").className = "ErrorText"
                return false;
            }

        }
        function ValidateUiData()
        {
            ValidationMessage = "";

            if (!document.getElementById("<%=txtDeclarationDateFrom.ClientID %>").value == "")
            {
                if (!IsValidDate(document.getElementById("<%= txtDeclarationDateFrom.ClientID %>").value))
                {
                    ValidationMessage += "Полето Дата на декларацията\"от\" е невалидна дата<br/>";
                }
            }
            if (!document.getElementById("<%=txtDeclarationDateTo.ClientID %>").value == "")
            {
                if (!IsValidDate(document.getElementById("<%= txtDeclarationDateTo.ClientID %>").value))
                {
                    ValidationMessage += "Полето Дата на декларацията\"до\" е невалидна дата<br/>";
                }
            }

            return (ValidationMessage == "");
        }

        function ShowPrintAllDeclarationOfAccidents()
        {
            var hfDeclarName = document.getElementById("<%= hfDeclarName.ClientID %>").value;
            var hfWorkerName = document.getElementById("<%= hfWorkerName.ClientID %>").value;
            var hfDateFrom = document.getElementById("<%= hfDateFrom.ClientID %>").value;
            var hfDateTo = document.getElementById("<%= hfDateTo.ClientID %>").value;
            var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

            var url = "";
            var pageName = "PrintAllDeclarationOfAccidents"
            var param = "";
            
            url = "../PrintContentPages/" + pageName + ".aspx?DeclarNumber=" + hfDeclarName 
                        + "&WorkerName=" + hfWorkerName + "&DateFrom=" + hfDateFrom
                        + "&DateTo=" + hfDateTo + "&SortBy=" + hdnSortBy;
            
            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        } 

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_Click" />
            <asp:HiddenField ID="hdnSortBy" runat="server" />
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hfDeclarName" runat="server" />
            <asp:HiddenField ID="hfWorkerName" runat="server" />
            <asp:HiddenField ID="hfDateFrom" runat="server" />
            <asp:HiddenField ID="hfDateTo" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
            <div style="height: 30px;">
            </div>
            <center>
                <span class="HeaderText">Управление на декларациите за злополука</span>
                <br />
                <br />
                <div class="FilterArea" style="padding-bottom: 0px; width: 730px;">
                    <div class="FilterLegend">Филтър</div>
                    <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                        <tr style="height: 30px">
                            <td align="left">
                                <span class="InputLabel" style="padding-left: 35px">№ Декларация:</span>
                                <asp:TextBox ID="txtDeclarationNumber" Width="100px" CssClass="InputField" MaxLength="10"
                                    runat="server"></asp:TextBox>
                                <span class="InputLabel" style="padding-left: 20px">Име на пострадалия:</span>
                                <asp:TextBox ID="txtWorkerFullName" Width="300px" CssClass="InputField" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td align="left">
                                <span class="InputLabel" style="padding-left: 35px">Дата на декларацията от:</span>
                                <asp:TextBox ID="txtDeclarationDateFrom" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                                <span class="InputLabel" style="padding-left: 10px">до</span>
                                <asp:TextBox ID="txtDeclarationDateTo" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td align="center">
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="Button" OnClientClick="return ClearHiddenFields()"
                                    OnClick="btnSearch_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                <div style="padding-left: 30px; display: inline">
                                </div>
                                <asp:LinkButton ID="btnPrintAllDeclarationOfAccidents" runat="server" CssClass="Button" OnClientClick="ShowPrintAllDeclarationOfAccidents(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="ErrorText" ID="lblMessageValidation" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 15px; vertical-align: middle" align="center">
                    <asp:Label ID="lblSearchMessage" runat="server" Text=""></asp:Label>
                </div>
                <br />
                <div style="text-align: center;">
                    <div style="width: 730px; margin: 0 auto; text-align: left; vertical-align: top;
                        height: 35px;">
                        <div style="display: inline; position: relative; top: -16px; padding-left: 5px">
                        </div>
                        <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:110px; padding-left:5px;">Нова декларация</div><b></b></asp:LinkButton>
                        <div style="display: inline; position: relative; top: -16px; padding-left: 100px">
                            <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Първа страница" CssClass="PaginationButton"
                                OnClick="btnFirst_Click" />
                            <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Предишна страница" CssClass="PaginationButton"
                                OnClick="btnPrev_Click" />
                            <asp:Label Width="90px" ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
                            <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Следваща страница" CssClass="PaginationButton"
                                OnClick="btnNext_Click" />
                            <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Последна страница" CssClass="PaginationButton"
                                OnClick="btnLast_Click" />
                            <span style="min-width: 100px; padding: 45px">&nbsp;</span> <span style="text-align: right;">
                                Отиди на страница</span>
                            <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
                            <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Отиди на страница" CssClass="PaginationButton"
                                OnClick="btnGoto_Click" />
                        </div>
                    </div>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 600px; margin: 0 auto;">
                        <div runat="server" id="DeclarationsGrid" style="text-align: center;">
                        </div>
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
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
