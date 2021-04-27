<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddApplicant_SelectPerson.aspx.cs" Inherits="PMIS.Applicants.ContentPages.AddApplicant_SelectPerson"
    Title="Регистриране на кандидат за военна служба" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Common.js" type="text/javascript"></script>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript">

        var ValidationMessage;


        function IsDataValid()
        {
            var identityNumber = document.getElementById("<%= txtIdentNumber.ClientID %>").value;

            if (ValidateData(identityNumber)) {
                function validIdentNumberCallback() {
                    document.getElementById("<%= btnGo.ClientID %>").click();
                }

                function invalidIdentNumberCallback() {
                    document.getElementById("lblMessage").innerHTML = "Невалидно ЕГН";
                    document.getElementById("lblMessage").className = "ErrorText";
                }
            
                //Call function form Coomon.js
                isValidIdentityNumber(identityNumber, validIdentNumberCallback, invalidIdentNumberCallback);
            }
            else
            {
                document.getElementById("lblMessage").innerHTML = ValidationMessage;
                document.getElementById("lblMessage").className = "ErrorText";
            }
        }

        function ValidateData(identityNumber)
        {

            ValidationMessage = "";

            if (SelectedItem(document.getElementById("<%=ddMilitaryDepartments.ClientID %>")) == -1)
            {
                ValidationMessage += GetErrorMessageMandatory("Военно окръжие") + "</br>";
            }

            if (identityNumber == "")
            {
                ValidationMessage += GetErrorMessageMandatory("ЕГН");
            }
            else
            {
                if (!isOnlyDigits(identityNumber))
                {
                    ValidationMessage += GetErrorMessageNumber("ЕГН");
                }
                else
                {
                    if (identityNumber.length != 10)
                    {
                        ValidationMessage += ("Полето ЕГН изисква 10 цифри");
                    }
                }
            }

            return (ValidationMessage == "")
        }

        function SelectedItem(ddlObject)
        {
            var selectedItemId;
            var cheked = false

            var i = 0;
            do
            {
                if (ddlObject[i].selected)
                {
                    cheked = true;
                    selectedItemId = ddlObject[i].value

                }
                i++;

            } while (!cheked)

            return selectedItemId;
        }

        document.onkeypress = function(e) {
            if (!e) e = window.event;   // resolve event instance
            if (e.keyCode == '13') {
                IsDataValid();
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 30px">
            </div>
            <center>
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2">
                            <span id="lblHeaderTitle" runat="server" class="HeaderText">Регистриране на кандидат за военна служба</span>
                        </td>
                    </tr>
                    <tr style="height: 30px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMilitaryDepartment" runat="server" CssClass="InputLabel" Text="Военно окръжие:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddMilitaryDepartments" CssClass="RequiredInputField" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 40%;">
                            <asp:Label ID="lblIdentNumber" runat="server" CssClass="InputLabel" Text="ЕГН:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtIdentNumber" runat="server" CssClass="RequiredInputField" Width="120"
                                MaxLength="10"></asp:TextBox>
                        </td>
                    <tr>
                </table>
                <div id="lblMessage" style="text-align: center; margin-top: 10px; margin-bottom:10px; min-height: 30px; vertical-align:text-top">
                </div>
                <table>
                    <tr>
                        <td>
                            <div id="btnSave" style="display: inline;" onclick="IsDataValid();" class="Button">
                                <i></i>
                                <div style="width: 70px; display: inline">
                                    Продължи</div>
                                <b></b>
                            </div>
                            <div style="padding-left: 30px; display: inline">
                            </div>
                            <asp:LinkButton ID="btnBack" runat="server" CheckForChanges="true" CssClass="Button"
                                OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </center>
            <div style="min-height: 50px;">
            </div>
            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="display: none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
