using System;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist_EducationWork : RESPage
    {
        public override string PageUIKey
        {
            get
            {
                return "RES_HUMANRES";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePersonWorkPlaceData")
            {
                JSSavePersonWorkPlaceData();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadCivilEducations")
            {
                JSLoadCivilEducations();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCivilEducation")
            {
                JSSaveCivilEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadCivilEducation")
            {
                JSLoadCivilEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCivilEducation")
            {
                JSDeleteCivilEducation();
                return;
            }


            // -------------Method for Initiali loaded Tab and LightBox MilitaryEducations and  MilitaryEducationAcademy -------------

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilitaryEduAndMilitaryEduAcademy")
            {
                LoadMilitaryEduAndMilitaryEduAcademy();
                return;
            }

            //JS request for Tab  LightBox MilitaryEducations
            //Use this to get data for this person and bind it in LightBox when it is in edit mode
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilitaryEducation")
            {
                JSLoadMilitaryEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilitaryEducation")
            {
                JSSaveMilitaryEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilitaryEducation")
            {
                JSDeleteMilitaryEducation();
                return;
            }


            //JS request for Tab  LightBox MilitaryEducations

            //Use this to get data for this person and bind it in LightBox when it is in edit mode
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilitaryEducationAcademy")
            {
                JSLoadMilitaryEducationAcademy();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilitaryEducationAcademy")
            {
                JSSaveMilitaryEducationAcademy();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilitaryEducationAcademy")
            {
                JSDeleteMilitaryEducationAcademy();
                return;
            }

            // -------------Method for Initiali loaded Tab and LightBox TrainingCource -------------

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTrainigCources")
            {
                JSLoadMilitaryTrainingCources();
                return;
            }

            //JS request for Tab  LightBox TrainingCource
            //Use this to get data for this person and bind it in LightBox when it is in edit mode
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTrainigCource")
            {
                JSLoadMilitaryTrainingCource();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveTrainigCource")
            {
                JSSaveMilitaryTrainingCource();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteTrainigCource")
            {
                JSDeleteMilitaryTrainingCource();
                return;
            }




            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadForeignLanguages")
            {
                JSLoadForeignLanguages();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveForeignLanguage")
            {
                JSSaveForeignLanguage();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadForeignLanguage")
            {
                JSLoadForeignLanguage();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteForeignLanguage")
            {
                JSDeleteForeignLanguage();
                return;
            }


            //5. ScientificTitle

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadScientificTitles")
            {
                JSLoadScientificTitles();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveScientificTitle")
            {
                JSSaveScientificTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadScientificTitle")
            {
                JSLoadScientificTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteScientificTitle")
            {
                JSDeleteScientificTitle();
                return;
            }

            //Specialities
            
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadSpecialities")
            {
                JSLoadSpecialities();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveSpeciality")
            {
                JSSaveSpeciality();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadSpeciality")
            {
                JSLoadSpeciality();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteSpeciality")
            {
                JSDeleteSpeciality();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateSpeciality")
            {
                JSRepopulateSpeciality();
                return;
            }
          
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetNKPDByCode")
            {
                JSGetNKPDByCode();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetWorkplaceInfo")
            {
                JSGetWorkplaceInfo();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateSearchNKPDLightBoxFilter")
            {
                JSPopulateSearchNKPDLightBoxFilter();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateNKPDLevelOptionsOnChange")
            {
                JSRepopulateNKPDLevelOptionsOnChange();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSearchNKPD")
            {
                JSSearchNKPD();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadVitoshaMilRepSpecs")
            {
                JSLoadVitoshaMilRepSpecs();
                return;
            }
        }

        private void JSGetNKPDByCode()
        {
            string response = "";
            string stat = "";
            string message = "";
            string nkpdid = "";

            try
            {
                string NKPDCode = Request.Form["NKPDCode"];
                NKPDCode = NKPDCode.Replace(" ", "");

                NKPD nkpd = NKPDUtil.GetNKPDByCode(NKPDCode, CurrentUser);
                if (nkpd == null || !nkpd.IsActive)
                {
                    message = "Невалиден код";
                }
                else
                {
                    message = nkpd.Name;
                    nkpdid = nkpd.Id.ToString();
                }

                stat = AJAXTools.OK;

                response = "<message>" + AJAXTools.EncodeForXML(message) + @"</message>" +
                           "<nkpdid>" + AJAXTools.EncodeForXML(nkpdid) + @"</nkpdid>";
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

        private void JSGetWorkplaceInfo()
        {
            string response = "";
            string stat = "";
            string companyInfo = "";
            string nkpdInfo = "";

            try
            {
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                if (reservist.Person.WorkCompany != null)
                    companyInfo = reservist.Person.WorkCompany.UnifiedIdentityCode + " " + reservist.Person.WorkCompany.CompanyName;

                if (reservist.Person.WorkPositionNKPDId != null)
                    nkpdInfo = reservist.Person.WorkPositionNKPD.ClassAndCodeAndNameDisplay;

                stat = AJAXTools.OK;

                response = "<companyInfo>" + AJAXTools.EncodeForXML(companyInfo) + @"</companyInfo>" +
                           "<nkpdInfo>" + AJAXTools.EncodeForXML(nkpdInfo) + @"</nkpdInfo>";
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

        //Save Person workplace data (ajax call)
        private void JSSavePersonWorkPlaceData()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden 
                || GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden 
                || GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden 
                || GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int reservistId = 0;
            int.TryParse(Request.Params["ReservistId"], out reservistId);

            int compID = 0;
            int.TryParse(Request.Params["compid"], out compID);

            int nkpdId = 0;
            int.TryParse(Request.Params["NKPDId"], out nkpdId);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Reservists");

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);
                if (reservist == null)
                {
                    throw new Exception("Невалиден резервист");
                }

                Company company = CompanyUtil.GetCompany(compID, CurrentUser);

                string message = "";
                if (reservist.Person.WorkCompanyId == null && reservist.Person.WorkPositionNKPDId == null)
                {
                    message = "Данните за местоработата са добавени";
                }
                else
                {
                    message = "Данните за местоработата са обновени";
                }

                reservist.Person.WorkCompanyId = (company != null ? company.CompanyId : (int?)null);
                reservist.Person.WorkPositionNKPDId = (nkpdId > 0 ? nkpdId : (int?)null);
                PersonUtil.SavePersonWorkData(reservist.Person, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;

                response = "<message>" + AJAXTools.EncodeForXML(message) + @"</message>";
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

        //Load Civil Educations table and light-box (ajax call)
        private void JSLoadCivilEducations()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetCivilEducationsTable(reservistId);
                string lightBoxHTML = GetCivilEducationLightBox();
                string UIItems = GetCivilEducationUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
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

        //Save a particular civil education (ajax call)
        private void JSSaveCivilEducation()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int civilEducationId = int.Parse(Request.Form["CivilEducationId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                string personEducationCode = Request.Form["PersonEducationCode"];
                string personSchoolSubjectCode = Request.Form["PersonSchoolSubjectCode"];
                int graduateYear = int.Parse(Request.Form["GraduateYear"]);
                string learningMethodKey = Request.Form["LearningMethodKey"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonCivilEducation existingPersonCivilEducation = PersonCivilEducationUtil.GetPersonCivilEducation(reservist.Person.IdentNumber, personEducationCode, graduateYear, CurrentUser);

                if (existingPersonCivilEducation != null &&
                    existingPersonCivilEducation.CivilEducationId != civilEducationId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраната образователна степен вече е въведена за избраната година на завършване</status>";
                }
                else
                {
                    PersonCivilEducation personCivilEducation = new PersonCivilEducation(CurrentUser);

                    personCivilEducation.CivilEducationId = civilEducationId;
                    personCivilEducation.PersonEducationCode = personEducationCode;
                    personCivilEducation.PersonSchoolSubjectCode = personSchoolSubjectCode;
                    personCivilEducation.GraduateYear = graduateYear;
                    personCivilEducation.LearningMethod = LearningMethodUtil.GetLearningMethod(CurrentUser, learningMethodKey);

                    PersonCivilEducationUtil.SavePersonCivilEducation(personCivilEducation, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedCivilEducationTable = GetCivilEducationsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedCivilEducationTable>" + AJAXTools.EncodeForXML(refreshedCivilEducationTable) + @"</refreshedCivilEducationTable>";
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
        private void JSLoadCivilEducation()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU") == UIAccessLevel.Hidden
                                      )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int civilEducationId = int.Parse(Request.Params["CivilEducationId"]);

                PersonCivilEducation civilEducation = PersonCivilEducationUtil.GetPersonCivilEducation(civilEducationId, CurrentUser);

                stat = AJAXTools.OK;
                response = @"<personCivilEducation>
                                <educationCode>" + AJAXTools.EncodeForXML(civilEducation.PersonEducationCode) + @"</educationCode>
                                <schoolSubjectCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(civilEducation.PersonSchoolSubjectCode) ? civilEducation.PersonSchoolSubjectCode : "") + @"</schoolSubjectCode>
                                <schoolSubjectName>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(civilEducation.PersonSchoolSubjectCode) ? civilEducation.PersonSchoolSubject.PersonSchoolSubjectName : "") + @"</schoolSubjectName>
                                <graduateYear>" + AJAXTools.EncodeForXML(civilEducation.GraduateYear.ToString()) + @"</graduateYear>
                                <learningMethodKey>" + AJAXTools.EncodeForXML(civilEducation.LearningMethod.LearningMethodKey) + @"</learningMethodKey>
                             </personCivilEducation>";
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

        private void JSPopulateSearchNKPDLightBoxFilter()
        {
            string stat = "";
            string response = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                
                for (int i = 1; i <= 4; i++)
                {
                    SearchNKPDFilter filter = new SearchNKPDFilter();
                    filter.Level = i;
                    filter.ParentIDs = "";

                    List<NKPD> NKPDs = NKPDUtil.GetNKPDListForSearch(filter, CurrentUser);

                    sb.Append("<nkpdLevel_" + i.ToString() + ">");

                    sb.Append("<n>");
                    sb.Append("<id>" + ListItems.GetOptionAll().Value + "</id>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionAll().Text) + "</name>");
                    sb.Append("</n>");

                    foreach (NKPD nkpd in NKPDs)
                    {
                        sb.Append("<n>");
                        sb.Append("<id>" + nkpd.Id.ToString() + "</id>");
                        sb.Append("<name>" + AJAXTools.EncodeForXML(nkpd.Code + " " + nkpd.Name) + "</name>");
                        sb.Append("</n>");
                    }

                    sb.Append("</nkpdLevel_" + i.ToString() + ">");
                }

                stat = AJAXTools.OK;
                response = sb.ToString();
              
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

        private void JSRepopulateNKPDLevelOptionsOnChange()
        {
            string stat = "";
            string response = "";

            try
            {
                string parentIDs = Request.Form["ParentIDs"];
                int level = int.Parse(Request.Form["Level"]);

                StringBuilder sb = new StringBuilder();

                for (int i = level + 1; i <= 4; i++)
                {
                    SearchNKPDFilter filter = new SearchNKPDFilter();
                    filter.Level = i;
                    filter.ParentIDs = parentIDs;

                    List<NKPD> NKPDs = NKPDUtil.GetNKPDListForSearch(filter, CurrentUser);

                    sb.Append("<nkpdLevel_" + i.ToString() + ">");

                    sb.Append("<n>");
                    sb.Append("<id>" + ListItems.GetOptionAll().Value + "</id>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionAll().Text) + "</name>");
                    sb.Append("</n>");

                    foreach (NKPD nkpd in NKPDs)
                    {
                        sb.Append("<n>");
                        sb.Append("<id>" + nkpd.Id.ToString() + "</id>");
                        sb.Append("<name>" + AJAXTools.EncodeForXML(nkpd.Code + " " + nkpd.Name) + "</name>");
                        sb.Append("</n>");
                    }

                    sb.Append("</nkpdLevel_" + i.ToString() + ">");
                }

                stat = AJAXTools.OK;
                response = sb.ToString();

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

        private void JSSearchNKPD()
        {
            string stat = "";
            string response = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                string parentIDs = Request.Form["ParentIDs"];
                string nkpdCode = Request.Form["NKPDCode"];
                string nkpdName = Request.Form["NKPDName"];

                SearchNKPDFilter filter = new SearchNKPDFilter();
                filter.ParentIDs = parentIDs;
                filter.NKPDCode = nkpdCode;
                filter.NKPDName = nkpdName;

                List<NKPD> NKPDs = NKPDUtil.GetNKPDListForSearch(filter, CurrentUser);

                foreach (NKPD nkpd in NKPDs)
                {
                    sb.Append("<n>");
                    sb.Append("<id>" + nkpd.Id.ToString() + "</id>");
                    sb.Append("<code>" + AJAXTools.EncodeForXML(nkpd.CodeDisplay) + "</code>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(nkpd.Name) + "</name>");
                    sb.Append("</n>");
                }

                stat = AJAXTools.OK;
                response = sb.ToString();

                //This is just to prevent JS errors if there is no response
                if (response == "")
                    response = AJAXTools.OK;

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

        private void JSLoadVitoshaMilRepSpecs()
        {
            string stat = "";
            string response = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                string vitoshaMilRepSpecTypeID = Request.Form["VitoshaMilRepSpecTypeID"];

                foreach (VitoshaMilitaryReportSpeciality vmrs in VitoshaMilitaryReportSpecialityUtil.GetVitoshaMilitaryReportSpecialitysByType(vitoshaMilRepSpecTypeID, CurrentUser))
                {
                    sb.Append("<m>");
                    sb.Append("<code>" + vmrs.VitoshaMilReportingSpecialityCode.ToString() + "</code>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(vmrs.CodeAndName) + "</name>");
                    sb.Append("</m>");
                }

                stat = AJAXTools.OK;
                response = sb.ToString();

                //This is just to prevent JS errors if there is no response
                if (response == "")
                    response = AJAXTools.OK;

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

        //Delete a particular civil education (ajax call)
        private void JSDeleteCivilEducation()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int civilEducationId = int.Parse(Request.Params["CivilEducationId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonCivilEducationUtil.DeletePersonCivilEducation(civilEducationId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedCivilEducationTable = GetCivilEducationsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedCivilEducationTable>" + AJAXTools.EncodeForXML(refreshedCivilEducationTable) + @"</refreshedCivilEducationTable>";
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

        //Render the CivilEducations table
        public string GetCivilEducationsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsEducationHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_EDU") == UIAccessLevel.Hidden;
            bool IsSubjectHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_SUBJECT") == UIAccessLevel.Hidden;
            bool IsYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_GREDYEAR") == UIAccessLevel.Hidden;
            bool IsLearningMethodHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_SCOOLSUBJECT") == UIAccessLevel.Hidden;

            if (IsEducationHidden &&
                IsSubjectHidden &&
                IsYearHidden &&
                IsLearningMethodHidden
                )
            {
                return "";
            }

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU") == UIAccessLevel.Enabled && !isPreview
                                      );

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewCivilEducation();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
       " + (!IsEducationHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Образователна степен</th>" : "") + @"
         " + (!IsSubjectHidden ? @"<th style='width: 230px; vertical-align: bottom;'>Специалност</th>" : "") + @"                    
            " + (!IsYearHidden ? @"<th style='width: 100px; vertical-align: bottom;'>Година на завършване</th>" : "") + @"
  " + (!IsLearningMethodHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Начин на обучение</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonCivilEducation> civilEducations = PersonCivilEducationUtil.GetAllPersonCivilEducationsByPersonID(reservist.PersonId, CurrentUser);


            foreach (PersonCivilEducation civilEducation in civilEducations)
            {
                counter++;

                string deleteHTML = "";

                if (civilEducation.CanDelete)
                {
                    if (notHideDelEditHTML)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteCivilEducation(" + civilEducation.CivilEducationId.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (notHideDelEditHTML)
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditCivilEducation(" + civilEducation.CivilEducationId.ToString() + ");' />";
                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsEducationHidden ? @"<td style='text-align: left;'>" + (civilEducation.PersonEducation != null ? civilEducation.PersonEducation.PersonEducationName : "") + @"</td>" : "") + @"
          " + (!IsSubjectHidden ? @"<td style='text-align: left;'>" + (civilEducation.PersonSchoolSubject != null ? civilEducation.PersonSchoolSubject.PersonSchoolSubjectName.ToString() : "") + @"</td>" : "") + @"                    
             " + (!IsYearHidden ? @"<td style='text-align: left;'>" + civilEducation.GraduateYear.ToString() + @"</td>" : "") + @"
   " + (!IsLearningMethodHidden ? @"<td style='text-align: left;'>" + (civilEducation.LearningMethod != null ? civilEducation.LearningMethod.LearningMethodName.ToString() : "") + @"</td>" : "") + @"
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the CivilEducations light-box
        public string GetCivilEducationLightBox()
        {
            List<PersonEducation> listPersonEducation = PersonEducationUtil.GetAllPersonEducations(CurrentUser);
            List<IDropDownItem> ddiPersonEducation = new List<IDropDownItem>();

            foreach (PersonEducation personEducation in listPersonEducation)
            {
                ddiPersonEducation.Add(personEducation);
            }

            // Generates html for drop down list
            string personEducationHTML = ListItems.GetDropDownHtml(ddiPersonEducation, null, "ddCivilEduPersonEducation", true, null, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            List<LearningMethod> listLearningMethod = LearningMethodUtil.GetLearningMethods(CurrentUser);
            List<IDropDownItem> ddiLearningMethod = new List<IDropDownItem>();

            foreach (LearningMethod learningMethod in listLearningMethod)
            {
                ddiLearningMethod.Add(learningMethod);
            }

            // Generates html for drop down list
            string learningMethodHTML = ListItems.GetDropDownHtml(ddiLearningMethod, null, "ddCivilEduLearningMethod", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            string html = @"
<center>
    <input type=""hidden"" id=""hdnCivilEducationID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditCivilEducationTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCivilEduPersonEducation"" class=""InputLabel"">Образователна степен:</span>
            </td>
            <td style=""text-align: left;"">
                " + personEducationHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;vertical-align: top;"">
                <span id=""lblCivilEduPersonChoolSubject"" class=""InputLabel"">Специалност:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""hidden"" id=""hdnSchoolSubjectCode"" />
                <input type=""hidden"" id=""hdnSchoolSubjectName"" />
                <table>
                    <tr>
                        <td style=""text-align: bottom;"">
                            <div id=""txtSubject"" class=""ReadOnlyValue"" style=""background-color:#FFFFCC;width: 300px;min-height:15px;""/>
                        </td>
                        <td style=""vertical-align: top;"">
                            <input id=""btnSelectCivilSubject""
                                   onclick='civilEducationSelector.showDialog(""civilEducationSelectorForPerson"", CivilEducationSelector_OnSelectedCivilEducation);' 
                                   type=""button"" value=""Търсене"" class=""OpenCompanySelectorButton"" style=""margin-top:0px"" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCivilEduGraduateYear"" class=""InputLabel"">Година на завършване:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtCivilEduGraduateYear"" maxlength=""4"" class=""RequiredInputField"" style=""width: 50px;"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCivilEduLearningMethod"" class=""InputLabel"">Начин на обучение:</span>
            </td>
            <td style=""text-align: left;"">
                " + learningMethodHTML + @"
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditCivilEducationLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditCivilEducationLightBox"" style=""display: inline;"" onclick=""SaveAddEditCivilEducationLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditCivilEducationLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditCivilEducationLightBox"" style=""display: inline;"" onclick=""HideAddEditCivilEducationLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditCivilEducationLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the CivilEducation table
        public string GetCivilEducationUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_EDU");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduPersonEducation");
                disabledClientControls.Add("ddCivilEduPersonEducation");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduPersonEducation");
                hiddenClientControls.Add("ddCivilEduPersonEducation");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_SUBJECT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduPersonChoolSubject");
                disabledClientControls.Add("txtSubject");
                hiddenClientControls.Add("btnSelectCivilSubject");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduPersonChoolSubject");
                hiddenClientControls.Add("txtSubject");
                hiddenClientControls.Add("btnSelectCivilSubject");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_GREDYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduGraduateYear");
                disabledClientControls.Add("txtCivilEduGraduateYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduGraduateYear");
                hiddenClientControls.Add("txtCivilEduGraduateYear");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU_SCOOLSUBJECT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduLearningMethod");
                disabledClientControls.Add("ddCivilEduLearningMethod");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduLearningMethod");
                hiddenClientControls.Add("ddCivilEduLearningMethod");
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

        //Load Military Educations and MilitaryEducationAcademys table and light-box (ajax call)
        private void LoadMilitaryEduAndMilitaryEduAcademy()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                     (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU") == UIAccessLevel.Hidden
                                   && GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA") == UIAccessLevel.Hidden)
                                     )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableMilitaryEduHTML = GetMilitaryEducationsTable(reservistId);
                string lightBoxMilitaryEduHTML = GetMilitaryEducationLightBox();
                string UIItemsMilitaryEdu = GetMilitaryEducationUIItems();

                string tableMilitaryEduAcademyHTML = GetMilitaryEducationAcademysTable(reservistId);
                string lightMilitaryEduAcademyBoxHTML = GetMilitaryEducationAcademyLightBox();
                string UIItemsMilitaryEduAcademy = GetMilitaryEducationAcademyUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableMilitaryEduHTML>" + AJAXTools.EncodeForXML(tableMilitaryEduHTML) + @"</tableMilitaryEduHTML>
                    <lightBoxMilitaryEduHTML>" + AJAXTools.EncodeForXML(lightBoxMilitaryEduHTML) + @"</lightBoxMilitaryEduHTML>
                    <tableMilitaryEduAcademyHTML>" + AJAXTools.EncodeForXML(tableMilitaryEduAcademyHTML) + @"</tableMilitaryEduAcademyHTML>
                    <lightMilitaryEduAcademyBoxHTML>" + AJAXTools.EncodeForXML(lightMilitaryEduAcademyBoxHTML) + @"</lightMilitaryEduAcademyBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItemsMilitaryEdu))
                {
                    response += "<UIItemsMilitaryEdu>" + UIItemsMilitaryEdu + "</UIItemsMilitaryEdu>";
                }

                if (!String.IsNullOrEmpty(UIItemsMilitaryEduAcademy))
                {
                    response += "<UIItemsMilitaryEduAcademy>" + UIItemsMilitaryEduAcademy + "</UIItemsMilitaryEduAcademy>";
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

        //Load a particular MilitaryEducation (ajax call)
        private void JSLoadMilitaryEducation()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU") == UIAccessLevel.Hidden
                                      )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int militaryEducationId = int.Parse(Request.Params["MilitaryEducationId"]);

               

                PersonMilitaryEducation personMilitaryEducation = PersonMilitaryEducationUtil.GetPersonMilitaryEducation(militaryEducationId, CurrentUser);
                stat = AJAXTools.OK;

                string vitoshaMilitaryReportSpecialityOptions = "";
                StringBuilder sb = new StringBuilder();

                foreach (VitoshaMilitaryReportSpeciality vmrs in VitoshaMilitaryReportSpecialityUtil.GetVitoshaMilitaryReportSpecialitysByType((personMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducation.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : "") , CurrentUser))
                {
                    sb.Append("<vitoshaMilRepSpecOp>");
                    sb.Append("<code>" + vmrs.VitoshaMilReportingSpecialityCode.ToString() + "</code>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(vmrs.CodeAndName) + "</name>");
                    sb.Append("</vitoshaMilRepSpecOp>");
                }

                vitoshaMilitaryReportSpecialityOptions = sb.ToString();

                response = @"<personMilitaryEducation>
                                <militarySchoolId>" + AJAXTools.EncodeForXML(personMilitaryEducation.MilitarySchoolId > 0 ? personMilitaryEducation.MilitarySchoolId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</militarySchoolId>
                                <militaryEducationTypeCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducation.MilitaryEducationTypeCode) ? personMilitaryEducation.MilitaryEducationTypeCode : ListItems.GetOptionChooseOne().Value) + @"</militaryEducationTypeCode>
                                <militarySchoolSubjectId>" + AJAXTools.EncodeForXML(personMilitaryEducation.MilitarySchoolSubjectId > 0 ? personMilitaryEducation.MilitarySchoolSubjectId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</militarySchoolSubjectId>
                                <militaryArmsCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducation.MilitaryArmsCode) ? personMilitaryEducation.MilitaryArmsCode : ListItems.GetOptionChooseOne().Value) + @"</militaryArmsCode>
                                <countryId>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducation.CountryCode) ? personMilitaryEducation.CountryCode.ToString() : ListItems.GetOptionChooseOne().Value) + @"</countryId>
                                <graduateYear>" + AJAXTools.EncodeForXML(personMilitaryEducation.GraduateYear.ToString()) + @"</graduateYear>
                                <learningMethodKey>" + AJAXTools.EncodeForXML(personMilitaryEducation.LearningMethod.LearningMethodKey) + @"</learningMethodKey>
                                <vitoshaMilitaryReportSpecialityTypeCode>" + (personMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducation.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : "") + @"</vitoshaMilitaryReportSpecialityTypeCode> 
                                <vitoshaMilitaryReportSpecialityCode>" + (!string.IsNullOrEmpty(personMilitaryEducation.VitoshaMilitaryReportSpecialityCode) ? personMilitaryEducation.VitoshaMilitaryReportSpecialityCode : "") + @"</vitoshaMilitaryReportSpecialityCode>
                                <vitoshaMilitaryReportSpecialityOptions>" + vitoshaMilitaryReportSpecialityOptions + @"</vitoshaMilitaryReportSpecialityOptions>
                             </personMilitaryEducation>";
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

        //Render the MilitaryEducations table
        public string GetMilitaryEducationsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsMilitaryScoolHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTSCOOL") == UIAccessLevel.Hidden;
            bool IsMilitaryEducationTypeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTEDUTYPE") == UIAccessLevel.Hidden;
            bool IsMilitarySubjectHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTSCOOLSUBJECT") == UIAccessLevel.Hidden;
            bool IsArmsHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_ARMSCODE") == UIAccessLevel.Hidden;
            bool IsGraduateYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_GRADYEAR") == UIAccessLevel.Hidden;
            bool IsCountryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_COUNTRY") == UIAccessLevel.Hidden;
            bool IsLearningMethodHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_LRNMETHOD") == UIAccessLevel.Hidden;
            bool IsVitoshaMilitaryReportSpecialityHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_VITOSHAMILITARYREPORTSPECIALITY") == UIAccessLevel.Hidden;

            if (IsMilitaryScoolHidden &&
               IsMilitaryEducationTypeHidden &&
               IsMilitarySubjectHidden &&
               IsArmsHidden &&
               IsGraduateYearHidden &&
               IsCountryHidden &&
               IsLearningMethodHidden &&
               IsVitoshaMilitaryReportSpecialityHidden
               )
            {
                return "";
            }

            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU") == UIAccessLevel.Hidden)
            {
                return "";
            }
            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU") == UIAccessLevel.Enabled && !isPreview
                                      );

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewMilitaryEducation();' />";


            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
       " + (!IsMilitaryScoolHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Военно училище</th>" : "") + @"
         " + (!IsMilitaryEducationTypeHidden ? @"<th style='width: 230px; vertical-align: bottom;'>Вид на военното образование</th>" : "") + @"                    
" + (!IsMilitarySubjectHidden ? @"<th style='width: 230px; vertical-align: bottom;'>Военна специалност</th>" : "") + @"                    
            " + (!IsArmsHidden ? @"<th style='width: 130px; vertical-align: bottom;'>Род войска</th>" : "") + @"
  " + (!IsGraduateYearHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Година на завършване</th>" : "") + @"
            " + (!IsCountryHidden ? @"<th style='width: 100px; vertical-align: bottom;'>Държава</th>" : "") + @"
  " + (!IsLearningMethodHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Начин на обучение</th>" : "") + @"
  " + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<th style='width: 120px; vertical-align: bottom;'>ВОС</th>" : "") + @"
                                   <th style='width: 70px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;


            List<PersonMilitaryEducation> listPersonMilitaryEducations = PersonMilitaryEducationUtil.GetAllPersonMilitaryEducationsByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonMilitaryEducation personMilitaryEducation in listPersonMilitaryEducations)
            {
                counter++;

                string deleteHTML = "";

                if (personMilitaryEducation.CanDelete)
                {
                    if (notHideDelEditHTML)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteMilitaryEducation(" + personMilitaryEducation.MilitaryEducationId.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (notHideDelEditHTML)
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditMilitaryEducation(" + personMilitaryEducation.MilitaryEducationId.ToString() + ");' />";
                }


                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsMilitaryScoolHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.MilitarySchool != null ? personMilitaryEducation.MilitarySchool.MilitarySchoolName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsMilitaryEducationTypeHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.MilitaryEducationType != null ? personMilitaryEducation.MilitaryEducationType.MilitaryEducationTypeName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsMilitarySubjectHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.MilitarySchoolSubject != null ? personMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsArmsHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.MilitaryArms != null ? personMilitaryEducation.MilitaryArms.MilitaryArmsName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsGraduateYearHidden ? @"<td style='text-align: left;'>" + personMilitaryEducation.GraduateYear.ToString() + @"</td>" : "") + @"
        " + (!IsCountryHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.Country != null ? personMilitaryEducation.Country.CountryName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsLearningMethodHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.LearningMethod != null ? personMilitaryEducation.LearningMethod.LearningMethodName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducation.VitoshaMilitaryReportSpeciality.CodeAndName : "") + @"</td>" : "") + @"

<td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the MilitaryEducations light-box
        public string GetMilitaryEducationLightBox()
        {
            //1. Bind Dropdown List for MilitarySchools
            List<MilitarySchool> listMilitarySchool = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, false);
            List<IDropDownItem> ddMilitarySchool = new List<IDropDownItem>();

            foreach (MilitarySchool militarySchool in listMilitarySchool)
            {
                ddMilitarySchool.Add(militarySchool);
            }
            // Generates html for drop down list
            string personMilitarySchoolHTML = ListItems.GetDropDownHtml(ddMilitarySchool, null, "ddPersonMilitaryEducationMilitarySchool", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            //2. Bind Dropdown List for MilitaryEducationType
            List<MilitaryEducationType> listMilitaryEducationType = MilitaryEducationTypeUtil.GetAllMilitaryEducationTypes(CurrentUser);
            List<IDropDownItem> ddMilitaryEducationType = new List<IDropDownItem>();

            foreach (MilitaryEducationType militaryEducationType in listMilitaryEducationType)
            {
                ddMilitaryEducationType.Add(militaryEducationType);
            }
            // Generates html for drop down list
            string personMilitaryEducationTypeHTML = ListItems.GetDropDownHtml(ddMilitaryEducationType, null, "ddPersonMilitaryEducationMilitaryEducationType", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");


            //3. Bind Dropdown List for MilitarySchoolSubject
            List<MilitarySchoolSubject> listMilitarySchoolSubject = MilitarySchoolSubjectUtil.GetAllMilitarySchoolSubjects(CurrentUser);
            List<IDropDownItem> ddMilitarySchoolSubject = new List<IDropDownItem>();

            foreach (MilitarySchoolSubject militarySchoolSubject in listMilitarySchoolSubject)
            {
                DropDownItem ddi = new DropDownItem();
                ddi.Txt = militarySchoolSubject.MilitarySchoolSubjectName;
                ddi.Val = militarySchoolSubject.MilitarySchoolSubjectId.ToString();
                ddMilitarySchoolSubject.Add(ddi);
            }
            // Generates html for drop down list
            string personMilitarySchoolSubjectHTML = ListItems.GetDropDownHtml(ddMilitarySchoolSubject, null, "ddPersonMilitaryEducationMilitarySchoolSubject", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            //4. Bind Dropdown List for MilitaryArms
            List<MilitaryArms> listMilitaryArms = MilitaryArmsUtil.GetAllMilitaryArms(CurrentUser);
            List<IDropDownItem> ddMilitaryArms = new List<IDropDownItem>();

            foreach (MilitaryArms militaryArms in listMilitaryArms)
            {
                ddMilitaryArms.Add(militaryArms);
            }
            // Generates html for drop down list
            string personMilitaryArmsHTML = ListItems.GetDropDownHtml(ddMilitaryArms, null, "ddPersonMilitaryEducationMilitaryArms", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true'");

            //5. Bind Dropdown List form Country
            List<Country> listCountry = CountryUtil.GetCountries(CurrentUser);
            List<IDropDownItem> ddCountry = new List<IDropDownItem>();

            foreach (Country country in listCountry)
            {
                ddCountry.Add(country);
            }
            // Generates html for drop down list
            string personCountryHTML = ListItems.GetDropDownHtml(ddCountry, null, "ddPersonMilitaryEducationCountry", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            //6. Bind Dropdown List for LearningMethod
            List<LearningMethod> listLearningMethod = LearningMethodUtil.GetLearningMethods(CurrentUser);
            List<IDropDownItem> ddLearningMethod = new List<IDropDownItem>();

            foreach (LearningMethod learningMethod in listLearningMethod)
            {
                ddLearningMethod.Add(learningMethod);
            }

            // Generates html for drop down list
            string learningMethodHTML = ListItems.GetDropDownHtml(ddLearningMethod, null, "ddPersonMilitaryEducationLearningMethod", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            List<VitoshaMilitaryReportSpecialityType> listVitoshaMilitaryReportSpecialityType = VitoshaMilitaryReportSpecialityTypeUtil.GetAllVitoshaMilitaryReportSpecialityTypes(CurrentUser);
            List<IDropDownItem> ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType = new List<IDropDownItem>();
            foreach (VitoshaMilitaryReportSpecialityType vitoshaMilitaryReportSpecialityType in listVitoshaMilitaryReportSpecialityType)
            {
                ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType.Add(vitoshaMilitaryReportSpecialityType);
            }

            string vitoshaMilitaryReportSpecialityTypeHTML = ListItems.GetDropDownHtml(ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType, null, "ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='InputField' onchange='VitoshaMilRepSpecTypeLightBoxChanged(\"ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType\", \"ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality\");'");

            string html = @"
<center>
    <input type=""hidden"" id=""hdnMilitaryEducationID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 45%"">
        </colgroup>
        <colgroup style=""width: 55%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilitaryEducationTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitarySchool"" class=""InputLabel"">Военно училище:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitarySchoolHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitaryEducationType"" class=""InputLabel"">Вид на военното образование:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryEducationTypeHTML + @"
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitarySchoolSubject"" class=""InputLabel"">Военна специалност:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitarySchoolSubjectHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitaryArms"" class=""InputLabel"">Род войска:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryArmsHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationCountry""  class=""InputLabel"">Държава:</span>
            </td>
            <td style=""text-align: left;"">
                " + personCountryHTML + @"
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationGraduateYear"" class=""InputLabel"">Година на завършване:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryEducationGraduateYear"" UnsavedCheckSkipMe='true' maxlength=""4"" class=""RequiredInputField"" style=""width: 50px;"" />
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationLearningMethod"" class=""InputLabel"">Начин на обучение:</span>
            </td>
            <td style=""text-align: left;"">
                " + learningMethodHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationVitoshaMilitaryReportSpecialityType"" class=""InputLabel"">Тип ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + vitoshaMilitaryReportSpecialityTypeHTML + @"
            </td>
        </tr>   
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationVitoshaMilitaryReportSpeciality"" class=""InputLabel"">ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + ListItems.GetDropDownHtml(new List<IDropDownItem>(), null, "ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality", true, null, "", "style='width: 320px;' UnsavedCheckSkipMe='true' class='InputField' ") + @"
            </td>
        </tr>   
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditMilitaryEducationLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilitaryEducationLightBox"" style=""display: inline;"" onclick=""SaveAddEditMilitaryEducationLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilitaryEducationLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMilitaryEducationLightBox"" style=""display: inline;"" onclick=""HideAddEditMilitaryEducationLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddMilitaryEducationLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the MilitaryEducation table
        public string GetMilitaryEducationUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTSCOOL");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitarySchool");
                disabledClientControls.Add("ddPersonMilitaryEducationMilitarySchool");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitarySchool");
                hiddenClientControls.Add("ddPersonMilitaryEducationMilitarySchool");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTEDUTYPE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitaryEducationType");
                disabledClientControls.Add("ddPersonMilitaryEducationMilitaryEducationType");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitaryEducationType");
                hiddenClientControls.Add("ddPersonMilitaryEducationMilitaryEducationType");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_MLTSCOOLSUBJECT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitarySchoolSubject");
                disabledClientControls.Add("ddPersonMilitaryEducationMilitarySchoolSubject");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitarySchoolSubject");
                hiddenClientControls.Add("ddPersonMilitaryEducationMilitarySchoolSubject");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_ARMSCODE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitaryArms");
                disabledClientControls.Add("ddPersonMilitaryEducationMilitaryArms");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitaryArms");
                hiddenClientControls.Add("ddPersonMilitaryEducationMilitaryArms");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_GRADYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationGraduateYear");
                disabledClientControls.Add("txtMilitaryEducationGraduateYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationGraduateYear");
                hiddenClientControls.Add("txtMilitaryEducationGraduateYear");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_COUNTRY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationCountry");
                disabledClientControls.Add("ddPersonMilitaryEducationCountry");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationCountry");
                hiddenClientControls.Add("ddPersonMilitaryEducationCountry");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_LRNMETHOD");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationLearningMethod");
                disabledClientControls.Add("ddPersonMilitaryEducationLearningMethod");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationLearningMethod");
                hiddenClientControls.Add("ddPersonMilitaryEducationLearningMethod");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU_VITOSHAMILITARYREPORTSPECIALITY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("lblMilitaryEducationVitoshaMilitaryReportSpeciality");
                disabledClientControls.Add("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("lblMilitaryEducationVitoshaMilitaryReportSpeciality");
                hiddenClientControls.Add("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality");
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

        //Save a particular military education (ajax call)
        private void JSSaveMilitaryEducation()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int militaryEducationId = int.Parse(Request.Form["MilitaryEducationId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                int militarySchoolId = int.Parse(Request.Form["MilitarySchoolId"]);
                string militaryEducationTypeCode = Request.Form["MilitaryEducationTypeCode"];
                int militarySchoolSubjectId = int.Parse(Request.Form["MilitarySchoolSubjectId"]);
                string militaryArmsCode = Request.Form["MilitaryArmsCode"];
                string countryId = Request.Form["CountryId"];
                int graduateYear = int.Parse(Request.Form["GraduateYear"]);
                string learningMethodKey = Request.Form["LearningMethodKey"];
                string vitoshaMilitaryReportSpecialityCode = Request.Form["VitoshaMilitaryReportSpecialityCode"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryEducation personMilitaryEducation = new PersonMilitaryEducation(CurrentUser);
                personMilitaryEducation.MilitarySchoolId = militarySchoolId;

                PersonMilitaryEducation existingPersonMilitaryEducation = PersonMilitaryEducationUtil.GetPersonMilitaryEducation(reservist.Person.IdentNumber, personMilitaryEducation.MilitarySchool.MilitarySchoolCode, graduateYear, CurrentUser);

                if (existingPersonMilitaryEducation != null &&
                    existingPersonMilitaryEducation.MilitaryEducationId != militaryEducationId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраното военно училище вече е въведено за избраната година на завършване</status>";
                }
                else
                {
                    personMilitaryEducation = new PersonMilitaryEducation(CurrentUser);

                    personMilitaryEducation.MilitaryEducationId = militaryEducationId;
                    personMilitaryEducation.MilitarySchoolId = militarySchoolId;
                    personMilitaryEducation.MilitaryEducationTypeCode = militaryEducationTypeCode;
                    personMilitaryEducation.MilitarySchoolSubjectId = militarySchoolSubjectId;
                    personMilitaryEducation.MilitaryArmsCode = militaryArmsCode;
                    personMilitaryEducation.CountryCode = countryId;
                    personMilitaryEducation.GraduateYear = graduateYear;
                    personMilitaryEducation.LearningMethod = LearningMethodUtil.GetLearningMethod(CurrentUser, learningMethodKey);
                    personMilitaryEducation.VitoshaMilitaryReportSpecialityCode = vitoshaMilitaryReportSpecialityCode;

                    PersonMilitaryEducationUtil.SavePersonMilitaryEducation(personMilitaryEducation, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedCivilEducationTable = GetMilitaryEducationsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedMilitaryEducationTable>" + AJAXTools.EncodeForXML(refreshedCivilEducationTable) + @"</refreshedMilitaryEducationTable>";
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

        //Delete a particular military education (ajax call)
        private void JSDeleteMilitaryEducation()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int мilitaryEducationId = int.Parse(Request.Params["MilitaryEducationId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryEducationUtil.DeletePersonMilitaryEducation(мilitaryEducationId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedCivilEducationTable = GetMilitaryEducationsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMilitaryEducationTable>" + AJAXTools.EncodeForXML(refreshedCivilEducationTable) + @"</refreshedMilitaryEducationTable>";
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

        //Render the MilitaryEducationAcademys table
        private string GetMilitaryEducationAcademysTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsMilitaryEducationAcademyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_MILITACADEMY") == UIAccessLevel.Hidden;
            bool IsMilitaryEducationAcademyGstCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_GSTCODE") == UIAccessLevel.Hidden;
            bool IsMilitaryEducationSubjectHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_MLTSCOOLSUBJECT") == UIAccessLevel.Hidden;
            bool IsDurationYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_DURATIONYEAR") == UIAccessLevel.Hidden;
            bool IsGraduateYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_GRADYEAR") == UIAccessLevel.Hidden;
            bool IsCountryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_COUNTRY") == UIAccessLevel.Hidden;
            bool IsLearningMethodHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_LRNMETHOD") == UIAccessLevel.Hidden;
            bool IsVitoshaMilitaryReportSpecialityHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_VITOSHAMILITARYREPORTSPECIALITY") == UIAccessLevel.Hidden;
            
            if (IsMilitaryEducationAcademyHidden &&
              IsMilitaryEducationAcademyGstCodeHidden &&
              IsMilitaryEducationSubjectHidden &&
              IsDurationYearHidden &&
              IsGraduateYearHidden &&
              IsCountryHidden &&
              IsLearningMethodHidden &&
              IsVitoshaMilitaryReportSpecialityHidden)
            {
                return "";
            }

            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA") == UIAccessLevel.Hidden)
            {
                return "";
            }

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA") == UIAccessLevel.Enabled && !isPreview
               );

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewMilitaryEducationAcademy();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left; margin-top: 20px;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
       " + (!IsMilitaryEducationAcademyHidden ? @"<th style='width: 290px; vertical-align: bottom;'>Военна академия</th>" : "") + @"
         " + (!IsMilitaryEducationAcademyGstCodeHidden ? @"<th style='width: 50px; vertical-align: bottom;'>ГЩ</th>" : "") + @"                    
" + (!IsMilitaryEducationSubjectHidden ? @"<th style='width: 290px; vertical-align: bottom;'>Военна специалност</th>" : "") + @"                    
            " + (!IsDurationYearHidden ? @"<th style='width: 50px; vertical-align: bottom;'><span title='Продължителност (Години)'>Прод.(Години)</span></th>" : "") + @"
  " + (!IsGraduateYearHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Година на завършване</th>" : "") + @"
            " + (!IsCountryHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Държава</th>" : "") + @"
  " + (!IsLearningMethodHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Начин на обучение</th>" : "") + @"
  " + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<th style='width: 120px; vertical-align: bottom;'>ВОС</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonMilitaryEducationAcademy> listPersonMilitaryEducationAcademys = PersonMilitaryEducationAcademyUtil.GetAllPersonMilitaryEducationAcademysByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonMilitaryEducationAcademy personMilitaryEducationAcademy in listPersonMilitaryEducationAcademys)
            {
                counter++;

                string deleteHTML = "";

                if (personMilitaryEducationAcademy.CanDelete)
                {
                    if (notHideDelEditHTML)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteMilitaryEducationAcademy(" + personMilitaryEducationAcademy.MilitaryEducationAcademyId.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (notHideDelEditHTML)
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditMilitaryEducationAcademy(" + personMilitaryEducationAcademy.MilitaryEducationAcademyId.ToString() + ");' />";
                }

                string cheked = "";
                if (personMilitaryEducationAcademy.GstCode)
                {
                    cheked = "checked='checked'";
                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsMilitaryEducationAcademyHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducationAcademy.MilitaryAcademy != null ? personMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsMilitaryEducationAcademyGstCodeHidden ? @"<td align='center'><input type='checkbox' disabled='disabled'" + cheked + @"</input></td>" : "") + @"                    
        " + (!IsMilitaryEducationSubjectHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducationAcademy.MilitaryAcademySubject != null ? personMilitaryEducationAcademy.MilitaryAcademySubject.MilitarySubjectName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsDurationYearHidden ? @"<td style='text-align: left;'>" + personMilitaryEducationAcademy.DurationYear.ToString() + @"</td>" : "") + @"
        " + (!IsGraduateYearHidden ? @"<td style='text-align: left;'>" + personMilitaryEducationAcademy.GraduateYear.ToString() + @"</td>" : "") + @"
        " + (!IsCountryHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducationAcademy.Country != null ? personMilitaryEducationAcademy.Country.CountryName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsLearningMethodHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducationAcademy.LearningMethod != null ? personMilitaryEducationAcademy.LearningMethod.LearningMethodName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<td style='text-align: left;'>" + (personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.CodeAndName : "") + @"</td>" : "") + @"

                                
<td align='center' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;

        }

        //Render the MilitaryEducationAcademys light-box
        private string GetMilitaryEducationAcademyLightBox()
        {
            //1. Bind Dropdown List for MilitaryAcademys
            List<MilitaryAcademy> listMilitaryAcademy = MilitaryAcademyUtil.GetAllMilitaryAcademys(CurrentUser);
            List<IDropDownItem> ddMilitaryAcademy = new List<IDropDownItem>();

            foreach (MilitaryAcademy militaryAcademy in listMilitaryAcademy)
            {
                ddMilitaryAcademy.Add(militaryAcademy);
            }
            // Generates html for drop down list
            string personMilitaryAcademyHTML = ListItems.GetDropDownHtml(ddMilitaryAcademy, null, "ddPersonMilitaryEducationAcademyMilitaryAcademy", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");



            //2. Bind Dropdown List for MilitarySchoolSubject
            List<MilitarySubject> listMilitarySubject = MilitarySubjectUtil.GetAllMilitarySubjects(CurrentUser);
            List<IDropDownItem> ddMilitarySubject = new List<IDropDownItem>();

            foreach (MilitarySubject militarySubject in listMilitarySubject)
            {
                DropDownItem ddi = new DropDownItem();
                ddi.Txt = militarySubject.MilitarySubjectName;
                ddi.Val = militarySubject.MilitarySubjectCode;
                ddMilitarySubject.Add(ddi);
            }
            // Generates html for drop down list
            string personMilitaryAcademySchoolSubjectHTML = ListItems.GetDropDownHtml(ddMilitarySubject, null, "ddPersonMilitaryEducationAcademyMilitarySubject", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true'", true);


            //3. Bind Dropdown List form Country
            List<Country> listCountry = CountryUtil.GetCountries(CurrentUser);
            List<IDropDownItem> ddCountry = new List<IDropDownItem>();

            foreach (Country country in listCountry)
            {
                ddCountry.Add(country);
            }
            // Generates html for drop down list
            string personCountryHTML = ListItems.GetDropDownHtml(ddCountry, null, "ddPersonMilitaryEducationAcademyCountry", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            //4. Bind Dropdown List for LearningMethod
            List<LearningMethod> listLearningMethod = LearningMethodUtil.GetLearningMethods(CurrentUser);
            List<IDropDownItem> ddLearningMethod = new List<IDropDownItem>();

            foreach (LearningMethod learningMethod in listLearningMethod)
            {
                ddLearningMethod.Add(learningMethod);
            }

            // Generates html for drop down list
            string learningMethodHTML = ListItems.GetDropDownHtml(ddLearningMethod, null, "ddPersonMilitaryEducationAcademyLearningMethod", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            List<VitoshaMilitaryReportSpecialityType> listVitoshaMilitaryReportSpecialityType = VitoshaMilitaryReportSpecialityTypeUtil.GetAllVitoshaMilitaryReportSpecialityTypes(CurrentUser);
            List<IDropDownItem> ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType = new List<IDropDownItem>();
            foreach (VitoshaMilitaryReportSpecialityType vitoshaMilitaryReportSpecialityType in listVitoshaMilitaryReportSpecialityType)
            {
                ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType.Add(vitoshaMilitaryReportSpecialityType);
            }

            string vitoshaMilitaryReportSpecialityTypeHTML = ListItems.GetDropDownHtml(ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType, null, "ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='InputField' onchange='VitoshaMilRepSpecTypeLightBoxChanged(\"ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType\", \"ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality\");'");


            string html = @"
<center>
    <input type=""hidden"" id=""hdnMilitaryEducationAcademyID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 45%"">
        </colgroup>
        <colgroup style=""width: 55%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilitaryEducationAcademyTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyMilitarySchool"" class=""InputLabel"">Военна академия:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryAcademyHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitaryAcademyGstCode"" class=""InputLabel"">ГЩ:</span>
            </td>
            <td style=""text-align: left;""><input type='checkbox' UnsavedCheckSkipMe='true' id='chkboxGstCode'></input>
                " + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitaryAcademySchoolSubject"" class=""InputLabel"">Военна специалност:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryAcademySchoolSubjectHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyDurationYear"" class=""InputLabel"" >Продължителност (Години):</span>
            </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryEducationAcademyDurationYear"" UnsavedCheckSkipMe='true' maxlength='1' class=""RequiredInputField"" style=""width: 50px;"" />
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyGraduateYear"" class=""InputLabel"">Година на завършване:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryEducationAcademyGraduateYear"" UnsavedCheckSkipMe='true' maxlength='4' class=""RequiredInputField"" style=""width: 50px;"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationMilitaryArms""  class=""InputLabel"">Държава:</span>
            </td>
            <td style=""text-align: left;"">
                " + personCountryHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyLearningMethod"" class=""InputLabel"">Начин на обучение:</span>
            </td>
            <td style=""text-align: left;"">
                " + learningMethodHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType"" class=""InputLabel"">Тип ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + vitoshaMilitaryReportSpecialityTypeHTML + @"
            </td>
        </tr>   
         <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryEducationAcademyVitoshaMilitaryReportSpeciality"" class=""InputLabel"">ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + ListItems.GetDropDownHtml(new List<IDropDownItem>(), null, "ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality", true, null, "", "style='width: 320px;' UnsavedCheckSkipMe='true' class='InputField' ") + @"
            </td>
        </tr>   
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditMilitaryEducationAcademyLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilitaryEducationAcademyLightBox"" style=""display: inline;"" onclick=""SaveAddEditMilitaryEducationAcademyLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilitaryEducationAcademyLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMilitaryAcademyEducationLightBox"" style=""display: inline;"" onclick=""HideAddEditMilitaryEducationAcademyLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddMilitaryAcademyEducationLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the MilitaryEducationAcademy table
        public string GetMilitaryEducationAcademyUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_MILITACADEMY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationAcademyMilitarySchool");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyMilitaryAcademy");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationAcademyMilitarySchool");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyMilitaryAcademy");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_GSTCODE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitaryAcademyGstCode");
                disabledClientControls.Add("chkboxGstCode");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitaryAcademyGstCode");
                hiddenClientControls.Add("chkboxGstCode");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_MLTSCOOLSUBJECT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitaryAcademySchoolSubject");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyMilitarySubject");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitaryAcademySchoolSubject");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyMilitarySubject");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_DURATIONYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationAcademyDurationYear");
                disabledClientControls.Add("txtMilitaryEducationAcademyDurationYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationAcademyDurationYear");
                hiddenClientControls.Add("txtMilitaryEducationAcademyDurationYear");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_GRADYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationAcademyGraduateYear");
                disabledClientControls.Add("txtMilitaryEducationAcademyGraduateYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationAcademyGraduateYear");
                hiddenClientControls.Add("txtMilitaryEducationAcademyGraduateYear");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_COUNTRY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationMilitaryArms");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyCountry");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationMilitaryArms");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyCountry");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_LRNMETHOD");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationAcademyLearningMethod");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyLearningMethod");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationAcademyLearningMethod");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyLearningMethod");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA_VITOSHAMILITARYREPORTSPECIALITY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("lblMilitaryEducationAcademyVitoshaMilitaryReportSpeciality");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("lblMilitaryEducationAcademyVitoshaMilitaryReportSpeciality");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality");
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

        //Load a particular MilitaryEducationAcademy (ajax call)
        private void JSLoadMilitaryEducationAcademy()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int militaryEducationAcademyId = int.Parse(Request.Params["MilitaryEducationAcademyId"]);

                PersonMilitaryEducationAcademy personMilitaryEducationAcademy = PersonMilitaryEducationAcademyUtil.GetPersonMilitaryEducation(militaryEducationAcademyId, CurrentUser);
                stat = AJAXTools.OK;

                string vitoshaMilitaryReportSpecialityOptions = "";
                StringBuilder sb = new StringBuilder();

                foreach (VitoshaMilitaryReportSpeciality vmrs in VitoshaMilitaryReportSpecialityUtil.GetVitoshaMilitaryReportSpecialitysByType((personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : ""), CurrentUser))
                {
                    sb.Append("<vitoshaMilRepSpecOp>");
                    sb.Append("<code>" + vmrs.VitoshaMilReportingSpecialityCode.ToString() + "</code>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(vmrs.CodeAndName) + "</name>");
                    sb.Append("</vitoshaMilRepSpecOp>");
                }

                vitoshaMilitaryReportSpecialityOptions = sb.ToString();

                response = @"<personMilitaryEducationAcademy>
                                <MilitaryAcademyCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducationAcademy.MilitaryAcademyCode) ? personMilitaryEducationAcademy.MilitaryAcademyCode : ListItems.GetOptionChooseOne().Value) + @"</MilitaryAcademyCode>
                                <GstCode>" + AJAXTools.EncodeForXML(personMilitaryEducationAcademy.GstCode.ToString()) + @"</GstCode>
                                <MilitarySubjectId>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducationAcademy.MilitaryAcademySubjectCode) ? personMilitaryEducationAcademy.MilitaryAcademySubjectCode : ListItems.GetOptionChooseOne().Value) + @"</MilitarySubjectId>
                                <DurationYear>" + AJAXTools.EncodeForXML(personMilitaryEducationAcademy.DurationYear.ToString()) + @"</DurationYear>
                                <GraduateYear>" + AJAXTools.EncodeForXML(personMilitaryEducationAcademy.GraduateYear.ToString()) + @"</GraduateYear>
                                <Country>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducationAcademy.CountryCode) ? personMilitaryEducationAcademy.CountryCode : ListItems.GetOptionChooseOne().Value) + @"</Country>
                                <LearningMethodKey>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryEducationAcademy.LearningMethod.LearningMethodKey) ? personMilitaryEducationAcademy.LearningMethod.LearningMethodKey : ListItems.GetOptionChooseOne().Value) + @"</LearningMethodKey>
                                <vitoshaMilitaryReportSpecialityTypeCode>" + (personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : "") + @"</vitoshaMilitaryReportSpecialityTypeCode> 
                                <vitoshaMilitaryReportSpecialityCode>" + (!string.IsNullOrEmpty(personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode) ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode : "") + @"</vitoshaMilitaryReportSpecialityCode>
                                <vitoshaMilitaryReportSpecialityOptions>" + vitoshaMilitaryReportSpecialityOptions + @"</vitoshaMilitaryReportSpecialityOptions>					   
                             </personMilitaryEducationAcademy>";
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

        //Save a particular MilitaryEducationAcademy (ajax call)
        private void JSSaveMilitaryEducationAcademy()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int militaryEducationAcademyId = int.Parse(Request.Form["MilitaryEducationAcademyId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string academyCode = Request.Form["MilitaryAcademyCode"];
                string subjectCode = Request.Form["MilitaryAcademySubjectCode"];
                string countryCode = Request.Form["CountryCode"];
                string learningMethodKey = Request.Form["LearningMethodKey"];

                int durationYear = int.Parse(Request.Form["DurationYear"]);
                int graduateYear = int.Parse(Request.Form["GraduateYear"]);
                string vitoshaMilitaryReportSpecialityCode = Request.Form["VitoshaMilitaryReportSpecialityCode"];


                bool GstCode = Request.Form["GstCode"] == "true" ? true : false;

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryEducationAcademy existingPersonMilitaryEducationAcademy = PersonMilitaryEducationAcademyUtil.GetPersonMilitaryEducation(reservist.Person.IdentNumber, academyCode, graduateYear, CurrentUser);

                if (existingPersonMilitaryEducationAcademy != null &&
                    existingPersonMilitaryEducationAcademy.MilitaryEducationAcademyId != militaryEducationAcademyId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраното военна академия вече е въведена за избраната година на завършване</status>";
                }
                else
                {
                    PersonMilitaryEducationAcademy personMilitaryEducationAcademy = new PersonMilitaryEducationAcademy(CurrentUser);

                    personMilitaryEducationAcademy.MilitaryEducationAcademyId = militaryEducationAcademyId;

                    personMilitaryEducationAcademy.MilitaryAcademyCode = academyCode;
                    personMilitaryEducationAcademy.MilitaryAcademySubjectCode = subjectCode;
                    personMilitaryEducationAcademy.CountryCode = countryCode;

                    personMilitaryEducationAcademy.DurationYear = durationYear;
                    personMilitaryEducationAcademy.GraduateYear = graduateYear;

                    personMilitaryEducationAcademy.GstCode = GstCode;

                    personMilitaryEducationAcademy.LearningMethod = LearningMethodUtil.GetLearningMethod(CurrentUser, learningMethodKey);

                    personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode = vitoshaMilitaryReportSpecialityCode;

                    //Save - Insert/Update Data
                    PersonMilitaryEducationAcademyUtil.SavePersonMilitaryEducationAcademy(personMilitaryEducationAcademy, reservist.Person, CurrentUser, change);


                    change.WriteLog();

                    string refreshedMilitaryEducationAcademyTable = GetMilitaryEducationAcademysTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedMilitaryEducationAcademyTable>" + AJAXTools.EncodeForXML(refreshedMilitaryEducationAcademyTable) + @"</refreshedMilitaryEducationAcademyTable>";
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

        //Delete a particular MilitaryEducationAcademy (ajax call)
        private void JSDeleteMilitaryEducationAcademy()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int militaryEducationAcademyId = int.Parse(Request.Params["MilitaryEducationAcademyId"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryEducationAcademyUtil.DeletePersonMilitaryEducationAcademy(militaryEducationAcademyId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedMilitaryEducationAcademyTable = GetMilitaryEducationAcademysTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMilitaryEducationAcademyTable>" + AJAXTools.EncodeForXML(refreshedMilitaryEducationAcademyTable) + @"</refreshedMilitaryEducationAcademyTable>";
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

        //Load TrainingCources table and light-box (ajax call)
        private void JSLoadMilitaryTrainingCources()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetMilitaryTrainingCourcesTable(reservistId);
                string lightBoxHTML = GetMilitaryTrainingCourceLightBox();
                string UIItems = GetMilitaryTrainingCourceUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
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

        //Render the TrainingCource table
        private string GetMilitaryTrainingCourcesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsMilitaryTrainingCourceCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_TRNCOURCE") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceDurationMonthHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DURMONTH") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceDurationDayHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DURDAY") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceLevelHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_LEVEL") == UIAccessLevel.Hidden;
            bool IsCountryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_COUNTRY") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VACANN") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VACANNDATE") == UIAccessLevel.Hidden;
            bool IsMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_COMAMANDERRANK") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceDateOfCourcenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DATECOURCE") == UIAccessLevel.Hidden;
            bool IsPersonLanguageHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_LANGUAGE") == UIAccessLevel.Hidden;
            bool IsMilitarySchoolHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_MLTSCOOL") == UIAccessLevel.Hidden;
            bool IsMilitaryTrainingCourceNameDescriptionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DESRIPTION") == UIAccessLevel.Hidden;
            bool IsVitoshaMilitaryReportSpecialityHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VITOSHAMILITARYREPORTSPECIALITY") == UIAccessLevel.Hidden;

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            if (IsMilitaryTrainingCourceCodeHidden &&
               IsMilitaryTrainingCourceDurationMonthHidden &&
               IsMilitaryTrainingCourceDurationDayHidden &&
               IsMilitaryTrainingCourceLevelHidden &&
               IsCountryHidden &&
               IsMilitaryTrainingCourceVacAnnHidden &&
               IsMilitaryTrainingCourceDateWhenHidden &&
               IsMilitaryCommanderRankHidden &&
               IsMilitaryTrainingCourceDateOfCourcenHidden &&
               IsPersonLanguageHidden &&
               IsMilitarySchoolHidden &&
               IsMilitaryTrainingCourceNameDescriptionHidden &&
               IsVitoshaMilitaryReportSpecialityHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS") == UIAccessLevel.Enabled && !isPreview
                                        );


            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewTrainigCource();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
 " + (!IsMilitaryTrainingCourceCodeHidden ? @"<th style='width: 290px; vertical-align: bottom;'>Курс</th>" : "") + @"
 " + (!IsMilitaryTrainingCourceDurationMonthHidden ? @"<th style='width: 50px; vertical-align: bottom;'><span title='Продължителност (Месеци)'>Прод.(Месеци)</span></th>" : "") + @"                    
 " + (!IsMilitaryTrainingCourceDurationDayHidden ? @"<th style='width: 290px; vertical-align: bottom;'><span title='Продължителност (Дни)'>Прод.(Дни)</span></th>" : "") + @"                    
 " + (!IsMilitaryTrainingCourceLevelHidden ? @"<th style='width: 50px; vertical-align: bottom;'>Ниво</th>" : "") + @"
 " + (!IsCountryHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Държава</th>" : "") + @"
 " + (!IsMilitaryTrainingCourceVacAnnHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Заповед</th>" : "") + @"
 " + (!IsMilitaryTrainingCourceDateWhenHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Дата на заповедта</th>" : "") + @"
 " + (!IsMilitaryCommanderRankHidden ? @"<th style='width: 290px; vertical-align: bottom;'>Подписал заповедта</th>" : "") + @"                    
 " + (!IsMilitaryTrainingCourceDateOfCourcenHidden ? @"<th style='width: 50px; vertical-align: bottom;'>Влиза в сила от</th>" : "") + @"
 " + (!IsPersonLanguageHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Чужд език</th>" : "") + @"
 " + (!IsMilitarySchoolHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Място на провеждане</th>" : "") + @"
 " + (!IsMilitaryTrainingCourceNameDescriptionHidden ? @"<th style='width: 110px; vertical-align: bottom;'>Наименование</th>" : "") + @"
 " + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<th style='width: 120px; vertical-align: bottom;'>ВОС</th>" : "") + @"

<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonMilitaryTrainingCource> listPersonMilitaryTrainingCources = PersonMilitaryTrainingCourceUtil.GetAllPersonMilitaryTrainingCourceByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonMilitaryTrainingCource personMilitaryTrainingCource in listPersonMilitaryTrainingCources)
            {
                counter++;

                string deleteHTML = "";

                if (personMilitaryTrainingCource.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteTrainigCource(" + personMilitaryTrainingCource.MilitaryTrainingCourceId.ToString() + ");' />";
                }

                string editHTML = "";

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditTrainigCource(" + personMilitaryTrainingCource.MilitaryTrainingCourceId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
         <td style='text-align: center;'>" + counter.ToString() + @"</td>
     
" + (!IsMilitaryTrainingCourceCodeHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceCode) ? personMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName : "") + @"</td>" : "") + @"
     
" + (!IsMilitaryTrainingCourceDurationMonthHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "") + @"</td>" : "") + @"
     
" + (!IsMilitaryTrainingCourceDurationDayHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "") + @"</td>" : "") + @"
   
" + (!IsMilitaryTrainingCourceLevelHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "") + @"</td>" : "") + @"
        
" + (!IsCountryHidden ? @"<td style='text-align: left;'>" + personMilitaryTrainingCource.Country.CountryName + @"</td>" : "") + @"
      
" + (!IsMilitaryTrainingCourceVacAnnHidden ? @"<td style='text-align: left;'>" + (!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn) ? personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn : "") + @"</td>" : "") + @"
           
" + (!IsMilitaryTrainingCourceDateWhenHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen) + @"</td>" : "") + @"
   
" + (!IsMilitaryCommanderRankHidden ? @"<td style='text-align: left;'>" + (!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryCommanderRankCode) ? personMilitaryTrainingCource.MilitaryCommanderRank.MilitaryCommanderRankName : "") + @"</td>" : "") + @"
  
" + (!IsMilitaryTrainingCourceDateOfCourcenHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != null ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource) : "") + @"</td>" : "") + @"
   
" + (!IsPersonLanguageHidden ? @"<td style='text-align: left;'>" + (!String.IsNullOrEmpty(personMilitaryTrainingCource.PersonLanguageCode) ? personMilitaryTrainingCource.PersonLanguage.PersonLanguageName : "") + @"</td>" : "") + @"
 
" + (!IsMilitarySchoolHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.MilitarySchool != null ? personMilitaryTrainingCource.MilitarySchool.MilitarySchoolName : "") + @"</td>" : "") + @"
     
" + (!IsMilitaryTrainingCourceNameDescriptionHidden ? @"<td style='text-align: left;'>" + (!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription) ? personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription : "") + @"</td>" : "") + @"

" + (!IsVitoshaMilitaryReportSpecialityHidden ? @"<td style='text-align: left;'>" + (personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.CodeAndName : "") + @"</td>" : "") + @"
                                
<td align='center' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;

        }

        //Render the TrainingCource light-box
        private string GetMilitaryTrainingCourceLightBox()
        {
            //1. Bind Dropdown List for MilitaryTrainingCources
            List<MilitaryTrainingCource> listMilitaryTrainingCources = MilitaryTrainingCourceUtil.GetAllMilitaryTrainingCources(CurrentUser);
            List<IDropDownItem> ddMilitaryAcademy = new List<IDropDownItem>();

            foreach (MilitaryTrainingCource militaryTrainingCource in listMilitaryTrainingCources)
            {
                ddMilitaryAcademy.Add(militaryTrainingCource);
            }
            // Generates html for drop down list
            string personMilitaryTrainingCourceHTML = ListItems.GetDropDownHtml(ddMilitaryAcademy, null, "ddPersonMilitaryTrainingCourceMilitaryTrainingCources", true, null, "ddTrainingCourceChange(this)", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");


            //2. Bind Dropdown List form Countries
            List<Country> listCountry = CountryUtil.GetCountries(CurrentUser);
            List<IDropDownItem> ddCountry = new List<IDropDownItem>();

            foreach (Country country in listCountry)
            {
                ddCountry.Add(country);
            }
            // Generates html for drop down list
            string personMilitaryTrainingCountryHTML = ListItems.GetDropDownHtml(ddCountry, null, "ddPersonMilitaryTrainingCourceCountries", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            //3. Bind Dropdown List for MilitaryCommanderRanks
            List<MilitaryCommanderRank> listMilitaryCommanderRanks = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRanks)
            {
                ddMilitaryCommanderRank.Add(militaryCommanderRank);
            }
            // Generates html for drop down list
            string personMilitaryCommanderRankHTML = ListItems.GetDropDownHtml(ddMilitaryCommanderRank, null, "ddPersonMilitaryTrainingCourceMilitaryCommanderRanks", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true'", true);


            //4. Bind Dropdown List for PersonLanguages
            List<PersonLanguage> listPersonLanguage = PersonLanguageUtil.GetAllPersonLanguages(CurrentUser);
            List<IDropDownItem> ddPersonLanguage = new List<IDropDownItem>();

            foreach (PersonLanguage personLanguage in listPersonLanguage)
            {
                ddPersonLanguage.Add(personLanguage);
            }

            // Generates html for drop down list
            string personMilitaryLanguageHTML = ListItems.GetDropDownHtml(ddPersonLanguage, null, "ddPersonMilitaryTrainingCourceMilitaryLanguages", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true'");

            //5. Bind Dropdown List for MilitarySchools
            List<MilitarySchool> listMilitarySchool = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, false);
            List<IDropDownItem> ddMilitarySchool = new List<IDropDownItem>();

            foreach (MilitarySchool militarySchool in listMilitarySchool)
            {
                ddMilitarySchool.Add(militarySchool);
            }

            // Generates html for drop down list
            string personMilitarySchoolHTML = ListItems.GetDropDownHtml(ddMilitarySchool, null, "ddPersonMilitaryTrainingCourceMilitarySchools", true, null, "", "style='width: 230px;' UnsavedCheckSkipMe='true'");
                       
            List<VitoshaMilitaryReportSpecialityType> listVitoshaMilitaryReportSpecialityType = VitoshaMilitaryReportSpecialityTypeUtil.GetAllVitoshaMilitaryReportSpecialityTypes(CurrentUser);
            List<IDropDownItem> ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType = new List<IDropDownItem>();
            foreach (VitoshaMilitaryReportSpecialityType vitoshaMilitaryReportSpecialityType in listVitoshaMilitaryReportSpecialityType)
               ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType.Add(vitoshaMilitaryReportSpecialityType);

            string vitoshaMilitaryReportSpecialityTypeHTML = ListItems.GetDropDownHtml(ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType, null, "ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType", true, null, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='InputField' onchange='VitoshaMilRepSpecTypeLightBoxChanged(\"ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType\", \"ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality\");'");

            string html = @"
<center>
    <input type=""hidden"" id=""hdnTrainingCourceID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 45%"">
        </colgroup>
        <colgroup style=""width: 55%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilitaryTrainingCourceTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceCode"" class=""InputLabel"">Курс:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryTrainingCourceHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceDurationMonth"" class=""InputLabel"">Продължителност (месеци):</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryTrainingCourceDurationMonth"" UnsavedCheckSkipMe='true' maxlength='2' class=""InputField"" style=""width: 50px;"" />
                " + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceDurationDay"" class=""InputLabel"">Продължителност (дни):</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryTrainingCourceDurationDay"" UnsavedCheckSkipMe='true' maxlength='2'  style=""width: 50px;"" />
                " + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceLevel"" class=""InputLabel"">Ниво:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryTrainingCourceLevel"" UnsavedCheckSkipMe='true' maxlength='1' class=""InputField"" style=""width: 50px;"" />
                " + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCountry"" class=""InputLabel"">Държава:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryTrainingCountryHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceVacAnn"" class=""InputLabel"" >Заповед:</span>
            </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtMilitaryTrainingCourceVacAnn"" UnsavedCheckSkipMe='true' maxlength='7' class=""InputField"" style=""width: 75px;"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceDateWhen"" class=""InputLabel"">Дата на заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""txtMilitaryTrainingCourceDateWhenCont""><input type=""text"" id=""txtMilitaryTrainingCourceDateWhen"" UnsavedCheckSkipMe='true' class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /></span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryCommanderRank""  class=""InputLabel"">Подписал заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryCommanderRankHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceDateOfCource"" class=""InputLabel"">Влиза в сила от:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""txtMilitaryTrainingCourceDateOfCourceCont""><input type=""text"" id=""txtMilitaryTrainingCourceDateOfCource"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true""/></span>
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceLearningMethod"" class=""InputLabel"">Чужд език:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitaryLanguageHTML + @"
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceLearningMethod"" class=""InputLabel"">Място на провеждане:</span>
            </td>
            <td style=""text-align: left;"">
                " + personMilitarySchoolHTML + @"
            </td>
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; vertical-align: top;"">
                <span id=""lblMilitaryTrainingCourceNameDescription"" class=""InputLabel"">Наименование:</span>
            </td>
            <td style=""text-align: left;"">
<textarea id='txtMilitaryTrainingCourceNameDescription' cols='3' rows='5' UnsavedCheckSkipMe='true' class='InputField' style='width: 225px'></textarea>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType"" class=""InputLabel"">Тип ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + vitoshaMilitaryReportSpecialityTypeHTML + @"
            </td>
        </tr>   
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryTrainingCourceVitoshaMilitaryReportSpeciality"" class=""InputLabel"">ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + ListItems.GetDropDownHtml(new List<IDropDownItem>(), null, "ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality", true, null, "", "style='width: 320px;' UnsavedCheckSkipMe='true' class='InputField' ") + @"
            </td>
        </tr> 
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditTrainigCourceLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilitaryTrainingCourceLightBox"" style=""display: inline;"" onclick=""SaveAddEditMilitaryTrainingCourceLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilitaryTrainingCourceLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMilitaryAcademyEducationLightBox"" style=""display: inline;"" onclick=""HideAddEditMilitaryTrainingCourceLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddMilitaryAcademyEducationLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the MilitaryTrainingCource table
        public string GetMilitaryTrainingCourceUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_TRNCOURCE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceCode");
                disabledClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryTrainingCources");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceCode");
                hiddenClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryTrainingCources");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DURMONTH");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceDurationMonth");
                disabledClientControls.Add("txtMilitaryTrainingCourceDurationMonth");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceDurationMonth");
                hiddenClientControls.Add("txtMilitaryTrainingCourceDurationMonth");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DURDAY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceDurationDay");
                disabledClientControls.Add("txtMilitaryTrainingCourceDurationDay");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceDurationDay");
                hiddenClientControls.Add("txtMilitaryTrainingCourceDurationDay");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_LEVEL");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceLevel");
                disabledClientControls.Add("txtMilitaryTrainingCourceLevel");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceLevel");
                hiddenClientControls.Add("txtMilitaryTrainingCourceLevel");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_COUNTRY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCountry");
                disabledClientControls.Add("ddPersonMilitaryTrainingCourceCountries");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCountry");
                hiddenClientControls.Add("ddPersonMilitaryTrainingCourceCountries");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VACANN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceVacAnn");
                disabledClientControls.Add("txtMilitaryTrainingCourceVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceVacAnn");
                hiddenClientControls.Add("txtMilitaryTrainingCourceVacAnn");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VACANNDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceDateWhen");
                disabledClientControls.Add("txtMilitaryTrainingCourceDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceDateWhen");
                hiddenClientControls.Add("txtMilitaryTrainingCourceDateWhenCont");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_COMAMANDERRANK");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryCommanderRank");
                disabledClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryCommanderRank");
                hiddenClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DATECOURCE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceDateOfCource");
                disabledClientControls.Add("txtMilitaryTrainingCourceDateOfCource");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceDateOfCource");
                hiddenClientControls.Add("txtMilitaryTrainingCourceDateOfCourceCont");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_LANGUAGE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceLearningMethod");
                disabledClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryLanguages");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceLearningMethod");
                hiddenClientControls.Add("ddPersonMilitaryTrainingCourceMilitaryLanguages");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_MLTSCOOL");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceLearningMethod");
                disabledClientControls.Add("ddPersonMilitaryTrainingCourceMilitarySchools");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceLearningMethod");
                hiddenClientControls.Add("ddPersonMilitaryTrainingCourceMilitarySchools");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_DESRIPTION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceNameDescription");
                disabledClientControls.Add("txtMilitaryTrainingCourceNameDescription");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceNameDescription");
                hiddenClientControls.Add("txtMilitaryTrainingCourceNameDescription");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS_VITOSHAMILITARYREPORTSPECIALITY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("lblMilitaryTrainingCourceVitoshaMilitaryReportSpeciality");
                disabledClientControls.Add("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType");
                disabledClientControls.Add("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("lblMilitaryTrainingCourceVitoshaMilitaryReportSpeciality");
                hiddenClientControls.Add("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType");
                hiddenClientControls.Add("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality");
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

        //Load a particular TrainingCource (ajax call)
        private void JSLoadMilitaryTrainingCource()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int trainigCourceId = int.Parse(Request.Params["TrainigCourceId"]);

                PersonMilitaryTrainingCource personMilitaryTrainingCource = PersonMilitaryTrainingCourceUtil.GetPersonMilitaryTrainingCource(trainigCourceId, CurrentUser);
                stat = AJAXTools.OK;
                            
                string vitoshaMilitaryReportSpecialityOptions = "";
                StringBuilder sb = new StringBuilder();

                foreach (VitoshaMilitaryReportSpeciality vmrs in VitoshaMilitaryReportSpecialityUtil.GetVitoshaMilitaryReportSpecialitysByType((personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : ""), CurrentUser))
                {
                    sb.Append("<vitoshaMilRepSpecOp>");
                    sb.Append("<code>" + vmrs.VitoshaMilReportingSpecialityCode.ToString() + "</code>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(vmrs.CodeAndName) + "</name>");
                    sb.Append("</vitoshaMilRepSpecOp>");
                }

                vitoshaMilitaryReportSpecialityOptions = sb.ToString();

                response = @"<PersonMilitaryTrainingCource>
                                <MilitaryTrainingCourceCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceCode) ? personMilitaryTrainingCource.MilitaryTrainingCourceCode : ListItems.GetOptionChooseOne().Value) + @"</MilitaryTrainingCourceCode>
                                <MilitaryTrainingCourceDurationMonth>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "") + @"</MilitaryTrainingCourceDurationMonth>
                                <MilitaryTrainingCourceDurationDay>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "") + @"</MilitaryTrainingCourceDurationDay>
                                <MilitaryTrainingCourceLevel>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "") + @"</MilitaryTrainingCourceLevel>
                                <MilitaryTrainingCourceCountryCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.CountryCode) ? personMilitaryTrainingCource.CountryCode : ListItems.GetOptionChooseOne().Value) + @"</MilitaryTrainingCourceCountryCode>
                                <MilitaryTrainingCourceVacAnn>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn) ? personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn : "") + @"</MilitaryTrainingCourceVacAnn>
                                <MilitaryTrainingCourceDateWhen>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.HasValue ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen) : "") + @"</MilitaryTrainingCourceDateWhen>
                                <MilitaryTrainingCommanderRankCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryCommanderRankCode) ? personMilitaryTrainingCource.MilitaryCommanderRankCode : "") + @"</MilitaryTrainingCommanderRankCode>
                                <MilitaryTrainingCourceDateOfCource>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.HasValue ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource) : "") + @"</MilitaryTrainingCourceDateOfCource>
                                <MilitaryTrainingPersonLanguageCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.PersonLanguageCode) ? personMilitaryTrainingCource.PersonLanguageCode : ListItems.GetOptionChooseOne().Value) + @"</MilitaryTrainingPersonLanguageCode>
                                <MilitaryTrainingMilitarySchooCode>" + AJAXTools.EncodeForXML(personMilitaryTrainingCource.MilitarySchoolId > 0 ? personMilitaryTrainingCource.MilitarySchoolId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</MilitaryTrainingMilitarySchooCode>
                                <MilitaryTrainingCourceNameDescription>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription) ? personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription : "") + @"</MilitaryTrainingCourceNameDescription>
                                <vitoshaMilitaryReportSpecialityTypeCode>" + (personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID : "") + @"</vitoshaMilitaryReportSpecialityTypeCode> 
                                <vitoshaMilitaryReportSpecialityCode>" + (!string.IsNullOrEmpty(personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode) ? personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode : "") + @"</vitoshaMilitaryReportSpecialityCode>
                                <vitoshaMilitaryReportSpecialityOptions>" + vitoshaMilitaryReportSpecialityOptions + @"</vitoshaMilitaryReportSpecialityOptions>
                             </PersonMilitaryTrainingCource>";
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

        //Save a particular TrainingCource (ajax call)
        private void JSSaveMilitaryTrainingCource()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int militaryTrainigCourceId = int.Parse(Request.Form["TrainigCourceId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string militaryCourceCode = Request.Form["MilitaryCourceCode"];
                string militaryCountryCode = Request.Form["MilitaryCountryCode"];
                string militaryCommanderRanksCode = Request.Form["MilitaryCommanderRanksCode"];
                string militaryLanguageCode = Request.Form["MilitaryLanguageCode"];
                int militarySchoolId = int.Parse(Request.Form["MilitarySchoolId"]);

                string courceVacAnn = Request.Form["CourceVacAnn"];

                string CourceNameDescription = Request.Form["CourceNameDescription"];

                DateTime courceDateWhen = CommonFunctions.ParseDate(Request.Form["CourceDateWhen"]).Value;

                string vitoshaMilitaryReportSpecialityCode = Request.Form["VitoshaMilitaryReportSpecialityCode"];


                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryTrainingCource existingPersonMilitaryTrainingCource = PersonMilitaryTrainingCourceUtil.GetPersonMilitaryTrainingCource(reservist.Person.IdentNumber, militaryCourceCode, courceDateWhen, CurrentUser);

                if (existingPersonMilitaryTrainingCource != null &&
                    existingPersonMilitaryTrainingCource.MilitaryTrainingCourceId != militaryTrainigCourceId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избрания курс вече е въведен за избраната дата</status>";
                }
                else
                {
                    PersonMilitaryTrainingCource personMilitaryTrainingCource = new PersonMilitaryTrainingCource(CurrentUser);

                    personMilitaryTrainingCource.MilitaryTrainingCourceId = militaryTrainigCourceId;
                    personMilitaryTrainingCource.MilitaryTrainingCourceCode = militaryCourceCode;

                    if (militaryCountryCode != "-1")
                    {
                        personMilitaryTrainingCource.CountryCode = militaryCountryCode;
                    }

                    if (militaryCommanderRanksCode != "-1")
                    {
                        personMilitaryTrainingCource.MilitaryCommanderRankCode = militaryCommanderRanksCode;
                    }
                    else
                    {
                        personMilitaryTrainingCource.MilitaryCommanderRankCode = "";
                    }

                    if (militaryLanguageCode != "-1")
                    {
                        personMilitaryTrainingCource.PersonLanguageCode = militaryLanguageCode;
                    }
                    else
                    {
                        personMilitaryTrainingCource.PersonLanguageCode = "";
                    }

                    personMilitaryTrainingCource.MilitarySchoolId = militarySchoolId;

                    if (Request.Form["CourceDurationMonth"] != "")
                    {
                        personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth = int.Parse(Request.Form["CourceDurationMonth"]);

                    }
                    if (Request.Form["CourceDurationDay"] != "")
                    {
                        personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay = int.Parse(Request.Form["CourceDurationDay"]);

                    }
                    if (Request.Form["CourceLevel"] != "")
                    {
                        personMilitaryTrainingCource.MilitaryTrainingCourceLevel = int.Parse(Request.Form["CourceLevel"]);

                    }
                    personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn = courceVacAnn;

                    personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription = CourceNameDescription;

                    personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen = courceDateWhen;

                    if (!string.IsNullOrEmpty(Request.Form["CourceDateOfCource"]))
                    {
                        personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource = CommonFunctions.ParseDate(Request.Form["CourceDateOfCource"]);
                    }

                    personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode = vitoshaMilitaryReportSpecialityCode;

                    PersonMilitaryTrainingCourceUtil.SavePersonMilitaryEducationAcademy(personMilitaryTrainingCource, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedMilitaryTrainingCource = GetMilitaryTrainingCourcesTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedMilitaryTrainingCource>" + AJAXTools.EncodeForXML(refreshedMilitaryTrainingCource) + @"</refreshedMilitaryTrainingCource>";
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

        //Delete a particular TrainingCource (ajax call)
        private void JSDeleteMilitaryTrainingCource()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int militaryTrainigCourceId = int.Parse(Request.Form["TrainigCourceId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryTrainingCourceUtil.DeletePersonMilitaryTrainingCource(militaryTrainigCourceId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedMilitaryTrainingCourceTable = GetMilitaryTrainingCourcesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMilitaryTrainingCourceTable>" + AJAXTools.EncodeForXML(refreshedMilitaryTrainingCourceTable) + @"</refreshedMilitaryTrainingCourceTable>";
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

        //Load ForeignLanguage table and light-box (ajax call)
        private void JSLoadForeignLanguages()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetForeignLanguagesTable(reservistId);
                string lightBoxHTML = GetForeignLanguageLightBox();
                string UIItems = GetForeignLanguageUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
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

        //Save a particular ForeignLanguage (ajax call)
        private void JSSaveForeignLanguage()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int PersonLangEduForeignLanguageId = int.Parse(Request.Form["ForeignLanguageId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string LanguageCode = Request.Form["LanguageCode"];
                string LanguageLevelOfKnowledgeKey = Request.Form["LanguageLevelOfKnowledgeKey"];
                string LanguageFormOfKnowledgeKey = Request.Form["LanguageFormOfKnowledgeKey"];
                string LanguageStanAg = Request.Form["LanguageStanAg"];
                string LanguageDiplomaKey = Request.Form["LanguageDiplomaKey"];
                string LanguageVacAnn = Request.Form["LanguageVacAnn"];


                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonLangEduForeignLanguage existingPersonLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetPersonLangEduForeignLanguage(reservist.Person.IdentNumber, LanguageCode, CurrentUser);

                if (existingPersonLangEduForeignLanguage != null &&
                    existingPersonLangEduForeignLanguage.PersonLangEduForeignLanguageId != PersonLangEduForeignLanguageId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраният език вече е въведен</status>";
                }
                else
                {
                    PersonLangEduForeignLanguage personLangEduForeignLanguage = new PersonLangEduForeignLanguage(CurrentUser);

                    personLangEduForeignLanguage.PersonLangEduForeignLanguageId = PersonLangEduForeignLanguageId;
                    personLangEduForeignLanguage.LanguageCode = LanguageCode;
                    personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey = LanguageLevelOfKnowledgeKey;
                    personLangEduForeignLanguage.LanguageFormOfKnowledgeKey = LanguageFormOfKnowledgeKey;
                    personLangEduForeignLanguage.LanguageStanAg = LanguageStanAg;

                    personLangEduForeignLanguage.LanguageDiplomaKey = LanguageDiplomaKey;
                    personLangEduForeignLanguage.LanguageVacAnn = LanguageVacAnn;

                    if (!string.IsNullOrEmpty(Request.Form["LanguageDateWhen"]))
                    {
                        personLangEduForeignLanguage.LanguageDateWhen = CommonFunctions.ParseDate(Request.Form["LanguageDateWhen"]);

                    }

                    PersonLangEduForeignLanguageUtil.SavePersonMilitaryEducationAcademy(personLangEduForeignLanguage, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedForeignLanguageTable = GetForeignLanguagesTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedForeignLanguageTable>" + AJAXTools.EncodeForXML(refreshedForeignLanguageTable) + @"</refreshedForeignLanguageTable>";
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

        //Load a particular ForeignLanguage (ajax call)
        private void JSLoadForeignLanguage()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG") == UIAccessLevel.Hidden
                                      )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personLangEduForeignLanguageId = int.Parse(Request.Params["ForeignLanguageId"]);

                PersonLangEduForeignLanguage personLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetPersonLangEduForeignLanguage(personLangEduForeignLanguageId, CurrentUser);


                stat = AJAXTools.OK;

                response = @"<PersonForeignLanguage>
                                <LanguageCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageCode) ? personLangEduForeignLanguage.LanguageCode : ListItems.GetOptionChooseOne().Value) + @"</LanguageCode>
                                <LanguageLevelOfKnowledgeKey>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey) ? personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey : ListItems.GetOptionChooseOne().Value) + @"</LanguageLevelOfKnowledgeKey>
                                <LanguageFormOfKnowledgeKey>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageFormOfKnowledgeKey) ? personLangEduForeignLanguage.LanguageFormOfKnowledgeKey : ListItems.GetOptionChooseOne().Value) + @"</LanguageFormOfKnowledgeKey>
                                <LanguageStanAg>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageStanAg) ? personLangEduForeignLanguage.LanguageStanAg : "") + @"</LanguageStanAg>
                                <LanguageDiplomaKey>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageDiplomaKey) ? personLangEduForeignLanguage.LanguageDiplomaKey : ListItems.GetOptionChooseOne().Value) + @"</LanguageDiplomaKey>
                                <LanguageVacAnn>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personLangEduForeignLanguage.LanguageVacAnn) ? personLangEduForeignLanguage.LanguageVacAnn : "") + @"</LanguageVacAnn>
                                <LanguageDateWhen>" + AJAXTools.EncodeForXML(personLangEduForeignLanguage.LanguageDateWhen.HasValue ? CommonFunctions.FormatDate(personLangEduForeignLanguage.LanguageDateWhen) : "") + @"</LanguageDateWhen>
                             </PersonForeignLanguage>";

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

        //Delete a particular ForeignLanguage (ajax call)
        private void JSDeleteForeignLanguage()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personLangEduForeignLanguageId = int.Parse(Request.Form["ForeignLanguageId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonLangEduForeignLanguageUtil.DeletePersonMilitaryTrainingCource(personLangEduForeignLanguageId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedForeignLanguageTable = GetForeignLanguagesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedForeignLanguageTable>" + AJAXTools.EncodeForXML(refreshedForeignLanguageTable) + @"</refreshedForeignLanguageTable>";
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

        //Render the ForeignLanguages table
        private string GetForeignLanguagesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsLanguageCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LNG") == UIAccessLevel.Hidden;
            bool IsLanguageLevelOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LVLKNG") == UIAccessLevel.Hidden;
            bool IsLanguageFormOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LVLFORM") == UIAccessLevel.Hidden;
            bool IsLanguageStanAgHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_STANAG") == UIAccessLevel.Hidden;
            bool IsLanguageDiplomaKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DPLM") == UIAccessLevel.Hidden;
            bool IsLanguageVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DOCNUM") == UIAccessLevel.Hidden;
            bool IsLanguageDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DOCDATE") == UIAccessLevel.Hidden;

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (IsLanguageCodeHidden &&
                IsLanguageLevelOfKnowledgeKeyHidden &&
                IsLanguageFormOfKnowledgeKeyHidden &&
                IsLanguageStanAgHidden &&
                IsLanguageDiplomaKeyHidden &&
                IsLanguageVacAnnHidden && IsLanguageDateWhenHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG") == UIAccessLevel.Enabled && !isPreview
                                        );

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewForeignLanguage();' />";


            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsLanguageCodeHidden ? @"<th style='width: 100px; vertical-align: bottom;'>Език</th>" : "") + @"
  " + (!IsLanguageLevelOfKnowledgeKeyHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Степен на владеене</th>" : "") + @"                    
  " + (!IsLanguageFormOfKnowledgeKeyHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Форма на владеене</th>" : "") + @"
  " + (!IsLanguageStanAgHidden ? @"<th style='width: 70px; vertical-align: bottom;'>STANAG</br>СГЧП</th>" : "") + @"
  " + (!IsLanguageDiplomaKeyHidden ? @"<th style='width: 130px; vertical-align: bottom;'>Диплом</th>" : "") + @"
  " + (!IsLanguageVacAnnHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Номер на документа</th>" : "") + @"                    
  " + (!IsLanguageDateWhenHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на документа</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonLangEduForeignLanguage> listPersonLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetAllPersonLangEduForeignLanguageByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonLangEduForeignLanguage personLangEduForeignLanguage in listPersonLangEduForeignLanguage)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";
                if (string.IsNullOrEmpty(personLangEduForeignLanguage.LanguageStanAg))
                {
                    if (personLangEduForeignLanguage.CanDelete)
                    {
                        if (notHideDelEditHTML)
                        {
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteForeignLanguage(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
                        }
                    }

                    if (notHideDelEditHTML)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditForeignLanguage(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
                    }
                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>

        " + (!IsLanguageCodeHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.PersonLanguage != null ? personLangEduForeignLanguage.PersonLanguage.PersonLanguageName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsLanguageLevelOfKnowledgeKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageLevelOfKnowledge != null ? personLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageFormOfKnowledgeKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageFormOfKnowledge != null ? personLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageStanAgHidden ? @"<td style='text-align: left;'>" + personLangEduForeignLanguage.LanguageStanAg.ToString() + @"</td>" : "") + @"
        " + (!IsLanguageDiplomaKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageDiplomaKey != null ? personLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageVacAnnHidden ? @"<td style='text-align: left;'>" + personLangEduForeignLanguage.LanguageVacAnn.ToString() + @"</td>" : "") + @"
        " + (!IsLanguageDateWhenHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageDateWhen != null ? CommonFunctions.FormatDate(personLangEduForeignLanguage.LanguageDateWhen) : "") + @"</td>" : "") + @"

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the ForeignLanguages light-box
        private string GetForeignLanguageLightBox()
        {
            // Generates html for drop down list
            List<PersonLanguage> listPersonLanguages = PersonLanguageUtil.GetAllPersonLanguages(CurrentUser);
            List<IDropDownItem> ddiPersonLanguage = new List<IDropDownItem>();

            foreach (PersonLanguage personLanguage in listPersonLanguages)
            {
                ddiPersonLanguage.Add(personLanguage);
            }

            // Generates html for drop down list
            string PersonLanguagesHTML = ListItems.GetDropDownHtml(ddiPersonLanguage, null, "ddPersonLangEduForeignLanguagePersonLanguage", true, null, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LanguageLevelOfKnowledge> listLanguageLevelOfKnowledges = LanguageLevelOfKnowledgeUtil.GetAllLanguageLevelOfKnowledges(CurrentUser);
            List<IDropDownItem> ddiLanguageLevelOfKnowledge = new List<IDropDownItem>();

            foreach (LanguageLevelOfKnowledge languageLevelOfKnowledge in listLanguageLevelOfKnowledges)
            {
                ddiLanguageLevelOfKnowledge.Add(languageLevelOfKnowledge);
            }

            // Generates html for drop down list
            string LanguageLevelOfKnowledgeHTML = ListItems.GetDropDownHtml(ddiLanguageLevelOfKnowledge, null, "ddPersonLangEduForeignLanguageLanguageLevel", true, null, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LanguageFormOfKnowledge> listLanguageFormOfKnowledges = LanguageFormOfKnowledgeUtil.GetAllLanguageFormOfKnowledges(CurrentUser);
            List<IDropDownItem> ddiLanguageFormOfKnowledge = new List<IDropDownItem>();

            foreach (LanguageFormOfKnowledge LanguageFormOfKnowledge in listLanguageFormOfKnowledges)
            {
                ddiLanguageFormOfKnowledge.Add(LanguageFormOfKnowledge);
            }

            // Generates html for drop down list
            string LanguageFormOfKnowledgeHTML = ListItems.GetDropDownHtml(ddiLanguageFormOfKnowledge, null, "ddPersonLangEduForeignLanguageLanguageForm", true, null, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");


            // Generates html for drop down list
            List<LanguageDiploma> listLanguageDiplomas = LanguageDiplomaUtil.GetAllLanguageDiplomas(CurrentUser);
            List<IDropDownItem> ddiLanguageDiploma = new List<IDropDownItem>();

            foreach (LanguageDiploma LanguageDiploma in listLanguageDiplomas)
            {
                ddiLanguageDiploma.Add(LanguageDiploma);
            }

            // Generates html for drop down list
            string LanguageDiplomaHTML = ListItems.GetDropDownHtml(ddiLanguageDiploma, null, "ddPersonLangEduForeignLanguageLanguageDiploma", true, null, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField'");

            string html = @"
<center>
    <input type=""hidden"" id=""hdnForeignLanguageID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditForeignLanguageTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguagePersonLanguage"" class=""InputLabel"">Език:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonLanguagesHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguageLanguageLevel"" class=""InputLabel"">Степен на владеене:</span>
            </td>
            <td style=""text-align: left;"">
                " + LanguageLevelOfKnowledgeHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguageLanguageForm"" class=""InputLabel"">Форма на владеене:</span>
            </td>
            <td style=""text-align: left;"">
                " + LanguageFormOfKnowledgeHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguageLanguageDiploma"" class=""InputLabel"">Диплом:</span>
            </td>
            <td style=""text-align: left;"">
                " + LanguageDiplomaHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguageLanguageVacAnn"" class=""InputLabel"">Номер на документа:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonLangEduForeignLanguageLanguageVacAnn"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonLangEduForeignLanguageLanguageDateWhen"" class=""InputLabel"">Дата на документа:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""txtPersonLangEduForeignLanguageLanguageDateWhenCont""><input type=""text"" id=""txtPersonLangEduForeignLanguageLanguageDateWhen"" UnsavedCheckSkipMe='true' class='" + CommonFunctions.DatePickerCSS() + @"' maxlength=""10"" style=""width: 70px;"" inLightBox=""true"" /></span>
            </td>
        </tr>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditForeignLanguageLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditForeignLanguageLightBox"" style=""display: inline;"" onclick=""SaveAddEditForeignLanguageLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditForeignLanguageLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditForeignLanguageLightBox"" style=""display: inline;"" onclick=""HideAddEditForeignLanguageLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditForeignLanguageLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the ForeignLanguage table
        public string GetForeignLanguageUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LNG");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguagePersonLanguage");
                disabledClientControls.Add("ddPersonLangEduForeignLanguagePersonLanguage");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguagePersonLanguage");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguagePersonLanguage");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LVLKNG");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageLevel");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageLevel");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageLevel");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageLevel");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_LVLFORM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageForm");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageForm");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageForm");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageForm");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DPLM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageDiploma");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageDiploma");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageDiploma");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageDiploma");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DOCNUM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageVacAnn");
                disabledClientControls.Add("txtPersonLangEduForeignLanguageLanguageVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageVacAnn");
                hiddenClientControls.Add("txtPersonLangEduForeignLanguageLanguageVacAnn");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG_DOCDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageDateWhen");
                disabledClientControls.Add("txtPersonLangEduForeignLanguageLanguageDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageDateWhen");
                hiddenClientControls.Add("txtPersonLangEduForeignLanguageLanguageDateWhenCont");
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


        //Load ScientificTitle table and light-box (ajax call)
        private void JSLoadScientificTitles()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE") == UIAccessLevel.Hidden
                                       )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetScientificTitlesTable(reservistId);
                string lightBoxHTML = GetScientificTitleLightBox();
                string UIItems = GetScientificTitleUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
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

        //Save a particular ScientificTitle (ajax call)
        private void JSSaveScientificTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personScientificTitleId = int.Parse(Request.Form["ScientificTitleId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string personScientificTitleKey = Request.Form["ScientificTitleKey"];
                int personScientificTitleYear = int.Parse(Request.Form["ScientificTitleYear"]);
                string personScientificTitleNumberProtocol = Request.Form["ScientificTitleNumberProtocol"];
                string personScientificTitleDesription = Request.Form["ScientificTitleDesription"];


                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonScientificTitle existingPersonScientificTitle = PersonScientificTitleUtil.GetPersonScientificTitle(reservist.Person.IdentNumber, personScientificTitleNumberProtocol, personScientificTitleYear, CurrentUser);

                if (existingPersonScientificTitle != null &&
                    existingPersonScientificTitle.PersonScientificTitleId != personScientificTitleId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраното научно звание вече е въведено за годината</status>";
                }
                else
                {
                    PersonScientificTitle personScientificTitle = new PersonScientificTitle(CurrentUser);

                    personScientificTitle.PersonScientificTitleId = personScientificTitleId;
                    personScientificTitle.ScientificTitleKey = personScientificTitleKey;
                    personScientificTitle.PersonScientificTitleYear = personScientificTitleYear;
                    personScientificTitle.PersonScientificTitleNumberProtocol = personScientificTitleNumberProtocol;
                    personScientificTitle.PersonScientificTitleDesription = personScientificTitleDesription;


                    PersonScientificTitleUtil.SavePersonScientificTitle(personScientificTitle, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedScientificTitleTable = GetScientificTitlesTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedScientificTitleTable>" + AJAXTools.EncodeForXML(refreshedScientificTitleTable) + @"</refreshedScientificTitleTable>";
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

        // Load a particular ScientificTitle (ajax call)
        private void JSLoadScientificTitle()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personScientificTitleId = int.Parse(Request.Form["ScientificTitleId"]);

                PersonScientificTitle personScientificTitle = PersonScientificTitleUtil.GetPersonScientificTitle(personScientificTitleId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personScientificTitle>
                                        <scientificTitleKey>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personScientificTitle.ScientificTitleKey) ? personScientificTitle.ScientificTitleKey : ListItems.GetOptionChooseOne().Value) + @"</scientificTitleKey>
                                        <scientificTitleYear>" + AJAXTools.EncodeForXML(personScientificTitle.PersonScientificTitleYear.HasValue ? personScientificTitle.PersonScientificTitleYear.Value.ToString() : "") + @"</scientificTitleYear>
                                        <scientificTitleNumberProtocol>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personScientificTitle.PersonScientificTitleNumberProtocol) ? personScientificTitle.PersonScientificTitleNumberProtocol : "") + @"</scientificTitleNumberProtocol>
                                        <scientificTitleDesription>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personScientificTitle.PersonScientificTitleDesription) ? personScientificTitle.PersonScientificTitleDesription : "") + @"</scientificTitleDesription>
                                     </personScientificTitle>";
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

        //  Delete a particular ScientificTitle (ajax call)
        private void JSDeleteScientificTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personScientificTitleId = int.Parse(Request.Form["ScientificTitleId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonScientificTitleUtil.DeletePersonScientificTitle(personScientificTitleId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedScientificTitleTable = GetScientificTitlesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedScientificTitleTable>" + AJAXTools.EncodeForXML(refreshedScientificTitleTable) + @"</refreshedScientificTitleTable>";
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

        //  Render the ScientificTitles table
        private string GetScientificTitlesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsScientificTitleScientificTitleNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_SCTTITLE") == UIAccessLevel.Hidden;
            bool IsScientificTitleYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_YEAR") == UIAccessLevel.Hidden;
            bool IsScientificTitleNumberProtocolHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_NUMPROT") == UIAccessLevel.Hidden;
            bool IsScientificTitleDesriptionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_DESCRIPTION") == UIAccessLevel.Hidden;

            if (IsScientificTitleScientificTitleNameHidden &&
              IsScientificTitleYearHidden &&
              IsScientificTitleNumberProtocolHidden &&
              IsScientificTitleDesriptionHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewScientificTitle();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsScientificTitleScientificTitleNameHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Научно звание</th>" : "") + @"
  " + (!IsScientificTitleYearHidden ? @"<th style='width: 70px; vertical-align: bottom;'>Година</th>" : "") + @"                    
  " + (!IsScientificTitleNumberProtocolHidden ? @"<th style='width: 70px; vertical-align: bottom;'>№ Протокол</th>" : "") + @"
  " + (!IsScientificTitleDesriptionHidden ? @"<th style='width: 400px; vertical-align: bottom;'>Научна специалност, къде е</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonScientificTitle> listPersonScientificTitle = PersonScientificTitleUtil.GetAllPersonScientificTitleByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonScientificTitle personScientificTitle in listPersonScientificTitle)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personScientificTitle.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteScientificTitle(" + personScientificTitle.PersonScientificTitleId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditScientificTitle(" + personScientificTitle.PersonScientificTitleId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>

        " + (!IsScientificTitleScientificTitleNameHidden ? @"<td style='text-align: left;'>" + (personScientificTitle.ScientificTitle != null ? personScientificTitle.ScientificTitle.ScientificTitleName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsScientificTitleYearHidden ? @"<td style='text-align: left;'>" + (personScientificTitle.PersonScientificTitleYear.HasValue ? personScientificTitle.PersonScientificTitleYear.Value.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsScientificTitleNumberProtocolHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personScientificTitle.PersonScientificTitleNumberProtocol) ? personScientificTitle.PersonScientificTitleNumberProtocol.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsScientificTitleDesriptionHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personScientificTitle.PersonScientificTitleDesription) ? personScientificTitle.PersonScientificTitleDesription.ToString() : "") + @"</td>" : "") + @"                    

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the ScientificTitles light-box
        private string GetScientificTitleLightBox()
        {
            // Generates html for drop down list
            List<ScientificTitle> listScientificTitle = ScientificTitleUtil.GetAllScientificTitles(CurrentUser);
            List<IDropDownItem> ddiScientificTitle = new List<IDropDownItem>();

            foreach (ScientificTitle personScientificTitle in listScientificTitle)
            {
                ddiScientificTitle.Add(personScientificTitle);
            }

            // Generates html for drop down list
            string PersonScientificTitlesHTML = ListItems.GetDropDownHtml(ddiScientificTitle, null, "ddPersonScientificTitleScientificTitle", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField'", true);

            string html = @"
<center>
    <input type=""hidden"" id=""hdnScientificTitleID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditScientificTitleTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonScientificTitleScientificTitle"" class=""InputLabel"">Научно звание:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonScientificTitlesHTML + @"
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonScientificTitleScientificTitleYear"" class=""InputLabel"">Година:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonScientificTitleScientificTitleYear"" UnsavedCheckSkipMe='true' maxlength=""4"" class=""RequiredInputField"" style=""width: 70px;"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonScientificTitleScientificNumberProtocol"" class=""InputLabel"">№ Протокол</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonScientificTitleScientificNumberProtocol"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""RequiredInputField"" style=""width: 70px;"" />
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; vertical-align: top;"">
                <span id=""lblPersonScientificTitleScientificDesription"" class=""InputLabel"">Научна специалност,<br />къде е:</span>
            </td>
            <td style=""text-align: left;"">
<textarea id='txtPersonScientificTitleScientificDesription' cols='3' rows='5' UnsavedCheckSkipMe='true' class='InputField' style='width: 250px'></textarea>
            </td>
        </tr>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditScientificTitleLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditScientificTitleLightBox"" style=""display: inline;"" onclick=""SaveAddEditScientificTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditScientificTitleLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditScientificTitleLightBox"" style=""display: inline;"" onclick=""HideAddEditScientificTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditScientificTitleLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the ScientificTitle table
        public string GetScientificTitleUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_SCTTITLE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonScientificTitleScientificTitle");
                disabledClientControls.Add("ddPersonScientificTitleScientificTitle");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonScientificTitleScientificTitle");
                hiddenClientControls.Add("ddPersonScientificTitleScientificTitle");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_YEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonScientificTitleScientificTitleYear");
                disabledClientControls.Add("txtPersonScientificTitleScientificTitleYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonScientificTitleScientificTitleYear");
                hiddenClientControls.Add("txtPersonScientificTitleScientificTitleYear");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_NUMPROT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonScientificTitleScientificNumberProtocol");
                disabledClientControls.Add("txtPersonScientificTitleScientificNumberProtocol");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonScientificTitleScientificNumberProtocol");
                hiddenClientControls.Add("txtPersonScientificTitleScientificNumberProtocol");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE_DESCRIPTION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonScientificTitleScientificDesription");
                disabledClientControls.Add("txtPersonScientificTitleScientificDesription");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonScientificTitleScientificDesription");
                hiddenClientControls.Add("txtPersonScientificTitleScientificDesription");
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

        private void JSLoadSpecialities()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY") == UIAccessLevel.Hidden
                                       )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);
            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetSpecialitiesTable(reservist.Person.PersonId);
                string lightBoxHTML = GetSpecialityLightBox();
                string UIItems = GetSpecialityUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
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

        
        private string GetSpecialitiesTable(int pPersonID)
        {
             
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsSpecialtyProfessionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY_PROFESSION") == UIAccessLevel.Hidden;
            bool IsSpecialtySpecialtyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY_SPECIALITY") == UIAccessLevel.Hidden;

            if (IsSpecialtyProfessionHidden && IsSpecialtySpecialtyHidden)
                return "";
            

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY") == UIAccessLevel.Enabled && !isPreview
                                        );

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewSpeciality();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsSpecialtyProfessionHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Професия</th>" : "") + @"
  " + (!IsSpecialtySpecialtyHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Специалност</th>" : "") + @"                     
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonSpeciality> listPersonSpecialities = PersonSpecialityUtil.GetAllPersonSpecialities(pPersonID, CurrentUser);

            foreach (PersonSpeciality personSpeciality in listPersonSpecialities)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personSpeciality.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteSpeciality(" + personSpeciality.PersonSpecialityID.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditSpeciality(" + personSpeciality.PersonSpecialityID.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsSpecialtyProfessionHidden ? @"<td style='text-align: left;'>" + (personSpeciality.Profession != null ? personSpeciality.Profession.ProfessionName : "") + @"</td>" : "") + @"
        " + (!IsSpecialtySpecialtyHidden ? @"<td style='text-align: left;'>" + (personSpeciality.Speciality != null ? personSpeciality.Speciality.SpecialityName : "") + @"</td>" : "") + @"                    
      
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        private void JSLoadSpeciality()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY") == UIAccessLevel.Hidden
                                     )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personSpecialityID = int.Parse(Request.Form["PersonSpecialityID"]);

                PersonSpeciality personSpeciality = PersonSpecialityUtil.GetPersonSpeciality(personSpecialityID, CurrentUser);

                string specialityOptions = "";
                StringBuilder sb = new StringBuilder();

                sb.Append("<specialityOp>");
                sb.Append("<value>" + ListItems.GetOptionChooseOne().Value + "</value>");
                sb.Append("<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>");
                sb.Append("</specialityOp>");

                foreach (Speciality sp in SpecialityUtil.GetSpecialities((personSpeciality != null ? personSpeciality.ProfessionID : 0), CurrentUser))
                {
                    sb.Append("<specialityOp>");
                    sb.Append("<value>" + sp.SpecialityId.ToString() + "</value>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(sp.SpecialityName) + "</name>");
                    sb.Append("</specialityOp>");
                }

                specialityOptions = sb.ToString();

                stat = AJAXTools.OK;
                response = @"<personSpeciality>
                                <profession>" + (personSpeciality != null ? personSpeciality.ProfessionID.ToString() : "" ) + @"</profession>
                                <speciality>" + (personSpeciality != null && personSpeciality.SpecialityID.HasValue ? personSpeciality.SpecialityID.Value.ToString() : "") + @"</speciality>
                                <specialityOps>" + specialityOptions + @"</specialityOps>
                             </personSpeciality>";
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

        private string GetSpecialityLightBox()
        {
            // Generates html for drop down list
            List<Profession> listProfessions = ProfessionUtil.GetAllProfessions(CurrentUser);
            List<IDropDownItem> ddiProfessions = new List<IDropDownItem>();

            foreach (Profession profession in listProfessions)
            {
                ddiProfessions.Add(profession);
            }

            // Generates html for drop down list
            string PersonSpecialityProfessionsHTML = ListItems.GetDropDownHtml(ddiProfessions, null, "ddPersonSpecialityProfession", true, null, "", "style='width: 340px;' UnsavedCheckSkipMe='true' class='RequiredInputField' onchange='ddProfession_Changed();'", true);

                string html = @"
                <center>
                    <input type=""hidden"" id=""hdnPersonSpecialityID"" />
                    <table width=""80%"" style=""text-align: center;"">
                        <colgroup style=""width: 40%"">
                        </colgroup>
                        <colgroup style=""width: 60%"">
                        </colgroup>
                        <tr style=""height: 15px"">
                        </tr>
                        <tr>
                            <td colspan=""2"" align=""center"">
                                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditSpecialityTitle""></span>
                            </td>
                        </tr>
                        <tr style=""height: 15px"">
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonSpecialityProfession"" class=""InputLabel"">Профисия:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + PersonSpecialityProfessionsHTML + @"
                            </td>
                        </tr> 
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonSpecialitySpeciality"" class=""InputLabel"">Специалност:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <select id=""ddPersonSpecialitySpeciality"" style=""width: 340px;"" UnsavedCheckSkipMe=""true"" class=""InputField"" ></select> 
                            </td>
                        </tr>                      
                        <tr style=""height: 46px; padding-top: 5px;"">
                            <td colspan=""2"">
                                <span id=""spanAddEditSpecialityLightBox"" class=""ErrorText"" style=""display: none;"">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""2"" style=""text-align: center;"">
                                <table style=""margin: 0 auto;"">
                                    <tr>
                                        <td>
                                            <div id=""btnSaveAddEditSpecialityLightBox"" style=""display: inline;"" onclick=""SaveAddEditSpecialityLightBox();""
                                                class=""Button"">
                                                <i></i>
                                                <div id=""btnSaveAddEditSpecialityLightBoxText"" style=""width: 70px;"">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id=""btnCloseAddEditSpecialityLightBox"" style=""display: inline;"" onclick=""HideAddEditSpecialityLightBox();""
                                                class=""Button"">
                                                <i></i>
                                                <div id=""btnCloseAddEditSpecialityLightBoxText"" style=""width: 70px;"">
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
        
        public string GetSpecialityUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY_PROFESSION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonSpecialityProfession");
                disabledClientControls.Add("ddPersonSpecialityProfession");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonSpecialityProfession");
                hiddenClientControls.Add("ddPersonSpecialityProfession");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY_SPECIALITY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonSpecialitySpeciality");
                disabledClientControls.Add("ddPersonSpecialitySpeciality");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonSpecialitySpeciality");
                hiddenClientControls.Add("ddPersonSpecialitySpeciality");
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

        private void JSRepopulateSpeciality()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int professionID = 0;

                if (!String.IsNullOrEmpty(Request.Form["ProfessionID"]))
                    professionID = int.Parse(Request.Form["ProfessionID"]);


                string specialityOptions = "";
                StringBuilder sb = new StringBuilder();

                sb.Append("<specialityOp>");
                sb.Append("<value>" + ListItems.GetOptionChooseOne().Value + "</value>");
                sb.Append("<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>");
                sb.Append("</specialityOp>");

                foreach (Speciality sp in SpecialityUtil.GetSpecialities(professionID, CurrentUser))
                {
                    sb.Append("<specialityOp>");
                    sb.Append("<value>" + sp.SpecialityId.ToString() + "</value>");
                    sb.Append("<name>" + AJAXTools.EncodeForXML(sp.SpecialityName) + "</name>");
                    sb.Append("</specialityOp>");
                }

                specialityOptions = sb.ToString();

                response = "<specialities>" + specialityOptions + "</specialities>";
            
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

        private void JSSaveSpeciality()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personSpecialityID = int.Parse(Request.Form["PersonSpecialityID"]);
                int reservistID = int.Parse(Request.Form["ReservistID"]);
                int professionID = int.Parse(Request.Form["ProfessionID"]);

                int? specialityID = null;
                if (Request.Form["SpecialityID"] != ListItems.GetOptionChooseOne().Value)
                    specialityID = int.Parse(Request.Form["SpecialityID"]);

           

                Reservist reservist = ReservistUtil.GetReservist(reservistID, CurrentUser);

                PersonSpeciality existingPersonSpeciality = PersonSpecialityUtil.GetPersonSpeciality(personSpecialityID, CurrentUser);

                if (existingPersonSpeciality != null &&
                    existingPersonSpeciality.PersonSpecialityID != personSpecialityID)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Вече е въведена такава специалност</status>";
                }
                else
                {
                    PersonSpeciality personSpeciality = new PersonSpeciality(CurrentUser);

                    personSpeciality.PersonSpecialityID = personSpecialityID;
                    personSpeciality.ProfessionID = professionID;
                    personSpeciality.SpecialityID = specialityID;

                    PersonSpecialityUtil.SavePersonSpeciality(personSpeciality, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedSpecialityTable = GetSpecialitiesTable(reservistID);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedSpecialityTable>" + AJAXTools.EncodeForXML(refreshedSpecialityTable) + @"</refreshedSpecialityTable>";
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

        private void JSDeleteSpeciality()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personSpecialityID = int.Parse(Request.Form["PersonSpecialityID"]);
                int reservistID = int.Parse(Request.Form["ReservistID"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistID, CurrentUser);

                PersonSpecialityUtil.DeletePersonSpeciality(personSpecialityID, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedSpecialityTable = GetSpecialitiesTable(reservistID);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedSpecialityTable>" + AJAXTools.EncodeForXML(refreshedSpecialityTable) + @"</refreshedSpecialityTable>";
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

    public static class AddEditReservist_EducationWork_PageUtil
    {
        public static string GetTabContent(AddEditReservist page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string html = @" <div style=""width: 900px; overflow-x: auto;"">";

            // Section reservist Workplace
            int reservistId = page.ReservistId;
            Reservist reservist = ReservistUtil.GetReservist(reservistId, page.CurrentUser);

            html += @"<input type=""hidden"" id=""hdnCompanyID"" value=""" + (reservist.Person.WorkCompany != null ? reservist.Person.WorkCompany.CompanyId.ToString() : "") + @"""/>
                      <input type=""hidden"" id=""hdnWorkPositionNKPDID"" value=""" + (reservist.Person.WorkPositionNKPD != null ? reservist.Person.WorkPositionNKPD.Id.ToString() : "") + @"""/>";

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE") != UIAccessLevel.Hidden)
            {
                bool IsWorkCompanyHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE_COMPANY") == UIAccessLevel.Hidden;
                bool IsWorkPositionHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE_POSITION") == UIAccessLevel.Hidden;

                bool IsWorkCompanyDisabled = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE_COMPANY") == UIAccessLevel.Disabled;
                bool IsWorkPositionDisabled = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE_POSITION") == UIAccessLevel.Disabled;

                html += @"
                    <fieldset style=""width: 880px; padding: 0px; margin-top: 10px; float: left;"">
                        <table id=""tblRequestHeaderSection"" class=""InputRegion"" style=""width: 880px; padding: 0 10px 10px 10px; margin-top: 0px;"">
                            <tr>
                                <td colspan=""3"" style=""text-align: left;"">
                                   <span id=""lblWorkPlaceTitle"" style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;"">Месторабота</span>
                                </td>
                                <td style=""text-align: right; padding-bottom: 5px;"">
                                   <div style=""position: relative; left: 10px; " + ((IsWorkCompanyDisabled && IsWorkPositionDisabled || isPreview || IsWorkCompanyHidden && IsWorkPositionHidden || IsWorkCompanyHidden && IsWorkPositionDisabled || IsWorkCompanyDisabled && IsWorkPositionHidden) ? "display: none;" : "inline") + @""">
                                      <img runat=""server"" id=""btnImgSave"" src=""../Images/save.png"" alt=""Запис"" title=""Запис"" class=""GridActionIcon"" onclick=""IsWorkplaceDataValid(btnSavePersonWorkPlaceData_Click);"" />
                                   </div>
                                </td>
                            </tr>
                            <tr>
                                <td style=""text-align: left; width: 30%;"">
                                    <span id=""lblCompanyName"" style=""padding-right: 5px; " + (IsWorkCompanyHidden ? "display: none;'" : "") + @""" class=""InputLabel"">Име на фирмата:</span>
                                </td>
                                <td style=""text-align: left; width: 18%;"">
                                    <span id=""lblBulstat"" class=""InputLabel"" " + (IsWorkCompanyHidden ? "style='display: none;'" : "") + @">" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":</span>
                                </td>
                                <td style=""text-align: left; width: 22%;"">
                                    <span id=""lblOwnershipType"" class=""InputLabel"" " + (IsWorkCompanyHidden ? "style='display: none;'" : "") + @">Вид собственост:</span>
                                </td>
                                <td style=""text-align: left; width: 30%;"">
                                    <span id=""lblWorkPositionNKPD"" class=""InputLabel"" " + (IsWorkPositionHidden ? "style='display: none;'" : "") + @">Код по НКПД:</span>
                                </td>
                            </tr>
                            <tr>
                                <td style=""text-align: left; vertical-align: top;"">
                                    <div id=""lblCompanyNameValue"" class=""ReadOnlyValue"" style=""background-color: #FFFFCC; " + (IsWorkCompanyHidden ? "display: none;" : "") + @""">" + (reservist.Person.WorkCompany != null ? reservist.Person.WorkCompany.CompanyName : "&nbsp;") + @"</div>
                                    <input id=""btnSelectCompany"" 
                                           type=""button"" 
                                           value=""Търсене на фирма"" 
                                           class=""OpenCompanySelectorButton"" 
                                           style=""" + (IsWorkCompanyHidden || IsWorkCompanyDisabled || isPreview ? "display: none;" : "") + @""" 
                                           onclick='companySelector.showDialog(""companySelectorForWorkplace"", CompanySelector_OnSelectedCompany);' />
                                </td>
                                <td style=""text-align: left; vertical-align: top;"">
                                    <span id=""lblUnifiedIdentityCodeValue"" class=""ReadOnlyValue"" style=""" + (IsWorkCompanyHidden ? "display: none;" : "") + @""">" + (reservist.Person.WorkCompany != null ? reservist.Person.WorkCompany.UnifiedIdentityCode : "") + @"</span>
                                </td>
                                <td style=""text-align: left; vertical-align: top; position: relative;"" nowrap>
                                    <span id=""lblOwnershipTypeValue"" class=""ReadOnlyValue"" style=""" + (IsWorkCompanyHidden ? "display: none;" : "") + @""">" + (reservist.Person.WorkCompany != null && reservist.Person.WorkCompany.OwnershipType != null ? reservist.Person.WorkCompany.OwnershipType.OwnershipTypeName : "") + @"</span>
                                </td>
                                <td style=""text-align: left; vertical-align: top; height: 60px;"">
                                    <input id=""txtWorkPositionNKPDCode"" " + ((IsWorkPositionDisabled || isPreview) ? "disabled='true'" : "") + @" class=""InputField"" style='width: 90px; " + (IsWorkPositionHidden ? "display: none;" : "") + @"' maxLength=""20"" value=""" + (reservist.Person.WorkPositionNKPD != null ? reservist.Person.WorkPositionNKPD.CodeDisplay : "") + @""" onfocus='txtWorkPositionNKPDCode_Focus()' onblur='txtWorkPositionNKPDCode_Blur()' /> 
                                    <img id=""imgOpenSearchNKPD"" src='../Images/view_list.png' onclick='ShowSearchNKPDLightBox();' style='cursor:pointer; vertical-align:top; " + (IsWorkPositionHidden || IsWorkPositionDisabled || isPreview ? "display: none;" : "") + @"' title='Търсене по НКПД'/><br />
                                    <span id=""lblWorkPositionNKPDMessage"" class=""ReadOnlyValue"" style=""font-size: 0.8em; " + (IsWorkPositionHidden ? "display: none;" : "") + @""">" + (reservist.Person.WorkPositionNKPD != null ? reservist.Person.WorkPositionNKPD.Name : "") + @"</span>
                                    <div id=""lboxSearchNKPD"" style=""display: none;"" class=""lboxSearchNKPD"">
                                            <img src='../Images/Close.png' style='cursor: pointer; float:right;' onclick='HideWorkPositionSearchNKPDCodeLightBox();' />

                                            <table>
                                                <tr>
                                                    <td colspan=""2"" style=""text-align: center; padding-top: 10px; padding-bottom: 10px; font-size: 0.95em; font-weight: bold;"">
                                                        Списък на длъжностите в Националната класификация на професиите и длъжностите
                                                    </td>
                                                </tr>
                                                
                                                <tr style=""min-height: 17px"">
                                                    <td style=""text-align: right; width: 50px;"">
                                                        <span id=""lblNKPDLevel1"" class=""InputLabel"">Клас:</span>
                                                    </td>
                                                    <td style=""text-align: left;"">
                                                        <select id='ddNKPDLevel1' style='width: 690px;' onchange='ddNKPDLevel_Changed(1);'></select>
                                                    </td>
                                                </tr>
                                                <tr style=""min-height: 17px"">
                                                    <td style=""text-align: right;"">
                                                    </td>
                                                    <td style=""text-align: left;"">
                                                        <select id='ddNKPDLevel2' style='width: 690px;' onchange='ddNKPDLevel_Changed(2);'></select>
                                                    </td>
                                                </tr>
                                                <tr style=""min-height: 17px"">
                                                    <td style=""text-align: right;"">
                                                    </td>
                                                    <td style=""text-align: left;"">
                                                        <select id='ddNKPDLevel3' style='width: 690px;' onchange='ddNKPDLevel_Changed(3);'></select>
                                                    </td>
                                                </tr>
                                                 <tr style=""min-height: 17px"">
                                                    <td style=""text-align: right;"">
                                                    </td>
                                                    <td style=""text-align: left;"">
                                                        <select id='ddNKPDLevel4' style='width: 690px;'></select>
                                                    </td>
                                                </tr>
                                                <tr style=""min-height: 17px"">
                                                    <td style=""text-align: right;"">
                                                        <span id=""lblNKPDCodeFilter"" class=""InputLabel"">Код:</span>
                                                    </td>
                                                    <td style=""text-align: left;"">
                                                        <input type='text' id='txtNKPDCodeFilter' class='InputField' style='width: 120px;' />&nbsp;&nbsp;
                                                        <span id=""lblNKPDNameFilter"" class=""InputLabel"">Длъжност:</span>
                                                        <input type='text' id='txtNKPDNameFilter' class='InputField' style='width: 480px;' />
                                                    </td>
                                                </tr>
                                                <tr style=""min-height: 17px""> 
                                                    <td colspan=""2"" style=""text-align: center;""> 
                                                        <table style=""margin: 0 auto;"">
                                                           <tr>
                                                              <td>
                                                                 <div id=""btnSearchNKPDLightBox"" style=""display: inline;"" onclick=""btnSearchNKPDLightBox_Click();""
                                                                     class=""Button"">
                                                                     <i></i>
                                                                     <div id=""btnSearchNKPDLightBoxText"" style=""width: 70px;"">
                                                                         Търси</div>
                                                                     <b></b>
                                                                 </div>
                                                              </td>
                                                           </tr>
                                                        </table>
                                                    </td>
                                                </tr>  
                                                <tr style=""min-height: 17px"">
                                                   <td colspan=""2"" style=""text-align: left;""> 
                                                      <div id='pnlNKPDResult'>
                                                      </div>
                                                   </td>
                                                </tr>
                                            </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan=""4"" style=""text-align: center;"">
                                    <span id=""lblWorkplaceDataMessage""></span>
                                </td>
                            </tr>                         
                        </table>           
                    </fieldset>";
            }

            //Table ForeignLanguage

            html += @"<div style=""width: 900px; overflow-x: auto;"">
                     ";

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_FRGNLNG") != UIAccessLevel.Hidden)
            {
                html += @"  

                        <div style=""height: 10px;""></div>
                        <div id=""divForeignLanguage"">
                           <div style=""text-align: left;"">
                              <span id=""lnkForeignLanguage"" class=""DataTableExpandLink"" onclick=""lnkForeignLanguage_Click();"">Чужди езици</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingForeignLanguage"" />
                           </div>
                           <div id=""tblForeignLanguage"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlForeignLanguageMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageForeignLanguage""></span>
                           </div>
                           <div id=""lboxForeignLanguage"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnForeignLanguageLoaded"" value=""0"" />
                        </div>";
            }

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_CVLEDU") != UIAccessLevel.Hidden)
            {
                //Table CivilEducation
                html += @"

                    <div style=""height: 10px;""></div>
                    <div id=""divCivilEducation"">
                       <div style=""text-align: left;"">
                          <span id=""lnkCivilEducation"" class=""DataTableExpandLink"" onclick=""lnkCivilEducation_Click();"">Гражданско образование</span>
                          <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingCivilEducation"" />
                       </div>
                       <div id=""tblCivilEducation"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                       <div id=""pnlCivilEducationMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                          <span id=""lblMessageCivilEducation""></span>
                       </div>
                       <div id=""lboxCivilEducation"" style=""display: none;"" class=""lboxCivilEducation""></div>
                       <input type=""hidden"" id=""hdnCivilEducationLoaded"" value=""0"" />
                    </div>";

            }

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VU") != UIAccessLevel.Hidden || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_MLTEDU_VA") != UIAccessLevel.Hidden)
            {
                //Table MilitaryEducation
                html += @"<div style=""height: 10px;""></div>
<div id=""divMilitaryEducation"">

   <div style=""text-align: left;"">
      <span id=""lnkMilitaryEducation"" class=""DataTableExpandLink"" onclick=""lnkMilitaryEducation_Click();"">Военно образование</span>
      <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingMilitaryEducation"" />
   </div>

   <div id=""tblMilitaryEducation"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>

   <div id=""pnlMilitaryEducationMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
      <span id=""lblMessageMilitaryEducation""></span>
   </div>

   <div id=""lboxMilitaryEducation"" style=""display: none;"" class=""lboxCivilEducation""></div>

   <input type=""hidden"" id=""hdnMilitaryEducationLoaded"" value=""0"" />


   <div id=""tblMilitaryEducationAcademy"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>

   <div id=""pnlMilitaryEducationAcademyMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
      <span id=""lblMessageMilitaryEducationAcademy""></span>
   </div>

   <div id=""lboxMilitaryEducationAcademy"" style=""display: none;"" class=""lboxCivilEducation""></div>

   <input type=""hidden"" id=""hdnMilitaryEducationAcademyLoaded"" value=""0"" />

</div>";

            }


            //Table Trraining course

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_TRNCRS") != UIAccessLevel.Hidden)
            {
                html += @"<div style=""height: 10px;""></div>
                                <div id=""divTrainigCource"">
                                   <div style=""text-align: left;"">
                                      <span id=""lnkTrainigCource"" class=""DataTableExpandLink"" onclick='lnkTrainigCource_Click();'>Курсова подготовка</span>
                                      <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingTrainigCource"" />
                                   </div>
                                   <div id=""tblTrainigCource"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                                   <div id=""pnlTrainigCourceMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                                      <span id=""lblMessageTrainigCource""></span>
                                   </div>
                                   <div id=""lboxTrainigCource"" style=""display: none;"" class=""lboxCivilEducation""></div>
                                   <input type=""hidden"" id=""hdnTrainigCourceLoaded"" value=""0"" />
                                </div>
                         ";
            }

            //Table ScientificTitle
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SCTTITLE") != UIAccessLevel.Hidden)
            {
                html += @"
                    <div style=""width: 900px; overflow-x: auto;"">

                    <div style=""height: 10px;""></div>
                    <div id=""divScientificTitle"">
                       <div style=""text-align: left;"">
                          <span id=""lnkScientificTitle"" class=""DataTableExpandLink"" onclick=""lnkScientificTitle_Click();"">Научно звание</span>
                          <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingScientificTitle"" />
                       </div>
                       <div id=""tblScientificTitle"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                       <div id=""pnlScientificTitleMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                          <span id=""lblMessageScientificTitle""></span>
                       </div>
                       <div id=""lboxScientificTitle"" style=""display: none;"" class=""lboxCivilEducation""></div>
                       <input type=""hidden"" id=""hdnScientificTitleLoaded"" value=""0"" />
                    </div>

                ";
            }
                     
            //Table Speciality
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_SPECIALITY") != UIAccessLevel.Hidden)
            {
                html += @"
                    <div style=""width: 900px; overflow-x: auto;"">

                    <div style=""height: 10px;""></div>
                    <div id=""divScientificTitle"">
                       <div style=""text-align: left;"">
                          <span id=""lnkScientificTitle"" class=""DataTableExpandLink"" onclick=""lnkSpeciality_Click();"">Професия и специалност</span>
                          <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingSpeciality"" />
                       </div>
                       <div id=""tblSpeciality"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                       <div id=""pnlSpecialityMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                          <span id=""lblMessageSpeciality""></span>
                       </div>
                       <div id=""lboxSpeciality"" style=""display: none;"" class=""lboxPersonSpeciality""></div>
                       <input type=""hidden"" id=""hdnSpecialityLoaded"" value=""0"" />
                    </div>
                ";
              
            }         

            html += @"</div>";
           

            return html;
        }

        public static string GetTabWorkPlaceContentUIItems(AddEditReservist page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string UIItemsXML = "";

            bool screenDisabled = false;
            bool workPlaceDisabled = false;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            screenDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                             page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled || isPreview;

            workPlaceDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                   page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                   page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Disabled ||
                                   page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE") == UIAccessLevel.Disabled;

            if(screenDisabled || workPlaceDisabled)
                hiddenClientControls.Add("btnImgSave");

            UIAccessLevel l;

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK_WORKPLACE");

            if (l == UIAccessLevel.Disabled || screenDisabled || workPlaceDisabled)
            {
                disabledClientControls.Add("lblBulstat");
                disabledClientControls.Add("lblUnifiedIdentityCodeValue");
                hiddenClientControls.Add("btnSelectCompany");

                disabledClientControls.Add("lblCompanyName");
                disabledClientControls.Add("lblCompanyNameValue");

                disabledClientControls.Add("lblOwnershipType");
                disabledClientControls.Add("lblOwnershipTypeValue");

                disabledClientControls.Add("lblWorkPositionNKPD");
                disabledClientControls.Add("txtWorkPositionNKPDCode");
                disabledClientControls.Add("lblWorkPositionNKPDMessage");
                hiddenClientControls.Add("imgOpenSearchNKPD");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblWorkPlaceTitle");
                hiddenClientControls.Add("lblBulstat");
                hiddenClientControls.Add("lblUnifiedIdentityCodeValue");
                hiddenClientControls.Add("btnSelectCompany");

                hiddenClientControls.Add("lblCompanyName");
                hiddenClientControls.Add("lblCompanyNameValue");

                hiddenClientControls.Add("lblOwnershipType");
                hiddenClientControls.Add("lblOwnershipTypeValue");

                hiddenClientControls.Add("lblWorkPositionNKPD");
                hiddenClientControls.Add("txtWorkPositionNKPDCode");
                hiddenClientControls.Add("lblWorkPositionNKPDMessage");
                hiddenClientControls.Add("imgOpenSearchNKPD");
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
