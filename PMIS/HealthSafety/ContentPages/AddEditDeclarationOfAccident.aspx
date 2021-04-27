<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditDeclarationOfAccident.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditDeclarationOfAccident"
    Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript">

        var selectedTab = "btnTabEmpl";
        var TabEmplStatus = "Visited"

        var TabWorkerStatus = "NoVisited";
        var TabAccStatus = "NoVisited";
        var TabHarmStatus = "NoVisited";
        var TabWithStatus = "NoVisited"
        var TabHeirStatus = "NoVisited";

        var TypeOfDdl;

        var ValidationMessage;

        var TabEmplDisabledHiddenStatus = "Not Checked"
        var TabWorkerDisabledHiddenStatus = "Not Checked";
        var TabAccDisabledHiddenStatus = "Not Checked";
        var TabHarmDisabledHiddenStatus = "Not Checked";
        var TabWithDisabledHiddenStatus = "Not Checked"
        var TabHeirDisabledHiddenStatus = "Not Checked";

        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

        function PageLoad()
        {
            var btnPrintDecOfAcc = document.getElementById("<%= btnPrintDeclarationOfAccident.ClientID %>");
            var hfDeclarId = document.getElementById("<%= hfdeclarationId.ClientID %>");
            if (hfDeclarId.value != "" && hfDeclarId.value != "0")
            {
                btnPrintDecOfAcc.style.display = "";
            }
            else
            {
                btnPrintDecOfAcc.style.display = "none";
            }

            //Set maxlanth to textarea
            if (TabAccStatus != "NoVisited")
            {
                SetClientTextAreaMaxLength("txtAccTaskType", "1000");
                SetClientTextAreaMaxLength("txtAccDeviationFromTask", "1000");
                SetClientTextAreaMaxLength("txtAccInjurDesc", "2000");
            }
            if (TabHarmStatus != "NoVisited")
            {
                SetClientTextAreaMaxLength("txtHarmType", "4000");
                SetClientTextAreaMaxLength("txtHarmBodyParts", "4000");
            }

            //Set Logic for changes
            hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';

            // JSLoadData(selectedTab);
            document.getElementById(selectedTab).className = "ActiveTabTall";

            //Set visibility TabHeir
            document.getElementById('btnTabHeir').style.visibility = document.getElementById("<%= hdnTabHeirVisibility.ClientID %>").value;

            //Set Disable Save Button if Need
            var btnSaveSate = document.getElementById("<%= hfBtnSave.ClientID %>").value;
            if (btnSaveSate == "Disable")
            {
                document.getElementById('btnSave').className = "DisabledButton";
                document.getElementById('btnSave').attributes.removeNamedItem("onclick");
            }
        }

        function TabHover(tab)
        {
            if (tab.id != selectedTab)
            {
                tab.className = "HoverTabTall";
            }
        }

        function TabOut(tab)
        {
            if (tab.id != selectedTab)
            {
                tab.className = "InactiveTabTall";
            }
        }

        function TabClick(tab, initialload)
        {
            document.getElementById("lblMessage").innerHTML = "";
            if (selectedTab != "")
            {
                document.getElementById(selectedTab).className = "InactiveTabTall";
            }
            tab.className = "ActiveTabTall";
            selectedTab = tab.id;

            if (IsTabVisited())
            {
                JSLoadData(selectedTab);
            }
            else
            {
                ShowDiv();
            }
        }

        function IsTabVisited()
        {
            switch (selectedTab)
            {
                case "btnTabEmpl":
                    return false;
                    break;

                case "btnTabWorker":
                    if (TabWorkerStatus == "NoVisited")
                    {
                        TabWorkerStatus = "Visited"
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;

                case "btnTabAcc":
                    if (TabAccStatus == "NoVisited")
                    {
                        TabAccStatus = "Visited"
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    break;

                case "btnTabHarm":

                    if (TabHarmStatus == "NoVisited")
                    {
                        TabHarmStatus = "Visited"
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;

                case "btnTabWith":

                    if (TabWithStatus == "NoVisited")
                    {
                        TabWithStatus = "Visited"
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case "btnTabHeir":

                    if (TabHeirStatus == "NoVisited")
                    {
                        TabHeirStatus = "Visited"
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
        }

        function JSLoadData(selectedTabID)
        {
            var url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JSDisplayData";
            var params = "";
            params += "selectedTabID=" + selectedTabID;
            params += "&declarationId=" + document.getElementById("<%= hfdeclarationId.ClientID %>").value;
            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();

            RefreshDatePickers();
        }

        function response_handler(xml)
        {
            var disabledItems = "";
            var hiddenItems = "";

            switch (selectedTab)
            {
                case "btnTabEmpl":
                    document.getElementById("<%=divEmpl.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    break;

                case "btnTabWorker":
                    document.getElementById("<%=divWorker.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    disabledItems = document.getElementById('tabDisabledControlsWorker').value;
                    hiddenItems = document.getElementById('tabHiddenControlsWorker').value;
                    break;

                case "btnTabAcc":
                    document.getElementById("<%=divAcc.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    disabledItems = document.getElementById('tabDisabledControlsAcc').value;
                    hiddenItems = document.getElementById('tabHiddenControlsAcc').value;
                    break;

                case "btnTabHarm":
                    document.getElementById("<%=divHarm.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    disabledItems = document.getElementById('tabDisabledControlsHarm').value;
                    hiddenItems = document.getElementById('tabHiddenControlsHarm').value;
                    break;

                case "btnTabWith":
                    document.getElementById("<%=divWith.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    disabledItems = document.getElementById('tabDisabledControlsWith').value;
                    hiddenItems = document.getElementById('tabHiddenControlsWith').value;
                    break;

                case "btnTabHeir":
                    document.getElementById("<%=divHeir.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    disabledItems = document.getElementById('tabDisabledControlsHeir').value;
                    hiddenItems = document.getElementById('tabHiddenControlsHeir').value;
                    break;
                default:
                    break;
            }
            if (disabledItems != "")
            {
                document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
                CheckDisabledClientControls();
            }
            if (hiddenItems != "")
            {
                document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenItems;
                CheckHiddenClientControls();
            }

            RefreshDatePickers();
            ShowDiv();
            document.getElementById(selectedTab).className = "ActiveTabTall";

            AppendNewInputs();
        }

        function ShowDiv()
        {
            switch (selectedTab)
            {
                case "btnTabEmpl":
                    //Chek if we have any change about State of Client Controls
                    if (TabEmplDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabEmplDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divWorker.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divAcc.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHarm.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWith.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHeir.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divEmpl.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                case "btnTabWorker":
                    //Chek if we have any change about State of Client Controls
                    if (TabWorkerDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabWorkerDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divEmpl.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divAcc.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHarm.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWith.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHeir.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divWorker.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                case "btnTabAcc":
                    //Chek if we have any change about State of Client Controls
                    if (TabAccDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabAccDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divEmpl.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWorker.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHarm.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWith.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHeir.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divAcc.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                case "btnTabHarm":
                    //Chek if we have any change about State of Client Controls
                    if (TabHarmDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabHarmDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divEmpl.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWorker.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divAcc.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWith.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHeir.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divHarm.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                case "btnTabWith":
                    //Chek if we have any change about State of Client Controls
                    if (TabWithDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabWithDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divEmpl.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWorker.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divAcc.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHarm.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHeir.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divWith.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                case "btnTabHeir":
                    //Chek if we have any change about State of Client Controls
                    if (TabHeirDisabledHiddenStatus == "Not Checked")
                    {
                        ChekDisableHiddenState(selectedTab);
                        TabHeirDisabledHiddenStatus = "Cheked";
                    }
                    document.getElementById("<% =divEmpl.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWorker.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divAcc.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divHarm.ClientID %>").setAttribute("style", "display:none");
                    document.getElementById("<% =divWith.ClientID %>").setAttribute("style", "display:none");

                    document.getElementById("<%=divHeir.ClientID %>").setAttribute("style", "visibility:visible");

                    break;
                default:
                    break;
            }
            RefreshDatePickers();
        }

        function ChekDisableHiddenState(selectedTabID)
        {
            var url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JSChekDisableHiddenState";
            var params = "";
            params += "selectedTabID=" + selectedTabID;
            params += "&declarationId=" + document.getElementById("<%= hfdeclarationId.ClientID %>").value;
            
            function response_handler(xml)
            {
                //Set New Disabled Fields
                var disabledItems = xmlValue(xml, "disabledControlsList");
                if (disabledItems != "")
                {
                    document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
                    CheckDisabledClientControls();
                }
                //Set New Hidden Fields
                var hiddenItems = xmlValue(xml, "hiddenControlsList")
                if (hiddenItems !== "")
                {
                    document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenItems;
                    CheckHiddenClientControls();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function ddlChange(ddlObject, ddlType)
        {

            TypeOfDdl = ddlType;
            var cheked = false
            var selectedItemId;
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


            //We have all nesseary parameters : selectedTab, selectedItemId and type of selected DDl Type
            // 1 - Oblast
            // 2 - Obstina
            // 3 - NMA     

            var url;
            switch (ddlType)
            {
                case 1: //Oblast
                    url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JS_Region";
                    break;
                case 2: //Obstina
                    url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JS_Municipality";
                    break;
                case 3: //NMA
                    url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JS_City";
                    break;
                default:
                    break;
            }
            var params = "";

            params += "selectedItemId=" + selectedItemId;

            if ((selectedItemId != "") && (selectedItemId != -1))
            {
                var myAJAX = new AJAX(url, true, params, response_handler_Dll);
                myAJAX.Call();
            }
            else
            {
                if (ddlType == 2)
                {
                    switch (selectedTab)
                    {
                        case "btnTabEmpl":
                            ClearSelectList(document.getElementById("ddlEmplCity"));
                            break;
                        case "btnTabWorker":
                            ClearSelectList(document.getElementById("ddlWCity"));
                            break;
                        case "btnTabAcc":
                            ClearSelectList(document.getElementById("ddlAccCity"));
                            break;
                        case "btnTabWith":
                            ClearSelectList(document.getElementById("ddlWithCity"));
                            break;
                        case "btnTabHeir":
                            ClearSelectList(document.getElementById("ddlHeirCity"));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        function response_handler_Dll(xml)
        {

            switch (selectedTab)
            {
                case "btnTabEmpl":

                    switch (TypeOfDdl)
                    {
                        case 1: //Region

                            ClearSelectList(document.getElementById("ddlEmpMunicipality"));

                            var DTM = xml.getElementsByTagName("municipality");

                            for (var i = 0; i < DTM.length; i++)
                            {
                                var DTMId = DTM[i].getElementsByTagName("municipalityId")[0].childNodes[0].nodeValue;
                                var DTMFullName = DTM[i].getElementsByTagName("municipalyName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlEmpMunicipality"), DTMId, DTMFullName);
                            }

                            ClearSelectList(document.getElementById("ddlEmplCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlEmplCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtEmplPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 2: //Municipality

                            ClearSelectList(document.getElementById("ddlEmplCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlEmplCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtEmplPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 3: //City
                            //bind only PostCode
                            document.getElementById("txtEmplPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;
                        default:
                            break;
                    }

                    break;

                //-----------------------------------------------------------------------------------------------------                                                              
                case "btnTabWorker":

                    switch (TypeOfDdl)
                    {
                        case 1: //Region

                            ClearSelectList(document.getElementById("ddlWMunicipality"));

                            var DTM = xml.getElementsByTagName("municipality");

                            for (var i = 0; i < DTM.length; i++)
                            {
                                var DTMId = DTM[i].getElementsByTagName("municipalityId")[0].childNodes[0].nodeValue;
                                var DTMFullName = DTM[i].getElementsByTagName("municipalyName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWMunicipality"), DTMId, DTMFullName);
                            }

                            ClearSelectList(document.getElementById("ddlWCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtWPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 2: //Municipality

                            ClearSelectList(document.getElementById("ddlWCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtWPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 3: //City
                            //bind only PostCode
                            document.getElementById("txtWPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;
                        default:
                            break;
                    }

                    break;


                //---------------------------------------------------------------------------------------------------                                                                

                case "btnTabAcc":

                    switch (TypeOfDdl)
                    {
                        case 1: //Region

                            ClearSelectList(document.getElementById("ddlAccMunicipality"));

                            var DTM = xml.getElementsByTagName("municipality");

                            for (var i = 0; i < DTM.length; i++)
                            {
                                var DTMId = DTM[i].getElementsByTagName("municipalityId")[0].childNodes[0].nodeValue;
                                var DTMFullName = DTM[i].getElementsByTagName("municipalyName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlAccMunicipality"), DTMId, DTMFullName);
                            }

                            ClearSelectList(document.getElementById("ddlAccCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlAccCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtAccPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 2: //Municipality

                            ClearSelectList(document.getElementById("ddlAccCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlAccCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtAccPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 3: //City
                            //bind only PostCode
                            document.getElementById("txtAccPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;
                        default:
                            break;
                    }

                    break;


                //-------------------------------------------------------------------------------------------------                                                              

                case "btnTabWith":
                    switch (TypeOfDdl)
                    {
                        case 1: //Region

                            ClearSelectList(document.getElementById("ddlWithMunicipality"));

                            var DTM = xml.getElementsByTagName("municipality");

                            for (var i = 0; i < DTM.length; i++)
                            {
                                var DTMId = DTM[i].getElementsByTagName("municipalityId")[0].childNodes[0].nodeValue;
                                var DTMFullName = DTM[i].getElementsByTagName("municipalyName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWithMunicipality"), DTMId, DTMFullName);
                            }

                            ClearSelectList(document.getElementById("ddlWithCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWithCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtWitPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 2: //Municipality

                            ClearSelectList(document.getElementById("ddlWithCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlWithCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtWitPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 3: //City
                            //bind only PostCode
                            document.getElementById("txtWitPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;
                        default:
                            break;
                    }

                    break;

                //-------------------------------------------------------------------------------------------------------                                                              
                case "btnTabHeir":

                    switch (TypeOfDdl)
                    {
                        case 1: //Region

                            ClearSelectList(document.getElementById("ddlHeirMunicipality"));

                            var DTM = xml.getElementsByTagName("municipality");

                            for (var i = 0; i < DTM.length; i++)
                            {
                                var DTMId = DTM[i].getElementsByTagName("municipalityId")[0].childNodes[0].nodeValue;
                                var DTMFullName = DTM[i].getElementsByTagName("municipalyName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlHeirMunicipality"), DTMId, DTMFullName);
                            }

                            ClearSelectList(document.getElementById("ddlHeirCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlHeirCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtHeirPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 2: //Municipality

                            ClearSelectList(document.getElementById("ddlHeirCity"));

                            DTM = xml.getElementsByTagName("city");

                            for (i = 0; i < DTM.length; i++)
                            {
                                DTMId = DTM[i].getElementsByTagName("cityId")[0].childNodes[0].nodeValue;
                                DTMFullName = DTM[i].getElementsByTagName("cityName")[0].childNodes[0].nodeValue;

                                AddToSelectList(document.getElementById("ddlHeirCity"), DTMId, DTMFullName);
                            }

                            document.getElementById("txtHeirPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;

                        case 3: //City
                            //bind only PostCode
                            document.getElementById("txtHeirPostCode").value = xml.getElementsByTagName("postCode")[0].text
                            break;
                        default:
                            break;
                    }

                    break;

                default:
                    break;

            }

        }

        function GetAplicantTypeRadioValue()
        {
            var id = document.getElementById("<%=rblAplicantType.ClientID%>");
            var radioButtons = id.getElementsByTagName("input");
            for (var x = 0; x < radioButtons.length; x++)
            {
                if (radioButtons[x].checked)
                {
                    return radioButtons[x].value;
                }
            }

            return "";
        }

        function SetAplicantFieldsVisability()
        {
            var aplType = GetAplicantTypeRadioValue();
            if (aplType != "1")
            {
                document.getElementById("<%=txtAplicantEmplPosition.ClientID%>").style.display = "none";
                document.getElementById("<%=txtAplicantEmplPosition.ClientID%>").value = "";
                document.getElementById("<%=txtAplicantEmplName.ClientID%>").style.display = "none";
                document.getElementById("<%=txtAplicantEmplName.ClientID%>").value = "";
                document.getElementById("<%=lblAplicantEmplPositionInfo.ClientID%>").style.display = "none";
                document.getElementById("<%=lblAplicantEmplNameInfo.ClientID%>").style.display = "none";
            }
            else
            {
                document.getElementById("<%=txtAplicantEmplPosition.ClientID%>").style.display = "";
                document.getElementById("<%=txtAplicantEmplPosition.ClientID%>").style.width = "200px";
                document.getElementById("<%=txtAplicantEmplName.ClientID%>").style.display = "";
                document.getElementById("<%=txtAplicantEmplName.ClientID%>").style.width = "300px";
                document.getElementById("<%=lblAplicantEmplPositionInfo.ClientID%>").style.display = "";
                document.getElementById("<%=lblAplicantEmplNameInfo.ClientID%>").style.display = "";
            }
        }

        // Validate Declaration Item
        function ValidateDeclartionItem()
        {
            document.getElementById("lblMessage").innerHTML = "";
            ValidationMessage = "";

            var arrayDisabledHiddenMandatoryFields = [];

            //Validate mandatory Fileds by Defaults

            //1.Declaration Number
            if (document.getElementById("<%= txtDeclarationNumber.ClientID %>").value == "")
            {
                if ((document.getElementById("<%= txtDeclarationNumber.ClientID %>").isDisabled == true) ||
                   (document.getElementById("<%= txtDeclarationNumber.ClientID %>").style.display == "none"))
                {
                    arrayDisabledHiddenMandatoryFields.push("Декларация №");
                }
                else
                {
                    ValidationMessage += "</br>" + GetErrorMessageMandatory("Декларация №");
                }
            }
            //2.Declaration Date
            if (document.getElementById("<%= txtDeclarationDate.ClientID %>").value == "")
            {
                if ((document.getElementById("<%= txtDeclarationDate.ClientID %>").isDisabled == true) ||
                   (document.getElementById("<%= txtDeclarationDate.ClientID %>").style.display == "none"))
                {
                    arrayDisabledHiddenMandatoryFields.push("Дата");
                }
                else
                {
                    ValidationMessage += "</br>" + GetErrorMessageMandatory("Дата");
                }
            }
            else
            {
                if (!IsValidDate(document.getElementById("<%= txtDeclarationDate.ClientID %>").value))
                {
                    ValidationMessage += "</br>" + GetErrorMessageDate("Дата");
                }
            }

            //Aplicant
            var aplicantType = GetAplicantTypeRadioValue();
            if (aplicantType == "1")
            {
                //1.Aplicant Position
                if (document.getElementById("<%= txtAplicantEmplPosition.ClientID %>").value == "")
                {
                    if ((document.getElementById("<%= txtAplicantEmplPosition.ClientID %>").isDisabled == true) ||
                   (document.getElementById("<%= txtAplicantEmplPosition.ClientID %>").style.display == "none"))
                    {
                        arrayDisabledHiddenMandatoryFields.push("Длъжност на осигурителя");
                    }
                    else
                    {
                        ValidationMessage += "</br>" + GetErrorMessageMandatory("Длъжност на осигурителя");
                    }
                }
                //2.Aplicant Name
                if (document.getElementById("<%= txtAplicantEmplName.ClientID %>").value == "")
                {
                    if ((document.getElementById("<%= txtAplicantEmplName.ClientID %>").isDisabled == true) ||
                   (document.getElementById("<%= txtAplicantEmplName.ClientID %>").style.display == "none"))
                    {
                        arrayDisabledHiddenMandatoryFields.push("Имена на осигурителя");
                    }
                    else
                    {
                        ValidationMessage += "</br>" + GetErrorMessageMandatory("Имена на осигурителя");
                    }
                }
            }

            //3.Employeer
            if (GetSelectedItemId(document.getElementById("ddlEmployerId")) == -1)
            {
                if ((document.getElementById("ddlEmployerId").isDisabled == true) ||
                   (document.getElementById("ddlEmployerId").style.display == "none"))
                {
                    arrayDisabledHiddenMandatoryFields.push("Пълно наименование");
                }
                else
                {
                    ValidationMessage += "</br>Данни за осигурителя - " + GetErrorMessageMandatory("Пълно наименование");
                }
            }

            //4.WorkerFullName
            if (TabWorkerStatus == "Visited")
            {
                if (document.getElementById("txtWorkerFullName").value == "")
                {
                    if ((document.getElementById("txtWorkerFullName").isDisabled == true) ||
                   (document.getElementById("txtWorkerFullName").style.display == "none"))
                    {
                        arrayDisabledHiddenMandatoryFields.push("Трите имена на пострадалия");
                    }
                    else
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageMandatory("Трите имена на пострадалия");
                    }
                }
            }
            else
            {
                if (document.getElementById("<%= hfdeclarationId.ClientID %>").value == 0)
                {
                    ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageMandatory("Трите имена на пострадалия");
                }
            }

            if (arrayDisabledHiddenMandatoryFields.length > 0)
            {
                ValidationMessage += "</br>" + GetErrorMessageNoRights(arrayDisabledHiddenMandatoryFields);
            }

            //Validate other Fields

            //Tab Header 
            if (document.getElementById("<%= txtReferenceDate.ClientID %>").value != "")
            {
                if (!IsValidDate(document.getElementById("<%= txtReferenceDate.ClientID %>").value))
                {
                    ValidationMessage += "</br>" + GetErrorMessageMandatory("от дата");
                }
            }

            //Validate TabEml

            if (document.getElementById("txtEmplNumberOfEmployees").value != "")
            {
                if (!isInt(document.getElementById("txtEmplNumberOfEmployees").value))
                {
                    ValidationMessage += "</br>Данни за осигурителя - " + GetErrorMessageNumber("Списъчен брой на работниците и служителите");
                }
            }

            if (document.getElementById("txtEmplFemaleEmployees").value != "")
            {
                if (!isInt(document.getElementById("txtEmplFemaleEmployees").value))
                {
                    ValidationMessage += "</br>Данни за осигурителя - " + GetErrorMessageNumber("от тях жени");
                }
            }


            if (TabWorkerStatus == "Visited")
            {
                if (document.getElementById("txtWorkerEgn").value != "")
                {
                    if (!isValidEgn(document.getElementById("txtWorkerEgn").value))
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageNumber("ЕГН");
                    }
                }
                if (document.getElementById("txtWBirthDate").value != "")
                {
                    if (!IsValidDate(document.getElementById("txtWBirthDate").value))
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageDate("Дата на раждане");
                    }
                }
                if (document.getElementById("txtWHireDate").value != "")
                {
                    if (!IsValidDate(document.getElementById("txtWHireDate").value))
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageDate("Дата на постъпване");
                    }
                }
                if (document.getElementById("txtWYearsOnService").value != "")
                {
                    if (!isInt(document.getElementById("txtWYearsOnService").value))
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageNumber("Трудов стаж (години)");
                    }
                }
                if (document.getElementById("txtWCurrentJobYearsOnService").value != "")
                {
                    if (!isInt(document.getElementById("txtWCurrentJobYearsOnService").value))
                    {
                        ValidationMessage += "</br>Данни за пострадалия - " + GetErrorMessageNumber("Трудов стаж по посочената професия");
                    }
                }

            }

            if (TabAccStatus == "Visited")
            {
                var minHourday = 0;
                var valMin = "";
                var valHour = "";
                var valDay = "";

                if (document.getElementById("txtAccDateTimeHours").value != "")
                {
                    if (!isValidHour(document.getElementById("txtAccDateTimeHours").value))
                    {
                        valMin = "</br>Данни за злополуката - " + GetErrorMessageNumber("Злополуката е станала в (часа)");
                    }
                    minHourday++;
                }

                if (document.getElementById("txtAccDateTimeMinutes").value != "")
                {
                    if (!isValidMin(document.getElementById("txtAccDateTimeMinutes").value))
                    {
                        valHour = "</br>Данни за злополуката - " + GetErrorMessageNumber("Злополуката е станала в (минути)");
                    }
                    minHourday++;
                }

                if (document.getElementById("txtAccDateTimeDay").value != "")
                {
                    if (!IsValidDate(document.getElementById("txtAccDateTimeDay").value))
                    {
                        valDay = "</br>Данни за злополуката - " + GetErrorMessageDate("На дата");
                    }
                    minHourday++;
                }

                if ((minHourday == 1) || (minHourday == 2))
                {
                    ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageMandatory("Злополуката е станала в");
                }
                else
                {
                    ValidationMessage += valMin;
                    ValidationMessage += valHour;
                    ValidationMessage += valDay;
                }

                if (document.getElementById("txtAccWorkFromHour1").value != "")
                {
                    if (!isValidHour(document.getElementById("txtAccWorkFromHour1").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("Работно време от (часа)");
                    }
                }

                if (document.getElementById("txtAccWorkFromMin1").value != "")
                {
                    if (document.getElementById("txtAccWorkFromHour1").value == "")
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageMandatory("Работно време от (часа)");
                    }
                    if (!isValidMin(document.getElementById("txtAccWorkFromMin1").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("Работно време от (минути)");
                    }
                }

                if (document.getElementById("txtAccWorkToHour1").value != "")
                {
                    if (!isValidHour(document.getElementById("txtAccWorkToHour1").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("Работно време до (часа)");
                    }
                }

                if (document.getElementById("txtAccWorkToMin1").value != "")
                {
                    if (document.getElementById("txtAccWorkToHour1").value == "")
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageMandatory("Работно време до (часа)");
                    }

                    if (!isValidMin(document.getElementById("txtAccWorkToMin1").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("Работно време до (минути)");
                    }
                }

                if (document.getElementById("txtAccWorkFromHour2").value != "")
                {
                    if (!isValidHour(document.getElementById("txtAccWorkFromHour2").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("и от (часа)");
                    }
                }

                if (document.getElementById("txtAccWorkFromMin2").value != "")
                {
                    if (document.getElementById("txtAccWorkFromHour2").value == "")
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageMandatory("и от (часа)");
                    }
                    if (!isValidMin(document.getElementById("txtAccWorkFromMin2").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("и от (минути)");
                    }
                }

                if (document.getElementById("txtAccWorkToHour2").value != "")
                {
                    if (!isValidHour(document.getElementById("txtAccWorkToHour2").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("до (часа)");
                    }
                }
                if (document.getElementById("txtAccWorkToMin2").value != "")
                {
                    if (document.getElementById("txtAccWorkToHour2").value == "")
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageMandatory("до (часа)");
                    }
                    if (!isValidMin(document.getElementById("txtAccWorkToMin2").value))
                    {
                        ValidationMessage += "</br>Данни за злополуката - " + GetErrorMessageNumber("до (минути)");
                    }
                }

            }
            if (TabHarmStatus == "Visited")
            {

            }
            if (TabWithStatus == "Visited")
            {

            }
            if (TabHeirStatus == "Visited")
            {
                if (document.getElementById("txtHeirEgn").value != "")
                {
                    if (!isValidEgn(document.getElementById("txtHeirEgn").value))
                    {
                        ValidationMessage += "</br>Данни за наследника - " + GetErrorMessageNumber("ЕГН");
                    }
                }
            }

            if (ValidationMessage == "")
            {
                ForceNoChanges();
            }
            return (ValidationMessage == "");
        }

        function SaveDeclaration()
        {

            if (ValidateDeclartionItem())
            {
                ForceCheck();

                var url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JSSaveData";

                //Set DeclarationId in parameters
                var params = "&declarationId=" + document.getElementById("<%= hfdeclarationId.ClientID %>").value;
                params += "&tabShownNow=" + selectedTab;
                //GetInitialFiledsFromServerControl
                params += "&txtDeclarationNumber=" + document.getElementById("<%= txtDeclarationNumber.ClientID %>").value +
                "&txtReferenceNumber=" + document.getElementById("<%= txtReferenceNumber.ClientID %>").value +
                "&txtReferenceDate=" + document.getElementById("<%= txtReferenceDate.ClientID %>").value +
                "&txtDeclarationDate=" + document.getElementById("<%= txtDeclarationDate.ClientID %>").value +
                "&txtFileNumber=" + document.getElementById("<%= txtFileNumber.ClientID %>").value;

                var aplicantType = GetAplicantTypeRadioValue();
                if (aplicantType != "")
                {
                    params += "&rblAplicantType=" + aplicantType;
                    if (aplicantType == "1")
                    {

                        params += "&txtAplicantEmplPosition=" + document.getElementById("<%= txtAplicantEmplPosition.ClientID %>").value +
                        "&txtAplicantEmplName=" + document.getElementById("<%= txtAplicantEmplName.ClientID %>").value;
                    }
                }

                //Parameter for TabEml    
                params += "&ddlEmployerId=" + GetSelectedItemId(document.getElementById("ddlEmployerId")) +
                "&txtEmplEik=" + document.getElementById("txtEmplEik").value +
                "&ddlEmplCityId=" + GetSelectedItemId(document.getElementById("ddlEmplCity")) +
                "&txtEmplStreet=" + document.getElementById("txtEmplStreet").value +
                "&txtEmplStreetNum=" + document.getElementById("txtEmplStreetNum").value +
                "&txtEmplDistrict=" + document.getElementById("txtEmplDistrict").value +
                "&txtEmplBlock=" + document.getElementById("txtEmplBlock").value +
                "&txtEmplEntrance=" + document.getElementById("txtEmplEntrance").value +
                "&txtEmplFloor=" + document.getElementById("txtEmplFloor").value +
                "&txtEmplApt=" + document.getElementById("txtEmplApt").value +
                "&txtEmplPhone=" + document.getElementById("txtEmplPhone").value +
                "&txtEmplFax=" + document.getElementById("txtEmplFax").value +
                "&txtEmplEmail=" + document.getElementById("txtEmplEmail").value +
                "&txtEmplNumberOfEmployees=" + document.getElementById("txtEmplNumberOfEmployees").value +
                "&txtEmplFemaleEmployees=" + document.getElementById("txtEmplFemaleEmployees").value;

                //Parameter for TabWorker 
                if (TabWorkerStatus != "NoVisited") //tab was visited
                {
                    params += "&TabWorkerStatus=OK";

                    params += "&txtWorkerFullName=" + document.getElementById("txtWorkerFullName").value +
                 "&txtWorkerEgn=" + document.getElementById("txtWorkerEgn").value +
                 "&txtWStreet=" + document.getElementById("txtWStreet").value +
                 "&txtWStreetNum=" + document.getElementById("txtWStreetNum").value +
                 "&txtWDistrict=" + document.getElementById("txtWDistrict").value +
                 "&txtWBlock=" + document.getElementById("txtWBlock").value +
                 "&txtWEntrance=" + document.getElementById("txtWEntrance").value +
                 "&txtWFloor=" + document.getElementById("txtWFloor").value +
                 "&txtWApt=" + document.getElementById("txtWApt").value +
                 "&txtWPhone=" + document.getElementById("txtWPhone").value +
                 "&txtWFax=" + document.getElementById("txtWFax").value +
                 "&txtWEmail=" + document.getElementById("txtWEmail").value +
                 "&txtWBirthDate=" + document.getElementById("txtWBirthDate").value +
                 "&txtWCitizenship=" + document.getElementById("txtWCitizenship").value +

                 "&txtWHireDate=" + document.getElementById("txtWHireDate").value +
                 "&txtWJobTitle=" + document.getElementById("txtWJobTitle").value +
                 "&txtWJobCode=" + document.getElementById("txtWJobCode").value +

                 "&txtWYearsOnService=" + document.getElementById("txtWYearsOnService").value +
                 "&txtWCurrentJobYearsOnService=" + document.getElementById("txtWCurrentJobYearsOnService").value +
                 "&txtWBranch=" + document.getElementById("txtWBranch").value;

                    var ddlWCityId = "&ddlWCityId=" + GetSelectedItemId(document.getElementById("ddlWCity"));
                    params += ddlWCityId;

                    var rbWGender = "";
                    if (document.getElementById("rbWGender1").checked) //men
                    {
                        rbWGender = "&rbWGender=1";
                    }
                    if (document.getElementById("rbWGender2").checked) //men
                    {
                        rbWGender = "&rbWGender=2";
                    }
                    params += rbWGender;

                    var rbWHireType = "";
                    if (document.getElementById("rbWHireType1").checked) //limited
                    {
                        rbWHireType = "&rbWHireType=1";
                    }
                    if (document.getElementById("rbWHireType2").checked) //unlimited
                    {
                        rbWHireType = "&rbWHireType=2";
                    }
                    params += rbWHireType;

                    var rbWorkTime = "";
                    if (document.getElementById("rbWWorkTime1").checked) //full time
                    {
                        rbWorkTime = "&rbWorkTime=1";
                    }
                    if (document.getElementById("rbWWorkTime2").checked) //not full time
                    {
                        rbWorkTime = "&rbWorkTime=2";
                    }
                    params += rbWorkTime;

                    var rbWJobCategory = "";
                    if (document.getElementById("rbWJobCategory1").checked) //Category 1
                    {
                        rbWJobCategory = "&rbWJobCategory=1";
                    }
                    if (document.getElementById("rbWJobCategory2").checked) //Category 2
                    {
                        rbWJobCategory = "&rbWJobCategory=2";
                    }
                    if (document.getElementById("rbWJobCategory3").checked) //Category 3
                    {
                        rbWJobCategory = "&rbWJobCategory=3";
                    }
                    params += rbWJobCategory;

                }
                //Parameter for TabAcc
                if (TabAccStatus != "NoVisited")  //tab was visited
                {
                    params += "&TabAccStatus=OK";

                    params += "&txtAccDateTimeHours=" + document.getElementById("txtAccDateTimeHours").value +
                 "&txtAccDateTimeMinutes=" + document.getElementById("txtAccDateTimeMinutes").value +
                 "&txtAccDateTimeDay=" + document.getElementById("txtAccDateTimeDay").value +
                 "&txtAccWorkFromHour1=" + document.getElementById("txtAccWorkFromHour1").value +
                 "&txtAccWorkFromMin1=" + document.getElementById("txtAccWorkFromMin1").value +
                 "&txtAccWorkToHour1=" + document.getElementById("txtAccWorkToHour1").value +
                 "&txtAccWorkToMin1=" + document.getElementById("txtAccWorkToMin1").value +
                 "&txtAccWorkFromHour2=" + document.getElementById("txtAccWorkFromHour2").value +
                 "&txtAccWorkFromMin2=" + document.getElementById("txtAccWorkFromMin2").value +
                 "&txtAccWorkToHour2=" + document.getElementById("txtAccWorkToHour2").value +
                 "&txtAccWorkToMin2=" + document.getElementById("txtAccWorkToMin2").value +
                 "&txtAccPlace=" + document.getElementById("txtAccPlace").value +
                 "&txtAccCountry=" + document.getElementById("txtAccCountry").value +
                 "&txtAccStreet=" + document.getElementById("txtAccStreet").value +
                 "&txtAccStreetNum=" + document.getElementById("txtAccStreetNum").value +
                 "&txtAccDistrict=" + document.getElementById("txtAccDistrict").value +
                 "&txtAccBlock=" + document.getElementById("txtAccBlock").value +
                 "&txtAccEntrance=" + document.getElementById("txtAccEntrance").value +
                 "&txtAccFloor=" + document.getElementById("txtAccFloor").value +
                 "&txtAccApt=" + document.getElementById("txtAccApt").value +
                 "&txtAccPhone=" + document.getElementById("txtAccPhone").value +
                 "&txtAccFax=" + document.getElementById("txtAccFax").value +
                 "&txtAccEmail=" + document.getElementById("txtAccEmail").value +
                 "&txtAccHappenedAtOther=" + document.getElementById("txtAccHappenedAtOther").value +
                 "&txtAccJobType=" + document.getElementById("txtAccJobType").value +
                 "&txtAccTaskType=" + document.getElementById("txtAccTaskType").value +
                 "&txtAccDeviationFromTask=" + document.getElementById("txtAccDeviationFromTask").value +
                 "&txtAccInjurDesc=" + document.getElementById("txtAccInjurDesc").value +
                 "&txtAccPlannedActions=" + document.getElementById("txtAccPlannedActions").value +
                 "&txtAccLostDays=" + document.getElementById("txtAccLostDays").value;

                    var ddlAccCityId = "&ddlAccCity=" + GetSelectedItemId(document.getElementById("ddlAccCity"));
                    params += ddlAccCityId;

                    var rbAccHappenedAt = "";
                    if (document.getElementById("rbAccHappenedAt1").checked) //Category 1
                    {
                        rbAccHappenedAt = "&rbAccHappenedAt=1";
                        document.getElementById("txtAccHappenedAtOther").value = "";  //clear textbox from any value
                    }
                    if (document.getElementById("rbAccHappenedAt2").checked) //Category 2
                    {
                        rbAccHappenedAt = "&rbAccHappenedAt=2";
                        document.getElementById("txtAccHappenedAtOther").value = "";  //clear textbox from any value
                    }
                    if (document.getElementById("rbAccHappenedAt3").checked) //Category 3
                    {
                        rbAccHappenedAt = "&rbAccHappenedAt=3";
                    }
                    params += rbAccHappenedAt;

                    var rbAccInjHasRights = "";
                    if (document.getElementById("rbAccInjHasRights1").checked) //full time
                    {
                        rbAccInjHasRights = "&rbAccInjHasRights=1";
                    }
                    if (document.getElementById("rbAccInjHasRights2").checked) //Category 2
                    {
                        rbAccInjHasRights = "&rbAccInjHasRights=2";
                    }
                    if (document.getElementById("rbAccInjHasRights3").checked) //Category 3
                    {
                        rbAccInjHasRights = "&rbAccInjHasRights=3";
                    }
                    params += rbAccInjHasRights;


                    var rbAccLegalRef = "";
                    if (document.getElementById("rbAccLegalRef1").checked) //full time
                    {
                        rbAccLegalRef = "&rbAccLegalRef=1";
                    }
                    if (document.getElementById("rbAccLegalRef2").checked) //full time
                    {
                        rbAccLegalRef = "&rbAccLegalRef=2";
                    }
                    params += rbAccLegalRef;

                }

                //Parameter for TabHarm
                if (TabHarmStatus != "NoVisited")  //tab was visited
                {
                    params += "&TabHarmStatus=OK";

                    params += "&txtHarmType=" + document.getElementById("txtHarmType").value +
                     "&txtHarmBodyParts=" + document.getElementById("txtHarmBodyParts").value;

                    var rbHarmResult = "";
                    if (document.getElementById("rbHarmResult1").checked) //full time
                    {
                        rbHarmResult = "&rbHarmResult=1";
                    }
                    if (document.getElementById("rbHarmResult2").checked) //Category 2
                    {
                        rbHarmResult = "&rbHarmResult=2";
                    }
                    if (document.getElementById("rbHarmResult3").checked) //Category 2
                    {
                        rbHarmResult = "&rbHarmResult=3";
                    }
                    params += rbHarmResult;

                }
                //Parameter for TabWith
                if (TabWithStatus != "NoVisited")  //tab was visited
                {
                    params += "&TabWithStatus=OK";

                    params += "&txtWitnessFullName=" + document.getElementById("txtWitnessFullName").value +
                         "&txtWitStreet=" + document.getElementById("txtWitStreet").value +
                         "&txtWitStreetNum=" + document.getElementById("txtWitStreetNum").value +
                         "&txtWitDistrict=" + document.getElementById("txtWitDistrict").value +
                         "&txtWitBlock=" + document.getElementById("txtWitBlock").value +
                         "&txtWitEntrance=" + document.getElementById("txtWitEntrance").value +
                         "&txtWitFloor=" + document.getElementById("txtWitFloor").value +
                         "&txtWitApt=" + document.getElementById("txtWitApt").value +
                         "&txtWitPhone=" + document.getElementById("txtWitPhone").value +
                         "&txtWitFax=" + document.getElementById("txtWitFax").value +
                         "&txtWitEmail=" + document.getElementById("txtWitEmail").value;

                    var WitCityId = "&ddlWithCity=" + GetSelectedItemId(document.getElementById("ddlWithCity"));

                    params += WitCityId;


                }
                //Parameter for TabHeir
                if (TabHeirStatus != "NoVisited")  //tab was visited
                {
                    params += "&TabHeirStatus=OK";

                    params += "&txtHeirFullName=" + document.getElementById("txtHeirFullName").value +
                         "&txtHeirEgn=" + document.getElementById("txtHeirEgn").value +
                         "&txtHeirStreet=" + document.getElementById("txtHeirStreet").value +
                         "&txtHeirStreetNum=" + document.getElementById("txtHeirStreetNum").value +
                         "&txtHeirDistrict=" + document.getElementById("txtHeirDistrict").value +
                         "&txtHeirBlock=" + document.getElementById("txtHeirBlock").value +
                         "&txtHeirEntrance=" + document.getElementById("txtHeirEntrance").value +
                         "&txtHeirFloor=" + document.getElementById("txtHeirFloor").value +
                         "&txtHeirApt=" + document.getElementById("txtHeirApt").value +
                         "&txtHeirPhone=" + document.getElementById("txtHeirPhone").value +
                         "&txtHeirFax=" + document.getElementById("txtHeirFax").value +
                         "&txtHeirEmail=" + document.getElementById("txtHeirEmail").value;

                    var HeirCity = "&ddlHeirCity=" + GetSelectedItemId(document.getElementById("ddlHeirCity"));

                    params += HeirCity;

                }


                function response_handler(xml)
                {
                    //Set New Disabled Fields
                    var disabledItems = xmlValue(xml, "disabledControlsList");
                    if (disabledItems != "")
                    {
                        document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
                        CheckDisabledClientControls();
                    }
                    //Set New Hidden Fields
                    var hiddenItems = xmlValue(xml, "hiddenControlsList")
                    if (hiddenItems !== "")
                    {
                        document.getElementById(hdnHiddenClientControls).value += hiddenItems;
                        CheckHiddenClientControls();
                    }

                    //Set LocationHash
                    document.getElementById("<%= hdnLocationHash.ClientID %>").value = xmlValue(xml, "locationHash");
                    //Set new value using function SetHash() for Refresh page F5
                    SetHash();
                    //Set  Value to redirect to other page withot promt about unsaved changes
                    document.getElementById("<%= hdnSavedChanges.ClientID %>").value = "True";
                    CheckForSavedChanges();

                    //Clear fields in Validation Message Label
                    document.getElementById("lblValidationMessage").innerHTML = "";
                    //Get Message   
                    document.getElementById("lblMessage").innerHTML = xmlValue(xml, "resultOperation");
                    //Set New declarationId after Insert and print button
                    var btnPrintDecOfAcc = document.getElementById("<%= btnPrintDeclarationOfAccident.ClientID %>");
                    var hfDeclarId = document.getElementById("<%= hfdeclarationId.ClientID %>");
                    hfDeclarId.value = xmlValue(xml, "newDeclarationId");
                    if (hfDeclarId.value != "" && hfDeclarId.value != "0")
                    {
                        var lblHeaderCell = document.getElementById("<%= lblHeaderCell.ClientID %>");
                        lblHeaderCell.innerHTML = "Редактиране на декларация за злополука";
                        document.title = lblHeaderCell.innerHTML;

                        btnPrintDecOfAcc.style.display = "";
                    }
                    else
                    {
                        btnPrintDecOfAcc.style.display = "none";
                    }
                    switch (document.getElementById("lblMessage").innerHTML)
                    {
                        case "Декларация за трудова злополука е добавенa":
                            document.getElementById("lblMessage").className = "SuccessText"
                            break;
                        case "Данните на декларация за трудова злополука са обновени":
                            document.getElementById("lblMessage").className = "SuccessText"
                            break;
                        case "Декларация за трудова злополука не е добавенa":
                            document.getElementById("lblMessage").className = "ErrorText"
                            break;
                        case "Данните на декларация за трудова злополука не са обновени":
                            document.getElementById("lblMessage").className = "ErrorText"
                            break;
                        default:
                            break
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
            else
            {
                //Show Validation Message
                document.getElementById("lblValidationMessage").innerHTML = ValidationMessage;
                document.getElementById("lblValidationMessage").className = "ErrorText";
            }
        }


        function GetSelectedItemId(ddlObject)
        {
            var selectedItemId = "";

            if (ddlObject[0] == null) return selectedItemId;

            var cheked = false;
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

        function ddlChangeEmpl(ddlObject)
        {
            var selectedEmplId = GetSelectedItemId(ddlObject);
            if (selectedEmplId == -1) return;
            var url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JSGetEmplData";
            var params = "&employerId=" + selectedEmplId;

            function response_handler3(xml)
            {
                document.getElementById("txtEmplEik").value = xmlValue(xml, "EmplEik");
                document.getElementById("txtEmplStreet").value = xmlValue(xml, "EmplStreet");
                document.getElementById("txtEmplStreetNum").value = xmlValue(xml, "EmplStreetNum");
                document.getElementById("txtEmplDistrict").value = xmlValue(xml, "EmplDistrict");
                document.getElementById("txtEmplBlock").value = xmlValue(xml, "EmplBlock");
                document.getElementById("txtEmplEntrance").value = xmlValue(xml, "EmplEntrance");
                document.getElementById("txtEmplFloor").value = xmlValue(xml, "EmplFloor");
                document.getElementById("txtEmplApt").value = xmlValue(xml, "EmplApt");
                document.getElementById("txtEmplPhone").value = xmlValue(xml, "EmplPhone");
                document.getElementById("txtEmplFax").value = xmlValue(xml, "EmplFax");
                document.getElementById("txtEmplEmail").value = xmlValue(xml, "EmplEmail");
                document.getElementById("txtEmplNumberOfEmployees").value = xmlValue(xml, "EmplNumberOfEmployees");
                document.getElementById("txtEmplFemaleEmployees").value = xmlValue(xml, "EmplFemaleEmployees");
                document.getElementById("txtEmplPostCode").value = xmlValue(xml, "PostCode");

                var regionId = xmlValue(xml, "RegionId");
                var municipalityId = xmlValue(xml, "MunicipalityId");
                var cityId = xmlValue(xml, "CityId");

                //Bind ddlEmpMunicipality
                ClearSelectList(document.getElementById("ddlEmpMunicipality"));

                var DTM = xml.getElementsByTagName("municipality");

                for (i = 0; i < DTM.length; i++)
                {
                    DTMId = xmlValue(DTM[i], "municipalityId");
                    DTMFullName = xmlValue(DTM[i], "municipalityName")

                    AddToSelectList(document.getElementById("ddlEmpMunicipality"), DTMId, DTMFullName);
                }

                //Bind ddlEmpMunicipality
                ClearSelectList(document.getElementById("ddlEmplCity"));

                DTM = xml.getElementsByTagName("city");

                for (i = 0; i < DTM.length; i++)
                {
                    DTMId = xmlValue(DTM[i], "cityId");
                    DTMFullName = xmlValue(DTM[i], "cityName")

                    AddToSelectList(document.getElementById("ddlEmplCity"), DTMId, DTMFullName);
                }

                //Now set selected items
                SetSelectedItemId(document.getElementById("ddlEmpRegion"), regionId);
                SetSelectedItemId(document.getElementById("ddlEmpMunicipality"), municipalityId);
                SetSelectedItemId(document.getElementById("ddlEmplCity"), cityId);
            }

            var myAJAX = new AJAX(url, true, params, response_handler3);
            myAJAX.Call();
        }

        function SetSelectedItemId(ddlObject, selectedItemId)
        {
            var cheked = false;
            var i = 0;
            do
            {
                if (ddlObject[i].value == selectedItemId)
                {
                    cheked = true;
                    ddlObject[i].selected = true;
                }
                i++;

            } while (!cheked)
        }

        function TabHerVisibility(vsbl)
        {
            if (vsbl == 1)
            {
                document.getElementById('btnTabHeir').style.visibility = "visible";
                document.getElementById("<%= hdnTabHeirVisibility.ClientID %>").value = "visible";
            }
            else
            {
                document.getElementById('btnTabHeir').style.visibility = "hidden";
                document.getElementById("<%= hdnTabHeirVisibility.ClientID %>").value = "hidden";

                //Clear all fields in this tab
                if (TabHeirStatus != "NoVisited")
                {
                    //                document.getElementById("txtHeirFullName").value ="";
                    //                document.getElementById("txtHeirEgn").value ="";
                    //                document.getElementById("txtHeirStreet").value ="";
                    //                document.getElementById("txtHeirStreetNum").value ="";
                    //                document.getElementById("txtHeirDistrict").value ="";
                    //                document.getElementById("txtHeirBlock").value ="";
                    //                document.getElementById("txtHeirEntrance").value ="";
                    //                document.getElementById("txtHeirFloor").value ="";
                    //                document.getElementById("txtHeirApt").value ="";
                    //                document.getElementById("txtHeirPhone").value ="";
                    //                document.getElementById("txtHeirFax").value ="";
                    //                document.getElementById("txtHeirEmail").value ="";

                    TabHeirStatus = "NoVisited";
                }
            }
        }

        function SetOldPostCode(object)
        {
            object.setAttribute('oldPostCode', object.value);
        }
        function FillCityFromPostCode(object)
        {

            var oldPostCode = object.getAttribute('oldPostCode');

            var postCode;
            var ddlRegion;
            var ddlMunicipality;
            var ddlCity;

            postCode = object.value;

            if (oldPostCode != postCode)
            {
                switch (selectedTab)
                {
                    case "btnTabEmpl":
                        ddlRegion = "ddlEmpRegion";
                        ddlMunicipality = "ddlEmpMunicipality";
                        ddlCity = "ddlEmplCity";
                        break;
                    case "btnTabWorker":
                        ddlRegion = "ddlWRegion";
                        ddlMunicipality = "ddlWMunicipality";
                        ddlCity = "ddlWCity";
                        break;
                    case "btnTabAcc":
                        ddlRegion = "ddlAccRegion";
                        ddlMunicipality = "ddlAccMunicipality";
                        ddlCity = "ddlAccCity";
                        break;
                    case "btnTabWith":
                        ddlRegion = "ddlWithRegion";
                        ddlMunicipality = "ddlWithMunicipality";
                        ddlCity = "ddlWithCity";
                        break;
                    case "btnTabHeir":
                        ddlRegion = "ddlHeirRegion";
                        ddlMunicipality = "ddlHeirMunicipality";
                        ddlCity = "ddlHeirCity";
                        break;
                    default:
                        break;
                }

                var url = "AddEditDeclarationOfAccident.aspx?AjaxMethod=JSGetCityFromPostCode";
                var params = "&postCode=" + postCode;

                function response_handler_PostCode(xml)
                {
                    var statusResult = xmlValue(xml, "statusResult");

                    if (statusResult == "NO") return;

                    var regionId = xmlValue(xml, "selectedRegionId");
                    var municipalityId = xmlValue(xml, "selectedMunicipalityID");
                    var cityId = xmlValue(xml, "selectedCityId");

                    //Bind ddlEmpMunicipality
                    ClearSelectList(document.getElementById(ddlMunicipality));

                    var DTM = xml.getElementsByTagName("municipality");

                    for (i = 0; i < DTM.length; i++)
                    {
                        DTMId = xmlValue(DTM[i], "municipalityId");
                        DTMFullName = xmlValue(DTM[i], "municipalyName")

                        AddToSelectList(document.getElementById(ddlMunicipality), DTMId, DTMFullName);
                    }

                    //Bind ddlEmpMunicipality
                    ClearSelectList(document.getElementById(ddlCity));

                    DTM = xml.getElementsByTagName("city");

                    for (i = 0; i < DTM.length; i++)
                    {
                        DTMId = xmlValue(DTM[i], "cityId");
                        DTMFullName = xmlValue(DTM[i], "cityName")

                        AddToSelectList(document.getElementById(ddlCity), DTMId, DTMFullName);
                    }

                    //Now set selected items
                    SetSelectedItemId(document.getElementById(ddlRegion), regionId);
                    SetSelectedItemId(document.getElementById(ddlMunicipality), municipalityId);
                    SetSelectedItemId(document.getElementById(ddlCity), cityId);
                }

                var myAJAX = new AJAX(url, true, params, response_handler_PostCode);
                myAJAX.Call();
            }
        }

        function ShowPrintDeclarationOfAccident()
        {
            var hfDeclarationId = document.getElementById("<%= hfdeclarationId.ClientID %>").value;

            var url = "";
            var pageName = "PrintDeclarationOfAccident"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?DeclarationOfAccidentID=" + hfDeclarationId;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }

        function SetVisibility(rbNumber)
        {
            if (rbNumber == 3)
            {
                document.getElementById("txtAccHappenedAtOther").style.visibility = "visible";
            }
            else
            {
                document.getElementById("txtAccHappenedAtOther").style.visibility = "hidden";
            }
        }
  
    </script>

    <asp:HiddenField ID="hfdeclarationId" runat="server" />
    <asp:HiddenField ID="hdnLocationHash" runat="server" />
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
    <asp:HiddenField ID="hdnTabHeirVisibility" runat="server" />
    <asp:HiddenField ID="hfBtnSave" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <center style="width: 100%">
        <div style="height: 20px">
        </div>
        <table class="ManagedNoticeOfAccident">
            <tr style="min-height: 30px">
                <td>
                </td>
                <td>
                </td>
                <td colspan="4>" align="center">
                    <span style="padding-left: 50px" class="HeaderText">Информация попълвана от служител
                        в ТП на НОИ:</span>
                </td>
            </tr>
            <tr style="min-height: 30px">
                <td align="right">
                    <asp:Label runat="server" ID="lblDeclarationNumber" class="InputLabel">Декларация №: </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDeclarationNumber" Style="width: 135px" MaxLength="100" runat="server" CssClass="RequiredInputField"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblReferenceNumber" CssClass="InputLabel">Входящ №: </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtReferenceNumber" Style="width: 65px" MaxLength="200" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:Label runat="server" ID="lblReferenceDate" CssClass="InputLabel">от дата:</asp:Label>
                    <asp:TextBox ID="txtReferenceDate" Style="width: 75px" MaxLength="10" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr style="min-height: 30px">
                <td align="right">
                    <asp:Label runat="server" ID="lblDeclarationDate" CssClass="InputLabel">Дата:</asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDeclarationDate" Style="width: 75px" MaxLength="10" runat="server"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblFileNumber" CssClass="InputLabel">Досие №:</asp:Label>
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtFileNumber" Style="width: 65px" MaxLength="200" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div>
            <br />
            <asp:Label ID="lblHeaderCell" runat="server" CssClass="HeaderText"></asp:Label>
            <br />
            <span>Приложение към чл.3, ал.1 от Наредбата за установяване, разследване, регистриране
                и отчитане на трудовите злополуки</span>
        </div>
        <table class="Div1" width="800px" cellspacing="0" cellpadding="0">
            <tr>
                <td align="center" style="width: 100%">
                    <div id="TabSummary">
                        <ul>
                            <li class="InactiveTabTall" id="btnTabEmpl" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                    Данни за осигурителя</a></li>
                            <li class="InactiveTabTall" id="btnTabWorker" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                    Данни за пострадалия</a></li>
                            <li class="InactiveTabTall" id="btnTabAcc" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                    Данни за злополуката</a></li>
                            <li class="InactiveTabTall" id="btnTabHarm" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                    Данни за увреждането</a></li>
                            <li class="InactiveTabTall" id="btnTabWith" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                    Данни за свидетеля</a></li>
                            <li class="InactiveTabTall" id="btnTabHeir" onclick="TabClick(this, false);" onmouseover="TabHover(this);"
                                onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px;">
                                    Данни за наследника</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="DeclarationTabsBottomLine" />
                </td>
            </tr>
        </table>
        <div id="divEmpl" class="Div1" runat="server">
        </div>
        <div id="divWorker" class="Div1" runat="server">
        </div>
        <div id="divAcc" class="Div1" runat="server">
        </div>
        <div id="divHarm" class="Div1" runat="server">
        </div>
        <div id="divWith" class="Div1" runat="server">
        </div>
        <div id="divHeir" class="Div1" runat="server">
        </div>
        <br />
        <table>
            <tr style="min-height: 30px">
                <td colspan="3>" align="left">
                    <asp:Label runat="server" ID="lblAplicantinfo" CssClass="SmallHeaderText">Декларацията се подава от:</asp:Label>
                </td>
            </tr>
            <tr style="min-height: 15px">
                <td rowspan="3" align="left" style="width: 170px; padding-left: 100px;">
                    <asp:RadioButtonList ID="rblAplicantType" runat="server">
                        <asp:ListItem Value="1" Text="Осигурителя" onclick="SetAplicantFieldsVisability()"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Пострадалия" onclick="SetAplicantFieldsVisability()"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Наследника" onclick="SetAplicantFieldsVisability()"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td align="left" style="min-width: 210px;">
                    <asp:TextBox ID="txtAplicantEmplPosition" Style="width: 200px" MaxLength="300" runat="server"></asp:TextBox>
                </td>
                <td align="left" style="min-width: 310px;">
                    <asp:TextBox ID="txtAplicantEmplName" Style="width: 300px" MaxLength="500" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label runat="server" ID="lblAplicantEmplPositionInfo" CssClass="ThinLabel">длъжност</asp:Label>
                </td>
                <td align="center">
                    <asp:Label runat="server" ID="lblAplicantEmplNameInfo" CssClass="ThinLabel">имена</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <div id="lblValidationMessage" style="text-align: center;">
        </div>
        <span id="lblMessage"></span>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <div id="btnSave" style="display: inline;" onclick="SaveDeclaration();" class="Button">
                        <i></i>
                        <div style="width: 70px; display: inline">
                            Запис</div>
                        <b></b>
                    </div>
                    <asp:LinkButton ID="btnPrintDeclarationOfAccident" runat="server" CssClass="Button"
                        OnClientClick="ShowPrintDeclarationOfAccident(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <%-- <input id="btnSave" type="button" class="Button" onclick="SaveDeclaration()" style="width: 70px;
                    padding-left: 5px" validationgroup="Save" value="Запис"></input>--%>
                    <div style="padding-left: 45px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnCancel" runat="server" CheckForChanges="true" CssClass="Button"
                        OnClick="btnCancel_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
