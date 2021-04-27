using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.IO;

using PMIS.Common;
using PMIS.Reserve.Common;


namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_Owner : RESPage
    {
        private string technicsTypeKey = null;
        public string TechnicsTypeKey
        {
            get
            {
                if (technicsTypeKey == null)
                {
                    int technicsId = int.Parse(Request.Params["TechnicsId"]);
                    Technics technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                    technicsTypeKey = technics.TechnicsType.TypeKey;
                }

                return technicsTypeKey;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetDriverInfo")
            {
                JSGetDriverInfo();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveOwner")
            {
                JSSaveOwner();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetOwnerInfo")
            {
                JSGetOwnerInfo();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCompanyInfo")
            {
                JSGetCompanyInfo();
                return;
            }
        }

        //Get driver info by ident number (ajax call)
        private void JSGetDriverInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OWNER") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string identNumber = Request.Params["IdentNumber"];

            string stat = "";
            string response = "";

            try
            {
                int reservistId = 0;
                string driverFullName = "";
                string driverMilitaryRank = "";
                string driverPermCity = "";
                string driverPermAddress = "";
                string driverHomePhone = "";
                string driverMobilePhone = "";
                string driverEmail = "";
                string driverMilitaryReportSpecialties = "";
                string driverDrvLicenseCategories = "";
                string driverMilRepStatus = "";

                Reservist reservist = ReservistUtil.GetReservistByIdentNumber(identNumber, CurrentUser);

                if (reservist != null)
                {
                    reservistId = reservist.ReservistId;
                    driverFullName = reservist.Person.FullName;
                    driverMilitaryRank = reservist.Person.MilitaryRank != null ? reservist.Person.MilitaryRank.LongName : "";
                    driverPermCity = reservist.Person.PermCityId.HasValue ? reservist.Person.PermCity.RegionMunicipalityAndCity : "";
                    driverPermAddress = reservist.Person.PermAddress;
                    driverHomePhone = reservist.Person.HomePhone.HasValue ? reservist.Person.HomePhone.Value.ToString() : "";
                    driverMobilePhone = reservist.Person.MobilePhone;
                    driverEmail = reservist.Person.Email;

                    List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(reservist.PersonId, CurrentUser);

                    foreach (PersonMilitaryReportSpeciality personMilRepSpecialty in personMilitaryReportSpecialities)
                    {
                        driverMilitaryReportSpecialties += (driverMilitaryReportSpecialties == "" ? "" : "<br />") +
                            personMilRepSpecialty.MilitaryReportSpeciality.CodeAndName;
                    }

                    foreach (DrivingLicenseCategory drivingLicenseCategory in reservist.Person.DrivingLicenseCategories)
                    {
                        driverDrvLicenseCategories += (driverDrvLicenseCategories == "" ? "" : ", ") +
                            drivingLicenseCategory.DrivingLicenseCategoryName;
                    }

                    driverMilRepStatus = reservist.CurrResMilRepStatus != null && reservist.CurrResMilRepStatus.MilitaryReportStatus != null ? reservist.CurrResMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName : "";
                }

                stat = AJAXTools.OK;
                
                response = @"
                    <reservistId>" + reservistId.ToString() + @"</reservistId>
                    <driverFullName>" + AJAXTools.EncodeForXML(driverFullName) + @"</driverFullName>
                    <driverMilitaryRank>" + AJAXTools.EncodeForXML(driverMilitaryRank) + @"</driverMilitaryRank>
                    <driverPermCity>" + AJAXTools.EncodeForXML(driverPermCity) + @"</driverPermCity>
                    <driverPermAddress>" + AJAXTools.EncodeForXML(driverPermAddress) + @"</driverPermAddress>
                    <driverHomePhone>" + AJAXTools.EncodeForXML(driverHomePhone) + @"</driverHomePhone>
                    <driverMobilePhone>" + AJAXTools.EncodeForXML(driverMobilePhone) + @"</driverMobilePhone>
                    <driverEmail>" + AJAXTools.EncodeForXML(driverEmail) + @"</driverEmail>
                    <driverMilitaryReportSpecialties>" + AJAXTools.EncodeForXML(driverMilitaryReportSpecialties) + @"</driverMilitaryReportSpecialties>
                    <driverDrvLicenseCategories>" + AJAXTools.EncodeForXML(driverDrvLicenseCategories) + @"</driverDrvLicenseCategories>
                    <driverMilRepStatus>" + AJAXTools.EncodeForXML(driverMilRepStatus) + @"</driverMilRepStatus>
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

        private void JSSaveOwner()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;
                if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
                {
                    int.TryParse(Request.Params["TechnicsId"], out technicsId);
                }

                string technicsTypeKey = "";
                if (!String.IsNullOrEmpty(Request.Params["TechnicsTypeKey"]))
                {
                    technicsTypeKey = Request.Params["TechnicsTypeKey"].ToString();
                }

                int? driverReservistId = null;
                if (!String.IsNullOrEmpty(Request.Params["DriverReservistId"]) &&
                    Request.Params["DriverReservistId"] != "0")
                {
                    driverReservistId = int.Parse(Request.Params["DriverReservistId"]);
                }

                int ownershipLeasing = int.Parse(Request.Params["OwnershipLeasing"]);

                int? ownershipCompanyId = null;
                if (!String.IsNullOrEmpty(Request.Params["OwnershipCompanyId"]) &&
                    Request.Params["OwnershipCompanyId"] != "0")
                {
                    ownershipCompanyId = int.Parse(Request.Params["OwnershipCompanyId"]);
                }

                if (technicsId == 0)
                {
                    throw new Exception("Техниката не е намерена");
                }
                else
                {
                    TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, CurrentUser);

                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_" + technicsType.TypeKey);

                    Technics technics = new Technics(CurrentUser);
                    technics.TechnicsId = technicsId;
                    technics.DriverReservistId = driverReservistId;
                    technics.OwnershipLeasing = ownershipLeasing == 1;
                    technics.OwnershipCompanyId = ownershipCompanyId;
                    
                    TechnicsUtil.SaveTechnics_Owner(technics, CurrentUser, change);

                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<response>Информацията е записана успешно</response>" +
                               "<ownershipCompanyId>" + (ownershipCompanyId != 0 ? ownershipCompanyId.ToString() : "0") + "</ownershipCompanyId>";
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

        private void JSGetOwnerInfo()
        {
            string response = "";
            string stat = "";
            string ownerInfo = "";

            try
            {
                int technicsId = int.Parse(Request.Form["TechnicsId"]);
                Technics technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                string missingValue = "<липсва>";
                ownerInfo = missingValue;
                if (technics.OwnershipCompany != null)
                {
                    ownerInfo = (technics.OwnershipCompany != null ? technics.OwnershipCompany.UnifiedIdentityCode + " " + technics.OwnershipCompany.CompanyName : "");
                }

                stat = AJAXTools.OK;

                response = "<ownerInfo>" + AJAXTools.EncodeForXML(ownerInfo) + @"</ownerInfo>";
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

        private void JSGetCompanyInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OWNER") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int companyId = int.Parse(Request.Params["CompanyId"]);

            string stat = "";
            string response = "";

            try
            {
                string region = "";
                string municipality = "";
                string city = "";
                string district = "";
                string address = "";
                string postCode = "";
                string phone = "";

                Company company = CompanyUtil.GetCompany(companyId, CurrentUser);

                if (company != null)
                {
                    region = company.City != null && company.City.Region != null ? company.City.Region.RegionName : "";
                    municipality = company.City != null && company.City.Municipality != null ? company.City.Municipality.MunicipalityName : "";
                    city = company.City != null ? company.City.CityName : "";
                    district = company.City != null && company.District != null ? company.District.DistrictName : "";
                    address = company.Address;
                    postCode = company.PostCode;
                    phone = company.Phone;
                }

                stat = AJAXTools.OK;

                response = @"
                    <region>" + AJAXTools.EncodeForXML(region) + @"</region>
                    <municipality>" + AJAXTools.EncodeForXML(municipality) + @"</municipality>
                    <city>" + AJAXTools.EncodeForXML(city) + @"</city>
                    <district>" + AJAXTools.EncodeForXML(district) + @"</district>
                    <address>" + AJAXTools.EncodeForXML(address) + @"</address>
                    <postCode>" + AJAXTools.EncodeForXML(postCode) + @"</postCode>
                    <phone>" + AJAXTools.EncodeForXML(phone) + @"</phone>
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
    }

    public static class AddEditTechnics_Owner_PageUtil
    {
        public static string GetTabContent(AddEditTechnics page, User currentUser)
        {
            string driverReservistId = "0";
            string driverIdentNumber = "";
            string driverFullName = "";
            string driverMilitaryRank = "";
            string driverPermCity = "";
            string driverPermAddress = "";
            string driverHomePhone = "";
            string driverMobilePhone = "";
            string driverEmail = "";
            string driverMilitaryReportSpecialties = "";
            string driverDrvLicenseCategories = "";
            string driverMilRepStatus = "";

            Technics technics = TechnicsUtil.GetTechnics(page.TechnicsId, page.CurrentUser);
            
            if (technics.DriverReservistId.HasValue)
            {
                Reservist driver = technics.DriverReservist;

                driverReservistId = driver.ReservistId.ToString();
                driverIdentNumber = driver.Person.IdentNumber;
                driverFullName = driver.Person.FullName;
                driverMilitaryRank = driver.Person.MilitaryRank != null ? driver.Person.MilitaryRank.LongName : "";
                driverPermCity = driver.Person.PermCityId.HasValue ? driver.Person.PermCity.RegionMunicipalityAndCity : "";
                driverPermAddress = driver.Person.PermAddress;
                driverHomePhone = driver.Person.HomePhone.HasValue ? driver.Person.HomePhone.Value.ToString() : "";
                driverMobilePhone = driver.Person.MobilePhone;
                driverEmail = driver.Person.Email;

                List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(driver.PersonId, page.CurrentUser);

                foreach(PersonMilitaryReportSpeciality personMilRepSpecialty in personMilitaryReportSpecialities)
                {
                    driverMilitaryReportSpecialties += (driverMilitaryReportSpecialties == "" ? "" : "<br />") +
                        personMilRepSpecialty.MilitaryReportSpeciality.CodeAndName;
                }

                foreach (DrivingLicenseCategory drivingLicenseCategory in driver.Person.DrivingLicenseCategories)
                {
                    driverDrvLicenseCategories += (driverDrvLicenseCategories == "" ? "" : ", ") +
                        drivingLicenseCategory.DrivingLicenseCategoryName;
                }

                driverMilRepStatus = driver.CurrResMilRepStatus != null && driver.CurrResMilRepStatus.MilitaryReportStatus != null ? driver.CurrResMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName : "";
            }

            string html = @"

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Данни за собственика</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td colspan=""2"" style=""text-align: left; padding-left: 25px;"">
                        <input type=""checkbox"" id=""chkOwnershipLeasing"" " + (technics.OwnershipLeasing ? " checked='checked' " : "") + @" /> <label for=""chkOwnershipLeasing"" id=""lblOwnershipLeasing"" class=""InputLabel"">Отдадено на лизинг</label>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; width: 130px; vertical-align: top;"">
                        <span id=""lblOwnershipCompanyName"" class=""InputLabel"" >Име на фирмата:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblCompanyNameValue"" class=""ReadOnlyValue"" style=""vertical-align: top;"">" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.CompanyName : "") + @"</span>
                        <img src=""../Images/clear-icon.png"" id=""btnImgClearCompany"" title=""Премахване на избраната фирма"" alt=""Премахване на избраната фирма"" style=""cursor: pointer; width: 16px; height: 16px;" + (technics.OwnershipCompany != null ? "" : "visibility: hidden;") + @"""
                             onclick=""ClearSelectedCompany();"" />
                        <span id=""lblOwnershipUnifiedIdentityCode"" class=""InputLabel"" style=""margin-left: 50px; vertical-align: top;"">" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":</span>
                        <span id=""lblUnifiedIdentityCodeValue"" class=""ReadOnlyValue"" style=""vertical-align: top;"">" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.UnifiedIdentityCode : "") + @"</span><br/>
                        <input id=""btnSelectCompany"" 
                               type=""button"" 
                               value=""Търсене на фирма"" 
                               class=""OpenCompanySelectorButton"" 
                               onclick='companySelector.showDialog(""companySelectorForWorkplace"", CompanySelector_OnSelectedCompany);' />
                        <input type=""hidden"" id=""hdnCompanyID"" value=""" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.CompanyId.ToString() : "") + @"""/>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblOwnershipType"" class=""InputLabel"""">Вид собственост:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblOwnershipTypeValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null && technics.OwnershipCompany.OwnershipType != null ? technics.OwnershipCompany.OwnershipType.OwnershipTypeName : "") + @"</span>
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
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Адрес</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table style=""margin: 0 auto;"">
                <tr>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblOwnershipRegion"" class=""InputLabel"">Област:</span>
                    </td>
                    <td style=""text-align: left; width: 170px;"">
                        <span id=""lblOwnershipRegionValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null && technics.OwnershipCompany.City != null && technics.OwnershipCompany.City.Region != null ? technics.OwnershipCompany.City.Region.RegionName : "") + @"</span>
                    </td>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblOwnershipMunicipality"" class=""InputLabel"">Община:</span>
                    </td>
                    <td style=""text-align: left; width: 170px;"">
                        <span id=""lblOwnershipMunicipalityValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null && technics.OwnershipCompany.City != null && technics.OwnershipCompany.City.Municipality != null ? technics.OwnershipCompany.City.Municipality.MunicipalityName : "") + @"</span>
                    </td>                    
                    <td style=""text-align: right; width: 215px;"">
                        <span id=""lblOwnershipCity"" class=""InputLabel"">Населено място:</span>
                    </td>
                    <td style=""text-align: left; width: 170px;"">
                        <span id=""lblOwnershipCityValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null && technics.OwnershipCompany.City != null ? technics.OwnershipCompany.City.CityName : "") + @"</span>
                    </td>
                    
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"" rowspan=""2"">
                        <span id=""lblOwnershipAddress"" class=""InputLabel"">Адрес:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left; vertical-align: top;"" rowspan=""2"">
                        <span id=""lblOwnershipAddressValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.Address : "") + @"</span>
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblOwnershipDistrict"" class=""InputLabel"">Район:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblOwnershipDistrictValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null && technics.OwnershipCompany.District != null ? technics.OwnershipCompany.District.DistrictName : "") + @"</span>
                    </td>
                </tr>
                <tr>                    
                    <td style=""text-align: right;"">
                        <span id=""lblOwnershipPostCode"" class=""InputLabel"">Пощенски код:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblOwnershipPostCodeValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.PostCode : "") + @"</span>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblOwnershipPhone"" class=""InputLabel"">Телефон:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"" rowspan=""2"">
                        <span id=""lblOwnershipPhoneValue"" class=""ReadOnlyValue"">" + (technics.OwnershipCompany != null ? technics.OwnershipCompany.Phone : "") + @"</span>
                    </td>
            </table>
         </td>
      </tr>
   </table>
</fieldset>";

            if (page.TechnicsTypeKey != "TRAILERS" && page.TechnicsTypeKey != "VESSELS" && page.TechnicsTypeKey != "FUEL_CONTAINERS")
            {
                html += @"<div style=""height: 10px;""></div>

                <fieldset id=""fsDriverInfo"" style=""width: 830px; padding: 0px;"">
                   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                      <tr style=""height: 3px;"">
                      </tr>
                      <tr>
                         <td style=""text-align: left;"">
                            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Данни за водача</span>
                         </td>
                      </tr>
                      <tr>
                         <td style=""text-align: left;"">
                            <table cellpadding=""2"">
                                <tr id=""tblDriverInfoRow1"">
                                    <td style=""text-align: right; min-width: 80px;"">
                                        <span id=""lblDriverIdentNumber"" class=""InputLabel"">ЕГН:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top; width: 130px;"">
                                        <input type=""text"" id=""txtDriverIdentNumber"" class=""InputField"" style=""width: 100px;"" maxlength=""10"" value=""" + CommonFunctions.HtmlEncoding(driverIdentNumber) + @""" 
                                             onfocus=""DriverIdentNumberFocus();"" onblur=""DriverIdentNumberBlur();"" />
                                    </td>
                                    <td style=""text-align: right;vertical-align: top; width: 100px;"">
                                        <div id=""lblDriverFullNameUIItems"">
                                           <span id=""lblDriverFullName"" class=""InputLabel"">Трите имена:</span>
                                        </div>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top; width: 200px;"">
                                        <div id=""lblDriverFullNameValueUIItems"">
                                           <span id=""lblDriverFullNameValue"" class=""ReadOnlyValue"">" + driverFullName + @"</span>
                                        </div>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top; width: 120px;"">
                                        <div id=""lblDriverMilitaryRankUIItems"">
                                           <span id=""lblDriverMilitaryRank"" class=""InputLabel"">Военно звание:</span>
                                        </div>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top; width: 200px;"">
                                        <div id=""lblDriverMilitaryRankValueUIItems"">
                                           <span id=""lblDriverMilitaryRankValue"" class=""ReadOnlyValue"">" + driverMilitaryRank + @"</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id=""tblDriverInfoRow2"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverPermCity"" class=""InputLabel"">Град:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"" colspan=""5"">
                                        <span id=""lblDriverPermCityValue"" class=""ReadOnlyValue"">" + driverPermCity + @"</span> &nbsp;&nbsp;&nbsp;
                                        <span id=""lblDriverPermAddress"" class=""InputLabel"">Адрес:</span>
                                        <span id=""lblDriverPermAddressValue"" class=""ReadOnlyValue"">" + driverPermAddress + @"</span>
                                    </td>
                                </tr>
                                <tr id=""tblDriverInfoRow3"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverHomePhone"" class=""InputLabel"">Телефон:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""lblDriverHomePhoneValue"" class=""ReadOnlyValue"">" + driverHomePhone + @"</span>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverMobilePhone"" class=""InputLabel"">Мобилен:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""lblDriverMobilePhoneValue"" class=""ReadOnlyValue"">" + driverMobilePhone + @"</span>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverEmail"" class=""InputLabel"">E-mail:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""lblDriverEmailValue"" class=""ReadOnlyValue"">" + driverEmail + @"</span>
                                    </td>
                                </tr>
                                <tr id=""tblDriverInfoRow4"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverMilRepSpecialties"" class=""InputLabel"">ВОС:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"" colspan=""5"">
                                        <span id=""lblDriverMilRepSpecialtiesValue"" class=""ReadOnlyValue"">" + driverMilitaryReportSpecialties + @"</span>
                                    </td>
                                </tr>
                                <tr id=""tblDriverInfoRow5"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblDriverDrivingLicenseCategories"" class=""InputLabel"">Категория:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"" colspan=""5"">
                                        <span id=""lblDriverDrivingLicenseCategoriesValue"" class=""ReadOnlyValue"">" + driverDrvLicenseCategories + @"</span> &nbsp;&nbsp;&nbsp;
                                        <span id=""lblDriverMilRepStatus"" class=""InputLabel"">Състояние по отчета:</span>
                                        <span id=""lblDriverMilRepStatusValue"" class=""ReadOnlyValue"">" + driverMilRepStatus + @"</span>
                                    </td>
                                </tr>
                                <tr id=""tblDriverInfoRow6"" style=""display: none;"">
                                    <td style=""text-align: left; vertical-align: top; padding-left: 90px;"" colspan=""6"">
                                        <span class=""ErrorText"">В системата няма запис с такъв ЕГН от резерва</span>
                                    </td>
                                </tr>
                            </table>
                         </td>
                      </tr>
                   </table>
                </fieldset>";   
            }
            
            if (page.TechnicsTypeKey == "VESSELS")
            {
                if (page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divVesselCrew'>" + AddEditTechnics_VESSELS_PageUtil.GetVesselCrewTable(page, currentUser) + "</div>";
                    html += AddEditTechnics_VESSELS_PageUtil.GetVesselCrewLightBox(page, currentUser);
                }
            }

html += @"<input type=""hidden"" id=""hdnDriverReservistId"" value=""" + driverReservistId + @""" />

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnUnifiedIdentityCodeLabel"" value=""" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @"""/>
";

            return html;
        }

        public static string GetTabUIItems(AddEditTechnics page)
        {
            string UIItemsXML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool screenDisabled = false;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER") == UIAccessLevel.Disabled || isPreview;
            
            UIAccessLevel ownershipAccessLevel = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP");

            UIAccessLevel l;

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_OWNERSHIPTYPE");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblOwnershipType");
                disabledClientControls.Add("lblOwnershipTypeValue");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOwnershipType");
                hiddenClientControls.Add("lblOwnershipTypeValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_OWNERSHIPLEASING");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("chkOwnershipLeasing");
                disabledClientControls.Add("lblOwnershipLeasing");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("chkOwnershipLeasing");
                hiddenClientControls.Add("lblOwnershipLeasing");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_OWNER");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblOwnershipCompanyName");
                disabledClientControls.Add("lblCompanyNameValue");
                hiddenClientControls.Add("btnImgClearCompany");
                hiddenClientControls.Add("btnSelectCompany");
                disabledClientControls.Add("lblOwnershipUnifiedIdentityCode");
                disabledClientControls.Add("lblUnifiedIdentityCodeValue");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOwnershipCompanyName");
                hiddenClientControls.Add("lblCompanyNameValue");
                hiddenClientControls.Add("btnImgClearCompany");
                hiddenClientControls.Add("btnSelectCompany");
                hiddenClientControls.Add("lblOwnershipUnifiedIdentityCode");
                hiddenClientControls.Add("lblUnifiedIdentityCodeValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_OWNERADDRESSCITY");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblOwnershipPostCode");
                disabledClientControls.Add("lblOwnershipPostCodeValue");
                disabledClientControls.Add("lblOwnershipCity");
                disabledClientControls.Add("lblOwnershipCityValue");
                disabledClientControls.Add("lblOwnershipRegion");
                disabledClientControls.Add("lblOwnershipRegionValue");
                disabledClientControls.Add("lblOwnershipMunicipality");
                disabledClientControls.Add("lblOwnershipMunicipalityValue");
                disabledClientControls.Add("lblOwnershipDistrict");
                disabledClientControls.Add("lblOwnershipDistrictValue");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOwnershipPostCode");
                hiddenClientControls.Add("lblOwnershipPostCodeValue");
                hiddenClientControls.Add("lblOwnershipCity");
                hiddenClientControls.Add("lblOwnershipCityValue");
                hiddenClientControls.Add("lblOwnershipRegion");
                hiddenClientControls.Add("lblOwnershipRegionValue");
                hiddenClientControls.Add("lblOwnershipMunicipality");
                hiddenClientControls.Add("lblOwnershipMunicipalityValue");
                hiddenClientControls.Add("lblOwnershipDistrict");
                hiddenClientControls.Add("lblOwnershipDistrictValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_OWNERADDRESS");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblOwnershipAddress");
                disabledClientControls.Add("lblOwnershipAddressValue");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOwnershipAddress");
                hiddenClientControls.Add("lblOwnershipAddressValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_OWNERSHIP_PHONE");

            if (ownershipAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblOwnershipPhone");
                disabledClientControls.Add("lblOwnershipPhoneValue");
            }
            else if (ownershipAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOwnershipPhone");
                hiddenClientControls.Add("lblOwnershipPhoneValue");
            }

            UIAccessLevel driverAccessLevel = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER");

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_IDENTNUMBER");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverIdentNumber");
                disabledClientControls.Add("txtDriverIdentNumber");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverIdentNumber");
                hiddenClientControls.Add("txtDriverIdentNumber");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_FULLNAME");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverFullNameUIItems");
                disabledClientControls.Add("lblDriverFullNameValueUIItems");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverFullNameUIItems");
                hiddenClientControls.Add("lblDriverFullNameValueUIItems");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_MILITARYRANK");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverMilitaryRankUIItems");
                disabledClientControls.Add("lblDriverMilitaryRankValueUIItems");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverMilitaryRankUIItems");
                hiddenClientControls.Add("lblDriverMilitaryRankValueUIItems");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_CITY");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverPermCity");
                disabledClientControls.Add("lblDriverPermCityValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverPermCity");
                hiddenClientControls.Add("lblDriverPermCityValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_ADDRESS");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverPermAddress");
                disabledClientControls.Add("lblDriverPermAddressValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverPermAddress");
                hiddenClientControls.Add("lblDriverPermAddressValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_HOMEPHONE");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverHomePhone");
                disabledClientControls.Add("lblDriverHomePhoneValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverHomePhone");
                hiddenClientControls.Add("lblDriverHomePhoneValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_MOBILEPHONE");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverMobilePhone");
                disabledClientControls.Add("lblDriverMobilePhoneValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverMobilePhone");
                hiddenClientControls.Add("lblDriverMobilePhoneValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_EMAIL");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverEmail");
                disabledClientControls.Add("lblDriverEmailValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverEmail");
                hiddenClientControls.Add("lblDriverEmailValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_MILITARYREPORTSPECIALTIES");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverMilRepSpecialties");
                disabledClientControls.Add("lblDriverMilRepSpecialtiesValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverMilRepSpecialties");
                hiddenClientControls.Add("lblDriverMilRepSpecialtiesValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_DRVLICENSECATEGORIES");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverDrivingLicenseCategories");
                disabledClientControls.Add("lblDriverDrivingLicenseCategoriesValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverDrivingLicenseCategories");
                hiddenClientControls.Add("lblDriverDrivingLicenseCategoriesValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OWNER_DRIVER_MILREPSTATUS");

            if (driverAccessLevel == UIAccessLevel.Disabled || l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblDriverMilRepStatus");
                disabledClientControls.Add("lblDriverMilRepStatusValue");
            }
            else if (driverAccessLevel == UIAccessLevel.Hidden || l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriverMilRepStatus");
                hiddenClientControls.Add("lblDriverMilRepStatusValue");
            }

            // If technic is Vessel, then setup UI items for the light box
            if (page.TechnicsTypeKey == "VESSELS")
            {
                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_CATEGORY");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblVesselCrewCategoryLightBox");
                    disabledClientControls.Add("ddVesselCrewCategoryLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselCrewCategoryLightBox");
                    hiddenClientControls.Add("ddVesselCrewCategoryLightBox");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_IDENTNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblIdentNumberLightBox");
                    disabledClientControls.Add("txtIdentNumberLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIdentNumberLightBox");
                    hiddenClientControls.Add("txtIdentNumberLightBox");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_MILITARYRANK");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblMilitaryRankLightBox");
                    disabledClientControls.Add("ddMilitaryRankLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRankLightBox");
                    hiddenClientControls.Add("ddMilitaryRankLightBox");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_FULLNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblFullNameLightBox");
                    disabledClientControls.Add("txtFullNameLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFullNameLightBox");
                    hiddenClientControls.Add("txtFullNameLightBox");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_HASAPPOINTMENT");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHasAppointmentLightBox");
                    disabledClientControls.Add("chkHasAppointmentLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHasAppointmentLightBox");
                    hiddenClientControls.Add("chkHasAppointmentLightBox");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_ADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAddressLightBox");
                    disabledClientControls.Add("txtAddressLightBox");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAddressLightBox");
                    hiddenClientControls.Add("txtAddressLightBox");
                }
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
