using System;
using System.Collections.Generic;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist_MilitaryReport : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadNewMilRepStatuses")
            {
                JSLoadNewMilRepStatuses();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilRepSpecs")
            {
                JSLoadMilRepSpecs();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilRepSpec")
            {
                JSLoadMilRepSpec();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilRepSpec")
            {
                JSSaveMilRepSpec();
                return;
            }


            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPositionTitle")
            {
                JSLoadPositionTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePositionTitle")
            {
                JSSavePositionTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePositionTitle")
            {
                JSDeletePositionTitle();
                return;
            }


            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilRepStatus")
            {
                JSSaveMilRepStatus();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadReservistMilRepStatus")
            {
                JSLoadReservistMilRepStatus();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilRepStatusHistory")
            {
                JSLoadMilRepStatusHistory();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadReservistAppointmentHistory")
            {
                JSLoadReservistAppointmentHistory();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilRepSpec")
            {
                JSDeleteMilRepSpec();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilitaryReportTab")
            {
                JSSaveMilitaryReportTab();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintMK")
            {
                JSPrintMK();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintPZ")
            {
                JSPrintPZ();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintАK")
            {
                JSPrintАK();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintАSK")
            {
                JSPrintАSK();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintUO")
            {
                JSPrintUO();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadReservistMilitaryReportStatusSection")
            {
                JSLoadReservistMilitaryReportStatusSection();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSTransferToVitosha")
            {
                int reservistID = int.Parse(Request.Params["ReservistID"]);
                int militaryUnitID = int.Parse(Request.Params["MilitaryUnitID"]);

                JSTransferToVitosha(reservistID, militaryUnitID);
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMedCert")
            {
                JSLoadMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMedCert")
            {
                JSSaveMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMedCert")
            {
                JSDeleteMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPsychCert")
            {
                JSLoadPsychCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePsychCert")
            {
                JSSavePsychCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePsychCert")
            {
                JSDeletePsychCert();
                return;
            }
        }                          

        //Get the available new statuses for a particular Reservist (ajax call)
        private void JSLoadNewMilRepStatuses()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_MILREPSTATUS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                ReservistMilRepStatus currentResMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservistId, CurrentUser);

                List<MilitaryReportStatus> allStatuses = MilitaryReportStatusUtil.GetAllMilitaryReportStatuses(CurrentUser);

                //If there is a current status then filter the statuses
                if (currentResMilRepStatus != null)
                {
                    string currResMilRepStatus = currentResMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey;

                    //Remove the current status from the list
                    allStatuses.RemoveAll(x => x.MilitaryReportStatusKey == currResMilRepStatus);
                }

                allStatuses.RemoveAll(x => x.MilitaryReportStatusKey == "COMPULSORY_RESERVE_MOB_APPOINTMENT");

                response = "<statuses>";

                response += "<s>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<key>" + "" + "</key>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</s>";                

                foreach (MilitaryReportStatus status in allStatuses)
                {
                    response += "<s>" +
                                "<id>" + status.MilitaryReportStatusId.ToString() + "</id>" +
                                "<key>" + status.MilitaryReportStatusKey + "</key>" +
                                "<name>" + AJAXTools.EncodeForXML(status.MilitaryReportStatusName) + "</name>" +
                                "</s>";
                }

                response += "<mildeptid>" + (currentResMilRepStatus != null ? currentResMilRepStatus.SourceMilDepartmentId.ToString() : "-1") + "</mildeptid>";

                response += "</statuses>";

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

        //Get the military report specialities for a type (ajax call)
        private void JSLoadMilRepSpecs()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int milRepSpecTypeID = int.Parse(Request.Form["MilRepSpecTypeID"]);                

                List<MilitaryReportSpeciality> milRepSpecs = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, milRepSpecTypeID);              

                response = "<milrepspecs>";

                response += "<m>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</m>";

                foreach (MilitaryReportSpeciality speciality in milRepSpecs)
                {
                    response += "<m>" +
                                "<id>" + speciality.MilReportSpecialityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(speciality.CodeAndName) + "</name>" +
                                "</m>";
                }

                response += "</milrepspecs>";

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

        private void JSLoadMilRepSpec()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personMilRepSpecID = int.Parse(Request.Form["PersonMilRepSpecID"]);

                PersonMilitaryReportSpeciality personMilitaryReportSpeciality = PersonMilitaryReportSpecialityUtil.GetPersonMilitaryReportSpeciality(personMilRepSpecID, CurrentUser);                

                response = "<response>";

                response += "<milrepspectypeid>" + personMilitaryReportSpeciality.MilitaryReportSpeciality.MilReportSpecialityTypeId + "</milrepspectypeid>";

                response += "<milrepspecid>" + personMilitaryReportSpeciality.MilitaryReportSpeciality.MilReportSpecialityId + "</milrepspecid>";
                response += "<isPrimary>" + (personMilitaryReportSpeciality.IsPrimary ? "1" : "0") + "</isPrimary>";

                response += "<milrepspec>";

                List<MilitaryReportSpeciality> milRepSpecs = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, personMilitaryReportSpeciality.MilitaryReportSpeciality.MilReportSpecialityTypeId);

                foreach (MilitaryReportSpeciality mrs in milRepSpecs)
                {
                    response += "<mrs>";
                    response += "<id>" + mrs.MilReportSpecialityId + "</id>";
                    response += "<name>" + AJAXTools.EncodeForXML(mrs.CodeAndName) + "</name>";
                    response += "</mrs>";
                }

                if ((from m in milRepSpecs 
                     where m.MilReportSpecialityId == personMilitaryReportSpeciality.MilitaryReportSpecialityID 
                     select m).Count() == 0)
                {
                    MilitaryReportSpeciality mrs = personMilitaryReportSpeciality.MilitaryReportSpeciality;

                    response += "<mrs>";
                    response += "<id>" + mrs.MilReportSpecialityId + "</id>";
                    response += "<name>" + AJAXTools.EncodeForXML(mrs.CodeAndName) + "</name>";
                    response += "</mrs>";
                }

                response += "</milrepspec>";

                response += "</response>";

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

        //Save a particular Person Military Report Speciality (ajax call)
        private void JSSaveMilRepSpec()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personMilRepSpecID = int.Parse(Request.Form["PersonMilRepSpecID"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                int milRepSpecId = int.Parse(Request.Form["MilRepSpecId"]);
                bool isPrimary = Request.Form["IsPrimary"] == "1";

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryReportSpeciality existingPersonMilitaryReportSpeciality = PersonMilitaryReportSpecialityUtil.GetPersonMilitaryReportSpeciality(reservist.PersonId, milRepSpecId, CurrentUser);

                if (existingPersonMilitaryReportSpeciality != null &&
                    existingPersonMilitaryReportSpeciality.PersonMilRepSpecID != personMilRepSpecID)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Вече е въведена такава ВОС</status>";
                }
                else
                {
                    PersonMilitaryReportSpeciality personMilitaryReportSpeciality = new PersonMilitaryReportSpeciality(CurrentUser);

                    personMilitaryReportSpeciality.PersonMilRepSpecID = personMilRepSpecID;
                    personMilitaryReportSpeciality.PersonID = reservist.PersonId;
                    personMilitaryReportSpeciality.MilitaryReportSpecialityID = milRepSpecId;
                    personMilitaryReportSpeciality.IsPrimary = isPrimary;

                    PersonMilitaryReportSpecialityUtil.SavePersonMilitaryReportSpeciality(personMilitaryReportSpeciality, CurrentUser, change);                   

                    change.WriteLog();                    

                    string refreshedPersonMilitaryReportSpecialityTable = AddEditReservist_MilitaryReport_PageUtil.GetMilRepSpecTable(reservist, CurrentUser, this);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedMilRepSpecTable>" + AJAXTools.EncodeForXML(refreshedPersonMilitaryReportSpecialityTable) + @"</refreshedMilRepSpecTable>";

                    List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(reservist.PersonId, CurrentUser);
                    foreach (PersonMilitaryReportSpeciality personMilitaryReportSpecialitie in personMilitaryReportSpecialities)
                    {
                        if (personMilitaryReportSpecialitie.IsPrimary)
                        {
                            response += @"
                                          <basePersonMilitaryReportSpecialityCode>" + AJAXTools.EncodeForXML(personMilitaryReportSpecialitie.MilitaryReportSpeciality.MilReportingSpecialityCode) + "</basePersonMilitaryReportSpecialityCode>";
                            break;
                        }
                    }

                    if (personMilitaryReportSpecialities.Count == 0)
                    {
                        response += @"
                                          <basePersonMilitaryReportSpecialityCode></basePersonMilitaryReportSpecialityCode>";
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

        //Delete a particular Person Military Report Speciality (ajax call)
        private void JSDeleteMilRepSpec()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personMilRepSpecID = int.Parse(Request.Params["PersonMilRepSpecID"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMilitaryReportSpecialityUtil.DeletePersonMilitaryReportSpeciality(personMilRepSpecID, CurrentUser, change);                

                change.WriteLog();

                string refreshedPersonMilitaryReportSpecialityTable = AddEditReservist_MilitaryReport_PageUtil.GetMilRepSpecTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMilRepSpecTable>" + AJAXTools.EncodeForXML(refreshedPersonMilitaryReportSpecialityTable) + @"</refreshedMilRepSpecTable>";

                List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(reservist.PersonId, CurrentUser);
                foreach (PersonMilitaryReportSpeciality personMilitaryReportSpecialitie in personMilitaryReportSpecialities)
                {
                    if (personMilitaryReportSpecialitie.IsPrimary)
                    {
                        response += @"
                                          <basePersonMilitaryReportSpecialityCode>" + AJAXTools.EncodeForXML(personMilitaryReportSpecialitie.MilitaryReportSpeciality.MilReportingSpecialityCode) + "</basePersonMilitaryReportSpecialityCode>";
                        break;
                    }
                }

                if (personMilitaryReportSpecialities.Count == 0)
                {
                    response += @"
                                          <basePersonMilitaryReportSpecialityCode></basePersonMilitaryReportSpecialityCode>";
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



        private void JSLoadPositionTitle()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_POSITIONTITLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personPositionTitleID = int.Parse(Request.Form["PersonPositionTitleID"]);

                PersonPositionTitle personPositionTitle = PersonPositionTitleUtil.GetPersonPositionTitle(personPositionTitleID, CurrentUser);

                response = "<response>";

                response += "<positiontitleid>" + personPositionTitle.PositionTitle.PositionTitleId + "</positiontitleid>";
                response += "<positiontitlename>" + personPositionTitle.PositionTitle.PositionTitleName + "</positiontitlename>";
                response += "<isPrimary>" + (personPositionTitle.IsPrimary ? "1" : "0") + "</isPrimary>";

                response += "</response>";

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

        //Save a particular Person Position Title (ajax call)
        private void JSSavePositionTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPositionTitleID = int.Parse(Request.Form["PersonPositionTitleID"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                int positionTitleId = int.Parse(Request.Form["PositionTitleId"]);
                bool isPrimary = Request.Form["IsPrimary"] == "1";

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPositionTitle existingPersonPositionTitle = PersonPositionTitleUtil.GetPersonPositionTitle(reservist.PersonId, positionTitleId, CurrentUser);

                if (existingPersonPositionTitle != null &&
                    existingPersonPositionTitle.PersonPositionTitleID != personPositionTitleID)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Вече е въведена такава длъжност</status>";
                }
                else
                {
                    PersonPositionTitle personPersonPositionTitle = new PersonPositionTitle(CurrentUser);

                    personPersonPositionTitle.PersonPositionTitleID = personPositionTitleID;
                    personPersonPositionTitle.PersonID = reservist.PersonId;
                    personPersonPositionTitle.PositionTitleID = positionTitleId;
                    personPersonPositionTitle.IsPrimary = isPrimary;

                    PersonPositionTitleUtil.SavePersonPositionTitle(personPersonPositionTitle, CurrentUser, change);

                    change.WriteLog();

                    string refreshedPersonPositionTitleTable = AddEditReservist_MilitaryReport_PageUtil.GetPositionTitlesTable(reservist, CurrentUser, this);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedPositionTitleTable>" + AJAXTools.EncodeForXML(refreshedPersonPositionTitleTable) + @"</refreshedPositionTitleTable>";

                    List<PersonPositionTitle> personPositionTitles = PersonPositionTitleUtil.GetAllPersonPositionTitles(reservist.PersonId, CurrentUser);
                    foreach (PersonPositionTitle personPositionTitle in personPositionTitles)
                    {
                        if (personPositionTitle.IsPrimary)
                        {
                            response += @"
                                          <basePersonPositionTitle>" + AJAXTools.EncodeForXML(personPositionTitle.PositionTitle.PositionTitleName) + "</basePersonPositionTitle>";
                            break;
                        }
                    }

                    if (personPositionTitles.Count == 0)
                    {
                        response += @"
                                          <basePersonPositionTitle></basePersonPositionTitle>";
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

        //Delete a particular Person Position Title (ajax call)
        private void JSDeletePositionTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPositionTitleID = int.Parse(Request.Params["PersonPositionTitleID"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPositionTitleUtil.DeletePersonPositionTitle(personPositionTitleID, CurrentUser, change);

                change.WriteLog();

                string refreshedPersonPositionTitleTable = AddEditReservist_MilitaryReport_PageUtil.GetPositionTitlesTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPositionTitleTable>" + AJAXTools.EncodeForXML(refreshedPersonPositionTitleTable) + @"</refreshedPositionTitleTable>";

                List<PersonPositionTitle> personPositionTitles = PersonPositionTitleUtil.GetAllPersonPositionTitles(reservist.PersonId, CurrentUser);
                foreach (PersonPositionTitle personPositionTitle in personPositionTitles)
                {
                    if (personPositionTitle.IsPrimary)
                    {
                        response += @"
                                          <basePersonPositionTitle>" + AJAXTools.EncodeForXML(personPositionTitle.PositionTitle.PositionTitleName) + "</basePersonPositionTitle>";
                        break;
                    }
                }

                if (personPositionTitles.Count == 0)
                {
                    response += @"
                                          <basePersonPositionTitle></basePersonPositionTitle>";
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



        private void JSLoadReservistMilRepStatus()
        {            
            string stat = "";
            string response = "";

            try
            {
                int reservistMilRepStatusId = int.Parse(Request.Form["ReservistMilRepStatusId"]);

                ReservistMilRepStatus reservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepStatus(reservistMilRepStatusId, CurrentUser);
                List<VoluntaryReserveAnnex> voluntaryReserveAnnexes = VoluntaryReserveAnnexUtil.GetVoluntaryReserveAnnexesByReservistMilRepStatusId(reservistMilRepStatusId, CurrentUser);

                response = "<response>";

                response += "<reservistMilRepStatus>";
                response += "<MilitaryReportStatusID>" + reservistMilRepStatus.MilitaryReportStatusId + "</MilitaryReportStatusID>";
                response += "<MilitaryReportStatusKey>" + reservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey + "</MilitaryReportStatusKey>";
                response += "<EnrolDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.EnrolDate) + "</EnrolDate>";
                response += "<SourceMilDepartmentID>" + reservistMilRepStatus.SourceMilDepartmentId + "</SourceMilDepartmentID>";
                response += "<VoluntaryContractNumber>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Voluntary_ContractNumber) + "</VoluntaryContractNumber>";
                response += "<VoluntaryContractDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ContractDate) + "</VoluntaryContractDate>";
                response += "<VoluntaryExpireDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ExpireDate) + "</VoluntaryExpireDate>";
                response += "<VoluntaryDurationMonths>" + reservistMilRepStatus.Voluntary_DurationMonths + "</VoluntaryDurationMonths>";
                response += "<Annexes>";
                for (int i = 0; i < voluntaryReserveAnnexes.Count; i++)
                {
                    response += "<Annex>";
                    response += "<AnnexID>" + voluntaryReserveAnnexes[i].VoluntaryReserveAnnexId + "</AnnexID>";
                    response += "<AnnexNumber>" + AJAXTools.EncodeForXML(voluntaryReserveAnnexes[i].AnnexNumber) + "</AnnexNumber>";
                    response += "<AnnexDate>" + CommonFunctions.FormatDate(voluntaryReserveAnnexes[i].AnnexDate) + "</AnnexDate>";
                    response += "<AnnexDurationMonths>" + voluntaryReserveAnnexes[i].AnnexDurationMonths + "</AnnexDurationMonths>";
                    response += "<AnnexExpireDate>" + CommonFunctions.FormatDate(voluntaryReserveAnnexes[i].AnnexExpireDate) + "</AnnexExpireDate>";
                    response += "</Annex>";
                }
                response += "</Annexes>";
                response += "<VoluntaryFulfilPlaceID>" + (reservistMilRepStatus.Voluntary_FulfilPlaceID != null ? reservistMilRepStatus.Voluntary_FulfilPlaceID.Value.ToString() : "-1" ) + "</VoluntaryFulfilPlaceID>";
                response += "<VoluntaryFulfilPlaceText>" + AJAXTools.EncodeForXML((reservistMilRepStatus.Voluntary_FulfilPlace != null ? reservistMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""))  + "</VoluntaryFulfilPlaceText>";
                response += "<VoluntaryMilitaryRankID>" + reservistMilRepStatus.Voluntary_MilitaryRankId + "</VoluntaryMilitaryRankID>";
                response += "<VoluntaryMilitaryPosition>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Voluntary_MilitaryPosition) + "</VoluntaryMilitaryPosition>";
                response += "<VoluntaryMilRepSpecialityTypeID>" + (reservistMilRepStatus.Voluntary_MilRepSpeciality != null ? reservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportSpecialityTypeId.ToString() : "") + "</VoluntaryMilRepSpecialityTypeID>";
                response += "<VoluntaryMilRepSpecialityID>" + reservistMilRepStatus.Voluntary_MilRepSpecialityId + "</VoluntaryMilRepSpecialityID>";
                response += "<RemovedDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Date) + "</RemovedDate>";
                response += "<RemovedReasonID>" + reservistMilRepStatus.Removed_ReasonId + "</RemovedReasonID>";
                response += "<RemovedDeceasedDeathCert>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Removed_Deceased_DeathCert) + "</RemovedDeceasedDeathCert>";
                response += "<RemovedDeceasedDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Deceased_Date) + "</RemovedDeceasedDate>";
                response += "<RemovedAgeLimitOrder>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Removed_AgeLimit_Order) + "</RemovedAgeLimitOrder>";
                response += "<RemovedAgeLimitDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Removed_AgeLimit_Date) + "</RemovedAgeLimitDate>";
                response += "<RemovedAgeLimitSignedBy>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Removed_AgeLimit_SignedBy) + "</RemovedAgeLimitSignedBy>";
                response += "<RemovedNotSuitableCert>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Removed_NotSuitable_Cert) + "</RemovedNotSuitableCert>";
                response += "<RemovedNotSuitableDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.Removed_NotSuitable_Date) + "</RemovedNotSuitableDate>";
                response += "<RemovedNotSuitableSignedBy>" + AJAXTools.EncodeForXML(reservistMilRepStatus.Removed_NotSuitable_SignedBy) + "</RemovedNotSuitableSignedBy>";
                response += "<MilEmployedAdministrationID>" + reservistMilRepStatus.MilEmployed_AdministrationId + "</MilEmployedAdministrationID>";
                response += "<MilEmployedDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.MilEmployed_Date) + "</MilEmployedDate>";
                response += "<TemporaryRemovedReasonID>" + reservistMilRepStatus.TemporaryRemoved_ReasonId + "</TemporaryRemovedReasonID>";
                response += "<TemporaryRemovedDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.TemporaryRemoved_Date) + "</TemporaryRemovedDate>";
                response += "<TemporaryRemovedDuration>" + reservistMilRepStatus.TemporaryRemoved_Duration + "</TemporaryRemovedDuration>";
                response += "<PostponeTypeID>" + reservistMilRepStatus.Postpone_TypeId + "</PostponeTypeID>";
                response += "<PostponeYear>" + reservistMilRepStatus.Postpone_Year + "</PostponeYear>";
                response += "<DestMilDepartmentID>" + reservistMilRepStatus.DestMilDepartmentId + "</DestMilDepartmentID>";
                response += "<DischargeDate>" + CommonFunctions.FormatDate(reservistMilRepStatus.DischargeDate) + "</DischargeDate>";
                response += "</reservistMilRepStatus>";

                response += "<milrepspec>";

                if (reservistMilRepStatus.Voluntary_MilRepSpeciality != null)
                {
                    List<MilitaryReportSpeciality> milRepSpecs = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, reservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportSpecialityTypeId);

                    foreach (MilitaryReportSpeciality mrs in milRepSpecs)
                    {
                        response += "<mrs>";
                        response += "<id>" + mrs.MilReportSpecialityId + "</id>";
                        response += "<name>" + AJAXTools.EncodeForXML(mrs.CodeAndName) + "</name>";
                        response += "</mrs>";
                    }

                    if ((from m in milRepSpecs 
                         where m.MilReportSpecialityId == reservistMilRepStatus.Voluntary_MilRepSpecialityId 
                         select m).Count() == 0)
                    {
                        MilitaryReportSpeciality mrs = reservistMilRepStatus.Voluntary_MilRepSpeciality;

                        response += "<mrs>";
                        response += "<id>" + mrs.MilReportSpecialityId + "</id>";
                        response += "<name>" + AJAXTools.EncodeForXML(mrs.CodeAndName) + "</name>";
                        response += "</mrs>";
                    }
                }

                response += "</milrepspec>";

                response += "</response>";

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

        // Save Military Report Status by ajax request
        private void JSSaveMilRepStatus()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int reservistID = int.Parse(Request.Form["ReservistId"]);
                int reservistMilRepStatusID = int.Parse(Request.Form["ReservistMilRepStatusID"]);
                string militaryReportStatusKey = Request.Form["MilitaryReportStatusKey"];                

                Reservist reservist = ReservistUtil.GetReservist(reservistID, CurrentUser);                                

                ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(CurrentUser);
                List<VoluntaryReserveAnnex> annexes = new List<VoluntaryReserveAnnex>();
                List<int> annexesToDelete = new List<int>();

                reservistMilRepStatus.ReservistMilRepStatusId = reservistMilRepStatusID;
                reservistMilRepStatus.ReservistId = reservistID;
                reservistMilRepStatus.IsCurrent = true;

                // if there is change in PostponeYear, when the status is POSTPONED, then automatically save new status
                if (militaryReportStatusKey == "POSTPONED")
                {
                    ReservistMilRepStatus oldReservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservistID, CurrentUser);

                    int? newPostponeYear = (!String.IsNullOrEmpty(Request.Form["PostponeYear"]) ? (int?)int.Parse(Request.Form["PostponeYear"]) : null);

                    if (oldReservistMilRepStatus != null &&
                        oldReservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey == "POSTPONED" &&
                        oldReservistMilRepStatus.Postpone_Year != newPostponeYear)
                        reservistMilRepStatus.ReservistMilRepStatusId = 0;
                }

                if (!String.IsNullOrEmpty(Request.Form["MilitaryReportStatusID"]) && Request.Form["MilitaryReportStatusID"] != ListItems.GetOptionChooseOne().Value)
                    reservistMilRepStatus.MilitaryReportStatusId = int.Parse(Request.Form["MilitaryReportStatusID"]);

                reservistMilRepStatus.EnrolDate = (!String.IsNullOrEmpty(Request.Form["EnrolDate"]) ? CommonFunctions.ParseDate(Request.Form["EnrolDate"]) : (DateTime?)null);

                if (!String.IsNullOrEmpty(Request.Form["SourceMilDepartmentID"]) && Request.Form["SourceMilDepartmentID"] != ListItems.GetOptionChooseOne().Value)
                    reservistMilRepStatus.SourceMilDepartmentId = int.Parse(Request.Form["SourceMilDepartmentID"]);

                switch (militaryReportStatusKey)
                {
                    case "VOLUNTARY_RESERVE":
                        reservistMilRepStatus.Voluntary_ContractNumber = Request.Form["VoluntaryContractNumber"];
                        reservistMilRepStatus.Voluntary_ContractDate = (!String.IsNullOrEmpty(Request.Form["VoluntaryContractDate"]) ? CommonFunctions.ParseDate(Request.Form["VoluntaryContractDate"]) : (DateTime?)null);
                        reservistMilRepStatus.Voluntary_ExpireDate = (!String.IsNullOrEmpty(Request.Form["VoluntaryExpireDate"]) ? CommonFunctions.ParseDate(Request.Form["VoluntaryExpireDate"]) : (DateTime?)null);

                        reservistMilRepStatus.Voluntary_DurationMonths = (!String.IsNullOrEmpty(Request.Form["VoluntaryDurationMonths"]) ? (int?)int.Parse(Request.Form["VoluntaryDurationMonths"]) : null);

                        var annexesCount = Int32.Parse(Request.Form["AnnexesCount"]);
                        for (int i = 1; i <= annexesCount; i++)
                        {
                            if (Request.Form["IsDeleted" + i] == "no")
                            {
                                VoluntaryReserveAnnex annex = new VoluntaryReserveAnnex(CurrentUser);

                                if (!String.IsNullOrEmpty(Request.Form["AnnexID" + i]))
                                    annex.VoluntaryReserveAnnexId = int.Parse(Request.Form["AnnexID" + i]);
                                annex.AnnexNumber = Request.Form["AnnexNumber" + i];
                                annex.AnnexDate = (!String.IsNullOrEmpty(Request.Form["AnnexDate" + i]) ? CommonFunctions.ParseDate(Request.Form["AnnexDate" + i]) : (DateTime?)null);
                                annex.AnnexDurationMonths = (!String.IsNullOrEmpty(Request.Form["AnnexDurationMonths" + i]) ? (int?)int.Parse(Request.Form["AnnexDurationMonths" + i]) : null);
                                annex.AnnexExpireDate = (!String.IsNullOrEmpty(Request.Form["AnnexExpireDate" + i]) ? CommonFunctions.ParseDate(Request.Form["AnnexExpireDate" + i]) : (DateTime?)null);
                                annexes.Add(annex);
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Request.Form["AnnexID" + i]))
                                    annexesToDelete.Add(int.Parse(Request.Form["AnnexID" + i]));
                            }
                        }

                        if (int.Parse(Request.Form["VoluntaryFulfilPlaceID"]) != -1)
                            reservistMilRepStatus.Voluntary_FulfilPlaceID = int.Parse(Request.Form["VoluntaryFulfilPlaceID"]);

                        if (!String.IsNullOrEmpty(Request.Form["VoluntaryMilitaryRankID"]) && Request.Form["VoluntaryMilitaryRankID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.Voluntary_MilitaryRankId = Request.Form["VoluntaryMilitaryRankID"];
                        reservistMilRepStatus.Voluntary_MilitaryPosition = Request.Form["VoluntaryMilitaryPosition"];
                        if (!String.IsNullOrEmpty(Request.Form["VoluntaryMilRepSpecialityID"]) && Request.Form["VoluntaryMilRepSpecialityID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.Voluntary_MilRepSpecialityId = int.Parse(Request.Form["VoluntaryMilRepSpecialityID"]);
                        break;
                    case "REMOVED":
                        reservistMilRepStatus.Removed_Date = (!String.IsNullOrEmpty(Request.Form["RemovedDate"]) ? CommonFunctions.ParseDate(Request.Form["RemovedDate"]) : (DateTime?)null);
                        if (!String.IsNullOrEmpty(Request.Form["RemovedReasonID"]) && Request.Form["RemovedReasonID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.Removed_ReasonId = int.Parse(Request.Form["RemovedReasonID"]);
                        reservistMilRepStatus.Removed_Deceased_DeathCert = Request.Form["RemovedDeceasedDeathCert"];
                        reservistMilRepStatus.Removed_Deceased_Date = (!String.IsNullOrEmpty(Request.Form["RemovedDeceasedDate"]) ? CommonFunctions.ParseDate(Request.Form["RemovedDeceasedDate"]) : (DateTime?)null);
                        reservistMilRepStatus.Removed_AgeLimit_Order = Request.Form["RemovedAgeLimitOrder"];
                        reservistMilRepStatus.Removed_AgeLimit_Date = (!String.IsNullOrEmpty(Request.Form["RemovedAgeLimitDate"]) ? CommonFunctions.ParseDate(Request.Form["RemovedAgeLimitDate"]) : (DateTime?)null);
                        reservistMilRepStatus.Removed_AgeLimit_SignedBy = Request.Form["RemovedAgeLimitSignedBy"];
                        reservistMilRepStatus.Removed_NotSuitable_Cert = Request.Form["RemovedNotSuitableCert"];
                        reservistMilRepStatus.Removed_NotSuitable_Date = (!String.IsNullOrEmpty(Request.Form["RemovedNotSuitableDate"]) ? CommonFunctions.ParseDate(Request.Form["RemovedNotSuitableDate"]) : (DateTime?)null);
                        reservistMilRepStatus.Removed_NotSuitable_SignedBy = Request.Form["RemovedNotSuitableSignedBy"];
                        break;
                    case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                        if (!String.IsNullOrEmpty(Request.Form["MilEmployedAdministrationID"]) && Request.Form["MilEmployedAdministrationID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.MilEmployed_AdministrationId = int.Parse(Request.Form["MilEmployedAdministrationID"]);
                        reservistMilRepStatus.MilEmployed_Date = (!String.IsNullOrEmpty(Request.Form["MilEmployedDate"]) ? CommonFunctions.ParseDate(Request.Form["MilEmployedDate"]) : (DateTime?)null);
                        break;
                    case "TEMPORARY_REMOVED":
                        if (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedReasonID"]) && Request.Form["TemporaryRemovedReasonID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.TemporaryRemoved_ReasonId = int.Parse(Request.Form["TemporaryRemovedReasonID"]);
                        reservistMilRepStatus.TemporaryRemoved_Date = (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedDate"]) ? CommonFunctions.ParseDate(Request.Form["TemporaryRemovedDate"]) : (DateTime?)null);
                        if (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedDuration"]) && Request.Form["TemporaryRemovedDuration"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.TemporaryRemoved_Duration = int.Parse(Request.Form["TemporaryRemovedDuration"]);
                        break;
                    case "POSTPONED":
                        if (!String.IsNullOrEmpty(Request.Form["PostponeTypeID"]) && Request.Form["PostponeTypeID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.Postpone_TypeId = int.Parse(Request.Form["PostponeTypeID"]);
                        reservistMilRepStatus.Postpone_Year = (!String.IsNullOrEmpty(Request.Form["PostponeYear"]) ? (int?)int.Parse(Request.Form["PostponeYear"]) : null);
                        break;
                    case "DISCHARGED":
                        if (!String.IsNullOrEmpty(Request.Form["DestMilDepartmentID"]) && Request.Form["DestMilDepartmentID"] != ListItems.GetOptionChooseOne().Value)
                            reservistMilRepStatus.DestMilDepartmentId = int.Parse(Request.Form["DestMilDepartmentID"]);
                        reservistMilRepStatus.DischargeDate = (!String.IsNullOrEmpty(Request.Form["DischargeDate"]) ? CommonFunctions.ParseDate(Request.Form["DischargeDate"]) : (DateTime?)null);
                        break;
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Reservists");

                bool modifiedMilitaryDepartment = false;

                // check for changes in SourceMilitaryDepartment and clear reservist Punkt field
                if (reservistMilRepStatusID != 0)
                {
                    ReservistMilRepStatus oldReservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepStatus(reservistMilRepStatusID, CurrentUser);

                    if (oldReservistMilRepStatus.SourceMilDepartmentId != reservistMilRepStatus.SourceMilDepartmentId)
                    {
                        modifiedMilitaryDepartment = true;

                        reservist.PunktID = null;

                        string logDescription = "";
                        logDescription += "Име: " + reservist.Person.FullName;
                        logDescription += "<br />ЕГН: " + reservist.Person.IdentNumber;

                        ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EditMilRep", logDescription, null, reservist.Person, CurrentUser);

                        ReservistUtil.SaveReservist_WhenEditingMilitaryReportTab(reservist, CurrentUser, changeEvent);
                    }
                }

                //When changing the current Military Report Status of particular reservist then
                //remove that reservist from any Equipment Requests and also clear the current Mobilization Appointment, if any
                if (modifiedMilitaryDepartment ||
                    reservistMilRepStatusID == 0)
                {
                    List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservistID, CurrentUser);

                    //Remove the Reservist from each Equipment Request
                    foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                    {
                        FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);
                    }

                    //Clear the Mobilization Appointment
                    ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(reservistID, CurrentUser, change);
                }

                //Save the Person changes
                ReservistMilRepStatusUtil.SaveReservistMilRepStatus(reservistMilRepStatus, CurrentUser, change);

                //Save annexes
                foreach (var annex in annexes)
                {
                    annex.ReservistMilRepStatusId = reservistMilRepStatus.ReservistMilRepStatusId;
                    VoluntaryReserveAnnexUtil.SaveVoluntaryReserveAnnex(annex, CurrentUser, change);
                }

                //Delete annexes
                foreach (var annex in annexesToDelete)
                {
                    VoluntaryReserveAnnexUtil.DeleteVoluntaryReserveAnnex(annex, CurrentUser, change);
                }

                //When the reservist is DISCHARGED then automatically add a new status
                //to get him moved to the new department
                if (militaryReportStatusKey == "DISCHARGED")
                {
                    ReservistMilRepStatus autoMoveToNewMilDeptStatus = new ReservistMilRepStatus(CurrentUser);

                    autoMoveToNewMilDeptStatus.ReservistId = reservistMilRepStatus.ReservistId;
                    autoMoveToNewMilDeptStatus.IsCurrent = true;
                    autoMoveToNewMilDeptStatus.MilitaryReportStatusId = reservistMilRepStatus.MilitaryReportStatusId;
                    autoMoveToNewMilDeptStatus.EnrolDate = reservistMilRepStatus.DischargeDate;
                    autoMoveToNewMilDeptStatus.SourceMilDepartmentId = reservistMilRepStatus.DestMilDepartmentId;

                    ReservistMilRepStatusUtil.SaveReservistMilRepStatus(autoMoveToNewMilDeptStatus, CurrentUser, change);
                }

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<result>
                                <response>OK</response>
                                <ReservistMilRepStatusId>" + reservistMilRepStatus.ReservistMilRepStatusId + @"</ReservistMilRepStatusId>
                                <GroupManagementSection>" + AJAXTools.EncodeForXML(AddEditReservist_MilitaryReport_PageUtil.GetGroupManagementSection(reservist, CurrentUser)) + @"</GroupManagementSection>
                                <ReservistAppointmentSection>" + AJAXTools.EncodeForXML(AddEditReservist_MilitaryReport_PageUtil.GetReservistAppointmentSection(reservist, CurrentUser, this)) + @"</ReservistAppointmentSection>
                            </result>";
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

        private void JSLoadReservistAppointmentHistory()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_HISTORY") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                
                response = "<response>" + AJAXTools.EncodeForXML(AddEditReservist_MilitaryReport_PageUtil.GetReservistAppointmentHistoryLightBox(CurrentUser, Request, this)) + "</response>";

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

        private void JSLoadMilRepStatusHistory()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_HISTORY") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<response>" + AJAXTools.EncodeForXML(AddEditReservist_MilitaryReport_PageUtil.GetMilRepStatusHistoryLightBox(CurrentUser, Request)) + "</response>";

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

        //Save Military Report Tab (ajax call)
        private void JSSaveMilitaryReportTab()
        {
            string stat = "";
            string response = "";

            try
            {                               
                int reservistId = int.Parse(Request.Form["ReservistId"]);
                int? administrationId = null;
                if (!String.IsNullOrEmpty(Request.Form["AdministrationId"]) && Request.Form["AdministrationId"] != ListItems.GetOptionChooseOne().Value)
                            administrationId = int.Parse(Request.Form["AdministrationId"]);                
                int? clInformationAccLevelBgId = null;
                if (!String.IsNullOrEmpty(Request.Form["ClInformationAccLevelBgId"]) && Request.Form["ClInformationAccLevelBgId"] != ListItems.GetOptionChooseOne().Value)
                            clInformationAccLevelBgId = int.Parse(Request.Form["ClInformationAccLevelBgId"]);
                DateTime? clInformationAccLevelBgExpDate = (!String.IsNullOrEmpty(Request.Form["ClInformationAccLevelBgExpDate"]) ? CommonFunctions.ParseDate(Request.Form["ClInformationAccLevelBgExpDate"]) : (DateTime?)null);
                string groupManagementSection = Request.Form["GroupManagementSection"];
                string section = Request.Form["Section"];
                string deliverer = Request.Form["Deliverer"];
                int? punktId = null;
                if (!String.IsNullOrEmpty(Request.Form["PunktId"]) && Request.Form["PunktId"] != ListItems.GetOptionChooseOne().Value)
                    punktId = int.Parse(Request.Form["PunktId"]);
                int? needCourse = !string.IsNullOrEmpty(Request.Form["NeedCourse"]) ? (int?)int.Parse(Request.Form["NeedCourse"]) : null;
                int? appointmentIsDelivered = !string.IsNullOrEmpty(Request.Form["AppointmentIsDelivered"]) ? (int?)int.Parse(Request.Form["AppointmentIsDelivered"]) : null;
                int? isSuitableForMobAppointment = !string.IsNullOrEmpty(Request.Form["IsSuitableForMobAppointment"]) ? int.Parse(Request.Form["IsSuitableForMobAppointment"]) : 0;

                Change change = new Change(CurrentUser, "RES_Reservists");

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);
                Person person = reservist.Person;

                person.AdministrationID = administrationId;
                person.ClInformationAccLevelBgID = clInformationAccLevelBgId;
                person.ClInformationAccLevelBgExpDate = clInformationAccLevelBgExpDate;

                string logDescription = "";
                logDescription += "Име: " + person.FullName;
                logDescription += "<br />ЕГН: " + person.IdentNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EditMilRep", logDescription, null, person, CurrentUser);

                PersonUtil.SavePerson_WhenEditingMilitaryReportTab(person, CurrentUser, changeEvent);

                reservist.GroupManagementSection = groupManagementSection;
                reservist.Section = section;
                reservist.Deliverer = deliverer;
                reservist.PunktID = punktId;

                ReservistUtil.SaveReservist_WhenEditingMilitaryReportTab(reservist, CurrentUser, changeEvent);
                if (needCourse.HasValue)
                    ReservistUtil.SaveReservist_SetNeedCourse(reservist.ReservistId, needCourse.Value, CurrentUser, changeEvent);
                if (appointmentIsDelivered.HasValue)
                    ReservistUtil.SaveReservist_SetAppointmentIsDelivered(reservist.ReservistId, appointmentIsDelivered.Value, CurrentUser, changeEvent);
                if (isSuitableForMobAppointment.HasValue)
                    ReservistUtil.SaveReservist_SetIsSuitableForMobAppointment(person.PersonId, isSuitableForMobAppointment.Value, CurrentUser, changeEvent);

                if (changeEvent.ChangeEventDetails.Count > 0)
                    change.AddEvent(changeEvent);                

                change.WriteLog();                

                stat = AJAXTools.OK;
                response = "<status>OK</status>";
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

        private void JSPrintMK()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);
            string result = AddEditReservist_MilitaryReport_PageUtil.PrintMK(reservistId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=MK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintPZ()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);
            string result = AddEditReservist_MilitaryReport_PageUtil.PrintPZ(reservistId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=PZ.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintАK()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);
            string result = AddEditReservist_MilitaryReport_PageUtil.PrintАK(reservistId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=AK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintАSK()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);
            string result = AddEditReservist_MilitaryReport_PageUtil.PrintАSK(reservistId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=ASK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintUO()
        {
            int reservistId = int.Parse(Request.Params["ReservistId"]);
            string result = AddEditReservist_MilitaryReport_PageUtil.PrintUO(reservistId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=UO.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSLoadReservistMilitaryReportStatusSection()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int reservistId = int.Parse(Request.Params["ReservistId"]);
                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                response = "<response>" + AJAXTools.EncodeForXML(AddEditReservist_MilitaryReport_PageUtil.GetReservistMilitaryReportStatusSection(reservist, CurrentUser, this)) + "</response>";

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

        private void JSTransferToVitosha(int pReservistID, int pMilitaryUnitID)
        {
          
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                Reservist reservist = ReservistUtil.GetReservist(pReservistID, CurrentUser);
                if (reservist != null)
                {
                    PersonUtil.TransferToVitosha(reservist.PersonId, pMilitaryUnitID, CurrentUser, change);
                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<status>OK</status><msg>Прехвърлянето беше успешно.</msg>";
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

        private void JSLoadMedCert()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int medCertID = int.Parse(Request.Form["MedCertID"]);

                PersonMedCert personMedCert = PersonMedCertUtil.GetPersonMedCert(medCertID, CurrentUser);

                response = "<response>";

                response += "<medCertDate>" + CommonFunctions.FormatDate(personMedCert.MedCertDate) + "</medCertDate>";
                response += "<protNum>" + AJAXTools.EncodeForXML(personMedCert.ProtNum) + "</protNum>";
                response += "<conclusionID>" + AJAXTools.EncodeForXML(personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionId.ToString() : ListItems.GetOptionChooseOne().Value) + "</conclusionID>";
                response += "<medRubricID>" + AJAXTools.EncodeForXML(personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricID.ToString() : ListItems.GetOptionChooseOne().Value) + "</medRubricID>";
                response += "<medCertExpirationDate>" + CommonFunctions.FormatDate(personMedCert.ExpirationDate) + "</medCertExpirationDate>";

                response += "</response>";

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

        //Save a particular Med Certs (ajax call)
        private void JSSaveMedCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int reservistId = int.Parse(Request.Form["ReservistId"]);
                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMedCert personMedCert = new PersonMedCert(CurrentUser);

                personMedCert.MedCertID = int.Parse(Request.Form["MedCertID"]);;
                personMedCert.PersonID = reservist.PersonId;
                personMedCert.MedCertDate = (!String.IsNullOrEmpty(Request.Form["MedCertDate"]) ? CommonFunctions.ParseDate(Request.Form["MedCertDate"]) : (DateTime?)null);
                personMedCert.ProtNum = Request.Form["MedCertProtNum"];
                personMedCert.ConclusionID = (!String.IsNullOrEmpty(Request.Form["MedCertConclusionId"]) && Request.Form["MedCertConclusionId"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["MedCertConclusionId"]) : (int?)null);
                personMedCert.MedRubricID = (!String.IsNullOrEmpty(Request.Form["MedCertMedRubricID"]) && Request.Form["MedCertMedRubricID"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["MedCertMedRubricID"]) : (int?)null);
                personMedCert.ExpirationDate = (!String.IsNullOrEmpty(Request.Form["MedCertExpirationDate"]) ? CommonFunctions.ParseDate(Request.Form["MedCertExpirationDate"]) : (DateTime?)null);

                PersonMedCertUtil.SavePersonMedCert(personMedCert, CurrentUser, change);

                change.WriteLog();

                string refreshedMedCertTable = AddEditReservist_MilitaryReport_PageUtil.GetMedCertTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMedCertTable>" + AJAXTools.EncodeForXML(refreshedMedCertTable) + @"</refreshedMedCertTable>";
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

        //Delete a particular Med Cert Title (ajax call)
        private void JSDeleteMedCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int medCertID = int.Parse(Request.Params["MedCertID"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonMedCertUtil.DeletePersonMedCert(medCertID, CurrentUser, change);

                change.WriteLog();

                string refreshedMedCertTable = AddEditReservist_MilitaryReport_PageUtil.GetMedCertTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMedCertTable>" + AJAXTools.EncodeForXML(refreshedMedCertTable) + @"</refreshedMedCertTable>";
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

        private void JSLoadPsychCert()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int psychCertID = int.Parse(Request.Form["PsychCertID"]);

                PersonPsychCert personPsychCert = PersonPsychCertUtil.GetPersonPsychCert(psychCertID, CurrentUser);

                response = "<response>";

                response += "<psychCertDate>" + CommonFunctions.FormatDate(personPsychCert.PsychCertDate) + "</psychCertDate>";
                response += "<protNum>" + AJAXTools.EncodeForXML(personPsychCert.ProtNum) + "</protNum>";
                response += "<conclusionID>" + AJAXTools.EncodeForXML(personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionId.ToString() : ListItems.GetOptionChooseOne().Value) + "</conclusionID>";
                response += "<psychCertExpirationDate>" + CommonFunctions.FormatDate(personPsychCert.ExpirationDate) + "</psychCertExpirationDate>";

                response += "</response>";

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

        //Save a particular Psych Certs (ajax call)
        private void JSSavePsychCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int reservistId = int.Parse(Request.Form["ReservistId"]);
                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPsychCert personPsychCert = new PersonPsychCert(CurrentUser);

                personPsychCert.PsychCertID = int.Parse(Request.Form["PsychCertID"]); ;
                personPsychCert.PersonID = reservist.PersonId;
                personPsychCert.PsychCertDate = (!String.IsNullOrEmpty(Request.Form["PsychCertDate"]) ? CommonFunctions.ParseDate(Request.Form["PsychCertDate"]) : (DateTime?)null);
                personPsychCert.ProtNum = Request.Form["PsychCertProtNum"];
                personPsychCert.ConclusionID = (!String.IsNullOrEmpty(Request.Form["PsychCertConclusionId"]) && Request.Form["PsychCertConclusionId"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["PsychCertConclusionId"]) : (int?)null);
                personPsychCert.ExpirationDate = (!String.IsNullOrEmpty(Request.Form["PsychCertExpirationDate"]) ? CommonFunctions.ParseDate(Request.Form["PsychCertExpirationDate"]) : (DateTime?)null);

                PersonPsychCertUtil.SavePersonPsychCert(personPsychCert, CurrentUser, change);

                change.WriteLog();

                string refreshedPsychCertTable = AddEditReservist_MilitaryReport_PageUtil.GetPsychCertTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPsychCertTable>" + AJAXTools.EncodeForXML(refreshedPsychCertTable) + @"</refreshedPsychCertTable>";
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

        //Delete a particular Psych Cert Title (ajax call)
        private void JSDeletePsychCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int psychCertID = int.Parse(Request.Params["PsychCertID"]);
                int reservistId = int.Parse(Request.Params["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPsychCertUtil.DeletePersonPsychCert(psychCertID, CurrentUser, change);

                change.WriteLog();

                string refreshedPsychCertTable = AddEditReservist_MilitaryReport_PageUtil.GetPsychCertTable(reservist, CurrentUser, this);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPsychCertTable>" + AJAXTools.EncodeForXML(refreshedPsychCertTable) + @"</refreshedPsychCertTable>";
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

    public static class AddEditReservist_MilitaryReport_PageUtil
    {
        public static string GetTabContent(int reservistId, string moduleKey, User currentUser, AddEditReservist page)
        {            
            Reservist reservist = ReservistUtil.GetReservist(reservistId, currentUser);

            string personAdmClAccessAndMilRepSpecSection = GetPersonAdmClAccessAndMilRepSpecSection(reservist, currentUser, page);
            string personAdmClAccessAndMilRepSpecLightBox = GetMilRepSpecLightBox(currentUser);
            string personPositionTitleLightBox = GetPersonPositionTitleLightBox(currentUser);
            string reservistMilitaryReportStatusSection = GetReservistMilitaryReportStatusSection(reservist, currentUser, page);
            string addEditMilRepStatusLightBox = GetAddEditMilRepStatusLightBox(reservist, currentUser, moduleKey, page);
            string milRepStatusHistoryLightBox = @"<div id=""divMilRepStatusHistoryLightBox"" style=""display: none;"" class=""lboxMilRepStatusHistory""></div>";
            string reservistAppointmentSection = GetReservistAppointmentSection(reservist, currentUser, page);
            string reservistAppointmentHistoryLightBox = @"<div id=""divReservistAppointmentHistoryLightBox"" style=""display: none;"" class=""lboxReservistAppointmentHistory""></div>";
            string groupManagementSection = @"<div id=""divGroupManagementSection"" >" + GetGroupManagementSection(reservist, currentUser) + "</div>";
            string medCertSection = GetMedCertSection(reservist, currentUser, page);
            string medCertLightBox = GetMedCertLightBox(currentUser);
            string psychCertSection = GetPsychCertSection(reservist, currentUser, page);
            string psychCertLightBox = GetPsychCertLightBox(currentUser);

            string html = @"
<div style=""height: 10px;""></div>
" + personAdmClAccessAndMilRepSpecSection + @"
" + personAdmClAccessAndMilRepSpecLightBox + @"
" + personPositionTitleLightBox + @"
<div style=""height: 10px;""></div>
<div id=""divReservistMilitaryReportStatusSection"">" + reservistMilitaryReportStatusSection + @"</div>
" + addEditMilRepStatusLightBox + @"
" + milRepStatusHistoryLightBox + @"
<div style=""height: 10px;""></div>
<div id=""divReservistAppointmentSection"">" + reservistAppointmentSection + @"</div>
" + reservistAppointmentHistoryLightBox + @"
<div style=""height: 10px;""></div>
" + groupManagementSection + @"
<div style=""height: 10px;""></div>
" + medCertSection + @"
" + medCertLightBox + @"
<div style=""height: 10px;""></div>
" + psychCertSection + @"
" + psychCertLightBox + @"
<div style=""height: 10px;""></div>
";

            return html;
        }

        public static string GetPersonAdmClAccessAndMilRepSpecSection(Reservist reservist, User currentUser, AddEditReservist page)
        {            
            Person person = reservist.Person;
            int isSuitableForMobAppointmentValue = 0;

            string ddAdministrationHtml = "";

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(currentUser);

            List<IDropDownItem> administrationsDropDownItems = new List<IDropDownItem>();
            foreach (Administration administration in administrations)
                administrationsDropDownItems.Add(administration as IDropDownItem);

            ddAdministrationHtml = ListItems.GetDropDownHtml(administrationsDropDownItems, null, "ddAdmClAccessAndMilRepSpecSectionAdministration", true, person.Administration, null, @"style=""width: 220px;""");

            string ddClInformationAccLevelBgHtml = "";

            List<ClInformation> clInformations = ClInformationUtil.GetAllClInformationBG(currentUser);

            List<IDropDownItem> clInformationsDropDownItems = new List<IDropDownItem>();
            foreach (ClInformation clInformation in clInformations)
                clInformationsDropDownItems.Add(clInformation as IDropDownItem);

            ddClInformationAccLevelBgHtml = ListItems.GetDropDownHtml(clInformationsDropDownItems, null, "ddAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg", true, ClInformationUtil.GetClInformationBG(person.ClInformationAccLevelBgID.ToString(), currentUser), null, @"style=""width: 160px;""");

            string clInformationAccLevelBgExpDate = person != null ? CommonFunctions.FormatDate(person.ClInformationAccLevelBgExpDate) : "";
            switch(person.IsSuitableForMobAppointment)
            {
                case true:
                    isSuitableForMobAppointmentValue = 1;
                    break;
                case false:
                    isSuitableForMobAppointmentValue = 0;
                    break;
            }

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">   
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td>
            <table>
                <tr>
                    <td style=""vertical-align: top; width: 380px;"">
                        <table>
                            <tr>
                                 <td style=""text-align: right; width: 150px;"">
                                    <span id=""lblAdmClAccessAndMilRepSpecSectionAdministration"" class=""InputLabel"">Работил/служил в:</span>
                                </td>
                                <td style=""text-align: left; width: 250px;""> "
                                    + ddAdministrationHtml +
                                @"</td>
                            </tr>
                            <tr>
                                 <td style=""text-align: right;"">
                                    <span id=""lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg"" class=""InputLabel"">Вид допуск:</span>
                                </td>
                                <td style=""text-align: left;""> "
                                    + ddClInformationAccLevelBgHtml +
                                @"</td>
                            </tr>
                            <tr>
                                 <td style=""text-align: right;"">
                                    <span id=""lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate"" class=""InputLabel"">Валиден до:</span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""spanAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate"">
                                        <input type=""text"" id=""txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" value='" + clInformationAccLevelBgExpDate + @"'/>
                                    </span>
                                </td>
                            </tr>

                            <tr>
                                <td style=""text-align: right;"">
                                    <span id=""spanAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate"">
                                        <input type=""checkbox"" id=""suitableForMobAppointmentCheckBox"" " + (isSuitableForMobAppointmentValue == 1 ? @"checked=""checked""" : "") + @"/>
                                    </span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""suitableForMobAppointmentLabel"" class=""InputLabel"">Подходящ за МН</span>
                                </td>
                            </tr>
                           
                            <tr>
                                <td colspan=""2"" style=""text-align:center;"">                                   
                                    <div id=""btnTransferToVitosha"" style=""display: inline-block;margin-top:10px;"" onclick=""btnTransferToVitosha_Click();"" class=""Button"" olddisabled="""">
                                        <i></i><div id=""btnTransferToVitoshaText"" style=""width: 170px;"">Прехвърляне към Витоша</div><b></b>
                                    </div>                                    
                                </td>
                            </tr>
                            " + (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PRINT_AK") != UIAccessLevel.Hidden ?
                           @"
                             <tr>
                                <td colspan=""2"" style=""text-align:center;"">                                   
                                    <div id=""btnPrintAK"" style=""display: inline-block;margin-top:-5px;"" onclick=""PrintAK();"" class=""Button"" olddisabled="""">
                                        <i></i><div id=""btnPrintAK"" style=""width: 170px;"">Печат на азбучна карта</div><b></b>
                                    </div>                                    
                                </td>
                            </tr>
                            " : "") 
                             + (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PRINT_ASK") != UIAccessLevel.Hidden ?
                           @"
                             <tr>
                                <td colspan=""2"" style=""text-align:center;"">                                   
                                    <div id=""btnPrintASK"" style=""display: inline-block;margin-top:-5px;"" onclick=""PrintASK();"" class=""Button"" olddisabled="""" title=""Печат на азбучна служебна карта"">
                                        <i></i><div id=""btnPrintASK"" style=""width: 170px;"">Печат на АСК</div><b></b>
                                    </div>                                    
                                </td>
                            </tr>
                            " : "") + @"
                        </table>
                    </td>
                    <td style=""text-align: center; width: 450px; vertical-align: top;"">
                        <center>
                        <div id=""divMilRepSpecTable"" style=""margin-top: 15px;"">
                            " + GetMilRepSpecTable(reservist, currentUser, page) +
                      @"</div>
                        <div id=""divPositionTitleTable"" style=""margin-top: 35px;"">
                            " + GetPositionTitlesTable(reservist, currentUser, page) +
                      @"</div>
                        </center>
                    </td>
                </tr>
                <tr>        
                    <td colspan=""2"" style=""text-align: center;"">
                        <span id=""spanPersonAdmClAccessAndMilRepSpecSectionMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
            </table>
         </td>
      </tr>
   </table>
    <div id=""lboxTransferToVitosha"" style=""display: none;"" class=""lboxTransferToVitosha"">" + GetTransferToVitoshaLightBox(reservist, page) + @"</div>
</fieldset>
";

            return html;
        }

        //Render the Military Report Specialities table
        public static string GetMilRepSpecTable(Reservist reservist, User currentUser, RESPage page)
        {
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Hidden)
                return "";

            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewMilRepSpec();' />";
            }

            html.Append(@"<table class='CommonHeaderTable' style='text-align: center;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
                                   <th style='width: 380px; vertical-align: bottom;'>ВОС</th>
                                   <th style='width: 50px; vertical-align: bottom;' title='Основна ВОС'>Осн.</th>
                                   <th style='width: 40px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(reservist.PersonId, currentUser);

            foreach (PersonMilitaryReportSpeciality personMilitaryReportSpeciality in personMilitaryReportSpecialities)
            {
                counter++;

                string deleteHTML = "";

                if (personMilitaryReportSpeciality.CanDelete)
                {
                    if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Enabled && !isPreview &&
                        !(personMilitaryReportSpeciality.IsPrimary && personMilitaryReportSpecialities.Count > 1)
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteMilRepSpec(" + personMilitaryReportSpeciality.PersonMilRepSpecID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSPEC") == UIAccessLevel.Enabled && !isPreview
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditMilRepSpec(" + personMilitaryReportSpeciality.PersonMilRepSpecID.ToString() + ");' />";

                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
                                    <td style='text-align: left;'>" + personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName + @"</td>
                                    <td style='text-align: center;'><input type='checkbox' UnsavedCheckSkipMe='true' disabled " + (personMilitaryReportSpeciality.IsPrimary ? "checked" : "") + @" /></td>
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            html.Append(@"<input type=""hidden"" id=""hdnPersonMilitaryReportSpecialitiesCount"" value=""" + counter.ToString() + @""" />");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Military Report Specialities light-box
        public static string GetMilRepSpecLightBox(User currentUser)
        {
            List<MilitaryReportSpecialityType> listMilRepSpecTypes = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(currentUser);
            List<IDropDownItem> ddiMilRepSpecType = new List<IDropDownItem>();

            foreach (MilitaryReportSpecialityType militaryReportSpecialityType in listMilRepSpecTypes)
            {
                ddiMilRepSpecType.Add(militaryReportSpecialityType);
            }

            // Generates html for drop down list
            string ddMilRepSpecTypeHTML = ListItems.GetDropDownHtml(ddiMilRepSpecType, null, "ddMilRepSpecTypeLightBox", true, null, "MilRepSpecTypeLightBoxChanged();", "style='width: 180px;' UnsavedCheckSkipMe='true' ", true);

            string html = @"
<div id=""divMilRepSpecLightBox"" style=""display: none;"" class=""lboxMilRepSpec"">
<center>
    <input type=""hidden"" id=""hdnPersonMilRepSpecID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 20%"">
        </colgroup>
        <colgroup style=""width: 80%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilRepSpecTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilRepSpecTypeLightBox"" class=""InputLabel"">Тип ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddMilRepSpecTypeHTML + @"
            </td>
        </tr>
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilRepSpecLightBox"" class=""InputLabel"">ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                <select id=""ddMilRepSpecLightBox"" style=""width: 100%"" UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" ></select>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <input type=""checkbox"" UnsavedCheckSkipMe=""true"" id=""chkMilRepSpecIsPrimaryLightBox"" />
                <input type=""hidden"" UnsavedCheckSkipMe=""true"" id=""hdnMilRepSpecIsPrimaryOldLightBox"" />
            </td>
            <td style=""text-align: left;"">
                <label for=""chkMilRepSpecIsPrimaryLightBox""><span id=""lblMilRepSpecIsPrimaryLightBox"" class=""InputLabel"">Основна ВОС</span></label>
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditMilRepSpecLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilRepSpecLightBox"" style=""display: inline;"" onclick=""SaveAddEditMilRepSpecLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilRepSpecLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMilRepSpecLightBox"" style=""display: inline;"" onclick=""HideAddEditMilRepSpecLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditMilRepSpecLightBoxText"" style=""width: 70px;"">
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
</div>";

            return html;
        }

        //Render the Position Titles table
        public static string GetPositionTitlesTable(Reservist reservist, User currentUser, RESPage page)
        {
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_POSITIONTITLES") == UIAccessLevel.Hidden)
                return "";

            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
            page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_POSITIONTITLES") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewPositionTitle();' />";
            }

            html.Append(@"<table class='CommonHeaderTable' style='text-align: center;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
                                   <th style='width: 380px; vertical-align: bottom;'>Длъжност</th>
                                   <th style='width: 50px; vertical-align: bottom;' title='Основна длъжност'>Осн.</th>
                                   <th style='width: 40px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonPositionTitle> personPositionTitles = PersonPositionTitleUtil.GetAllPersonPositionTitles(reservist.PersonId, currentUser);

            foreach (PersonPositionTitle personPositionTitle in personPositionTitles)
            {
                counter++;

                string deleteHTML = "";

                if (personPositionTitle.CanDelete)
                {
                    if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_POSITIONTITLES") == UIAccessLevel.Enabled && !isPreview &&
                        !(personPositionTitle.IsPrimary && personPositionTitles.Count > 1)
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeletePositionTitle(" + personPositionTitle.PersonPositionTitleID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_POSITIONTITLES") == UIAccessLevel.Enabled && !isPreview
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditPositionTitle(" + personPositionTitle.PersonPositionTitleID.ToString() + ");' />";

                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
                                    <td style='text-align: left;'>" + personPositionTitle.PositionTitle.PositionTitleName + @"</td>
                                    <td style='text-align: center;'><input type='checkbox' UnsavedCheckSkipMe='true' disabled " + (personPositionTitle.IsPrimary ? "checked" : "") + @" /></td>
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            html.Append(@"<input type=""hidden"" id=""hdnPersonPositionTitlesCount"" value=""" + counter.ToString() + @""" />");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Person Position Title light-box
        public static string GetPersonPositionTitleLightBox(User currentUser)
        {
            List<PositionTitle> listPositionTitles = PositionTitleUtil.GetAllPositionTitles(currentUser);
            List<IDropDownItem> ddiPositionTitle = new List<IDropDownItem>();

            foreach (PositionTitle positionTitle in listPositionTitles)
            {
                ddiPositionTitle.Add(positionTitle);
            }

            // Generates html for drop down list
            string ddPositionTitleHTML = ListItems.GetDropDownHtml(ddiPositionTitle, null, "ddPositionTitleLightBox", true, null, "", "style='width: auto;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);

            string html = @"
<div id=""divPositionTitleLightBox"" style=""display: none;"" class=""lboxPositionTitle"">
<center>
    <input type=""hidden"" id=""hdnPersonPositionTitleID"" />
    <input type=""hidden"" id=""hdnExtraAddedPositionTitleID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 20%"">
        </colgroup>
        <colgroup style=""width: 80%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditPositionTitleTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPositionTitleLightBox"" class=""InputLabel"">Длъжност:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddPositionTitleHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <input type=""checkbox"" UnsavedCheckSkipMe=""true"" id=""chkPositionTitleIsPrimaryLightBox"" />
                <input type=""hidden"" UnsavedCheckSkipMe=""true"" id=""hdnPositionTitleIsPrimaryOldLightBox"" />
            </td>
            <td style=""text-align: left;"">
                <label for=""chkPositionTitleIsPrimaryLightBox""><span id=""lblPositionTitleIsPrimaryLightBox"" class=""InputLabel"">Основна длъжност</span></label>
            </td>
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditPositionTitleLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditPositionTitleLightBox"" style=""display: inline;"" onclick=""SaveAddEditPositionTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditPositionTitleLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEdiPositionTitleLightBox"" style=""display: inline;"" onclick=""HideAddEditPositionTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditPositionTitleLightBoxText"" style=""width: 70px;"">
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
</div>";

            return html;
        }


        public static string GetReservistMilitaryReportStatusSection(Reservist reservist, User currentUser, RESPage page)
        {
            ReservistMilRepStatus currentMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservist.ReservistId, currentUser);
            ReservistMilRepStatus firstMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepFirstStatusByReservistId(reservist.ReservistId, currentUser);
            List<VoluntaryReserveAnnex> voluntaryReserveAnnexes = currentMilRepStatus != null ? VoluntaryReserveAnnexUtil.GetVoluntaryReserveAnnexesByReservistMilRepStatusId(currentMilRepStatus.ReservistMilRepStatusId, currentUser): new List<VoluntaryReserveAnnex>();

            string voluntaryDisplay = "none";
            string removedDisplay = "none";
            string milEmplDisplay = "none";
            string temporaryRemovedDisplay = "none";
            string postponedDisplay = "none";
            string dischargedDisplay = "none";

            string militaryReportStatusKey = currentMilRepStatus != null ? currentMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey : "";
            switch (militaryReportStatusKey)
            {
                case "VOLUNTARY_RESERVE":
                    voluntaryDisplay = "";
                    break;
                case "REMOVED":
                    removedDisplay = "";
                    break;
                case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                    milEmplDisplay = "";
                    break;
                case "TEMPORARY_REMOVED":
                    temporaryRemovedDisplay = "";
                    break;
                case "POSTPONED":
                    postponedDisplay = "";
                    break;
                case "DISCHARGED":
                    dischargedDisplay = "";
                    break;
            }

            string firstEnrolDate = (firstMilRepStatus != null ? CommonFunctions.FormatDate(firstMilRepStatus.EnrolDate) : "");
            string currEnrolDate = (currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.EnrolDate) : "");
            string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName : MilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
            string currSourceMilDepartmentName = (currentMilRepStatus != null && currentMilRepStatus.SourceMilDepartment != null ? currentMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "");

            string voluntaryContractNumber = currentMilRepStatus != null ? currentMilRepStatus.Voluntary_ContractNumber : "";
            string voluntaryContractDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Voluntary_ContractDate) : "";
            string voluntaryExpireDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Voluntary_ExpireDate) : "";
            string voluntaryDurationMonths = currentMilRepStatus != null ? currentMilRepStatus.Voluntary_DurationMonths.ToString() : "";
            string voluntaryFulfilPlace = (currentMilRepStatus != null && currentMilRepStatus.Voluntary_FulfilPlace != null ? currentMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : "");
            string voluntaryMilitaryRank = currentMilRepStatus != null && currentMilRepStatus.Voluntary_MilitaryRank != null ? currentMilRepStatus.Voluntary_MilitaryRank.LongName : "";
            string voluntaryMilitaryPosition = currentMilRepStatus != null ? currentMilRepStatus.Voluntary_MilitaryPosition : "";
            string voluntaryMilRepSpecType = currentMilRepStatus != null && currentMilRepStatus.Voluntary_MilRepSpeciality != null ? currentMilRepStatus.Voluntary_MilRepSpeciality.MilReportSpecialityType.TypeName : "";
            string voluntaryMilRepSpec = currentMilRepStatus != null && currentMilRepStatus.Voluntary_MilRepSpeciality != null ? currentMilRepStatus.Voluntary_MilRepSpeciality.CodeAndName: ""; 
            string removedDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Removed_Date) : "";
            string removedReason = currentMilRepStatus != null && currentMilRepStatus.Removed_Reason != null ? currentMilRepStatus.Removed_Reason.Text() : ""; ;
            string removedDeceasedDeathCert = currentMilRepStatus != null ? currentMilRepStatus.Removed_Deceased_DeathCert : "";
            string removedDeceasedDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Removed_Deceased_Date) : "";
            string removedAgeLimitOrder = currentMilRepStatus != null ? currentMilRepStatus.Removed_AgeLimit_Order : "";
            string removedAgeLimitDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Removed_AgeLimit_Date) : "";
            string removedAgeLimitSignedBy = currentMilRepStatus != null ? currentMilRepStatus.Removed_AgeLimit_SignedBy : "";
            string removedNotSuitableCert = currentMilRepStatus != null ? currentMilRepStatus.Removed_NotSuitable_Cert : "";
            string removedNotSuitableDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Removed_NotSuitable_Date) : "";
            string removedNotSuitableSignedBy = currentMilRepStatus != null ? currentMilRepStatus.Removed_NotSuitable_SignedBy : "";
            string milEmplAdministration = currentMilRepStatus != null && currentMilRepStatus.MilEmployed_Administration != null ? currentMilRepStatus.MilEmployed_Administration.AdministrationName : ""; ;
            string milEmplDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.MilEmployed_Date) : "";
            string temporaryRemovedReason = currentMilRepStatus != null && currentMilRepStatus.TemporaryRemoved_Reason != null ? currentMilRepStatus.TemporaryRemoved_Reason.Text() : ""; ;
            string temporaryRemovedDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.TemporaryRemoved_Date) : "";
            string temporaryRemovedDuration = currentMilRepStatus != null ? currentMilRepStatus.TemporaryRemoved_Duration.ToString() : ""; ;
            string postponeType = currentMilRepStatus != null && currentMilRepStatus.Postpone_Type != null ? currentMilRepStatus.Postpone_Type.Text() : ""; ;
            string postponeYear = currentMilRepStatus != null ? currentMilRepStatus.Postpone_Year.ToString() : ""; ;
            string destMilDepartment = currentMilRepStatus != null && currentMilRepStatus.DestMilDepartment != null ? currentMilRepStatus.DestMilDepartment.MilitaryDepartmentName : ""; ;
            string dischargeDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.DischargeDate) : "";

            string removedDeceasedDisplay = "none";
            string removedAgeLimitDisplay = "none";
            string removedNotSuitableDisplay = "none";
            switch (removedReason)
            {
                case "Починал":
                    removedDeceasedDisplay = "";
                    break;
                case "Пределна възраст":
                    removedAgeLimitDisplay = "";
                    break;
                case "НГВС с изключване":
                    removedNotSuitableDisplay = "";
                    break;
            }

            string annexRows = "";
            var annexRowsCount = voluntaryReserveAnnexes.Count;
            for (int i = 1; i <= annexRowsCount; i++)
            {
                var annexNumber = voluntaryReserveAnnexes[i - 1].AnnexNumber;
                var annexDate = CommonFunctions.FormatDate(voluntaryReserveAnnexes[i - 1].AnnexDate);
                var annexDurationMonths = voluntaryReserveAnnexes[i - 1].AnnexDurationMonths.ToString();
                var annexExpireDate = CommonFunctions.FormatDate(voluntaryReserveAnnexes[i - 1].AnnexExpireDate);

                annexRows += @"<tr id=""rowAnnex" + i + @""" class=""rowAnnex"" style=""min-height: 17px"">
                                <td style=""text-align: right;"">
                                    <span id=""lblAnnexNumber" + i + @""" class=""InputLabel"">Доп. сп. №:</span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""txtAnnexNumber" + i + @""" class=""ReadOnlyValue"" style=""width: 120px;"">" + annexNumber + @"</span>
                                </td>
                                <td style=""text-align: right;"">
                                    <span id=""lblAnnexDate" + i + @""" class=""InputLabel"">от дата:</span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""txtAnnexDate" + i + @""" class=""ReadOnlyValue"" style=""width: 80px;"">" + annexDate + @"</span>
                                </td>
                                <td style=""text-align: right;"">
                                    <span id=""lblAnnexDurationMonths" + i + @""" class=""InputLabel"">Срок:</span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""txtAnnexDurationMonths" + i + @""" class=""ReadOnlyValue"">" + annexDurationMonths + @"</span>
                                </td>
                                <td style=""text-align: right;"" nowrap>
                                    <span id=""lblAnnexExpireDate" + i + @""" class=""InputLabel"">изтича на:</span>
                                </td>
                                <td style=""text-align: left;"">
                                    <span id=""txtAnnexExpireDate" + i + @""" class=""ReadOnlyValue"">" + annexExpireDate + @"</span>
                                </td>
                            </tr>";
            }

            string btnAddNewStatusHTML = "";
            string btnEditCurrStatusHTML = "";
            string btnHistoryStatusesHTML = "";

            string btnPrintUOHTML = "";

            btnAddNewStatusHTML = @"<img id=""btnAddNewResMilRepStatus"" src='../Images/index_new.png' alt='Смяна на състоянието по отчета' title='Смяна на състоянието по отчета' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnAddNewResMilRepStatus_Click();'  />";

            btnEditCurrStatusHTML = @"<img id=""btnEditCurrResMilRepStatus"" src='../Images/index_preferences.png' alt='Редактиране детайлите на текущото състояние по отчета' title='Редактиране детайлите на текущото състояние по отчета' class='GridActionIcon' style='width: 22px; height: 22px;display: " + (currentMilRepStatus != null ? "" : "none") + ";' onclick='btnEditCurrStatusHTML_Click();'  />";

            btnHistoryStatusesHTML = @"<img id=""btnHistoryStatuses"" src='../Images/index_view.png' alt='История' title='История' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnHistoryStatuses_Click();'  />";

            if (currentMilRepStatus != null &&
                page.GetUIItemAccessLevel("RES_PRINT") != UIAccessLevel.Hidden &&
                page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_RESERVISTS") != UIAccessLevel.Hidden)
            {
                btnPrintUOHTML = @"<tr>
                               <td colspan=""3"" style=""text-align:center;"">                                   
                                   <div id=""btnPrintUO"" style=""display: inline-block;margin-top:10px;"" onclick=""PrintUO();"" class=""Button"" olddisabled="""">
                                       <i></i><div id=""btnPrintUO"" style=""width: 170px;"">Печат на удостоверение</div><b></b>
                                   </div>                                    
                               </td>
                           </tr>";
            }

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">
   <input type=""hidden"" id=""reservistMilRepStatusId"" value=""" + (currentMilRepStatus!= null ? currentMilRepStatus.ReservistMilRepStatusId.ToString() : "0")  + @""" />
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 200px;"">
                        <span id=""lblFirstEnrolDate"" class=""InputLabel"">Първоначално зачисляване:</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        <span id=""lblFirstEnrolDateValue"" class=""ReadOnlyValue"">" + firstEnrolDate + @"</span>
                    </td>
                    <td style=""text-align: right; width: 160px;"">
                        <span id=""lblCurrEnrolDate"" class=""InputLabel"">Дата на промяна:</span>
                    </td>
                    <td style=""text-align: left; width: 200px;"">
                        <span id=""lblCurrEnrolDateValue"" class=""ReadOnlyValue"">" + currEnrolDate + @"</span>
                    </td>
                    <td style=""width: 150px;"">
                       <div style=""text-align: right; position: relative; top: -5px; left: 5px;"">
                          " + btnAddNewStatusHTML + btnEditCurrStatusHTML + btnHistoryStatusesHTML + @"
                       </div>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblCurrMilitaryReportStatus"" class=""InputLabel"">Състояние по отчета:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblCurrMilitaryReportStatusValue"" class=""ReadOnlyValue"">" + currMilRepStatusName + @"</span>
                    </td>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblCurrSourceMilDepartmentName"" class=""InputLabel"">Военно окръжие:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblCurrSourceMilDepartmentNameValue"" class=""ReadOnlyValue"">" + currSourceMilDepartmentName + @"</span>
                    </td>
                </tr>   
                <tr>
                    <td colspan=""4"" style=""text-align: center;"">
                        <div id=""divSectionVoluntary"" style=""display: " + voluntaryDisplay + @"; width: 100%;"">
                            <table id=""tblVoluntary"" style=""text-align: center; width: 100%;"">
                                <tr id=""rowVoluntary1"" style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryContractNumber"" class=""InputLabel"">Договор №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryContractNumber"" class=""ReadOnlyValue"" style=""width: 120px;"">" + voluntaryContractNumber + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryContractDate"" class=""InputLabel"">от дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryContractDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + voluntaryContractDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryDurationMonths"" class=""InputLabel"">Срок:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryDurationMonths"" class=""ReadOnlyValue"">" + voluntaryDurationMonths + @"</span>
                                    </td>

                                    <td style=""text-align: right;"" nowrap>
                                        <span id=""lblVoluntaryExpireDate"" class=""InputLabel"">изтича на:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryExpireDate"" class=""ReadOnlyValue"">" + voluntaryExpireDate + @"</span>
                                    </td>

                                </tr>"
                                + annexRows +
                                @"<tr id=""rowVoluntary2"" style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryFulfilPlace"" class=""InputLabel"">Място на изпълнение:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryFulfilPlace"" class=""ReadOnlyValue"" style=""width: 120px;"">" + voluntaryFulfilPlace + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryMilitaryRank"" class=""InputLabel"">На звание:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtVoluntaryMilitaryRank"" class=""ReadOnlyValue"">" + voluntaryMilitaryRank + @"</span>                                
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryMilitaryPosition"" class=""InputLabel"">Длъжност:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryMilitaryPosition"" class=""ReadOnlyValue"">" + voluntaryMilitaryPosition + @"</span>                               
                                    </td>

                                    <td colspan=""2""></td>
                                </tr>
                                <tr id=""rowVoluntary3"" style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryMilRepSpecType"" class=""InputLabel"">Тип ВОС:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtVoluntaryMilRepSpecType"" class=""ReadOnlyValue"">" + voluntaryMilRepSpecType + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryMilRepSpec"" class=""InputLabel"">ВОС:</span>
                                    </td>
                                    <td colspan=""3"" style=""text-align: left;""> 
                                        <span id=""txtVoluntaryMilRepSpec"" class=""ReadOnlyValue"">" + voluntaryMilRepSpec + @"</span>
                                    </td>

                                    <td colspan=""2""></td>
                                </tr>
                            </table>
                        </div>
                        <div id=""divSectionRemoved"" style=""display: " + removedDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedDate"" class=""InputLabel"">Изключен от отчет:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + removedDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedReason"" class=""InputLabel"">Причина за изключване:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtRemovedReason"" class=""ReadOnlyValue"">" + removedReason + @"</span>
                                    </td>
                                </tr>
                                <tr id=""rowRemovedDeceased"" style=""min-height: 17px; display: " + removedDeceasedDisplay + @";"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedDeceasedDeathCert"" class=""InputLabel"">Смъртен акт №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedDeceasedDeathCert"" class=""ReadOnlyValue"">" + removedDeceasedDeathCert + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedDeceasedDate"" class=""InputLabel"">Дата на смъртен акт:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedDeceasedDate"" class=""ReadOnlyValue"">" + removedDeceasedDate + @"</span>
                                    </td>
                                </tr>

                                <tr id=""rowRemovedAgeLimit1"" style=""min-height: 17px; display: " + removedAgeLimitDisplay + @";"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedAgeLimitOrder"" class=""InputLabel"">Заповед №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedAgeLimitOrder"" class=""ReadOnlyValue"">" + removedAgeLimitOrder + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedAgeLimitDate"" class=""InputLabel"">Дата на заповед:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedAgeLimitDate"" class=""ReadOnlyValue"">" + removedAgeLimitDate + @"</span>
                                    </td>
                                </tr>
                                <tr id=""rowRemovedAgeLimit2"" style=""min-height: 17px; display: " + removedAgeLimitDisplay + @";"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedAgeLimitSignedBy"" class=""InputLabel"">Подписана от:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedAgeLimitSignedBy"" class=""ReadOnlyValue"">" + removedAgeLimitSignedBy + @"</span>
                                    </td>
                                </tr>

                                <tr id=""rowRemovedNotSuitable1"" style=""min-height: 17px; display: " + removedNotSuitableDisplay + @";"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedNotSuitableCert"" class=""InputLabel"">Удостоверение №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedNotSuitableCert"" class=""ReadOnlyValue"">" + removedNotSuitableCert + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedNotSuitableDate"" class=""InputLabel"">Дата на удостоверение:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedNotSuitableDate"" class=""ReadOnlyValue"">" + removedNotSuitableDate + @"</span>
                                    </td>
                                </tr>
                                <tr id=""rowRemovedNotSuitable2"" style=""min-height: 17px; display: " + removedNotSuitableDisplay + @";"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedNotSuitableSignedBy"" class=""InputLabel"">Подписано от:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedNotSuitableSignedBy"" class=""ReadOnlyValue"">" + removedNotSuitableSignedBy + @"</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id=""divSectionMilEmployed"" style=""display: " + milEmplDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblMilEmplAdministration"" class=""InputLabel"">Ведомство:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtMilEmplAdministration"" class=""ReadOnlyValue"">" + milEmplAdministration + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblMilEmplDate"" class=""InputLabel"">Дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtMilEmplDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + milEmplDate + @"</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id=""divSectionTemporaryRemoved"" style=""display: " + temporaryRemovedDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedReason"" class=""InputLabel"">Причина за временно снемане:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtTemporaryRemovedReason"" class=""ReadOnlyValue"">" + temporaryRemovedReason + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedDate"" class=""InputLabel"">Начална дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtTemporaryRemovedDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + temporaryRemovedDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedDuration"" class=""InputLabel"">Продължителност:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtTemporaryRemovedDuration"" class=""ReadOnlyValue"" style=""width: 40px"">" + temporaryRemovedDuration + @"</span>    
                                        <span class=""InputLabel"">месеца</span>                           
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id=""divSectionPostpone"" style=""display: " + postponedDisplay + @"; width: 100%; padding-top: 15px;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right; vertical-align: top; width: 120px;"">
                                        <span id=""lblPostponeType"" class=""InputLabel"">Вид отсрочване:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;""> 
                                        <span id=""txtPostponeType"" class=""ReadOnlyValue"">" + postponeType + @"</span>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeWorkCompany"" class=""InputLabel"">Месторабота:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;""> 
                                        <span id=""lblPostponeWorkCompanyValue"" class=""ReadOnlyValue"">" + (reservist.Person.WorkCompany == null ? "" : reservist.Person.WorkCompany.UnifiedIdentityCode + " " + reservist.Person.WorkCompany.CompanyName) + @"</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeYear"" class=""InputLabel"">За коя година:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""txtPostponeYear"" class=""ReadOnlyValue"" style=""width: 40px"">" + postponeYear + @"</span>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeWorkPositionNKPD"" class=""InputLabel"">НКПД:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;""> 
                                        <span id=""lblPostponeWorkPositionNKPDValue"" class=""ReadOnlyValue"">" + (reservist.Person.WorkPositionNKPD == null ? "" : reservist.Person.WorkPositionNKPD.ClassAndCodeAndNameDisplay) + @"</span>
                                    </td>
                                </tr>
                                " + btnPrintUOHTML + @"
                                
                            </table>
                        </div>
                        <div id=""divSectionDischarged"" style=""display: " + dischargedDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblDestMilDepartment"" class=""InputLabel"">ВО, в което отива:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtDestMilDepartment"" class=""ReadOnlyValue"">" + destMilDepartment + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblDischargeDate"" class=""InputLabel"">Дата на отчисляване:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtDischargeDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + dischargeDate + @"</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanMilRepStatusSectionMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }        

        //Render the AddEditMilRepStatus light-box
        public static string GetAddEditMilRepStatusLightBox(Reservist reservist, User currentUser, string moduleKey, AddEditReservist pPage)
        {
            string ddCurrSourceMilitaryDepartmentHtml = "";
            string ddDestMilitaryDepartmentHtml = "";

            List<MilitaryDepartment> milDepts = MilitaryDepartmentUtil.GetAllMilitaryDepartments(currentUser);
            List<MilitaryDepartment> allMilDepts = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsWithoutRestrictions(currentUser);

            List<IDropDownItem> milDeptsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryDepartment milDept in milDepts)
                milDeptsDropDownItems.Add(milDept as IDropDownItem);

            List<IDropDownItem> allMilDeptsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryDepartment milDept in allMilDepts)
                allMilDeptsDropDownItems.Add(milDept as IDropDownItem);

            ddCurrSourceMilitaryDepartmentHtml = ListItems.GetDropDownHtml(milDeptsDropDownItems, null, "ddCurrSourceMilDepartmentNameLightBox", true, null, null, @"UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" style=""width: auto;""");
            ddDestMilitaryDepartmentHtml = ListItems.GetDropDownHtml(allMilDeptsDropDownItems, null, "ddDestMilDepartmentLightBox", true, null, null, @"UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" style=""width: auto;""");

            string ddVoluntaryMilitaryRankHtml = "";

            List<MilitaryRank> milRanks = MilitaryRankUtil.GetAllMilitaryRanks(currentUser);

            List<IDropDownItem> milRanksDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryRank milRank in milRanks)
                milRanksDropDownItems.Add(milRank as IDropDownItem);

            ddVoluntaryMilitaryRankHtml = ListItems.GetDropDownHtml(milRanksDropDownItems, null, "ddVoluntaryMilitaryRankLightBox", true, null, null, @"style=""width: 120px;""  UnsavedCheckSkipMe=""true""");

            string ddVoluntaryMilRepSpecTypeHtml = "";

            List<MilitaryReportSpecialityType> milRepSpecTypes = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(currentUser);

            List<IDropDownItem> milRepSpecTypeDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryReportSpecialityType milRepSpecType in milRepSpecTypes)
                milRepSpecTypeDropDownItems.Add(milRepSpecType as IDropDownItem);

            ddVoluntaryMilRepSpecTypeHtml = ListItems.GetDropDownHtml(milRepSpecTypeDropDownItems, null, "ddVoluntaryMilRepSpecTypeLightBox", true, null, "VoluntaryMilRepSpecTypeLightBoxChanged();", @"UnsavedCheckSkipMe=""true""");

            string ddRemovedReasonsHtml = "";

            List<GTableItem> removedReasons = GTableItemUtil.GetAllGTableItemsByTableName("MilRepStat_RemovedReasons", moduleKey, 1, 0, 0, currentUser);

            List<IDropDownItem> removedReasonsDropDownItems = new List<IDropDownItem>();
            foreach (GTableItem removedReason in removedReasons)
                removedReasonsDropDownItems.Add(removedReason as IDropDownItem);

            ddRemovedReasonsHtml = ListItems.GetDropDownHtml(removedReasonsDropDownItems, null, "ddRemovedReasonLightBox", true, null, "RemovedReasonsLightBoxChanged()", @"UnsavedCheckSkipMe=""true""");

            string ddMilEmplAdministrationHtml = "";

            List<Administration> milEmplAdministrations = AdministrationUtil.GetAllAdministrations(currentUser);

            List<IDropDownItem> milEmplAdministrationsDropDownItems = new List<IDropDownItem>();
            foreach (Administration milEmplAdministration in milEmplAdministrations)
                milEmplAdministrationsDropDownItems.Add(milEmplAdministration as IDropDownItem);

            ddMilEmplAdministrationHtml = ListItems.GetDropDownHtml(milEmplAdministrationsDropDownItems, null, "ddMilEmplAdministrationLightBox", true, null, null, @"UnsavedCheckSkipMe=""true""");

            string ddTemporaryRemovedReasonsHtml = "";

            List<GTableItem> temporaryRemovedReasons = GTableItemUtil.GetAllGTableItemsByTableName("MilRepStat_ТemporaryRemovedReasons", moduleKey, 1, 0, 0, currentUser);

            List<IDropDownItem> temporaryRemovedReasonsDropDownItems = new List<IDropDownItem>();
            foreach (GTableItem temporaryRemovedReason in temporaryRemovedReasons)
                temporaryRemovedReasonsDropDownItems.Add(temporaryRemovedReason as IDropDownItem);

            ddTemporaryRemovedReasonsHtml = ListItems.GetDropDownHtml(temporaryRemovedReasonsDropDownItems, null, "ddTemporaryRemovedReasonsLightBox", true, null, null, @"UnsavedCheckSkipMe=""true""");

            string ddPostponeTypeHtml = "";

            List<PostponeType> postponeTypes = PostponeTypeUtil.GetAllPostponeTypes(currentUser);

            List<IDropDownItem> postponeTypesDropDownItems = new List<IDropDownItem>();
            foreach (PostponeType postponeType in postponeTypes)
                postponeTypesDropDownItems.Add(postponeType as IDropDownItem);

            ddPostponeTypeHtml = ListItems.GetDropDownHtml(postponeTypesDropDownItems, null, "ddPostponeTypeLightBox", true, null, null, @"UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" style=""width: auto;""");

            List<MilitaryReportStatus> milRepStatuses = MilitaryReportStatusUtil.GetAllMilitaryReportStatuses(currentUser);

            string todayDate = CommonFunctions.FormatDate(DateTime.Now.Date);

            //Generate MilitaryUnitSelector
            MilitaryUnitSelector.MilitaryUnitSelector voluntaryFulfilPlace = new MilitaryUnitSelector.MilitaryUnitSelector();
            voluntaryFulfilPlace.Page = pPage;
            voluntaryFulfilPlace.DataSourceWebPage = "DataSource.aspx";
            voluntaryFulfilPlace.DataSourceKey = "MilitaryUnit";
            voluntaryFulfilPlace.ResultMaxCount = 1000;
            voluntaryFulfilPlace.Style.Add("width", "100px");
            voluntaryFulfilPlace.DivMainCss = "isDivMainClass";
            voluntaryFulfilPlace.DivListCss = "isDivListClass";
            voluntaryFulfilPlace.DivFullListCss = "isDivFullListClass";
            voluntaryFulfilPlace.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            voluntaryFulfilPlace.IncludeOnlyActual = false;
            voluntaryFulfilPlace.ID = "itmsVoluntaryFulfilPlace";          
           
            if (pPage.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYFULFILPLACE") == UIAccessLevel.Disabled)
                voluntaryFulfilPlace.Enabled = false;
           
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            voluntaryFulfilPlace.RenderControl(tw);


            string html = @"
<div id=""lboxAddEditMilRepStatus"" style=""display: none;"" class=""lboxAddEditMilRepStatus"">
<center>
    <input type=""hidden"" id=""hdnReservistMilRepStatusID"" />
    <table width=""100%"" style=""text-align: center;"">
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""4"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilRepStatusTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCurrentMilRepStatusLightBox"" class=""InputLabel"">Текущо състояние:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""lblCurrentMilRepStatusValueLightBox"" class=""ReadOnlyValue""></span>
            </td>
            <td style=""text-align: right;"">
                <span id=""lblNewMilRepStatus"" class=""InputLabel"">Ново състояние:</span>
            </td>
            <td style=""text-align: left;"">
                <select id=""ddNewMilRepStatus"" onchange=""NewMilRepStatusChanged();"" UnsavedCheckSkipMe=""true"" class=""RequiredInputField""></select>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCurrEnrolDateLightBox"" class=""InputLabel"">Дата на промяна:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanCurrEnrolDateLightBox"">
                    <input id=""txtCurrEnrolDateLightBox"" class=""RequiredInputField " + CommonFunctions.DatePickerCSS() + @""" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                </span>
            </td>
            <td style=""text-align: right;"">
                <span id=""lblCurrSourceMilDepartmentNameLightBox"" class=""InputLabel"">Военно окръжие:</span>
            </td>
            <td style=""text-align: left;""> "
                + ddCurrSourceMilitaryDepartmentHtml + @"
            </td>
        </tr>
        <tr>
            <td colspan=""4"" style=""text-align: center;"">
                <div id=""divVoluntary"" style=""display: none; width: 100%;"">
                    <table id=""tblVoluntaryLightBox"" style=""text-align: center; width: 100%;"">
                        <tr id=""rowVoluntaryLightBox1"" style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryContractNumberLightBox"" class=""InputLabel"">Договор №:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtVoluntaryContractNumberLightBox"" class=""InputField"" style=""width: 120px;"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryContractDateLightBox"" class=""InputLabel"">от дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanVoluntaryContractDateLightBox"">
                                    <input id=""txtVoluntaryContractDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryDurationMonthsLightBox"" class=""InputLabel"">Срок:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtVoluntaryDurationMonthsLightBox"" class=""InputField"" UnsavedCheckSkipMe=""true"" style=""width:100px;""></input>
                            </td>
                            <td style=""text-align: right;"" nowrap>
                                <span id=""lblVoluntaryExpireDateLightBox"" class=""InputLabel"">изтича на:</span>
                            </td>
                            <td style=""text-align: left;"" nowrap>
                                <span id=""spanVoluntaryExpireDateLightBox"">
                                    <input id=""txtVoluntaryExpireDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                            <td class=""btnAddAnnex"">
                                <img id=""btnAddAnnex"" src=""../Images/addrow.gif"" alt=""Добавяне на допълнително споразумение"" title=""Добавяне на допълнително споразумение"" class=""btnNewTableRecordIcon"" onclick=""btnAddAnnex_Click();"" />
                            </td>
                        </tr>

                        <tr id=""rowVoluntaryLightBox2"" style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryFulfilPlaceLightBox"" class=""InputLabel"">Място на изпълнение:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanVoluntaryFulfilPlaceLightBox"">" + sw.ToString() + @"</span>                                
                            </td>

                            <td style=""text-align: right; width: 70px"">
                                <span id=""lblVoluntaryMilitaryRankLightBox"" class=""InputLabel"">На звание:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddVoluntaryMilitaryRankHtml + @"                                
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryMilitaryPositionLightBox"" class=""InputLabel"">Длъжност:</span>
                            </td>
                            <td style=""text-align: left;"" colspan=""3"">
                                <input id=""txtVoluntaryMilitaryPositionLightBox"" class=""InputField"" style=""width: 300px"" UnsavedCheckSkipMe=""true""></input>                               
                            </td>
                        </tr>

                        <tr id=""rowVoluntaryLightBox3"" style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryMilRepSpecTypeLightBox"" class=""InputLabel"">Тип ВОС:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddVoluntaryMilRepSpecTypeHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryMilRepSpecLightBox"" class=""InputLabel"">ВОС:</span>
                            </td>
                            <td colspan=""5"" style=""text-align: left;""> 
                                <select id=""ddVoluntaryMilRepSpecLightBox"" style=""width: 513px"" UnsavedCheckSkipMe=""true""></select>
                            </td>
                        </tr>                        
                    </table>
                </div>
                <div id=""divRemoved"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">                                
                                <span id=""lblRemovedDateLightBox"" class=""InputLabel"">Изключен от отчет:</span>                                
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanRemovedDateLightBox"">
                                    <input id=""txtRemovedDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedReasonLightBox"" class=""InputLabel"">Причина за изключване:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddRemovedReasonsHtml +
                            @"
                            </td>
                        </tr>

                        <tr id=""rowRemovedDeceasedLightBox"" style=""min-height: 17px; display: none;"">
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedDeceasedDeathCertLightBox"" class=""InputLabel"">Смъртен акт №:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtRemovedDeceasedDeathCertLightBox"" class=""InputField"" style=""width: 240px;"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedDeceasedDateLightBox"" class=""InputLabel"">Дата на смъртен акт:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanRemovedDeceasedDateLightBox"">
                                    <input id=""txtRemovedDeceasedDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                        </tr>

                        <tr id=""rowRemovedAgeLimit1LightBox"" style=""min-height: 17px; display: none;"">
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedAgeLimitOrderLightBox"" class=""InputLabel"">Заповед №:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtRemovedAgeLimitOrderLightBox"" class=""InputField"" style=""width: 240px;"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedAgeLimitDateLightBox"" class=""InputLabel"">Дата на заповед:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanRemovedAgeLimitDateLightBox"">
                                    <input id=""txtRemovedAgeLimitDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                        </tr>
                        <tr id=""rowRemovedAgeLimit2LightBox"" style=""min-height: 17px; display: none;"">
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedAgeLimitSignedByLightBox"" class=""InputLabel"">Подписана от:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtRemovedAgeLimitSignedByLightBox"" class=""InputField"" UnsavedCheckSkipMe=""true"" style=""width: 240px;""></input>
                            </td>
                        </tr>

                        <tr id=""rowRemovedNotSuitable1LightBox"" style=""min-height: 17px; display: none;"">
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedNotSuitableCertLightBox"" class=""InputLabel"">Удостоверение №:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtRemovedNotSuitableCertLightBox"" class=""InputField"" style=""width: 240px;"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedNotSuitableDateLightBox"" class=""InputLabel"">Дата на удостоверение:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanRemovedNotSuitableDateLightBox"">
                                    <input id=""txtRemovedNotSuitableDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                        </tr>
                        <tr id=""rowRemovedNotSuitable2LightBox"" style=""min-height: 17px; display: none;"">
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedNotSuitableSignedByLightBox"" class=""InputLabel"">Подписано от:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtRemovedNotSuitableSignedByLightBox"" class=""InputField"" UnsavedCheckSkipMe=""true"" style=""width: 240px;""></input>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id=""divMilEmployed"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblMilEmplAdministrationLightBox"" class=""InputLabel"">Ведомство:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddMilEmplAdministrationHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblMilEmplDateLightBox"" class=""InputLabel"">Дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanMilEmplDateLightBox"">
                                    <input id=""txtMilEmplDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""divTemporaryRemoved"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedReasonLightBox"" class=""InputLabel"">Причина за временно снемане:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddTemporaryRemovedReasonsHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedDateLightBox"" class=""InputLabel"">Начална дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanTemporaryRemovedDateLightBox"">
                                    <input id=""txtTemporaryRemovedDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                <span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedDurationLightBox"" class=""InputLabel"">Продължителност:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtTemporaryRemovedDurationLightBox"" class=""InputField"" style=""width: 40px"" UnsavedCheckSkipMe=""true""></input>    
                                <span class=""InputLabel"">месеца</span>                           
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""divPostpone"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right; vertical-align: top; width: 120px;"">
                                <span id=""lblPostponeTypeLightBox"" class=""InputLabel"">Вид отсрочване:</span>
                            </td>
                            <td style=""text-align: left; vertical-align: top;""> "
                                + ddPostponeTypeHtml +
                            @"
                            </td>
                            <td style=""text-align: right; vertical-align: top;"">
                                <span id=""lblPostponeWorkCompanyLightBox"" class=""InputLabel"">Месторабота:</span>
                            </td>
                            <td style=""text-align: left; vertical-align: top;"">
                               <span id=""lblPostponeWorkCompanyValueLightBox"" class=""ReadOnlyValue"">" +
                                  (reservist.Person.WorkCompany == null ? "" : reservist.Person.WorkCompany.UnifiedIdentityCode + " " + reservist.Person.WorkCompany.CompanyName) +
                               @"</span>
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right; vertical-align: top;"">
                                <span id=""lblPostponeYearLightBox"" class=""InputLabel"">За коя година:</span>
                            </td>
                            <td style=""text-align: left; vertical-align: top;"">
                                <input id=""txtPostponeYearLightBox"" class=""RequiredInputField"" style=""width: 40px"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right; vertical-align: top;"">
                                <span id=""lblPostponeWorkPositionNKPDLightBox"" class=""InputLabel"">НКПД:</span>
                            </td>
                            <td style=""text-align: left; vertical-align: top;"">
                                <span id=""lblPostponeWorkPositionNKPDValueLightBox"" class=""ReadOnlyValue"">" + 
                                     (reservist.Person.WorkPositionNKPD == null ? "" : reservist.Person.WorkPositionNKPD.ClassAndCodeAndNameDisplay) + 
                              @"</span>              
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""divDischarged"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblDestMilDepartmentLightBox"" class=""InputLabel"">ВО, в което отива:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddDestMilitaryDepartmentHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblDischargeDateLightBox"" class=""InputLabel"">Дата на отчисляване:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanDischargeDateLightBox"">
                                    <input id=""txtDischargeDateLightBox"" class=""RequiredInputField " + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true"" defaultvalue=""" + todayDate + @"""></input>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>                    
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""4"">
                <span id=""spanAddEditMilRepStatusLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""4"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilRepStatusLightBox"" onclick=""SaveAddEditMilRepStatusLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilRepStatusLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditCivilEducationLightBox"" onclick=""HideAddEditMilRepStatusLightBox();""
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
</center>
</div>
";

            return html;
        }

        public static string GetMilRepStatusHistoryLightBox(User currentUser, HttpRequest Request)
        {           
            string html = "";

            string htmlNoResults = "";

            List<ReservistMilRepStatus> reservistMilRepStatuses = new List<ReservistMilRepStatus>();            
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            if (Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null)
            {                
                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            int reservistId = 0;
            int.TryParse((Request.Params["ReservistId"]).ToString(), out reservistId);

            allRows = ReservistMilRepStatusUtil.GetAllReservistMilRepStatusByReservistIdCount(reservistId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            reservistMilRepStatuses = ReservistMilRepStatusUtil.GetAllReservistMilRepStatusByReservistId(reservistId, orderBy, pageIndex, pageLength, currentUser);            

            // No data found
            if (reservistMilRepStatuses.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }
           
            //Set pagination section
            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnNext = "src='../Images/ButtonNext.png'";
            string btnLast = "src='../Images/ButtonLast.png'";

            if (pageIndex == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (pageIndex == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string pageTablePagination = " | " + pageIndex + " от " + maxPage + " | ";

            // Setup the header of the grid
            html += @"<center>
                      <div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>История на отчета</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblReservistMilRepStatusHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 80px; " + headerStyle + @"'>Дата</th>
                               <th style='width: 130px; " + headerStyle + @"'>Статус</th>
                               <th style='width: 180px; " + headerStyle + @"'>ВО</th>
                               <th style='width: 80px; " + headerStyle + @"'>Отчислен</th>
                               <th style='width: 180px; " + headerStyle + @"'>ВО, в което отива</th>
                           </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (ReservistMilRepStatus reservistMilRepStatus in reservistMilRepStatuses)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(reservistMilRepStatus.EnrolDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (reservistMilRepStatus.MilitaryReportStatus != null ? reservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName : "")  + @"</td>
                                 <td style='" + cellStyle + @"'>" + (reservistMilRepStatus.SourceMilDepartment != null ? reservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(reservistMilRepStatus.DischargeDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (reservistMilRepStatus.DestMilDepartment != null ? reservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : "") + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnReservistMilRepStatusHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblReservistMilRepStatusHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseReservistMilRepStatusHistoryTable' runat='server' class='Button' onclick=""HideReservistMilRepStatusHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        public static string GetReservistAppointmentSection(Reservist reservist, User currentUser, RESPage page)
        {
            ReservistAppointment reservistAppointment = ReservistAppointmentUtil.GetCurrentReservistAppointmentByReservistId(reservist.ReservistId, currentUser);
            FillReservistsRequest request = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservist.ReservistId, currentUser).FirstOrDefault();

            string reqOrderNumber = reservistAppointment != null ? reservistAppointment.ReqOrderNumber : "";
            string receiptAppointmentDate = reservistAppointment != null ? CommonFunctions.FormatDate(reservistAppointment.ReceiptAppointmentDate) : "";
            string militaryCommandName = reservistAppointment != null ? reservistAppointment.MilitaryCommandName : "";
            string militaryCommandSuffix = reservistAppointment != null ? reservistAppointment.MilitaryCommandSuffix : "";
            string reservistReadiness = reservistAppointment != null && reservistAppointment.ReservistReadinessId.HasValue ? ReadinessUtil.ReadinessName(reservistAppointment.ReservistReadinessId.Value) : "";
            string militaryRank = reservistAppointment != null ? reservistAppointment.MilitaryRankName : "";
            string milRepSpec = reservistAppointment != null ? reservistAppointment.MilReportingSpecialityCode + " " + reservistAppointment.MilReportingSpecialityName : "";
            string position = reservistAppointment != null ? reservistAppointment.Position : "";
            string appointmentTime = reservistAppointment != null && reservistAppointment.AppointmentTime.HasValue ? reservistAppointment.AppointmentTime.Value.ToString() : "";
            string appointmentPlace = reservistAppointment != null ? reservistAppointment.AppointmentPlace : "";
            string needCourseChecked = request != null && request.NeedCourse ? @"checked=""checked""" : "";
            string appointmentIsDelivered = request != null && request.AppointmentIsDelivered ? @"checked=""checked""" : "";
            
            string btnHistoryAppointmentsHTML = "";
            btnHistoryAppointmentsHTML = @"<img id=""btnHistoryAppointments"" src='../Images/index_view.png' alt='История' title='История' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnHistoryAppointments_Click();'  />";

            string btnPrintMKHTML = "";
            string btnPrintPZHTML = "";

            if (reservistAppointment != null &&
                page.GetUIItemAccessLevel("RES_PRINT") != UIAccessLevel.Hidden &&
                page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS") != UIAccessLevel.Hidden)
            {
                if (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_MK") != UIAccessLevel.Hidden)
                    btnPrintMKHTML = @"<div id=""btnPrintMK"" style=""display: inline;"" onclick=""PrintMK();""class=""Button"">
                                           <i></i>
                                           <div id=""btnPrintMKText"" style=""width: 90px;"">Печат на МК</div>
                                           <b></b>
                                       </div>";

                if (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_PZ") != UIAccessLevel.Hidden)
                    btnPrintPZHTML = @"<div id=""btnPrintPZ"" style=""display: inline;"" onclick=""PrintPZ();""class=""Button"">
                                           <i></i>
                                           <div id=""btnPrintPZText"" style=""width: 90px;"">Печат на ПЗ</div>
                                           <b></b>
                                       </div>";
            }

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">
   <input type=""hidden"" id=""reservistAppointmentId"" value=""" + (reservistAppointment != null ? reservistAppointment.ReservistAppointmentId.ToString() : "0") + @""" />
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 150px;"">
                        <span id=""lblReqOrderNumber"" class=""InputLabel"">Номер на заявка:</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        <span id=""lblReqOrderNumberValue"" class=""ReadOnlyValue"">" + reqOrderNumber + @"</span>
                    </td>
                    <td style=""text-align: right; width: 260px;"">
                        <span id=""lblReceiptAppointmentDate"" class=""InputLabel"">Дата на получаване на назначение:</span>
                    </td>
                    <td style=""text-align: left; width: 150px;"">
                        <span id=""lblReceiptAppointmentDateValue"" class=""ReadOnlyValue"">" + receiptAppointmentDate + @"</span>
                    </td>
                    <td style=""width: 150px;"">
                       <div style=""text-align: right; position: relative; top: -5px; left: 5px;"">
                          " + btnHistoryAppointmentsHTML + @"
                       </div>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblMilitaryCommandName"" class=""InputLabel"">Команда:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblMilitaryCommandNameValue"" class=""ReadOnlyValue"">" + militaryCommandName + @"</span>
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblReservistReadiness"" class=""InputLabel"">Начин на явяване:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblReservistReadinessValue"" class=""ReadOnlyValue"">" + reservistReadiness + @"</span>
                    </td>
                </tr> 
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblMilitaryCommandSuffix"" class=""InputLabel"">Буква:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblMilitaryCommandSuffixValue"" class=""ReadOnlyValue"">" + militaryCommandSuffix + @"</span>
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblAppointmentPosition"" class=""InputLabel"">Длъжност:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblAppointmentPositionValue"" class=""ReadOnlyValue"">" + position + @"</span>
                    </td>        
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblAppointmentMilitaryRank"" class=""InputLabel"">На звание:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblAppointmentMilitaryRankValue"" class=""ReadOnlyValue"">" + militaryRank + @"</span>
                    </td>  
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblMilRepSpec"" class=""InputLabel"">Назначен на ВОС:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"">
                        <span id=""lblMilRepSpecValue"" class=""ReadOnlyValue"">" + milRepSpec + @"</span>
                    </td>
                    
                </tr> " + (request != null ?               
                @"<tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <input type=""checkbox"" id=""cbNeedCourse"" class=""InputLabel"" " + needCourseChecked + @"/>                        
                    </td>
                    <td colspan=""3"" style=""text-align: left;"">
                        <span id=""lblNeedCourse"" class=""ReadOnlyValue"">Нуждае се от курс</span>
                    </td>
                    
                </tr> " : "") + @"
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblAppointmentTime"" class=""InputLabel"">Време за явяване:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblAppointmentTimeValue"" class=""ReadOnlyValue"">" + appointmentTime + @"</span>
                    </td>
                    <td style=""text-align: right; display: none;"">
                        <span id=""lblAppointmentPlace"" class=""InputLabel"">Място на явяване:</span>
                    </td>
                    <td style=""text-align: left; display: none;"">
                        <span id=""lblAppointmentPlaceValue"" class=""ReadOnlyValue"">" + appointmentPlace + @"</span>
                    </td>
                </tr>" + (request != null ?
                @"<tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <input type=""checkbox"" id=""cbAppointmentIsDelivered"" class=""InputLabel"" " + appointmentIsDelivered + @"/>                        
                    </td>
                    <td colspan=""3"" style=""text-align: left;"">
                        <span id=""lblAppointmentIsDelivered"" class=""ReadOnlyValue"">Връчено МН</span>
                    </td>
                    
                </tr> " : "") + @"
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanReservistAppointmentSectionMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
                <tr>        
                    <td colspan=""5"" style=""text-align: center; position: relative; height: 15px;"">
                        <div style=""position: absolute; top: 0px; right: -30px; width: 250px;"">
                            " + btnPrintMKHTML + @"
                            " + btnPrintPZHTML + @"
                        </div>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetReservistAppointmentHistoryLightBox(User currentUser, HttpRequest Request, AddEditReservist_MilitaryReport page)
        {
            string html = "";

            string htmlNoResults = "";

            bool IsReqOrderNumberHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_REQORDERNUMBER") == UIAccessLevel.Hidden;
            bool IsReceiptAppointmentDateHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_RECEIPTAPPOINTMENTDATE") == UIAccessLevel.Hidden;
            bool IsMilitaryCommandNameHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYCOMMAND") == UIAccessLevel.Hidden;
            bool IsMilitaryCommandSuffixHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYCOMMANDSUFFIX") == UIAccessLevel.Hidden;
            bool IsReservistReadinessHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_READINESS") == UIAccessLevel.Hidden;
            bool IsMilitaryRankNameHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYRANK") == UIAccessLevel.Hidden;
            bool IsPositionHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_POSITION") == UIAccessLevel.Hidden;
            bool IsMilReportingSpecialityNameHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILREPSPEC") == UIAccessLevel.Hidden;
            bool IsAppointmentTimeHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_APPOINTMENTTIME") == UIAccessLevel.Hidden;
            bool IsAppointmentPlaceHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_APPOINTMENTPLACE") == UIAccessLevel.Hidden;                   

            List<ReservistAppointment> reservistAppointments = new List<ReservistAppointment>();
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            if (Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null)
            {
                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            int reservistId = 0;
            int.TryParse((Request.Params["ReservistId"]).ToString(), out reservistId);

            allRows = ReservistAppointmentUtil.GetAllReservistAppointmentsByReservistIdCount(reservistId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            reservistAppointments = ReservistAppointmentUtil.GetAllReservistAppointmentsByReservistId(reservistId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (reservistAppointments.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }

            //Set pagination section
            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnNext = "src='../Images/ButtonNext.png'";
            string btnLast = "src='../Images/ButtonLast.png'";

            if (pageIndex == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (pageIndex == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string pageTablePagination = " | " + pageIndex + " от " + maxPage + " | ";

            // Setup the header of the grid
            html += @"<center>
                      <div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>История на мобилизационните назначения</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnReservistAppointmentHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnReservistAppointmentHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnReservistAppointmentHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnReservistAppointmentHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnReservistAppointmentHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblReservistAppointmentHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
  (!IsReqOrderNumberHidden ? @"<th style='width: 80px; " + headerStyle + @"'>Номер на заявка</th>" : "") +
  (!IsReceiptAppointmentDateHidden ? @"<th style='width: 130px; " + headerStyle + @"'>Дата на получаване на назначение</th>" : "") +
  (!IsMilitaryCommandNameHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Команда</th>" : "") +
  (!IsMilitaryCommandSuffixHidden ? @"<th style='width: 50px; " + headerStyle + @"'>Буква</th>" : "") +
  (!IsMilitaryRankNameHidden ? @"<th style='width: 80px; " + headerStyle + @"'>На звание</th>" : "") +
  (!IsReservistReadinessHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Начин на явяване</th>" : "") +
  (!IsMilReportingSpecialityNameHidden ? @"<th style='width: 250px; " + headerStyle + @"'>Назначен на ВОС</th>" : "") +
  (!IsPositionHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Длъжност</th>" : "") +
  (!IsAppointmentTimeHidden ? @"<th style='width: 80px; " + headerStyle + @"'>Време за явяване</th>" : "") +
  (!IsAppointmentPlaceHidden ? @"<th style='width: 180px; display: none; " + headerStyle + @"'>Място на явяване</th>" : "") +
                           @"</tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (ReservistAppointment reservistAppointment in reservistAppointments)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
  (!IsReqOrderNumberHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.ReqOrderNumber + @"</td>" : "") +
  (!IsReceiptAppointmentDateHidden ? @"<td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(reservistAppointment.ReceiptAppointmentDate) + @"</td>" : "") +
  (!IsMilitaryCommandNameHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.MilitaryCommandName + @"</td>" : "") +
  (!IsMilitaryCommandSuffixHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.MilitaryCommandSuffix + @"</td>" : "") +
  (!IsMilitaryRankNameHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.MilitaryRankName + @"</td>" : "") +
  (!IsReservistReadinessHidden ? @"<td style='" + cellStyle + @"'>" + (reservistAppointment.ReservistReadinessId.HasValue ? ReadinessUtil.ReadinessName(reservistAppointment.ReservistReadinessId.Value) : "") + @"</td>" : "") +
  (!IsMilReportingSpecialityNameHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.MilReportingSpecialityName + @"</td>" : "") +
  (!IsPositionHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.Position + @"</td>" : "") +
  (!IsAppointmentTimeHidden ? @"<td style='" + cellStyle + @"'>" + reservistAppointment.AppointmentTime + @"</td>" : "") +
  (!IsAppointmentPlaceHidden ? @"<td style='display: none; " + cellStyle + @"'>" + reservistAppointment.AppointmentPlace + @"</td>" : "") +
                              @"</tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnReservistAppointmentHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblReservistAppointmentHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseReservistAppointmentHistoryTable' runat='server' class='Button' onclick=""HideReservistAppointmentHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        public static string GetGroupManagementSection(Reservist reservist, User currentUser)
        {            
            string ddGMSPunktHtml = "";

            ReservistMilRepStatus currentMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservist.ReservistId, currentUser);

            List<RequestCommandPunkt> requestCommandPunkts = new List<RequestCommandPunkt>();

            if (currentMilRepStatus != null && currentMilRepStatus.SourceMilDepartmentId.HasValue)
                requestCommandPunkts = RequestCommandPunktUtil.GetAllRequestCommandPunktByMilDeptID(currentMilRepStatus.SourceMilDepartmentId.Value, currentUser);

            List<IDropDownItem> requestCommandPunktsDropDownItems = new List<IDropDownItem>();
            foreach (RequestCommandPunkt requestCommandPunkt in requestCommandPunkts)
                requestCommandPunktsDropDownItems.Add(requestCommandPunkt as IDropDownItem);

            ddGMSPunktHtml = ListItems.GetDropDownHtml(requestCommandPunktsDropDownItems, null, "ddGMSPunkt", true, reservist.Punkt, null, @"style=""width: 180px;""");            

            string groupManagementSection = reservist != null ? reservist.GroupManagementSection : "";
            string section = reservist != null ? reservist.Section : "";
            string deliverer = reservist != null ? reservist.Deliverer : "";

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">   
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 10px;"">
      </tr>
      <tr>
           <td style=""text-align: right; width: 150px;"">
                <span id=""lblGMSGroupManagementSection"" class=""InputLabel"">ГРС:</span>
           </td>
           <td style=""text-align: left; width: 250px;"">
                <input type=""text"" id=""txtGMSGroupManagementSection"" class='InputField' value='" + groupManagementSection + @"' />
           </td>
           <td style=""text-align: right;"">
                <span id=""lblGMSSection"" class=""InputLabel"">Секция:</span>
           </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtGMSSection"" class='InputField' value='" + section + @"' />
           </td>
      </tr>
      <tr>
           <td style=""text-align: right;"">
                <span id=""lblGMSDeliverer"" class=""InputLabel"">Връчител:</span>
           </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtGMSDeliverer"" class='InputField' value='" + deliverer + @"' />
           </td>
           <td style=""text-align: right;"">
                <span id=""lblGMSPunkt"" class=""InputLabel"">ПИ/КПП:</span>
           </td>
            <td style=""text-align: left;""> "
              + ddGMSPunktHtml +
             @"</td>
       </tr>                 
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanGMSMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetMedCertSection(Reservist reservist, User currentUser, AddEditReservist page)
        {
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Hidden)
                return "";

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">   
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Медицинско освидетелстване</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left; padding-left: 10px;"">
            <div id=""divMedCertTable"">
                " + GetMedCertTable(reservist, currentUser, page) + @"
            </div>
         </td>
      </tr>
      <tr>        
         <td style=""text-align: left; padding-left: 10px;"">
            <span id=""spanMedCertSectionMsg"" class=""ErrorText""></span>&nbsp;
         </td>        
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetMedCertTable(Reservist reservist, User currentUser, RESPage page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsMedCertDateHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_DATE") == UIAccessLevel.Hidden;
            bool IsProtNumHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_PROTNUM") == UIAccessLevel.Hidden;
            bool IsConclusionHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_CONCLUSION") == UIAccessLevel.Hidden;
            bool IsMedRubricHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_MEDRUBRIC") == UIAccessLevel.Hidden;
            bool IsExpirationDateHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_EXPIRATIONDATE") == UIAccessLevel.Hidden;

            string newHTML = "";

            if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewMedCert();' />";
            }

            StringBuilder tableHTML = new StringBuilder();

            tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
     " + (!IsMedCertDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>" : "") + @"
         " + (!IsProtNumHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Протокол</th>" : "") + @"                    
      " + (!IsConclusionHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Решение</th>" : "") + @"
       " + (!IsMedRubricHidden ? @"<th style='width: 240px; vertical-align: bottom;'>Медицинска рубрика</th>" : "") + @"
  " + (!IsExpirationDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            var personMedCerts = PersonMedCertUtil.GetAllPersonMedCerts(reservist.PersonId, page.CurrentUser);

            foreach (var personMedCert in personMedCerts)
            {
                counter++;

                string deleteHTML = "";

                if (personMedCert.CanDelete)
                {
                    if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteMedCert(" + personMedCert.MedCertID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Enabled && !isPreview
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditMedCert(" + personMedCert.MedCertID.ToString() + ");' />";

                }

                tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center;'>" + counter.ToString() + @"</td>
       " + (!IsMedCertDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.MedCertDate) + @"</td>" : "") + @"
           " + (!IsProtNumHidden ? @"<td style='text-align: left;'>" + personMedCert.ProtNum + @"</td>" : "") + @"                    
        " + (!IsConclusionHidden ? @"<td style='text-align: left;'>" + (personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>" : "") + @"
         " + (!IsMedRubricHidden ? @"<td style='text-align: left;'>" + (personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricTitle.ToString() : "") + @"</td>" : "") + @"
    " + (!IsExpirationDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.ExpirationDate) + @"</td>" : "") + @"
                                     <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            tableHTML.Append("</table>");

            return tableHTML.ToString();
        }

        //Render the Med Cert light-box
        public static string GetMedCertLightBox(User currentUser)
        {
            string ddConclusionHtml = "";

            List<MilitaryMedicalConclusion> militaryMedicalConclusions = MilitaryMedicalConclusionUtil.GetAllMilitaryMedicalConclusions(currentUser);

            List<IDropDownItem> militaryMedicalConclusionsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryMedicalConclusion militaryMedicalConclusion in militaryMedicalConclusions)
                militaryMedicalConclusionsDropDownItems.Add(militaryMedicalConclusion as IDropDownItem);

            ddConclusionHtml = ListItems.GetDropDownHtml(militaryMedicalConclusionsDropDownItems, null, "ddMedCertConclusion", true, null, null, @"style=""width: 120px;""");

            string ddMedRubricHTML = "";

            List<MedicalRubric> medicalRubrics = MedicalRubricUtil.GetAllMedicalRubrics(currentUser);

            List<IDropDownItem> medicalRubricsDropDownItems = new List<IDropDownItem>();

            foreach (MedicalRubric medicalRubric in medicalRubrics)
                medicalRubricsDropDownItems.Add(medicalRubric as IDropDownItem);

            ddMedRubricHTML = ListItems.GetDropDownHtml(medicalRubricsDropDownItems, null, "ddMedCertMedRubric", true, null, null, @"style=""width: 120px;""");


            string html = @"
<div id=""divMedCertLightBox"" style=""display: none;"" class=""lboxMedCert"">
<center>
    <input type=""hidden"" id=""hdnMedCertID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMedCertTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertDate"" class=""InputLabel"">Комисия от дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanMedCertDate"">
                    <input type=""text"" id=""txtMedCertDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertProtNum"" class=""InputLabel"">Протокол:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMedCertProtNum"" class='InputField' />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertConclusion"" class=""InputLabel"">Решение:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddConclusionHtml + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertMedRubric"" class=""InputLabel"">Медицинска рубрика:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddMedRubricHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertExpirationDate"" class=""InputLabel"">Дата на валидност:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanMedCertExpirationDate"">
                    <input type=""text"" id=""txtMedCertExpirationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>

        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditMedCertLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMedCertLightBox"" style=""display: inline;"" onclick=""SaveAddEditMedCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMedCertLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMedCertLightBox"" style=""display: inline;"" onclick=""HideAddEditMedCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditMedCertLightBoxText"" style=""width: 70px;"">
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
</div>";

            return html;
        }

        public static string GetPsychCertSection(Reservist reservist, User currentUser, AddEditReservist page)
        {
            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Hidden)
                return "";

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">   
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Психологическа пригодност</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left; padding-left: 10px;"">
            <div id=""divPsychCertTable"">
                " + GetPsychCertTable(reservist, currentUser, page) + @"
            </div>
         </td>
      </tr>
      <tr>        
         <td style=""text-align: left; padding-left: 10px;"">
            <span id=""spanPsychCertSectionMsg"" class=""ErrorText""></span>&nbsp;
         </td>        
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetPsychCertTable(Reservist reservist, User currentUser, RESPage page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsPsychCertDateHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_DATE") == UIAccessLevel.Hidden;
            bool IsProtNumHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_PROTNUM") == UIAccessLevel.Hidden;
            bool IsConclusionHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_CONCLUSION") == UIAccessLevel.Hidden;
            bool IsExpirationDateHidden = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_EXPIRATIONDATE") == UIAccessLevel.Hidden;

            string newHTML = "";

            if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Enabled && !isPreview
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewPsychCert();' />";
            }

            StringBuilder tableHTML = new StringBuilder();

            tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
   " + (!IsPsychCertDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>" : "") + @"
         " + (!IsProtNumHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Протокол</th>" : "") + @"                    
      " + (!IsConclusionHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Решение</th>" : "") + @"
  " + (!IsExpirationDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            var personPsychCerts = PersonPsychCertUtil.GetAllPersonPsychCerts(reservist.PersonId, page.CurrentUser);

            foreach (var personPsychCert in personPsychCerts)
            {
                counter++;

                string deleteHTML = "";

                if (personPsychCert.CanDelete)
                {
                    if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Enabled && !isPreview
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeletePsychCert(" + personPsychCert.PsychCertID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Enabled && !isPreview
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditPsychCert(" + personPsychCert.PsychCertID.ToString() + ");' />";

                }

                tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center;'>" + counter.ToString() + @"</td>
     " + (!IsPsychCertDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.PsychCertDate) + @"</td>" : "") + @"
           " + (!IsProtNumHidden ? @"<td style='text-align: left;'>" + personPsychCert.ProtNum + @"</td>" : "") + @"                    
        " + (!IsConclusionHidden ? @"<td style='text-align: left;'>" + (personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>" : "") + @"
    " + (!IsExpirationDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.ExpirationDate) + @"</td>" : "") + @"
                                     <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            tableHTML.Append("</table>");

            return tableHTML.ToString();
        }

        //Render the Psych Cert light-box
        public static string GetPsychCertLightBox(User currentUser)
        {
            string ddConclusionHtml = "";

            List<MilitaryMedicalConclusion> militaryMedicalConclusions = MilitaryMedicalConclusionUtil.GetAllMilitaryMedicalConclusions(currentUser);

            List<IDropDownItem> militaryMedicalConclusionsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryMedicalConclusion militaryMedicalConclusion in militaryMedicalConclusions)
                militaryMedicalConclusionsDropDownItems.Add(militaryMedicalConclusion as IDropDownItem);

            ddConclusionHtml = ListItems.GetDropDownHtml(militaryMedicalConclusionsDropDownItems, null, "ddPsychCertConclusion", true, null, null, @"style=""width: 120px;""");

            string html = @"
<div id=""divPsychCertLightBox"" style=""display: none;"" class=""lboxPsychCert"">
<center>
    <input type=""hidden"" id=""hdnPsychCertID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditPsychCertTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertDate"" class=""InputLabel"">Комисия от дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanPsychCertDate"">
                    <input type=""text"" id=""txtPsychCertDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertProtNum"" class=""InputLabel"">Протокол:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPsychCertProtNum"" class='InputField' />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertConclusion"" class=""InputLabel"">Решение:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddConclusionHtml + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertExpirationDate"" class=""InputLabel"">Дата на валидност:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanPsychCertExpirationDate"">
                    <input type=""text"" id=""txtPsychCertExpirationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>

        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditPsychCertLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditPsychCertLightBox"" style=""display: inline;"" onclick=""SaveAddEditPsychCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditPsychCertLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditPsychCertLightBox"" style=""display: inline;"" onclick=""HideAddEditPsychCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditPsychCertLightBoxText"" style=""width: 70px;"">
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
</div>";

            return html;
        }



        public static string GetTabUIItems(int pReservistID, User pCurrentUser, AddEditReservist page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string UIItemsXML = "";

            bool screenDisabled = false;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            screenDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                               page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                               page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Disabled || isPreview;

            if (screenDisabled)
            {
                hiddenClientControls.Add("btnSaveMilitaryReportTab");
            }

            UIAccessLevel l;

            Reservist reservist = ReservistUtil.GetReservist(pReservistID, pCurrentUser);
            List<VoluntaryReserveAnnex> annexes = reservist.CurrResMilRepStatus != null ? VoluntaryReserveAnnexUtil.GetVoluntaryReserveAnnexesByReservistMilRepStatusId(reservist.CurrResMilRepStatus.ReservistMilRepStatusId, pCurrentUser) : new List<VoluntaryReserveAnnex>();
            //section PersonAdmClAccessAndMilRepSpec
           
            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_TRANSFERTOVITOSHA");

            if (reservist.Person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_TransferToVitosha") ||
                (l == UIAccessLevel.Hidden || l == UIAccessLevel.Disabled))
            {
                hiddenClientControls.Add("btnTransferToVitosha");
            }
   
            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_ADMINISTRATION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblAdmClAccessAndMilRepSpecSectionAdministration");
                disabledClientControls.Add("ddAdmClAccessAndMilRepSpecSectionAdministration");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAdmClAccessAndMilRepSpecSectionAdministration");
                hiddenClientControls.Add("ddAdmClAccessAndMilRepSpecSectionAdministration");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_CLINFORMATION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg");
                disabledClientControls.Add("ddAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg");
                hiddenClientControls.Add("ddAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_CLINFORMATIONEXPDATE");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate");
                disabledClientControls.Add("txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate");
                hiddenClientControls.Add("spanAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_ISSUITABLEFORMOBAPPOITMENT");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("suitableForMobAppointmentLabel");
                disabledClientControls.Add("suitableForMobAppointmentCheckBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("suitableForMobAppointmentLabel");
                hiddenClientControls.Add("suitableForMobAppointmentCheckBox");
            }

            // section Military Report Status

            bool milReportStatusSectionDisabled = screenDisabled || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS") == UIAccessLevel.Disabled;

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_MILREPSTATUS");

            if (l != UIAccessLevel.Enabled || milReportStatusSectionDisabled)
            {
                hiddenClientControls.Add("btnAddNewResMilRepStatus");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_HISTORY");

            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnHistoryStatuses");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS");

            if (l != UIAccessLevel.Enabled || milReportStatusSectionDisabled)
            {
                hiddenClientControls.Add("btnEditCurrResMilRepStatus");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_ENROLDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblCurrEnrolDateLightBox");
                disabledClientControls.Add("txtCurrEnrolDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblFirstEnrolDate");
                hiddenClientControls.Add("lblFirstEnrolDateValue");
                hiddenClientControls.Add("lblCurrEnrolDate");
                hiddenClientControls.Add("lblCurrEnrolDateValue");
                hiddenClientControls.Add("lblCurrEnrolDateLightBox");
                hiddenClientControls.Add("spanCurrEnrolDateLightBox");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_SOURCEMILDEPT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblCurrSourceMilDepartmentNameLightBox");
                disabledClientControls.Add("ddCurrSourceMilDepartmentNameLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCurrSourceMilDepartmentNameLightBox");
                hiddenClientControls.Add("ddCurrSourceMilDepartmentNameLightBox");
                hiddenClientControls.Add("lblCurrSourceMilDepartmentName");
                hiddenClientControls.Add("lblCurrSourceMilDepartmentNameValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYCONTRACTNUMBER");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryContractNumberLightBox");
                disabledClientControls.Add("txtVoluntaryContractNumberLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryContractNumberLightBox");
                hiddenClientControls.Add("txtVoluntaryContractNumberLightBox");
                hiddenClientControls.Add("lblVoluntaryContractNumber");
                hiddenClientControls.Add("txtVoluntaryContractNumber");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYCONTRACTDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryContractDateLightBox");
                disabledClientControls.Add("txtVoluntaryContractDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryContractDateLightBox");
                hiddenClientControls.Add("spanVoluntaryContractDateLightBox");
                hiddenClientControls.Add("lblVoluntaryContractDate");
                hiddenClientControls.Add("txtVoluntaryContractDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYEXPIREDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryExpireDateLightBox");
                disabledClientControls.Add("txtVoluntaryExpireDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryExpireDateLightBox");
                hiddenClientControls.Add("spanVoluntaryExpireDateLightBox");
                hiddenClientControls.Add("lblVoluntaryExpireDate");
                hiddenClientControls.Add("txtVoluntaryExpireDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYDURATIONMONTHS");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryDurationMonthsLightBox");
                disabledClientControls.Add("txtVoluntaryDurationMonthsLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryDurationMonthsLightBox");
                hiddenClientControls.Add("txtVoluntaryDurationMonthsLightBox");
                hiddenClientControls.Add("lblVoluntaryDurationMonths");
                hiddenClientControls.Add("txtVoluntaryDurationMonths");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYRESERVEANNEXES");
            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                for (int i = 1; i <= annexes.Count; i++)
                {
                    disabledClientControls.Add("lblAnnexNumberLightBox" + i);
                    disabledClientControls.Add("lblAnnexDateLightBox" + i);
                    disabledClientControls.Add("lblAnnexDurationMonthsLightBox" + i);
                    disabledClientControls.Add("lblAnnexExpireDateLightBox" + i);
                    disabledClientControls.Add("txtAnnexNumberLightBox" + i);
                    disabledClientControls.Add("txtAnnexDateLightBox" + i);
                    disabledClientControls.Add("txtAnnexDurationMonthsLightBox" + i);
                    disabledClientControls.Add("txtAnnexExpireDateLightBox" + i);

                    hiddenClientControls.Add("btnAddAnnex" + i);
                    hiddenClientControls.Add("btnRemoveAnnex" + i);
                }
                hiddenClientControls.Add("btnAddAnnex");
            }
            if (l == UIAccessLevel.Hidden)
            {
                for (int i = 1; i <= annexes.Count; i++)
                {
                    hiddenClientControls.Add("rowAnnex" + i);                    
                    hiddenClientControls.Add("rowAnnexLightBox" + i);

                    hiddenClientControls.Add("btnAddAnnex" + i);
                    hiddenClientControls.Add("btnRemoveAnnex" + i);
                }
                hiddenClientControls.Add("btnAddAnnex");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYFULFILPLACE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryFulfilPlaceLightBox");            
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryFulfilPlaceLightBox");              
                hiddenClientControls.Add("spanVoluntaryFulfilPlaceLightBox");

                hiddenClientControls.Add("lblVoluntaryFulfilPlace");
                hiddenClientControls.Add("txtVoluntaryFulfilPlace");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYMILRANK");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryMilitaryRankLightBox");
                disabledClientControls.Add("ddVoluntaryMilitaryRankLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryMilitaryRankLightBox");
                hiddenClientControls.Add("ddVoluntaryMilitaryRankLightBox");
                hiddenClientControls.Add("lblVoluntaryMilitaryRank");
                hiddenClientControls.Add("txtVoluntaryMilitaryRank");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYMILPOSITION");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryMilitaryPositionLightBox");
                disabledClientControls.Add("txtVoluntaryMilitaryPositionLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryMilitaryPositionLightBox");
                hiddenClientControls.Add("txtVoluntaryMilitaryPositionLightBox");
                hiddenClientControls.Add("lblVoluntaryMilitaryPosition");
                hiddenClientControls.Add("txtVoluntaryMilitaryPosition");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYMILREPSPEC");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryMilRepSpecTypeLightBox");
                disabledClientControls.Add("ddVoluntaryMilRepSpecTypeLightBox");
                disabledClientControls.Add("lblVoluntaryMilRepSpecLightBox");
                disabledClientControls.Add("ddVoluntaryMilRepSpecLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryMilRepSpecTypeLightBox");
                hiddenClientControls.Add("ddVoluntaryMilRepSpecTypeLightBox");
                hiddenClientControls.Add("lblVoluntaryMilRepSpecLightBox");
                hiddenClientControls.Add("ddVoluntaryMilRepSpecLightBox");
                hiddenClientControls.Add("lblVoluntaryMilRepSpec");
                hiddenClientControls.Add("txtVoluntaryMilRepSpec");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedDateLightBox");
                disabledClientControls.Add("txtRemovedDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedDateLightBox");
                hiddenClientControls.Add("spanRemovedDateLightBox");
                hiddenClientControls.Add("lblRemovedDate");
                hiddenClientControls.Add("txtRemovedDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDREASON");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedReasonLightBox");
                disabledClientControls.Add("ddRemovedReasonLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedReasonLightBox");
                hiddenClientControls.Add("ddRemovedReasonLightBox");
                hiddenClientControls.Add("lblRemovedReason");
                hiddenClientControls.Add("txtRemovedReason");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDDECEASEDDEATHCERT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedDeceasedDeathCertLightBox");
                disabledClientControls.Add("txtRemovedDeceasedDeathCertLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedDeceasedDeathCertLightBox");
                hiddenClientControls.Add("txtRemovedDeceasedDeathCertLightBox");
                hiddenClientControls.Add("lblRemovedDeceasedDeathCert");
                hiddenClientControls.Add("txtRemovedDeceasedDeathCert");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDDECEASEDDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedDeceasedDateLightBox");
                disabledClientControls.Add("txtRemovedDeceasedDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedDeceasedDateLightBox");
                hiddenClientControls.Add("spanRemovedDeceasedDateLightBox");
                hiddenClientControls.Add("lblRemovedDeceasedDate");
                hiddenClientControls.Add("txtRemovedDeceasedDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDAGELIMITORDER");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedAgeLimitOrderLightBox");
                disabledClientControls.Add("txtRemovedAgeLimitOrderLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedAgeLimitOrderLightBox");
                hiddenClientControls.Add("txtRemovedAgeLimitOrderLightBox");
                hiddenClientControls.Add("lblRemovedAgeLimitOrder");
                hiddenClientControls.Add("txtRemovedAgeLimitOrder");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDAGELIMITDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedAgeLimitDateLightBox");
                disabledClientControls.Add("txtRemovedAgeLimitDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedAgeLimitDateLightBox");
                hiddenClientControls.Add("spanRemovedAgeLimitDateLightBox");
                hiddenClientControls.Add("lblRemovedAgeLimitDate");
                hiddenClientControls.Add("txtRemovedAgeLimitDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDAGELIMITSIGNEDBY");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedAgeLimitSignedByLightBox");
                disabledClientControls.Add("txtRemovedAgeLimitSignedByLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedAgeLimitSignedByLightBox");
                hiddenClientControls.Add("txtRemovedAgeLimitSignedByLightBox");
                hiddenClientControls.Add("lblRemovedAgeLimitSignedBy");
                hiddenClientControls.Add("txtRemovedAgeLimitSignedBy");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDNOTSUITABLECERT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedNotSuitableCertLightBox");
                disabledClientControls.Add("txtRemovedNotSuitableCertLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedNotSuitableCertLightBox");
                hiddenClientControls.Add("txtRemovedNotSuitableCertLightBox");
                hiddenClientControls.Add("lblRemovedNotSuitableCert");
                hiddenClientControls.Add("txtRemovedNotSuitableCert");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDNOTSUITABLEDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedNotSuitableDateLightBox");
                disabledClientControls.Add("txtRemovedNotSuitableDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedNotSuitableDateLightBox");
                hiddenClientControls.Add("spanRemovedNotSuitableDateLightBox");
                hiddenClientControls.Add("lblRemovedNotSuitableDate");
                hiddenClientControls.Add("txtRemovedNotSuitableDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDNOTSUITABLESIGNEDBY");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedNotSuitableSignedByLightBox");
                disabledClientControls.Add("txtRemovedNotSuitableSignedByLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedNotSuitableSignedByLightBox");
                hiddenClientControls.Add("txtRemovedNotSuitableSignedByLightBox");
                hiddenClientControls.Add("lblRemovedNotSuitableSignedBy");
                hiddenClientControls.Add("txtRemovedNotSuitableSignedBy");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_MILEMPLADMINISTRATION");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblMilEmplAdministrationLightBox");
                disabledClientControls.Add("ddMilEmplAdministrationLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilEmplAdministrationLightBox");
                hiddenClientControls.Add("ddMilEmplAdministrationLightBox");
                hiddenClientControls.Add("lblMilEmplAdministration");
                hiddenClientControls.Add("txtMilEmplAdministration");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_MILEMPLDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblMilEmplDateLightBox");
                disabledClientControls.Add("txtMilEmplDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilEmplDateLightBox");
                hiddenClientControls.Add("spanMilEmplDateLightBox");
                hiddenClientControls.Add("lblMilEmplDate");
                hiddenClientControls.Add("txtMilEmplDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDREASON");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedReasonLightBox");
                disabledClientControls.Add("ddTemporaryRemovedReasonsLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedReasonLightBox");
                hiddenClientControls.Add("ddTemporaryRemovedReasonsLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedReason");
                hiddenClientControls.Add("txtTemporaryRemovedReason");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedDateLightBox");
                disabledClientControls.Add("txtTemporaryRemovedDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedDateLightBox");
                hiddenClientControls.Add("spanTemporaryRemovedDateLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedDate");
                hiddenClientControls.Add("txtTemporaryRemovedDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDDURATION");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedDurationLightBox");
                disabledClientControls.Add("txtTemporaryRemovedDurationLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedDurationLightBox");
                hiddenClientControls.Add("txtTemporaryRemovedDurationLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedDuration");
                hiddenClientControls.Add("txtTemporaryRemovedDuration");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_POSTPONETYPE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblPostponeTypeLightBox");
                disabledClientControls.Add("ddPostponeTypeLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPostponeTypeLightBox");
                hiddenClientControls.Add("ddPostponeTypeLightBox");
                hiddenClientControls.Add("lblPostponeType");
                hiddenClientControls.Add("txtPostponeType");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_POSTPONEYEAR");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblPostponeYearLightBox");
                disabledClientControls.Add("txtPostponeYearLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPostponeYearLightBox");
                hiddenClientControls.Add("txtPostponeYearLightBox");
                hiddenClientControls.Add("lblPostponeYear");
                hiddenClientControls.Add("txtPostponeYear");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_POSTPONEWORKCOMPANYANDNKPD");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                //It is always read-only
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPostponeWorkCompany");
                hiddenClientControls.Add("lblPostponeWorkCompanyValue");
                hiddenClientControls.Add("lblPostponeWorkPositionNKPD");
                hiddenClientControls.Add("lblPostponeWorkPositionNKPDValue");

                hiddenClientControls.Add("lblPostponeWorkCompanyLightBox");
                hiddenClientControls.Add("lblPostponeWorkCompanyValueLightBox");
                hiddenClientControls.Add("lblPostponeWorkPositionNKPDLightBox");
                hiddenClientControls.Add("lblPostponeWorkPositionNKPDValueLightBox");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_DESTMILDEPT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblDestMilDepartmentLightBox");
                disabledClientControls.Add("ddDestMilDepartmentLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDestMilDepartmentLightBox");
                hiddenClientControls.Add("ddDestMilDepartmentLightBox");
                hiddenClientControls.Add("lblDestMilDepartment");
                hiddenClientControls.Add("txtDestMilDepartment");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS_EDITMILREPSTATUS_DISCHARGEDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblDischargeDateLightBox");
                disabledClientControls.Add("txtDischargeDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDischargeDateLightBox");
                hiddenClientControls.Add("spanDischargeDateLightBox");
                hiddenClientControls.Add("lblDischargeDate");
                hiddenClientControls.Add("txtDischargeDate");
            }

            // section reservist appointment

            bool reservistAppointmentSectionDisabled = screenDisabled  || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT") == UIAccessLevel.Disabled;

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_HISTORY");

            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnHistoryAppointments");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_REQORDERNUMBER");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblReqOrderNumber");
                disabledClientControls.Add("lblReqOrderNumberValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReqOrderNumber");
                hiddenClientControls.Add("lblReqOrderNumberValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_RECEIPTAPPOINTMENTDATE");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblReceiptAppointmentDate");
                disabledClientControls.Add("lblReceiptAppointmentDateValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReceiptAppointmentDate");
                hiddenClientControls.Add("lblReceiptAppointmentDateValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYCOMMAND");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblMilitaryCommandName");
                disabledClientControls.Add("lblMilitaryCommandNameValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryCommandName");
                hiddenClientControls.Add("lblMilitaryCommandNameValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYCOMMANDSUFFIX");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblMilitaryCommandSuffix");
                disabledClientControls.Add("lblMilitaryCommandSuffixValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryCommandSuffix");
                hiddenClientControls.Add("lblMilitaryCommandSuffixValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_READINESS");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblReservistReadiness");
                disabledClientControls.Add("lblReservistReadinessValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReservistReadiness");
                hiddenClientControls.Add("lblReservistReadinessValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILITARYRANK");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentMilitaryRank");
                disabledClientControls.Add("lblAppointmentMilitaryRankValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentMilitaryRank");
                hiddenClientControls.Add("lblAppointmentMilitaryRankValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_POSITION");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentPosition");
                disabledClientControls.Add("lblAppointmentPositionValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentPosition");
                hiddenClientControls.Add("lblAppointmentPositionValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_MILREPSPEC");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblMilRepSpec");
                disabledClientControls.Add("lblMilRepSpecValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilRepSpec");
                hiddenClientControls.Add("lblMilRepSpecValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_NEEDCOURSE");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblNeedCourse");
                disabledClientControls.Add("cbNeedCourse");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblNeedCourse");
                hiddenClientControls.Add("cbNeedCourse");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_APPOINTMENTTIME");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentTime");
                disabledClientControls.Add("lblAppointmentTimeValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentTime");
                hiddenClientControls.Add("lblAppointmentTimeValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_APPOINTMENTPLACE");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentPlace");
                disabledClientControls.Add("lblAppointmentPlaceValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentPlace");
                hiddenClientControls.Add("lblAppointmentPlaceValue");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_APPOINTMENT_APPOINTMENTISDELIVERED");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentIsDelivered");
                disabledClientControls.Add("cbAppointmentIsDelivered");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentIsDelivered");
                hiddenClientControls.Add("cbAppointmentIsDelivered");
            }

            //section GroupManagementSection
            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_GROUPMANAGEMENTSECTION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSGroupManagementSection");
                disabledClientControls.Add("txtGMSGroupManagementSection");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSGroupManagementSection");
                hiddenClientControls.Add("txtGMSGroupManagementSection");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_SECTION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSSection");
                disabledClientControls.Add("txtGMSSection");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSSection");
                hiddenClientControls.Add("txtGMSSection");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_DELIVERER");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSDeliverer");
                disabledClientControls.Add("txtGMSDeliverer");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSDeliverer");
                hiddenClientControls.Add("txtGMSDeliverer");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PUNKT");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSPunkt");
                disabledClientControls.Add("ddGMSPunkt");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSPunkt");
                hiddenClientControls.Add("ddGMSPunkt");
            }

            //section medical certificate

            bool medCertSectionDisabled = screenDisabled || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT") == UIAccessLevel.Disabled;
            
            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_DATE");

            if (l == UIAccessLevel.Disabled || medCertSectionDisabled)
            {
                disabledClientControls.Add("lblMedCertDate");
                disabledClientControls.Add("txtMedCertDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMedCertDate");
                hiddenClientControls.Add("spanMedCertDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_PROTNUM");

            if (l == UIAccessLevel.Disabled || medCertSectionDisabled)
            {
                disabledClientControls.Add("lblMedCertProtNum");
                disabledClientControls.Add("txtMedCertProtNum");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMedCertProtNum");
                hiddenClientControls.Add("txtMedCertProtNum");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_CONCLUSION");

            if (l == UIAccessLevel.Disabled || medCertSectionDisabled)
            {
                disabledClientControls.Add("lblMedCertConclusion");
                disabledClientControls.Add("ddMedCertConclusion");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMedCertConclusion");
                hiddenClientControls.Add("ddMedCertConclusion");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_MEDRUBRIC");

            if (l == UIAccessLevel.Disabled || medCertSectionDisabled)
            {
                disabledClientControls.Add("lblMedCertMedRubric");
                disabledClientControls.Add("ddMedCertMedRubric");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMedCertMedRubric");
                hiddenClientControls.Add("ddMedCertMedRubric");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MEDCERT_EXPIRATIONDATE");

            if (l == UIAccessLevel.Disabled || medCertSectionDisabled)
            {
                disabledClientControls.Add("lblMedCertExpirationDate");
                disabledClientControls.Add("txtMedCertExpirationDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMedCertExpirationDate");
                hiddenClientControls.Add("spanMedCertExpirationDate");
            }

            //section psychological certificate

            bool psychCertSectionDisabled = screenDisabled || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT") == UIAccessLevel.Disabled;
         
            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_DATE");

            if (l == UIAccessLevel.Disabled || psychCertSectionDisabled)
            {
                disabledClientControls.Add("lblPsychCertDate");
                disabledClientControls.Add("txtPsychCertDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPsychCertDate");
                hiddenClientControls.Add("spanPsychCertDate");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_PROTNUM");

            if (l == UIAccessLevel.Disabled || psychCertSectionDisabled)
            {
                disabledClientControls.Add("lblPsychCertProtNum");
                disabledClientControls.Add("txtPsychCertProtNum");
            }
            if (l == UIAccessLevel.Hidden )
            {
                hiddenClientControls.Add("lblPsychCertProtNum");
                hiddenClientControls.Add("txtPsychCertProtNum");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_CONCLUSION");

            if (l == UIAccessLevel.Disabled || psychCertSectionDisabled)
            {
                disabledClientControls.Add("lblPsychCertConclusion");
                disabledClientControls.Add("ddPsychCertConclusion");
            }
            if (l == UIAccessLevel.Hidden )
            {
                hiddenClientControls.Add("lblPsychCertConclusion");
                hiddenClientControls.Add("ddPsychCertConclusion");
            }

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_PSYCHCERT_EXPIRATIONDATE");

            if (l == UIAccessLevel.Disabled || psychCertSectionDisabled)
            {
                disabledClientControls.Add("lblPsychCertExpirationDate");
                disabledClientControls.Add("txtPsychCertExpirationDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPsychCertExpirationDate");
                hiddenClientControls.Add("spanPsychCertExpirationDate");
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

        public static string PrintMK(int reservistId, User currentUser)
        {
            List<int> reservistIDs = new List<int>();
            reservistIDs.Add(reservistId);

            string result = GeneratePrintReservistsUtil.PrintMK(reservistIDs, currentUser);
            return result;
        }

        public static string PrintPZ(int reservistId, User currentUser)
        {
            List<int> reservistIDs = new List<int>();
            reservistIDs.Add(reservistId);

            string result = GeneratePrintReservistsUtil.PrintPZ(reservistIDs, currentUser);
            return result;
        }

        public static string PrintАK(int reservistId, User currentUser)
        {
            List<int> reservistIDs = new List<int>();
            reservistIDs.Add(reservistId);

            string result = GeneratePrintReservistsUtil.PrintAK(reservistIDs, currentUser);
            return result;
        }

        public static string PrintАSK(int reservistId, User currentUser)
        {
            List<int> reservistIDs = new List<int>();
            reservistIDs.Add(reservistId);

            string result = GeneratePrintReservistsUtil.PrintASK(reservistIDs, currentUser);
            return result;
        }

        public static string PrintUO(int reservistId, User currentUser)
        {
            List<int> reservistIDs = new List<int>();
            reservistIDs.Add(reservistId);

            string result = GeneratePrintReservistsUtil.PrintUO(reservistIDs, currentUser);
            return result;
        }

        private static string GetTransferToVitoshaLightBox(Reservist pReservist, AddEditReservist pPage)
        {
            //Generate MilitaryUnitSelector
            MilitaryUnitSelector.MilitaryUnitSelector milUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            milUnitSelector.Page = pPage;
            milUnitSelector.DataSourceWebPage = "DataSource.aspx";
            milUnitSelector.DataSourceKey = "MilitaryUnit";
            milUnitSelector.ResultMaxCount = 1000;
            milUnitSelector.Style.Add("width", "90%");
            milUnitSelector.DivMainCss = "isDivMainClassRequired";
            milUnitSelector.DivListCss = "isDivListClass";
            milUnitSelector.DivFullListCss = "isDivFullListClass";
            milUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            milUnitSelector.IncludeOnlyActual = false;
            milUnitSelector.ID = "itmsTransferToVitoshaMilUnitSelector";
            milUnitSelector.Enabled = true;
                        
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            milUnitSelector.RenderControl(tw);

            string html = @"
                            <center>
                               
                                <table width=""95%"" style=""text-align: center;"">
                                    <colgroup style=""width: 30%""></colgroup>
                                    <colgroup style=""width: 70%""></colgroup>
                                   
                                    <tr>
                                        <td colspan=""2"" align=""center"">
                                            <span class=""HeaderText"" style=""text-align: center;"">Прехвърляне към Витоша</span>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan=""2"" align=""center"">
                                            <span class=""ReadOnlyValue"" style=""text-align: center;"">" + pReservist.Person.FullName + " (ЕГН: " + pReservist.Person.IdentNumber + @")</span>
                                        </td>
                                    </tr>
                                    <tr style=""height: 5px""></tr> 

                                    <tr style=""height: 15px""></tr> 
                                        
                                    <tr style=""min-height: 17px"" id=""trTransferToVitoshaMilitaryUnit"" >    
                                        <td align='right'>
                                            <span class='InputLabel' style='padding-left: 10px'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>                                                                                                
                                        </td>
                                        <td align='left'> <span>" + sw.ToString() + @" </span></td>                                       
                                    </tr>                                     
                                    <tr style=""height: 46px; padding-top: 5px;"">
                                        <td colspan=""2"">
                                            <span id=""spanTransferToVitoshaLightBoxMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan=""2"" style=""text-align: center;"">
                                            <table style=""margin: 0 auto;"">
                                                <tr>
                                                    <td>
                                                         <div id=""btnOKTransferToVitoshaLightBox"" style=""display: none;"" onclick=""RefreshTransferToVitoshaLightBox();""
                                                            class=""Button"">
                                                            <i></i>
                                                            <div id=""btnOKTransferToVitoshaLightBoxText"" style=""width: 70px;"">
                                                                OK </div>
                                                            <b></b>
                                                        </div>
                                                        <div id=""btnSaveTransferToVitoshaLightBox"" style=""display: inline;"" onclick=""SaveTransferToVitoshaLightBox();""
                                                            class=""Button"">
                                                            <i></i>
                                                            <div id=""btnSaveTransferToVitoshaLightBoxText"" style=""width: 80px;"">
                                                                Прехвърляне </div>
                                                            <b></b>
                                                        </div>
                                                        <div id=""btnCloseTransferToVitoshaLightBox"" style=""display: inline;"" onclick=""HideTransferToVitoshaLightBox();""
                                                            class=""Button"">
                                                            <i></i>
                                                            <div id=""btnCloseTransferToVitoshaLightBoxText"" style=""width: 70px;"">
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
    }
}
