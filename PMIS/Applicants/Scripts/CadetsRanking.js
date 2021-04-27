function SearchCadet()
{
    var lblFullNameValue = document.getElementById(lblFullNameValueClientID);

    var txtIdentityNumber = document.getElementById(txtIdentityNumberClientID);
    var militarySchoolId = GetSelectedItemId(document.getElementById(ddlMilitarySchoolsClientID));
    var year = GetSelectedItemId(document.getElementById(ddlSchoolYearsClientID));
    var specializationId = GetSelectedItemId(document.getElementById(ddlSpecializationsClientID));

    if (TrimString(txtIdentityNumber.value) != "")
    {
        var url = "CadetsRanking.aspx?AjaxMethod=JSSearchCadet";
        var params = "";
        params += "IdentityNumber=" + txtIdentityNumber.value;
        params += "&MilitarySchoolId=" + militarySchoolId;
        params += "&Year=" + year;
        params += "&SpecializationId=" + specializationId;
        
        function response_handler(xml)
        {
            if (xmlValue(xml, "response") != "OK")
            {
                lblFullNameValue.innerHTML = xmlNodeText(xml.childNodes[1]);
                lblFullNameValue.className = "ErrorText";
                document.getElementById("imgAddCadet").style.display = "none";
            }
            else
            {
                lblFullNameValue.innerHTML = xmlNodeText(xml.childNodes[1]);
                lblFullNameValue.className = "";
                document.getElementById("imgAddCadet").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function RankCadet()
{
    var lblFullNameValue = document.getElementById(lblFullNameValueClientID);

    var txtIdentityNumber = document.getElementById(txtIdentityNumberClientID);
    var militarySchoolId = GetSelectedItemId(document.getElementById(ddlMilitarySchoolsClientID));
    var year = GetSelectedItemId(document.getElementById(ddlSchoolYearsClientID));
    var specializationId = GetSelectedItemId(document.getElementById(ddlSpecializationsClientID));

    if (TrimString(txtIdentityNumber.value) != "" && document.getElementById("imgAddCadet").style.display === "")
    {
        var url = "CadetsRanking.aspx?AjaxMethod=JSRankCadet";
        var params = "";
        params += "IdentityNumber=" + txtIdentityNumber.value;
        params += "&MilitarySchoolId=" + militarySchoolId;
        params += "&Year=" + year;
        params += "&SpecializationId=" + specializationId;
        
        function response_handler(xml)
        {
            if (xmlValue(xml, "response") != "OK")
            {
                lblFullNameValue.innerHTML = xmlNodeText(xml.childNodes[1]);
                lblFullNameValue.className = "ErrorText";
            }
            else
            {
                document.getElementById(txtIdentityNumberClientID).value = "";
                document.getElementById(hdnRefreshReasonClientID).value = "RANKED";
                document.getElementById(btnRefreshClientID).click();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeclassingCadet(cadetSchoolSubjectId)
{
    YesNoDialog("Желаете ли да декласирате курсанта?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "CadetsRanking.aspx?AjaxMethod=JSDeclassingCadet";
        var params = "";
        params += "CadetSchoolSubjectId=" + cadetSchoolSubjectId;
        
        function response_handler(xml)
        {
            if (xmlValue(xml, "response") == "OK")
            {
                document.getElementById(hdnRefreshReasonClientID).value = "DECLASSED";
                document.getElementById(btnRefreshClientID).click();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}