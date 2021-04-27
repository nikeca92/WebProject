var isNewPerson = true;
var isValidIdentNumber = false;

function TabLoaded_PersonalData()
{
    LoadPickLists_PersonalData();
    SetEventHandlers();

    SetClientTextAreaMaxLength("txtPermAddress", "500");
    SetClientTextAreaMaxLength("txtCurrAddress", "1500");
    SetClientTextAreaMaxLength("txtParentsContact", "2000");
}

function LoadPickLists_PersonalData()
{
    var configPickListDrvLicCategories =
    {
        width: 175,
        allLabel: "<Всички>"
    }

    categories = document.getElementById("hdnDrvLicCategoriesClientID").value;
    categories = eval(categories);
    PickListUtil.AddPickList("pickListDrvLicCategories", categories, "divPickListDrvLicCategories", configPickListDrvLicCategories);

    //UIItems
    if (document.getElementById("lblDrvLicCategories").disabled)
    {
        PickListUtil.DisablePickList("pickListDrvLicCategories", false, true);
    }
}

//This function load the person's details by ReservistId
function LoadPersonalDetailsByReservistId()
{
    ClearAllMessages();

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadPersonalDetailsByReservistId";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    
    function LoadPersonalDetailsByReservistId_CallBack(xml)
    {
        var person = xml.getElementsByTagName("person")[0];

        var personId = xmlValue(person, "personId");
        if (personId != 0)
        {
            isNewPerson = false;
            isValidIdentNumber = true;
        }
        else
        {
            isNewPerson = true;
            isValidIdentNumber = false;
        }

        var identNumber = xmlValue(person, "identNumber");
        var resMilRepStatus = xmlValue(person, "resMilRepStatus");
        var currMilDepartment = xmlValue(person, "currMilDepartment");
        var personStatus = xmlValue(person, "personStatus");
        var firstName = xmlValue(person, "firstName");
        var lastName = xmlValue(person, "lastName");
        var initials = xmlValue(person, "initials");
        var militaryRankName = xmlValue(person, "militaryRankName");
        var militaryRankID = xmlValue(person, "militaryRankId");
        var militaryRankDR = xmlValue(person, "militaryRankDR") == "1" ? true : false;

        ActivateMilitaryRankEditing(personId);
        
        var militaryCategoryName = xmlValue(person, "militaryCategoryName");
        var lastModified = xmlValue(person, "lastModified");
        var IDCardNumber = xmlValue(person, "IDCardNumber");
        var IDCardIssuedBy = xmlValue(person, "IDCardIssuedBy");
        var IDCardIssueDate = xmlValue(person, "IDCardIssueDate");
        var genderId = xmlValue(person, "genderId");
        var birthCountryId = xmlValue(xml, "birthCountryId");
        var birthCityId = xmlValue(xml, "birthCityId");
        var birthMunicipalityId = xmlValue(xml, "birthMunicipalityId");
        var birthRegionId = xmlValue(xml, "birthRegionId");
        var birthPostCode = xmlValue(xml, "birthPostCode");
        var birthCityIfAbroad = xmlValue(xml, "birthCityIfAbroad");
        var birthAbroad = xmlValue(xml, "birthAbroad");
        var drivingLicenseCategories = xmlValue(person, "drivingLicenseCategories");
        var hasMilitarySrv = xmlValue(person, "hasMilitarySrv");        
        var militaryTraining = xmlValue(person, "militaryTraining");
        var recordOfServiceSeries = xmlValue(person, "recordOfServiceSeries");
        var recordOfServiceNumber = xmlValue(person, "recordOfServiceNumber");
        var recordOfServiceDate = xmlValue(person, "recordOfServiceDate");
        var recordOfServiceCopy = xmlValue(person, "recordOfServiceCopy");
        var permCityId = xmlValue(person, "permCityId");
        var permPostCode = xmlValue(person, "permPostCode");
        var permSecondPostCode = xmlValue(person, "permSecondPostCode");
        var permRegionId = xmlValue(person, "permRegionId");
        var permMunicipalityId = xmlValue(person, "permMunicipalityId");
        var permDistrictId = xmlValue(person, "permDistrictId");
        var permAddress = xmlValue(person, "permAddress");
        var currCityId = xmlValue(person, "currCityId");
        var currPostCode = xmlValue(person, "currPostCode");
        var presSecondPostCode = xmlValue(person, "presSecondPostCode");
        var currRegionId = xmlValue(person, "currRegionId");
        var currMunicipalityId = xmlValue(person, "currMunicipalityId");
        var currDistrictId = xmlValue(person, "currDistrictId");
        var currAddress = xmlValue(person, "currAddress");
        var homePhone = xmlValue(person, "homePhone");
        var mobilePhone = xmlValue(person, "mobilePhone");
        var businessPhone = xmlValue(person, "businessPhone");
        var email = xmlValue(person, "email");
        var maritalStatus = xmlValue(person, "maritalStatus");
        var parentsContact = xmlValue(person, "parentsContact");
        var childCount = xmlValue(person, "childCount");
        var sizeClothingId = xmlValue(person, "sizeClothingId");
        var sizeHatId = xmlValue(person, "sizeHatId");
        var sizeShoesId = xmlValue(person, "sizeShoesId");
        var personHeight = xmlValue(person, "personHeight");
        var isAbroad = xmlValue(person, "isAbroad");
        var abroadCountryId = xmlValue(person, "abroadCountryId");
        var abroadSince = xmlValue(person, "abroadSince");
        var abroadPeriod = xmlValue(person, "abroadPeriod");
        
        document.getElementById(hdnPersonIdClientID).value = personId;

        document.getElementById("lblMilitaryReportStatusValue").innerHTML = resMilRepStatus;
        document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
        document.getElementById("lblPersonStatusValue").innerHTML = personStatus;
        document.getElementById("txtIdentNumber").value = identNumber;
        document.getElementById("lblIdentNumberValue").innerHTML = identNumber;
        document.getElementById("txtFirstName").value = firstName;
        document.getElementById("txtLastName").value = lastName;
        document.getElementById("txtInitials").innerHTML = initials;
        document.getElementById("lblMilitaryRankValue").innerHTML = militaryRankName;
        document.getElementById("chkMilitaryRankDR").checked = militaryRankDR;
        
        
        document.getElementById("hdnMilitaryRankID").value = militaryRankID;
        document.getElementById("lblMilitaryCategoryValue").innerHTML = militaryCategoryName;
        document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

        //If the Personal Data tab is visible
        if (document.getElementById("txtIDCardNumber"))
        {
            document.getElementById("txtIDCardNumber").value = IDCardNumber;
            document.getElementById("txtIDCardIssuedBy").value = IDCardIssuedBy;
            document.getElementById("txtIDCardIssueDate").value = IDCardIssueDate;
            document.getElementById("ddGender").value = genderId;
            document.getElementById("ddBirthCountry").value = birthCountryId;
            ddBirthCountry_Changed();

            if (parseInt(birthAbroad) == 1)
            {
                document.getElementById("txtBirthCityIfAbroad").value = birthCityIfAbroad;
            }
            else
            {
                document.getElementById("txtBirthPostCode").value = birthPostCode;
                document.getElementById("ddBirthRegion").value = birthRegionId;

                ClearSelectList(document.getElementById("ddBirthMunicipality"), true);

                var b_municipalities = xml.getElementsByTagName("b_m");

                for (var i = 0; i < b_municipalities.length; i++)
                {
                    var id = xmlValue(b_municipalities[i], "id");
                    var name = xmlValue(b_municipalities[i], "name");

                    AddToSelectList(document.getElementById("ddBirthMunicipality"), id, name);
                };

                document.getElementById("ddBirthMunicipality").value = birthMunicipalityId;


                ClearSelectList(document.getElementById("ddBirthCity"), true);

                var b_cities = xml.getElementsByTagName("b_c");

                for (var i = 0; i < b_cities.length; i++)
                {
                    var id = xmlValue(b_cities[i], "id");
                    var name = xmlValue(b_cities[i], "name");

                    AddToSelectList(document.getElementById("ddBirthCity"), id, name);
                };

                document.getElementById("ddBirthCity").value = birthCityId;
            }

            PickListUtil.SetSelection("pickListDrvLicCategories", drivingLicenseCategories);
            SetRadioGroupValueByName("hasMilitarySrv", hasMilitarySrv);            
            SetRadioGroupValueByName("MilitaryTraining", militaryTraining);
            document.getElementById("txtRecordOfServiceSeries").value = recordOfServiceSeries;
            document.getElementById("txtRecordOfServiceNumber").value = recordOfServiceNumber;
            document.getElementById("txtRecordOfServiceDate").value = recordOfServiceDate;
            document.getElementById("chkRecordOfServiceCopy").checked = (recordOfServiceCopy == "1");
            document.getElementById("hdnRecordOfServiceSeries").value = recordOfServiceSeries;
            document.getElementById("hdnRecordOfServiceNumber").value = recordOfServiceNumber;
            document.getElementById("hdnRecordOfServiceDate").value = recordOfServiceDate;
            document.getElementById("hdnRecordOfServiceCopy").value = recordOfServiceCopy;

            document.getElementById("txtPermPostCode").value = permSecondPostCode;

            //If there is a Perm City then load the entire info
            if (permCityId != "-1")
            {
                document.getElementById("ddPermRegion").value = permRegionId;

                ClearSelectList(document.getElementById("ddPermMunicipality"), true);

                var p_municipalities = xml.getElementsByTagName("p_m");

                for (var i = 0; i < p_municipalities.length; i++)
                {
                    var id = xmlValue(p_municipalities[i], "id");
                    var name = xmlValue(p_municipalities[i], "name");

                    AddToSelectList(document.getElementById("ddPermMunicipality"), id, name);
                };

                document.getElementById("ddPermMunicipality").value = permMunicipalityId;


                ClearSelectList(document.getElementById("ddPermCity"), true);

                var p_cities = xml.getElementsByTagName("p_c");

                for (var i = 0; i < p_cities.length; i++)
                {
                    var id = xmlValue(p_cities[i], "id");
                    var name = xmlValue(p_cities[i], "name");

                    AddToSelectList(document.getElementById("ddPermCity"), id, name);
                };

                document.getElementById("ddPermCity").value = permCityId;


                ClearSelectList(document.getElementById("ddPermDistrict"), true);

                var p_districts = xml.getElementsByTagName("p_d");

                for (var i = 0; i < p_districts.length; i++)
                {
                    var id = xmlValue(p_districts[i], "id");
                    var name = xmlValue(p_districts[i], "name");

                    AddToSelectList(document.getElementById("ddPermDistrict"), id, name);
                };

                if (permDistrictId != "-1")
                    document.getElementById("ddPermDistrict").value = permDistrictId;
            }
            else
            {
                document.getElementById("ddPermRegion").selectedIndex = 0;
                ClearSelectList(document.getElementById("ddPermMunicipality"), false);
                ClearSelectList(document.getElementById("ddPermCity"), false);
                ClearSelectList(document.getElementById("ddPermDistrict"), false);
            }

            document.getElementById("txtPermAddress").value = permAddress;

            document.getElementById("txtCurrPostCode").value = presSecondPostCode;

            //If there is a Curr City then load the entire info
            if (currCityId != "-1")
            {
                document.getElementById("ddCurrRegion").value = currRegionId;

                ClearSelectList(document.getElementById("ddCurrMunicipality"), true);

                var c_municipalities = xml.getElementsByTagName("c_m");

                for (var i = 0; i < c_municipalities.length; i++)
                {
                    var id = xmlValue(c_municipalities[i], "id");
                    var name = xmlValue(c_municipalities[i], "name");

                    AddToSelectList(document.getElementById("ddCurrMunicipality"), id, name);
                };

                document.getElementById("ddCurrMunicipality").value = currMunicipalityId;


                ClearSelectList(document.getElementById("ddCurrCity"), true);

                var c_cities = xml.getElementsByTagName("c_c");

                for (var i = 0; i < c_cities.length; i++)
                {
                    var id = xmlValue(c_cities[i], "id");
                    var name = xmlValue(c_cities[i], "name");

                    AddToSelectList(document.getElementById("ddCurrCity"), id, name);
                };

                document.getElementById("ddCurrCity").value = currCityId;


                ClearSelectList(document.getElementById("ddCurrDistrict"), true);

                var c_districts = xml.getElementsByTagName("c_d");

                for (var i = 0; i < c_districts.length; i++)
                {
                    var id = xmlValue(c_districts[i], "id");
                    var name = xmlValue(c_districts[i], "name");

                    AddToSelectList(document.getElementById("ddCurrDistrict"), id, name);
                };

                if (currDistrictId != "-1")
                    document.getElementById("ddCurrDistrict").value = currDistrictId;
            }
            else
            {
                document.getElementById("ddCurrRegion").selectedIndex = 0;
                ClearSelectList(document.getElementById("ddCurrMunicipality"), false);
                ClearSelectList(document.getElementById("ddCurrCity"), false);
                ClearSelectList(document.getElementById("ddCurrDistrict"), false);
            }

            document.getElementById("txtCurrAddress").value = currAddress;
            document.getElementById("txtHomePhone").value = homePhone;
            document.getElementById("txtMobilePhone").value = mobilePhone;
            document.getElementById("txtBusinessPhone").value = businessPhone;
            document.getElementById("txtEmail").value = email;
            document.getElementById("ddMaritalStatus").value = maritalStatus;
            document.getElementById("txtParentsContact").value = parentsContact;
            document.getElementById("txtChildCount").value = childCount;
            document.getElementById("ddSizeClothing").value = sizeClothingId;
            document.getElementById("ddSizeHat").value = sizeHatId;
            document.getElementById("ddSizeShoes").value = sizeShoesId;
            document.getElementById("txtPersonHeight").value = personHeight;
            document.getElementById("chkIsAbroad").checked = (isAbroad == "1");
            chkIsAbroad_Click();
            document.getElementById("ddAbroadCountry").value = abroadCountryId;
            document.getElementById("txtAbroadSince").value = abroadSince;
            document.getElementById("txtAbroadPeriod").value = abroadPeriod;

            LoadRecordOfServiceArchivesSection();
            LoadConvictionSection();
            LoadDualCitizenshipSection();
        }

        isLoadedPersonalData = true;
        LoadOriginalValues();
        ShowContent();
    }

    var myAJAX = new AJAX(url, true, params, LoadPersonalDetailsByReservistId_CallBack);
    myAJAX.Call();
}

