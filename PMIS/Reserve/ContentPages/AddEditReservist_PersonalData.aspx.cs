using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;
using PMIS.Common.DAL;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist_PersonalData : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilitaryRank")
            {
                JSLoadMilitaryRank();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilitaryRank")
            {
                JSSaveMilitaryRank();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPersonalDetailsByReservistId")
            {
                JSLoadPersonalDetailsByReservistId();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPersonalDetailsByPersonId")
            {
                JSLoadPersonalDetailsByPersonId();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePersonalData")
            {
                JSSavePersonalData();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckIdentNumber")
            {
                JSCheckIdentNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateMunicipality")
            {
                JSRepopulateMunicipality();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateCity")
            {
                JSRepopulateCity();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulatePostCode")
            {
                JSRepopulatePostCode();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateRegionMunicipalityCity")
            {
                JSRepopulateRegionMunicipalityCity();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulatePostCodeAndDistrict")
            {
                JSRepopulatePostCodeAndDistrict();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateDistrictPostCode")
            {
                JSRepopulateDistrictPostCode();
                return;
            }
    
                        
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateRegionMunicipalityCityDistrict")
            {
                JSRepopulateRegionMunicipalityCityDistrict();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshSizeClothingList")
            {
                JSRefreshSizeClothingList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshSizeHatList")
            {
                JSRefreshSizeHatList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshSizeShoesList")
            {
                JSRefreshSizeShoesList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRecordOfServiceArchives")
            {
                JSLoadRecordOfServiceArchives();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRecordOfService")
            {
                JSSaveRecordOfService();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRecordOfService")
            {
                JSLoadRecordOfService();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRecordOfService")
            {
                JSDeleteRecordOfService();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadConvictions")
            {
                JSLoadConvictions();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveConviction")
            {
                JSSaveConviction();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadConviction")
            {
                JSLoadConviction();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteConviction")
            {
                JSDeleteConviction();
                return;
            }


            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadDualCitizenships")
            {
                JSLoadDualCitizenships();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveDualCitizenship")
            {
                JSSaveDualCitizenship();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadDualCitizenship")
            {
                JSLoadDualCitizenship();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteDualCitizenship")
            {
                JSDeleteDualCitizenship();
                return;
            }
        }

        // Load a particular MilitaryRan (ajax call)
        private void JSLoadMilitaryRank()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personID = int.Parse(Request.Form["PersonID"]);

                PersonMilitaryRank personMilitaryRank = PersonMilitaryRankUtil.GetPersonMilitaryRank(personID, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personMilitaryRank>
                                        <MilitaryRankMilitaryRankId>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(personMilitaryRank.MilitaryRankId) ? ListItems.GetOptionChooseOne().Value : personMilitaryRank.MilitaryRankId) + @"</MilitaryRankMilitaryRankId>
                                        <MilitaryRankVacAnn>" + AJAXTools.EncodeForXML(personMilitaryRank.VacAnn) + @"</MilitaryRankVacAnn>
                                        <MilitaryRankDateArchive>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personMilitaryRank.DateArchive)) + @"</MilitaryRankDateArchive>
                                        <MilitaryRankDateWhen>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personMilitaryRank.DateWhen)) + @"</MilitaryRankDateWhen>
                                        <MilitaryRankMilitaryCommanderRankCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryRank.MilitaryCommanderRankCode) ? personMilitaryRank.MilitaryCommanderRankCode : ListItems.GetOptionChooseOne().Value) + @"</MilitaryRankMilitaryCommanderRankCode>
                                        <MilitaryRankDR>" + (personMilitaryRank.IsDR ? "1" : "0") + @"</MilitaryRankDR>     
                             </personMilitaryRank>";

            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Save a particular MilitaryRank (ajax call)
        private void JSSaveMilitaryRank()
        {
            /*
             "&PersonID=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilitaryRankMilitaryRankID=" + document.getElementById("ddPersonMilitaryRankMilitaryRank").value +
                     "&MilitaryRankVacAnn=" + MilitaryRankVacAnn +
                     "&MilitaryRankDateArchive=" + document.getElementById("txtPersonMilitaryRankDateArchive").value +
                     "&MilitaryRankDateWhen=" + document.getElementById("txtPersonMilitaryRankDateWhen").value +
                     "&MilitaryRankMilitaryCommanderRankCode=" + MilitaryRankMilitaryCommanderRankCode;
            */
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                //int personArchiveTitleId = int.Parse(Request.Form["ArchiveTitleId"]);
                int personID = int.Parse(Request.Form["PersonID"]);

                string personMilitaryRankMilitaryRankId = Request.Form["MilitaryRankMilitaryRankId"];
                string personMilitaryRankVacAnn = Request.Form["MilitaryRankVacAnn"];
                DateTime personMilitaryRankDateArchive = CommonFunctions.ParseDate(Request.Form["MilitaryRankDateArchive"]).Value;
                DateTime personMilitaryRankDateWhen = CommonFunctions.ParseDate(Request.Form["MilitaryRankDateWhen"]).Value;
                string personMilitaryRankMilitaryCommanderRankCode = Request.Form["MilitaryRankMilitaryCommanderRankCode"];
                bool personMilitaryRankDR = Request.Form["MilitaryRankDR"].ToString() == "1" ? true : false;

                Person person = PersonUtil.GetPerson(personID, CurrentUser);

                PersonMilitaryRank personMilitaryRank = new PersonMilitaryRank(CurrentUser);

                personMilitaryRank.PersonId = personID;
                personMilitaryRank.MilitaryRankId = personMilitaryRankMilitaryRankId;
                if (personMilitaryRankVacAnn != "")
                    personMilitaryRank.VacAnn = personMilitaryRankVacAnn;
                personMilitaryRank.DateArchive = personMilitaryRankDateArchive;
                personMilitaryRank.DateWhen = personMilitaryRankDateWhen;
                if (personMilitaryRankMilitaryCommanderRankCode != "-1")
                    personMilitaryRank.MilitaryCommanderRankCode = personMilitaryRankMilitaryCommanderRankCode;

                personMilitaryRank.IsDR = personMilitaryRankDR;

                PersonMilitaryRankUtil.SavePersonMilitaryRank(personMilitaryRank, person, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<militaryRankShortName>" + (personMilitaryRank.MilitaryRank != null ? AJAXTools.EncodeForXML(personMilitaryRank.MilitaryRank.ShortName) : "") + "</militaryRankShortName>" +
                           "<militaryCategoryName>" + (personMilitaryRank.MilitaryRank != null && personMilitaryRank.MilitaryRank.MilitaryCategory != null ? AJAXTools.EncodeForXML(personMilitaryRank.MilitaryRank.MilitaryCategory.CategoryName) : "") + "</militaryCategoryName>" +
                           "<militaryRankDR>" + (personMilitaryRank.IsDR ? "1" : "0") + "</militaryRankDR>";

            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load Personal details (ajax call)
        private void JSLoadPersonalDetailsByReservistId()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int reservistId = 0;
            if (!String.IsNullOrEmpty(Request.Form["ReservistId"]))
                reservistId = int.Parse(Request.Form["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                string drivingLicenseCategories = "";

                foreach (DrivingLicenseCategory category in reservist.Person.DrivingLicenseCategories)
                {
                    drivingLicenseCategories += (drivingLicenseCategories == "" ? "" : ",") +
                        category.DrivingLicenseCategoryId.ToString();
                }

                string resMilRepStatus = MilitaryReportStatusUtil.GetLabelWhenLackOfStatus();
                string resCurrMilDepartment = MilitaryReportStatusUtil.GetLabelWhenLackOfStatus();

                if (reservist.CurrResMilRepStatus != null)
                {
                    resMilRepStatus = reservist.CurrResMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName;

                    if (reservist.CurrResMilRepStatus.SourceMilDepartment != null)
                        resCurrMilDepartment = reservist.CurrResMilRepStatus.SourceMilDepartment.MilitaryDepartmentName;
                    else
                        resCurrMilDepartment = "";
                }

                stat = AJAXTools.OK;

                PersonMilitaryRank personMilitaryRank = PersonMilitaryRankUtil.GetPersonMilitaryRank(reservist.Person.PersonId, CurrentUser);

                response = @"
                    <person>
                         <personId>" + AJAXTools.EncodeForXML(reservist.Person.PersonId.ToString()) + @"</personId>
                         <identNumber>" + AJAXTools.EncodeForXML(reservist.Person.IdentNumber) + @"</identNumber>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(resMilRepStatus) + @"</resMilRepStatus> 
                         <currMilDepartment>" + AJAXTools.EncodeForXML(resCurrMilDepartment) + @"</currMilDepartment> 
                         <personStatus>" + AJAXTools.EncodeForXML(ReservistUtil.GetPersonStatus(reservist.Person, CurrentUser)) + @"</personStatus>
                         <firstName>" + AJAXTools.EncodeForXML(reservist.Person.FirstName) + @"</firstName>
                         <lastName>" + AJAXTools.EncodeForXML(reservist.Person.LastName) + @"</lastName>
                         <initials>" + AJAXTools.EncodeForXML(reservist.Person.Initials) + @"</initials>
                         <militaryRankId>" + AJAXTools.EncodeForXML(reservist.Person.MilitaryRank != null ? reservist.Person.MilitaryRank.MilitaryRankId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</militaryRankId>
                         <militaryRankName>" + AJAXTools.EncodeForXML(reservist.Person.MilitaryRank != null ? reservist.Person.MilitaryRank.ShortName.ToString() : "") + @"</militaryRankName>
                         <militaryRankDR>" + (personMilitaryRank != null ? (personMilitaryRank.IsDR ? "1" : "0") : "0") + @"</militaryRankDR>
                         <militaryCategoryName>" + AJAXTools.EncodeForXML(reservist.Person.MilitaryRank != null ? reservist.Person.MilitaryRank.MilitaryCategory.CategoryName : "") + @"</militaryCategoryName>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(reservist.Person.LastModifiedDate)) + @"</lastModified>
                         <IDCardNumber>" + AJAXTools.EncodeForXML(reservist.Person.IDCardNumber) + @"</IDCardNumber>
                         <IDCardIssuedBy>" + AJAXTools.EncodeForXML(reservist.Person.IDCardIssuedBy) + @"</IDCardIssuedBy>
                         <IDCardIssueDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(reservist.Person.IDCardIssueDate)) + @"</IDCardIssueDate>
                         <genderId>" + AJAXTools.EncodeForXML(reservist.Person.Gender != null ? reservist.Person.Gender.GenderId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</genderId>
                         <birthCountryId>" + AJAXTools.EncodeForXML(reservist.Person.BirthCountry != null ? reservist.Person.BirthCountry.CountryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthCountryId>
                         <birthCityId>" + AJAXTools.EncodeForXML(reservist.Person.BirthCity != null ? reservist.Person.BirthCity.CityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthCityId>
                         <birthMunicipalityId>" + AJAXTools.EncodeForXML(reservist.Person.BirthCity != null ? reservist.Person.BirthCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthMunicipalityId>
                         <birthRegionId>" + AJAXTools.EncodeForXML(reservist.Person.BirthCity != null ? reservist.Person.BirthCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthRegionId>
                         <birthPostCode>" + AJAXTools.EncodeForXML(reservist.Person.BirthCity != null ? reservist.Person.BirthCity.PostCode.ToString() : "") + @"</birthPostCode>
                         <birthCityIfAbroad>" + AJAXTools.EncodeForXML(reservist.Person.BirthCityIfAbroad) + @"</birthCityIfAbroad>
                         <birthAbroad>" + AJAXTools.EncodeForXML(reservist.Person.BirthCountry != null && reservist.Person.BirthCountry.IsBulgaria ? "0" : "1") + @"</birthAbroad>
                         <drivingLicenseCategories>" + AJAXTools.EncodeForXML(drivingLicenseCategories) + @"</drivingLicenseCategories>
                         <hasMilitarySrv>" + AJAXTools.EncodeForXML(reservist.Person.HasMilitaryService != null ? reservist.Person.HasMilitaryService.ToString() : "") + @"</hasMilitarySrv>                         
                         <militaryTraining>" + AJAXTools.EncodeForXML(reservist.Person.MilitaryTraining != null ? reservist.Person.MilitaryTraining.ToString() : "") + @"</militaryTraining>
                         <recordOfServiceSeries>" + AJAXTools.EncodeForXML(reservist.Person.RecordOfServiceSeries) + @"</recordOfServiceSeries>
                         <recordOfServiceNumber>" + AJAXTools.EncodeForXML(reservist.Person.RecordOfServiceNumber) + @"</recordOfServiceNumber>
                         <recordOfServiceDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(reservist.Person.RecordOfServiceDate)) + @"</recordOfServiceDate>
                         <recordOfServiceCopy>" + AJAXTools.EncodeForXML(reservist.Person.RecordOfServiceCopy ? "1" : "0") + @"</recordOfServiceCopy>
                         <permCityId>" + AJAXTools.EncodeForXML(reservist.Person.PermCityId != null ? reservist.Person.PermCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permCityId>
                         <permPostCode>" + AJAXTools.EncodeForXML(reservist.Person.PermDistrictId != null && !String.IsNullOrEmpty(reservist.Person.PermDistrict.PostCode) ? reservist.Person.PermDistrict.PostCode : reservist.Person.PermCityId != null ? reservist.Person.PermCity.PostCode.ToString() : "") + @"</permPostCode>
                         <permSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(reservist.Person.PermSecondPostCode) ? "" : reservist.Person.PermSecondPostCode) + @"</permSecondPostCode>
                         <permRegionId>" + AJAXTools.EncodeForXML(reservist.Person.PermCityId != null ? reservist.Person.PermCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permRegionId>
                         <permMunicipalityId>" + AJAXTools.EncodeForXML(reservist.Person.PermCityId != null ? reservist.Person.PermCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permMunicipalityId>
                         <permDistrictId>" + AJAXTools.EncodeForXML(reservist.Person.PermDistrictId != null ? reservist.Person.PermDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permDistrictId>
                         <permAddress>" + AJAXTools.EncodeForXML(reservist.Person.PermAddress) + @"</permAddress>
                         <currCityId>" + AJAXTools.EncodeForXML(reservist.Person.PresCityId != null ? reservist.Person.PresCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currCityId>
                         <currPostCode>" + AJAXTools.EncodeForXML(reservist.Person.PresDistrictId != null && !String.IsNullOrEmpty(reservist.Person.PresDistrict.PostCode) ? reservist.Person.PresDistrict.PostCode : reservist.Person.PresCityId != null ? reservist.Person.PresCity.PostCode.ToString() : "") + @"</currPostCode>
                         <presSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(reservist.Person.PresSecondPostCode) ? "" : reservist.Person.PresSecondPostCode) + @"</presSecondPostCode>
                         <currRegionId>" + AJAXTools.EncodeForXML(reservist.Person.PresCityId != null ? reservist.Person.PresCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currRegionId>
                         <currMunicipalityId>" + AJAXTools.EncodeForXML(reservist.Person.PresCityId != null ? reservist.Person.PresCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currMunicipalityId>
                         <currDistrictId>" + AJAXTools.EncodeForXML(reservist.Person.PresDistrictId != null ? reservist.Person.PresDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currDistrictId>
                         <currAddress>" + AJAXTools.EncodeForXML(reservist.Person.PresAddress) + @"</currAddress>
                         <homePhone>" + AJAXTools.EncodeForXML(reservist.Person.HomePhone != null ? reservist.Person.HomePhone.ToString() : "") + @"</homePhone>
                         <mobilePhone>" + AJAXTools.EncodeForXML(reservist.Person.MobilePhone) + @"</mobilePhone>
                         <businessPhone>" + AJAXTools.EncodeForXML(reservist.Person.BusinessPhone) + @"</businessPhone>
                         <email>" + AJAXTools.EncodeForXML(reservist.Person.Email) + @"</email>
                         <maritalStatus>" + AJAXTools.EncodeForXML(reservist.Person.MaritalStatus != null ? reservist.Person.MaritalStatus.MaritalStatusKey.ToString() : ListItems.GetOptionChooseOne().Value) + @"</maritalStatus>
                         <parentsContact>" + AJAXTools.EncodeForXML(reservist.Person.ParentsContact) + @"</parentsContact>
                         <childCount>" + AJAXTools.EncodeForXML(reservist.Person.ChildCount.HasValue ? reservist.Person.ChildCount.ToString() : "") + @"</childCount>
                         <sizeClothingId>" + AJAXTools.EncodeForXML(reservist.Person.SizeClothingId.HasValue ? reservist.Person.SizeClothingId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeClothingId>
                         <sizeHatId>" + AJAXTools.EncodeForXML(reservist.Person.SizeHatId.HasValue ? reservist.Person.SizeHatId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeHatId>
                         <sizeShoesId>" + AJAXTools.EncodeForXML(reservist.Person.SizeShoesId.HasValue ? reservist.Person.SizeShoesId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeShoesId>
                         <personHeight>" + AJAXTools.EncodeForXML(reservist.Person.PersonHeight.HasValue ? reservist.Person.PersonHeight.ToString() : "") + @"</personHeight>
                         <isAbroad>" + AJAXTools.EncodeForXML(reservist.Person.IsAbroad ? "1" : "0") + @"</isAbroad>
                         <abroadCountryId>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(reservist.Person.AbroadCountryId) ? reservist.Person.AbroadCountryId : ListItems.GetOptionChooseOne().Value) + @"</abroadCountryId>
                         <abroadSince>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(reservist.Person.AbroadSince)) + @"</abroadSince>
                         <abroadPeriod>" + AJAXTools.EncodeForXML(reservist.Person.AbroadPeriod.HasValue ? reservist.Person.AbroadPeriod.ToString() : "") + @"</abroadPeriod>
                    </person>";

                if (reservist.Person.BirthCountry != null && reservist.Person.BirthCountry.IsBulgaria && reservist.Person.BirthCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(reservist.Person.BirthCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(reservist.Person.BirthCity.MunicipalityId, CurrentUser);

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<b_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</b_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<b_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</b_c>";
                    }
                }

                if (reservist.Person.PermCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(reservist.Person.PermCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(reservist.Person.PermCity.MunicipalityId, CurrentUser);
                    List<District> districts = reservist.Person.PermCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<p_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</p_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<p_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</p_c>";
                    }

                    response += "<p_d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</p_d>";

                    foreach (District district in districts)
                    {
                        response += "<p_d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</p_d>";
                    }
                }

                if (reservist.Person.PresCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(reservist.Person.PresCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(reservist.Person.PresCity.MunicipalityId, CurrentUser);
                    List<District> districts = reservist.Person.PresCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<c_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</c_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c_c>";
                    }

                    response += "<c_d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</c_d>";

                    foreach (District district in districts)
                    {
                        response += "<c_d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</c_d>";
                    }
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load Personal details (ajax call)
        private void JSLoadPersonalDetailsByPersonId()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int personId = 0;
            if (!String.IsNullOrEmpty(Request.Form["PersonId"]))
                personId = int.Parse(Request.Form["PersonId"]);

            string stat = "";
            string response = "";

            try
            {
                Person person = PersonUtil.GetPerson(personId, CurrentUser);

                string drivingLicenseCategories = "";

                foreach (DrivingLicenseCategory category in person.DrivingLicenseCategories)
                {
                    drivingLicenseCategories += (drivingLicenseCategories == "" ? "" : ",") +
                        category.DrivingLicenseCategoryId.ToString();
                }

                PersonMilitaryRank personMilitaryRank = PersonMilitaryRankUtil.GetPersonMilitaryRank(person.PersonId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"
                    <person>
                         <identNumber>" + AJAXTools.EncodeForXML(person.IdentNumber) + @"</identNumber>
                         <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(MilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</resMilRepStatus>  
                         <currMilDepartment>" + AJAXTools.EncodeForXML(MilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <personStatus>" + AJAXTools.EncodeForXML(ReservistUtil.GetPersonStatus(person, CurrentUser)) + @"</personStatus>
                         <firstName>" + AJAXTools.EncodeForXML(person.FirstName) + @"</firstName>
                         <lastName>" + AJAXTools.EncodeForXML(person.LastName) + @"</lastName>
                         <initials>" + AJAXTools.EncodeForXML(person.Initials) + @"</initials>
                         <militaryRankId>" + AJAXTools.EncodeForXML(person.MilitaryRank != null ? person.MilitaryRank.MilitaryRankId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</militaryRankId>
                         <militaryRankName>" + AJAXTools.EncodeForXML(person.MilitaryRank != null ? person.MilitaryRank.ShortName.ToString() : "") + @"</militaryRankName>
                         <militaryRankDR>" + (personMilitaryRank != null ? (personMilitaryRank.IsDR ? "1" : "0") : "0") + @"</militaryRankDR>
                         <militaryCategoryName>" + AJAXTools.EncodeForXML(person.MilitaryRank != null ? person.MilitaryRank.MilitaryCategory.CategoryName : "") + @"</militaryCategoryName>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(person.LastModifiedDate)) + @"</lastModified>
                         <IDCardNumber>" + AJAXTools.EncodeForXML(person.IDCardNumber) + @"</IDCardNumber>
                         <IDCardIssuedBy>" + AJAXTools.EncodeForXML(person.IDCardIssuedBy) + @"</IDCardIssuedBy>
                         <IDCardIssueDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(person.IDCardIssueDate)) + @"</IDCardIssueDate>
                         <genderId>" + AJAXTools.EncodeForXML(person.Gender != null ? person.Gender.GenderId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</genderId>
                         <birthCountryId>" + AJAXTools.EncodeForXML(person.BirthCountry != null ? person.BirthCountry.CountryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthCountryId>
                         <birthCityId>" + AJAXTools.EncodeForXML(person.BirthCity != null ? person.BirthCity.CityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthCityId>
                         <birthMunicipalityId>" + AJAXTools.EncodeForXML(person.BirthCity != null ? person.BirthCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthMunicipalityId>
                         <birthRegionId>" + AJAXTools.EncodeForXML(person.BirthCity != null ? person.BirthCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</birthRegionId>
                         <birthPostCode>" + AJAXTools.EncodeForXML(person.BirthCity != null ? person.BirthCity.PostCode.ToString() : "") + @"</birthPostCode>
                         <birthCityIfAbroad>" + AJAXTools.EncodeForXML(person.BirthCityIfAbroad) + @"</birthCityIfAbroad>
                         <birthAbroad>" + AJAXTools.EncodeForXML(person.BirthCountry != null && person.BirthCountry.IsBulgaria ? "0" : "1") + @"</birthAbroad>
                         <drivingLicenseCategories>" + AJAXTools.EncodeForXML(drivingLicenseCategories) + @"</drivingLicenseCategories>
                         <hasMilitarySrv>" + AJAXTools.EncodeForXML(person.HasMilitaryService != null ? person.HasMilitaryService.ToString() : "") + @"</hasMilitarySrv>                         
                         <militaryTraining>" + AJAXTools.EncodeForXML(person.MilitaryTraining != null ? person.MilitaryTraining.ToString() : "") + @"</militaryTraining>
                         <recordOfServiceSeries>" + AJAXTools.EncodeForXML(person.RecordOfServiceSeries) + @"</recordOfServiceSeries>
                         <recordOfServiceNumber>" + AJAXTools.EncodeForXML(person.RecordOfServiceNumber) + @"</recordOfServiceNumber>
                         <recordOfServiceDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(person.RecordOfServiceDate)) + @"</recordOfServiceDate>
                         <recordOfServiceCopy>" + AJAXTools.EncodeForXML(person.RecordOfServiceCopy ? "1" : "0") + @"</recordOfServiceCopy>
                         <permCityId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permCityId>
                         <permPostCode>" + AJAXTools.EncodeForXML(person.PermDistrictId != null && !String.IsNullOrEmpty(person.PermDistrict.PostCode) ? person.PermDistrict.PostCode : person.PermCityId != null ? person.PermCity.PostCode.ToString() : "") + @"</permPostCode>
                         <permSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PermSecondPostCode) ? "" : person.PermSecondPostCode) + @"</permSecondPostCode>
                         <permRegionId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permRegionId>
                         <permMunicipalityId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permMunicipalityId>
                         <permDistrictId>" + AJAXTools.EncodeForXML(person.PermDistrictId != null ? person.PermDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permDistrictId>
                         <permAddress>" + AJAXTools.EncodeForXML(person.PermAddress) + @"</permAddress>
                         <currCityId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currCityId>
                         <currPostCode>" + AJAXTools.EncodeForXML(person.PresDistrictId != null && !String.IsNullOrEmpty(person.PresDistrict.PostCode) ? person.PresDistrict.PostCode : person.PresCityId != null ? person.PresCity.PostCode.ToString() : "") + @"</currPostCode>
                         <presSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PresSecondPostCode) ? "" : person.PresSecondPostCode) + @"</presSecondPostCode>
                         <currRegionId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currRegionId>
                         <currMunicipalityId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currMunicipalityId>
                         <currDistrictId>" + AJAXTools.EncodeForXML(person.PresDistrictId != null ? person.PresDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</currDistrictId>
                         <currAddress>" + AJAXTools.EncodeForXML(person.PresAddress) + @"</currAddress>
                         <homePhone>" + AJAXTools.EncodeForXML(person.HomePhone != null ? person.HomePhone.ToString() : "") + @"</homePhone>
                         <mobilePhone>" + AJAXTools.EncodeForXML(person.MobilePhone) + @"</mobilePhone>
                         <businessPhone>" + AJAXTools.EncodeForXML(person.BusinessPhone) + @"</businessPhone>
                         <email>" + AJAXTools.EncodeForXML(person.Email) + @"</email>
                         <maritalStatus>" + AJAXTools.EncodeForXML(person.MaritalStatus != null ? person.MaritalStatus.MaritalStatusKey.ToString() : ListItems.GetOptionChooseOne().Value) + @"</maritalStatus>
                         <parentsContact>" + AJAXTools.EncodeForXML(person.ParentsContact) + @"</parentsContact>
                         <childCount>" + AJAXTools.EncodeForXML(person.ChildCount.HasValue ? person.ChildCount.ToString() : "") + @"</childCount>
                         <sizeClothingId>" + AJAXTools.EncodeForXML(person.SizeClothingId.HasValue ? person.SizeClothingId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeClothingId>
                         <sizeHatId>" + AJAXTools.EncodeForXML(person.SizeHatId.HasValue ? person.SizeHatId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeHatId>
                         <sizeShoesId>" + AJAXTools.EncodeForXML(person.SizeShoesId.HasValue ? person.SizeShoesId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</sizeShoesId>
                         <personHeight>" + AJAXTools.EncodeForXML(person.PersonHeight.HasValue ? person.PersonHeight.ToString() : "") + @"</personHeight>
                         <isAbroad>" + AJAXTools.EncodeForXML(person.IsAbroad ? "1" : "0") + @"</isAbroad>
                         <abroadCountryId>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(person.AbroadCountryId) ? person.AbroadCountryId : ListItems.GetOptionChooseOne().Value) + @"</abroadCountryId>
                         <abroadSince>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(person.AbroadSince)) + @"</abroadSince>
                         <abroadPeriod>" + AJAXTools.EncodeForXML(person.AbroadPeriod.HasValue ? person.AbroadPeriod.ToString() : "") + @"</abroadPeriod>
                         <militaryUnit>" + AJAXTools.EncodeForXML(person.MilitaryUnit != null ? person.MilitaryUnit.DisplayTextForSelection : "") + @"</militaryUnit>
                    </person>";

                if (person.BirthCountry != null && person.BirthCountry.IsBulgaria && person.BirthCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.BirthCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(person.BirthCity.MunicipalityId, CurrentUser);

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<b_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</b_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<b_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</b_c>";
                    }
                }

                if (person.PermCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.PermCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(person.PermCity.MunicipalityId, CurrentUser);
                    List<District> districts = person.PermCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<p_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</p_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<p_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</p_c>";
                    }

                    response += "<p_d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</p_d>";

                    foreach (District district in districts)
                    {
                        response += "<p_d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</p_d>";
                    }
                }

                if (person.PresCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.PresCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(person.PresCity.MunicipalityId, CurrentUser);
                    List<District> districts = person.PresCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<c_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</c_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c_c>";
                    }

                    response += "<c_d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</c_d>";

                    foreach (District district in districts)
                    {
                        response += "<c_d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</c_d>";
                    }
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Save Personal details (ajax call)
        private void JSSavePersonalData()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? personId = null;
            if (!String.IsNullOrEmpty(Request.Params["PersonId"]))
            {
                personId = int.Parse(Request.Params["PersonId"]);
            }

            int? reservistId = null;
            if (!String.IsNullOrEmpty(Request.Params["ReservistId"]))
            {
                reservistId = int.Parse(Request.Params["ReservistId"]);
            }

            string firstName = Request.Params["FirstName"];
            string lastName = Request.Params["LastName"];
            string identNumber = Request.Params["IdentNumber"];
            string initials = Request.Params["Initials"];

            int? genderId = null;
            if (!String.IsNullOrEmpty(Request.Params["GenderId"]) &&
                Request.Params["GenderId"] != ListItems.GetOptionChooseOne().Value)
            {
                genderId = int.Parse(Request.Params["GenderId"]);
            }

            string militaryRankId = "";
            if (!String.IsNullOrEmpty(Request.Params["MilitaryRankId"]) &&
                Request.Params["MilitaryRankId"] != ListItems.GetOptionChooseOne().Value)
            {
                militaryRankId = Request.Params["MilitaryRankId"];
            }

            string IDCardNumber = Request.Params["IDCardNumber"];
            string IDCardIssuedBy = Request.Params["IDCardIssuedBy"];
            string IDCardIssueDate = Request.Params["IDCardIssueDate"];

            string birthCountryId = "";
            if (!String.IsNullOrEmpty(Request.Params["BirthCountryId"]) &&
                Request.Params["BirthCountryId"] != ListItems.GetOptionChooseOne().Value)
            {
                birthCountryId = Request.Params["BirthCountryId"];
            }

            int isBirthAbroad = int.Parse(Request.Params["IsBirthAbroad"]);

            string birthCityIfAbroad = Request.Params["BirthCityIfAbroad"];

            int? birthCityId = null;
            if (!String.IsNullOrEmpty(Request.Params["BirthCityId"]) &&
                Request.Params["BirthCityId"] != ListItems.GetOptionChooseOne().Value)
            {
                birthCityId = int.Parse(Request.Params["BirthCityId"]);
            }

            string drivingLicenseCategories = Request.Form["DrivingLicenseCategories"];

            int? hasMilitarySrv = null;
            if (!String.IsNullOrEmpty(Request.Form["HasMilitarySrv"]) &&
                Request.Form["HasMilitarySrv"] != ListItems.GetOptionChooseOne().Value)
            {
                hasMilitarySrv = int.Parse(Request.Form["HasMilitarySrv"]);
            }

            string militaryTraining = Request.Form["MilitaryTraining"];
            string recordOfServiceSeries = Request.Form["RecordOfServiceSeries"];
            string recordOfServiceNumber = Request.Form["RecordOfServiceNumber"];
            string recordOfServiceDate = Request.Form["RecordOfServiceDate"];
            string recordOfServiceCopy = Request.Form["RecordOfServiceCopy"];

            int? permCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["PermCityID"]) &&
                Request.Form["PermCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                permCityId = int.Parse(Request.Form["PermCityID"]);
            }

            int? permDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["PermDistrictID"]) &&
                Request.Form["PermDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                permDistrictId = int.Parse(Request.Form["PermDistrictID"]);
            }

            string permSecondPostCode = Request.Form["PermSecondPostCode"];
            string permAddress = Request.Form["PermAddress"];

            int? presCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["PresCityID"]) &&
                Request.Form["PresCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                presCityId = int.Parse(Request.Form["PresCityID"]);
            }

            string presSecondPostCode = Request.Form["PresSecondPostCode"];
            string presAddress = Request.Form["PresAddress"];

            int? presDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["PresDistrictID"]) &&
                Request.Form["PresDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                presDistrictId = int.Parse(Request.Form["PresDistrictID"]);
            }

            string homePhone = Request.Form["HomePhone"];
            string mobilePhone = Request.Form["MobilePhone"];
            string businessPhone = Request.Form["BusinessPhone"];
            string email = Request.Form["Email"];
            string maritalStatusKey = Request.Form["MaritalStatusKey"];
            string parentsContact = Request.Form["ParentsContact"];
            string childCount = Request.Form["ChildCount"];
            string sizeClothingId = Request.Form["SizeClothingID"];
            string sizeHatId = Request.Form["SizeHatID"];
            string sizeShoesId = Request.Form["SizeShoesID"];
            string personHeight = Request.Form["PersonHeight"];
            string isAbroad = Request.Form["IsAbroad"];
            string abroadCountryId = Request.Form["AbroadCountryId"];
            string abroadSince = Request.Form["AbroadSince"];
            string abroadPeriod = Request.Form["AbroadPeriod"];

            Person person = new Person(CurrentUser);
            Person oldPerson = null;
            if (personId.HasValue)
                oldPerson = PersonUtil.GetPerson(personId.Value, CurrentUser);

            person.PersonId = personId.HasValue ? personId.Value : 0;
            person.IdentNumber = identNumber;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.Initials = initials;
            person.Gender = genderId.HasValue ? GenderUtil.GetGender(CurrentUser, genderId.Value) : null;
            person.MilitaryRank = militaryRankId != "" ? MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser) : null;
            person.IDCardNumber = IDCardNumber;
            person.IDCardIssuedBy = IDCardIssuedBy;
            person.IDCardIssueDate = (!String.IsNullOrEmpty(IDCardIssueDate) ? CommonFunctions.ParseDate(IDCardIssueDate) : (DateTime?)null);
            person.BirthCountry = (!String.IsNullOrEmpty(birthCountryId) ? CountryUtil.GetCountry(birthCountryId, CurrentUser) : null);

            if (isBirthAbroad == 1)
            {
                person.BirthCityIfAbroad = birthCityIfAbroad;
            }
            else
            {
                person.BirthCityId = birthCityId;
                person.BirthCity = birthCityId.HasValue ? CityUtil.GetCity(birthCityId.Value, CurrentUser) : null;
            }

            person.DrivingLicenseCategories = new List<DrivingLicenseCategory>();

            if (!String.IsNullOrEmpty(drivingLicenseCategories))
            {
                List<DrivingLicenseCategory> listDrivingCategories = DrivingLicenseCategoryUtil.GetDrivingLicenseCategoryByCategoryId(drivingLicenseCategories, CurrentUser);
                foreach (DrivingLicenseCategory drivingLicenseCategory in listDrivingCategories)
                {
                    person.DrivingLicenseCategories.Add(drivingLicenseCategory);
                }
            }

            person.HasMilitaryService = hasMilitarySrv;

            if (militaryTraining != "")
                person.MilitaryTraining = int.Parse(militaryTraining);

            person.RecordOfServiceSeries = recordOfServiceSeries;
            person.RecordOfServiceNumber = recordOfServiceNumber;
            person.RecordOfServiceDate = (!String.IsNullOrEmpty(recordOfServiceDate) ? CommonFunctions.ParseDate(recordOfServiceDate) : (DateTime?)null);
            person.RecordOfServiceCopy = (recordOfServiceCopy == "1");

            person.PermCityId = permCityId;
            person.PermDistrictId = permDistrictId;
            person.PermSecondPostCode = permSecondPostCode;
            person.PermAddress = permAddress;
            person.PresCityId = presCityId;
            person.PresAddress = presAddress;
            person.PresDistrictId = presDistrictId;
            person.PresSecondPostCode = presSecondPostCode;

            if (!String.IsNullOrEmpty(homePhone))
                person.HomePhone = long.Parse(homePhone);

            person.MobilePhone = mobilePhone;
            person.BusinessPhone = businessPhone;
            person.Email = email;
            person.MaritalStatus = MaritalStatusUtil.GetMaritalStatus(CurrentUser, maritalStatusKey);
            person.ParentsContact = parentsContact;

            if (!String.IsNullOrEmpty(childCount))
                person.ChildCount = int.Parse(childCount);

            if (!String.IsNullOrEmpty(sizeClothingId) && sizeClothingId != ListItems.GetOptionChooseOne().Value)
                person.SizeClothingId = int.Parse(sizeClothingId);

            if (!String.IsNullOrEmpty(sizeHatId) && sizeHatId != ListItems.GetOptionChooseOne().Value)
                person.SizeHatId = int.Parse(sizeHatId);

            if (!String.IsNullOrEmpty(sizeShoesId) && sizeShoesId != ListItems.GetOptionChooseOne().Value)
                person.SizeShoesId = int.Parse(sizeShoesId);

            if (!String.IsNullOrEmpty(personHeight))
                person.PersonHeight = int.Parse(personHeight);

            person.IsAbroad = !String.IsNullOrEmpty(isAbroad) && isAbroad == "1";

            if (!String.IsNullOrEmpty(abroadCountryId) && abroadCountryId != ListItems.GetOptionChooseOne().Value)
                person.AbroadCountryId = abroadCountryId;

            person.AbroadSince = (!String.IsNullOrEmpty(abroadSince) ? CommonFunctions.ParseDate(abroadSince) : (DateTime?)null);

            if (!String.IsNullOrEmpty(abroadPeriod))
                person.AbroadPeriod = int.Parse(abroadPeriod);

            person.OtherInfo = null;

            Reservist reservist = new Reservist(CurrentUser);
            reservist.ReservistId = reservistId.HasValue ? reservistId.Value : 0;
            reservist.Person = person;
            reservist.PersonId = person.PersonId;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Reservists");

                //Save the Person changes
                if (person.PersonId == 0)
                {
                    PersonUtil.SavePerson_WhenAddingEditingReservist(person, "ADM_PersonDetails_Add", CurrentUser, change);
                }
                else
                {
                    PersonUtil.SavePerson_WhenAddingEditingReservist(person, "ADM_PersonDetails_Edit", CurrentUser, change);
                }

                //Add the Reservist if this is a new record       
                if (reservist.ReservistId == 0)
                {
                    ReservistUtil.AddReservist(reservist, CurrentUser, change);
                    ReservistUtil.ImportMilitaryReportSpecialityFromVitosha(reservist, CurrentUser);
                }

                if (person.IsAbroad && (oldPerson == null || oldPerson.IsAbroad != person.IsAbroad))
                {
                    ReservistMilRepStatus status = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservist.ReservistId, CurrentUser);

                    if (status == null || status.MilitaryReportStatus == null || status.MilitaryReportStatus.MilitaryReportStatusKey != "TEMPORARY_REMOVED")
                    {
                        ReservistMilRepStatusUtil.SetMilRepStatusTo_TEMPORARY_REMOVED(reservist.ReservistId, 1, person.AbroadSince, person.AbroadPeriod, CurrentUser, change);

                        List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservist.ReservistId, CurrentUser);

                        //Remove the Reservist from each Equipment Request
                        foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                        {
                            FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);
                        }

                        //Clear the Mobilization Appointment
                        ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(reservist.ReservistId, CurrentUser, change);
                    }
                }

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <reservistId>" + AJAXTools.EncodeForXML(reservist.ReservistId.ToString()) + @"</reservistId>
                             <personId>" + AJAXTools.EncodeForXML(reservist.Person.PersonId.ToString()) + @"</personId>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Check ident Number(ajax call)
        private void JSCheckIdentNumber()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string identNumber = Request.Params["IdentNumber"];

            string stat = "";
            string response = "";

            try
            {
                int reservistId = 0;
                int personId = 0;
                int isValidIdentNumber = 0;
                int noAccess = 0;
                int isMilitaryReportingPerson = 0;

                Reservist reservist = ReservistUtil.GetReservistByIdentNumber(identNumber, CurrentUser);

                if (reservist != null)
                {
                    reservistId = reservist.ReservistId;
                    personId = reservist.PersonId;
                    isValidIdentNumber = 1;
                }
                else
                {
                    Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);

                    if (person != null)
                    {
                        personId = person.PersonId;
                        isValidIdentNumber = 1;

                        if (Config.GetWebSetting("KOD_KZV_Check_Reservist").ToLower() == "true" &&
                            CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Reservist")) &&
                            !CommonFunctions.IsKeyInList(person.CategoryCode, Config.GetWebSetting("KOD_KAT_Restricted_Reservist_Exceptions")))
                        {
                            noAccess = 1;
                        }
                    }
                    else
                    {
                        if (PersonUtil.IsValidIdentityNumber(identNumber, CurrentUser))
                        {
                            isValidIdentNumber = 1;
                        }
                    }
                }

                stat = AJAXTools.OK;

                response = @"
                    <reservistId>" + reservistId.ToString() + @"</reservistId>
                    <personId>" + personId.ToString() + @"</personId>
                    <isValidIdentNumber>" + isValidIdentNumber.ToString() + @"</isValidIdentNumber>
                    <noAccess>" + noAccess.ToString() + @"</noAccess>
                    <isMilitaryReportingPerson>" + isMilitaryReportingPerson.ToString() + @"</isMilitaryReportingPerson>
                    ";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Get the Municipalities for a particular Region (ajax call)
        private void JSRepopulateMunicipality()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int regionId = 0;

                if (!String.IsNullOrEmpty(Request.Form["RegionId"]))
                    regionId = int.Parse(Request.Form["RegionId"]);

                response = "<municipalities>";

                if (regionId == 0 || regionId == int.Parse(ListItems.GetOptionChooseOne().Value))
                    response += "<m>" +
                                 "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                 "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                 "</m>";

                List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);

                foreach (Municipality municipality in municipalities)
                {
                    response += "<m>" +
                                "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                "</m>";
                }

                response += "</municipalities>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate the Cities when changing the Municipality (ajax call)
        private void JSRepopulateCity()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int municipalityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["MunicipalityId"]))
                    municipalityId = int.Parse(Request.Form["MunicipalityId"]);

                response = "<cities>";

                if (municipalityId == 0 || municipalityId == int.Parse(ListItems.GetOptionChooseOne().Value))
                    response += "<c>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</c>";

                List<City> cities = CityUtil.GetCities(municipalityId, CurrentUser);

                foreach (City city in cities)
                {
                    response += "<c>" +
                                "<id>" + city.CityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                "</c>";
                }

                response += "</cities>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate the PostCode when changing the City (ajax call)
        private void JSRepopulatePostCode()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int cityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                    cityId = int.Parse(Request.Form["CityId"]);

                string postCode = "";

                if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    City city = CityUtil.GetCity(cityId, CurrentUser);
                    postCode = city.PostCode.ToString();
                }

                stat = AJAXTools.OK;
                response = "<postCode>" + AJAXTools.EncodeForXML(postCode) + "</postCode>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate the Region, the Municipalityand the City when changing the PostCode (ajax call)
        private void JSRepopulateRegionMunicipalityCity()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int postCode = 0;

                if (!String.IsNullOrEmpty(Request.Form["PostCode"]))
                {
                    try
                    {
                        postCode = int.Parse(Request.Form["PostCode"]);
                    }
                    catch
                    {
                        postCode = 0;
                    }
                }

                //District district = DistrictUtil.GetDistrictByPostCode(postCode.ToString(), CurrentUser);
                City foundCity = null;                
                foundCity = CityUtil.GetCityByPostCode(postCode, CurrentUser);

                if (postCode > 0 && foundCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(foundCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(foundCity.MunicipalityId, CurrentUser);

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c>";
                    }

                    response += "<cityId>" + foundCity.CityId.ToString() + @"</cityId>" +
                                "<municipalityId>" + foundCity.MunicipalityId.ToString() + @"</municipalityId>" +
                                "<regionId>" + foundCity.RegionId.ToString() + @"</regionId>";
                }
                else
                {
                    response = "<cityId>0</cityId>";
                }

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate the PostCode and District when changing the City (ajax call)
        private void JSRepopulatePostCodeAndDistrict()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int cityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                    cityId = int.Parse(Request.Form["CityId"]);

                string cityPostCode = "";
                string districts = "<districts>" +
                                   "<d>" +
                                   "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                   "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                   "</d>";

                if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    City city = CityUtil.GetCity(cityId, CurrentUser);
                    cityPostCode = city.PostCode.ToString();

                    foreach (District district in city.Districts)
                    {
                        districts += "<d>" +
                                     "<id>" + district.DistrictId.ToString() + "</id>" +
                                     "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                     "</d>";
                    }
                }

                districts += "</districts>";

                stat = AJAXTools.OK;
                response = "<cityPostCode>" + AJAXTools.EncodeForXML(cityPostCode) + "</cityPostCode>" +
                           districts;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate the PostCode when changing the District (ajax call)
        private void JSRepopulateDistrictPostCode()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int districtId = 0;

                if (!String.IsNullOrEmpty(Request.Form["DistrictId"]))
                    districtId = int.Parse(Request.Form["DistrictId"]);

                string postCode = "";

                if (districtId != 0 && districtId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    District district = DistrictUtil.GetDistrict(districtId, CurrentUser);
                    postCode = district.PostCode;

                    if (postCode == "")
                    {
                        int cityId = 0;
                        if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                            cityId = int.Parse(Request.Form["CityId"]);

                        if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                        {
                            City city = CityUtil.GetCity(cityId, CurrentUser);
                            postCode = city.PostCode.ToString();
                        }
                    }
                }

                stat = AJAXTools.OK;
                response = "<districtPostCode>" + AJAXTools.EncodeForXML(postCode) + "</districtPostCode>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }
        
        //Populate the Region, the Municipalityand, the City and the District when changing the PostCode (ajax call)
        private void JSRepopulateRegionMunicipalityCityDistrict()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int postCode = 0;

                if (!String.IsNullOrEmpty(Request.Form["PostCode"]))
                {
                    try
                    {
                        postCode = int.Parse(Request.Form["PostCode"]);
                    }
                    catch
                    {
                        postCode = 0;
                    }
                }

                City foundCity = null;

                foundCity = CityUtil.GetCityByPostCode(postCode, CurrentUser);

                if (postCode > 0 && foundCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(foundCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(foundCity.MunicipalityId, CurrentUser);
                    List<District> districts = foundCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c>";
                    }

                    response += "<d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</d>";

                    foreach (District district in districts)
                    {
                        response += "<d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</d>";
                    }

                    response += "<districtId></districtId>" +
                                "<cityId>" + foundCity.CityId.ToString() + @"</cityId>" +
                                "<municipalityId>" + foundCity.MunicipalityId.ToString() + @"</municipalityId>" +
                                "<regionId>" + foundCity.RegionId.ToString() + @"</regionId>";
                }
                else
                {
                    response = "<cityId>0</cityId>";
                }

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Refresh the list SizeClothing (ajax call)
        private void JSRefreshSizeClothingList()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<sizes>";

                response += "<s>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</s>";

                List<GTableItem> sizeClothing = GTableItemUtil.GetAllGTableItemsByTableName("SizeClothing", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem size in sizeClothing)
                {
                    response += "<s>" +
                                "<id>" + size.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(size.TableValue.ToString()) + "</name>" +
                                "</s>";
                }

                response += "</sizes>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Refresh the list SizeHat (ajax call)
        private void JSRefreshSizeHatList()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<sizes>";

                response += "<s>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</s>";

                List<GTableItem> sizeHat = GTableItemUtil.GetAllGTableItemsByTableName("SizeHat", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem size in sizeHat)
                {
                    response += "<s>" +
                                "<id>" + size.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(size.TableValue.ToString()) + "</name>" +
                                "</s>";
                }

                response += "</sizes>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Refresh the list SizeShoes (ajax call)
        private void JSRefreshSizeShoesList()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<sizes>";

                response += "<s>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</s>";

                List<GTableItem> sizeShoes = GTableItemUtil.GetAllGTableItemsByTableName("SizeShoes", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem size in sizeShoes)
                {
                    response += "<s>" +
                                "<id>" + size.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(size.TableValue.ToString()) + "</name>" +
                                "</s>";
                }

                response += "</sizes>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load RecordOfServiceArchives table and light-box (ajax call)
        private void JSLoadRecordOfServiceArchives()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetRecordOfServiceArchivesTable(reservistId);
                string lightBoxHTML = GetRecordOfServiceArchivesLightBox();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Render the RecordOfServiceArchives table
        public string GetRecordOfServiceArchivesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            List<PersonRecordOfServiceArchive> recordOfServiceArchive = PersonRecordOfServiceArchiveUtil.GetPersonRecordOfServiceArchiveByPersonID(reservist.PersonId, CurrentUser);

            if (recordOfServiceArchive.Count == 0)
            {
                tableHTML = "";
            }
            else
            {
                html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
                                   <th style='width: 100px; vertical-align: bottom;'>Серия</th>
                                   <th style='width: 200px; vertical-align: bottom;'>Номер</th>                   
                                   <th style='width: 100px; vertical-align: bottom;'>Дата на издаване</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Дубликат</th>
                                   <th style='width: 260px; vertical-align: bottom;'>Забележка</th>
                                   <th style='width: 50px;vertical-align: top;'></th>
                                </tr>
                              </thead>");

                int counter = 0;

                recordOfServiceArchive = recordOfServiceArchive.OrderBy(x => x.CreatedDate).ToList();
                foreach (PersonRecordOfServiceArchive recordOfService in recordOfServiceArchive)
                {
                    counter++;

                    string deleteHTML = "";

                    if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_RECORDOFSERVICEARCHIVE") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteRecordOfService(" + recordOfService.RecordOfServiceId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_RECORDOFSERVICEARCHIVE") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditRecordOfService(" + recordOfService.RecordOfServiceId.ToString() + ");' />";
                    }

                    html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
                                    <td style='text-align: left;'>" + (recordOfService.RecordOfServiceSeries != null ? recordOfService.RecordOfServiceSeries : "") + @"</td>
                                    <td style='text-align: left;'>" + (recordOfService.RecordOfServiceNumber != null ? recordOfService.RecordOfServiceNumber : "") + @"</td>    
                                    <td style='text-align: left;'>" + (recordOfService.RecordOfServiceDate != null ? CommonFunctions.FormatDate(recordOfService.RecordOfServiceDate) : "") + @"</td>                              
                                    <td style='text-align: left;'>" + (recordOfService.RecordOfServiceCopy ? "Да" : "Не") + @"</td>
                                    <td style='text-align: left;'>" + (recordOfService.RecordOfServiceComment != null ? CommonFunctions.HtmlEncoding(recordOfService.RecordOfServiceComment) : "") + @"</td>
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                                ");
                }

                html.Append("</table>");

                tableHTML = html.ToString();
            }

            return tableHTML;
        }

        //Render the RecordOfServiceArchives light-box
        public string GetRecordOfServiceArchivesLightBox()
        {
            string html = @"
<center>
    <input type=""hidden"" id=""hdnRecordOfServiceArchiveID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 50%"">
        </colgroup>
        <colgroup style=""width: 50%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditRecordOfServiceArchiveTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblRecordOfServiceArchiveSeries"" class=""InputLabel"">Серия:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtRecordOfServiceArchiveSeries"" class=""InputField"" maxlength=""20"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblRecordOfServiceArchiveNumber"" class=""InputLabel"">№</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtRecordOfServiceArchiveNumber"" class=""InputField"" maxlength=""100"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblRecordOfServiceArchiveDate"" class=""InputLabel"">Дата на издаване:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanRecordOfServiceArchiveDate"" >
                    <input type=""text"" id=""txtRecordOfServiceArchiveDate"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 70px;"" maxlength=""10"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblRecordOfServiceArchiveCopy"" class=""InputLabel"">Дубликат</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""checkbox"" id=""chkRecordOfServiceArchiveCopy"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; vertical-align: top;"">
                <span id=""lblRecordOfServiceArchiveComment"" class=""InputLabel"">Забележка:</span>
            </td>
            <td style=""text-align: left;"">
                <textarea id=""txtRecordOfServiceArchiveComment"" rows=""3"" cols=""3"" class=""InputField"" style=""width: 250px;""></textarea>
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditRecordOfServiceLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditRecordOfServiceLightBox"" style=""display: inline;"" onclick=""SaveAddEditRecordOfServiceLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditRecordOfServiceLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditRecordOfServiceLightBox"" style=""display: inline;"" onclick=""HideAddEditRecordOfServiceLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditRecordOfServiceLightBoxText"" style=""width: 69px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>";

            return html;
        }

        //Save a particular RecordOfService (ajax call)
        private void JSSaveRecordOfService()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int recordOfServiceId = int.Parse(Request.Form["RecordOfServiceId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                string recordOfServiceSeries = Request.Form["RecordOfServiceSeries"];
                string recordOfServiceNumber = Request.Form["RecordOfServiceNumber"];
                string recordOfServiceDate = Request.Form["RecordOfServiceDate"];
                string recordOfServiceCopy = Request.Form["RecordOfServiceCopy"];
                string recordOfServiceComment = Request.Form["RecordOfServiceComment"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonRecordOfServiceArchive personRecordOfServiceArchive = new PersonRecordOfServiceArchive(CurrentUser);

                personRecordOfServiceArchive.RecordOfServiceId = recordOfServiceId;
                personRecordOfServiceArchive.PersonId = reservist.PersonId;
                personRecordOfServiceArchive.RecordOfServiceSeries = recordOfServiceSeries;
                personRecordOfServiceArchive.RecordOfServiceNumber = recordOfServiceNumber;
                personRecordOfServiceArchive.RecordOfServiceDate = (!String.IsNullOrEmpty(recordOfServiceDate) ? CommonFunctions.ParseDate(recordOfServiceDate) : (DateTime?)null);
                personRecordOfServiceArchive.RecordOfServiceCopy = (recordOfServiceCopy == "1");
                personRecordOfServiceArchive.RecordOfServiceComment = recordOfServiceComment;

                if (PersonRecordOfServiceArchiveUtil.SavePersonRecordOfServiceArchive(personRecordOfServiceArchive, reservist.Person, CurrentUser, change))
                {
                    PersonUtil.RemovePersonRecordOfService(reservist.PersonId, CurrentUser);
                }

                change.WriteLog();

                string refreshedRecordOfServiceTable = GetRecordOfServiceArchivesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedRecordOfServiceTable>" + AJAXTools.EncodeForXML(refreshedRecordOfServiceTable) + @"</refreshedRecordOfServiceTable>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load a particular RecordOfService (ajax call)
        private void JSLoadRecordOfService()
        {
            string stat = "";
            string response = "";

            try
            {
                int recordOfServiceId = int.Parse(Request.Params["RecordOfServiceId"]);

                PersonRecordOfServiceArchive personRecordOfServiceArchive = PersonRecordOfServiceArchiveUtil.GetPersonRecordOfServiceArchive(recordOfServiceId, CurrentUser);

                stat = AJAXTools.OK;
                response = @"<personRecordOfServiceArchive>
                                <recordOfServiceSeries>" + AJAXTools.EncodeForXML(personRecordOfServiceArchive.RecordOfServiceSeries) + @"</recordOfServiceSeries>
                                <recordOfServiceNumber>" + AJAXTools.EncodeForXML(personRecordOfServiceArchive.RecordOfServiceNumber) + @"</recordOfServiceNumber>
                                <recordOfServiceDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personRecordOfServiceArchive.RecordOfServiceDate)) + @"</recordOfServiceDate>
                                <recordOfServiceCopy>" + AJAXTools.EncodeForXML(personRecordOfServiceArchive.RecordOfServiceCopy ? "1" : "0") + @"</recordOfServiceCopy>
                                <recordOfServiceComment>" + AJAXTools.EncodeForXML(personRecordOfServiceArchive.RecordOfServiceComment) + @"</recordOfServiceComment>
                             </personRecordOfServiceArchive>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete a particular RecordOfService (ajax call)
        private void JSDeleteRecordOfService()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int recordOfServiceId = int.Parse(Request.Params["RecordOfServiceId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonRecordOfServiceArchiveUtil.DeletePersonRecordOfServiceArchive(recordOfServiceId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedRecordOfServiceTable = GetRecordOfServiceArchivesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedRecordOfServiceTable>" + AJAXTools.EncodeForXML(refreshedRecordOfServiceTable) + @"</refreshedRecordOfServiceTable>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load Convictions table and light-box (ajax call)
        private void JSLoadConvictions()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetConvictionsTable(reservistId);
                string lightBoxHTML = GetConvictionLightBox();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Render the Convictions table
        public string GetConvictionsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsConvictionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_CONVICTION") == UIAccessLevel.Hidden;
            bool IsConvictionReasonHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_CONVICTIONREASON") == UIAccessLevel.Hidden;
            bool IsDateFromHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_DATEFROM") == UIAccessLevel.Hidden;
            bool IsDateToHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_DATETO") == UIAccessLevel.Hidden;

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewConviction();' />";
            }

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
      " + (!IsConvictionHidden ? @"<th style='width: 200px; vertical-align: bottom;'>Съдимост</th>" : "") + @"
" + (!IsConvictionReasonHidden ? @"<th style='width: 260px; vertical-align: bottom;'>Причина/наказание за съдимост</th>" : "") + @"                    
        " + (!IsDateFromHidden ? @"<th style='width: 80px; vertical-align: bottom;'>От дата</th>" : "") + @"
          " + (!IsDateToHidden ? @"<th style='width: 80px; vertical-align: bottom;'>До дата</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonConviction> convictions = PersonConvictionUtil.GetAllPersonConvictionsByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonConviction conviction in convictions)
            {
                counter++;

                string deleteHTML = "";

                if (conviction.CanDelete)
                {
                    if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteConviction(" + conviction.ConvictionId.ToString() + ");' />";

                    }
                }

                string editHTML = "";

                if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Enabled && !isPreview
              )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditConviction(" + conviction.ConvictionId.ToString() + ");' />";

                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
       " + (!IsConvictionHidden ? @"<td style='text-align: left;'>" + (conviction.Conviction != null ? conviction.Conviction.ConvictionName : "") + @"</td>" : "") + @"
 " + (!IsConvictionReasonHidden ? @"<td style='text-align: left;'>" + (conviction.ConvictionReason != null ? conviction.ConvictionReason.ConvictionReasonName.ToString() : "") + @"</td>" : "") + @"                    
         " + (!IsDateFromHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(conviction.DateFrom) + @"</td>" : "") + @"
           " + (!IsDateToHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(conviction.DateTo) + @"</td>" : "") + @"
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Conviction light-box
        public string GetConvictionLightBox()
        {
            List<Conviction> listConviction = ConvictionUtil.GetAllConvictions(CurrentUser);
            List<IDropDownItem> ddiConviction = new List<IDropDownItem>();

            foreach (Conviction conviction in listConviction)
            {
                ddiConviction.Add(conviction);
            }

            // Generates html for drop down list
            string convictionHTML = ListItems.GetDropDownHtml(ddiConviction, null, "ddConviction", true, null, "", "style='width: 260px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            List<ConvictionReason> listConvictionReason = ConvictionReasonUtil.GetAllConvictionReasons(CurrentUser);
            List<IDropDownItem> ddiConvictionReason = new List<IDropDownItem>();

            foreach (ConvictionReason convictionReason in listConvictionReason)
            {
                ddiConvictionReason.Add(convictionReason);
            }

            // Generates html for drop down list
            string convictionReasonHTML = ListItems.GetDropDownHtml(ddiConvictionReason, null, "ddConvictionReason", true, null, "", "style='width: 260px;' UnsavedCheckSkipMe='true' ", true);


            string html = @"
<center>
    <input type=""hidden"" id=""hdnConvictionID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 50%"">
        </colgroup>
        <colgroup style=""width: 50%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditConvictionTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblConviction"" class=""InputLabel"">Съдимост:</span>
            </td>
            <td style=""text-align: left;"">
                " + convictionHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblConvictionReason"" class=""InputLabel"">Причина/наказание за съдимост:</span>
            </td>
            <td style=""text-align: left;"">
                " + convictionReasonHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblConvDateFrom"" class=""InputLabel"">От дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""contConvDateFrom"">
                   <input type=""text"" id=""txtConvDateFrom"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" UnsavedCheckSkipMe=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblConvDateTo"" class=""InputLabel"">До дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""contConvDateTo"">
                   <input type=""text"" id=""txtConvDateTo"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" UnsavedCheckSkipMe=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditConvictionLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditConvictionLightBox"" style=""display: inline;"" onclick=""SaveAddEditConvictionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditConvictionLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditConvictionLightBox"" style=""display: inline;"" onclick=""HideAddEditConvictionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditConvictionLightBoxText"" style=""width: 70px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>";

            return html;
        }

        //Save a particular conviction (ajax call)
        private void JSSaveConviction()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int convictionId = int.Parse(Request.Form["ConvictionId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                string convictionCode = Request.Form["ConvictionCode"];
                string convictionReasonCode = Request.Form["ConvictionReasonCode"];
                DateTime dateFrom = CommonFunctions.ParseDate(Request.Form["DateFrom"]).Value;
                DateTime? dateTo = (!String.IsNullOrEmpty(Request.Form["DateTo"]) ? CommonFunctions.ParseDate(Request.Params["DateTo"]) : (DateTime?)null);


                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonConviction existingPersonConviction = PersonConvictionUtil.GetPersonConviction(reservist.Person.IdentNumber, dateFrom, CurrentUser);

                if (existingPersonConviction != null &&
                    existingPersonConviction.ConvictionId != convictionId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Вече е въведена съдимост за избраната дата</status>";
                }
                else
                {
                    PersonConviction personConviction = new PersonConviction(CurrentUser);

                    personConviction.ConvictionId = convictionId;
                    personConviction.ConvictionCode = convictionCode;
                    personConviction.ConvictionReasonCode = convictionReasonCode;
                    personConviction.DateFrom = dateFrom;
                    personConviction.DateTo = dateTo;

                    PersonConvictionUtil.SavePersonConviction(personConviction, reservist.Person, CurrentUser, change);

                    if (personConviction.ConvictionCode == "3" && personConviction.DateFrom <= DateTime.Now && (!personConviction.DateTo.HasValue || personConviction.DateTo.Value > DateTime.Now))
                    {
                        int? duration = null;

                        if (personConviction.DateTo.HasValue)
                        {
                            duration = Math.Abs(12 * (personConviction.DateFrom.Year - personConviction.DateTo.Value.Year) + personConviction.DateFrom.Month - personConviction.DateTo.Value.Month);
                        }

                        ReservistMilRepStatus status = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservist.ReservistId, CurrentUser);

                        if (status == null || status.MilitaryReportStatus == null || status.MilitaryReportStatus.MilitaryReportStatusKey != "TEMPORARY_REMOVED")
                        {
                            ReservistMilRepStatusUtil.SetMilRepStatusTo_TEMPORARY_REMOVED(reservist.ReservistId, 2, personConviction.DateFrom, duration, CurrentUser, change);

                            List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservist.ReservistId, CurrentUser);

                            //Remove the Reservist from each Equipment Request
                            foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                            {
                                FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);
                            }

                            //Clear the Mobilization Appointment
                            ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(reservist.ReservistId, CurrentUser, change);
                        }
                    }

                    change.WriteLog();

                    string refreshedConvictionTable = GetConvictionsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedConvictionTable>" + AJAXTools.EncodeForXML(refreshedConvictionTable) + @"</refreshedConvictionTable>";
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load a particular Civil Education (ajax call)
        private void JSLoadConviction()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int convictionId = int.Parse(Request.Params["ConvictionId"]);

                PersonConviction conviction = PersonConvictionUtil.GetPersonConviction(convictionId, CurrentUser);

                stat = AJAXTools.OK;
                response = @"<personConviction>
                                <convictionCode>" + AJAXTools.EncodeForXML(conviction.ConvictionCode) + @"</convictionCode>
                                <convictionReasonCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(conviction.ConvictionReasonCode) ? conviction.ConvictionReasonCode : ListItems.GetOptionChooseOne().Value) + @"</convictionReasonCode>
                                <dateFrom>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(conviction.DateFrom)) + @"</dateFrom>
                                <dateTo>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(conviction.DateTo)) + @"</dateTo>
                             </personConviction>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete a particular conviction (ajax call)
        private void JSDeleteConviction()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int convictionId = int.Parse(Request.Params["ConvictionId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonConvictionUtil.DeletePersonConviction(convictionId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedConvictionTable = GetConvictionsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedConvictionTable>" + AJAXTools.EncodeForXML(refreshedConvictionTable) + @"</refreshedConvictionTable>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }


        //Load DualCitizenship table and light-box (ajax call)
        private void JSLoadDualCitizenships()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetDualCitizenshipTable(reservistId);
                string lightBoxHTML = GetDualCitizenshipLightBox();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Render the DualCitizenship table
        public string GetDualCitizenshipTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsCountryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP_COUNTRY") == UIAccessLevel.Hidden;

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
            GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewDualCitizenship();' />";
            }

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
         " + (!IsCountryHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Държава</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonDualCitizenship> dualCitizenships = PersonDualCitizenshipUtil.GetAllPersonDualCitizenshipByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonDualCitizenship dualCitizenship in dualCitizenships)
            {
                counter++;

                string deleteHTML = "";

                if (dualCitizenship.CanDelete)
                {
                    if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteDualCitizenship(" + dualCitizenship.DualCitizenshipId.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Enabled &&
              GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Enabled && !isPreview
              )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditDualCitizenship(" + dualCitizenship.DualCitizenshipId.ToString() + ");' />";

                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
          " + (!IsCountryHidden ? @"<td style='text-align: left;'>" + (dualCitizenship.Country != null ? dualCitizenship.Country.CountryName : "") + @"</td>" : "") + @"
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the DualCitizenship light-box
        public string GetDualCitizenshipLightBox()
        {
            List<Country> listCountry = CountryUtil.GetCountries(CurrentUser);
            List<IDropDownItem> ddiCountry = new List<IDropDownItem>();

            foreach (Country country in listCountry)
            {
                ddiCountry.Add(country);
            }

            // Generates html for drop down list
            string dualCitizenshipCountryHTML = ListItems.GetDropDownHtml(ddiCountry, null, "ddDualCitizenshipCountry", true, null, "", "style='width: 180px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            string html = @"
<center>
    <input type=""hidden"" id=""hdnDualCitizenshipID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditDualCitizenshipTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblDualCitizenshipCountry"" class=""InputLabel"">Държава:</span>
            </td>
            <td style=""text-align: left;"">
                " + dualCitizenshipCountryHTML + @"
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditDualCitizenshipLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditDualCitizenshipLightBox"" style=""display: inline;"" onclick=""SaveAddEditDualCitizenshipLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditDualCitizenshipLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditDualCitizenshipLightBox"" style=""display: inline;"" onclick=""HideAddEditDualCitizenshipLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditConvictionLightBoxText"" style=""width: 70px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>";

            return html;
        }

        //Save a particular DualCitizenship (ajax call)
        private void JSSaveDualCitizenship()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int dualCitizenshipId = int.Parse(Request.Form["DualCitizenshipId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                string countryId = Request.Form["CountryId"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonDualCitizenship existingPersonDualCitizenship = PersonDualCitizenshipUtil.GetPersonDualCitizenship(reservist.Person.IdentNumber, countryId, CurrentUser);

                if (existingPersonDualCitizenship != null &&
                    existingPersonDualCitizenship.DualCitizenshipId != dualCitizenshipId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Вече е въведено двойно гражданство за избраната държава</status>";
                }
                else
                {
                    PersonDualCitizenship personDualCitizenship = new PersonDualCitizenship(CurrentUser);

                    personDualCitizenship.DualCitizenshipId = dualCitizenshipId;
                    personDualCitizenship.CountryId = countryId;

                    PersonDualCitizenshipUtil.SavePersonDualCitizenship(personDualCitizenship, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedPersonDualCitizenshipTable = GetDualCitizenshipTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedPersonDualCitizenshipTable>" + AJAXTools.EncodeForXML(refreshedPersonDualCitizenshipTable) + @"</refreshedPersonDualCitizenshipTable>";
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load a particular DualCitizenship (ajax call)
        private void JSLoadDualCitizenship()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int dualCitizenshipId = int.Parse(Request.Params["DualCitizenshipId"]);

                PersonDualCitizenship dualCitizenship = PersonDualCitizenshipUtil.GetPersonDualCitizenship(dualCitizenshipId, CurrentUser);

                stat = AJAXTools.OK;
                response = @"<personDualCitizenship>
                                <countryId>" + AJAXTools.EncodeForXML(dualCitizenship.CountryId) + @"</countryId>
                             </personDualCitizenship>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete a particular DualCitizenship (ajax call)
        private void JSDeleteDualCitizenship()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int dualCitizenshipId = int.Parse(Request.Params["DualCitizenshipId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonDualCitizenshipUtil.DeletePersonDualCitizenship(dualCitizenshipId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedDualCitizenshipTable = GetDualCitizenshipTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedDualCitizenshipTable>" + AJAXTools.EncodeForXML(refreshedDualCitizenshipTable) + @"</refreshedDualCitizenshipTable>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }
    }


    public static class AddEditReservist_PersonalData_PageUtil
    {
        public static string GetTabContent(string moduleKey, User currentUser)
        {
            List<IDropDownItem> ddiGenders = new List<IDropDownItem>();
            List<Gender> genders = GenderUtil.GetGenders(currentUser);

            foreach (Gender gender in genders)
            {
                ddiGenders.Add(gender);
            }

            string gendersHTML = ListItems.GetDropDownHtml(ddiGenders, null, "ddGender", true, null, "", "style='width: 70px;'");

            List<IDropDownItem> ddiBirthCountries = new List<IDropDownItem>();
            List<Country> birthCountries = CountryUtil.GetCountries(currentUser);

            foreach (Country country in birthCountries)
            {
                ddiBirthCountries.Add(country);
            }

            Country bulgaria = CountryUtil.GetCountryBulgaria(currentUser);

            string birthCountriesHTML = ListItems.GetDropDownHtml(ddiBirthCountries, null, "ddBirthCountry", true, bulgaria, "ddBirthCountry_Changed();", "style='width: 200px;'");


            List<IDropDownItem> ddiRegions = new List<IDropDownItem>();
            List<Region> regions = RegionUtil.GetRegions(currentUser);

            foreach (Region region in regions)
            {
                ddiRegions.Add(region);
            }

            string birthRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddBirthRegion", true, null, "ddBirthRegion_Changed();", "style='width: 120px;'");
            string birthMunicipalitiesHTML = ListItems.GetDropDownHtml(null, null, "ddBirthMunicipality", true, null, "ddBirthMunicipality_Changed();", "style='width: 170px;'");
            string birthCityHTML = ListItems.GetDropDownHtml(null, null, "ddBirthCity", true, null, "ddBirthCity_Changed();", "style='width: 170px;'");


            string drvLicCategoriesJSON = "";

            List<DrivingLicenseCategory> categories = DrivingLicenseCategoryUtil.GetAllDrivingLicenseCategories(currentUser);

            foreach (DrivingLicenseCategory category in categories)
            {
                string pickListItem = "{value : '" + category.DrivingLicenseCategoryId.ToString() + "' , label : '" + category.DrivingLicenseCategoryName.Replace("'", "\\'") + "'}";
                drvLicCategoriesJSON += (drvLicCategoriesJSON == "" ? "" : ",") + pickListItem;
            }

            if (drvLicCategoriesJSON != "")
                drvLicCategoriesJSON = "[" + drvLicCategoriesJSON + "]";


            string permRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddPermRegion", true, null, "ddPermRegion_Changed();", "style='width: 170px;'");
            string permMunicipalitiesHTML = ListItems.GetDropDownHtml(null, null, "ddPermMunicipality", true, null, "ddPermMunicipality_Changed();", "style='width: 198px;'");
            string permCityHTML = ListItems.GetDropDownHtml(null, null, "ddPermCity", true, null, "ddPermCity_Changed();", "style='width: 170px;' class='RequiredInputField'");
            string permDistrictHTML = ListItems.GetDropDownHtml(null, null, "ddPermDistrict", true, null, "ddPermDistrict_Changed();", "style='width: 170px;'");

            string currRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddCurrRegion", true, null, "ddCurrRegion_Changed();", "style='width: 170px;'");
            string currMunicipalitiesHTML = ListItems.GetDropDownHtml(null, null, "ddCurrMunicipality", true, null, "ddCurrMunicipality_Changed();", "style='width: 198px;'");
            string currCityHTML = ListItems.GetDropDownHtml(null, null, "ddCurrCity", true, null, "ddCurrCity_Changed();", "style='width: 170px;' class='RequiredInputField'");
            string currDistrictHTML = ListItems.GetDropDownHtml(null, null, "ddCurrDistrict", true, null, "ddCurrDistrict_Changed();", "style='width: 170px;'");

            List<MaritalStatus> maritalStatuses = MaritalStatusUtil.GetMaritalStatuses(currentUser);
            List<IDropDownItem> ddiMaritalStatuses = new List<IDropDownItem>();

            foreach (MaritalStatus maritalStatus in maritalStatuses)
            {
                ddiMaritalStatuses.Add(maritalStatus);
            }

            string maritalStatusHTML = ListItems.GetDropDownHtml(ddiMaritalStatuses, null, "ddMaritalStatus", true, null, "", "style='width: 160px;' class='RequiredInputField'");

            List<GTableItem> sizeClothing = GTableItemUtil.GetAllGTableItemsByTableName("SizeClothing", moduleKey, 1, 0, 0, currentUser);
            List<IDropDownItem> ddiSizeClothing = new List<IDropDownItem>();
            foreach (GTableItem sizeClothingItem in sizeClothing)
            {
                ddiSizeClothing.Add(sizeClothingItem);
            }

            string sizeClothingHTML = ListItems.GetDropDownHtml(ddiSizeClothing, null, "ddSizeClothing", true, null, "", "style='width: 80px;'");
            string editSizeClothingHTML = @"<img id=""imgMaintSizeClothing"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('SizeClothing', 1, 1, RefreshSizeClothingList);"" />";

            List<GTableItem> sizeHat = GTableItemUtil.GetAllGTableItemsByTableName("SizeHat", moduleKey, 1, 0, 0, currentUser);
            List<IDropDownItem> ddiSizeHat = new List<IDropDownItem>();
            foreach (GTableItem sizeHatItem in sizeHat)
            {
                ddiSizeHat.Add(sizeHatItem);
            }

            string sizeHatHTML = ListItems.GetDropDownHtml(ddiSizeHat, null, "ddSizeHat", true, null, "", "style='width: 60px;'");
            string editSizeHatHTML = @"<img id=""imgMaintSizeHat"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('SizeHat', 1, 1, RefreshSizeHatList);"" />";

            List<GTableItem> sizeShoes = GTableItemUtil.GetAllGTableItemsByTableName("SizeShoes", moduleKey, 1, 0, 0, currentUser);
            List<IDropDownItem> ddiSizeShoes = new List<IDropDownItem>();
            foreach (GTableItem sizeShoesItem in sizeShoes)
            {
                ddiSizeShoes.Add(sizeShoesItem);
            }

            string sizeShoesHTML = ListItems.GetDropDownHtml(ddiSizeShoes, null, "ddSizeShoes", true, null, "", "style='width: 60px;'");
            string editSizeShoesHTML = @"<img id=""imgMaintSizeShoes"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('SizeShoes', 1, 1, RefreshSizeShoesList);"" />";

            List<IDropDownItem> ddiAbroadCountries = new List<IDropDownItem>();
            List<Country> abroadCountries = CountryUtil.GetCountries(currentUser);

            foreach (Country country in abroadCountries)
            {
                if (country.CountryId != bulgaria.CountryId)
                    ddiAbroadCountries.Add(country);
            }

            string abroadCountriesHTML = ListItems.GetDropDownHtml(ddiAbroadCountries, null, "ddAbroadCountry", true, null, "", "style='width: 200px;'");


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblIDCardNumber"" class=""InputLabel"">Лична карта номер:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtIDCardNumber"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right;"" nowrap>
            <span id=""lblIDCardIssuedBy"" class=""InputLabel"">издадена от:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtIDCardIssuedBy"" class=""InputField"" style=""width: 100px;"" maxlength=""100"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblIDCardIssueDate"" class=""InputLabel"">на:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""spanIDCardIssueDate"" >
                <input type=""text"" id=""txtIDCardIssueDate"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 70px;"" maxlength=""10"" />
            </span>
         </td>
         <td style=""text-align: right;"">
            <span id=""lblGender"" class=""InputLabel"">Пол:</span>
         </td>
         <td style=""text-align: left;"">
            " + gendersHTML + @"
         </td>
      </tr>
      <tr>
         <td style=""text-align: right; vertical-align: top;"">
            <span id=""lblBirthCountry"" class=""InputLabel"">Месторождение:</span>
         </td>
         <td style=""text-align: left; vertical-align: top;"" colspan=""2"">
            " + birthCountriesHTML + @"
         </td>
         <td colspan=""5"" style=""text-align: left; vertical-align: top;"" >
            <div style=""height: 40px; width: 450px;"">
                <div id=""birthCityIfAbroad"" style=""display: none;"">
                   <span id=""lblBirthCityIfAbroad"" class=""InputLabel"">Населено място:</span>
                   <input type=""text"" id=""txtBirthCityIfAbroad"" class=""InputField"" style=""width: 165px;"" maxlength=""200"" />
                </div>
                <div id=""birthCityIfNotAbroad"">
                   <table cellspacing=""0"" cellpadding=""0"">
                      <tr>
                         <td style=""text-align: right;"">
                            <span id=""lblBirthRegion"" class=""InputLabel"">Област:</span>
                         </td>
                         <td style=""text-align: left; padding-left: 3px;"">
                            " + birthRegionsHTML + @"
                            <span id=""lblBirthMunicipality"" class=""InputLabel"" style=""padding-left: 9px;"">Община:</span>
                            " + birthMunicipalitiesHTML + @"
                         </td>
                      </tr>
                      <tr style=""height: 3px;""></tr>
                      <tr>
                         <td style=""text-align: right;"">
                            <span id=""lblBirthPostCode"" class=""InputLabel"">П. код:</span>
                         </td>
                         <td style=""text-align: left; padding-left: 3px;;"">
                            <input type=""text"" id=""txtBirthPostCode"" class=""InputField"" style=""width: 70px;"" 
                               onfocus=""txtBirthPostCode_Focus();"" onblur=""txtBirthPostCode_Blur();"" />
                            <span id=""lblBirthCity"" class=""InputLabel"" style=""padding-left: 5px;"">Населено място:</span>
                            " + birthCityHTML + @"
                         </td>
                      </tr>
                   </table>
                </div>
            </div>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblDrvLicCategories"" class=""InputLabel"">Шофьорска книжка:</span>
         </td>
         <td style=""text-align: left;"" colspan=""2"">
            <div id=""divPickListDrvLicCategories"" style=""display: inline-table; margin: 0px;""></div>
         </td>
         <td style=""text-align: right;"" colspan=""3"">
            <span id=""lblWentToMilitary"" class=""InputLabel"" style=""padding-left: 10px"">Бил ли е на военна служба?:</span>
         </td>
         <td style=""text-align: left;"" colspan=""2"">
            <input type=""radio"" id=""hasMilitarySrv1"" name=""hasMilitarySrv"" value=""1"" title=""Да"" />
            <span id=""lblWentToMilitaryYes"" class=""InputLabel"">Да</span>&nbsp;
            <input type=""radio"" id=""hasMilitarySrv2"" name=""hasMilitarySrv"" value=""2"" title=""Не"" />
            <span id=""lblWentToMilitaryNo"" class=""InputLabel"">Не</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"" colspan=""6"">
            <span id=""lblMilitaryTraining"" class=""InputLabel"">Военна подготовка:</span>
         </td>
         <td style=""text-align: left;"" colspan=""2"">
            <input type=""radio"" id=""militaryTraining1"" name=""MilitaryTraining"" value=""1"" title=""Има"" />
            <span id=""lblMilitaryTraining1"" class=""InputLabel"">Има</span>&nbsp;
            <input type=""radio"" id=""militaryTraining2"" name=""MilitaryTraining"" value=""2"" title=""Няма"" />
            <span id=""lblMilitaryTraining2"" class=""InputLabel"">Няма</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"" colspan=""1"" nowrap>
            <span id=""lblRecordOfServiceSeries"" class=""InputLabel"">Военна книжка серия:</span>
         </td>
         <td style=""text-align: left;"" colspan=""7"">
            <input type=""text"" id=""txtRecordOfServiceSeries"" class=""InputField"" style=""width: 25px;"" maxlength=""20"" />
            <span id=""lblRecordOfServiceNumber"" class=""InputLabel"">№</span>
            <input type=""text"" id=""txtRecordOfServiceNumber"" class=""InputField"" style=""width: 80px;"" maxlength=""100"" />
            <span id=""lblRecordOfServiceDate"" class=""InputLabel"">Дата на издаване:</span>
            <span id=""spanRecordOfServiceDate"">
                <input type=""text"" id=""txtRecordOfServiceDate"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 70px;"" maxlength=""10"" />
            </span>
            <input type=""checkbox"" id=""chkRecordOfServiceCopy"" /> <span id=""lblRecordOfServiceCopy"" class=""InputLabel"">Дубликат</span>
            <img src=""../Images/index_new.png"" id=""imgNewRecordOfService"" alt=""Архивиране на военна книжка"" title=""Архивиране на военна книжка"" class=""btnNewTableRecordIcon"" onclick=""NewRecordOfService();"" style=""padding-left: 10px; vertical-align: middle; display: none;"" />
         </td>
      </tr>
      <tr>
         <td colspan=""8"" style=""padding-left: 2px; padding-right: 180px;"">
            <table>
               <td style=""text-align: left; vertical-align: top;"">
                   <div style=""text-align: left; display: none;"" id=""divRecordOfServiceTableTitle"">
                      <span id=""lblRecordOfServiceArchive"" class=""InputLabel"">Архив военна книжка</span>
                      <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px;"" id=""imgLoadingRecordOfServiceArchive"" title=""Зареждане"" />
                   </div>
                   <div id=""tblRecordOfServiceArchive"" style=""text-align: left; padding-top: 2px;""></div>
                   <div id=""pnlRecordOfServiceArchiveMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                      <span id=""lblMessageRecordOfServiceArchive""></span>
                   </div>
                   <div id=""lboxRecordOfServiceArchive"" style=""display: none;"" class=""lboxRecordOfServiceArchive""></div>
               </td>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Постоянен адрес</span>
            &nbsp;&nbsp;
            <img src=""../Images/copy.png"" id=""btnImgCopyAddress"" title=""Копиране от Настоящ адрес"" alt=""Копиране от Настоящ адрес"" style=""cursor: pointer;""
               onclick=""CopyPresAddressToCurr();"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table style=""margin: 0 auto;"">
                <tr>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblPermRegion"" class=""InputLabel"">Област:</span>
                    </td>
                    <td style=""text-align: left; width: 140px;"">
                         " + permRegionsHTML + @"
                    </td>
                    <td style=""text-align: right; width: 90px;"">
                        <span id=""lblPermMunicipality"" class=""InputLabel"">Община:</span>
                    </td>
                    <td style=""text-align: left; width: 150px;"">
                         " + permMunicipalitiesHTML + @"
                    </td>                  
                    <td style=""text-align: right; width: 210px;"">
                        <span id=""lblPermCity"" class=""InputLabel"">Населено място:</span>
                    </td>
                    <td style=""text-align: left; width: 180px;"">
                        " + permCityHTML + @"
                    </td>
                   
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"" rowspan=""2"">
                        <span id=""lblPermAddress"" class=""InputLabel"">Адрес:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"" rowspan=""2"">
                        <textarea id=""txtPermAddress"" cols=""3"" rows='3' class='InputField' style='width: 99%;'></textarea>
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblPermDistrict"" class=""InputLabel"">Район:</span>
                    </td>
                    <td style=""text-align: left;"">
                         " + permDistrictHTML + @"
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblPermPostCode"" class=""InputLabel"">Пощенски код:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <input id=""txtPermPostCode"" onfocus=""txtPermPostCode_Focus();"" onblur=""txtPermPostCode_Blur();""
                            type=""text"" class=""InputField"" style=""width: 50px;"" />
                    </td>                  
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Настоящ адрес</span>
            &nbsp;&nbsp;
            <img src=""../Images/copy.png"" id=""btnImgCopyAddress2"" title=""Копиране от Постоянен адрес"" alt=""Копиране от Постоянен адрес"" style=""cursor: pointer;""
               onclick=""CopyPermAddressToCurr();"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table style=""margin: 0 auto;"">
                <tr>                                     
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblCurrRegion"" class=""InputLabel"">Област:</span>
                    </td>
                    <td style=""text-align: left; width: 140px;"">
                         " + currRegionsHTML + @"
                    </td>
                    <td style=""text-align: right; width: 90px;"">
                        <span id=""lblCurrMunicipality"" class=""InputLabel"">Община:</span>
                    </td>
                    <td style=""text-align: left;  width: 150px;"">
                         " + currMunicipalitiesHTML + @"
                    </td>
                    <td style=""text-align: right; width: 210px;"">
                        <span id=""lblCurrCity"" class=""InputLabel"">Населено място:</span>
                    </td>
                    <td style=""text-align: left; width: 180px;"">
                        " + currCityHTML + @"
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"" rowspan=""2"">
                        <span id=""lblCurrAddress"" class=""InputLabel"">Адрес:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"" rowspan=""2"">
                        <textarea id=""txtCurrAddress"" cols=""3"" rows='3' class='InputField' style='width: 99%;'></textarea>
                    </td>
                     <td style=""text-align: right;"">
                        <span id=""lblCurrDistrict"" class=""InputLabel"">Район:</span>
                    </td>
                    <td style=""text-align: left;"">
                         " + currDistrictHTML + @"
                    </td>
                </tr>
                <tr>
                   <td style=""text-align: right;"">
                        <span id=""lblCurrPostCode"" class=""InputLabel"">Пощенски код:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <input id=""txtCurrPostCode"" onfocus=""txtCurrPostCode_Focus();"" onblur=""txtCurrPostCode_Blur();""
                            type=""text"" class=""InputField"" style=""width: 50px;"" />
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>


<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 130px;"">
                        <span id=""lblMobilePhone"" class=""InputLabel"">Мобилен телефон:</span>
                    </td>
                    <td style=""text-align: left; width: 180px;"">
                        <input id=""txtMobilePhone"" type=""text"" class=""InputField"" style=""width: 170px;"" maxlength=""50"" />
                    </td>
                    <td style=""text-align: right; width: 130px;"">
                        <span id=""lblHomePhone"" class=""InputLabel"">Дом. телефон:</span>
                    </td>
                    <td style=""text-align: left; width: 180px;"">
                        <input id=""txtHomePhone"" type=""text"" class=""InputField"" style=""width: 170px;"" maxlength=""10"" />
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblEmail"" class=""InputLabel"">E-mail:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <input id=""txtEmail"" type=""text"" class=""InputField"" style=""width: 170px;"" maxlength=""500"" />
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblBusinessPhone"" class=""InputLabel"">Сл. телефон:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <input id=""txtBusinessPhone"" type=""text"" class=""InputField"" style=""width: 170px;"" maxlength=""500"" />
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>


<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 150px;"">
                        <span id=""lblMaritalStatus"" class=""InputLabel"">Семейно положение:</span>
                    </td>
                    <td style=""text-align: left; width: 160px;"">
                        " + maritalStatusHTML + @"
                    </td>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblChildCount"" class=""InputLabel"">Брой деца:</span>
                    </td>
                    <td style=""text-align: left; width: 420px;"">
                        <input id=""txtChildCount"" type=""text"" class=""InputField"" style=""width: 30px;"" maxlength=""10"" />
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblParentsContact"" class=""InputLabel"">Лице за контакт:</span>
                    </td>
                    <td style=""text-align: left;"" colspan=""3"">
                        <textarea id=""txtParentsContact"" class=""InputField"" style=""width: 480px; height: 40px;""></textarea>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 160px;"">
                        <span id=""lblSizeClothing"" class=""InputLabel"">Номер на облекло:</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        " + sizeClothingHTML + editSizeClothingHTML + @"
                    </td>
                    <td style=""text-align: right; width: 100px;"">
                        <span id=""lblSizeHat"" class=""InputLabel"">Шапка:</span>
                    </td>
                    <td style=""text-align: left; width: 80px;"">
                        " + sizeHatHTML + editSizeHatHTML + @"
                    </td>
                    <td style=""text-align: right; width: 100px;"">
                        <span id=""lblSizeShoes"" class=""InputLabel"">Обувки:</span>
                    </td>
                    <td style=""text-align: left; width: 80px;"">
                        " + sizeShoesHTML + editSizeShoesHTML + @"
                    </td>
                    <td style=""text-align: right; width: 100px;"">
                        <span id=""lblPersonHeight"" class=""InputLabel"">Ръст:</span>
                    </td>
                    <td style=""text-align: left; width: 100px;"">
                        <input id=""txtPersonHeight"" type=""text"" class=""InputField"" style=""width: 30px;"" maxlength=""10"" />
                        <span id=""lblPersonHeightMeasure"" class=""InputLabel"">см.</span>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px; display: none;"" id=""pnlConvictionDualCitizenshipSpace""></div>

<fieldset style=""width: 830px; padding: 0px; display: none;"" id=""pnlConvictionDualCitizenship"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 1px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table style=""width: 100%;"">
                <tr>
                    <td style=""text-align: left; padding-left: 22px; width: 65%; vertical-align: top;"">
                        <div style=""text-align: left; display: none;"" id=""divConvictionTableTitle"">
                           <span id=""lblConvictionTable"" class=""InputLabel"">Съдимост</span>
                           <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px;"" id=""imgLoadingConviction"" title=""Зареждане"" />
                        </div>
                        <div id=""tblConviction"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                        <div id=""pnlConvictionMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                           <span id=""lblMessageConviction""></span>
                        </div>
                        <div id=""lboxConviction"" style=""display: none;"" class=""lboxConviction""></div>
                    </td>
                    <td style=""text-align: left; padding-left: 32px; width: 35%; vertical-align: top;"">
                        <div style=""text-align: left; display: none;"" id=""divDualCitizenshipTableTitle"">
                           <span id=""lblDualCitizenshipTable"" class=""InputLabel"">Двойно гражданство</span>
                           <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px;"" id=""imgLoadingDualCitizenship"" title=""Зареждане"" />
                        </div>
                        <div id=""tblDualCitizenship"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                        <div id=""pnlDualCitizenshipMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                           <span id=""lblMessageDualCitizenship""></span>
                        </div>
                        <div id=""lboxDualCitizenship"" style=""display: none;"" class=""lboxDualCitizenship""></div>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 110px; padding-right: 8px;"">
                        <input type=""checkbox"" id=""chkIsAbroad"" onclick=""chkIsAbroad_Click();"" />
                        <span id=""lblIsAbroad"" class=""InputLabel"">В чужбина</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        " + abroadCountriesHTML + @"
                    </td>
                    <td style=""text-align: right; width: 90px;"">
                        <span id=""lblAbroadSince"" class=""InputLabel"">от дата:</span>
                    </td>
                    <td style=""text-align: left; width: 100px;"">
                        <span id=""spAbroadSince"">
                            <input type=""text"" id=""txtAbroadSince"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 70px;"" maxlength=""10"" />
                        </span>
                    </td>
                    <td style=""text-align: right; width: 100px;"">
                        <span id=""lblAbroadPeriod"" class=""InputLabel"">за период:</span>
                    </td>
                    <td style=""text-align: left; width: 130px;"">
                        <input id=""txtAbroadPeriod"" type=""text"" class=""InputField"" style=""width: 30px;"" maxlength=""10"" />
                        <span id=""lblAbroadPeriodMeasure"" class=""InputLabel"">месеца</span>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>



<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnDrvLicCategoriesClientID"" value=""" + CommonFunctions.HtmlEncoding(drvLicCategoriesJSON) + @""" />
<input type=""hidden"" id=""hdnRecordOfServiceSeries"" />
<input type=""hidden"" id=""hdnRecordOfServiceNumber"" />
<input type=""hidden"" id=""hdnRecordOfServiceDate"" />
<input type=""hidden"" id=""hdnRecordOfServiceCopy"" />
<input type=""hidden"" id=""hdnIsNewRecordOfService"" />
";

            return html;
        }

        public static string GetTabUIItems(AddEditReservist page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string UIItemsXML = "";

            bool screenDisabled = false;
            bool personalDataDisabled = false;

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (page.ReservistId == 0) // add mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Disabled;

                personalDataDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                       page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Disabled ||
                                       page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA") == UIAccessLevel.Disabled;

                if (screenDisabled)
                {
                    hiddenClientControls.Add("btnSavePersonalData");
                }

                if (personalDataDisabled)
                {
                    hiddenClientControls.Add("btnSavePersonalData");
                }

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_IDCARDNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardNumber");
                    disabledClientControls.Add("txtIDCardNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_IDCARDISSUEDBY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardIssuedBy");
                    disabledClientControls.Add("txtIDCardIssuedBy");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_IDCARDISSUEDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardIssueDate");
                    disabledClientControls.Add("txtIDCardIssueDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("spanIDCardIssueDate");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_GENDER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblGender");
                    disabledClientControls.Add("ddGender");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblGender");
                    hiddenClientControls.Add("ddGender");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_BIRTHPLACE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblBirthCountry");
                    disabledClientControls.Add("ddBirthCountry");
                    disabledClientControls.Add("lblBirthCityIfAbroad");
                    disabledClientControls.Add("txtBirthCityIfAbroad");
                    disabledClientControls.Add("lblBirthPostCode");
                    disabledClientControls.Add("txtBirthPostCode");
                    disabledClientControls.Add("lblBirthCity");
                    disabledClientControls.Add("ddBirthCity");
                    disabledClientControls.Add("lblBirthRegion");
                    disabledClientControls.Add("ddBirthRegion");
                    disabledClientControls.Add("lblBirthMunicipality");
                    disabledClientControls.Add("ddBirthMunicipality");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBirthCountry");
                    hiddenClientControls.Add("ddBirthCountry");
                    hiddenClientControls.Add("lblBirthCityIfAbroad");
                    hiddenClientControls.Add("txtBirthCityIfAbroad");
                    hiddenClientControls.Add("lblBirthPostCode");
                    hiddenClientControls.Add("txtBirthPostCode");
                    hiddenClientControls.Add("lblBirthCity");
                    hiddenClientControls.Add("ddBirthCity");
                    hiddenClientControls.Add("lblBirthRegion");
                    hiddenClientControls.Add("ddBirthRegion");
                    hiddenClientControls.Add("lblBirthMunicipality");
                    hiddenClientControls.Add("ddBirthMunicipality");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_DRIVINGLICENSE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblDrvLicCategories");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    //In this case we hide div element
                    hiddenClientControls.Add("divPickListDrvLicCategories");
                    hiddenClientControls.Add("lblDrvLicCategories");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_HASMILITARYSERVICE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblWentToMilitary");
                    disabledClientControls.Add("hasMilitarySrv1");
                    disabledClientControls.Add("lblWentToMilitaryYes");
                    disabledClientControls.Add("hasMilitarySrv2");
                    disabledClientControls.Add("lblWentToMilitaryNo");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWentToMilitary");
                    hiddenClientControls.Add("hasMilitarySrv1");
                    hiddenClientControls.Add("lblWentToMilitaryYes");
                    hiddenClientControls.Add("hasMilitarySrv2");
                    hiddenClientControls.Add("lblWentToMilitaryNo");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_MILITARYTRAINING");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryTraining");
                    disabledClientControls.Add("militaryTraining1");
                    disabledClientControls.Add("lblMilitaryTraining1");
                    disabledClientControls.Add("militaryTraining2");
                    disabledClientControls.Add("lblMilitaryTraining2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryTraining");
                    hiddenClientControls.Add("militaryTraining1");
                    hiddenClientControls.Add("lblMilitaryTraining1");
                    hiddenClientControls.Add("militaryTraining2");
                    hiddenClientControls.Add("lblMilitaryTraining2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_RECORDOFSERVICE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblRecordOfServiceSeries");
                    disabledClientControls.Add("txtRecordOfServiceSeries");
                    disabledClientControls.Add("lblRecordOfServiceNumber");
                    disabledClientControls.Add("txtRecordOfServiceNumber");
                    disabledClientControls.Add("lblRecordOfServiceDate");
                    disabledClientControls.Add("txtRecordOfServiceDate");
                    disabledClientControls.Add("chkRecordOfServiceCopy");
                    disabledClientControls.Add("lblRecordOfServiceCopy");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRecordOfServiceSeries");
                    hiddenClientControls.Add("txtRecordOfServiceSeries");
                    hiddenClientControls.Add("lblRecordOfServiceNumber");
                    hiddenClientControls.Add("txtRecordOfServiceNumber");
                    hiddenClientControls.Add("lblRecordOfServiceDate");
                    hiddenClientControls.Add("spanRecordOfServiceDate");
                    hiddenClientControls.Add("chkRecordOfServiceCopy");
                    hiddenClientControls.Add("lblRecordOfServiceCopy");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_PERMCITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPermPostCode");
                    disabledClientControls.Add("txtPermPostCode");
                    disabledClientControls.Add("lblPermCity");
                    disabledClientControls.Add("ddPermCity");
                    disabledClientControls.Add("lblPermRegion");
                    disabledClientControls.Add("ddPermRegion");
                    disabledClientControls.Add("lblPermMunicipality");
                    disabledClientControls.Add("ddPermMunicipality");
                    disabledClientControls.Add("lblPermDistrict");
                    disabledClientControls.Add("ddPermDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("txtPermPostCode");
                    hiddenClientControls.Add("lblPermCity");
                    hiddenClientControls.Add("ddPermCity");
                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("ddPermRegion");
                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("ddPermMunicipality");
                    hiddenClientControls.Add("lblPermDistrict");
                    hiddenClientControls.Add("ddPermDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_PERMADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPermAddress");
                    disabledClientControls.Add("txtPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("txtPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_CURRCITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrPostCode");
                    disabledClientControls.Add("txtCurrPostCode");
                    disabledClientControls.Add("lblCurrCity");
                    disabledClientControls.Add("ddCurrCity");
                    disabledClientControls.Add("lblCurrRegion");
                    disabledClientControls.Add("ddCurrRegion");
                    disabledClientControls.Add("lblCurrMunicipality");
                    disabledClientControls.Add("ddCurrMunicipality");
                    disabledClientControls.Add("lblCurrDistrict");
                    disabledClientControls.Add("ddCurrDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrPostCode");
                    hiddenClientControls.Add("txtCurrPostCode");
                    hiddenClientControls.Add("lblCurrCity");
                    hiddenClientControls.Add("ddCurrCity");
                    hiddenClientControls.Add("lblCurrRegion");
                    hiddenClientControls.Add("ddCurrRegion");
                    hiddenClientControls.Add("lblCurrMunicipality");
                    hiddenClientControls.Add("ddCurrMunicipality");
                    hiddenClientControls.Add("lblCurrDistrict");
                    hiddenClientControls.Add("ddCurrDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_CURRADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrAddress");
                    disabledClientControls.Add("txtCurrAddress");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrAddress");
                    hiddenClientControls.Add("txtCurrAddress");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_MOBILEPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMobilePhone");
                    disabledClientControls.Add("txtMobilePhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobilePhone");
                    hiddenClientControls.Add("txtMobilePhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_HOMEPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblHomePhone");
                    disabledClientControls.Add("txtHomePhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHomePhone");
                    hiddenClientControls.Add("txtHomePhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_BUSINESSPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblBusinessPhone");
                    disabledClientControls.Add("txtBusinessPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBusinessPhone");
                    hiddenClientControls.Add("txtBusinessPhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_EMAIL");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblEmail");
                    disabledClientControls.Add("txtEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmail");
                    hiddenClientControls.Add("txtEmail");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_MARITALSTATUS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMaritalStatus");
                    disabledClientControls.Add("ddMaritalStatus");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaritalStatus");
                    hiddenClientControls.Add("ddMaritalStatus");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_CHILDCOUNT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblChildCount");
                    disabledClientControls.Add("txtChildCount");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblChildCount");
                    hiddenClientControls.Add("txtChildCount");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_PARENTSCONTACT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblParentsContact");
                    disabledClientControls.Add("txtParentsContact");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblParentsContact");
                    hiddenClientControls.Add("txtParentsContact");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_SIZECLOTHING");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeClothing");
                    disabledClientControls.Add("ddSizeClothing");
                    hiddenClientControls.Add("imgMaintSizeClothing");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeClothing");
                    hiddenClientControls.Add("ddSizeClothing");
                    hiddenClientControls.Add("imgMaintSizeClothing");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_SIZEHAT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeHat");
                    disabledClientControls.Add("ddSizeHat");
                    hiddenClientControls.Add("imgMaintSizeHat");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeHat");
                    hiddenClientControls.Add("ddSizeHat");
                    hiddenClientControls.Add("imgMaintSizeHat");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_SIZESHOES");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeShoes");
                    disabledClientControls.Add("ddSizeShoes");
                    hiddenClientControls.Add("imgMaintSizeShoes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeShoes");
                    hiddenClientControls.Add("ddSizeShoes");
                    hiddenClientControls.Add("imgMaintSizeShoes");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_PERSONHEIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPersonHeight");
                    disabledClientControls.Add("txtPersonHeight");
                    disabledClientControls.Add("lblPersonHeightMeasure");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPersonHeight");
                    hiddenClientControls.Add("txtPersonHeight");
                    hiddenClientControls.Add("lblPersonHeightMeasure");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_ISABROAD");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("chkIsAbroad");
                    disabledClientControls.Add("lblIsAbroad");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("chkIsAbroad");
                    hiddenClientControls.Add("lblIsAbroad");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_ABROADCOUNTRY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("ddAbroadCountry");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddAbroadCountry");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_ABROADSINCE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblAbroadSince");
                    disabledClientControls.Add("txtAbroadSince");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAbroadSince");
                    hiddenClientControls.Add("spAbroadSince");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_ABROADPERIOD");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblAbroadPeriod");
                    disabledClientControls.Add("txtAbroadPeriod");
                    disabledClientControls.Add("lblAbroadPeriodMeasure");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAbroadPeriod");
                    hiddenClientControls.Add("txtAbroadPeriod");
                    hiddenClientControls.Add("lblAbroadPeriodMeasure");
                }
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled || isPreview;

                personalDataDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                       page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                       page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Disabled;

                if (screenDisabled)
                {
                    hiddenClientControls.Add("btnSavePersonalData");
                }

                if (personalDataDisabled)
                {
                    hiddenClientControls.Add("btnSavePersonalData");
                }

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_IDENTNUMBER");

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_IDCARDNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardNumber");
                    disabledClientControls.Add("txtIDCardNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_IDCARDISSUEDBY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardIssuedBy");
                    disabledClientControls.Add("txtIDCardIssuedBy");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_IDCARDISSUEDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIDCardIssueDate");
                    disabledClientControls.Add("txtIDCardIssueDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("spanIDCardIssueDate");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_GENDER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblGender");
                    disabledClientControls.Add("ddGender");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblGender");
                    hiddenClientControls.Add("ddGender");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_BIRTHPLACE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblBirthCountry");
                    disabledClientControls.Add("ddBirthCountry");
                    disabledClientControls.Add("lblBirthCityIfAbroad");
                    disabledClientControls.Add("txtBirthCityIfAbroad");
                    disabledClientControls.Add("lblBirthPostCode");
                    disabledClientControls.Add("txtBirthPostCode");
                    disabledClientControls.Add("lblBirthCity");
                    disabledClientControls.Add("ddBirthCity");
                    disabledClientControls.Add("lblBirthRegion");
                    disabledClientControls.Add("ddBirthRegion");
                    disabledClientControls.Add("lblBirthMunicipality");
                    disabledClientControls.Add("ddBirthMunicipality");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBirthCountry");
                    hiddenClientControls.Add("ddBirthCountry");
                    hiddenClientControls.Add("lblBirthCityIfAbroad");
                    hiddenClientControls.Add("txtBirthCityIfAbroad");
                    hiddenClientControls.Add("lblBirthPostCode");
                    hiddenClientControls.Add("txtBirthPostCode");
                    hiddenClientControls.Add("lblBirthCity");
                    hiddenClientControls.Add("ddBirthCity");
                    hiddenClientControls.Add("lblBirthRegion");
                    hiddenClientControls.Add("ddBirthRegion");
                    hiddenClientControls.Add("lblBirthMunicipality");
                    hiddenClientControls.Add("ddBirthMunicipality");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DRIVINGLICENSE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblDrvLicCategories");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    //In this case we hide div element
                    hiddenClientControls.Add("divPickListDrvLicCategories");
                    hiddenClientControls.Add("lblDrvLicCategories");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_HASMILITARYSERVICE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblWentToMilitary");
                    disabledClientControls.Add("hasMilitarySrv1");
                    disabledClientControls.Add("lblWentToMilitaryYes");
                    disabledClientControls.Add("hasMilitarySrv2");
                    disabledClientControls.Add("lblWentToMilitaryNo");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWentToMilitary");
                    hiddenClientControls.Add("hasMilitarySrv1");
                    hiddenClientControls.Add("lblWentToMilitaryYes");
                    hiddenClientControls.Add("hasMilitarySrv2");
                    hiddenClientControls.Add("lblWentToMilitaryNo");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_MILITARYTRAINING");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryTraining");
                    disabledClientControls.Add("militaryTraining1");
                    disabledClientControls.Add("lblMilitaryTraining1");
                    disabledClientControls.Add("militaryTraining2");
                    disabledClientControls.Add("lblMilitaryTraining2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryTraining");
                    hiddenClientControls.Add("militaryTraining1");
                    hiddenClientControls.Add("lblMilitaryTraining1");
                    hiddenClientControls.Add("militaryTraining2");
                    hiddenClientControls.Add("lblMilitaryTraining2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_RECORDOFSERVICE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblRecordOfServiceSeries");
                    disabledClientControls.Add("txtRecordOfServiceSeries");
                    disabledClientControls.Add("lblRecordOfServiceNumber");
                    disabledClientControls.Add("txtRecordOfServiceNumber");
                    disabledClientControls.Add("lblRecordOfServiceDate");
                    disabledClientControls.Add("txtRecordOfServiceDate");
                    disabledClientControls.Add("chkRecordOfServiceCopy");
                    disabledClientControls.Add("lblRecordOfServiceCopy");
                    hiddenClientControls.Add("imgNewRecordOfService");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRecordOfServiceSeries");
                    hiddenClientControls.Add("txtRecordOfServiceSeries");
                    hiddenClientControls.Add("lblRecordOfServiceNumber");
                    hiddenClientControls.Add("txtRecordOfServiceNumber");
                    hiddenClientControls.Add("lblRecordOfServiceDate");
                    hiddenClientControls.Add("spanRecordOfServiceDate");
                    hiddenClientControls.Add("chkRecordOfServiceCopy");
                    hiddenClientControls.Add("lblRecordOfServiceCopy");
                    hiddenClientControls.Add("imgNewRecordOfService");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_PERMCITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPermPostCode");
                    disabledClientControls.Add("txtPermPostCode");
                    disabledClientControls.Add("lblPermCity");
                    disabledClientControls.Add("ddPermCity");
                    disabledClientControls.Add("lblPermRegion");
                    disabledClientControls.Add("ddPermRegion");
                    disabledClientControls.Add("lblPermMunicipality");
                    disabledClientControls.Add("ddPermMunicipality");
                    disabledClientControls.Add("lblPermDistrict");
                    disabledClientControls.Add("ddPermDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("txtPermPostCode");
                    hiddenClientControls.Add("lblPermCity");
                    hiddenClientControls.Add("ddPermCity");
                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("ddPermRegion");
                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("ddPermMunicipality");
                    hiddenClientControls.Add("lblPermDistrict");
                    hiddenClientControls.Add("ddPermDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_PERMADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPermAddress");
                    disabledClientControls.Add("txtPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("txtPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddress");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CURRCITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrPostCode");
                    disabledClientControls.Add("txtCurrPostCode");
                    disabledClientControls.Add("lblCurrCity");
                    disabledClientControls.Add("ddCurrCity");
                    disabledClientControls.Add("lblCurrRegion");
                    disabledClientControls.Add("ddCurrRegion");
                    disabledClientControls.Add("lblCurrMunicipality");
                    disabledClientControls.Add("ddCurrMunicipality");
                    disabledClientControls.Add("lblCurrDistrict");
                    disabledClientControls.Add("ddCurrDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrPostCode");
                    hiddenClientControls.Add("txtCurrPostCode");
                    hiddenClientControls.Add("lblCurrCity");
                    hiddenClientControls.Add("ddCurrCity");
                    hiddenClientControls.Add("lblCurrRegion");
                    hiddenClientControls.Add("ddCurrRegion");
                    hiddenClientControls.Add("lblCurrMunicipality");
                    hiddenClientControls.Add("ddCurrMunicipality");
                    hiddenClientControls.Add("lblCurrDistrict");
                    hiddenClientControls.Add("ddCurrDistrict");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CURRADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrAddress");
                    disabledClientControls.Add("txtCurrAddress");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrAddress");
                    hiddenClientControls.Add("txtCurrAddress");
                    hiddenClientControls.Add("btnImgCopyAddress2");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_MOBILEPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMobilePhone");
                    disabledClientControls.Add("txtMobilePhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobilePhone");
                    hiddenClientControls.Add("txtMobilePhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_HOMEPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblHomePhone");
                    disabledClientControls.Add("txtHomePhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHomePhone");
                    hiddenClientControls.Add("txtHomePhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_BUSINESSPHONE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblBusinessPhone");
                    disabledClientControls.Add("txtBusinessPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBusinessPhone");
                    hiddenClientControls.Add("txtBusinessPhone");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_EMAIL");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblEmail");
                    disabledClientControls.Add("txtEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmail");
                    hiddenClientControls.Add("txtEmail");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_MARITALSTATUS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMaritalStatus");
                    disabledClientControls.Add("ddMaritalStatus");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaritalStatus");
                    hiddenClientControls.Add("ddMaritalStatus");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CHILDCOUNT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblChildCount");
                    disabledClientControls.Add("txtChildCount");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblChildCount");
                    hiddenClientControls.Add("txtChildCount");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_PARENTSCONTACT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblParentsContact");
                    disabledClientControls.Add("txtParentsContact");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblParentsContact");
                    hiddenClientControls.Add("txtParentsContact");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_SIZECLOTHING");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeClothing");
                    disabledClientControls.Add("ddSizeClothing");
                    hiddenClientControls.Add("imgMaintSizeClothing");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeClothing");
                    hiddenClientControls.Add("ddSizeClothing");
                    hiddenClientControls.Add("imgMaintSizeClothing");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_SIZEHAT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeHat");
                    disabledClientControls.Add("ddSizeHat");
                    hiddenClientControls.Add("imgMaintSizeHat");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeHat");
                    hiddenClientControls.Add("ddSizeHat");
                    hiddenClientControls.Add("imgMaintSizeHat");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_SIZESHOES");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblSizeShoes");
                    disabledClientControls.Add("ddSizeShoes");
                    hiddenClientControls.Add("imgMaintSizeShoes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSizeShoes");
                    hiddenClientControls.Add("ddSizeShoes");
                    hiddenClientControls.Add("imgMaintSizeShoes");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_PERSONHEIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblPersonHeight");
                    disabledClientControls.Add("txtPersonHeight");
                    disabledClientControls.Add("lblPersonHeightMeasure");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPersonHeight");
                    hiddenClientControls.Add("txtPersonHeight");
                    hiddenClientControls.Add("lblPersonHeightMeasure");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_ISABROAD");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("chkIsAbroad");
                    disabledClientControls.Add("lblIsAbroad");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("chkIsAbroad");
                    hiddenClientControls.Add("lblIsAbroad");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_ABROADCOUNTRY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("ddAbroadCountry");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddAbroadCountry");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_ABROADSINCE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblAbroadSince");
                    disabledClientControls.Add("txtAbroadSince");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAbroadSince");
                    hiddenClientControls.Add("spAbroadSince");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_ABROADPERIOD");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblAbroadPeriod");
                    disabledClientControls.Add("txtAbroadPeriod");
                    disabledClientControls.Add("lblAbroadPeriodMeasure");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAbroadPeriod");
                    hiddenClientControls.Add("txtAbroadPeriod");
                    hiddenClientControls.Add("lblAbroadPeriodMeasure");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_CONVICTION");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblConviction");
                    disabledClientControls.Add("ddConviction");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblConviction");
                    hiddenClientControls.Add("ddConviction");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_CONVICTIONREASON");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblConvictionReason");
                    disabledClientControls.Add("ddConvictionReason");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblConvictionReason");
                    hiddenClientControls.Add("ddConvictionReason");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_DATEFROM");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblConvDateFrom");
                    disabledClientControls.Add("txtConvDateFrom");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblConvDateFrom");
                    hiddenClientControls.Add("contConvDateFrom");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION_DATETO");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblConvDateTo");
                    disabledClientControls.Add("txtConvDateTo");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblConvDateTo");
                    hiddenClientControls.Add("contConvDateTo");
                }

                l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP_COUNTRY");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblDualCitizenshipCountry");
                    disabledClientControls.Add("ddDualCitizenshipCountry");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblDualCitizenshipCountry");
                    hiddenClientControls.Add("ddDualCitizenshipCountry");
                }
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_SIZECLOTHING") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintSizeClothing");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_SIZEHAT") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintSizeHat");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_SIZESHOES") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintSizeShoes");
            }

            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach (string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach (string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }
    }
}
