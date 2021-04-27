<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="EquipmentReservistRequestMilitaryDepartment.aspx.cs" Inherits="PMIS.Reserve.ContentPages.EquipmentReservistRequestMilitaryDepartment" 
         Title="Определяне на ВО на заявка за окомплектоване с ресурс от резерва" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    
.AddMilitaryDepartmentLightBox
{
	width: 480px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 250px;
	left: 40%;
	min-height: 160px;
	z-index: 1000;
	padding-top: 10px;
}

</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);

    function PageEndRequestHandler(sender)
    {
        if (sender._postBackSettings.sourceElement.id == "<%= ddMilitaryCommand.ClientID %>")
        {
            LoadOriginalValues();
        }
    }

    function ShowAddMilitaryDepartmentLightBox()
    {

        var url = "EquipmentReservistRequestMilitaryDepartment.aspx?AjaxMethod=JSPopulateMilitaryDepartments";

        var params = "RequestCommandID=" + document.getElementById("RequestCommandID").value;

        function response_handler(xml)
        {
            ClearSelectList(document.getElementById("ddMilitaryDepartments"), true);

            var militaryDepartments = xml.getElementsByTagName("md");

            for (var i = 0; i < militaryDepartments.length; i++)
            {
                var id = xmlValue(militaryDepartments[i], "id");
                var name = xmlValue(militaryDepartments[i], "name");

                AddToSelectList(document.getElementById("ddMilitaryDepartments"), id, name);
            };

            // clean message label in the light box and hide it
            document.getElementById("spanAddMilitaryDepartmentLightBoxMessage").style.display = "none";
            document.getElementById("spanAddMilitaryDepartmentLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("AddMilitaryDepartmentLightBox").style.display = "";
            CenterLightBox("AddMilitaryDepartmentLightBox");
        }
        
        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }

    // Close the light box
    function HideAddMilitaryDepartmentLightBox()
    {
        document.getElementById("HidePage").style.display = "none";
        document.getElementById("AddMilitaryDepartmentLightBox").style.display = "none";
    }

    // Select and add a MilitaryCommand
    function SaveAddMilitaryDepartmentLightBox()
    {
        if (document.getElementById("ddMilitaryDepartments").value != "-1")
        {
            var url = "EquipmentReservistRequestMilitaryDepartment.aspx?AjaxMethod=JSAddMilitaryDepartment";

            var params = "RequestCommandID=" + document.getElementById("RequestCommandID").value +
                         "&MilitaryDepartmentID=" + document.getElementById("ddMilitaryDepartments").value +
                         "&EquipmentReservistsRequestID=" + document.getElementById("<%= hfEquipmentReservistsRequestID.ClientID %>").value;

            function response_handler(xml)
            {
                if (xmlNodeText(xml.childNodes[0]) == "OK")
                {
                    //document.getElementById("<%= hdnRefreshReason.ClientID %>").value = "SAVED";
                    HideAddMilitaryDepartmentLightBox();
                    document.getElementById("<%= btnRefresh.ClientID %>").click();
                }
                else
                {
                    var lblMessage = document.getElementById("spanAddMilitaryDepartmentLightBoxMessage");
                    lblMessage.className = "ErrorText";
                    lblMessage.innerHTML = "Грешка при запис на данните";
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
    }

    // Delete a particular Command from the Request
    function DeleteMilitaryDepartment(militaryDepartmentID, militaryDepartmentName)
    {
        YesNoDialog("Желаете ли да изтриете " + militaryDepartmentName + "?", ConfirmYes, null);

        function ConfirmYes()
        {
            var url = "EquipmentReservistRequestMilitaryDepartment.aspx?AjaxMethod=JSDeleteMilitaryDepartment";

            var params = "RequestCommandID=" + document.getElementById("RequestCommandID").value +
                     "&MilitaryDepartmentID=" + militaryDepartmentID +
                     "&EquipmentReservistsRequestID=" + document.getElementById("<%= hfEquipmentReservistsRequestID.ClientID %>").value;

            function response_handler(xml)
            {
                if (xmlNodeText(xml.childNodes[0]) == "OK")
                {
                    //document.getElementById("<%= hdnRefreshReason.ClientID %>").value = "SAVED";                
                    document.getElementById("<%= btnRefresh.ClientID %>").click();
                }
                else
                {
                    var lblMessage = document.getElementById("<%= lblGridMessage.ClientID %>");
                    lblMessage.className = "ErrorText";
                    lblMessage.innerHTML = "Грешка при изтриване на данни";
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
    }

    function SaveReservistsCount()
    {
        var lblMessage = document.getElementById("<%=lblGridMessage.ClientID %>");
        lblMessage.innerHTML = "";
        
        var positionsCounter = document.getElementById("positionsCounter");
        var militaryDepartmentsCounter = document.getElementById("militaryDepartmentsCounter");

        if (positionsCounter && militaryDepartmentsCounter)
        {
            var positionsCount = parseInt(positionsCounter.value);
            var militaryDepartmentsCount = parseInt(militaryDepartmentsCounter.value);
            var url = "EquipmentReservistRequestMilitaryDepartment.aspx?AjaxMethod=JSSaveReservistsCount";

            var params = "PositionsCount=" + positionsCount;
            params += "&MilitaryDepartmentsCount=" + militaryDepartmentsCount;
            params += "&RequestCommandID=" + document.getElementById("RequestCommandID").value;
            params += "&EquipmentReservistsRequestID=" + document.getElementById("<%= hfEquipmentReservistsRequestID.ClientID %>").value;
//            params += "&ResponsibleMilitaryUnitID=" + document.getElementById("ddResponsibleMilitaryUnit.ClientID").value;
//            params += "&OrderNumDate=" + document.getElementById("ddOrderNum.ClientID").options[document.getElementById("ddOrderNum.ClientID").selectedIndex].text;
//            params += "&ResponsibleMilitaryUnitName=" + document.getElementById("ddResponsibleMilitaryUnit.ClientID").options[document.getElementById("ddResponsibleMilitaryUnit.ClientID").selectedIndex].text;

            var isDataValid = true;

            for (var i = 1; i <= positionsCount; i++)
            {
                var requestCommandPositionID = document.getElementById("requestCommandPositionID" + i).value;
                var positionReservistsCount = document.getElementById("reservistsCount" + i).value;

                params += "&RequestCommandPositionID" + i + "=" + requestCommandPositionID;

                var currentReservists = 0;
                
                for (var j = 1; j <= militaryDepartmentsCount; j++)
                {
                    var reqCommandPositionMilDeptID = document.getElementById("reqCommandPositionMilDeptID" + i + "_" + j).value;
                    var militaryDepartmentID = document.getElementById("militaryDepartmentID" + i + "_" + j).value
                    var reservistsCount = document.getElementById("reservistsCount" + i + "_" + j).value;

                    if (reservistsCount != "" && !isInt(reservistsCount))
                    {
                        isDataValid = false;
                        lblMessage.className = "ErrorText";
                        lblMessage.innerHTML += "<br /> Невалидно число на ред №" + i;
                    }

                    if (reservistsCount != "" && isInt(reservistsCount))
                    {
                        currentReservists += parseInt(reservistsCount);
                    }

                    params += "&ReqCommandPositionMilDeptID" + i + "_" + j + "=" + reqCommandPositionMilDeptID;
                    params += "&MilitaryDepartmentID" + i + "_" + j + "=" + militaryDepartmentID;
                    params += "&ReservistsCount" + i + "_" + j + "=" + reservistsCount;
                }

                if (currentReservists > positionReservistsCount)
                {
                    isDataValid = false;
                    lblMessage.className = "ErrorText";
                    lblMessage.innerHTML += "<br /> На ред № " + i + " сумата от разпределените бройки между военните окръжия надвишава общия брой запасни";
                }
            }

            function response_handler(xml)
            {
                var lblMessage = document.getElementById("<%=lblGridMessage.ClientID %>");
                if (xmlNodeText(xml.childNodes[0]) == "OK")
                {
                    document.getElementById("<%= hdnRefreshReason.ClientID %>").value = "SAVED";
                    document.getElementById("<%= btnRefresh.ClientID %>").click();
                    LoadOriginalValues();                 
                }
                else
                {
                    lblMessage.className = "ErrorText";
                    lblMessage.innerHTML = "Грешка при запис на данните";
                }
            }

            if (isDataValid) {
                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }
    }

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Определяне на ВО на заявка за окомплектоване с ресурс от резерва</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
       <fieldset style="width: 800px; padding: 0px;">
          <table class="InputRegion" style="width: 800px; padding: 10px; padding-top: 0px; margin-top: 0px;">
            <tr style="height: 25px;">
                <td colspan="6" style="text-align: left;">
                    <span style="color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Заявка</span>
                </td>
            </tr>
            <tr style="height: 25px;">
                <td style="text-align: left; width: 10%;">
                    <asp:Label ID="lblRequestNumber" runat="server" CssClass="InputLabel" Text="Заявка №:"></asp:Label>
                </td>
                <td style="text-align: left; width: 12%;">
                    <asp:Label ID="txtRequestNumber" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
                <td style="text-align: left; width: 8%;">
                    <asp:Label ID="lblRequestDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>
                </td>
                <td style="text-align: left; width: 12%;">
                   <asp:Label ID="txtRequestDate" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
                <td style="text-align: right; width: 31%;">
                    <asp:Label ID="lblEquipWithResRequestsStatus" runat="server" CssClass="InputLabel" Text="Статус на заявката:"></asp:Label>
                </td>
                <td style="text-align: left; width: 27%;">
                    <asp:Label ID="txtEquipWithResRequestsStatus" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>               
            </tr>
            <tr style="height: 25px;">
                <td style="text-align: left;" colspan="4">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel" Text="Заявката е от ВПН/Структура:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtMilitaryUnit" runat="server" CssClass="ReadOnlyValue" ></asp:Label>
                            </td>  
                        </tr>
                    </table>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblAdministration" runat="server" CssClass="InputLabel" Text="От кое министерство/ведомство:"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:Label ID="txtAdministration" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
            </tr>
          </table>
        </fieldset>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
       <fieldset style="width: 800px; padding: 0px;">
          <table class="InputRegion" style="width: 800px; padding: 10px; padding-top: 0px; margin-top: 0px;">
            <tr style="height: 25px;">
                <td colspan="6" style="text-align: left;">
                    <span style="color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Команда</span>
                </td>
            </tr>
            <tr style="height: 25px;">
                <td style="text-align: left; width: 10%;">
                    <asp:Label ID="lblMilitaryCommand" runat="server" CssClass="InputLabel" Text="Команда:"></asp:Label>
                </td>
                <td style="text-align: left; width: 29%;">
                    <asp:DropDownList runat="server" ID="ddMilitaryCommand" UnsavedCheckSkipMe="true" CssClass="InputField" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryCommand_Changed"></asp:DropDownList>
                </td>
                <td style="text-align: rigth; width: 16%;">
                    <asp:Label ID="lblTime" runat="server" CssClass="InputLabel" Text="Време за явяване:"></asp:Label>
                </td>
                <td style="text-align: left; width: 10%;">
                    <asp:Label ID="txtTime" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
                <td style="text-align: right; width: 10%;">
                    <asp:Label ID="lblReadiness" runat="server" CssClass="InputLabel" Text="Готовност:"></asp:Label>
                </td>
                <td style="text-align: left; width: 25%;">
                    <asp:Label ID="txtReadiness" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
            </tr>
            <tr style="height: 25px;">
                <td style="text-align: left;" colspan="6">
                    <table cellpadding="0" cellspacing="0">
                        <td>
                            <asp:Label ID="lblDeliveryLocation" runat="server" CssClass="InputLabel" Text="Място за доставяне:"></asp:Label>
                        </td>
                        <td style="padding-left: 3px;">
                            <asp:Label ID="txtDeliveryLocation" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                        </td>
                    </table>
                </td>
            </tr>
          </table>
       </fieldset>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 850px; margin: 0 auto;">
       <div runat="server" id="pnlDataGrid" style="text-align: center;" align="center"></div>
    </div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 30px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="SaveReservistsCount(); return false;"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<div id="AddMilitaryDepartmentLightBox" class="AddMilitaryDepartmentLightBox" style="display: none; text-align: center;">
        <center>
            <table width="80%" style="text-align: center;">
                <colgroup style="width: 40%">
                </colgroup>
                <colgroup style="width: 60%">
                </colgroup>
                <tr style="height: 15px">
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <span class="HeaderText" style="text-align: center;">Избор на военно окръжие</span>
                    </td>
                </tr>
                <tr style="height: 15px">
                </tr>
                <tr style="min-height: 17px">
                    <td style="text-align: right;">
                        <span id="lblSelectMilitaryDepartment" class="InputLabel">Военно окръжие:</span>
                    </td>
                    <td style="text-align: left;">
                        <select id="ddMilitaryDepartments" UnsavedCheckSkipMe="true" class="RequiredInputField" style="width: auto;" ></select>
                    </td>
                </tr>                      
                <tr style="height: 35px">
                    <td colspan="2" style="padding-top: 5px;">
                        <span id="spanAddMilitaryDepartmentLightBoxMessage" class="ErrorText" style="display: none;">
                        </span>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <table style="margin: 0 auto;">
                            <tr>
                                <td>
                                    <div id="btnSaveAddMilitaryDepartmentLightBox" style="display: inline;" onclick="SaveAddMilitaryDepartmentLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnSaveAddMilitaryDepartmentLightBoxText" style="width: 70px;">Добавяне</div>
                                        <b></b>
                                    </div>
                                    <div id="btnCloseAddMilitaryDepartmentLightBox" style="display: inline;" onclick="HideAddMilitaryDepartmentLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnCloseAddMilitaryDepartmentLightBoxText" style="width: 70px;">Затвори</div>
                                        <b></b>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>

<asp:LinkButton ID="btnRefresh" runat="server" CssClass="HiddenButton" OnClick="ddMilitaryCommand_Changed"></asp:LinkButton>

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