function JSLoadPersonalDetailsByIdentNumber(identNumber) 
{
    ClearAllMessages();

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadPersonalDetailsByIdentNumber";
    var params = "";
    params += "IdentNumber=" + identNumber;
    var myAJAX = new AJAX(url, true, params, LoadPersonalDetailsByPersonIdOrIdentNumber_CallBack);
    myAJAX.Call();
}

var tempPersonId = 0;
//This function load the person's details by PersonId
function LoadPersonalDetailsByPersonId(personId) {
    tempPersonId = personId;
    ClearAllMessages();

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadPersonalDetailsByPersonId";
    var params = "";
    params += "PersonId=" + personId;
    var myAJAX = new AJAX(url, true, params, LoadPersonalDetailsByPersonIdOrIdentNumber_CallBack);
    myAJAX.Call();
}

function LoadPersonalDetailsByPersonIdOrIdentNumber_CallBack(xml) {
    var person = xml.getElementsByTagName("person")[0];

    var identNumber = xmlValue(person, "identNumber");

    if (tempPersonId != 0) 
    {
        isNewPerson = false;
        isValidIdentNumber = true;
    }
    else {
        isNewPerson = true;
        
        if (identNumber != "") 
        {
            isValidIdentNumber = true;
        }
        else 
        {
            isValidIdentNumber = false;
        }
    }

    tempPersonId = 0;

    var resMilRepStatus = xmlValue(person, "resMilRepStatus");
    var currMilDepartment = xmlValue(person, "currMilDepartment");
    var personStatus = xmlValue(person, "personStatus");
    var firstName = xmlValue(person, "firstName");
    var lastName = xmlValue(person, "lastName");
    var initials = xmlValue(person, "initials");
    var militaryRankId = xmlValue(person, "militaryRankId");
    var militaryRankName = xmlValue(person, "militaryRankName");
    
    var militaryCategoryName = xmlValue(person, "militaryCategoryName");
    var lastModified = xmlValue(person, "lastModified");
    var IDCardNumber = xmlValue(person, "IDCardNumber");
    var IDCardIssuedBy = xmlValue(person, "IDCardIssuedBy");
    var IDCardIssueDate = xmlValue(person, "IDCardIssueDate");
    var genderId = xmlValue(person, "genderId");
    var birthCountryId = xmlValue(xml, "birthCountryId");
    var birthCityId = xmlValue(xml, "birthCityId");
    var birthMunicipalityId = xmlValue(xml, "birthMunicipalityId");
    var birthRegionId = xmlValue(xml, "birthRegionId");
    var birthPostCode = xmlValue(xml, "birthPostCode");
    var birthCityIfAbroad = xmlValue(xml, "birthCityIfAbroad");
    var birthAbroad = xmlValue(xml, "birthAbroad");
    var drivingLicenseCategories = xmlValue(person, "drivingLicenseCategories");
    var hasMilitarySrv = xmlValue(person, "hasMilitarySrv");    
    var militaryTraining = xmlValue(person, "militaryTraining");
    var recordOfServiceSeries = xmlValue(person, "recordOfServiceSeries");
    var recordOfServiceNumber = xmlValue(person, "recordOfServiceNumber");
    var recordOfServiceDate = xmlValue(person, "recordOfServiceDate");
    var recordOfServiceCopy = xmlValue(person, "recordOfServiceCopy");
    var permCityId = xmlValue(person, "permCityId");
    var permPostCode = xmlValue(person, "permPostCode");
    var permSecondPostCode = xmlValue(person, "permSecondPostCode");
    var permRegionId = xmlValue(person, "permRegionId");
    var permMunicipalityId = xmlValue(person, "permMunicipalityId");
    var permDistrictId = xmlValue(person, "permDistrictId");
    var permAddress = xmlValue(person, "permAddress");
    var currCityId = xmlValue(person, "currCityId");
    var currPostCode = xmlValue(person, "currPostCode");
    var presSecondPostCode = xmlValue(person, "presSecondPostCode");
    var currRegionId = xmlValue(person, "currRegionId");
    var currMunicipalityId = xmlValue(person, "currMunicipalityId");
    var currDistrictId = xmlValue(person, "currDistrictId");
    var currAddress = xmlValue(person, "currAddress");
    var homePhone = xmlValue(person, "homePhone");
    var mobilePhone = xmlValue(person, "mobilePhone");
    var businessPhone = xmlValue(person, "businessPhone");
    var email = xmlValue(person, "email");
    var maritalStatus = xmlValue(person, "maritalStatus");
    var parentsContact = xmlValue(person, "parentsContact");
    var childCount = xmlValue(person, "childCount");
    var sizeClothingId = xmlValue(person, "sizeClothingId");
    var sizeHatId = xmlValue(person, "sizeHatId");
    var sizeShoesId = xmlValue(person, "sizeShoesId");
    var personHeight = xmlValue(person, "personHeight");
    var isAbroad = xmlValue(person, "isAbroad");
    var abroadCountryId = xmlValue(person, "abroadCountryId");
    var abroadSince = xmlValue(person, "abroadSince");
    var abroadPeriod = xmlValue(person, "abroadPeriod");
    var militaryUnit = xmlValue(person, "militaryUnit");
    var militaryRankDR = xmlValue(person, "militaryRankDR") == "1" ? true : false;
    
    document.getElementById("lblMilitaryReportStatusValue").innerHTML = resMilRepStatus;
    document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
    document.getElementById("lblPersonStatusValue").innerHTML = personStatus;
    document.getElementById("txtFirstName").value = firstName;
    document.getElementById("txtLastName").value = lastName;
    document.getElementById("txtInitials").innerHTML = initials;
    document.getElementById("lblMilitaryRankValue").innerHTML = militaryRankName;
    document.getElementById("chkMilitaryRankDR").checked = militaryRankDR;
    
    document.getElementById("hdnMilitaryRankID").value = militaryRankId;
    document.getElementById("lblMilitaryCategoryValue").innerHTML = militaryCategoryName;
    document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

    document.getElementById(lblCurrentMilitaryUnitValueClientID).innerHTML = militaryUnit;

    if (!initials || TrimString(initials) == "")
        PopulateInitials();

    //If the Personal Data tab is visible
    if (document.getElementById("txtIDCardNumber")) {
        document.getElementById("txtIDCardNumber").value = IDCardNumber;
        document.getElementById("txtIDCardIssuedBy").value = IDCardIssuedBy;
        document.getElementById("txtIDCardIssueDate").value = IDCardIssueDate;
        document.getElementById("ddGender").value = genderId;
        document.getElementById("ddBirthCountry").value = birthCountryId;
        ddBirthCountry_Changed();

        if (parseInt(birthAbroad) == 1) {
            document.getElementById("txtBirthCityIfAbroad").value = birthCityIfAbroad;
        }
        else {
            document.getElementById("txtBirthPostCode").value = birthPostCode;
            document.getElementById("ddBirthRegion").value = birthRegionId;

            ClearSelectList(document.getElementById("ddBirthMunicipality"), true);

            var b_municipalities = xml.getElementsByTagName("b_m");

            for (var i = 0; i < b_municipalities.length; i++) {
                var id = xmlValue(b_municipalities[i], "id");
                var name = xmlValue(b_municipalities[i], "name");

                AddToSelectList(document.getElementById("ddBirthMunicipality"), id, name);
            };

            document.getElementById("ddBirthMunicipality").value = birthMunicipalityId;


            ClearSelectList(document.getElementById("ddBirthCity"), true);

            var b_cities = xml.getElementsByTagName("b_c");

            for (var i = 0; i < b_cities.length; i++) {
                var id = xmlValue(b_cities[i], "id");
                var name = xmlValue(b_cities[i], "name");

                AddToSelectList(document.getElementById("ddBirthCity"), id, name);
            };

            document.getElementById("ddBirthCity").value = birthCityId;
        }

        PickListUtil.SetSelection("pickListDrvLicCategories", drivingLicenseCategories);
        SetRadioGroupValueByName("hasMilitarySrv", hasMilitarySrv);        
        SetRadioGroupValueByName("MilitaryTraining", militaryTraining);
        document.getElementById("txtRecordOfServiceSeries").value = recordOfServiceSeries;
        document.getElementById("txtRecordOfServiceNumber").value = recordOfServiceNumber;
        document.getElementById("txtRecordOfServiceDate").value = recordOfServiceDate;
        document.getElementById("chkRecordOfServiceCopy").checked = (recordOfServiceCopy == "1");
        document.getElementById("hdnRecordOfServiceSeries").value = recordOfServiceSeries;
        document.getElementById("hdnRecordOfServiceNumber").value = recordOfServiceNumber;
        document.getElementById("hdnRecordOfServiceDate").value = recordOfServiceDate;
        document.getElementById("hdnRecordOfServiceCopy").value = recordOfServiceCopy;
        
        document.getElementById("txtPermPostCode").value = permSecondPostCode;

        //If there is a Perm City then load the entire info
        if (permCityId != "-1") {            
            document.getElementById("ddPermRegion").value = permRegionId;

            ClearSelectList(document.getElementById("ddPermMunicipality"), true);

            var p_municipalities = xml.getElementsByTagName("p_m");

            for (var i = 0; i < p_municipalities.length; i++) {
                var id = xmlValue(p_municipalities[i], "id");
                var name = xmlValue(p_municipalities[i], "name");

                AddToSelectList(document.getElementById("ddPermMunicipality"), id, name);
            };

            document.getElementById("ddPermMunicipality").value = permMunicipalityId;


            ClearSelectList(document.getElementById("ddPermCity"), true);

            var p_cities = xml.getElementsByTagName("p_c");

            for (var i = 0; i < p_cities.length; i++) {
                var id = xmlValue(p_cities[i], "id");
                var name = xmlValue(p_cities[i], "name");

                AddToSelectList(document.getElementById("ddPermCity"), id, name);
            };

            document.getElementById("ddPermCity").value = permCityId;


            ClearSelectList(document.getElementById("ddPermDistrict"), true);

            var p_districts = xml.getElementsByTagName("p_d");

            for (var i = 0; i < p_districts.length; i++) {
                var id = xmlValue(p_districts[i], "id");
                var name = xmlValue(p_districts[i], "name");

                AddToSelectList(document.getElementById("ddPermDistrict"), id, name);
            };

            if (permDistrictId != "-1")
                document.getElementById("ddPermDistrict").value = permDistrictId;
        }
        else {
            document.getElementById("ddPermRegion").selectedIndex = 0;
            ClearSelectList(document.getElementById("ddPermMunicipality"), false);
            ClearSelectList(document.getElementById("ddPermCity"), false);
            ClearSelectList(document.getElementById("ddPermDistrict"), false);
        }

        document.getElementById("txtPermAddress").value = permAddress;

        document.getElementById("txtCurrPostCode").value = presSecondPostCode;

        //If there is a Curr City then load the entire info
        if (currCityId != "-1") {
            document.getElementById("ddCurrRegion").value = currRegionId;

            ClearSelectList(document.getElementById("ddCurrMunicipality"), true);

            var c_municipalities = xml.getElementsByTagName("c_m");

            for (var i = 0; i < c_municipalities.length; i++) {
                var id = xmlValue(c_municipalities[i], "id");
                var name = xmlValue(c_municipalities[i], "name");

                AddToSelectList(document.getElementById("ddCurrMunicipality"), id, name);
            };

            document.getElementById("ddCurrMunicipality").value = currMunicipalityId;


            ClearSelectList(document.getElementById("ddCurrCity"), true);

            var c_cities = xml.getElementsByTagName("c_c");

            for (var i = 0; i < c_cities.length; i++) {
                var id = xmlValue(c_cities[i], "id");
                var name = xmlValue(c_cities[i], "name");

                AddToSelectList(document.getElementById("ddCurrCity"), id, name);
            };

            document.getElementById("ddCurrCity").value = currCityId;


            ClearSelectList(document.getElementById("ddCurrDistrict"), true);

            var c_districts = xml.getElementsByTagName("c_d");

            for (var i = 0; i < c_districts.length; i++) {
                var id = xmlValue(c_districts[i], "id");
                var name = xmlValue(c_districts[i], "name");

                AddToSelectList(document.getElementById("ddCurrDistrict"), id, name);
            };

            if (permDistrictId != "-1")
                document.getElementById("ddCurrDistrict").value = currDistrictId;
        }
        else {
            document.getElementById("ddCurrRegion").selectedIndex = 0;
            ClearSelectList(document.getElementById("ddCurrMunicipality"), false);
            ClearSelectList(document.getElementById("ddCurrCity"), false);
            ClearSelectList(document.getElementById("ddCurrDistrict"), false);
        }

        document.getElementById("txtCurrAddress").value = currAddress;
        document.getElementById("txtHomePhone").value = homePhone;
        document.getElementById("txtMobilePhone").value = mobilePhone;
        document.getElementById("txtBusinessPhone").value = businessPhone;
        document.getElementById("txtEmail").value = email;
        document.getElementById("ddMaritalStatus").value = maritalStatus;
        document.getElementById("txtParentsContact").value = parentsContact;
        document.getElementById("txtChildCount").value = childCount;
        document.getElementById("ddSizeClothing").value = sizeClothingId;
        document.getElementById("ddSizeHat").value = sizeHatId;
        document.getElementById("ddSizeShoes").value = sizeShoesId;
        document.getElementById("txtPersonHeight").value = personHeight;
        document.getElementById("chkIsAbroad").checked = (isAbroad == "1");
        chkIsAbroad_Click();
        document.getElementById("ddAbroadCountry").value = abroadCountryId;
        document.getElementById("txtAbroadSince").value = abroadSince;
        document.getElementById("txtAbroadPeriod").value = abroadPeriod;
    }
}

