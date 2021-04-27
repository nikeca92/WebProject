<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditCadet.aspx.cs" Inherits="PMIS.Applicants.ContentPages.EditCadet" Title="Регистриране на курсант" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.MilitarySchoolSpecializationsLightBox
{
	min-width: 400px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 200px;
	left: 25%;
	z-index: 1000;
	padding: 10px;
}

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
</style>

    <div id="jsItemSelectorDiv" runat="server">
    </div>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CivilEducationSelector.js'></script>    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="contentDiv" style="display: none; min-height: 230px;">
                <div style="height: 20px">
                </div>
                <center>
                    <div style="margin-left: 30px;">
                        <div align="left">
                            <table style="width: 900px;" align="left">
                                <tr>
                                    <td align="center">
                                        <span id="lblHeaderTitle" runat="server" class="HeaderText">Регистриране на курсант</span>
                                    </td>
                                </tr>
                                <tr style="height: 10px; vertical-align: middle">
                                    <td align="center">
                                        <div id="lblHeaderSubTitle" runat="server" class="HeaderText" style="padding-right: 100px;
                                            font-size: 1.2em; padding-left: 210px; text-align: justify">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;">
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
                                                    <table style="position:relative; bottom: 10px; width:100%; margin-top:20px;">   
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
                                    </td>
                                </tr>
                            </table>
                            <fieldset style="width: 876px; padding-bottom: 0px; margin-left: 5px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                    <table style="width: 876px;">
                                <tr>
                                    <td>
                                        <span class="InputLabel" id="lblFirstName">Трите имена:</span>
                                        <span class="ReadOnlyValue" id="lblFirstNameValue"></span>
                                        <span class="ReadOnlyValue" id="lblLastNameValue"></span>
                                    </td>
                                    <td align="right">
                                        <div id="divEdit" runat="server">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="Button" OnClick="btnEdit_Click"
                                                CheckForChanges="true"><i></i><div style="width:190px; padding-left:5px;">Редактиране на лични данни</div><b></b></asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <span class="InputLabel" id="lblIdentNumber">ЕГН:</span>
                                        <span class="ReadOnlyValue" id="lblIdentNumberValue"></span>
                                        <span class="InputLabel" style="padding-left: 10px" id="lblGender">Пол:</span>
                                        <span class="ReadOnlyValue" id="lblGenderValue"></span>
                                        <span class="InputLabel" id="lblLastModified" style="padding-left: 10px">Последна актуализация:</span>
                                        <span class="ReadOnlyValue" id="lblLastModifiedValue"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span class="InputLabel" id="lblAge">Възраст:</span>
                                        <span class="ReadOnlyValue" id="lblAgeValue"></span>
                                    </td>
                                </tr>
                            </table>
                            </fieldset>
                            <table>
                                <tr>
                                    <td>
                                        <fieldset style="width: 430px; padding-bottom: 0px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                                <table style="width: 430px;">                                   
                                            <tr>
                                                <td align="left">
                                                    <div style="min-height: 5px">
                                                    </div>
                                                    <span id="lblPermAddresTitle" class="SmallHeaderText">Постоянен адрес</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPermPostCode" class="InputLabel">Пощенски код:</span>
                                                    <span id="txtPermPostCode" class="ReadOnlyValue"></span>
                                                    <span id="lblPermCity" class="InputLabel" style="padding-left: 10px">Населено място:</span>
                                                    <span id="txtPermCity" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPermMunicipality" class="InputLabel">Община:</span>
                                                    <span id="txtPermMunicipality" class="ReadOnlyValue"></span>
                                                    <span id="lblPermRegion" class="InputLabel" style="padding-left: 10px">Област:</span>
                                                    <span id="txtPermRegion" class="ReadOnlyValue"></span>
                                                    <span id="lblPermDistrict" class="InputLabel" style="padding-left: 10px">Район:</span>
                                                    <span id="txtPermDistrict" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPermAddress" style="text-align: inherit; vertical-align: top" class="InputLabel">Адрес:</span>
                                                    <span id="txtPermAddress" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                        </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="width: 430px; padding-bottom: 0px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                                <table style="width: 430px;">
                                            <tr>
                                                <td align="left">
                                                    <div style="min-height: 5px">
                                                    </div>
                                                    <span id="lblPresAddressTitle" class="SmallHeaderText">Настоящ адрес</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPresPostCode" class="InputLabel">Пощенски код:</span>
                                                    <span id="txtPresPostCode" class="ReadOnlyValue"></span>
                                                    <span id="lblPresCity" class="InputLabel" style="padding-left: 10px">Населено място:</span>
                                                    <span id="txtPresCity" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPresMunicipality" class="InputLabel">Община:</span>
                                                    <span id="txtPresMunicipility" class="ReadOnlyValue"></span>
                                                    <span id="lblPresRegion" class="InputLabel" style="padding-left: 10px">Област:</span>
                                                    <span id="txtPresRegion" class="ReadOnlyValue"></span>
                                                    <span id="lblPresDistrict" class="InputLabel" style="padding-left: 10px">Район:</span>
                                                    <span id="txtPresDistrict" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblPresAddress" class="InputLabel">Адрес:</span>
                                                    <span id="txtPresAddress" class="ReadOnlyValue"></span>
                                                </td>
                                            </tr>
                                        </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>    
                            <table>
                                <tr style="vertical-align: top;">
                                    <td>
                                        <fieldset style="width: 430px; height: 106px; padding-bottom: 0px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                            <table style="width: 430px;">
                                                <tr>
                                                    <td align="left">
                                                        <div style="min-height: 5px">
                                                        </div>
                                                        <span id="lblContactAddressTitle" class="SmallHeaderText">Адрес за кореспонденция</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblContactPostCode" class="InputLabel">Пощенски код:</span> <span id="txtContactPostCode"
                                                            class="ReadOnlyValue"></span><span id="lblContactCity" class="InputLabel" style="padding-left: 10px">
                                                                Населено място:</span> <span id="txtContactCity" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblContactMunicipality" class="InputLabel">Община:</span> <span id="txtContactMunicipality"
                                                            class="ReadOnlyValue"></span><span id="lblContactRegion" class="InputLabel" style="padding-left: 10px">
                                                                Област:</span> <span id="txtContactRegion" class="ReadOnlyValue"></span><span id="lblContactDistrict"
                                                                    class="InputLabel" style="padding-left: 10px">Район:</span> <span id="txtContactDistrict"
                                                                        class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblContactAddress" style="text-align: inherit; vertical-align: top" class="InputLabel">
                                                            Адрес:</span> <span id="txtContactAddress" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="width: 430px; padding-bottom: 0px;">
                                            <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>                        
                                            <table style="width: 430px;">
                                                <tr>
                                                    <td align="left">
                                                        <div style="min-height: 5px">
                                                        </div>
                                                        <span id="lblIDCardNumber" class="InputLabel">Лична карта номер:</span>
                                                        <span id="txtIDCardNumber" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblIDCardIssuedBy" class="InputLabel">издадена от:</span>
                                                        <span id="txtIDCardIssuedBy" class="ReadOnlyValue"></span>
                                                        <span id="lblIDCardIssueDate" class="InputLabel" style="padding-left: 10px">на:</span>
                                                        <span id="txtIDCardIssueDate" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblHomePhone" class="InputLabel">Домашен телефон:</span>
                                                        <span id="txtHomePhone" class="ReadOnlyValue"></span>
                                                        <span id="lblMobilePhone" class="InputLabel" style="padding-left: 10px">Мобилен телефон:</span>
                                                        <span id="txtMobilePhone" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblEmail" class="InputLabel">E-mail:</span>
                                                        <span id="txtEmail" class="ReadOnlyValue"></span>
                                                        <span id="lblDrvLicCategories" class="InputLabel" style="padding-left: 10px">Шофьорска книжка:</span>
                                                        <span id="txtDrvLicCategories" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblServeInMilitary" class="InputLabel">Служещ в <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>:</span>
                                                        <span id="lblServeInMilitaryValue" class="ReadOnlyValue"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <fieldset style="width: 876px; padding-bottom: 0px;margin-left: 5px; margin-top:3px;">
			                  <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
			                  <table style="width: 876px;">
				                <tr>
					               <td align="left">
						              <div style="min-height: 5px"></div>
						              <span id="Span1" class="SmallHeaderText">Медицинско освидетелстване</span>
					               </td>
				                </tr>
				                <tr>
					                <td align="left">
					                    <div id="divMedCertHTML"></div>
					                </td>
					            <tr>
			                  </table>
		                    </fieldset>
		                    <fieldset style="width: 876px; padding-bottom: 0px;margin-left: 5px; margin-top:3px;">
			                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
			                    <table style="width: 876px;">
				                    <tr>
				                        <td align="left">
					                        <div style="min-height: 5px"></div>
					                            <span id="Span2" class="SmallHeaderText">Психологическа пригодност</span>
					                    </td>
				                    </tr>
				                    <tr>
				                        <td align="left">
				                            <div id="divPsychCertHTML"></div>
					                    </td>
				                    </tr>							
			                   </table>
		                   </fieldset>
                        </div>
                    </div>
            </div>
            <div style="height: 10px;">
            </div>
            <table style="width: 1400px;" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" style="width: 100%">
                        <div id="TabSummary">
                            <ul>
                            <% if (IsSubjectsVisible())
                               { %>
                                <li class="ActiveTab" id="btnTabSubjects" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);" isalrеadyvisited="true"><a href="#" onclick="return false;"
                                        style="width: 150px; text-align: center">Желани специалности</a></li>
                            <% } %>
                            <% if (IsEducationVisible())
                               { %>
                                <li class="InactiveTab" id="btnTabEducation" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 130px;
                                        text-align: center">Образование</a></li>
                            <% } %>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="EditApplTabsBottomLine" />
                    </td>
                </tr>
            </table>
            <div style="border: solid 1px #888888;">
            <div id="divSubjects" style="display: none; padding: 20px 0 0 20px;" runat="server">
            </div>
            <div id="divEducation" style="display: none; text-align: center; padding: 5px 0 0 20px;" runat="server">
            </div>
            </div>
            <div style="height: 10px;">
            </div>
            <div id="loadingDiv" class="LoadingDiv">
                <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
            </div>
            <div>
                <table>
                    <tr align="center">
                        <td>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>        
            
            <asp:HiddenField runat="server" ID="hdnPersonId" />
            <asp:HiddenField runat="server" ID="hdnCadetId" />
            <asp:HiddenField runat="server" ID="hdnIdentNumber" />
            <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentId" />
            </center>
           
    <div id="divMilitarySchoolSpecializationsLightBox" class="MilitarySchoolSpecializationsLightBox" style="display: none; text-align: center;">
        <img border='0' src='../Images/close.png' onclick="javascript:HideMilitarySchoolSpecializationsLightBox();"
            style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
        <div id="divMilitarySchoolSpecializationsContent"></div>
    </div> 
    <div id="CadetEducationLightBox" class="PersonAbilityLightBox" style="display: none; text-align: center;">
    </div>
    <div id="CadetLanguageLightBox" class="PersonAbilityLightBox" style="display: none; text-align: center;">
    </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/EditCadet.js'></script>

    <script type="text/javascript">    
        var hdnPersonIdClientID = "<%= hdnPersonId.ClientID %>";
        var hdnCadetIdClientID = "<%= hdnCadetId.ClientID %>";
        var hdnIdentNumberClientID = "<%= hdnIdentNumber.ClientID %>";
        var hdnMilitaryDepartmentIdClientID = "<%= hdnMilitaryDepartmentId.ClientID %>";
        var divSubjectsClientID = "<%= divSubjects.ClientID %>";
        var divEducationClientID = "<%= divEducation.ClientID %>";
        var divEditClientID = "<%= divEdit.ClientID %>";
        
    </script>

</asp:Content>
