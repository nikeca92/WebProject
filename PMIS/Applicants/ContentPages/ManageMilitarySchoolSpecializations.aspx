<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageMilitarySchoolSpecializations.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ManageMilitarySchoolSpecializations" Title="Специалности и специализации за военните училища" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnSortBy" Value="1" runat="server" />

   <script type="text/javascript">    
        var ddlMilitarySchoolsClientID = "<%= ddlMilitarySchools.ClientID %>";
        var ddlSchoolYearsClientID = "<%= ddlSchoolYears.ClientID %>";
        var btnRefreshClientID = "<%=btnRefresh.ClientID %>";
        var lblGridMessageClientID = "<%=lblGridMessage.ClientID %>";
        var hdnSortByClientID = "<%=hdnSortBy.ClientID %>";
        var divMilitSchoolSpecsGridClientID = "<%=divMilitarySchoolSpecializationsGrid.ClientID %>";
    </script>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>
    <script src="../Scripts/ManageMilitarySchoolSpecializations.js" type="text/javascript"></script>

    <style type="text/css">
        .SpecializationsTibleLightBox
        {
	        min-width: 480px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 180px;
	        left: 24%;	
	        min-height: 200px;
	        z-index: 1000;
	        padding-top: 10px;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <input type="hidden" id="CanLeave" value="true" />
        <center>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <span id="lblHeaderTitle" runat="server" class="HeaderText">Специалности и специализации за военните училища</span>
            </div>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <div style="width: 600px; margin: 0 auto;">
                    <div class="FilterArea">
                        <div class="FilterLegend">Филтър</div>
                        <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                            <tr style="height: 10px;">
                                <td colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right; width: 90px;">
                                    <asp:Label runat="server" ID="lblMilitarySchool" CssClass="InputLabel">Висше училище:</asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: top;">
                                    <asp:DropDownList runat="server" ID="ddlMilitarySchools" Style="min-width: 200px;" 
                                        onselectedindexchanged="ddlMilitarySchools_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="vertical-align: top; text-align: right; width: 90px;">
                                    <asp:Label runat="server" ID="lblSchoolYear" CssClass="InputLabel">За учебната:</asp:Label>
                                </td>
                                <td style="text-align: left; vertical-align: top;">
                                    <asp:DropDownList runat="server" ID="ddlSchoolYears" Style="min-width: 100px;">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-top: 15px;">
                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="width: 100%; padding-left: 25px;" align="left">
                                    <asp:LinkButton ID="btnAddSpecialization" runat="server" CssClass="Button" OnClientClick="ShowSpecializationsTable(1, 1); return false;"><i></i><div style="width:300px; padding-left:5px;">Добавявне на специалност и специализация</div><b></b></asp:LinkButton>
                                    <div style="padding-left: 100px; display: inline">
                                    </div>
                                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>                                    
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <div id="divMilitarySchoolSpecializationsGrid" style="text-align: center;" runat="server">
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
        
    <div id="divSpecializationsLightBox" class="SpecializationsTibleLightBox" style="padding: 10px; display: none; text-align: center;">
        <img border='0' src='../Images/close.png' onclick="javascript:HideSpecializationsLightBox();" style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
        <div id="divSpecializationsLightBoxContent"></div>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