//Check the ident number only when it is changed
function IdentNumberFocus()
{
    var txtIdentNumber = document.getElementById("txtIdentNumber");
    txtIdentNumber.setAttribute("oldvalue", txtIdentNumber.value);
}

//When the user type an IdentNumber then check if this is an existing Reservist and/or Person and
//take care about the particular case
function IdentNumberBlur()
{
    var txtIdentNumber = document.getElementById("txtIdentNumber");

    if (txtIdentNumber.value != txtIdentNumber.getAttribute("oldvalue"))
    {
        if (isOnlyDigits(txtIdentNumber.value))
        {
            var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSCheckIdentNumber";
            var params = "";
            params += "IdentNumber=" + document.getElementById("txtIdentNumber").value;
            var myAJAX = new AJAX(url, true, params, IdentNumberBlur_Callback);
            myAJAX.Call();
        }
        else
        {
            isValidIdentNumber = false;
            txtIdentNumber.value = txtIdentNumber.getAttribute("oldvalue");
        }
    }

    function IdentNumberBlur_Callback(xml) {
        var reservistId = parseInt(xmlValue(xml, "reservistId"));
        var personId = parseInt(xmlValue(xml, "personId"));
        isValidIdentNumber = parseInt(xmlValue(xml, "isValidIdentNumber")) == 1;
        var noAccess = parseInt(xmlValue(xml, "noAccess"));
        var isMilitaryReportingPerson = parseInt(xmlValue(xml, "isMilitaryReportingPerson"));

        if (!isValidIdentNumber) {
            txtIdentNumber.value = txtIdentNumber.getAttribute("oldvalue");
        }
        else if (reservistId > 0) //Redirect to the existing Reservist record                
        {
            JSRedirect("AddEditReservist.aspx?ReservistId=" + reservistId);
        }
        else {
            //Pre-load the existing Person data
            if (personId > 0) {
                if (noAccess == 1) {
                    JSRedirect("NoAccessToPerson.aspx?PersonId=" + personId + "&PageFrom=1");
                }
                else {
                    document.getElementById(hdnReservistIdClientID).value = reservistId;
                    document.getElementById(hdnPersonIdClientID).value = personId;

                    LoadPersonalDetailsByPersonId(personId);
                }
            }
        }
    }
}


