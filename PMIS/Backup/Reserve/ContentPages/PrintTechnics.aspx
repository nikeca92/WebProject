<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintTechnics.aspx.cs" Inherits="PMIS.Reserve.ContentPages.PrintTechnics" 
         Title="Печат на документи за техника" %>

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
	width: 1100px;
}

#SubShadowContainer
{
	width: 1102px;
}

.UncheckedRow
{
    color: #BBBBBB;
    font-style: italic;
}

</style>

<script type="text/javascript">

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

}

function SetPickListsSelection()
{
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryDepartments");
}

function ReloadMilitaryCommands() {
    document.getElementById("<%= hdnBtnReloadMilitaryCommands.ClientID %>").click();
}


//Function that sorts the table by a specific column
function SortTableBy(sort) {
    //If sorting by the same column them set the direction to be DESC
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort) {
        sort = sort + 100;
    }

    //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;
    document.getElementById("<%= btnRefresh.ClientID %>").click();
}

function CheckAll() {
    var checked = document.getElementById("chkAll").checked;
    var rowsCount = parseInt(document.getElementById("hdnRowsCount").value);

    for (var i = 1; i <= rowsCount; i++) {
        if (document.getElementById("chkRow" + i).checked && !checked)
            document.getElementById("row" + i).className += " UncheckedRow";

        if (!document.getElementById("chkRow" + i).checked && checked)
            document.getElementById("row" + i).className = document.getElementById("row" + i).className.replace(/(?:^|\s)UncheckedRow(?!\S)/g, '');
    
        document.getElementById("chkRow" + i).checked = checked;
    }

    document.getElementById("chkAll").title = checked ? "Прехамни всички" : "Избери всички";

    var lblSelectedRowsCount = document.getElementById("lblSelectedRowsCount");
    lblSelectedRowsCount.innerHTML = checked ? rowsCount : "0";
}

function CheckRow(rowIdx) {
    var checked = document.getElementById("chkRow" + rowIdx).checked;

    if (!checked) {
        document.getElementById("chkAll").checked = false;
        document.getElementById("chkAll").title = "Избери всички";

        document.getElementById("row" + rowIdx).className += " UncheckedRow";

        var lblSelectedRowsCount = document.getElementById("lblSelectedRowsCount");
        var selectedCount = parseInt(lblSelectedRowsCount.innerHTML);
        selectedCount--;
        lblSelectedRowsCount.innerHTML = selectedCount;
    }
    else {
        var rowsCount = parseInt(document.getElementById("hdnRowsCount").value);
        var checkedCount = 0;

        for (var i = 1; i <= rowsCount; i++) {
            if (document.getElementById("chkRow" + i).checked)
                checkedCount++;
        }

        var allChecked = (rowsCount == checkedCount);
        document.getElementById("chkAll").checked = allChecked;
        document.getElementById("chkAll").title = allChecked ? "Прехамни всички" : "Избери всички";

        document.getElementById("row" + rowIdx).className = document.getElementById("row" + rowIdx).className.replace(/(?:^|\s)UncheckedRow(?!\S)/g, '');

        var lblSelectedRowsCount = document.getElementById("lblSelectedRowsCount");
        var selectedCount = parseInt(lblSelectedRowsCount.innerHTML);
        selectedCount++;
        lblSelectedRowsCount.innerHTML = selectedCount;
    }
}

function Checkbox_Clicked(rowIdx) {
    CheckRow(rowIdx);
}

function Row_Clicked(rowIdx) {
    var checkbox = document.getElementById("chkRow" + rowIdx);
    checkbox.checked = !checkbox.checked;
    CheckRow(rowIdx);
}

function btnPrintMK_Click() {
    if (document.getElementById("hdnRowsCount")) {
        var rowsCount = parseInt(document.getElementById("hdnRowsCount").value);
        var selectedTechnicsIDs = "";

        for (var i = 1; i <= rowsCount; i++) {
            if (document.getElementById("chkRow" + i).checked) {
                var technicsId = document.getElementById("hdnTechnicsId" + i).value;
                selectedTechnicsIDs += (selectedTechnicsIDs == "" ? "" : ",") + technicsId;
            }
        }

        document.getElementById("<%= hdnSelectedTechnicsIDs.ClientID %>").value = selectedTechnicsIDs;
        document.getElementById("<%= btnPrintMKSrv.ClientID %>").click();
    }
}

function btnPrintPZ_Click() {
    if (document.getElementById("hdnRowsCount")) {
        var rowsCount = parseInt(document.getElementById("hdnRowsCount").value);
        var selectedTechnicsIDs = "";

        for (var i = 1; i <= rowsCount; i++) {
            if (document.getElementById("chkRow" + i).checked) {
                var technicsId = document.getElementById("hdnTechnicsId" + i).value;
                selectedTechnicsIDs += (selectedTechnicsIDs == "" ? "" : ",") + technicsId;
            }
        }

        document.getElementById("<%= hdnSelectedTechnicsIDs.ClientID %>").value = selectedTechnicsIDs;
        document.getElementById("<%= btnPrintPZSrv.ClientID %>").click();
    }
}

function btnClear_Click() {
    document.getElementById("<%= hdnMilitaryDepartmentSelected.ClientID %>").value = "";
    PickListUtil.ClearSelection("pickListMilitaryDepartments");
}

</script>


<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Печат на документи за техника</span>
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
                    <asp:Label ID="lblRegNumber" runat="server" CssClass="InputLabel" Text="Рег. номер:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 365px;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:TextBox runat="server" ID="txtRegNumber" CssClass="InputField" Width="600px"></asp:TextBox> 
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             
             
             
             
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 120px;">
                    <asp:Label ID="Label1" runat="server" CssClass="InputLabel" Text="Вид резерв:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 365px;">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddReadiness" CssClass="InputField" Width="165px"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>  
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
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
                        <asp:LinkButton ID="btnPrintMK" runat="server" CssClass="Button" OnClientClick="btnPrintMK_Click(); return false;"><i></i><div style="width:90px; padding-left:5px;">Печат на МК</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnPrintPZ" runat="server" CssClass="Button" OnClientClick="btnPrintPZ_Click(); return false;"><i></i><div style="width:90px; padding-left:5px;">Печат на ПЗ</div><b></b></asp:LinkButton>
                        
                        <asp:Button ID="btnPrintMKSrv" runat="server" OnClick="btnPrintMK_Click" />
                        <asp:Button ID="btnPrintPZSrv" runat="server" OnClick="btnPrintPZ_Click" />
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

<div style="text-align: center;" runat="server" id="pnlPaging" visible="false">
    <div style="width: 470px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
       <span style="padding: 10px">&nbsp;</span>
    
       <div style="display: inline; position: relative; top: -16px;">
          <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png" AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" />                        
          <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png" AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" />                        
          <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
          <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png" AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" />                        
          <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png" AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" />            
          <span style="padding: 30px">&nbsp;</span>
          <span style="text-align: right;">Отиди на страница</span>
          <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
          <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png" AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" /> 
       </div>
    </div>
</div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;">
    <div style="width: 1050px; margin: 0 auto;">
       <div runat="server" id="pnlResultsGrid" style="text-align: center;"></div>
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
<asp:HiddenField ID="hdnIdentNumber" runat="server" />
<asp:HiddenField ID="hdnReadiness" runat="server" />
<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />
<asp:HiddenField ID="hdnSelectedTechnicsIDs" runat="server" />

<asp:Button ID="hdnBtnReloadMilitaryCommands" runat="server" OnClick="hdnBtnReloadMilitaryCommands_Click" style="display: none;" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
