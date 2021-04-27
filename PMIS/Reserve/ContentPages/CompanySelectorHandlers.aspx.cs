using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class CompanySelectorHandlers : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] == null)
            {
                RedirectAccessDenied();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCollectModuleData")
            {
                JSCollectModuleData();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSearchCompanies")
            {
                JSSearchCompanies();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckIsBulstatAndCompanyName")
            {
                JSCheckIsBulstatAndCompanyName();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCompany")
            {
                JSSaveCompany();
                return;
            }

        }

        private void JSCollectModuleData()
        {
            StringBuilder response = new StringBuilder();
            response.Append("<init>");

            response.Append("<canInsertNewCompany>");
            response.Append(GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD") == UIAccessLevel.Hidden ? "no" : "yes");
            response.Append("</canInsertNewCompany>");

            response.Append("<unifiedIdentityCodeLabelText>");
            response.Append(CommonFunctions.GetLabelText("UnifiedIdentityCode"));
            response.Append("</unifiedIdentityCodeLabelText>");

            response.Append("<administrations>");
            List<Administration> administrations =  AdministrationUtil.GetAllAdministrations(CurrentUser);
            response.Append("<administration>");
            response.Append("<administrationID>-1</administrationID>");
            response.Append("<administrationName></administrationName>");
            response.Append("</administration>");

            foreach (Administration administration in administrations)
            {
                response.Append("<administration>");
                response.Append("<administrationID>" + administration.AdministrationId + "</administrationID>");
                response.Append("<administrationName>" + administration.AdministrationName + "</administrationName>");
                response.Append("</administration>");
            }
            response.Append("</administrations>");

            response.Append("<ownershipTypes>");
            List<OwnershipType> ownershipTypes = OwnershipTypeUtil.GetAllOwnershipTypes(CurrentUser);
            foreach (OwnershipType ownershipType in ownershipTypes)
            {
                response.Append("<ownershipType>");
                response.Append("<ownershipTypeID>" + ownershipType.OwnershipTypeId + "</ownershipTypeID>");
                response.Append("<ownershipTypeName>" + ownershipType.OwnershipTypeName + "</ownershipTypeName>"); 
                response.Append("</ownershipType>");
            }
            response.Append("</ownershipTypes>");

            response.Append("<regions>");
            List<Region> regions = RegionUtil.GetRegions(CurrentUser);

            response.Append("<region>");
            response.Append("<regionID>-1</regionID>");
            response.Append("<regionName></regionName>");
            response.Append("</region>");
            foreach (Region region in regions)
            {
                response.Append("<region>");
                response.Append("<regionID>" + region.RegionId.ToString() + "</regionID>");
                response.Append("<regionName>" + region.RegionName + "</regionName>");
                response.Append("</region>");
            }
            response.Append("</regions>");
                       
            response.Append("</init>");

            AJAX a = new AJAX(response.ToString(), Response);
            a.Write();
            Response.End();
            return;
        }

        private void JSSearchCompanies()
        {
            string searchType = Request.Params["SearchType"] != null ? Request.Params["SearchType"] : "";
            string searchText = Request.Params["SearchText"] != null ? Request.Params["SearchText"] : "";
            string response = CompanyUtil.CompanySelector_SearchCompanies(searchType, searchText, 50, CurrentUser);

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
            return;
        }

        private void JSCheckIsBulstatAndCompanyName()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string bulstat = Request.Params["Bulstat"];
            string companyName = Request.Params["CompanyName"];

            string stat = "";
            string response = "";

            try
            {
                int isValidBulstat = 1;
                int isAlreadyOccupiedCompanyName = 0;

                /*След като класификатора на фирмите ще съдържа и списък на хора (физически лица собственици на фирми), не е правилно да проверява за уникалност на името. Нормално е да има няколко човека с едно и също име.
                Company company = CompanyUtil.GetCompanyByCompanyName(companyName, CurrentUser);

                if (company != null)
                {   
                    isAlreadyOccupiedCompanyName = 1;                    
                }
                */ 

                if (!String.IsNullOrEmpty(bulstat) && !(CompanyUtil.IsValidUnifiedIdentityNumber(bulstat, CurrentUser) || PersonUtil.IsValidIdentityNumber(bulstat, CurrentUser)))
                {
                    isValidBulstat = 0;
                }


                stat = AJAXTools.OK;

                response = @"
                    <isValidBulstat>" + isValidBulstat.ToString() + @"</isValidBulstat>
                    <isAlreadyOccupiedCompanyName>" + isAlreadyOccupiedCompanyName.ToString() + @"</isAlreadyOccupiedCompanyName>
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

        private void JSSaveCompany()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT") != UIAccessLevel.Enabled
                || GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Enabled
                || GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int companyId = int.Parse(Request.Form["CompanyId"]);

            string bulstat = Request.Form["Bulstat"];
            string companyName = Request.Form["CompanyName"];
            string phone = Request.Form["Phone"];

            int ownershipTypeId = int.Parse(Request.Form["OwnershipTypeId"]);
            int? administrationId = Request.Form["AdministrationId"].ToString() != ListItems.GetOptionChooseOne().Value ? (int?)int.Parse(Request.Form["AdministrationId"]) : null;

            int? cityId = null;
            if (!String.IsNullOrEmpty(Request.Form["CityID"]) &&
                Request.Form["CityID"] != ListItems.GetOptionChooseOne().Value)
            {
                cityId = int.Parse(Request.Form["CityID"]);
            }

            string postCode = Request.Form["SecondPostCode"];
            string address = Request.Form["Address"];

            int? districtId = null;
            if (!String.IsNullOrEmpty(Request.Form["DistrictID"]) &&
                Request.Form["DistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                districtId = int.Parse(Request.Form["DistrictID"]);
            }

            Company company = new Company(CurrentUser);

            company.CompanyId = companyId;
            company.OwnershipTypeId = ownershipTypeId;
            company.AdministrationId = administrationId;
            company.UnifiedIdentityCode = bulstat;
            company.CompanyName = companyName;
            company.Phone = phone;

            company.CityId = cityId;
            company.Address = address;
            company.DistrictId = districtId;
            company.PostCode = postCode;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_MilitaryReportPersons");


                //Save the changes in table Person
                if (company.CompanyId == 0)
                {
                    CompanyUtil.SaveCompany(company, CurrentUser, change);
                }
                else
                {
                    CompanyUtil.SaveCompany(company, CurrentUser, change);
                }

                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <company>
                                <companyID>" + AJAXTools.EncodeForXML(company.CompanyId.ToString()) + @"</companyID>
                                <companyName>" + AJAXTools.EncodeForXML(company.CompanyName) + @"</companyName>
                                <companyUnifiedIdentityCode>" + AJAXTools.EncodeForXML(company.UnifiedIdentityCode) + @"</companyUnifiedIdentityCode>
                                <companyOwneshipType>" + AJAXTools.EncodeForXML(company.OwnershipType.OwnershipTypeName) + @"</companyOwneshipType>
                             </company>";


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
}
