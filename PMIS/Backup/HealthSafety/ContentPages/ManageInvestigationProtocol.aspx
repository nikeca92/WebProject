<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ManageInvestigationProtocol.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.ManageInvestigationProtocol"
    Title="Протоколи за резултатите от злополука" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">

function DeleteInvestigationProtocol(investigaitonProtocolId, investigaitonProtocolNumber) 
{
    YesNoDialog('Желаете ли да изтриете протокол ' + investigaitonProtocolNumber + '?', ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageInvestigationProtocol.aspx?AjaxMethod=DeleteInvestigationProtocol";
        var params = "";
        params += "investigaitonProtocolId=" + investigaitonProtocolId;
        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}
function response_handler(xml)
{
    if (xmlNodeText(xml.childNodes[0]) == "ERROR")
   {
	 alert("There was a server problem!");
   }
   else
   {
       document.getElementById("<%=hdnResultStatus.ClientID %>").value = xmlNodeText(xml.childNodes[0]);
       document.getElementById("<%=hdnResultMessage.ClientID %>").value = xmlNodeText(xml.childNodes[1]);

	  document.getElementById("<%= btnRefresh.ClientID %>").click();
   }
}

function SortTableBy(sort)
{
   if(document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
   {
      sort = sort + 4;            
   }
      document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;  
      document.getElementById("<%= hdnPageIdx.ClientID %>").value = 1; //when sort -> goto to 1st page                          
      document.getElementById("<%= btnRefresh.ClientID %>").click();
}
  
function ClearHiddenFields()
{
  	document.getElementById("<%=hdnResultStatus.ClientID %>").value="";
    document.getElementById("<%=hdnResultMessage.ClientID %>").value="";
}

function ShowPrintAllInvestigationProtocols()
{
    var hdnInvestProtNumber = document.getElementById("<%= hdnInvestProtNumber.ClientID %>").value;
    var hdnInjuredName = document.getElementById("<%= hdnInjuredName.ClientID %>").value;
    var hdnProtDateFrom = document.getElementById("<%= hdnProtDateFrom.ClientID %>").value;
    var hdnProtDateTo = document.getElementById("<%= hdnProtDateTo.ClientID %>").value;
    var hdnAccidentDateFrom = document.getElementById("<%= hdnAccidentDateFrom.ClientID %>").value;
    var hdnAccidentDateTo = document.getElementById("<%= hdnAccidentDateTo.ClientID %>").value;
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAllInvestigationProtocols"
    var param = "";
    
    url = "../PrintContentPages/" + pageName + ".aspx?InvestProtocolNumber=" + hdnInvestProtNumber
                + "&InjuredName=" + hdnInjuredName + "&ProtocolDateFrom=" + hdnProtDateFrom
                + "&ProtocolDateTo=" + hdnProtDateTo + "&AccidentDateFrom=" + hdnAccidentDateFrom
                + "&AccidentDateTo=" + hdnAccidentDateTo + "&SortBy=" + hdnSortBy;
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_Click" />
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hdnResultStatus" runat="server" />
            <asp:HiddenField ID="hdnResultMessage" runat="server" />
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:HiddenField ID="hdnSortBy" runat="server" />
            <asp:HiddenField ID="hdnInvestProtNumber" runat="server" />
            <asp:HiddenField ID="hdnInjuredName" runat="server" />
            <asp:HiddenField ID="hdnProtDateFrom" runat="server" />
            <asp:HiddenField ID="hdnProtDateTo" runat="server" />
            <asp:HiddenField ID="hdnAccidentDateFrom" runat="server" />
            <asp:HiddenField ID="hdnAccidentDateTo" runat="server" />
            <input type="hidden" id="CanLeave" value="true" />
            <div style="height: 30px;">
            </div>
            <center>
                <span class="HeaderText">Протоколи за резултатите от злополука</span>
                <br />
                <br />
                <div class="FilterArea" style="padding-bottom: 0px; width: 730px;">
                    <div class="FilterLegend">Филтър</div>
                    <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                        <tr style="height: 30px">
                            <td align="left">
                                <span class="InputLabel" style="padding-left: 35px">№ Протокол:</span>
                                <asp:TextBox ID="txtInvestigaitonProtocolNumber" Width="100px" CssClass="InputField"
                                    MaxLength="10" runat="server"></asp:TextBox>
                                <span class="InputLabel" style="padding-left: 20px">Име на пострадалия:</span>
                                <asp:TextBox ID="txtWorkerFullName" Width="300px" CssClass="InputField" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td align="left">
                                <span class="InputLabel" style="padding-left: 35px">Дата на протокола от:</span>
                                <asp:TextBox ID="txtInvProtDateFrom" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                                <span class="InputLabel" style="padding-left: 10px">до</span>
                                <asp:TextBox ID="txtInvProtDateTo" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td align="left">
                                <span class="InputLabel" style="padding-left: 35px">Дата на злополуката от:</span>
                                <asp:TextBox ID="txtAccDateTimeFrom" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                                <span class="InputLabel" style="padding-left: 10px">до</span>
                                <asp:TextBox ID="txtAccDateTimeTo" MaxLength="10" Width="75px" CssClass="InputField"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td align="center">
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="Button" OnClientClick="ClearHiddenFields()"
                                    OnClick="btnSearch_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                <div style="padding-left: 30px; display: inline">
                                </div>
                                <asp:LinkButton ID="btnPrintAllInvestigationProtocols" runat="server" CssClass="Button" OnClientClick="ShowPrintAllInvestigationProtocols(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="ErrorText" ID="lblMessageValidation" runat="server" Text="TTTT"></asp:Label>
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
                        <div style="display: inline; position: relative; top: -16px; padding-left: 20px">
                        </div>
                        <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:95px; padding-left:5px;">Нов протокол</div><b></b></asp:LinkButton>
                        <div style="display: inline; position: relative; top: -16px; padding-left: 120px">
                            <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Първа страница" CssClass="PaginationButton"
                                OnClick="btnFirst_Click" />
                            <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Предишна страница" CssClass="PaginationButton"
                                OnClick="btnPrev_Click" />
                            <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
                            <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Следваща страница" CssClass="PaginationButton"
                                OnClick="btnNext_Click" />
                            <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Последна страница" CssClass="PaginationButton"
                                OnClick="btnLast_Click" />
                            <span style="min-width: 100px; padding: 45px">&nbsp;</span> <span style="text-align: right;">
                                Отиди на страница</span>
                            <asp:TextBox ID="tbxGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
                            <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png"
                                OnClientClick="ClearHiddenFields()" AlternateText="Отиди на страница" CssClass="PaginationButton"
                                OnClick="btnGoto_Click" />
                        </div>
                    </div>
                </div>
                <%--<div style="height: 20px;">
                </div>--%>
                <div id="protocolsTableDiv" align="center" runat="server">
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center; min-height:15px">
                    <asp:Label ID="lblMessageGrid" runat="server"></asp:Label>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
