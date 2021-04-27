<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchTechnics_TRACTORS.aspx.cs" Inherits="PMIS.Reserve.ContentPages.SearchTechnics_TRACTORS" 
         Title="Списък на техниката водена на военен отчет" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    
.SelectionItem
{
    
}

.SelectionItem:hover
{
    cursor: pointer;
    background-color: #8D98B6;
    color: #FFFFFF;    
}

.ShadowContainer
{
    width: 1000px;
}
    
</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
//Function that sorts the table by a specific column
function SortTableBy(sort)
{
    //If sorting by the same column them set the direction to be DESC
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
    {
        sort = sort + 100;
    }
    
    //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;                           
    document.getElementById("<%= btnRefresh.ClientID %>").click();               
}

// Save FillTechnicsRequest by AJAX call
function ChooseTractor(technicsId, tractorId) {
    YesNoDialog('Желаете ли избраният запис да бъде използван за попълване на заявката и да му бъде издадено МН?', ConfirmYes, null);

    function ConfirmYes() {

        var url = "SearchTechnics_TRACTORS.aspx?AjaxMethod=JSChooseTractor";
        var params = "";
        params += "TechnicsID=" + technicsId;
        params += "&TractorID=" + tractorId;
        params += "&TechnicsRequestCommandPositionID=" + document.getElementById("<%= hfTechnicsRequestCommandPositionID.ClientID %>").value;
        params += "&MilitaryDepartmentID=" + document.getElementById("<%= hfMilitaryDepartmentID.ClientID %>").value;
        params += "&Readiness=" + document.getElementById("<%= hfReadiness.ClientID %>").value;
        
        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                document.getElementById("<%= btnBack.ClientID %>").click();
            }
            else {
                var message = xmlValue(xml, "message");
                alert(message);
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:HiddenField ID="hfEquipmentTechnicsRequestID" runat="server" />
<asp:HiddenField ID="hfMilitaryDepartmentID" runat="server" />
<asp:HiddenField ID="hfMilitaryCommandID" runat="server" />
<asp:HiddenField ID="hfTechnicsRequestCommandPositionID" runat="server" />
<asp:HiddenField ID="hfReadiness" runat="server" />
<asp:HiddenField ID="hfFromFulfilByCommand" runat="server" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
   <br/>
   <span class="HeaderText" style='padding-top: 8px;'><span id="lblSubHeaderTitle" runat="server" style="font-size: 0.93em;"></span></span>
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
             <tr style="height: 25px;">
                <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblRegNumber" CssClass="InputLabel">Регистрационен номер:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <asp:TextBox runat="server" ID="txtRegNumber" CssClass="InputField" Width="255px"></asp:TextBox>
                </td>
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblInventoryNumber" CssClass="InputLabel">Инвентарен номер:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:TextBox runat="server" ID="txtInventoryNumber" CssClass="InputField" Width="295px"></asp:TextBox>
                </td>
             </tr>
             <tr style="height: 25px;">      
                <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblTechnicsCategory" CssClass="InputLabel">Категория:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <asp:DropDownList runat="server" ID="ddTechnicsCategory" CssClass="InputField" Width="260px"></asp:DropDownList>
                </td>         
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblTractorKind" CssClass="InputLabel">Вид:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <asp:DropDownList runat="server" ID="ddTractorKind" CssClass="InputField" Width="300px"></asp:DropDownList>
                </td>                 
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblTractorMake" CssClass="InputLabel">Марка:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 260px;">
                   <%--<asp:DropDownList runat="server" ID="ddTractorMake" CssClass="InputField" Width="260px" AutoPostBack="true" OnSelectedIndexChanged="ddTractorMake_Changed"></asp:DropDownList>--%>
                   <asp:TextBox runat="server" ID="txtTractorMakeName" CssClass="InputField" Width="255px"></asp:TextBox>
                </td>
                 <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblTractorModel" CssClass="InputLabel">Модел:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 300px;">
                   <%--<asp:DropDownList runat="server" ID="ddTractorModel" CssClass="InputField" Width="300px"></asp:DropDownList>--%>
                   <asp:TextBox runat="server" ID="txtTractorModelName" CssClass="InputField" Width="295px"></asp:TextBox>
                </td>
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: top; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="Label1" CssClass="InputLabel" title="Нормативна категория">Нормативна к-я:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;" colspan="3">
                   <asp:DropDownList runat="server" ID="ddNormativeTechnics" CssClass="InputField" Width="735px" ></asp:DropDownList>
                </td>
             </tr>
             <tr style="height: 18px;">               
                 <td style="vertical-align: top; text-align: left; width: 150px; padding-left: 70px; padding-top: 15px;" colspan="4">
                   <asp:Label runat="server" ID="lblOwnership" CssClass="InputLabel" Font-Bold="true">Собственик</asp:Label>
                </td>
             </tr>
             <tr style="height: 25px;">               
                 <td style="vertical-align: bottom; text-align: right; width: 150px;">
                   <asp:Label runat="server" ID="lblOwnershipNumber" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom; width: 200px;">
                   <asp:TextBox runat="server" ID="txtOwnershipNumber" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                 <td style="vertical-align: bottom; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblOwnershipName" CssClass="InputLabel">Трите имена /<br />Име на фирмата:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom; width: 300px;">
                   <asp:TextBox runat="server" ID="txtOwnershipName" CssClass="InputField" Width="295px"></asp:TextBox>
                </td>
             </tr>
             
             <tr style="height: 25px;">             
             </tr>             
             
             <tr style="height: 25px;">
                <td colspan="4">
                    <table>
                        <tr style="height: 25px;">
                            <td colspan="6" style="vertical-align: bottom; text-align: center;">
                                <asp:RadioButtonList ID="rblAddress" runat="server" CssClass="InputField" Width="400px" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text="Адрес на собственик" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="2" Text="Адрес по местодомуване"></asp:ListItem>                        
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="height: 25px;">
                             <td style="vertical-align: bottom; text-align: right;">
                                <asp:Label runat="server" ID="lblRegion" CssClass="InputLabel">Област:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:DropDownList runat="server" ID="ddRegion" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddRegion_Changed"></asp:DropDownList>                  
                            </td>
                             <td style="vertical-align: bottom; text-align: right;">
                               <asp:Label runat="server" ID="lblMuniciplaity" CssClass="InputLabel">Община:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                               <asp:DropDownList runat="server" ID="ddMuniciplaity" CssClass="InputField" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddMuniciplaity_Changed"></asp:DropDownList>
                            </td>                           
                            <td style="vertical-align: bottom; text-align: right;">
                                <asp:Label runat="server" ID="lblCity" CssClass="InputLabel" Width="140px">Населено място:</asp:Label>
                            </td>
                            <td style="vertical-align: bottom; text-align: left;">
                              <asp:DropDownList runat="server" ID="ddCity" CssClass="InputField" Width="200px"  AutoPostBack="true" OnSelectedIndexChanged="ddCity_Changed"></asp:DropDownList>              
                            </td>
                           
                        </tr>
                        <tr style="height: 25px;">
                           <td style="vertical-align: top; text-align: right;">
                               <asp:Label runat="server" ID="lblAddress" CssClass="InputLabel">Aдрес:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;" colspan="3" rowspan="2">
                               <asp:TextBox ID="txtAddress" runat="server" CssClass="InputField" TextMode="MultiLine" Rows="3" Width="500px" ></asp:TextBox>
                            </td>
                            <td style="vertical-align: bottom; text-align: right;">
                                <asp:Label runat="server" ID="lblDistrict" CssClass="InputLabel">Район:</asp:Label>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <asp:DropDownList runat="server" ID="ddDistrict" CssClass="InputField" Width="200px"></asp:DropDownList>
                            </td>
                         </tr>
                         <tr style="height: 25px;">    
                             <td colspan="4"></td>          
                               
                                 <td style="text-align: right; vertical-align: bottom;">
                                    <asp:Label runat="server" ID="lblPostCode" CssClass="InputLabel">Пощенски код:</asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: bottom;">
                                    <asp:TextBox runat="server" ID="txtPostCode" CssClass="InputField" Width="50px"></asp:TextBox>
                                </td>
                        </tr>
                    </table>
                </td>
             </tr>
             
             <tr style="height: 25px;">             
             </tr>
             
             <tr>
                <td colspan="4" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                </td>
             </tr>
             <tr>
                <td colspan="2" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;" runat="server" id="pnlPaging" visible="false">
    <div style="width: 670px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">              
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

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
