// Generate specialization items grid
function ShowSpecializationsTable(orderBy, pageIdx)
{
    GetSpecializationItems(orderBy, pageIdx, 0);
}

// Shows the light box with specialization items and "disable" rest of the page
function ShowSpecializationsLightBox()
{
    document.getElementById(lblGridMessageClientID).innerHTML = "";
    document.getElementById("HidePage").style.display = "";
    document.getElementById("divSpecializationsLightBox").style.display = "";
    CenterLightBox("divSpecializationsLightBox");
}


// Close the light box and clear its content
function HideSpecializationsLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divSpecializationsLightBox").style.display = "none";
    document.getElementById("divSpecializationsLightBoxContent").innerHTML = "";
}

// Call web server and retrieve the generated specialization items grid from it
function GetSpecializationItems(orderBy, pageIdx, isPaging)
{
    var url = "ManageMilitarySchoolSpecializations.aspx?AjaxMethod=JSGetUnusedSpecializationItems";
    var params = "";
    params += "&MilitarySchoolID=" + GetSelectedItemId(document.getElementById(ddlMilitarySchoolsClientID));
    params += "&Year=" + GetSelectedItemId(document.getElementById(ddlSchoolYearsClientID));
    
    if (isPaging == 1)
        params += "&IsPaging=1";
    else
        params += "&IsPaging=0";
        
    var txtLightBoxSpecialization = document.getElementById('txtLightBoxSubject');
    if (txtLightBoxSpecialization != null)
        params += "&SubjectName=" + document.getElementById('txtLightBoxSubject').value;
    
    var txtLightBoxSubject = document.getElementById('txtLightBoxSubject');
    if (txtLightBoxSubject != null)    
        params += "&SpecializationName=" + document.getElementById('txtLightBoxSpecialization').value;
    
    params += "&SpecTableOrderBy=" + orderBy;
    params += "&SpecTablePageIdx=" + pageIdx;
    
    function response_handler(xml)
    {
        if(xmlValue(xml, "response") != "")
        {
            document.getElementById('divSpecializationsLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
            ShowSpecializationsLightBox();
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function SortSpecializationsTableBy(sort)
{
    var pageIdx = document.getElementById('hdnSpecializationsTablePageIdx').value;
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    if (orderBy == sort)
    {
        sort = sort + 100;
    }
    
    orderBy = sort;
    
    GetSpecializationItems(orderBy, pageIdx, 0);
}

//Go to the first page and refresh the grid
function BtnSpecializationsTableFirstClick()
{
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    var pageIdx = 1;
    var maxPage = document.getElementById('hdnSpecializationsTableMaxPage').value;
    
    GetSpecializationItems(orderBy, pageIdx, 1);
}

//Go to the previous page and refresh the grid
function BtnSpecializationsTablePrevClick()
{
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    var pageIdx = document.getElementById('hdnSpecializationsTablePageIdx').value;

    if (pageIdx > 1)
    {
        pageIdx--;
        GetSpecializationItems(orderBy, pageIdx, 1);
    }
}

//Go to the next page and refresh the grid
function BtnSpecializationsTableNextClick()
{
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    var pageIdx = document.getElementById('hdnSpecializationsTablePageIdx').value;
    var maxPage = document.getElementById('hdnSpecializationsTableMaxPage').value;

    if (pageIdx < maxPage)
    {
        pageIdx++;
        GetSpecializationItems(orderBy, pageIdx, 1);
    }
}

//Go to the last page and refresh the grid
function BtnSpecializationsTableLastClick()
{
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnSpecializationsTableMaxPage').value;

    pageIdx = maxPage;
    GetSpecializationItems(orderBy, pageIdx, 1);
}

//Go to a specific page (entered by the user) and refresh the grid
function BtnSpecializationsTableGotoClick()
{
    var orderBy = document.getElementById('hdnSpecializationsTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnSpecializationsTableMaxPage').value;
    var goToPage = document.getElementById('txtSpecializationsTableGotoPage').value;

    if (isInt(TrimString(goToPage)) && goToPage > 0 && goToPage <= maxPage)
    {
        pageIdx = goToPage;
        GetSpecializationItems(orderBy, pageIdx, 1);
    }
}

// Call web server to add selected specialization item to the contextual military school and year
function AddSpecializationToMilitarySchool(specializationId)
{
    var url = "ManageMilitarySchoolSpecializations.aspx?AjaxMethod=JSAddSpecializationToMilitarySchool";
    var params = "";
    params += "&MilitarySchoolID=" + GetSelectedItemId(document.getElementById(ddlMilitarySchoolsClientID));
    params += "&Year=" + GetSelectedItemId(document.getElementById(ddlSchoolYearsClientID));
    params += "&SpecializationID=" + specializationId;
    
    function response_handler(xml)
    {
        HideSpecializationsLightBox();
        var lblGridMessage = document.getElementById(lblGridMessageClientID);
        lblGridMessage.innerHTML = "Специализацията е добавена успешно";
        lblGridMessage.className = "SuccessText";
        GetMilitarySchoolSpecializationItems(1);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Call web server and retrieve the generated military school specialization items grid from it
function GetMilitarySchoolSpecializationItems(orderBy)
{
    var url = "ManageMilitarySchoolSpecializations.aspx?AjaxMethod=JSRefreshMilitarySchoolSpecializations";
    var params = "";
    params += "&MilitarySchoolID=" + GetSelectedItemId(document.getElementById(ddlMilitarySchoolsClientID));
    params += "&Year=" + GetSelectedItemId(document.getElementById(ddlSchoolYearsClientID));
    params += "&OrderBy=" + orderBy;
    
    function response_handler(xml)
    {
        if(xmlValue(xml, "response") != "")
        {
            document.getElementById(divMilitSchoolSpecsGridClientID).innerHTML = xmlNodeText(xml.childNodes[0]);
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function SortMilitarySchoolSpecializationsTableBy(sort)
{
    var hdnSortBy = document.getElementById(hdnSortByClientID);
    var orderBy = hdnSortBy.value;
    if (orderBy == sort)
    {
        sort = sort + 100;
    }
    
    hdnSortBy.value = sort;
    
    document.getElementById(btnRefreshClientID).click();
}

function DeleteMilitarySchoolSpecialization(militarySchoolSpecializationId)
{
    YesNoDialog("Желаете ли да изтриете специализацията?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageMilitarySchoolSpecializations.aspx?AjaxMethod=JSDeleteMilitarySchoolSpecialization";
        var params = "";
        params += "MilitarySchoolSpecializationID=" + militarySchoolSpecializationId;
        
        function response_handler(xml)
        {
            var lblGridMessage = document.getElementById(lblGridMessageClientID);
            lblGridMessage.innerHTML = "Специализацията е изтрита успешно";
            lblGridMessage.className = "SuccessText";
            GetMilitarySchoolSpecializationItems(1);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}