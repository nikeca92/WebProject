<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="MilitaryStructureAccessPerUser.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.MilitaryStructureAccessPerUser" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js'></script>
  
<style type="text/css">
.isDivMainClass
{
    font-family: Verdana;
    width: 260px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 250px;
}
</style>
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfUserID" runat="server" />
    <div style="height: 20px"></div>
    <center>
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                </td>
            </tr>
            <tr style="height: 15px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table>
                            <tr>
                               <td style="width: 50%; text-align: right;">
                                  <span id="lblUserName" class="InputLabel" runat="server" style="font-size: 1.15em;">Потребител:</span>
                               </td>
                               <td style="width: 50%; text-align: left;">
                                  <span id="lblUserNameValue" class="ReadOnlyValue" runat="server" style="font-size: 1.15em;"></span>
                               </td>
                            </tr>
                            <tr>
                               <td style="width: 50%; text-align: right;">
                                  <span id="lblFullName" class="InputLabel" runat="server" style="font-size: 1.15em;">Име:</span>
                               </td>
                               <td style="width: 50%; text-align: left;">
                                  <span id="lblFullNameValue" class="ReadOnlyValue" runat="server" style="font-size: 1.15em;"></span>
                               </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr style="height: 5px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <div style="width: 700px; margin: 0 auto;" id="divMilitaryDepartmentCont">
                           <fieldset style="width: 700px; padding-bottom: 0px;">
                           <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;">Военни окръжия</legend>
                              <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
                                 <tr>
                                    <td style="text-align: left; padding: 10px;">
                                       <table>
                                          <tr>
                                             <td style="text-align: left;">
                                                 <span id="lblMilitaryDepartment" class="InputLabel" runat="server">Военно окръжие</span>
                                             </td>
                                             <td style="width: 50px;"></td>
                                             <td style="text-align: left;">
                                                 <span id="lblSelectedMilitaryDepartments" class="InputLabel" runat="server">Избрани за потребителя</span>
                                             </td>
                                          </tr>
                                          <tr>
                                             <td style="vertical-align: top;">
                                                <asp:DropDownList runat="server" ID="ddMilitaryDepartment" Width="280px" CssClass="InputField"></asp:DropDownList>
                                             </td>
                                             <td style="vertical-align: top; text-align: center;">
                                                <input type="button" value=">>" title="Избор" onclick="btnSelectMilitaryDepartment_Click();"
                                                        style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                                        id="btnSelectMilitaryDepartment" />
                                             </td>
                                             <td style="vertical-align: top;">
                                                <asp:ListBox runat="server" ID="lstSelectedMilitaryDepartments" Width="280px" SelectionMode="Multiple" CssClass="InputField"></asp:ListBox>
                                                <span style="vertical-align: top;">
                                                   <img id="btnRemoveSelectedMilDepartments" src="../Images/delete.png" onclick="btnRemoveSelectedMilDepartments_Click();" alt='Изтриване' title='Изтриване на селектираните елементи' class='GridActionIcon'  />
                                                </span>
                                             </td>
                                          </tr>
                                       </table>
                                    </td>
                                 </tr>
                              </table>
                            </fieldset>
                        </div>
                    </center>
                </td>
            </tr>
            <tr style="height: 5px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <div style="width: 700px; margin: 0 auto;" id="divMilitaryUnitCont">
                           <fieldset style="width: 700px; padding-bottom: 0px;">
                           <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"><%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%></legend>
                              <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
                                 <tr>
                                    <td style="text-align: left; padding: 10px;">
                                       <table>
                                          <tr>
                                             <td style="text-align: left;">
                                                 <span id="lblMilitaryUnit" class="InputLabel" runat="server"><%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%></span>
                                             </td>
                                             <td style="width: 50px;"></td>
                                             <td style="text-align: left;">
                                                 <span id="lblSelectedMilitaryUnits" class="InputLabel" runat="server">Избрани за потребителя</span>
                                             </td>
                                          </tr>
                                          <tr>
                                             <td style="vertical-align: top;">
                                                <div id="musMilitaryUnitCont" style="width: 280px;">
                                                   <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit"                                  
                                                      DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" 
                                                      AllowPickUpNodesWithoutVPN="True" />
                                                </div>
                                             </td>
                                             <td style="vertical-align: top; text-align: center;">
                                                <input type="button" value=">>" title="Избор" onclick="btnSelectMilitaryUnit_Click();"
                                                        style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                                        id="btnSelectMilitaryUnit" />
                                             </td>
                                             <td style="vertical-align: top;">
                                                <asp:ListBox runat="server" ID="lstSelectedMilitaryUnits" Width="280px" SelectionMode="Multiple" CssClass="InputField"></asp:ListBox>
                                                <span style="vertical-align: top;">
                                                   <img id="btnRemoveSelectedMilUnits" src="../Images/delete.png" onclick="btnRemoveSelectedMilUnits_Click();" alt='Изтриване' title='Изтриване на селектираните елементи' class='GridActionIcon'  />
                                                </span>
                                             </td>
                                          </tr>
                                       </table>
                                    </td>
                                 </tr>
                              </table>
                            </fieldset>
                        </div>
                    </center>
                </td>
            </tr>
            <tr style="height: 40px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click" OnClientClick="btnSave_ClientClick();"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                    
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>  
            <tr style="height: 5px;">
                <td>
                    &nbsp;
                </td>
            </tr>        
        </table>
    </center>

    <asp:HiddenField ID="hdnSelectedMilitaryDepartments" runat="server" Value="" />
    <asp:HiddenField ID="hdnSelectedMilitaryUnits" runat="server" Value="" />
    
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
     function btnRemoveSelectedMilDepartments_Click()
     {
         RemoveListBoxSelection("<%= lstSelectedMilitaryDepartments.ClientID %>");
     }

     function btnRemoveSelectedMilUnits_Click()
     {
         RemoveListBoxSelection("<%= lstSelectedMilitaryUnits.ClientID %>");
     }

     function btnSelectMilitaryDepartment_Click()
     {
         var dropDown = document.getElementById("<%= ddMilitaryDepartment.ClientID %>");
     
         var val = dropDown.value;
         var text = dropDown.options[dropDown.selectedIndex].text;

         if (val != "" && val != "-1")
         {
             AddToDropDownIfNotExists("<%= lstSelectedMilitaryDepartments.ClientID %>", val, text);
             dropDown.value = "-1";
         }
     }

     function btnSelectMilitaryUnit_Click()
     {
         var val = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
         var text = MilitaryUnitSelectorUtil.GetSelectedText("<%= musMilitaryUnit.ClientID %>");

         if (val != "" && val != "-1")
         {
             AddToDropDownIfNotExists("<%= lstSelectedMilitaryUnits.ClientID %>", val, text);

             MilitaryUnitSelectorUtil.SetSelectedValue("<%= musMilitaryUnit.ClientID %>", "-1");
             MilitaryUnitSelectorUtil.SetSelectedText("<%= musMilitaryUnit.ClientID %>", "");
         }
     }

     function btnSave_ClientClick()
     {
         var selectedMilitaryDepartments = GetListBoxValues("<%= lstSelectedMilitaryDepartments.ClientID %>");
         document.getElementById("<%= hdnSelectedMilitaryDepartments.ClientID %>").value = selectedMilitaryDepartments;

         var selectedMilitaryUnits = GetListBoxValues("<%= lstSelectedMilitaryUnits.ClientID %>");
         document.getElementById("<%= hdnSelectedMilitaryUnits.ClientID %>").value = selectedMilitaryUnits;
     }
 </script>
   
</asp:Content>

