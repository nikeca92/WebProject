<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditCommittee.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditCommittee" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">

    .isDivMainClass
    {
        font-family: Verdana;
        width: 170px;
    }

    .isDivMainClass input
    {
       font-family: Verdana;
       font-weight: normal;
       font-size: 11px;
       width : 150px;
    }

    </style>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCommitteeID" runat="server" />
            <asp:HiddenField ID="hfFromHome" runat="server" />
            <asp:HiddenField ID="hfMilitaryUnitLabel" runat="server" />
            <div style="height: 20px">
            </div>
            <center style="width: 100%;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <span id="lblHeaderTitle" runat="server" class="HeaderText">Добавяне на комитет</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <table style="border: solid 1px #cdc9c9; width: 650px; margin: 0 auto;">
                                    <colgroup width="100px">
                                    </colgroup>
                                    <colgroup width="150px">
                                    </colgroup>
                                    <colgroup width="100px">
                                    </colgroup>
                                    <colgroup width="150px">
                                    </colgroup>
                                    <tr>
                                        <td colspan="4" style="text-align: center;">
                                            <asp:DropDownList ID="ddCommitteeType" runat="server" CssClass="RequiredInputField" Width="255px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; vertical-align: top;">
                                            <asp:Label ID="lblMilitaryForceType" runat="server" CssClass="InputLabel" Text="Вид ВС:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">
                                            <asp:DropDownList ID="ddMilitaryForceType" runat="server" CssClass="InputField" Width="155px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                                DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr style="height: 18px;">
                        <td>
                            <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="return ValidateCommitteeData();"
                                OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                            <asp:LinkButton ID="btnPrint" runat="server" CssClass="Button" OnClientClick="ShowPrintCommittee(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <table style="margin: 0 auto;">
                                <tr style="height: 35px;">
                                    <td style="text-align: left;">
                                        <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClientClick="ShowCommitteeMemberLightBox(); return false;"><i></i><div style="width:100px; padding-left:5px;">Нов член</div><b></b></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divCommitteeMembers" runat="server">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfMsg" runat="server" />
                            <asp:Label ID="lblCommitteeMemberMessage" runat="server" Text="">&nbsp;</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </center>
            <div id="CommitteeMemberLightBox" class="CommitteeMemberLightBox" style="display: none;
                text-align: center;">
                <asp:HiddenField ID="hfPersonID" runat="server" />
                <center>
                    <table width="80%" style="text-align: center;">
                        <colgroup style="width: 30%">
                        </colgroup>
                        <colgroup style="width: 70%">
                        </colgroup>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span class="HeaderText" style="text-align: center;">Добавяне на член</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblIdentNumber" class="InputLabel">ЕГН:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtIdentNumber" type="text" class="RequiredInputField" style="width: 120px" maxlength="500" />
                                <img border='0' src='../Images/search.png' alt='Търсене' title='Търсене' onclick='GetPerson();'
                                    style='width: 20px; cursor: pointer; vertical-align: top;' />
                            </td>
                        </tr>
                        <tr style="height: 10px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblName" class="InputLabel">Име:</span>
                            </td>
                            <td style="text-align: left;">
                                <span id="txtName" class="InputField"></span>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanLightBoxMessage" class="ErrorText" style="display: none;"></span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnSaveLightBox" style="display: inline;" onclick="SaveCommitteeMember();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnSaveLightBoxText" style="width: 70px;">
                                                    Избор</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseLightBox" style="display: inline;" onclick="HideCommitteeMemberLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnCloseLightBoxText" style="width: 70px;">
                                                    Затвори</div>
                                                <b></b>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="TrainingsLightBox" class="TrainingsLightBox" style="display: none; text-align: center;">
                <asp:HiddenField ID="hfTrainingID" runat="server" />
                <center>
                    <table width="80%" style="text-align: center;">
                        <colgroup style="width: 30%">
                        </colgroup>
                        <colgroup style="width: 70%">
                        </colgroup>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span class="HeaderText" style="text-align: center;">Обучение</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblTrainingDate" class="InputLabel">Дата:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTrainingDate" runat="server" CssClass="RequiredInputField" Width="70px"
                                    MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblTrainingYear" class="InputLabel">Година на обучение:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtTrainingYear" type="text" class="RequiredInputField" style="width: 50px;" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblTrainingDesc" class="InputLabel">Обучение:</span>
                            </td>
                            <td style="text-align: left;">
                                <textarea id="txtTrainingDesc" rows="3" class="RequiredInputField" style="width: 350px;"></textarea>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblLegalRef" class="InputLabel">Наредба:</span>
                            </td>
                            <td style="text-align: left;">
                                <textarea id="txtLegalRef" rows="3" class="InputField" style="width: 350px;"></textarea>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanTrainingsLightBoxMessage" class="ErrorText" style="display: none;">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnSaveTrainingLightBox" style="display: inline;" onclick="SaveTraining();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnSaveTrainingLightBoxText" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseTrainingLightBox" style="display: inline;" onclick="HideTrainingsLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnCloseTrainingLightBoxText" style="width: 70px;">
                                                    Затвори</div>
                                                <b></b>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <asp:HiddenField ID="hdnAddDisabledControls" runat="server" />
            <asp:HiddenField ID="hdnAddHiddenControls" runat="server" />
            <asp:HiddenField ID="hdnEditDisabledControls" runat="server" />
            <asp:HiddenField ID="hdnEditHiddenControls" runat="server" />
            <asp:HiddenField ID="hdnTrainingPersonID" runat="server" />
            <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshProtocolItems_Click"
                CssClass="HiddenButton" />
            <asp:HiddenField ID="hdnSavedChanges" runat="server" />
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

        function EndRequestHandlerPage(sender, args)
        {
            SetClientTextAreaMaxLength("txtTrainingDesc", "500");
            SetClientTextAreaMaxLength("txtLegalRef", "2000");
        }

        function PageLoad()
        {
            SetClientTextAreaMaxLength("txtTrainingDesc", "500");
            SetClientTextAreaMaxLength("txtLegalRef", "2000");
        }

        function PageLoad()
        {
            hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
        }

        // Validate committee properties in the light box and generates appropriate error messages, if needed
        function ValidateCommitteeData()
        {
            var res = true;
            var lblMessage = document.getElementById("<%= lblMessage.ClientID %>");
            lblMessage.innerHTML = "";

            var notValidFields = new Array();

            var ddCommitteeType = document.getElementById("<%= ddCommitteeType.ClientID %>");
            var ddMilitaryForceType = document.getElementById("<%= ddMilitaryForceType.ClientID %>");

            if (ddCommitteeType.value == "" || ddCommitteeType.value == "-1")
            {
                res = false;

                if (ddCommitteeType.disabled == true || ddCommitteeType.style.display == "none")
                    notValidFields.push("Тип(комитет или група)");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Тип (комитет или група)") + "<br />";
            }

            if (MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>") == "-1")
            {
                res = false;
                var militaryUnitLabel = document.getElementById("<%= hfMilitaryUnitLabel.ClientID %>").value;

                if (MilitaryUnitSelectorUtil.IsDisabled("<%= musMilitaryUnit.ClientID %>") || 
                    MilitaryUnitSelectorUtil.IsHidden("<%= musMilitaryUnit.ClientID %>"))
                    notValidFields.push(militaryUnitLabel);
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory(militaryUnitLabel) + "<br />";
            }

            var notValidFieldsCount = notValidFields.length;
            var fieldsStr = '"' + notValidFields.join(", ") + '"';

            if (notValidFieldsCount > 0)
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

        // Set corresponding to entered ident number name and id of the person by ajax request
        function GetPerson()
        {
            document.getElementById("spanLightBoxMessage").innerHTML = "";

            var url = "AddEditCommittee.aspx?AjaxMethod=JSGetPerson";

            var params = "IdentNumber=" + custEncodeURI(TrimString(document.getElementById("txtIdentNumber").value));

            function response_handler(xml)
            {
                if (xmlValue(xml, "response") == "ERROR")
                    alert("There was a server problem!");
                else if (xmlValue(xml, "response") == "NOTFOUND")
                {
                    document.getElementById("spanLightBoxMessage").innerHTML = "Няма намерен човек!";
                    document.getElementById("spanLightBoxMessage").style.display = "";

                    //disable button, because there is no choosen person yet
                    document.getElementById("btnSaveLightBox").className = "DisabledButton";
                    document.getElementById("btnSaveLightBox").setAttribute("onclick", "");

                    // reset values 
                    document.getElementById("<%= hfPersonID.ClientID %>").value = 0;
                    document.getElementById("txtName").innerHTML = "";
                }
                else
                {
                    document.getElementById("<%= hfPersonID.ClientID %>").value = xmlValue(xml, "PersonID");
                    document.getElementById("txtName").innerHTML = xmlValue(xml, "Name");

                    //enable choose button, because there is choosen person now
                    document.getElementById("btnSaveLightBox").className = "Button";
                    document.getElementById("btnSaveLightBox").setAttribute("onclick", "SaveCommitteeMember();");
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        var initRecommendDialogHeight = 0;

        // Display light box for adding new committee member
        function ShowCommitteeMemberLightBox()
        {
            //cleaning old values, if any
            document.getElementById("txtIdentNumber").value = "";
            document.getElementById("txtName").innerHTML = "";

            //disable button, because there is no choosen person yet
            document.getElementById("btnSaveLightBox").className = "DisabledButton";
            document.getElementById("btnSaveLightBox").setAttribute("onclick", "");

            // clean message label in the light box and hide it            
            document.getElementById("spanLightBoxMessage").style.display = "none";
            document.getElementById("spanLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("CommitteeMemberLightBox").style.display = "";
            CenterLightBox("CommitteeMemberLightBox");

            initRecommendDialogHeight = document.getElementById("CommitteeMemberLightBox").offsetHeight;
        }

        // Close the light box and refresh commitee members table
        function HideCommitteeMemberLightBox()
        {
            document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("CommitteeMemberLightBox").style.display = "none";
        }

        // Saves commitee member through ajax request, if light box values are valid, or displays generated error messages
        function SaveCommitteeMember()
        {
            var url = "AddEditCommittee.aspx?AjaxMethod=JSSaveCommitteeMember";

            var params = "CommitteeID=" + document.getElementById("<%= hfCommitteeID.ClientID %>").value +
                       "&PersonID=" + document.getElementById("<%= hfPersonID.ClientID %>").value;

            function response_handler(xml)
            {

                var hideDialog = true;
                var resultMsg = xmlValue(xml, "response");
                if (resultMsg != "OK" && resultMsg != "ERROR")
                {
                    var lightBoxMessage = document.getElementById("spanLightBoxMessage");
                    lightBoxMessage.innerHTML = "";
                    lightBoxMessage.style.display = "";
                    hideDialog = false;
                    lightBoxMessage.innerHTML = resultMsg;
                }
                else if (resultMsg != "OK")
                    document.getElementById("<%= hfMsg.ClientID %>").value = "FailCommitteeMemberSave";
                else
                    document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCommitteeMemberSave";

                if (hideDialog)
                    HideCommitteeMemberLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        // Delete commitee member through ajax request
        function DeleteCommitteeMember(commmitteeMemberID)
        {
            YesNoDialog('Желаете ли да изтриете члена?', ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "AddEditCommittee.aspx?AjaxMethod=JSDeleteCommitteeMember";
                var params = "CommitteeID=" + document.getElementById("<%= hfCommitteeID.ClientID %>").value +
                             "&CommmitteeMemberID=" + commmitteeMemberID;
                
                function response_handler(xml)
                {
                    if (xmlValue(xml, "response") != "OK")
                        document.getElementById("<%= hfMsg.ClientID %>").value = "FailCommmitteeMemberDelete";
                    else
                    {
                        document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCommmitteeMemberDelete";
                        document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }

        function ViewTrainingHistory(personId)
        {
            CustomCheckFormSave(function() { JSRedirect("AddEditTrainingHistory.aspx?PersonID=" + personId + "&FromCommittee=" + document.getElementById("<%= hfCommitteeID.ClientID %>").value); });
        }

        // reset all controls in lightbox to be visible and enabled
        function EnableAllLightBoxControls()
        {
            var lblDate = document.getElementById("lblTrainingDate");
            var lblYear = document.getElementById("lblTrainingYear");
            var lblDesc = document.getElementById("lblTrainingDesc");
            var lblLegalRef = document.getElementById("lblLegalRef");

            var txtDate = document.getElementById("<%= txtTrainingDate.ClientID %>");
            var txtYear = document.getElementById("txtTrainingYear");
            var txtDesc = document.getElementById("txtTrainingDesc");
            var txtLegalRef = document.getElementById("txtLegalRef");

            lblDate.disabled = false;
            lblDate.style.display = "";
            lblYear.disabled = false;
            lblYear.style.display = "";
            lblDesc.disabled = false;
            lblDesc.style.display = "";
            lblLegalRef.disabled = false;
            lblLegalRef.style.display = "";

            txtDate.disabled = false;
            txtDate.style.display = "";
            txtYear.disabled = false;
            txtYear.style.display = "";
            txtDesc.disabled = false;
            txtDesc.style.display = "";
            txtLegalRef.disabled = false;
            txtLegalRef.style.display = "";
        }

        var initRecommendDialogHeight = 0;

        // Display light box with training properties (for editing or adding new)
        function ShowTrainingLightBox(trainingID, personID)
        {
            document.getElementById("<%= hdnTrainingPersonID.ClientID %>").value = personID;

            EnableAllLightBoxControls();

            if (trainingID != 0) // gets current values if editing training
            {
                // set controls to be disabled and hidden, according to user role rights
                document.getElementById('<%= ((HiddenField)Master.FindControl("hdnDisabledClientControls")).ClientID %>').value = document.getElementById("<%= hdnEditDisabledControls.ClientID %>").value;
                document.getElementById('<%= ((HiddenField)Master.FindControl("hdnHiddenClientControls")).ClientID %>').value = document.getElementById("<%= hdnEditHiddenControls.ClientID %>").value;

                var url = "AddEditTrainingHistory.aspx?AjaxMethod=JSGetTraining";

                var params = "TrainingID=" + trainingID;

                function response_handler(xml)
                {
                    document.getElementById("<%= txtTrainingDate.ClientID %>").value = xmlValue(xml, "TrainingDate");
                    document.getElementById("txtTrainingYear").value = xmlValue(xml, "TrainingYear");
                    document.getElementById("txtTrainingDesc").value = xmlValue(xml, "TrainingDesc");
                    document.getElementById("txtLegalRef").value = xmlValue(xml, "LegalRef");
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
            else // cleaning old values, if adding new training
            {
                // set controls to be disabled and hidden, according to user role rights
                document.getElementById('<%= ((HiddenField)Master.FindControl("hdnDisabledClientControls")).ClientID %>').value = document.getElementById("<%= hdnAddDisabledControls.ClientID %>").value;
                document.getElementById('<%= ((HiddenField)Master.FindControl("hdnHiddenClientControls")).ClientID %>').value = document.getElementById("<%= hdnAddHiddenControls.ClientID %>").value;

                document.getElementById("<%= txtTrainingDate.ClientID %>").value = "";
                document.getElementById("txtTrainingYear").value = "";
                document.getElementById("txtTrainingDesc").value = "";
                document.getElementById("txtLegalRef").value = "";
            }

            // force disabling and hiding client controls in the lightbox
            CheckDisabledClientControls();
            CheckHiddenClientControls();

            document.getElementById("<%= hfTrainingID.ClientID %>").value = trainingID; // setting training ID(0 - if new training)

            // clean message label in the light box and hide it            
            document.getElementById("spanLightBoxMessage").style.display = "none";
            document.getElementById("spanLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("TrainingsLightBox").style.display = "";
            CenterLightBox("TrainingsLightBox");

            initRecommendDialogHeight = document.getElementById("TrainingsLightBox").offsetHeight;
        }

        // Close the light box and refresh trainings table
        function HideTrainingsLightBox()
        {
            document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("TrainingsLightBox").style.display = "none";
        }

        // Validate training properties in the light box and generates appropriate error messages, if needed
        function ValidateTraining()
        {
            var res = true;
            var lightBox = document.getElementById('TrainingsLightBox');
            var lightBoxMessage = document.getElementById("spanTrainingsLightBoxMessage");
            lightBoxMessage.innerHTML = "";

            var notValidFields = new Array();

            var trainingDate = document.getElementById("<%= txtTrainingDate.ClientID %>");
            var trainingYear = document.getElementById("txtTrainingYear");
            var trainingDesc = document.getElementById("txtTrainingDesc");
            var legalRef = document.getElementById("txtLegalRef");

            if (TrimString(trainingDate.value) == "")
            {
                res = false;

                if (trainingDate.disabled == true || trainingDate.style.display == "none")
                    notValidFields.push("Дата");
                else
                    lightBoxMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
            }
            else if (!IsValidDate(trainingDate.value))
            {
                res = false;
                lightBoxMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
            }

            if (TrimString(trainingYear.value) == "")
            {
                res = false;

                if (trainingYear.disabled == true || trainingYear.style.display == "none")
                    notValidFields.push("Година на обучение");
                else
                    lightBoxMessage.innerHTML += GetErrorMessageMandatory("Година на обучение") + "<br />";
            }
            else
                if (!isInt(TrimString(trainingYear.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Година на обучение") + "<br />";
            }

            if (TrimString(trainingDesc.value) == "")
            {
                res = false;

                if (trainingDesc.disabled == true || trainingDesc.style.display == "none")
                    notValidFields.push("Обучение");
                else
                    lightBoxMessage.innerHTML += GetErrorMessageMandatory("Обучение") + "<br />";
            }

            var notValidFieldsCount = notValidFields.length;
            if (notValidFieldsCount > 0)
            {
                lightBox.style.height = initRecommendDialogHeight + notValidFieldsCount * 20 + 15 + "px";
            }
            else
            {
                lightBox.style.height = initRecommendDialogHeight + "px";
            }


            if (notValidFieldsCount > 0)
            {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
            }

            return res;
        }

        // Saves training through ajax request, if light box values are valid, or displays generated error messages
        function SaveTraining()
        {
            if (ValidateTraining())
            {
                var url = "AddEditTrainingHistory.aspx?AjaxMethod=JSSaveTraining";

                var params = "PersonID=" + document.getElementById("<%= hdnTrainingPersonID.ClientID %>").value +
                     "&TrainingID=" + document.getElementById("<%= hfTrainingID.ClientID %>").value +
                     "&TrainingDate=" + TrimString(document.getElementById("<%= txtTrainingDate.ClientID %>").value) +
                     "&TrainingYear=" + TrimString(document.getElementById("txtTrainingYear").value) +
                     "&TrainingDesc=" + custEncodeURI(TrimString(document.getElementById("txtTrainingDesc").value)) +
                     "&LegalRef=" + custEncodeURI(TrimString(document.getElementById("txtLegalRef").value));

                function response_handler(xml)
                {

                    var hideDialog = true;
                    var resultMsg = xmlValue(xml, "response");
                    if (resultMsg != "OK" && resultMsg != "ERROR")
                    {
                        var lightBoxMessage = document.getElementById("spanTrainingsLightBoxMessage");
                        lightBoxMessage.innerHTML = "";
                        lightBoxMessage.style.display = "";
                        hideDialog = false;
                        lightBoxMessage.innerHTML = resultMsg;
                    }
                    else if (resultMsg != "OK")
                        document.getElementById("<%= hfMsg.ClientID %>").value = "FailTrainingSave";
                    else
                        document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessTrainingSave";

                    if (hideDialog)
                        HideTrainingsLightBox();
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
            else
            {
                document.getElementById("spanTrainingsLightBoxMessage").style.display = "";
            }
        }

        function ShowPrintCommittee()
        {
            var hfCommitteeID = document.getElementById("<%= hfCommitteeID.ClientID %>").value;

            var url = "";
            var pageName = "PrintCommittee"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?CommitteeID=" + hfCommitteeID;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }
   
    </script>

</asp:Content>
