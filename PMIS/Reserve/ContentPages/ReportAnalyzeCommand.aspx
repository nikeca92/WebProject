<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportAnalyzeCommand.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportAnalyzeCommand" 
         Title="Сведение - анализ за комплектуването на команда" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>
<script src="../Scripts/PickList.js" type="text/javascript"></script>

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
	width: 1250px;
}

#SubShadowContainer
{
	width: 1252px;
}

</style>

<script type="text/javascript">
function ShowPrintReport()
{
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value;
    var ddMilitaryReadinessID = document.getElementById("<%= ddMilitaryReadiness.ClientID %>").value; 
    var ddMilitaryCommandId = document.getElementById("<%= ddMilitaryCommand.ClientID %>").value;
    var ddMilitaryCommandSuffix = document.getElementById("<%= ddSubMilitaryCommand.ClientID %>").value;
    var hdnReportType = document.getElementById("<%= hdnReportTypeSelected.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportAnalyzeCommand"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                                                + "&MilitaryReadinessID=" + ddMilitaryReadinessID
                                                + "&MilitaryCommandId=" + ddMilitaryCommandId
                                                + "&MilitaryCommandSuffix=" + ddMilitaryCommandSuffix
                                                + "&ReportType=" + hdnReportType;                                                                
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function ExportReport() {
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value;
    var ddMilitaryReadinessID = document.getElementById("<%= ddMilitaryReadiness.ClientID %>").value; 
    var ddMilitaryCommandId = document.getElementById("<%= ddMilitaryCommand.ClientID %>").value;
    var ddMilitaryCommandSuffix = document.getElementById("<%= ddSubMilitaryCommand.ClientID %>").value;
    var hdnReportType = document.getElementById("<%= hdnReportTypeSelected.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportAnalyzeCommand"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                                            + "&MilitaryReadinessID=" + ddMilitaryReadinessID
                                            + "&MilitaryCommandId=" + ddMilitaryCommandId
                                            + "&MilitaryCommandSuffix=" + ddMilitaryCommandSuffix
                                            + "&ReportType=" + hdnReportType + "&Export=true";

    window.location = url;
}

window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

//Call this function when the page is loaded
function PageLoad()
{
    LoadPickLists();
}


function LoadPickLists()
{
    var configPickListMilitaryDepartments =
    {
        width: 340,
        allLabel: "<Всички>",
        onHide: function() {
            SetPickListsSelection();
            ReloadMilitaryCommands();
        }
    }

    militaryDepartments = document.getElementById("<%= hdnMilitaryDepartmentJson.ClientID %>").value;
    militaryDepartments = eval(militaryDepartments);
    PickListUtil.AddPickList("pickListMilitaryDepartments", militaryDepartments, "tdPickListMilitaryDepartments", configPickListMilitaryDepartments);

    var configPickListReportType =
    {
        width: 340,
        allLabel: "<Всички>",
        onHide: function() {
            SetPickListsSelection();
        }
    }

    reportType = document.getElementById("<%= hdnReportTypeJson.ClientID %>").value;
    reportType = eval(reportType);
    PickListUtil.AddPickList("pickListReportType", reportType, "tdPickListReportType", configPickListReportType);
}

function SetPickListsSelection()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryDepartments");
    document.getElementById("<%= hdnReportTypeSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListReportType");
}

function ReloadMilitaryCommands() {
    document.getElementById("<%= hdnBtnReloadMilitaryCommands.ClientID %>").click();
}

function btnClear_Click()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = "";
    PickListUtil.ClearSelection("pickListMilitaryDepartments");
}

</script>


<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Сведение - анализ за комплектуването на команда</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">                                      
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Военно окръжие:</asp:Label>
                   <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentJson" />
                   <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentSelected" />
                </td>
                <td style="text-align: left; vertical-align: top; width: 340px;">
                   <div id="tdPickListMilitaryDepartments"></div>
                </td>                
             </tr>             
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                 <td style="vertical-align: middle; text-align: right; width: 120px;">  
                    <asp:Label ID="lblMilitaryReadiness" runat="server" CssClass="InputLabel" Text="Готовност:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 365px;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddMilitaryReadiness" CssClass="InputField" Width="365px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryReadiness_Changed"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>                        
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">  
                    <asp:Label ID="lblMilitaryCommand" runat="server" CssClass="InputLabel" Text="Команда:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 365px;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddMilitaryCommand" CssClass="InputField" Width="365px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryCommand_Changed"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>                        
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">
                    <asp:Label ID="lblSubMilitaryCommand" runat="server" CssClass="InputLabel" Text="Подкоманда:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 365px;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddSubMilitaryCommand" CssClass="InputField" Width="365px"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>  
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">                                      
                   <asp:Label runat="server" ID="lblReportType" CssClass="InputLabel">Справка:</asp:Label>
                   <asp:HiddenField runat="server" ID="hdnReportTypeJson" />
                   <asp:HiddenField runat="server" ID="hdnReportTypeSelected" />
                </td>
                <td style="text-align: left; vertical-align: top; width: 340px;">
                   <div id="tdPickListReportType"></div>
                </td>                
             </tr> 
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td colspan="2" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="2" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" OnClientClick="btnClear_Click();" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline">
                        </div>
                        <asp:LinkButton ID="btnPrintReport" runat="server" CssClass="Button" OnClientClick="ShowPrintReport(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                    </center>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<Triggers>
   <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
</Triggers>
<ContentTemplate>

<div style="height: 30px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;">
    <div style="width: 1050px; margin: 0 auto;">
       <div runat="server" id="pnlReportGrid" style="text-align: center;"></div>
    </div>
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

<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnMilitaryCommandId" runat="server" />
<asp:HiddenField ID="hdnSubMilitaryCommandId" runat="server" />
<asp:HiddenField ID="hdnReportType" runat="server" />

<asp:Button ID="hdnBtnReloadMilitaryCommands" runat="server" OnClick="hdnBtnReloadMilitaryCommands_Click" style="display: none;" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
