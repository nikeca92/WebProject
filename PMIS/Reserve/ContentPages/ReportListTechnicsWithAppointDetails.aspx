<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportListTechnicsWithAppointDetails.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportListTechnicsWithAppointDetails" 
         Title="Списък на техниката с МН по определена заявка" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на техниката с МН по определена заявка</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <fieldset style="width: 930px; padding-bottom: 0px;">
       <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;">Заявка</legend>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D; border-spacing: 2px;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="text-align: left; width: 8%;">
                    <asp:Label ID="lblRequestNumber" runat="server" CssClass="InputLabel" Text="Заявка №:"></asp:Label>
                </td>
                <td style="text-align: left; width: 32%;">
                    <asp:Label ID="lblRequestNumberValue" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblRequestDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>
                    <asp:Label ID="lblRequestDateValue" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
                
                <td style="text-align: right; width: 31%;">
                    <asp:Label ID="lblEquipWithTechRequestsStatus" runat="server" CssClass="InputLabel" Text="Статус на заявката:"></asp:Label>
                </td>
                <td style="text-align: left; width: 25%;">
                    <asp:Label ID="lblEquipWithTechRequestsStatusValue" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>                                    
            </tr>
            <tr>
                <td style="text-align: left;" colspan="2">
                    <table cellpadding="0" cellspacing="0">
                       <tr>
                          <td nowrap="nowrap">
                             <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel" Text=""></asp:Label>
                          </td>
                          <td style="padding-left: 3px;">
                             <asp:Label ID="lblMilitaryUnitValue" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                          </td>
                       </tr>
                    </table>
                </td>
                
                <td style="text-align: right;">
                    <asp:Label ID="lblAdministration" runat="server" CssClass="InputLabel" Text="От кое министерство/ведомство:"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:Label ID="lblAdministrationValue" runat="server" CssClass="ReadOnlyValue"></asp:Label>
                </td>
            </tr>
            <tr style="height: 10px;">
               <td></td>
            </tr>
          </table>
        </fieldset>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 950px; margin: 0 auto;">
       <div runat="server" id="pnlItemsGrid" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
   <asp:LinkButton ID="btnPrint" runat="server" CssClass="Button" OnClientClick="ShowPrintReportListTechnicsWithAppointDetails(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
   <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<input type="hidden" id="CanLeave" value="true" />

<asp:HiddenField ID="hfEquipmentTechnicsRequestID" runat="server" />

</ContentTemplate>
 </asp:UpdatePanel>
 
 
 <script type="text/javascript">
     function ShowPrintReportListTechnicsWithAppointDetails()
     {
         var EquipmentTechnicsRequestID = document.getElementById("<%= hfEquipmentTechnicsRequestID.ClientID %>").value;
         
         var url = "";
         var pageName = "PrintReportListTechnicsWithAppointDetails"
         var param = "";

         url = "../PrintContentPages/" + pageName + ".aspx?EquipmentTechnicsRequestID=" + EquipmentTechnicsRequestID;

         var uplPopup = window.open(url, pageName, param);

         if (uplPopup != null)
             uplPopup.focus();
     }


     function ExportReport() {
         var EquipmentTechnicsRequestID = document.getElementById("<%= hfEquipmentTechnicsRequestID.ClientID %>").value;

         var url = "";
         var pageName = "PrintReportListTechnicsWithAppointDetails"
         var param = "";

         url = "../PrintContentPages/" + pageName + ".aspx?EquipmentTechnicsRequestID=" + EquipmentTechnicsRequestID + "&Export=true";

         window.location = url;
     }
    
     // Open in new window AddEditTechnics page in read only mode
     function PreviewTechnics(technicsId) {
         var url = "";
         var pageName = "AddEditTechnics"
         var param = "";

         url = "../ContentPages/" + pageName + ".aspx?TechnicsId=" + technicsId + "&Preview=1";

         var uplPopup = window.open(url, pageName, param);

         if (uplPopup != null)
             uplPopup.focus();
     }
 </script>
 
 </asp:Content>
