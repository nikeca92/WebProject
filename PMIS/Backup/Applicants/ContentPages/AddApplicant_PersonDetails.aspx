<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddApplicant_PersonDetails.aspx.cs" Inherits="PMIS.Applicants.ContentPages.AddApplicant_PersonDetails"
    Title="Регистриране на кандидат за военна служба" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style type="text/css">
         .lboxStatus
            {
                width: 650px;
                background-color: #EEEEEE;
                border: solid 1px #000000;
                position: fixed;
                top: 120px;
                left: 25%;
                min-height: 150px;
                z-index: 1000;
                padding-top: 10px;
            }
        .btnNewTableRecord
        {
            position: relative;
            top: -23px;
            text-align: right;
        }
        .btnNewTableRecordIcon
        {
            height: 18px;
            width: 18px;
            cursor: pointer;
        }
        .lboxMedCert
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxPsychCert
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }
     </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="contentDiv" style="display: none;">
                <div style="height: 20px">
                </div>
                <center>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <span id="lblHeaderTitle" runat="server" class="HeaderText">Регистриране на кандидат
                                    за военна служба</span>
                                <div style="height: 10px;">
                                </div>
                                <span id="lblHeaderSubTitle" runat="server" class="HeaderText" style="font-size: 1.2em;">
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">
                                <center>                                
                                    <table style="width: 800px;">
	                                    <tr>
	                                        <td style="text-align:left;">
		                                    <span class="InputLabel">Място на регистрация: <span runat="server" id="spanMilitaryDepartmentName" class="ReadOnlyValue"></span></span>
	                                        </td>
	                                        <td style="text-align:right;">
	                                            <span class="InputLabel" >Статус: <span id="spanPersonStatus" class="ReadOnlyValue"></span></span>
	                                            <img src="../Images/user_view.png" style="cursor:pointer;" onclick="ShowStatusLightBox();" title="Детайли"/>
	                                            
	                                        </td>
	                                    </tr>
                                    </table> 
                                    <div id="lboxStatus" style="display: none;" class="lboxStatus">
                                         <center>
                                           <div id="lboxStatus_Msg"></div>  
                                           <table style="position:relative; bottom: 10px; width:100%;  margin-top:20px;">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <div onclick="HideStatusLightBox();" class="Button">
                                                            <i></i>
                                                            <div style="width: 70px;">OK</div>
                                                            <b></b>
                                                        </div> 
                                                    </td>
                                                </tr>                                     
                                           </table> 
                                           
                                        </center>                                                    
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <fieldset style="width: 810px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Кандидат</legend>
                                        <table>
                                            <tr>
                                                <td style="text-align: right; width: 15%;">
                                                    <span class="InputLabel" id="lblFirstName">Име и презиме:</span>
                                                </td>
                                                <td colspan="3" style="text-align: left; width: 10%;">
                                                    <input type="text" class="RequiredInputField" style="width: 200px;" id="txtFirstName" maxlength="25" />
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                    <span class="InputLabel" id="lblLastName">Фамилия:</span>
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    <input type="text" class="RequiredInputField" style="width: 120px;" id="txtLastName" maxlength="15" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: bottom;">
                                                    <span class="InputLabel" id="lblIdentNumber">ЕГН:</span>
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <span class="ReadOnlyValue" id="lblIdentNumberValue"></span>
                                                </td>
                                                <td style="text-align: right; vertical-align: bottom;">
                                                    <span class="InputLabel" id="lblGender">Пол:</span>
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <div runat="server" id="pnlGenderContainer">
                                                    </div>
                                                </td>
                                                <td style="text-align: right; vertical-align: bottom;">
                                                    <span class="InputLabel" id="lblLastModified">Последна актуализация:</span>
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <span class="ReadOnlyValue" id="lblLastModifiedValue"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: bottom;">
                                                    <span class="InputLabel" id="lblAge">Възраст:</span>
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;" colspan="3">
                                                    <span class="ReadOnlyValue" id="lblAgeValue"></span>
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
                                    <fieldset style="width: 810px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Постоянен адрес</legend>
                                        <table>
                                            <tr>
                                                <td colspan="6" style="text-align: right;">
                                                    <span id="spCopyAddress">
                                                        <img src="../Images/copy.png" id="btnImgCopyAddressPerm" title="Копиране от Настоящ адрес" alt="Копиране от Настоящ адрес" style="cursor: pointer;"
                                                            onclick="CopyPresAddressToCurr();" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblPermRegion" class="InputLabel">Област:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divPermRegion" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblPermMunicipality" class="InputLabel">Община:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divPermMunicipality" runat="server">
                                                    </div>
                                                </td>                                                
                                                <td style="text-align: right; width: 210px;">
                                                    <span id="lblPermCity" class="InputLabel">Населено място:</span>
                                                </td>
                                                <td style="text-align: left; width: 200px;">
                                                    <div id="divPermCity" runat="server">
                                                    </div>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: top;" rowspan="2">
                                                    <span id="lblPermAddress" class="InputLabel">Адрес:</span>
                                                </td>
                                                <td colspan="3" style="text-align: left;" rowspan="2">
                                                    <textarea id="txtaPermAddress" cols="3" rows='3' class='InputField' style='width: 99%;'></textarea>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblPermDistrict" class="InputLabel">Район:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <div id="divPermDistrict" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td style="text-align: right;">
                                                    <span id="lblPermPostCode" class="InputLabel">Пощенски код:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtPermPostCode" onfocus='txtPermPostCode_Focus();' onblur='txtPermPostCode_Blur();'
                                                        type="text" class="InputField" style="width: 50px;" maxlength="4" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset style="width: 810px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Настоящ адрес</legend>
                                        <table>
                                            <tr>
                                                <td colspan="6" style="text-align: right;">
                                                    <span id="Span1">
                                                        <img src="../Images/copy.png" id="btnImgCopyAddressPres" title="Копиране от Постоянен адрес" alt="Копиране от Постоянен адрес" style="cursor: pointer;"
                                                            onclick="CopyPermAddressToCurr();" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblPresRegion" class="InputLabel">Област:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divPresRegion" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblPresMunicipality" class="InputLabel">Община:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divPresMunicipility" runat="server">
                                                    </div>
                                                </td>                                                
                                                <td style="text-align: right; width: 210px;">
                                                    <span id="lblPresCity" class="InputLabel">Населено място:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divPresCity" runat="server">
                                                    </div>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: top;" rowspan="2">
                                                    <span id="lblPresAddress" class="InputLabel">Адрес:</span>
                                                </td>
                                                <td colspan="3" style="text-align: left;" rowspan="2">
                                                    <textarea id="txtaPresAddress" cols="3" rows='3' class='InputField' style='width: 99%;'></textarea>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblPresDistrict" class="InputLabel">Район:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <div id="divPresDistrict" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td style="text-align: right;">
                                                    <span id="lblPresPostCode" class="InputLabel">Пощенски код:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtPresPostCode" onfocus='txtPresPostCode_Focus();' onblur='txtPresPostCode_Blur();'
                                                        type="text" class="InputField" style="width: 50px;" maxlength="4" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset style="width: 810px; padding: 5px;">
                                        <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Адрес за кореспонденция</legend>
                                        <table>
                                            <tr>
                                                <td colspan="6" style="text-align: right;">
                                                    <span id="Span2">
                                                        <img src="../Images/copy.png" id="btnImgCopyAddressContact" title="Копиране от Настоящ адрес" alt="Копиране от Настоящ адрес" style="cursor: pointer;"
                                                            onclick="CopyPresAddressToContact();" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblContactRegion" class="InputLabel">Област:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divContactRegion" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right; width: 80px;">
                                                    <span id="lblContactMunicipality" class="InputLabel">Община:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divContactMunicipality" runat="server">
                                                    </div>
                                                </td>                                                
                                                <td style="text-align: right; width: 210px;">
                                                    <span id="lblContactCity" class="InputLabel">Населено място:</span>
                                                </td>
                                                <td style="text-align: left; width: 170px;">
                                                    <div id="divContactCity" runat="server">
                                                    </div>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: top;" rowspan="2">
                                                    <span id="lblContactAddress" class="InputLabel">Адрес:</span>
                                                </td>
                                                <td colspan="3" style="text-align: left;" rowspan="2">
                                                    <textarea id="txtaContactAddress" cols="3" rows='3' class='InputField' style='width: 99%;'></textarea>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblContactDistrict" class="InputLabel">Район:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <div id="divContactDistrict" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td style="text-align: right;">
                                                    <span id="lblContactPostCode" class="InputLabel">Пощенски код:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtContactPostCode" onfocus='txtContactPostCode_Focus();' onblur='txtContactPostCode_Blur();'
                                                        type="text" class="InputField" style="width: 50px;" maxlength="4" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <div style="min-height: 10px;">
                                    </div>
                                    <fieldset style="width: 810px; padding: 5px;">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: right;" nowrap>
                                                    <span id="lblIDCardNumber" class="InputLabel">Лична карта номер:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="text" id="txtIDCardNumber" class="InputField" style="width: 100px;" maxlength="50" />
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblIDCardIssuedBy" class="InputLabel">издадена от:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="text" id="txtIDCardIssuedBy" class="InputField" style="width: 100px;" maxlength="100" />
                                                </td>
                                                <td style="text-align: right;">
                                                    <span id="lblIDCardIssueDate" class="InputLabel">на:</span>
                                                </td>
                                                <td style="text-align: left;" nowrap>
                                                    <span id="spanIDCardIssueDate" >
                                                        <input type="text" id="txtIDCardIssueDate" class=" + <%= PMIS.Common.CommonFunctions.DatePickerCSS() %> + @" style="width: 70px;" maxlength="10" /> 
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr style="min-height: 15px;">
                                                <td style="text-align: right;" nowrap>
                                                    <span id="lblHomePhone" class="InputLabel">Домашен телефон:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtHomePhone" type="text" class="InputField" style="width: 100px;" maxlength="10" />
                                                </td>
                                                <td style="text-align: right;" nowrap>
                                                    <span id="lblMobilePhone" class="InputLabel" style="padding-left: 10px">Мобилен телефон:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtMobilePhone" type="text" class="InputField" style="width: 100px;" maxlength="50" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <span id="lblEmail" class="InputLabel">E-mail:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input id="txtEmail" type="text" class="InputField" style="width: 200px;" maxlength="500" />
                                                </td>
                                                <td style="text-align: right;" nowrap>
                                                    <span class="InputLabel" style="margin: 0px;" id="lblDrvLicCategories">Шофьорска книжка:</span>
                                                </td>
                                                <td style="text-align: left;" nowrap>
                                                    <div id="tdPickListDrvLicCategories" style="display: inline-table; margin: 0px;">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" colspan="2">
                                                    <span id="lblWentToMilitary" class="InputLabel" style="padding-left: 10px">Бил ли е на военна служба?:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="radio" id="hasMilitarySrv1" name="hasMilitarySrv" value="1" title="Да" />
                                                    <span id="lblWentToMilitaryYes" class="InputLabel">Да</span>&nbsp;
                                                    <input type="radio" id="hasMilitarySrv2" name="hasMilitarySrv" value="2" title="Не" />
                                                    <span id="lblWentToMilitaryNo" class="InputLabel">Не</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" colspan="2">
                                                    <span id="lblMilitaryTraining" class="InputLabel" style="padding-left: 10px">Военна подготовка:</span>
                                                </td>
                                                <td style="text-align: left;">
                                                    <input type="radio" id="militaryTraining1" name="MilitaryTraining" value="1" title="Има" />
                                                    <span id="lblMilitaryTraining1" class="InputLabel">Има</span>&nbsp;
                                                    <input type="radio" id="militaryTraining2" name="MilitaryTraining" value="2" title="Няма" />
                                                    <span id="lblMilitaryTraining2" class="InputLabel">Няма</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                   
                                    <div style="min-height: 10px;"></div>
                                    <div id="divMedCertSection">
                                        <fieldset style="width: 810px; padding: 5px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Медицинско освидетелстване</legend>
                                            <table style="width: 800px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;">
                                              <tr style="height: 12px;">
                                              </tr>
                                              <tr>
                                                 <td colspan="6" style="text-align: left; padding-left: 10px;">
                                                    <div id="divMedCertTable"></div>
                                                    <div id="divMedCertLightBoxContainer"></div>
                                                 </td>
                                              </tr>
                                              <tr>        
                                                 <td colspan="6" style="text-align: left; padding-left: 10px;">
                                                    <span id="spanMedCertSectionMsg" class="ErrorText"></span>&nbsp;
                                                 </td>        
                                              </tr>
                                            </table>                                                                               
                                        </fieldset>
                                    </div>
                                    
                                    <div style="min-height: 10px;"></div>
                                    <div id="divPsychCertSection">
                                        <fieldset style="width: 810px; padding: 5px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Психологическа пригодност</legend>
                                            <table style="width: 800px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;">
                                              <tr style="height: 12px;">
                                              </tr>
                                              <tr>
                                                 <td colspan="6" style="text-align: left; padding-left: 10px;">
                                                    <div id="divPsychCertTable"></div>
                                                    <div id="divPsychCertLightBoxContainer"></div>
                                                 </td>
                                              </tr>
                                              <tr>        
                                                 <td colspan="6" style="text-align: left; padding-left: 10px;">
                                                    <span id="spanPsychCertSectionMsg" class="ErrorText"></span>&nbsp;
                                                 </td>        
                                              </tr>
                                            </table>                                                                               
                                        </fieldset>
                                    </div>
                                    
                                    <br />
                                    <div class="ErrorText" id="lblErrorMsg" style="min-height: 40px;">
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:center;">
                                <asp:Button runat="server" ID="btnGo" OnClick="btnGo_Click" />
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="Save_Click(); return false;">
                                    <i></i>
                                    <div style="width: 70px; padding-left: 5px;">
                                        <span id="lblGo" runat="server"></span>
                                    </div>
                                    <b></b>
                                </asp:LinkButton>
                                <div style="padding-left: 30px; display: inline">
                                </div>
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                    CheckForChanges="true">
                                    <i></i>
                                    <div style="width: 70px; padding-left: 5px;">
                                        <span id="lblBack" runat="server"></span>
                                    </div>
                                    <b></b>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="loadingDiv" class="LoadingDiv">
                <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
            </div>
            <div style="height: 20px;">
            </div>
            <input type="hidden" id="hdnPersonID" runat="server" />
            <asp:HiddenField runat="server" ID="hdnIdentNumber" />
            <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentID" />
            <asp:HiddenField runat="server" ID="hdnDrvLicCategories" />
            <asp:HiddenField runat="server" ID="hdnApplicantId" />
            
            <input type="hidden" id="hdnBirthCountryId" />
            <input type="hidden" id="hdnBirthCityId" />
            <input type="hidden" id="hdnBirthCityIfAbroad" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/PickList.js"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddApplicant_PersonDetails.js'></script>

    <script type="text/javascript">

        var hdnIdentNumberClientID = "<%= hdnIdentNumber.ClientID %>";
        var hdnPersonIDClientID = "<%= hdnPersonID.ClientID %>";
        var hdnMilitaryDepartmentIDClientID = "<%= hdnMilitaryDepartmentID.ClientID %>";
        var hdnDrvLicCategoriesClientID = "<%= hdnDrvLicCategories.ClientID %>";

        var btnSaveHelperClientID = "<%= btnGo.ClientID %>";
     
    </script>

</asp:Content>
