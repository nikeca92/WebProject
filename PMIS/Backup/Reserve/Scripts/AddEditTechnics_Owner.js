function TabLoaded_Owner()
{
    SetDriverInfoVisibility();
}

function SetDriverInfoVisibility() 
{
    if (document.getElementById("fsDriverInfo") != null) 
    {
        if (parseInt(document.getElementById("hdnDriverReservistId").value) != 0) {
            document.getElementById("tblDriverInfoRow2").style.display = "";
            document.getElementById("tblDriverInfoRow3").style.display = "";
            document.getElementById("tblDriverInfoRow4").style.display = "";
            document.getElementById("tblDriverInfoRow5").style.display = "";
            document.getElementById("tblDriverInfoRow6").style.display = "none";

            document.getElementById("lblDriverFullName").style.display = "";
            document.getElementById("lblDriverFullNameValue").style.display = "";
            document.getElementById("lblDriverMilitaryRank").style.display = "";
            document.getElementById("lblDriverMilitaryRankValue").style.display = "";
        }
        else {
            document.getElementById("tblDriverInfoRow2").style.display = "none";
            document.getElementById("tblDriverInfoRow3").style.display = "none";
            document.getElementById("tblDriverInfoRow4").style.display = "none";
            document.getElementById("tblDriverInfoRow5").style.display = "none";

            document.getElementById("lblDriverFullName").style.display = "none";
            document.getElementById("lblDriverFullNameValue").style.display = "none";
            document.getElementById("lblDriverMilitaryRank").style.display = "none";
            document.getElementById("lblDriverMilitaryRankValue").style.display = "none";

            document.getElementById("tblDriverInfoRow6").style.display = "none";

            if (document.getElementById("txtDriverIdentNumber").value.Trim() != "") {
                document.getElementById("tblDriverInfoRow6").style.display = "";
            }
        }
    }
}


//Check the driver ident number only when it is changed
function DriverIdentNumberFocus()
{
    var txtDriverIdentNumber = document.getElementById("txtDriverIdentNumber");
    txtDriverIdentNumber.setAttribute("oldvalue", txtDriverIdentNumber.value);
}

//When the user type an DriverIdentNumber then check if this is an existing Reservist and
//take care about this
function DriverIdentNumberBlur()
{
    var txtDriverIdentNumber = document.getElementById("txtDriverIdentNumber");
    
    if (txtDriverIdentNumber.value != txtDriverIdentNumber.getAttribute("oldvalue"))
    {
        ClearAllMessages();
    
        var url = "AddEditTechnics_Owner.aspx?AjaxMethod=JSGetDriverInfo";
        var params = "";
        params += "IdentNumber=" + txtDriverIdentNumber.value;
        params += "&TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        
        function DriverIdentNumberBlur_Callback(xml)
        {
            var reservistId = parseInt(xmlValue(xml, "reservistId"));
            var driverFullName = xmlValue(xml, "driverFullName");
            var driverMilitaryRank = xmlValue(xml, "driverMilitaryRank");
            var driverPermCity = xmlValue(xml, "driverPermCity");
            var driverPermAddress = xmlValue(xml, "driverPermAddress");
            var driverHomePhone = xmlValue(xml, "driverHomePhone");
            var driverMobilePhone = xmlValue(xml, "driverMobilePhone");
            var driverEmail = xmlValue(xml, "driverEmail");
            var driverMilitaryReportSpecialties = xmlValue(xml, "driverMilitaryReportSpecialties");
            var driverDrvLicenseCategories = xmlValue(xml, "driverDrvLicenseCategories");
            var driverMilRepStatus = xmlValue(xml, "driverMilRepStatus");

            document.getElementById("hdnDriverReservistId").value = reservistId;
            document.getElementById("lblDriverFullNameValue").innerHTML = driverFullName;
            document.getElementById("lblDriverMilitaryRankValue").innerHTML = driverMilitaryRank;
            document.getElementById("lblDriverPermCityValue").innerHTML = driverPermCity;
            document.getElementById("lblDriverPermAddressValue").innerHTML = driverPermAddress;
            document.getElementById("lblDriverHomePhoneValue").innerHTML = driverHomePhone;
            document.getElementById("lblDriverMobilePhoneValue").innerHTML = driverMobilePhone;
            document.getElementById("lblDriverEmailValue").innerHTML = driverEmail;
            document.getElementById("lblDriverMilRepSpecialtiesValue").innerHTML = driverMilitaryReportSpecialties;
            document.getElementById("lblDriverDrivingLicenseCategoriesValue").innerHTML = driverDrvLicenseCategories;
            document.getElementById("lblDriverMilRepStatusValue").innerHTML = driverMilRepStatus;

            SetDriverInfoVisibility();
        }

        var myAJAX = new AJAX(url, true, params, DriverIdentNumberBlur_Callback);
        myAJAX.Call();
    }
}

//Save the technics's owner info
function SaveOwnerInfo(saveOwnerInfoFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabOwner")) {
        var url = "AddEditTechnics_Owner.aspx?AjaxMethod=JSSaveOwner";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&TechnicsTypeKey=" + custEncodeURI(document.getElementById(hdnTechnicsTypeKeyClientID).value);
        params += "&OwnershipLeasing=" + custEncodeURI(document.getElementById("chkOwnershipLeasing").checked ? "1" : "0");
        params += "&OwnershipCompanyId=" + custEncodeURI(document.getElementById("hdnCompanyID").value);
        params += "&DriverReservistId=" + custEncodeURI(document.getElementById("hdnDriverReservistId").value);

        var myAJAX = new AJAX(url, true, params, SaveOwnerInfo_Callback);
        myAJAX.Call();
    } else {
        saveOwnerInfoFinishCallback();
    }
    
    function SaveOwnerInfo_Callback(xml)
    {
        RefreshInputsOfSpecificContainer(document.getElementById(divOwnerClientID), true);
        Refresh_MilitaryReport_Postpone_OwnerInfo();

        saveOwnerInfoFinishCallback();
    }
}

