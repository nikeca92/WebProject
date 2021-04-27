<%@ Page Language="C#"  MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" 
    CodeBehind="TechParams.aspx.cs" Inherits="PMIS.Reserve.ContentPages.TechParams" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 20px;"></div>
                      
               <div style="margin: 0 auto; width: 800px">
                  <div class="HomePageHeader">
                     Параметри на техниката
                  </div>
                  <div style="height: 20px;"></div>
                  <div>                    
                    <table width="100%" style="border-collapse: collapse; vertical-align: middle; color: #0B449D;">
                        <tr>
                            <td style="vertical-align: top; text-align: left;">
                                <asp:Label runat="server" ID="lblTechnicsTypes" CssClass="InputLabel">Вид техника:</asp:Label>
                                <asp:DropDownList runat="server" ID="ddTechnicsTypes" CssClass="InputField" OnSelectedIndexChanged="ddTechnicsTypes_Changed"
                                                  AutoPostBack="True" Width="210px" >
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                    </table>
                    <div style="height: 20px;"></div>
                    <div runat="server" id="pnlLinks"></div>
                       
                  </div>
                  
               </div>  
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="CanLeave" value="true" />
</asp:Content>