//Save the personal details
function SavePersonalData(savePersonalDataFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabPersonalData"))
    {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSSavePersonalData";
        var params = "";

        params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
        params += "&PersonId=" + document.getElementById(hdnPersonIdClientID).value;
        
        params += "&IdentNumber=" + document.getElementById("txtIdentNumber").value;
        params += "&FirstName=" + custEncodeURI(document.getElementById("txtFirstName").value);
        params += "&LastName=" + custEncodeURI(document.getElementById("txtLastName").value);
        params += "&Initials=" + custEncodeURI(document.getElementById("txtInitials").innerHTML);
        params += "&MilitaryRankId=" + document.getElementById("hdnMilitaryRankID").value;
        params += "&IDCardNumber=" + custEncodeURI(document.getElementById("txtIDCardNumber").value);
        params += "&IDCardIssuedBy=" + custEncodeURI(document.getElementById("txtIDCardIssuedBy").value);
        params += "&IDCardIssueDate=" + document.getElementById("txtIDCardIssueDate").value;
        params += "&GenderId=" + document.getElementById("ddGender").value;
        params += "&BirthCountryId=" + document.getElementById("ddBirthCountry").value;
        params += "&IsBirthAbroad=" + (IsAbroad("ddBirthCountry") ? "1" : "0");
        params += "&BirthCityIfAbroad=" + custEncodeURI(document.getElementById("txtBirthCityIfAbroad").value);
        params += "&BirthCityId=" + document.getElementById("ddBirthCity").value;
        params += "&DrivingLicenseCategories=" + PickListUtil.GetSelectedValues("pickListDrvLicCategories");
        params += "&HasMilitarySrv=" + GetRadioGroupValueByName("hasMilitarySrv");        
        params += "&MilitaryTraining=" + GetRadioGroupValueByName("MilitaryTraining");
        params += "&RecordOfServiceSeries=" + custEncodeURI(document.getElementById("txtRecordOfServiceSeries").value);
        params += "&RecordOfServiceNumber=" + custEncodeURI(document.getElementById("txtRecordOfServiceNumber").value);
        params += "&RecordOfServiceDate=" + document.getElementById("txtRecordOfServiceDate").value;        
        params += "&RecordOfServiceCopy=" + (document.getElementById("chkRecordOfServiceCopy").checked ? "1" : "0");
        params += "&PermCityID=" + document.getElementById("ddPermCity").value;
        params += "&PermDistrictID=" + document.getElementById("ddPermDistrict").value;
        params += "&PermSecondPostCode=" + document.getElementById("txtPermPostCode").value;
        params += "&PermAddress=" + custEncodeURI(document.getElementById("txtPermAddress").value);
        params += "&PresCityID=" + document.getElementById("ddCurrCity").value;
        params += "&PresAddress=" + custEncodeURI(document.getElementById("txtCurrAddress").value);
        params += "&PresDistrictID=" + document.getElementById("ddCurrDistrict").value;
        params += "&PresSecondPostCode=" + document.getElementById("txtCurrPostCode").value;
        params += "&HomePhone=" + custEncodeURI(document.getElementById("txtHomePhone").value);
        params += "&MobilePhone=" + custEncodeURI(document.getElementById("txtMobilePhone").value);
        params += "&BusinessPhone=" + custEncodeURI(document.getElementById("txtBusinessPhone").value);
        params += "&Email=" + custEncodeURI(document.getElementById("txtEmail").value);
        params += "&MaritalStatusKey=" + custEncodeURI(document.getElementById("ddMaritalStatus").value);
        params += "&ParentsContact=" + custEncodeURI(document.getElementById("txtParentsContact").value);
        params += "&ChildCount=" + custEncodeURI(document.getElementById("txtChildCount").value);
        params += "&SizeClothingID=" + custEncodeURI(document.getElementById("ddSizeClothing").value);
        params += "&SizeHatID=" + custEncodeURI(document.getElementById("ddSizeHat").value);
        params += "&SizeShoesID=" + custEncodeURI(document.getElementById("ddSizeShoes").value);
        params += "&PersonHeight=" + custEncodeURI(document.getElementById("txtPersonHeight").value);
        params += "&IsAbroad=" + (document.getElementById("chkIsAbroad").checked ? "1" : "0");
        params += "&AbroadCountryId=" + custEncodeURI(document.getElementById("ddAbroadCountry").value);
        params += "&AbroadSince=" + custEncodeURI(document.getElementById("txtAbroadSince").value);
        params += "&AbroadPeriod=" + custEncodeURI(document.getElementById("txtAbroadPeriod").value);

        var myAJAX = new AJAX(url, true, params, SavePersonalData_Callback);
        myAJAX.Call();
    } else {
        savePersonalDataFinishCallback();
    }

    function SavePersonalData_Callback(xml)
    {
        var status = xmlValue(xml, "response");
        var reservistId = xmlValue(xml, "reservistId");
        var personId = xmlValue(xml, "personId");
        
        if (document.getElementById(hdnReservistIdClientID).value == 0)
        {
            document.getElementById(hdnReservistIdClientID).value = reservistId;
            document.getElementById(hdnPersonIdClientID).value = personId;
        
            location.hash = "AddEditReservist.aspx?ReservistId=" + reservistId;

            //2015-01-15: It was here before, but it was causing issues since we moved all tabs to be saved at once because
            //it was possible if the first tab to be saved, then show the tabs and open a the second tab before its save function is called
            //and it was causing to save blank companies
            //GoToEditMode();
        }
        else
        {
            document.getElementById(hdnReservistIdClientID).value = reservistId;
            document.getElementById(hdnPersonIdClientID).value = personId;
        }

        document.getElementById("hdnRecordOfServiceSeries").value = document.getElementById("txtRecordOfServiceSeries").value;
        document.getElementById("hdnRecordOfServiceNumber").value = document.getElementById("txtRecordOfServiceNumber").value;
        document.getElementById("hdnRecordOfServiceDate").value = document.getElementById("txtRecordOfServiceDate").value;        
        document.getElementById("hdnRecordOfServiceCopy").value = (document.getElementById("chkRecordOfServiceCopy").checked ? "1" : "0");

        RefreshInputsOfSpecificContainer(document.getElementById("tblPersonalDataHeader"), true);
        RefreshInputsOfSpecificContainer(document.getElementById(divPersonalDataClientID), true);

        //Refresh the military report status section because when changing the IsAbroad info then the person could become TEMPORARY_REMOVED
        MilitaryReport_Refresh_ReservistMilitaryReportStatusSection(function() {
            savePersonalDataFinishCallback();
        });
    }
   
}

//Check if the enetered personal data is valid
function IsPersonalDataValid()
{
    var tabNameHeader = "Лични данни: ";
    var ValidationMessage = "";

    if (IsTabAlreadyVisited("btnTabPersonalData")) {
        
        var notValidFields = new Array();

        if (isNewPerson) {
            if (document.getElementById("txtIdentNumber").value.Trim() == "") {
                
                if (document.getElementById("txtIdentNumber").disabled == true || document.getElementById("txtIdentNumber").style.display == "none") {
                    notValidFields.push("ЕГН");
                }
                else {
                    ValidationMessage += tabNameHeader + GetErrorMessageMandatory("ЕГН") + "</br>";
                }
            }
            else {
                if (!isValidIdentNumber) {
                    ValidationMessage += tabNameHeader + "Въведеното ЕГН е невалидно" + "</br>";
                }
            }
        }

        if (document.getElementById("txtFirstName").value.Trim() == "") {
            
            if (document.getElementById("txtFirstName").disabled == true || document.getElementById("txtFirstName").style.display == "none") {
                notValidFields.push("Име и презиме");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Име и презиме") + "</br>";
            }
        }

        if (document.getElementById("txtLastName").value.Trim() == "") {
           
            if (document.getElementById("txtLastName").disabled == true || document.getElementById("txtLastName").style.display == "none") {
                notValidFields.push("Фамилия");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Фамилия") + "</br>";
            }
        }

        if (document.getElementById("txtIDCardIssueDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtIDCardIssueDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Лична карта издадена на") + "</br>";
            }
        }

        if (document.getElementById("txtRecordOfServiceDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtRecordOfServiceDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Военна книжка издадена на") + "</br>";
            }
        }
    
        if (document.getElementById("ddPermCity").value.Trim() == optionChooseOneValue) {
            
            if (document.getElementById("ddPermCity").disabled == true || document.getElementById("ddPermCity").style.display == "none") {
                notValidFields.push("Населено място (постоянен адрес)");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Населено място (постоянен адрес)") + "</br>";
            }
        }

        if (document.getElementById("ddCurrCity").value.Trim() == optionChooseOneValue) {
            
            if (document.getElementById("ddCurrCity").disabled == true || document.getElementById("ddCurrCity").style.display == "none") {
                notValidFields.push("Населено място (настоящ адрес)");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Населено място (настоящ адрес)") + "</br>";
            }
        }

        if (document.getElementById("txtHomePhone").value.Trim() != "") {
            if (!isInt(document.getElementById("txtHomePhone").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Домашен телефон") + "</br>";
            }
        }

        if (document.getElementById("ddMaritalStatus").value.Trim() == optionChooseOneValue) {
            
            if (document.getElementById("ddMaritalStatus").disabled == true || document.getElementById("ddMaritalStatus").style.display == "none") {
                notValidFields.push("Семейно положение");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Семейно положение") + "</br>";
            }
        }

        if (document.getElementById("txtChildCount").value.Trim() != "") {
            if (!isInt(document.getElementById("txtChildCount").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Брой деца") + "</br>";
            }
        }

        if (document.getElementById("txtPersonHeight").value.Trim() != "") {
            if (!isInt(document.getElementById("txtPersonHeight").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Ръст") + "</br>";
            }
        }

        if (document.getElementById("txtAbroadSince").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtAbroadSince").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Начална дата на пребиваване в чужбина") + "</br>";
            }
        }

        if (document.getElementById("txtAbroadPeriod").value.Trim() != "") {
            if (!isInt(document.getElementById("txtAbroadPeriod").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Период на пребиваване в чужбина") + "</br>";
            }
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += tabNameHeader + noRightsMessage + "<br />";
        }
    }
    return ValidationMessage;
}

//If a new Reservist has been added the go to edit mode with this function
function GoToEditMode()
{
    ClearAllMessages();

    if (document.getElementById("btnTabEducationWork"))
        document.getElementById("btnTabEducationWork").style.display = "";

    if (document.getElementById("btnTabMilitaryReport"))
        document.getElementById("btnTabMilitaryReport").style.display = "";

    if (document.getElementById("btnTabMilitaryService"))
        document.getElementById("btnTabMilitaryService").style.display = "";

    if (document.getElementById("btnTabOtherInfo"))
        document.getElementById("btnTabOtherInfo").style.display = "";

    document.getElementById("txtIdentNumber").style.display = "none";
    document.getElementById("lblIdentNumberValue").style.display = "";
    document.getElementById("lblIdentNumberValue").innerHTML = document.getElementById("txtIdentNumber").value;
    
    document.getElementById(lblHeaderTitleClientID).innerHTML = "Редактиране на резервист";
    document.title = document.getElementById(lblHeaderTitleClientID).innerHTML;

    LoadRecordOfServiceArchivesSection();
    LoadConvictionSection();
    LoadDualCitizenshipSection();
    
    var personId = document.getElementById(hdnPersonIdClientID).value;
    ActivateMilitaryRankEditing(personId);
}

//When changing the Birth Country then hide/show the Birth City input area (if abroad / if not abroad)
function ddBirthCountry_Changed()
{
    DisplayBirthCityInputArea();
}

function IsAbroad(ddCountryId)
{
    var ddBirthCountry = document.getElementById(ddCountryId);
    var birthCountryName = ddBirthCountry.options[ddBirthCountry.selectedIndex].text;

    return birthCountryName.toUpperCase() != "БЪЛГАРИЯ";
}

function DisplayBirthCityInputArea()
{
    //If not abroad
    if (!IsAbroad("ddBirthCountry"))
    {
        document.getElementById("birthCityIfAbroad").style.display = "none";
        document.getElementById("birthCityIfNotAbroad").style.display = "";

        document.getElementById("txtBirthPostCode").value = "";
        document.getElementById("ddBirthRegion").selectedIndex = 0;
        ClearSelectList(document.getElementById("ddBirthMunicipality"), false);
        ClearSelectList(document.getElementById("ddBirthCity"), false);
    }
    else
    {
        document.getElementById("birthCityIfAbroad").style.display = "";
        document.getElementById("birthCityIfNotAbroad").style.display = "none";

        document.getElementById("txtBirthCityIfAbroad").value = "";
    }
}

