<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="FulfilEquipmentReservistsRequest.aspx.cs" Inherits="PMIS.Reserve.ContentPages.FulfilEquipmentReservistsRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
    
.ViewFulfilmentLightBox
{
	min-width: 680px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 200px;
	left: 32%;	
	min-height: 200px;
	z-index: 1000;
	padding-top: 10px;
}
    
</style>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>
    <script src="../Scripts/FulfilEquipmentReservistsRequest.js" type="text/javascript"></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/ResFulfilmentRemoval.js'></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />
            <asp:HiddenField ID="hfMilitaryDepartmentID" runat="server" />
            <div style="height: 20px">
            </div>
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: center;">
                        <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <center>
                        <fieldset style="width: 750px; padding: 0px;">
                            <table class="InputRegion" style="width: 750px; padding: 10px; padding-top: 0px; margin-top: 0px;">
                                <tr>
                                    <td colspan="4" style="text-align: left;">
                                       <span style="color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Заявка</span>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 30%;">
                                        <asp:Label ID="lblRequestNumber" runat="server" CssClass="InputLabel" Text="Заявка №:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 16%;">
                                        <asp:Label ID="txtRequestNumber" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                                    </td>
                                    
                                    <td style="text-align: right; width: 30%;">
                                        <asp:Label ID="lblRequestDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 24%;">
                                        <asp:Label ID="txtRequestDate" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                                    </td>                                                                       
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 30%; vertical-align: top;">
                                       <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 16%;">
                                        <asp:Label ID="txtMilitaryUnit" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                                    </td>
                                    
                                    <td style="text-align: right; width: 30%; vertical-align: top;">
                                        <asp:Label ID="lblMilitaryDepartment" runat="server" CssClass="InputLabel" Text="Заявката се изпълнява от ВО:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 24%;">
                                        <asp:Label ID="txtMilitaryDepartment" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                                    </td>                                                                                                          
                                </tr>
                            </table>
                        </fieldset>
                        </center>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: center">
                        <center>                        
                            <fieldset style="width: 750px; padding: 0px;">
                                <table class="InputRegion" style="width: 750px; padding: 10px; padding-top: 0px; margin-top: 0px;">
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
                                        <td style="text-align: right; width: 20%;">
                                            <asp:Label ID="lblTime" runat="server" CssClass="InputLabel" Text="Време за явяване:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 12%;">
                                            <asp:Label ID="txtTime" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                                        </td>
                                        <td style="text-align: right; width: 10%;">
                                            <asp:Label ID="lblReadiness" runat="server" CssClass="InputLabel" Text="Готовност:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 19%;">
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
                        </center>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: center">
                        <center>                        
                            <fieldset style="width: 750px; padding: 0px;">
                                <table id="tblPunkt" class="InputRegion" style="width: 750px; padding: 10px; padding-top: 0px; margin-top: 0px;">
                                    <tr style="height: 25px;">
                                        <td colspan="5" style="text-align: left;">
                                            <span style="color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Пункт за извозване/КПП</span>
                                        </td>
                                        <td style="text-align: right; padding-bottom: 5px;">
                                           <div style="display: inline; position: relative; left: 10px;">
                                              <img runat="server" id="btnImgSave" src="../Images/save.png" alt="Запис" title="Запис" class='GridActionIcon' onclick='btnSavePunktClick();' />                                              
                                       </div>
                                    </td>
                                    </tr>
                                    <tr style="height: 25px;">
                                        <td style="text-align: left; width: 16%;">
                                            <asp:Label ID="lblRegion" runat="server" CssClass="InputLabel" Text="Област:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 29%;">
                                            <asp:DropDownList runat="server" ID="ddRegion" CssClass="InputField" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddRegion_Changed"></asp:DropDownList>
                                        </td>
                                        <td style="text-align: left; width: 10%;">
                                            <asp:Label ID="lblMuniciplaity" runat="server" CssClass="InputLabel" Text="Община:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 15%;">
                                            <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="220px" AutoPostBack="true" OnSelectedIndexChanged="ddMuniciplaity_Changed"></asp:DropDownList>
                                        </td>                                    
                                    </tr>
                                    <tr style="height: 25px;">
                                      <td style="text-align: left;">
                                            <asp:Label ID="lblCity" runat="server" CssClass="InputLabel" Text="Населено място:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="180px"></asp:DropDownList>
                                        </td>
                                        <td colspan="2" style="text-align: left;">
                                            <asp:TextBox ID="txtPlace" runat="server" CssClass="InputField" Width="290px"></asp:TextBox>
                                        </td>                                    
                                    </tr>
                                    <tr style="height: 25px;">
                                    <td colspan="4" style="text-align: center;">
                                        <asp:Label ID="lblPunktMessage" runat="server" Text=""></asp:Label>
                                     </td>
                                    </tr>
                                  </table>
                            </fieldset>                        
                        </center>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                    <center>
                        <div runat="server" id="pnlDataGrid" style="text-align: center;" align="center"></div>    
                    </center>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 25px;">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
             </table>
             
             <div style="height: 20px;"></div>             

            <div style="text-align: center;">
               <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
            </div>
            
            <div id="divViewFulfilmentLightBox" class="ViewFulfilmentLightBox" style="padding: 10px;
                display: none; text-align: center;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideViewFulfilmentLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divViewFulfilmentLightBoxContent">
                </div>
            </div>
            
            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="HiddenButton" OnClick="ddMilitaryCommand_Changed"></asp:LinkButton>

             <asp:HiddenField ID="hdnLocationHash" runat="server" />
             <asp:HiddenField ID="hdnSavedChanges" runat="server" />                          
             
         </ContentTemplate>
    </asp:UpdatePanel>               
   

    <script type="text/javascript">               
        var hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';        
        
        var hfEquipmentReservistsRequestID = "<%= hfEquipmentReservistsRequestID.ClientID %>";
        var hfMilitaryDepartmentID = "<%= hfMilitaryDepartmentID.ClientID %>";
        var ddMilitaryCommand = "<%= ddMilitaryCommand.ClientID %>";
        var ddCity = "<%= ddCity.ClientID %>";
        var txtPlace = "<%= txtPlace.ClientID %>";
        var lblPunktMessage = "<%= lblPunktMessage.ClientID %>";
        var btnRefresh = "<%= btnRefresh.ClientID %>";
    </script>

</asp:Content>
