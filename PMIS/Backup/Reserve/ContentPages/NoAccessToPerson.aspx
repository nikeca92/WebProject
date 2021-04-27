<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="NoAccessToPerson.aspx.cs" Inherits="PMIS.Reserve.ContentPages.NoAccessToPerson" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <div style="margin: 0 auto; width: 830px;">
                    <fieldset style="width: 830px; padding: 0px;">
                       <table class="InputRegion" style="width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;">
                          <tr style="height: 3px;">
                          </tr>
                          <tr>
                             <td style="text-align: left;">
                                <span style="color: #9B4439; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Ограничен достъп</span>
                             </td>
                          </tr>
                          <tr>
                             <td style="text-align: left;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td style="text-align: left;">
                                            <span id="lblCurrPostCode" class="InputLabel">Нямате достъп до лицето <b><%= PersonDisplayInfo %></b>.<%= PersonMilitaryEmployedAt %> Моля обърнете се към системния администратор.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; padding-top: 20px;">
                                            <asp:LinkButton ID="btnBack" runat="server" CheckForChanges="true" CssClass="Button"
                                                 OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                             </td>
                          </tr>
                       </table>
                    </fieldset>
                </div>
            </div>
            <div style="height: 20px;">
            </div>
            
            <asp:HiddenField runat="server" ID="hdnPageFrom" />
            <asp:HiddenField runat="server" ID="hdnPersonId" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