//When changing the Birth Region then refresh the Birth Municipality and the Birth City
function ddBirthRegion_Changed()
{
    var ddBirthRegion = document.getElementById("ddBirthRegion");
    RepopulateMunicipality(ddBirthRegion.value, "ddBirthMunicipality");
}

function RepopulateMunicipality(regionId, ddMunicipalityId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulateMunicipality";
    var params = "";
    params += "RegionId=" + regionId;
    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
    
    function RepopulateMunicipality_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddMunicipalityId), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++)
        {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
        };

        if (ddMunicipalityId == "ddBirthMunicipality")
            ddBirthMunicipality_Changed();
        else if (ddMunicipalityId == "ddPermMunicipality")
            ddPermMunicipality_Changed();
        else if (ddMunicipalityId == "ddCurrMunicipality")
            ddCurrMunicipality_Changed();
    }
}


//When changing the Birth Municipality then refresh the Birth City
function ddBirthMunicipality_Changed()
{
    var ddBirthMunicipality = document.getElementById("ddBirthMunicipality");
    RepopulateCity(ddBirthMunicipality.value, "ddBirthCity");
}

function RepopulateCity(municipalityId, ddCityId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulateCity";
    var params = "";
    params += "MunicipalityId=" + municipalityId;
    
    function RepopulateMunicipality_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddCityId), true);

        var cities = xml.getElementsByTagName("c");

        for (var i = 0; i < cities.length; i++)
        {
            var id = xmlValue(cities[i], "id");
            var name = xmlValue(cities[i], "name");

            AddToSelectList(document.getElementById(ddCityId), id, name);
        };

        if (ddCityId == "ddBirthCity")
            ddBirthCity_Changed();
        else if (ddCityId == "ddPermCity")
            ddPermCity_Changed();
        else if (ddCityId == "ddCurrCity")
            ddCurrCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

//When changing the Birth City then refresh the Birth Post Code
function ddBirthCity_Changed()
{
    var ddBirthCity = document.getElementById("ddBirthCity");
    RepopulatePostCode(ddBirthCity.value, "txtBirthPostCode");
}

