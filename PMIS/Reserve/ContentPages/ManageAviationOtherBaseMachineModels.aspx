<%@ Page Title="Модели базови машини (спец. инж. и самолетообслужваща техника)" Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageAviationOtherBaseMachineModels.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ManageAviationOtherBaseMachineModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">

    .AviationOtherBaseMachineModelLightBox
    {
	    width: 400px;
	    background-color: #EEEEEE;
	    border: solid 1px #000000;
	    position: fixed;
	    top: 200px;
	    left: 37%;
	    z-index: 1000;
	    padding: 0 10px;
    }

    </style>

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/ManageAviationOtherBaseMachineModels.js'></script>


<script type="text/javascript">

    var ddlAviationOtherBaseMachineMakesClientID = "<%= ddlAviationOtherBaseMachineMakes.ClientID %>";
    var btnRefreshClientID = "<%= btnRefresh.ClientID %>";
    var hdnRefreshReasonClientID = "<%= hdnRefreshReason.ClientID %>";
    var hdnSortByClientID = "<%= hdnSortBy.ClientID %>";
    var hdnPageIdxClientID = "<%= hdnPageIdx.ClientID %>";

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Модели базови машини (спец. инж. и самолетообслужваща техника)</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 480px; text-align: center; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td colspan="3"></td>
             </tr>
             <tr style="height: 25px;">
                <td style="vertical-align: top; text-align: right; width: 35%;">
                    <asp:Label runat="server" ID="lblAviationOtherBaseMachineMake" CssClass="InputLabel">Марка базова машина:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 45%;">
                    <asp:DropDownList runat="server" ID="ddlAviationOtherBaseMachineMakes" CssClass="InputField" Width="200px"
                        OnSelectedIndexChanged="ddlAviationOtherBaseMachineMakes_Change" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td style="text-align: left; vertical-align: top; width: 20%;">
                    <asp:ImageButton ID="imgBtnNewModel" runat="server" ImageUrl="../Images/add_new.png" 
                        AlternateText="Добавяне на нов модел базова машина (спец. инж. и самолетообслужваща техника)" ToolTip="Добавяне на нов модел базова машина (спец. авиационна техника)" 
                        OnClientClick="ShowAviationOtherBaseMachineModelLightBox(0); return false;" />
                </td>
             </tr> 
             <tr>
                <td colspan="3" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr style="height: 25px; display: none;">
                <td colspan="3" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                </td>
             </tr>             
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 500px; margin: 0 auto; text-align: center; vertical-align: top; height: 35px;">      
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

<div style="text-align: center;">
    <div style="width: 950px; margin: 0 auto;">
       <div runat="server" id="pnlDataGrid" style="text-align: center;"></div>
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

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

    <div id="AviationOtherBaseMachineModelLightBox" class="AviationOtherBaseMachineModelLightBox" style="display: none; text-align: center;">
        <input type='hidden' id='hdnAviationOtherBaseMachineModelId' name='hdnAviationOtherBaseMachineModelId' />
        <table style='text-align:center;'>
            <tr style='height: 15px'><td colspan="2"></td></tr>
            <tr>
                <td colspan='2' align='center'>
                    <span id='lblAviationOtherBaseMachineModelBoxTitle' class='SmallHeaderText'></span>
                </td>
            </tr>
            <tr style='height: 17px'><td colspan="2"></td></tr>
            <tr style='min-height: 17px;'>
                <td align='right' style='width: 160px;'>
                    <span id='lblAviationOtherBaseMachineMakeLightBox' class='InputLabel'>Марка:</span>
                </td>
                <td align='left' style='min-width: 220px;'>
                    <span id='lblAviationOtherBaseMachineMakeValue' class='ReadOnlyValue'></span>
                </td>
            </tr>      
            <tr style='min-height: 17px;'>
                <td align='right' style='width: 160px;'>
                    <span id='lblAviationOtherBaseMachineModelName' class='InputLabel'>Модел базова машина:</span>
                </td>
                <td align='left' style='min-width: 220px;'>
                    <input type='text' id='txtAviationOtherBaseMachineModelName' class='RequiredInputField' style='width: 200px;' maxlength='300' />
                </td>
            </tr>             
            <tr style='height: 30px'>
                <td colspan='2'> 
                    <span id='lblAviationOtherBaseMachineModelLightBoxMessage' class='ErrorText' style='display: none;'></span>
               </td>
            </tr>
            <tr>
                <td colspan='2' style='text-align: center;'>
                    <table style='margin: 0 auto;'>
                       <tr>
                          <td style='text-align: center;'>
                             <div id='btnSaveAviationOtherBaseMachineModel' style='display: inline;' onclick='SaveAviationOtherBaseMachineModel();' class='Button'><i></i><div id='btnSaveAviationOtherBaseMachineModelText' style='width:70px;'>Запис</div><b></b></div>
                             <div id='btnCloseAviationOtherBaseMachineModelBox' style='display: inline;' onclick='HideAviationOtherBaseMachineModelLightBox();' class='Button'><i></i><div id='btnCloseVehickleModelBoxText' style='width:70px;'>Затвори</div><b></b></div>
                          </td>
                       </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </div>

</ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