//Check if the enetered owner information is valid
function IsOwnerDataValid(callbackIfValid)
{
    var tabNameHeader = "Собственик/Водач: ";
    var ValidationMessage = "";

    if (IsTabAlreadyVisited("btnTabOwner")) {
        var notValidFields = new Array();

        if (document.getElementById("fsDriverInfo") != null && parseInt(document.getElementById("hdnDriverReservistId").value.Trim()) == 0 &&
        document.getElementById("txtDriverIdentNumber").value != "") {
            ValidationMessage += tabNameHeader + "Не е намерен запис от резерва с ЕГН на водача" + "</br>";
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += tabNameHeader + noRightsMessage + "<br />";
        }

        callbackIfValid(ValidationMessage);
    } else {
        callbackIfValid(ValidationMessage);
    }
}

function Refresh_MilitaryReport_Postpone_OwnerInfo() {
    //If the dom element(s) are not there then the tab was not opened yet and then there is no need to reload
    if (!document.getElementById("lblPostponeOwnerValue"))
        return;

    var url = "AddEditTechnics_Owner.aspx?AjaxMethod=JSGetOwnerInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function JSGetWorkplaceInfo_Callback(xml) {
        var ownerInfo = xmlValue(xml, "ownerInfo");

        document.getElementById("lblPostponeOwnerValue").innerHTML = ownerInfo;
        document.getElementById("lblPostponeOwnerValueLightBox").innerHTML = ownerInfo;
    }

    var myAJAX = new AJAX(url, true, params, JSGetWorkplaceInfo_Callback);
    myAJAX.Call();
}

function CompanySelector_OnSelectedCompany(companyId, companyName, companyUnifiedIdentityCode, owneshipType) {
    //Set the selected company ID immediately. In that way, if the user somehow saves the screen very fast it will be saved
    document.getElementById("hdnCompanyID").value = companyId;
    //Refresh the labels for company name, ownership type and unified identity code once the other address fields are pulled from the server. In that way, all the info for the company will be displayed at once
    RefreshInfoForSelectedCompany(companyId, companyName, companyUnifiedIdentityCode, owneshipType);
}

function RefreshInfoForSelectedCompany(companyId, companyName, companyUnifiedIdentityCode, owneshipType) {
    var url = "AddEditTechnics_Owner.aspx?AjaxMethod=JSGetCompanyInfo";
    var params = "";
    params += "&CompanyId=" + document.getElementById("hdnCompanyID").value;
    params += "&TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;

    function JSGetCompanyInfo_Callback(xml) {
        var region = xmlValue(xml, "region");
        var municipality = xmlValue(xml, "municipality");
        var city = xmlValue(xml, "city");
        var district = xmlValue(xml, "district");
        var address = xmlValue(xml, "address");
        var postCode = xmlValue(xml, "postCode");
        var phone = xmlValue(xml, "phone");

        document.getElementById("lblCompanyNameValue").innerHTML = companyName;
        document.getElementById("lblUnifiedIdentityCodeValue").innerHTML = companyUnifiedIdentityCode;
        document.getElementById("lblOwnershipTypeValue").innerHTML = owneshipType;

        document.getElementById("lblOwnershipRegionValue").innerHTML = region;
        document.getElementById("lblOwnershipMunicipalityValue").innerHTML = municipality;
        document.getElementById("lblOwnershipCityValue").innerHTML = city;
        document.getElementById("lblOwnershipDistrictValue").innerHTML = district;
        document.getElementById("lblOwnershipAddressValue").innerHTML = address;
        document.getElementById("lblOwnershipPostCodeValue").innerHTML = postCode;
        document.getElementById("lblOwnershipPhoneValue").innerHTML = phone;

        document.getElementById("btnImgClearCompany").style.visibility = "visible";
    }

    var myAJAX = new AJAX(url, true, params, JSGetCompanyInfo_Callback);
    myAJAX.Call();
}

function ClearSelectedCompany() {
    document.getElementById("hdnCompanyID").value = "";

    document.getElementById("lblCompanyNameValue").innerHTML = "";
    document.getElementById("lblUnifiedIdentityCodeValue").innerHTML = "";
    document.getElementById("lblOwnershipTypeValue").innerHTML = "";

    document.getElementById("lblOwnershipRegionValue").innerHTML = "";
    document.getElementById("lblOwnershipMunicipalityValue").innerHTML = "";
    document.getElementById("lblOwnershipCityValue").innerHTML = "";
    document.getElementById("lblOwnershipDistrictValue").innerHTML = "";
    document.getElementById("lblOwnershipAddressValue").innerHTML = "";
    document.getElementById("lblOwnershipPostCodeValue").innerHTML = "";
    document.getElementById("lblOwnershipPhoneValue").innerHTML = "";
    
    //Use "hidden" to keep the position of the other elements on the same place when the button is not visible
    document.getElementById("btnImgClearCompany").style.visibility = "hidden";
}