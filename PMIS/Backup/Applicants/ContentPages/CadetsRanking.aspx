<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="CadetsRanking.aspx.cs" Inherits="PMIS.Applicants.ContentPages.CadetsRanking" Title="Класиране на курсанти" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>
    
    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/CadetsRanking.js"></script>    

    <script type="text/javascript">
        
        var lblFullNameValueClientID = "<%=lblFullNameValue.ClientID %>";
        var txtIdentityNumberClientID = "<%= txtIdentityNumber.ClientID %>";
        var ddlMilitarySchoolsClientID = "<%= ddlMilitarySchools.ClientID %>";
        var ddlSchoolYearsClientID = "<%= ddlSchoolYears.ClientID %>";
        var ddlSpecializationsClientID = "<%= ddlSpecializations.ClientID %>";
        
        var hdnRefreshReasonClientID = "<%=hdnRefreshReason.ClientID %>";
        var btnRefreshClientID = "<%=btnRefresh.ClientID %>";

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" id="CanLeave" value="true" />
            <center>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Класиране на курсанти</span>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <div style="width: 900px; margin: 0 auto;">
                        <div class="FilterArea">
                            <div class="FilterLegend">Филтър</div>
                            <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                                <tr style="height: 10px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 120px;">
                                        <asp:Label runat="server" ID="lblMilitarySchool" CssClass="InputLabel">Военно училище:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlMilitarySchools" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddlMilitarySchools_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lblSchoolYear" CssClass="InputLabel">Учебна година:</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlSchoolYears" Width="70px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSchoolYears_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 120px;">
                                        <asp:Label runat="server" ID="lblSubject" CssClass="InputLabel">Специалност:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlSubjects" AutoPostBack="true" Width="600px"
                                            OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: right; width: 120px;">
                                        <asp:Label runat="server" ID="lblSpecialization" CssClass="InputLabel">Специализация:</asp:Label>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList runat="server" ID="ddlSpecializations" Width="600px">
                                        </asp:DropDownList>
                                    </td> 
                                </tr>
                                <tr>
                                    <td colspan="3" style="padding-top: 15px;">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%;">
                                        <center>
                                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                                            <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblIdentityNumber" runat="server" Text="ЕГН:"></asp:Label>
                                                    <asp:TextBox ID="txtIdentityNumber" runat="server" CssClass="InputField" Width="100px" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td style="width: 50px;" align="center">
                                                    <img id="imgSearchCadet" alt="Търсене на курсант по ЕГН" title="Търсене на курсант по ЕГН" style="cursor: pointer;" src="../Images/arrow_right.png" onclick="SearchCadet();" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFullName" runat="server" Text="Трите имена:"></asp:Label>
                                                    <asp:Label ID="lblFullNameValue" runat="server" Text="" CssClass="InputLabel" Font-Bold="true"></asp:Label>
                                                    <img id="imgAddCadet" alt="Добавяне на курсанта като класиран" title="Добавяне на курсанта като класиран" style="cursor: pointer; display: none;" src="../Images/arrow_down.png" onclick="RankCadet();" />                                                                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 20px;">
                </div>
                <div style="text-align: center;">
                    <asp:Label ID="lblGridTitle" runat="server" Text="Списък на класираните курсанти" class="SmallHeaderText"></asp:Label>
                    <div runat="server" id="pnlCadetsGrid" style="text-align: center;">
                    </div>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
                </div>
                <div style="height: 10px;">
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </div>
                <div style="height: 20px;">
                </div>
            </center>
            <asp:HiddenField ID="hdnRefreshReason" runat="server" />
            <asp:HiddenField ID="hfMilitarySchoolId" runat="server" />
            <asp:HiddenField ID="hfSpecializationId" runat="server" />
            <asp:HiddenField ID="hfSchoolYear" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
