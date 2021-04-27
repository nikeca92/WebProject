<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditInvestigationProtocol.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditInvestigationProtocol" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script src="../Scripts/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        var ValidationMessage;
        var orderBy;
        var pageIdx;
        var maxPage;
        var goToPage;

        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
        function PageLoad()
        {
            hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
        }

        function ShowLightTable()
        {
            var url = "AddEditInvestigationProtocol.aspx?AjaxMethod=JSGetDeclarationTableField";
            var params;
            
            function response_handler(xml)
            {
                document.getElementById('divTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                RefreshDatePickers();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
            
            setTimeout("ShowTableLightBox()", 300);
        }

        // Shows the light box with GTable items and "disable" rest of the page
        function ShowTableLightBox()
        {
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divTableLightBox").style.display = "";
            CenterLightBox("divTableLightBox");
        }
        // Close the light box and clear its content
        function HideTableLightBox()
        {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divTableLightBox").style.display = "none";
        }

        function DoAjaxSearch()
        {
            if (ValidateLightBoxItem())
            {
                document.getElementById('lblDeclarationMessage').innerHTML = "";
                //Refresh Table
                GetDeclarationTableItems(1, 1);
            }
            else
            {
                //Show Validation Message
                document.getElementById('lblDeclarationMessage').innerHTML = ValidationMessage
                document.getElementById('lblDeclarationMessage').className = "ErrorText"
            }
        }

        function BtnPagingClick(objectName)
        {

            switch (objectName)
            {
                case "btnFirst":
                    orderBy = parseInt(document.getElementById('hdnOrderBy').value);
                    pageIdx = 1;

                    GetDeclarationTableItems(orderBy, pageIdx);
                    break;

                case "btnPrev":
                    orderBy = parseInt(document.getElementById('hdnOrderBy').value);
                    pageIdx = parseInt(document.getElementById('hdnPageIndex').value);

                    if (pageIdx > 1)
                    {
                        pageIdx--;
                        GetDeclarationTableItems(orderBy, pageIdx);
                    }

                    break;

                case "btnNext":
                    orderBy = parseInt(document.getElementById('hdnOrderBy').value);
                    pageIdx = parseInt(document.getElementById('hdnPageIndex').value);
                    maxPage = parseInt(document.getElementById('hdnPageMaxPage').value);

                    if (pageIdx < maxPage)
                    {
                        pageIdx++;
                        GetDeclarationTableItems(orderBy, pageIdx);
                    }
                    break;


                case "btnLast":
                    orderBy = parseInt(document.getElementById('hdnOrderBy').value);
                    pageIdx = parseInt(document.getElementById('hdnPageMaxPage').value);

                    GetDeclarationTableItems(orderBy, pageIdx);
                    break;

                case "btnPageGo":
                    orderBy = parseInt(document.getElementById('hdnOrderBy').value);
                    maxPage = parseInt(document.getElementById('hdnPageMaxPage').value);
                    goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

                    if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
                    {
                        pageIdx = goToPage;
                        GetDeclarationTableItems(orderBy, pageIdx);
                    }
                    break;

                default:
                    break;
            }

        }

        function SortTableBy(sort)
        {

            pageIdx = parseInt(document.getElementById('hdnPageIndex').value);
            orderBy = parseInt(document.getElementById('hdnOrderBy').value);

            if (orderBy == sort)
            {
                sort = sort + 100;
            }

            orderBy = sort;
            pageIdx = 1; //We go to 1st page

            GetDeclarationTableItems(orderBy, pageIdx)

        }

        function GetDeclarationTableItems(orderBy, pageIdx)
        {
            var url = "AddEditInvestigationProtocol.aspx?AjaxMethod=JSGetDeclarationTableField";
            var params = "";
            //Set Filter Parameters
            params += "&txtDeclarationNumber=" + document.getElementById('txtDeclarationNumber').value;
            params += "&txtDeclarationDateFrom=" + document.getElementById('txtDeclarationDateFrom').value;
            params += "&txtDeclarationDateTo=" + document.getElementById('txtDeclarationDateTo').value;
            params += "&txtWorkerFullName=" + document.getElementById('txtWorkerFullName').value;
            //Set Paging Parameters
            params += "&hdnOrderBy=" + orderBy;
            params += "&hdnPageIndex=" + pageIdx;

            function response_handler(xml)
            {
                document.getElementById('divTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                RefreshDatePickers();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function SelectDeclarationId(declarationId)
        {
            // alert(declrationId);
            //Set data with selected declarationId and close window
            document.getElementById("<%= hfDeclarationId.ClientID %>").value = declarationId;
            document.getElementById("<%= txtDeclarationId.ClientID %>").value = declarationId;

            //Close window
            HideTableLightBox()
            //Call AJAX

            var url = "AddEditInvestigationProtocol.aspx?AjaxMethod=JSGetInvProtFields";
            var params = "declarationId=" + declarationId;
            
            function response_handler2(xml)
            {
                setInnerText(document.getElementById("<%= txtAccDateTime.ClientID %>"), xmlNodeText(xml.childNodes[0]));
                setInnerText(document.getElementById("<%= txtWorkerFullName.ClientID %>"), xmlNodeText(xml.childNodes[1]));
                setInnerText(document.getElementById("<%= hfDeclarationId.ClientID %>"), xmlNodeText(xml.childNodes[2]));
                setInnerText(document.getElementById("<%= txtDeclarationId.ClientID %>"), xmlNodeText(xml.childNodes[2]));

                document.getElementById("<%= hdnAccDateTime.ClientID %>").value = xmlNodeText(xml.childNodes[0]);
                document.getElementById("<%= hdnWorkerFullName.ClientID %>").value = xmlNodeText(xml.childNodes[1]);
            }

            var myAJAX = new AJAX(url, true, params, response_handler2);
            myAJAX.Call();
        }
        function ValidateProtocolItem()
        {
            document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "";
            ValidationMessage = "";
            //Validate for empty Fields
            if (document.getElementById("<%= txtInvestigaitonProtocolNumber.ClientID %>").value == "")
            {
                ValidationMessage = GetErrorMessageMandatory("номер на протокола") + "</br>";
            }

            //                       if (document.getElementById("<%= txtWorkerFullName.ClientID %>").innerText=="")
            //                     {
            //                        ValidationMessage+="Моля изберете от списъка с декларации име на пострадалия</br>";
            //                     }

            if (document.getElementById("<%= txtInvProtDate.ClientID %>").value == "")
            {
                ValidationMessage += GetErrorMessageMandatory("дата на протокол") + "</br>";
            }
            else
            {
                if (!IsValidDate(document.getElementById("<%= txtInvProtDate.ClientID %>").value))
                {
                    ValidationMessage += GetErrorMessageDate("дата на протокол") + "<br/>";
                }
            }

            if (!document.getElementById("<%= txtDateFrom.ClientID %>").value == "")
            {
                if (!IsValidDate(document.getElementById("<%= txtDateFrom.ClientID %>").value))
                {
                    ValidationMessage += GetErrorMessageDate("дата (от)") + "<br/>";
                }
            }

            if (!document.getElementById("<%= txtDateTo.ClientID %>").value == "")
            {
                if (!IsValidDate(document.getElementById("<%= txtDateTo.ClientID %>").value))
                {
                    ValidationMessage += GetErrorMessageDate("дата (до)") + "<br/>";
                }
            }

            document.getElementById("lblValidationMessage").innerHTML = ValidationMessage

            if (ValidationMessage == "")
            {
                ForceNoChanges();
            }

            return (ValidationMessage == "");

        }


        function ValidateLightBoxItem()
        {
            ValidationMessage = "";

            if (!document.getElementById('txtDeclarationDateFrom').value == "")
            {
                if (!IsValidDate(document.getElementById('txtDeclarationDateFrom').value))
                {
                    ValidationMessage += GetErrorMessageDate("Полето Дата на декларацията (от)") + "<br/>";
                }
            }
            if (!document.getElementById('txtDeclarationDateTo').value == "")
            {
                if (!IsValidDate(document.getElementById('txtDeclarationDateTo').value))
                {
                    ValidationMessage += GetErrorMessageDate("Полето Дата на декларацията (до)") + "<br/>";
                }
            }
            //            if(ValidationMessage=="")
            //                 {
            //                       if(document.getElementById('txtDeclarationNumber').value == "" && document.getElementById('txtWorkerFullName').value == "")
            //                           {
            //                           ValidationMessage="Моля въведете критерии за търсене";
            //                           }           
            //                 }

            return (ValidationMessage == "");
        }

        function ShowPrintInvestigationProtocol()
        {
            var hfInvestigaitonProtocolId = document.getElementById("<%= hfInvestigaitonProtocolId.ClientID %>").value;

            var url = "";
            var pageName = "PrintInvestigationProtocol"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?InvestigationProtocolID=" + hfInvestigaitonProtocolId;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }
    
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfInvestigaitonProtocolId" runat="server" />
            <asp:HiddenField ID="hfDeclarationId" runat="server" />
            <asp:TextBox ID="txtDeclarationId" runat="server" style="display: none;"></asp:TextBox>
            <asp:HiddenField ID="hdnAccDateTime" runat="server" />
            <asp:HiddenField ID="hdnWorkerFullName" runat="server" />
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
            <asp:HiddenField ID="hdnSavedChanges" runat="server" />
            <asp:HiddenField ID="hfFromHome" runat="server" />
            <div id="divTableLightBox" class="DeclarationLightBox" style="padding: 10px; display: none;
                text-align: center;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideTableLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divTableLightBoxContent">
                </div>
            </div>
            <center>
                <br />
                <asp:Label ID="lblHeaderCell" runat="server" CssClass="HeaderText"></asp:Label><br />
                <div style="height: 10px;">
                </div>
                <table id="HeaderTable" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblInvestigaitonProtocolNumber" runat="server" CssClass="HeaderText2">№</asp:Label>
                            <asp:TextBox ID="txtInvestigaitonProtocolNumber" Width="65px" runat="server" CssClass="RequiredInputField"></asp:TextBox>
                            <asp:Label ID="lblInvProtDate" runat="server" Style="padding-left: 15px" CssClass="HeaderText2">oт дата</asp:Label>
                            <asp:TextBox ID="txtInvProtDate" MaxLength="10" Width="75px" runat="server" CssClass="RequiredInputField"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" CssClass="HeaderText2">за резултатите от разследване на злополука</asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="HeaderTable2" class="TableAddEditInvestigationProtocol" runat="server">
                    <tr>
                        <td style="height: 30px" valign="middle">
                            <asp:Label ID="lblAccDateTime" runat="server" CssClass="InputLabel" Style="padding-right: 10px">станала на: </asp:Label>
                            <asp:Label ID="txtAccDateTime" runat="server" Width="65px" CssClass="InputField"></asp:Label>
                            <asp:Label ID="lblWorkerFullName" runat="server" CssClass="InputLabel" Style="padding-right: 10px;
                                padding-left: 15px">с</asp:Label>
                            <asp:Label ID="txtWorkerFullName" Width="350px" CssClass="InputField" runat="server"></asp:Label>
                            <img id="btnDeclarationId" alt="Редактиране на списъка" title="Изберете декларация"
                                style="cursor: pointer;" src="../Images/list_edit.png" onclick="ShowLightTable();" />
                        </td>
                    </tr>
                </table>
                <table id="MainTable" class="TableAddEditInvestigationProtocol" runat="server">
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 93px">
                            <asp:Label ID="lblDateFrom" runat="server" CssClass="InputLabel" Style="width: 30px">От</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDateFrom" MaxLength="10" Width="75px" CssClass="InputField" runat="server"></asp:TextBox>
                            <asp:Label ID="lblDateTo" runat="server" CssClass="InputLabel" Style="width: 30px">до</asp:Label>
                            <asp:TextBox ID="txtDateTo" MaxLength="10" Width="75px" CssClass="InputField" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblLegalReason" runat="server" CssClass="InputLabel" Style="width: 30px">на основание</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox Width="580px" ID="txtLegalReason" CssClass="InputField" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblOrderNum" runat="server" CssClass="InputLabel">и заповед № </asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrderNum" Width="400px" CssClass="InputField" runat="server" />
                            <asp:Label ID="lblCommissionStaff" runat="server" CssClass="InputLabel" Style="padding-left: 2px">комисия в състав:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCommissionChairman" runat="server" CssClass="InputLabel">Председател:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionChairman" Width="400px" CssClass="InputField" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCommissionMembers" runat="server" CssClass="InputLabel">Членове:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionMember1" Width="400px" CssClass="InputField" runat="server" MaxLength="2000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionMember2" CssClass="InputField" Width="400px" runat="server" MaxLength="2000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionMember3" CssClass="InputField" Width="400px" runat="server" MaxLength="2000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionMember4" CssClass="InputField" Width="400px" runat="server" MaxLength="2000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommissionMember5" CssClass="InputField" Width="400px" runat="server" MaxLength="2000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Label ID="lblMakeInvestigation" runat="server" Style="padding-left: 50px" CssClass="InputLabel">извърши разследване и установи следното:
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblInjured" runat="server" CssClass="InputLabel">I. Пострадал:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" Height="80px" ID="txtInjured" TextMode="MultiLine"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblAccidentDateAndPlace" runat="server" CssClass="InputLabel">II. Място и време на злополуката:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" Height="80px" ID="txtAccidentDateAndPlace"
                                TextMode="MultiLine" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblWitnesses" runat="server" CssClass="InputLabel">III. Свидетели на злополуката и оказали първа помощ:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" Height="80px" ID="txtWitnesses"
                                TextMode="MultiLine" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblJobGeneralDesc" runat="server" CssClass="InputLabel">IV. Обща характеристика на работата, извършвана от пострадалия
                                преди злополуката:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtJobGeneralDesc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblSpecificTaskActivity" runat="server" CssClass="InputLabel">V. Специфично физическо действие, извършвано от пострадалия
                                преди злополуката и свързания с това действие материален фактор (предмет, вещество
                                и др.):</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtSpecificTaskActivity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblDeviationOfNormalActivity" runat="server" CssClass="InputLabel">VI. Отклонение от нормалните действия и условия и материалния
                                фактор, свързан с тези отклонения:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" Height="80px" ID="txtDeviationOfNormalActivity"
                                TextMode="MultiLine" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblInjuryDetails" runat="server" CssClass="InputLabel">VII. Начин на увреждане и материалния фактор, причинил увреждането:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtInjuryDetails" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label ID="lblAnalysisOfAccidentCauses" runat="server" CssClass="InputLabel">VIII. Анализ на причините за възникване на злополуката:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtAnalysisOfAccidentCauses"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label runat="server" ID="lblLegalViolations" CssClass="InputLabel">IX. Допуснати нарушения на нормативни актове:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtLegalViolations" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label runat="server" ID="lblItruders" CssClass="InputLabel">X. Лица допуснали нарушенията или на които се предлага търсенето
                                на отговорност:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtItruders" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label runat="server" ID="lblActionsToAvoid" CssClass="InputLabel">XI. Необходими мерки за недопускане на подобни злополуки:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:TextBox CssClass="InputField" Width="640px" ID="txtActionsToAvoid" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="td1">
                            <asp:Label runat="server" ID="lblEnclosures" CssClass="InputLabel">XII. Приложения</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td class="td1">
                                <asp:TextBox CssClass="InputField" Width="640px" ID="txtEnclosures" runat="server"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="min-height: 15px">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                <div id="lblValidationMessage" class="ErrorText">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 5px;">
                            </div>
                        </td>
                    </tr>
                </table>
                <%--<div id="divSaveProtocol" style="display: inline; padding-right: 50px; padding-left: 200px"
                    onclick="SaveDeclaration();" CssClass="Button">
                    <i></i>
                    <div style="width: 70px; display: inline">
                        Запис</div>
                    <b></b>
                </div>--%>
                <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                <asp:LinkButton ID="btnPrintInvestigationProtocol" runat="server" CssClass="Button"
                    OnClientClick="ShowPrintInvestigationProtocol(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                <span style="margin-left: 50px;">
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="Button" OnClick="btnCancel_Click"
                        CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </span>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
