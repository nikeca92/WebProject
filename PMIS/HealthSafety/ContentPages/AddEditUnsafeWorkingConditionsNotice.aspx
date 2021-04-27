<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditUnsafeWorkingConditionsNotice.aspx.cs" Inherits="HealthSafety.ContentPages.AddEditUnsafeWorkingConditionsNotice" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.isDivMainClass
{
    font-family: Verdana;
    width: 270px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 245px;
}

</style>


<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>

<script type="text/javascript">
   
    window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
      
   function PageLoad() {   
       hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
   } 
   
   // Validate recommendation properties in the light box and generates appropriate error messages, if needed
   function ValidateNoticeData()
   {
      var res = true;
      var lblMessage = document.getElementById("<%= lblMessage.ClientID %>");
      lblMessage.innerHTML = "";
      
      var notValidFields = new Array();
      
      var noticeNumber = document.getElementById("<%= txtNoticeNumber.ClientID %>");
      var noticeDate = document.getElementById("<%= txtNoticeDate.ClientID %>");
      var riskReducingDueDate = document.getElementById("<%= txtRiskReducingDueDate.ClientID %>");
      
      if (TrimString(noticeNumber.value) == "")
      {
        res = false;
        
        if (noticeNumber.disabled == true || noticeNumber.style.display == "none")
            notValidFields.push("Сведение №");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Номер на сведението") + "<br />";
      }
      
      if (TrimString(noticeDate.value) == "")
      {
        res = false;
        
        if (noticeDate.disabled == true || noticeDate.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата на сведението") + "<br />";
      }
      else if (!IsValidDate(noticeDate.value))
      {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на сведението") + "<br />";
      }
      
      if (res && TrimString(riskReducingDueDate.value) != "" && !IsValidDate(riskReducingDueDate.value))
      {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Крайна дата на изпълнение на процедурите за намаляване на риска") + "<br />";
      }
        var notValidFieldsCount = notValidFields.length;
        var fieldsStr = '"' + notValidFields.join(", ") + '"';
        
      if(notValidFieldsCount > 0)
      {
         var noRightsMessage = GetErrorMessageNoRights(notValidFields);       
         lblMessage.innerHTML += "<br />" + noRightsMessage;
      }
      
      if (res)
      {
        ForceNoChanges();
        lblMessage.className = "SuccessText";
      }
      else
        lblMessage.className = "ErrorText";
      
      return res;
   }
   
   function RefreshGTableDegreeOfDanger()
   {
        document.getElementById("<%= btnRefreshGTableDegreeOfDanger.ClientID %>").click();
   }
   
    function ShowPrintUnsafeConditionsNotice()
    {
        var hfUnsafeWConditionsNoticeID = document.getElementById("<%= hfUnsafeWConditionsNoticeID.ClientID %>").value;

        var url = "";
        var pageName = "PrintUnsafeWorkingConditionsNotice"
        var param = "";
        
        url = "../PrintContentPages/" + pageName + ".aspx?UnsafeWConditionsNoticeID=" + hfUnsafeWConditionsNoticeID;
        
        var uplPopup = window.open(url, pageName, param);

        if (uplPopup != null)
            uplPopup.focus();
    }
   
 </script>

<div id="jsMilitaryUnitSelectorDiv" runat="server">
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfUnsafeWConditionsNoticeID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
    <asp:HiddenField ID="hdnLocationHash" runat="server" />
    <div style="height: 20px"></div>
    <center style="width: 100%;">
        <table>
            <tr style="min-height: 17px;">
                <td align="center" colspan="5">
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Добавяне на сведение за заболявания и наранявания свързани с работата</span>
                </td>                
            </tr>
            <tr style="min-height: 17px;">
                <td align="right" style="width: 150px;">
                    <asp:Label ID="lblNoticeNumber" runat="server" Text="Сведение №:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 310px;">
                    <asp:TextBox ID="txtNoticeNumber" runat="server" MaxLength="250" CssClass="RequiredInputField"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="right" style="width: 170px;">
                    <asp:Label ID="lblNoticeDate" runat="server" Text="Дата:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 270px;">
                    <asp:TextBox ID="txtNoticeDate" runat="server" MaxLength="10" Width="80px" CssClass="InputField"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="right" style="width: 150px;">
                    <asp:Label ID="lblReportingPersonName" runat="server" Text="Докладващо лице:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 310px;">
                    <asp:TextBox ID="txtReportingPersonName" runat="server" MaxLength="500" Width="300px" CssClass="InputField"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="right" style="width: 170px;">
                    <asp:Label ID="lblMilitaryUnit" runat="server"  CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 270px;">
                    <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                          DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="right" valign="top" rowspan="2" style="width: 150px;">
                    <asp:Label ID="lblViolationPlace" runat="server" Text="Местонахождение на нарушението:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" rowspan="2" style="width: 310px;">
                    <asp:TextBox ID="txtViolationPlace" runat="server" MaxLength="2000" Height="50px" Width="300px" TextMode="MultiLine" CssClass="InputField"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="right" style="width: 170px;">
                    <asp:Label ID="lblResponsiblePerson" runat="server" Text="Отговорно длъжностно лице:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 270px;">
                    <asp:TextBox ID="txtResponsiblePerson" runat="server" MaxLength="500" Width="245px" CssClass="InputField"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td style="width: 30px;">&nbsp;</td>
                <td align="right" style="width: 170px;">
                    <asp:Label ID="lblDangerDegree" runat="server" Text="Степен на опасност:" CssClass="InputLabel"></asp:Label>
                </td>
                <td align="left" style="width: 270px;">
                    <div style="float:left; clear: left;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDangerDegrees" runat="server" Width="250px" CssClass="InputField"></asp:DropDownList>
                                <asp:Button ID="btnRefreshGTableDegreeOfDanger" runat="server" OnClick="btnRefreshDegreeOfDanger_Click" style="display:none;"/>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshGTableDegreeOfDanger" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>                    
                    </div>
                    <div id="divDangerDegreesImg" runat="server">
                        <img alt="Редактиране на списъка" title="Редактиране на списъка" style="cursor: pointer;" src="../Images/list_edit.png" onclick="ShowGTable('DegreeOfDanger', 1, 1, RefreshGTableDegreeOfDanger);" />
                    </div>
                </td>
            </tr>
            <tr><td colspan="5" style="padding-top: 10px;"><img alt="" src="../Images/line.png" style="width: 950px; height: 7px;" /></td></tr>
            <tr style="min-height: 17px;">
                <td align="left" colspan="2" style="width: 460px;">
                    <asp:Label ID="lblDescOfUnsafeCondition" runat="server" Text="Описание на нездравословно или опасно условие, включително брой хора изложени или заплашени от такова условие/я:" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:Label ID="lblListOfViolatedRequirements" runat="server" Text="Лист, по номер и/или име, на всяко от изискванията за здравословни и безопасни условия на труд, които може да са нарушени, ако са известни:" CssClass="InputLabel"></asp:Label>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td colspan="2" style="width: 460px;">
                    <asp:TextBox ID="txtDescOfUnsafeCondition" TextMode="MultiLine" Width="450px" Height="100px" runat="server"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:TextBox ID="txtListOfViolatedRequirements" TextMode="MultiLine" Width="450px" Height="100px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr><td colspan="5" style="padding-top: 10px;"><img alt="" src="../Images/line.png" style="width: 950px; height: 7px;" /></td></tr>
            <tr style="min-height: 17px;">
                <td align="left" colspan="2" style="width: 460px;">
                    <asp:Label ID="lblRiskReducingDueDate" runat="server" Text="Препоръчаните процедури за намаляване на риска да приключат до:" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:TextBox ID="txtRiskReducingDueDate" MaxLength="10" Width="80px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="left" colspan="2" style="width: 460px;">
                    <asp:Label ID="lblTempProcedures" runat="server" Text="Временни:" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:Label ID="lblFinalProcedures" runat="server" Text="Окончателни:" CssClass="InputLabel"></asp:Label>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="left" colspan="2" style="width: 460px;">
                    <asp:TextBox ID="txtTempProcedures" TextMode="MultiLine" Width="450px" Height="100px" runat="server"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:TextBox ID="txtFinalProcedures" TextMode="MultiLine" Width="450px" Height="100px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="left" colspan="2" style="width: 460px;">
                    <asp:Label ID="lblTempProceduresEstResult" runat="server" Text="Прогнозна стойност:" CssClass="InputLabel"></asp:Label>
                    <asp:TextBox ID="txtTempProceduresEstResult" MaxLength="200" Width="250px" runat="server"></asp:TextBox>
                </td>
                <td style="width: 30px;">&nbsp;</td>
                <td align="left" colspan="2" style="width: 440px;">
                    <asp:Label ID="lblFinalProceduresEstResult" runat="server" Text="Прогнозна стойност:" CssClass="InputLabel"></asp:Label>
                    <asp:TextBox ID="txtFinalProceduresEstResult" MaxLength="200" Width="250px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr><td colspan="5" style="padding-top: 10px;"><img alt="" src="../Images/line.png" style="width: 950px; height: 7px;" /></td></tr>
            <tr style="min-height: 17px;">
                <td align="center" colspan="5">
                    <asp:Label ID="lblAdditionalInfo" runat="server" Text="Допълнителна информация относно това нарушение може да бъде получена от:" CssClass="InputLabel"></asp:Label>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="center" colspan="5">
                    <table>
                        <tr>
                            <td align="right" style="width: 200px;">
                                <asp:Label ID="lblAdditionalInfoContactPerson" runat="server" Text="Лице за връзка:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 700px;">
                                <asp:TextBox ID="txtAdditionalInfoContactPerson" runat="server" MaxLength="500" Width="500px" CssClass="InputField"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="min-height: 17px;">
                <td align="center" colspan="5">
                    <table>
                        <tr>
                            <td align="right" style="width: 200px;">
                                <asp:Label ID="lblAdditionalContactInfo" runat="server" Text="Информация за връзка:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td align="left" style="width: 700px;">
                                <asp:TextBox ID="txtAdditionalContactInfo" runat="server" MaxLength="1000" Width="500px" CssClass="InputField"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="min-height: 18px;">
                <td colspan="5">
                    <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="return ValidateNoticeData();" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnPrintUnsafeConditionsNotice" runat="server" CssClass="Button" OnClientClick="ShowPrintUnsafeConditionsNotice(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <span style="margin-left: 50px;">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                    </span>
                </td>
            </tr>
        </table>
    </center>
  </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
