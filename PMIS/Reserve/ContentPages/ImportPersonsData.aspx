<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" 
    CodeBehind="ImportPersonsData.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ImportPersonsData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js"></script>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/ImportPersonsData.js'></script>
    
    <script type="text/javascript">
        function GetImportExceptionsFile()
        {
            document.getElementById("<%= hdnBtnGetImportExceptionsFile.ClientID %>").click();
        }
    </script>
    
    <style type="text/css">
        .lboxImportFileFormatInfo
        {
            width: 700px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 20px;
            left: 23%;
            min-height: 430px;
            z-index: 1000;
            padding-top: 10px;
        }
    </style>

        <div id="lboxImportFileFormatInfo" style="display: none;" class="lboxImportFileFormatInfo">
            <center>
                <table width="80%" style="text-align: center;">
                    <colgroup style="width: 40%">
                    </colgroup>
                    <colgroup style="width: 60%">
                    </colgroup>
                    <tr style="height: 15px">
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span class="HeaderText" style="text-align: center;" id="lblImportFileFormatInfoTitle">Формат на файла за импорт на данни</span>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                    </tr>
                    <tr style="min-height: 17px; text-align: left;">
                        <td>
                           <div>- Данните се импортират от текстов файл.</div> 
                           <div>- Всеки отделен ред съдържа информация за едно военно-отчетно лице.</div>
                           <div>- Обработват се файлове съдържащи до 1000 реда, кодирани в UTF-8 или ANSI.</div>
                           <div>- Полетата за всяко военно-отчено лице са разделени със символа "|".</div>
                           <div>- Полетата са (в този ред): <br />
                              <div style="padding-left: 10px; font-style: italic;">
                                  <table>
                                    <tr><td>ЕГН</td></tr>
                                    <tr><td>име</td></tr>
                                    <tr><td>презиме</td></tr>
                                    <tr><td>фамилия</td></tr>
                                    <tr><td>инициали</td></tr>
                                    <tr><td>пощенски код (пост. адрес)</td></tr>
                                    <tr><td>населено място (пост. адрес)</td></tr>
                                    <tr><td>адрес (пост. адрес)</td></tr>
                                    <tr><td>пощенски код (наст. адрес)</td></tr>
                                    <tr><td>населено място (наст. адрес)</td></tr>
                                    <tr><td>адрес (наст. адрес)</td></tr>
                                    <tr><td>професия</td></tr>
                                    <tr><td>специалност</td></tr>
                                  </table>
                              </div>
                           </div>
                           <div>- Ако има нужда да се добавят повече от една специалности за едно военно-отчетно лице, тогава за това военно-отчетно лице може да присъстват повече от един ред с различни стойности в полето за специалност.</div>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <table style="margin: 0 auto;">
                                <tr>
                                    <td>
                                        <div id="btnCloseImportFileFormatInfoLightBox" style="display: inline;" onclick="HideImportFileFormatInfoLightBox();"
                                            class="Button">
                                            <i></i>
                                            <div id="btnCloseImportFileFormatInfoLightBoxText" style="width: 70px;">
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

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportPersonsData" />
            <asp:PostBackTrigger ControlID="hdnBtnGetImportExceptionsFile" />
            <asp:PostBackTrigger ControlID="lnkSampleImportDataFile" />
        </Triggers>
        <ContentTemplate>
            <div id="contentDiv" style="display: block;" runat="server">
                <div style="height: 20px">
                </div>
                <center>
                    <table style="width: 800px;">
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <span id="lblHeaderTitle" runat="server" class="HeaderText">Импорт на нови за АСУ ВОЛ</span>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align: right;">
                                <img src="../Images/speech_balloon_16.png" alt="Информация за формата на файла" title="Информация за формата на файла" 
                                   style="cursor: pointer; position: relative; top: 5px;" onclick="ShowImportFileFormatInfoLightBox();" />&nbsp;
                                <asp:LinkButton runat="server" ID="lnkSampleImportDataFile" OnClick="lnkSampleImportDataFile_Click">Примерен файл</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <span class="InputLabel" id="lblMilitaryDepartment">Военно окръжие:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddlMilitaryDepartments" CssClass="RequiredInputField" Width="240px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblFileToImport" runat="server" Text="Файл за импортиране:" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td class="HeaderText" style="text-align: left;">
                                <asp:FileUpload ID="fuFileToImport" EnableViewState="true" runat="server" Width="70%" />
                            </td>
                        </tr>                     
                      <tr style="min-height:15px">
                        <td colspan="2">
                            <span id="spanInfo" runat="server"></span>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:LinkButton ID="btnImportPersonsData" runat="server" CssClass="Button" OnClick="btnImportPersonsData_Click"><i></i><div style="width:150px; padding-left:5px;">Импортиране</div><b></b></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                        </td>
                      </tr>  
                    </table>
                </center>
            </div>     
            
            <div id="resultDiv" visible="false" runat="server">
                <div style="height: 20px">
                </div>
                <center>
                    <table style="width: 500px;">
                        <tr>
                            <td>
                                <span id="lblHeaderResultTitle" runat="server" class="HeaderText">Импорт на нови за АСУ ВОЛ (резултат)</span>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>                     
                      <tr style="min-height:15px">
                        <td>
                            <div id="spanResult" runat="server" style="text-align:left;"></div>
                        </td>
                      </tr>
                       <tr style="height: 20px;">
                        </tr>
                      <tr>
                        <td style="text-align: center;">
                            <asp:LinkButton ID="btnNewImport" runat="server" CssClass="Button" OnClick="btnNewImport_Click"><i></i><div style="width:150px; padding-left:5px;">Нов импорт</div><b></b></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        </td>
                      </tr>  
                    </table>
                </center>
            </div>         
            
             <asp:HiddenField ID="hdnImportExceptionsFile" runat="server" />
             <asp:Button ID="hdnBtnGetImportExceptionsFile" runat="server" CssClass="HiddenButton" OnClick="hdnBtnGetImportExceptionsFile_Click"/>
            
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
            <asp:HiddenField ID="hdnFromHome" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
