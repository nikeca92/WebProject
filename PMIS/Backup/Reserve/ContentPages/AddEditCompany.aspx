<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" 
    CodeBehind="AddEditCompany.aspx.cs" Inherits="PMIS.Reserve.ContentPages.AddEditCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditCompany.js'></script>

    <script type="text/javascript">

        var hdnCompanyIDClientID = "<%= hdnCompanyId.ClientID %>";
        var lblHeaderTitleClientID = "<%= lblHeaderTitle.ClientID %>";
        var ddOwnershipTypeClientID = "<%= ddOwnershipType.ClientID %>";
        var ddAdministrationClientID = "<%= ddAdministration.ClientID %>";

        var unifiedIdentityCodeLabelText = '<%= UnifiedIdentityCodeLabelText %>';
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="contentDiv" style="display: none;">
                <div style="height: 20px">
                </div>
                <center>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <span id="lblHeaderTitle" runat="server" class="HeaderText">Добавяне на фирма</span>
                                <div style="height: 10px;">
                                </div>
                                <span id="lblHeaderSubTitle" runat="server" class="HeaderText" style="font-size: 1.2em;">
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <fieldset style="width: 800px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Данни на фирмата</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right;">
                                                    <span id="lblCompanyName" class="InputLabel">Име:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="text" id="txtCompanyName" class="RequiredInputField" maxlength="500" />
                                                </td>
                                                <td style="text-align: right; vertical-align: bottom;">
                                                    <span class="InputLabel" id="lblBulstat"><%= UnifiedIdentityCodeLabelText %>/ЕГН:</span>
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <input type="text" id="txtBulstat" class="InputField" maxlength="50" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <span class="InputLabel" id="lblOwnershipType">Вид собственост:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList runat="server" ID="ddOwnershipType" CssClass="InputField"></asp:DropDownList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span class="InputLabel" id="lblAdministration">Министeрство:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList runat="server" ID="ddAdministration" CssClass="InputField" Width="220px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <span class="InputLabel" id="lblPhone">Телефон:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="text" class="InputField" style="" id="txtPhone" maxlength="50" />
                                                </td>                                                
                                            </tr>                                            
                                        </table>
                                    </fieldset>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <fieldset style="width: 800px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Адрес на фирмата</legend>
                                        <table>                                         
                                            <tr>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblRegion" class="InputLabel">Област:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divRegion" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblMunicipality" class="InputLabel">Община:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divMunicipality" runat="server">
                                                    </div>
                                                </td>                                                
                                                <td style="text-align: right; width: 180px;">
                                                    <span id="lblCity" class="InputLabel">Населено място:</span>
                                                </td>
                                                <td style="text-align: left; width: 200px;">
                                                    <div id="divCity" runat="server">
                                                    </div>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td rowspan="2" style="text-align: right; vertical-align: top;">
                                                    <span id="lblAddress" class="InputLabel">Адрес:</span>
                                                </td>
                                                <td colspan="3" rowspan="2" style="text-align: left;">
                                                    <textarea id="txtaAddress" cols="3" rows='3' class='InputField' style='width: 99%;'></textarea>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblDistrict" class="InputLabel">Район:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <div id="divDistrict" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                           <tr>                                                
                                                <td style="text-align: right; ">
                                                    <span id="lblPostCode" class="InputLabel">Пощенски код:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtPostCode" onfocus='txtPostCode_Focus()' onblur='txtPostCode_Blur()'
                                                        type="text" class="InputField" style="width: 50px;" maxlength="4" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <div class="ErrorText" id="lblMessage" style="min-height: 10px;">
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; padding-top: 5px;">
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="Save_Click(); return false;">
                                    <i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                    CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Затвори</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="loadingDiv" class="LoadingDiv" style="padding-top: 150px; padding-bottom: 60px; text-align: center;">
                <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
            </div>
            <div style="height: 20px;">
            </div>
            <asp:HiddenField runat="server" ID="hdnCompanyId" Value="0" />
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
            <asp:HiddenField ID="hdnFromHome" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
