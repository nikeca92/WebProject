<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportPostponeTechByAdministration.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportPostponeTechByAdministration" 
         Title="Отчет за изпълнение отсрочването на техника" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style>
    .SummaryStatsTable
    {
        border-collapse: collapse;
    }
    
    .SummaryStatsTable td
    {
        border: solid 1px #999999;
        padding: 3px;
    }
</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>
<script src="../Scripts/PickList.js" type="text/javascript"></script>

<script type="text/javascript">
window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

//Call this function when the page is loaded
function PageLoad()
{
    LoadPickLists();

    if (document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value != "")
        PickListUtil.SetSelection("pickListMilitaryDepartments", document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value);
}

function EndRequestHandlerPage(sender, args) {
    ReLoadPickLists();
}

function LoadPickLists()
{
    var configPickListMilitaryDepartments =
    {
        width: 275,
        allLabel: "<Всички>",
        onHide: function() {
            SetPickListsSelection();
            PickListMilitaryDepartments_Changed();
        }
    }

    var militaryDepartments = document.getElementById("<%= hdnMilitaryDepartmentJson.ClientID %>").value;
    var militaryDepartments = eval(militaryDepartments);
    PickListUtil.AddPickList("pickListMilitaryDepartments", militaryDepartments, "tdPickListMilitaryDepartments", configPickListMilitaryDepartments);
}

function ReLoadPickLists()
{
}

function SetPickListsSelection()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryDepartments");
    
    document.getElementById("<%= hdnMilitaryDepartmentSelectedText.ClientID %>").value = PickListUtil.GetSelectedText("pickListMilitaryDepartments");    
}

function PickListMilitaryDepartments_Changed() {
    document.getElementById("<%= btnPickListMilitaryDepartmentsChanged.ClientID %>").click();
}

function IsFilterDataValid() {
    var isValid = true;
    var errorMsg = "";

    if (!isValid) {
        document.getElementById("<%= lblFilterMessage.ClientID %>").innerHTML = errorMsg;
        document.getElementById("<%= lblFilterMessage.ClientID %>").className = "ErrorText";
    }
    else {
        document.getElementById("<%= lblFilterMessage.ClientID %>").innerHTML = "";
        document.getElementById("<%= lblFilterMessage.ClientID %>").className = "";
    }

    return isValid;
}

</script>

<!-- 
The filter area contains PickList fields and to prevent issues caused by UpdatePanel reloads that is why the filter area
is out of the update panel. The update panel contains only the results grid.
The picklists are re-built when the page is loaded. When the various drop-downs get changed then a post-back is fired to reload
the reladed lists on the server and then the page is reloaded. Then the picklists are refreshed (and we keep the selection in hidden fields).
-->

<div style="text-align: center; padding-top: 20px;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Отчет за изпълнение отсрочването на техника</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="vertical-align: middle; text-align: right; width: 120px;">
                               <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Военно окръжие:</asp:Label>
                               <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentJson" />
                               <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentSelected" />
                               <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentSelectedText" />
                               <asp:Button runat="server" ID="btnPickListMilitaryDepartmentsChanged" OnClick="btnPickListMilitaryDepartmentsChanged_Clicked" />
                            </td>
                            <td style="text-align: left; vertical-align: top; width: 320px;">
                               <div id="tdPickListMilitaryDepartments"></div>
                            </td>
                            <td style="vertical-align: middle; text-align: right; width: 65px;">                                      
                               <asp:Label runat="server" ID="lblPostponeYear" CssClass="InputLabel">Година:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: middle;">
                               <asp:DropDownList runat="server" ID="ddPostponeYears" CssClass="InputField" Width="70px"
                                    OnSelectedIndexChanged="ddPostponeYears_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
             </tr>
             <tr style="height: 10px;">
                 <td colspan="4"></td>
             </tr>
             <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="vertical-align: middle; text-align: right; width: 120px;">
                                <asp:Label runat="server" ID="lblRegion" CssClass="InputLabel">Област:</asp:Label> 
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:DropDownList runat="server" ID="ddRegion" CssClass="InputField" Width="170px" 
                                     OnSelectedIndexChanged="ddRegion_Changed" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: middle; text-align: right;">
                                <asp:Label runat="server" ID="lblMuniciplaity" CssClass="InputLabel" Width="80px">Община:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="170px"
                                    OnSelectedIndexChanged="ddMuniciplaity_Changed" AutoPostBack="true"></asp:DropDownList>
                            </td>                           
                            <td style="vertical-align: middle; text-align: right;">
                               <asp:Label runat="server" ID="lblCity" CssClass="InputLabel" Width="110px">Населено място:</asp:Label> 
                            </td>
                            <td style="vertical-align: bottom; text-align: left;">
                               <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="170px" 
                                    OnSelectedIndexChanged="ddCity_Changed" AutoPostBack="true"></asp:DropDownList>             
                            </td>
                        </tr>
                    </table>
                 </td>
             </tr>
             
             <tr style="height: 10px;">
                 <td colspan="4"></td>
             </tr>
             <tr >
                <td colspan="4" style="padding-top: 10px; padding-bottom: 10px;">
                    <asp:Label ID="lblFilterMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="4" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline">
                        </div>
                        <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClick="btnExcel_Click" ToolTip="Експортиране на резултатите към Excel" ><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
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
</Triggers>
<ContentTemplate>

<div style="height: 30px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;">
    <div style="width: 900px; margin: 0 auto;">
       <div runat="server" id="pnlReportGrid" style="text-align: center;"></div>
    </div>
</div>

<div style="text-align: center; padding-top: 15px;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
