<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ApplicantsExams.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ApplicantsExams" 
         Title="Изпити на кандидати" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    .ShadowContainer
    {
	    margin: 0 auto;
	    width: auto;
    }
</style>


<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);

    function PageEndRequestHandler(sender, args)
    {
        //When the Refresh button is clicked then this ID is empty!
        if (sender._postBackSettings.sourceElement.id == "" ||
            sender._postBackSettings.sourceElement.id == "<%= btnRefresh.ClientID %>")
        {
            LoadOriginalValues();
        }
    }

    function SaveExams()
    {
        var lblMessage = document.getElementById("<%=lblGridMessage.ClientID %>");
        lblMessage.innerHTML = "";
        
        var positionsCounter = document.getElementById("positionsCounter");
        var examsCounter = document.getElementById("examsCounter");

        if (positionsCounter && examsCounter)
        {
            var positionsCount = parseInt(positionsCounter.value);
            var examsCount = parseInt(examsCounter.value);
            var url = "ApplicantsExams.aspx?AjaxMethod=JSSaveExams";

            var params = "PositionsCount=" + positionsCount;
            params += "&ExamsCount=" + examsCount;
            params += "&VacancyAnnounceID=" + document.getElementById("<%= ddOrderNum.ClientID %>").value;
            params += "&ResponsibleMilitaryUnitID=" + document.getElementById("<%= ddResponsibleMilitaryUnit.ClientID %>").value;
            params += "&OrderNumDate=" + document.getElementById("<%= ddOrderNum.ClientID %>").options[document.getElementById("<%= ddOrderNum.ClientID %>").selectedIndex].text;
            params += "&ResponsibleMilitaryUnitName=" + document.getElementById("<%= ddResponsibleMilitaryUnit.ClientID %>").options[document.getElementById("<%= ddResponsibleMilitaryUnit.ClientID %>").selectedIndex].text;

            var isDataValid = true;

            for (var i = 1; i <= positionsCount; i++)
            {
                var applicantID = document.getElementById("applicantID" + i).value
                var examStatusID = document.getElementById("ddExamStatus" + i).value;                

                params += "&ApplicantID" + i + "=" + applicantID;
                params += "&ExamStatusID" + i + "=" + examStatusID;

                for (var j = 1; j <= examsCount; j++)
                {
                    var applicantExamMarkID = document.getElementById("applicantExamMarkID" + i + "_" + j).value
                    var vacancyAnnounceExamID = document.getElementById("vacancyAnnounceExamID" + i + "_" + j).value
                    var examName = document.getElementById("examName" + i + "_" + j).value
                    var mark = document.getElementById("mark" + i + "_" + j).value;
                    var points = document.getElementById("points" + i + "_" + j).value;

                    if (mark != "" && !isInt(mark))
                    {
                        isDataValid = false;
                        lblMessage.className = "ErrorText";
                        lblMessage.innerHTML += "<br />" + GetErrorMessageNumberColumn("Оценка " + examName);
                    }

                    if (points != "" && !isInt(points))
                    {
                        isDataValid = false;                        
                        lblMessage.className = "ErrorText";
                        lblMessage.innerHTML += "<br />" + GetErrorMessageNumberColumn("Точки " + examName);
                    }

                    params += "&ApplicantExamMarkID" + i + "_" + j + "=" + applicantExamMarkID;
                    params += "&VacancyAnnounceExamID" + i + "_" + j + "=" + vacancyAnnounceExamID;
                    params += "&Mark" + i + "_" + j + "=" + mark;
                    params += "&Points" + i + "_" + j + "=" + points;
                }                         
            }

            function response_handler(xml)
            {
                var lblMessage = document.getElementById("<%=lblGridMessage.ClientID %>");
                if (xmlNodeText(xml.childNodes[0]) == "OK")
                {
                    document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "SAVED";
                    document.getElementById("<%= btnRefresh.ClientID %>").click();                 
                }
                else
                {
                    lblMessage.className = "ErrorText";
                    lblMessage.innerHTML = "Грешка при запис на данните";
                }
            }

            if (isDataValid) {
                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }
    }

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Изпити на кандидати</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 700px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 320px; min-height: 27px;">
                   <asp:Label runat="server" ID="lblOrderNum" CssClass="InputLabel">Заповед №:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:DropDownList runat="server" ID="ddOrderNum" CssClass="InputField" AutoPostBack="true" OnTextChanged="ddOrderNum_Changed" UnsavedCheckSkipMe="true" style="width: 300px;"></asp:DropDownList>
                </td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 320px; min-height: 27px;">
                   <asp:Label runat="server" ID="lblResponsibleMilitaryUnit" CssClass="InputLabel"><%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%> отговорна за конкурса:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:DropDownList runat="server" ID="ddResponsibleMilitaryUnit" CssClass="InputField" AutoPostBack="true" UnsavedCheckSkipMe="true" style="width: 300px;"></asp:DropDownList>
                </td>
             </tr> 
             <tr style="height: 10px;">
                <td></td>
             </tr>                
             <tr>
                <td colspan="2">
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

<div style="text-align: center;">
   <div runat="server" id="pnlDataGrid" style="text-align: center;" align="center"></div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 10px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="SaveExams(); return false;"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