function RepopulatePostCode(cityId, txtPostCodeId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulatePostCode";
    var params = "";
    params += "CityId=" + cityId;
    
    function RepopulatePostCode_Callback(xml)
    {
        var postCode = xmlValue(xml, "postCode");
        
        document.getElementById(txtPostCodeId).value = postCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCode_Callback);
    myAJAX.Call();
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtBirthPostCode_Focus()
{
    var txtBirthPostCode = document.getElementById("txtBirthPostCode");
    txtBirthPostCode.setAttribute("oldvalue", txtBirthPostCode.value);
}

function txtBirthPostCode_Blur()
{
    var txtBirthPostCode = document.getElementById("txtBirthPostCode");

    if (txtBirthPostCode.value != txtBirthPostCode.getAttribute("oldvalue"))
    {
        RepopulateRegionMunicipalityCity(txtBirthPostCode.value, "ddBirthRegion", "ddBirthMunicipality", "ddBirthCity");
    }
}

function RepopulateRegionMunicipalityCity(postCode, ddRegionId, ddMunicipalityId, ddCityId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCity";
    var params = "";
    params += "PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));
        
        //Not found
        if (cityId == 0)
        {
            document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
            ClearSelectList(document.getElementById(ddCityId), false);
        }
        else //found
        {
           var regionId = xmlValue(xml, "regionId");
           var municipalityId = xmlValue(xml, "municipalityId");

           document.getElementById(ddRegionId).value = regionId;

           ClearSelectList(document.getElementById(ddMunicipalityId), true);

           var municipalities = xml.getElementsByTagName("m");

           for (var i = 0; i < municipalities.length; i++)
           {
               var id = xmlValue(municipalities[i], "id");
               var name = xmlValue(municipalities[i], "name");

               AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
           };

           document.getElementById(ddMunicipalityId).value = municipalityId;


           ClearSelectList(document.getElementById(ddCityId), true);

           var cities = xml.getElementsByTagName("c");

           for (var i = 0; i < cities.length; i++)
           {
               var id = xmlValue(cities[i], "id");
               var name = xmlValue(cities[i], "name");

               AddToSelectList(document.getElementById(ddCityId), id, name);
           };

           document.getElementById(ddCityId).value = cityId;
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCity_Callback);
    myAJAX.Call();
}

//When changing the Perm Region then refresh the Perm Municipality and the Perm City
function ddPermRegion_Changed()
{
    var ddPermRegion = document.getElementById("ddPermRegion");
    RepopulateMunicipality(ddPermRegion.value, "ddPermMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddPermMunicipality_Changed()
{
    var ddPermMunicipality = document.getElementById("ddPermMunicipality");
    RepopulateCity(ddPermMunicipality.value, "ddPermCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddPermCity_Changed()
{
    var ddPermCity = document.getElementById("ddPermCity");
    RepopulatePostCodeAndDistrict(ddPermCity.value, "txtPermPostCode", "ddPermDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtPermPostCode_Focus()
{
    var txtPermPostCode = document.getElementById("txtPermPostCode");
    txtPermPostCode.setAttribute("oldvalue", txtPermPostCode.value);
}

function txtPermPostCode_Blur()
{
    var txtPermPostCode = document.getElementById("txtPermPostCode");

    if (txtPermPostCode.value != txtPermPostCode.getAttribute("oldvalue"))
    {
        RepopulateRegionMunicipalityCityDistrict(txtPermPostCode.value, "ddPermRegion", "ddPermMunicipality", "ddPermCity", "ddPermDistrict");
    }
}

function RepopulatePostCodeAndDistrict(cityId, txtPostCodeId, ddDistrictsId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
    var params = "";
    params += "CityId=" + cityId;
    
    function RepopulatePostCodeAndDistrict_Callback(xml)
    {
        var cityPostCode = xmlValue(xml, "cityPostCode");

        document.getElementById(txtPostCodeId).value = cityPostCode;

        ClearSelectList(document.getElementById(ddDistrictsId), true);

        var districts = xml.getElementsByTagName("d");

        for (var i = 0; i < districts.length; i++)
        {
            var id = xmlValue(districts[i], "id");
            var name = xmlValue(districts[i], "name");

            AddToSelectList(document.getElementById(ddDistrictsId), id, name);
        };

        if (ddDistrictsId == "ddPermDistrict")
            ddPermDistrict_Changed();
        else if (ddDistrictsId == "ddCurrDistrict")
            ddCurrDistrict_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
    myAJAX.Call();
}


//When chaning the District then repopulate the PostCode
function ddPermDistrict_Changed()
{
    var ddPermDistrict = document.getElementById("ddPermDistrict");
    var ddPermCity = document.getElementById("ddPermCity");
    RepopulateDistrictPostCode(ddPermDistrict.value, ddPermCity.value, "txtPermPostCode");
}

function RepopulateDistrictPostCode(districtId, cityId, txtPostCodeId) {
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
    var params = "";
    params += "DistrictId=" + districtId;
    params += "&";
    params += "CityId=" + cityId;

    function RepopulateDistrictPostCode_Callback(xml) {
        var districtPostCode = xmlValue(xml, "districtPostCode");

        if (districtPostCode != "")
            document.getElementById(txtPostCodeId).value = districtPostCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulateDistrictPostCode_Callback);
    myAJAX.Call();
}


function RepopulateRegionMunicipalityCityDistrict(postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId)
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
    var params = "";
    params += "PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Found
        if (cityId != 0)
        {
            var regionId = xmlValue(xml, "regionId");
            var municipalityId = xmlValue(xml, "municipalityId");
            var districtId = xmlValue(xml, "districtId");

            document.getElementById(ddRegionId).value = regionId;

            ClearSelectList(document.getElementById(ddMunicipalityId), true);

            var municipalities = xml.getElementsByTagName("m");

            for (var i = 0; i < municipalities.length; i++)
            {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            document.getElementById(ddMunicipalityId).value = municipalityId;


            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++)
            {
                var id = xmlValue(cities[i], "id");
                var name = xmlValue(cities[i], "name");

                AddToSelectList(document.getElementById(ddCityId), id, name);
            };

            document.getElementById(ddCityId).value = cityId;


            ClearSelectList(document.getElementById(ddDistrictId), true);

            var districts = xml.getElementsByTagName("d");

            for (var i = 0; i < districts.length; i++)
            {
                var id = xmlValue(districts[i], "id");
                var name = xmlValue(districts[i], "name");

                AddToSelectList(document.getElementById(ddDistrictId), id, name);
            };

            if (parseInt(districtId) > 0)
                document.getElementById(ddDistrictId).value = districtId;
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCity_Callback);
    myAJAX.Call();
}



//When changing the Curr Region then refresh the Curr Municipality and the Curr City
function ddCurrRegion_Changed()
{
    var ddCurrRegion = document.getElementById("ddCurrRegion");
    RepopulateMunicipality(ddCurrRegion.value, "ddCurrMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddCurrMunicipality_Changed()
{
    var ddCurrMunicipality = document.getElementById("ddCurrMunicipality");
    RepopulateCity(ddCurrMunicipality.value, "ddCurrCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddCurrCity_Changed()
{
    var ddCurrCity = document.getElementById("ddCurrCity");
    RepopulatePostCodeAndDistrict(ddCurrCity.value, "txtCurrPostCode", "ddCurrDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtCurrPostCode_Focus()
{
    var txtCurrPostCode = document.getElementById("txtCurrPostCode");
    txtCurrPostCode.setAttribute("oldvalue", txtCurrPostCode.value);
}

function txtCurrPostCode_Blur()
{
    var txtCurrPostCode = document.getElementById("txtCurrPostCode");

    if (txtCurrPostCode.value != txtCurrPostCode.getAttribute("oldvalue"))
    {
        RepopulateRegionMunicipalityCityDistrict(txtCurrPostCode.value, "ddCurrRegion", "ddCurrMunicipality", "ddCurrCity", "ddCurrDistrict");
    }
}

//When chaning the District then repopulate the PostCode
function ddCurrDistrict_Changed()
{
    var ddCurrDistrict = document.getElementById("ddCurrDistrict");
    var ddCurrCity = document.getElementById("ddCurrCity");
    RepopulateDistrictPostCode(ddCurrDistrict.value, ddCurrCity.value, "txtCurrPostCode");
}

//Refresh the SizeClothing list when updating the list via the GTable maintenance
function RefreshSizeClothingList()
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRefreshSizeClothingList";
    var params = "";
    
    function RefreshSizeClothingList_Callback(xml)
    {
        var currentValue = document.getElementById("ddSizeClothing").value;

        ClearSelectList(document.getElementById("ddSizeClothing"), true);

        var sizes = xml.getElementsByTagName("s");
        var found = false;

        for (var i = 0; i < sizes.length; i++)
        {
            var id = xmlValue(sizes[i], "id");
            var name = xmlValue(sizes[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddSizeClothing"), id, name);
        };

        if (found)
            document.getElementById("ddSizeClothing").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshSizeClothingList_Callback);
    myAJAX.Call();
}

//Refresh the SizeHat list when updating the list via the GTable maintenance
function RefreshSizeHatList()
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRefreshSizeHatList";
    var params = "";
    
    function RefreshSizeHatList_Callback(xml)
    {
        var currentValue = document.getElementById("ddSizeHat").value;

        ClearSelectList(document.getElementById("ddSizeHat"), true);

        var sizes = xml.getElementsByTagName("s");
        var found = false;

        for (var i = 0; i < sizes.length; i++)
        {
            var id = xmlValue(sizes[i], "id");
            var name = xmlValue(sizes[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddSizeHat"), id, name);
        };

        if (found)
            document.getElementById("ddSizeHat").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshSizeHatList_Callback);
    myAJAX.Call();
}

//Refresh the SizeShoes list when updating the list via the GTable maintenance
function RefreshSizeShoesList()
{
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSRefreshSizeShoesList";
    var params = "";
    
    function RefreshSizeShoesList_Callback(xml)
    {
        var currentValue = document.getElementById("ddSizeShoes").value;

        ClearSelectList(document.getElementById("ddSizeShoes"), true);

        var sizes = xml.getElementsByTagName("s");
        var found = false;

        for (var i = 0; i < sizes.length; i++)
        {
            var id = xmlValue(sizes[i], "id");
            var name = xmlValue(sizes[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddSizeShoes"), id, name);
        };

        if (found)
            document.getElementById("ddSizeShoes").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshSizeShoesList_Callback);
    myAJAX.Call();
}

function CopyPresAddressToCurr()
{
    document.getElementById("txtPermPostCode").value = document.getElementById("txtCurrPostCode").value;
    CopyDropDown("ddCurrRegion", "ddPermRegion");
    CopyDropDown("ddCurrMunicipality", "ddPermMunicipality");
    CopyDropDown("ddCurrCity", "ddPermCity");
    CopyDropDown("ddCurrDistrict", "ddPermDistrict");
    document.getElementById("txtPermAddress").value = document.getElementById("txtCurrAddress").value;
}

function CopyPermAddressToCurr()
{
    document.getElementById("txtCurrPostCode").value = document.getElementById("txtPermPostCode").value;
    CopyDropDown("ddPermRegion", "ddCurrRegion");
    CopyDropDown("ddPermMunicipality", "ddCurrMunicipality");
    CopyDropDown("ddPermCity", "ddCurrCity");
    CopyDropDown("ddPermDistrict", "ddCurrDistrict");
    document.getElementById("txtCurrAddress").value = document.getElementById("txtPermAddress").value;
}

function chkIsAbroad_Click()
{
    if (!document.getElementById("chkIsAbroad").checked)
    {
        document.getElementById("ddAbroadCountry").disabled = true;
        document.getElementById("lblAbroadSince").disabled = true;
        document.getElementById("txtAbroadSince").disabled = true;
        document.getElementById("lblAbroadPeriod").disabled = true;
        document.getElementById("txtAbroadPeriod").disabled = true;
        document.getElementById("lblAbroadPeriodMeasure").disabled = true;

        document.getElementById("ddAbroadCountry").value = optionChooseOneValue;
        document.getElementById("txtAbroadSince").value = "";
        document.getElementById("txtAbroadPeriod").value = "";
    }
    else
    {
        document.getElementById("ddAbroadCountry").disabled = false;
        document.getElementById("lblAbroadSince").disabled = false;
        document.getElementById("txtAbroadSince").disabled = false;
        document.getElementById("lblAbroadPeriod").disabled = false;
        document.getElementById("txtAbroadPeriod").disabled = false;
        document.getElementById("lblAbroadPeriodMeasure").disabled = false;
    }
}

function LoadRecordOfServiceArchivesSection() {
    if (document.getElementById(hdnIsRecordOfServiceArchiveHiddenClientID).value == "1")
        return;
        
    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadRecordOfServiceArchives";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    
    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
        
    function LoadRecordOfServiceArchivesSection_CallBack(xml) {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        if (tableHTML != "") {
            document.getElementById("divRecordOfServiceTableTitle").style.display = "";
        }

        if (document.getElementById(hdnIsRecordOfServiceArchiveDisabledClientID).value == "0")
            document.getElementById("imgNewRecordOfService").style.display = "";     
        
        document.getElementById("tblRecordOfServiceArchive").innerHTML = tableHTML;
        document.getElementById("lboxRecordOfServiceArchive").innerHTML = lightBoxHTML;

        CheckDisabledClientControls();
        CheckHiddenClientControls();

        document.getElementById("imgLoadingRecordOfServiceArchive").style.visibility = "hidden";
    }

    var myAJAX = new AJAX(url, true, params, LoadRecordOfServiceArchivesSection_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the RecordOfServiceArchive table
function NewRecordOfService() {
    ShowAddEditRecordOfServiceLightBox(0);
}

//Open the light-box for editing a record in the RecordOfServiceArchive table
function EditRecordOfService(recordOfServiceId) {
    ShowAddEditRecordOfServiceLightBox(recordOfServiceId);
}

function ShowAddEditRecordOfServiceLightBox(recordOfServiceId) {
    ClearAllMessages();

    document.getElementById("hdnRecordOfServiceArchiveID").value = recordOfServiceId;

    //Archive record
    if (recordOfServiceId == 0) {
        document.getElementById("hdnIsNewRecordOfService").value = "1";        
    
        document.getElementById("lblAddEditRecordOfServiceArchiveTitle").innerHTML = "Архивиране на военна книжка";

        document.getElementById("txtRecordOfServiceArchiveSeries").value = document.getElementById("hdnRecordOfServiceSeries").value;
        document.getElementById("txtRecordOfServiceArchiveNumber").value = document.getElementById("hdnRecordOfServiceNumber").value;
        document.getElementById("txtRecordOfServiceArchiveDate").value = document.getElementById("hdnRecordOfServiceDate").value;
        document.getElementById("chkRecordOfServiceArchiveCopy").checked = (document.getElementById("hdnRecordOfServiceCopy").value == "1");
        document.getElementById("txtRecordOfServiceArchiveComment").value = "";
        
        // set fields as disabled
        document.getElementById("txtRecordOfServiceArchiveSeries").disabled = true;
        document.getElementById("txtRecordOfServiceArchiveNumber").disabled = true;
        document.getElementById("txtRecordOfServiceArchiveDate").disabled = true;
        document.getElementById("chkRecordOfServiceArchiveCopy").disabled = true;

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditRecordOfServiceLightBox").style.display = "none";
        document.getElementById("spanAddEditRecordOfServiceLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxRecordOfServiceArchive").style.display = "";
        CenterLightBox("lboxRecordOfServiceArchive");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditRecordOfServiceArchiveTitle").innerHTML = "Редактиране на архивирана военна книжка";

        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadRecordOfService";

        var params = "RecordOfServiceId=" + recordOfServiceId;

        function response_handler(xml) {
            var personRecordOfServiceArchive = xml.getElementsByTagName("personRecordOfServiceArchive")[0];

            var recordOfServiceSeries = xmlValue(personRecordOfServiceArchive, "recordOfServiceSeries");
            var recordOfServiceNumber = xmlValue(personRecordOfServiceArchive, "recordOfServiceNumber");
            var recordOfServiceDate = xmlValue(personRecordOfServiceArchive, "recordOfServiceDate");
            var recordOfServiceCopy = xmlValue(personRecordOfServiceArchive, "recordOfServiceCopy");
            var recordOfServiceComment = xmlValue(personRecordOfServiceArchive, "recordOfServiceComment");

            document.getElementById("txtRecordOfServiceArchiveSeries").value = recordOfServiceSeries;
            document.getElementById("txtRecordOfServiceArchiveNumber").value = recordOfServiceNumber;
            document.getElementById("txtRecordOfServiceArchiveDate").value = recordOfServiceDate;
            document.getElementById("chkRecordOfServiceArchiveCopy").checked = (recordOfServiceCopy == "1");
            document.getElementById("txtRecordOfServiceArchiveComment").value = recordOfServiceComment;

            // set fields as enabled
            document.getElementById("txtRecordOfServiceArchiveSeries").disabled = false;
            document.getElementById("txtRecordOfServiceArchiveNumber").disabled = false;
            document.getElementById("txtRecordOfServiceArchiveDate").disabled = false;
            document.getElementById("chkRecordOfServiceArchiveCopy").disabled = false;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditRecordOfServiceLightBox").style.display = "none";
            document.getElementById("spanAddEditRecordOfServiceLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxRecordOfServiceArchive").style.display = "";
            CenterLightBox("lboxRecordOfServiceArchive");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditRecordOfServiceLightBox() {
    document.getElementById("hdnIsNewRecordOfService").value = "0"

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxRecordOfServiceArchive").style.display = "none";
}

//Save Add/Edit RecordOfService
function SaveAddEditRecordOfServiceLightBox() {
    if (ValidateAddEditRecordOfService()) {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSSaveRecordOfService";

        var params = "RecordOfServiceId=" + document.getElementById("hdnRecordOfServiceArchiveID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&RecordOfServiceSeries=" + document.getElementById("txtRecordOfServiceArchiveSeries").value +
                     "&RecordOfServiceNumber=" + document.getElementById("txtRecordOfServiceArchiveNumber").value +
                     "&RecordOfServiceDate=" + document.getElementById("txtRecordOfServiceArchiveDate").value +
                     "&RecordOfServiceCopy=" + (document.getElementById("chkRecordOfServiceArchiveCopy").checked ? "1" : "0") +
                     "&RecordOfServiceComment=" + custEncodeURI(document.getElementById("txtRecordOfServiceArchiveComment").value);

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedRecordOfServiceTable = xmlValue(xml, "refreshedRecordOfServiceTable");

                if (refreshedRecordOfServiceTable != "") {
                    document.getElementById("divRecordOfServiceTableTitle").style.display = "";                    
                }

                document.getElementById("tblRecordOfServiceArchive").innerHTML = refreshedRecordOfServiceTable;

                document.getElementById("lblMessageRecordOfServiceArchive").className = "SuccessText";
                document.getElementById("lblMessageRecordOfServiceArchive").innerHTML = document.getElementById("hdnRecordOfServiceArchiveID").value == "0" ? "Военната книжка е архивирана успешно" : "Военната книжка е редактирана успешно";

                // clear fields after inserting a new archive
                if (document.getElementById("hdnIsNewRecordOfService").value == "1") {
                    document.getElementById("txtRecordOfServiceSeries").value = "";
                    document.getElementById("txtRecordOfServiceNumber").value = "";
                    document.getElementById("txtRecordOfServiceDate").value = "";
                    document.getElementById("chkRecordOfServiceCopy").checked = false;
                    document.getElementById("hdnRecordOfServiceSeries").value = "";
                    document.getElementById("hdnRecordOfServiceNumber").value = "";
                    document.getElementById("hdnRecordOfServiceDate").value = "";
                    document.getElementById("hdnRecordOfServiceCopy").value = "0";                   
                }

                HideAddEditRecordOfServiceLightBox();
            }
            else {
                document.getElementById("spanAddEditRecordOfServiceLightBox").className = "ErrorText";
                document.getElementById("spanAddEditRecordOfServiceLightBox").innerHTML = status;
                document.getElementById("spanAddEditRecordOfServiceLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit RecordOfService light-box
function ValidateAddEditRecordOfService() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditRecordOfServiceLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtRecordOfServiceArchiveSeries = document.getElementById("txtRecordOfServiceArchiveSeries");
    var txtRecordOfServiceArchiveNumber = document.getElementById("txtRecordOfServiceArchiveNumber");

    if (txtRecordOfServiceArchiveSeries.value.Trim() == "" && txtRecordOfServiceArchiveNumber.value.Trim() == "") {
        res = false;

        lblMessage.innerHTML += "Поне едно от двете полета (Серия или №) трябва да бъде попълнено<br />";
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular RecordOfService record
function DeleteRecordOfService(recordOfServiceId) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете архивираната военна книжка?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSDeleteRecordOfService";

        var params = "RecordOfServiceId=" + recordOfServiceId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedRecordOfServiceTable = xmlValue(xml, "refreshedRecordOfServiceTable");
                
                if (refreshedRecordOfServiceTable == "") {
                    document.getElementById("divRecordOfServiceTableTitle").style.display = "none";
                }
                
                document.getElementById("tblRecordOfServiceArchive").innerHTML = refreshedRecordOfServiceTable;

                document.getElementById("lblMessageRecordOfServiceArchive").className = "SuccessText";
                document.getElementById("lblMessageRecordOfServiceArchive").innerHTML = "Архивираната военна книжка е изтрита успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function LoadConvictionSection()
{
    if (document.getElementById(hdnIsConvictionHiddenClientID).value == "1")
        return;

    document.getElementById("pnlConvictionDualCitizenshipSpace").style.display = "";
    document.getElementById("pnlConvictionDualCitizenship").style.display = "";

    document.getElementById("divConvictionTableTitle").style.display = "";

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadConvictions";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadConvictionSection_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblConviction").innerHTML = tableHTML;
        document.getElementById("lboxConviction").innerHTML = lightBoxHTML;

        RefreshDatePickers();
        CheckDisabledClientControls();
        CheckHiddenClientControls();

        document.getElementById("imgLoadingConviction").style.visibility = "hidden";
    }

    var myAJAX = new AJAX(url, true, params, LoadConvictionSection_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the Conviction table
function NewConviction()
{
    ShowAddEditConvictionLightBox(0);
}

//Open the light-box for editing a record in the Conviction table
function EditConviction(convictionId)
{
    ShowAddEditConvictionLightBox(convictionId);
}

function ShowAddEditConvictionLightBox(convictionId)
{
    ClearAllMessages();

    document.getElementById("hdnConvictionID").value = convictionId;

    //New record
    if (convictionId == 0)
    {
        document.getElementById("lblAddEditConvictionTitle").innerHTML = "Въвеждане на съдимост";

        document.getElementById("ddConviction").value = optionChooseOneValue;
        document.getElementById("ddConvictionReason").value = optionChooseOneValue;
        document.getElementById("txtConvDateFrom").value = "";
        document.getElementById("txtConvDateTo").value = "";


        // clean message label in the light box and hide it
        document.getElementById("spanAddEditConvictionLightBox").style.display = "none";
        document.getElementById("spanAddEditConvictionLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxConviction").style.display = "";
        CenterLightBox("lboxConviction");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditConvictionTitle").innerHTML = "Редактиране на съдимост";

        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadConviction";

        var params = "ConvictionId=" + convictionId;

        function response_handler(xml)
        {
            var personConviction = xml.getElementsByTagName("personConviction")[0];

            var convictionCode = xmlValue(personConviction, "convictionCode");
            var convictionReasonCode = xmlValue(personConviction, "convictionReasonCode");
            var dateFrom = xmlValue(personConviction, "dateFrom");
            var dateTo = xmlValue(personConviction, "dateTo");

            document.getElementById("ddConviction").value = convictionCode;
            document.getElementById("ddConvictionReason").value = convictionReasonCode;
            document.getElementById("txtConvDateFrom").value = dateFrom;
            document.getElementById("txtConvDateTo").value = dateTo;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditConvictionLightBox").style.display = "none";
            document.getElementById("spanAddEditConvictionLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxConviction").style.display = "";
            CenterLightBox("lboxConviction");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditConvictionLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxConviction").style.display = "none";
}

//Save Add/Edit Conviction
function SaveAddEditConvictionLightBox()
{
    if (ValidateAddEditConviction())
    {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSSaveConviction";

        var params = "ConvictionId=" + document.getElementById("hdnConvictionID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&ConvictionCode=" + document.getElementById("ddConviction").value +
                     "&ConvictionReasonCode=" + document.getElementById("ddConvictionReason").value +
                     "&DateFrom=" + document.getElementById("txtConvDateFrom").value +
                     "&DateTo=" + document.getElementById("txtConvDateTo").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedConvictionTable = xmlValue(xml, "refreshedConvictionTable");

                document.getElementById("tblConviction").innerHTML = refreshedConvictionTable;

                document.getElementById("lblMessageConviction").className = "SuccessText";
                document.getElementById("lblMessageConviction").innerHTML = document.getElementById("hdnConvictionID").value == "0" ? "Съдимостта е добавена успешно" : "Съдимостта е редактирана успешно";

                HideAddEditConvictionLightBox();

                //Refresh the military report status section because when adding Conviction info then the person could become TEMPORARY_REMOVED    
                MilitaryReport_Refresh_ReservistMilitaryReportStatusSection(null);
            }
            else
            {
                document.getElementById("spanAddEditConvictionLightBox").className = "ErrorText";
                document.getElementById("spanAddEditConvictionLightBox").innerHTML = status;
                document.getElementById("spanAddEditConvictionLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Conviction light-box
function ValidateAddEditConviction()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditConvictionLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddConviction = document.getElementById("ddConviction");
    var txtConvDateFrom = document.getElementById("txtConvDateFrom");
    var txtConvDateTo = document.getElementById("txtConvDateTo");

    if (ddConviction.value == optionChooseOneValue)
    {
        res = false;

        if (ddConviction.disabled == true || ddConviction.style.display == "none")
            notValidFields.push("Съдимост");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Съдимост") + "<br />";
    }

    if (txtConvDateFrom.value.Trim() == "")
    {
        res = false;

        if (txtConvDateFrom.disabled == true || txtConvDateFrom.style.display == "none")
            notValidFields.push("От дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("От дата") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtConvDateFrom.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("От дата") + "<br />";
        }
    }

    if (txtConvDateTo.value.Trim() != "")
    {
        if (!IsValidDate(txtConvDateTo.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("От дата") + "<br />";
        }
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res)
    {
        lblMessage.className = "SuccessText";
    }
    else
    {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Conviction record
function DeleteConviction(convictionId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете съдимостта?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSDeleteConviction";

        var params = "ConvictionId=" + convictionId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedConvictionTable = xmlValue(xml, "refreshedConvictionTable");

                document.getElementById("tblConviction").innerHTML = refreshedConvictionTable;

                document.getElementById("lblMessageConviction").className = "SuccessText";
                document.getElementById("lblMessageConviction").innerHTML = "Съдимостта е изтрита успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



function LoadDualCitizenshipSection()
{
    if (document.getElementById(hdnIsDualCitizenshipHiddenClientID).value == "1")
        return;

    document.getElementById("pnlConvictionDualCitizenshipSpace").style.display = "";
    document.getElementById("pnlConvictionDualCitizenship").style.display = "";

    document.getElementById("divDualCitizenshipTableTitle").style.display = "";

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadDualCitizenships";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadDualCitizenshipSection_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblDualCitizenship").innerHTML = tableHTML;
        document.getElementById("lboxDualCitizenship").innerHTML = lightBoxHTML;

        CheckDisabledClientControls();
        CheckHiddenClientControls();

        document.getElementById("imgLoadingDualCitizenship").style.visibility = "hidden";
    }

    var myAJAX = new AJAX(url, true, params, LoadDualCitizenshipSection_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the DualCitizenship table
function NewDualCitizenship()
{
    ShowAddEditDualCitizenshipLightBox(0);
}

//Open the light-box for editing a record in the DualCitizenship table
function EditDualCitizenship(dualCitizenshipId)
{
    ShowAddEditDualCitizenshipLightBox(dualCitizenshipId);
}

function ShowAddEditDualCitizenshipLightBox(dualCitizenshipId)
{
    ClearAllMessages();

    document.getElementById("hdnDualCitizenshipID").value = dualCitizenshipId;

    //New record
    if (dualCitizenshipId == 0)
    {
        document.getElementById("lblAddEditDualCitizenshipTitle").innerHTML = "Въвеждане на двойно гражданство";

        document.getElementById("ddDualCitizenshipCountry").value = optionChooseOneValue;

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditDualCitizenshipLightBox").style.display = "none";
        document.getElementById("spanAddEditDualCitizenshipLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxDualCitizenship").style.display = "";
        CenterLightBox("lboxDualCitizenship");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditDualCitizenshipTitle").innerHTML = "Редактиране на двойно гражданство";

        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadDualCitizenship";

        var params = "DualCitizenshipId=" + dualCitizenshipId;

        function response_handler(xml)
        {
            var personDualCitizenship = xml.getElementsByTagName("personDualCitizenship")[0];

            var countryId = xmlValue(personDualCitizenship, "countryId");
            
            document.getElementById("ddDualCitizenshipCountry").value = countryId;
            
            // clean message label in the light box and hide it
            document.getElementById("spanAddEditDualCitizenshipLightBox").style.display = "none";
            document.getElementById("spanAddEditDualCitizenshipLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxDualCitizenship").style.display = "";
            CenterLightBox("lboxDualCitizenship");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditDualCitizenshipLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxDualCitizenship").style.display = "none";
}

//Save Add/Edit DualCitizenship
function SaveAddEditDualCitizenshipLightBox()
{
    if (ValidateAddEditDualCitizenship())
    {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSSaveDualCitizenship";

        var params = "DualCitizenshipId=" + document.getElementById("hdnDualCitizenshipID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&CountryId=" + document.getElementById("ddDualCitizenshipCountry").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPersonDualCitizenshipTable = xmlValue(xml, "refreshedPersonDualCitizenshipTable");

                document.getElementById("tblDualCitizenship").innerHTML = refreshedPersonDualCitizenshipTable;

                document.getElementById("lblMessageDualCitizenship").className = "SuccessText";
                document.getElementById("lblMessageDualCitizenship").innerHTML = document.getElementById("hdnDualCitizenshipID").value == "0" ? "Двойното гражданство е добавено успешно" : "Двойното гражданство е редактирано успешно";

                HideAddEditDualCitizenshipLightBox();
            }
            else
            {
                document.getElementById("spanAddEditDualCitizenshipLightBox").className = "ErrorText";
                document.getElementById("spanAddEditDualCitizenshipLightBox").innerHTML = status;
                document.getElementById("spanAddEditDualCitizenshipLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit DualCitizenship light-box
function ValidateAddEditDualCitizenship()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditDualCitizenshipLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddDualCitizenshipCountry = document.getElementById("ddDualCitizenshipCountry");

    if (ddDualCitizenshipCountry.value == optionChooseOneValue)
    {
        res = false;

        if (ddDualCitizenshipCountry.disabled == true || ddDualCitizenshipCountry.style.display == "none")
            notValidFields.push("Държава");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Държава") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res)
    {
        lblMessage.className = "SuccessText";
    }
    else
    {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular DualCitizenship record
function DeleteDualCitizenship(dualCitizenshipId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете двойното гражданство?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSDeleteDualCitizenship";

        var params = "DualCitizenshipId=" + dualCitizenshipId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedDualCitizenshipTable = xmlValue(xml, "refreshedDualCitizenshipTable");

                document.getElementById("tblDualCitizenship").innerHTML = refreshedDualCitizenshipTable;

                document.getElementById("lblMessageDualCitizenship").className = "SuccessText";
                document.getElementById("lblMessageDualCitizenship").innerHTML = "Двойното гражданство е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function PopulateInitials()
{
    var result = "";
    
    var names = document.getElementById("txtFirstName").value + " " + document.getElementById("txtLastName").value;

    var words = TrimString(names).split(/[-\,\s]+/);

    for (var i = 0; i < words.length; i++)
    {
        if (words[i].length > 0)
        {
            result += words[i][0].toUpperCase();
        }
    }

    document.getElementById("txtInitials").innerHTML = result;
}

function EditMilitaryRank(personId) {
    ShowEditMilitaryRankLightBox(personId);
}

function ShowEditMilitaryRankLightBox(personId) {
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    //document.getElementById("hdnArchiveTitleID").value = personId;

    document.getElementById("lblEditMilitaryRankTitle").innerHTML = "Редактиране на звание";

    var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSLoadMilitaryRank";

    var params = "PersonID=" + personId;

    function response_handler(xml) {
        var personMilitaryRank = xml.getElementsByTagName("personMilitaryRank")[0];

        var MilitaryRankMilitaryRankId = xmlValue(personMilitaryRank, "MilitaryRankMilitaryRankId");
        var MilitaryRankVacAnn = xmlValue(personMilitaryRank, "MilitaryRankVacAnn");
        var MilitaryRankDateArchive = xmlValue(personMilitaryRank, "MilitaryRankDateArchive");
        var MilitaryRankDateWhen = xmlValue(personMilitaryRank, "MilitaryRankDateWhen");
        var MilitaryRankMilitaryCommanderRankCode = xmlValue(personMilitaryRank, "MilitaryRankMilitaryCommanderRankCode");
        var MilitaryRankDR = (xmlValue(personMilitaryRank, "MilitaryRankDR") == "1" ? true : false);

        // if (document.getElementById("ddPersonArchiveTitleMilitaryRank") != undefined)
        document.getElementById("ddPersonMilitaryRankMilitaryRank").value = MilitaryRankMilitaryRankId;
        // if (document.getElementById("txtPersonArchiveTitleVacAnn") != undefined)
        document.getElementById("txtPersonMilitaryRankVacAnn").value = MilitaryRankVacAnn;
        // if (document.getElementById("txtPersonArchiveTitleDateArchive") != undefined)
        document.getElementById("txtPersonMilitaryRankDateArchive").value = MilitaryRankDateArchive;
        //  if (document.getElementById("txtPersonArchiveTitleDateWhen") != undefined)
        document.getElementById("txtPersonMilitaryRankDateWhen").value = MilitaryRankDateWhen;
        //   if (document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank") != undefined)
        document.getElementById("ddPersonMilitaryRankMilitaryCommanderRank").value = MilitaryRankMilitaryCommanderRankCode;

        document.getElementById("chkPersonMilitaryRankDR").checked = MilitaryRankDR;

        // clean message label in the light box and hide it
        document.getElementById("spanEditMilitaryRankLightBox").style.display = "none";
        document.getElementById("spanEditMilitaryRankLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById(lboxMilitaryRankID).style.display = "";
        CenterLightBox(lboxMilitaryRankID);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

//Close the light-box
function HideEditMilitaryRankLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById(lboxMilitaryRankID).style.display = "none";
}

function ValidateEditMilitaryRank() {
    var res = true;

    var lblMessage = document.getElementById("spanEditMilitaryRankLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonMilitaryRankMilitaryRank = document.getElementById("ddPersonMilitaryRankMilitaryRank");
    var txtPersonMilitaryRankDateArchive = document.getElementById("txtPersonMilitaryRankDateArchive");
    var txtPersonMilitaryRankDateWhen = document.getElementById("txtPersonMilitaryRankDateWhen");


    //Validate Mandatory Fields

    //1 MilitaryRank
    if (ddPersonMilitaryRankMilitaryRank.value == optionChooseOneValue) {
        res = false;

        if (ddPersonMilitaryRankMilitaryRank.disabled == true || ddPersonMilitaryRankMilitaryRank.style.display == "none")
            notValidFields.push("Звание");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Звание") + "<br />";
    }

    //2  DateArchive
    if (txtPersonMilitaryRankDateArchive.value.Trim() == "") {
        res = false;

        if (txtPersonMilitaryRankDateArchive.disabled == true || spanPersonMilitaryRankDateArchive.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
    else {
        if (!IsValidDate(txtPersonMilitaryRankDateArchive.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
        }
    }

    // 3 DateWhen
    if (txtPersonMilitaryRankDateWhen.value.Trim() == "") {
        res = false;

        if (txtPersonMilitaryRankDateWhen.disabled == true || spanPersonMilitaryRankDateWhen.style.display == "none")
            notValidFields.push("В сила от");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("В сила от") + "<br />";
    }
    else {
        if (!IsValidDate(txtPersonMilitaryRankDateWhen.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("В сила от") + "<br />";
        }
    }

    //Validate other fields - No  fields to validate


    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

//Save Add/Edit ArchiveTitle
function SaveEditMilitaryRankLightBox() {
    if (ValidateEditMilitaryRank()) {
        var url = "AddEditReservist_PersonalData.aspx?AjaxMethod=JSSaveMilitaryRank";

        //Set variable for fields thta can be not bound from UI Items logic but Save?Update can performe
        var MilitaryRankVacAnn = "";
        if (document.getElementById("txtPersonMilitaryRankVacAnn") != undefined)
            MilitaryRankVacAnn = document.getElementById("txtPersonMilitaryRankVacAnn").value;


        var MilitaryRankMilitaryCommanderRankCode = "-1";
        if (document.getElementById("ddPersonMilitaryRankMilitaryCommanderRank") != undefined)
            MilitaryRankMilitaryCommanderRankCode = document.getElementById("ddPersonMilitaryRankMilitaryCommanderRank").value;

        var MilitaryRankDR = "0";
        if (document.getElementById("chkPersonMilitaryRankDR") != undefined)
            MilitaryRankDR = document.getElementById("chkPersonMilitaryRankDR").checked ? "1" : "0";
            
        var params = "&PersonID=" + document.getElementById(hdnPersonIdClientID).value +
                     "&MilitaryRankMilitaryRankID=" + document.getElementById("ddPersonMilitaryRankMilitaryRank").value +
                     "&MilitaryRankVacAnn=" + MilitaryRankVacAnn +
                     "&MilitaryRankDateArchive=" + document.getElementById("txtPersonMilitaryRankDateArchive").value +
                     "&MilitaryRankDateWhen=" + document.getElementById("txtPersonMilitaryRankDateWhen").value +
                     "&MilitaryRankMilitaryCommanderRankCode=" + MilitaryRankMilitaryCommanderRankCode + 
                     "&MilitaryRankDR=" + MilitaryRankDR ;


        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var ddMilitaryRank = document.getElementById("ddPersonMilitaryRankMilitaryRank");
                var militaryRankShortName = xmlValue(xml, "militaryRankShortName");
                var militaryCategoryName = xmlValue(xml, "militaryCategoryName");
                var militaryRankDR = xmlValue(xml, "militaryRankDR") == "1" ? true : false;
                
                document.getElementById("hdnMilitaryRankID").value = document.getElementById("ddPersonMilitaryRankMilitaryRank").value;
                document.getElementById("lblMilitaryRankValue").innerHTML = militaryRankShortName;
                document.getElementById("lblMilitaryCategoryValue").innerHTML = militaryCategoryName;
                document.getElementById("chkMilitaryRankDR").checked = militaryRankDR;
                
                HideEditMilitaryRankLightBox();
            }
            else {
                document.getElementById("spanEditMilitaryRankLightBox").className = "ErrorText";
                document.getElementById("spanEditMilitaryRankLightBox").innerHTML = status;
                document.getElementById("spanEditMilitaryRankLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ActivateMilitaryRankEditing(personId) {
    var imgEditMilitaryRank = document.getElementById("imgEditMilitaryRank");
    imgEditMilitaryRank.setAttribute("onclick", "EditMilitaryRank(" + personId + ");");
    imgEditMilitaryRank.src = "../Images/list_edit.png";
    imgEditMilitaryRank.title = "Редактиране на звание";
    imgEditMilitaryRank.style.cursor = "pointer";
}

function SetEventHandlers() {
    var hasMilitaryServiceRadios = document.querySelectorAll('input[type=radio][name="hasMilitarySrv"]');

    for (var i = 0; i < hasMilitaryServiceRadios.length; i++) {
        if (hasMilitaryServiceRadios[i].addEventListener) {
            hasMilitaryServiceRadios[i].addEventListener("click", HasMilitaryService_Change);
        }
        else {
            hasMilitaryServiceRadios[i].attachEvent("onclick", HasMilitaryService_Change);
        }
    }
}

function HasMilitaryService_Change(event) {
    var hasMilitarySrv1 = document.getElementById("hasMilitarySrv1");

    if (hasMilitarySrv1.checked) {
        document.getElementById("militaryTraining1").checked = true;
    }
}