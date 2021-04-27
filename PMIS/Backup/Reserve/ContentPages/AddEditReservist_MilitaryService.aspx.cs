using System;
using System.Text;
using System.Collections.Generic;
using PMIS.Common;
using PMIS.Reserve.Common;
using System.Web.UI.WebControls;
using MilitaryUnitSelector;
using System.Web.UI;
using System.IO;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist_MilitaryService : RESPage
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
            //1. ArchiveTitle

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadArchiveTitles")
            {
                JSLoadArchiveTitles();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveArchiveTitle")
            {
                JSSaveArchiveTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadArchiveTitle")
            {
                JSLoadArchiveTitle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteArchiveTitle")
            {
                JSDeleteArchiveTitle();
                return;
            }



            //2. RewardIncentiv

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRewardIncentivs")
            {
                JSLoadRewardIncentivs();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRewardIncentiv")
            {
                JSSaveRewardIncentiv();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRewardIncentiv")
            {
                JSLoadRewardIncentiv();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRewardIncentiv")
            {
                JSDeleteRewardIncentiv();
                return;
            }


            //3. Penalty

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPenaltys")
            {
                JSLoadPenalties();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePenalty")
            {
                JSSavePenalty();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPenalty")
            {
                JSLoadPenalty();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePenalty")
            {
                JSDeletePenalty();
                return;
            }


            //4. Contract

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadContracts")
            {
                JSLoadContracts();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveContract")
            {
                JSSaveContract();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadContract")
            {
                JSLoadContract();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteContract")
            {
                JSDeleteContract();
                return;
            }



            //5. PreviousPosition

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPreviousPositions")
            {
                JSLoadPreviousPositions();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePreviousPosition")
            {
                JSSavePreviousPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPreviousPosition")
            {
                JSLoadPreviousPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePreviousPosition")
            {
                JSDeletePreviousPosition();
                return;
            }

            //Additional Method to this table

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilRepSpecs")
            {
                JSLoadMilRepSpecs();
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

            //6. Conscription

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadConscriptions")
            {
                JSLoadConscriptions();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveConscription")
            {
                JSSaveConscription();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadConscription")
            {
                JSLoadConscription();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteConscription")
            {
                JSDeleteConscription();
                return;
            }



            //7. Discharge

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadDischarges")
            {
                JSLoadDischarges();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveDischarge")
            {
                JSSaveDischarge();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadDischarge")
            {
                JSLoadDischarge();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteDischarge")
            {
                JSDeleteDischarge();
                return;
            }
        }

        // Table ArchiveTitle
        //Load ArchiveTitle table and light-box (ajax call)
        private void JSLoadArchiveTitles()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetArchiveTitlesTable(reservistId);
                string lightBoxHTML = GetArchiveTitleLightBox();

                string UIItems = GetArchiveTitleUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }
                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular ArchiveTitle (ajax call)
        private void JSLoadArchiveTitle()
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
                int personArchiveTitleId = int.Parse(Request.Form["ArchiveTitleId"]);

                PersonArchiveTitle personArchiveTitle = PersonArchiveTitleUtil.GetPersonArchiveTitle(personArchiveTitleId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personArchiveTitle>
                                        <ArchiveTitleMilitaryRankId>" + AJAXTools.EncodeForXML(personArchiveTitle.MilitaryRankId.ToString()) + @"</ArchiveTitleMilitaryRankId>
                                        <ArchiveTitleVacAnn>" + AJAXTools.EncodeForXML(personArchiveTitle.VacAnn) + @"</ArchiveTitleVacAnn>
                                        <ArchiveTitleDateArchive>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personArchiveTitle.DateArchive)) + @"</ArchiveTitleDateArchive>
                                        <ArchiveTitleDateWhen>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personArchiveTitle.DateWhen)) + @"</ArchiveTitleDateWhen>
                                        <ArchiveTitleMilitaryCommanderRankCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personArchiveTitle.MilitaryCommanderRankCode) ? personArchiveTitle.MilitaryCommanderRankCode : ListItems.GetOptionChooseOne().Value) + @"</ArchiveTitleMilitaryCommanderRankCode>
                                        <ArchiveTitleDR>" + (personArchiveTitle.IsDR ? "1" : "0") + @"</ArchiveTitleDR>
                             </personArchiveTitle>";

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

        //Save a particular ArchiveTitle (ajax call)
        private void JSSaveArchiveTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personArchiveTitleId = int.Parse(Request.Form["ArchiveTitleId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string personArchiveTitleMilitaryRankId = Request.Form["ArchiveTitleMilitaryRankId"];
                string personArchiveTitleVacAnn = Request.Form["ArchiveTitleVacAnn"];
                DateTime personArchiveTitleDateArchive = CommonFunctions.ParseDate(Request.Form["ArchiveTitleDateArchive"]).Value;
                DateTime personArchiveTitleDateWhen = CommonFunctions.ParseDate(Request.Form["ArchiveTitleDateWhen"]).Value;
                string personArchiveTitleMilitaryCommanderRankCode = Request.Form["ArchiveTitleMilitaryCommanderRankCode"];
                bool personArchiveTitleDR = (Request.Form["ArchiveTitleDR"].ToString() == "1" ? true : false);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonArchiveTitle existingPersonArchiveTitle = PersonArchiveTitleUtil.GetPersonArchiveTitle(reservist.Person.IdentNumber, personArchiveTitleMilitaryRankId, personArchiveTitleDateWhen, CurrentUser);

                if (existingPersonArchiveTitle != null &&
                    existingPersonArchiveTitle.PersonArchiveTitleId != personArchiveTitleId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраното звание вече е въведено за дата \"в сила от\"</status>";
                }
                else
                {
                    PersonArchiveTitle personArchiveTitle = new PersonArchiveTitle(CurrentUser);

                    personArchiveTitle.PersonArchiveTitleId = personArchiveTitleId;
                    personArchiveTitle.MilitaryRankId = personArchiveTitleMilitaryRankId;
                    if (personArchiveTitleVacAnn != "")
                        personArchiveTitle.VacAnn = personArchiveTitleVacAnn;
                    personArchiveTitle.DateArchive = personArchiveTitleDateArchive;
                    personArchiveTitle.DateWhen = personArchiveTitleDateWhen;
                    if (personArchiveTitleMilitaryCommanderRankCode != "-1")
                        personArchiveTitle.MilitaryCommanderRankCode = personArchiveTitleMilitaryCommanderRankCode;

                    personArchiveTitle.IsDR = personArchiveTitleDR;

                    PersonArchiveTitleUtil.SavePersonArchiveTitle(personArchiveTitle, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedArchiveTitleTable = GetArchiveTitlesTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedArchiveTitleTable>" + AJAXTools.EncodeForXML(refreshedArchiveTitleTable) + @"</refreshedArchiveTitleTable>";
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

        //  Delete a particular ArchiveTitle (ajax call)
        private void JSDeleteArchiveTitle()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personArchiveTitleId = int.Parse(Request.Form["ArchiveTitleId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonArchiveTitleUtil.DeletePersonArchiveTitle(personArchiveTitleId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedArchiveTitleTable = GetArchiveTitlesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedArchiveTitleTable>" + AJAXTools.EncodeForXML(refreshedArchiveTitleTable) + @"</refreshedArchiveTitleTable>";
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

        //  Render the ArchiveTitles table
        private string GetArchiveTitlesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsArchiveMilitaryRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_TITLE") == UIAccessLevel.Hidden;
            bool IsArchiveVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_VACANN") == UIAccessLevel.Hidden;
            bool IsArchiveDateArchiveHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEARCHIVE") == UIAccessLevel.Hidden;
            bool IsArchiveDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEWHEN") == UIAccessLevel.Hidden;
            bool IsArchiveMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_COMMANDERRANK") == UIAccessLevel.Hidden;
            bool IsArchiveDRHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DR") == UIAccessLevel.Hidden;

            if (IsArchiveMilitaryRankHidden &&
                IsArchiveVacAnnHidden &&
                IsArchiveDateArchiveHidden &&
                IsArchiveDateWhenHidden &&
                IsArchiveMilitaryCommanderRankHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewArchiveTitle();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsArchiveMilitaryRankHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Звание</th>" : "") + @"
  " + (!IsArchiveVacAnnHidden ? @"<th style='width: 120px; vertical-align: bottom;'>№ на заповедта</th>" : "") + @"                    
  " + (!IsArchiveDateArchiveHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Дата</th>" : "") + @"
  " + (!IsArchiveDateWhenHidden ? @"<th style='width: 120px; vertical-align: bottom;'>В сила от</th>" : "") + @"
  " + (!IsArchiveMilitaryCommanderRankHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Подписал заповедта</th>" : "") + @"
  " + (!IsArchiveDRHidden ? @"<th style='width: 50px; vertical-align: bottom;'>ДР</th>" : "") + @"

<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonArchiveTitle> listPersonArchiveTitle = PersonArchiveTitleUtil.GetAllPersonArchiveTitleByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonArchiveTitle personArchiveTitle in listPersonArchiveTitle)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personArchiveTitle.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteArchiveTitle(" + personArchiveTitle.PersonArchiveTitleId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditArchiveTitle(" + personArchiveTitle.PersonArchiveTitleId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>

        " + (!IsArchiveMilitaryRankHidden ? @"<td style='text-align: left;'>" + personArchiveTitle.MilitaryRank.LongName + @"</td>" : "") + @"
        " + (!IsArchiveVacAnnHidden ? @"<td style='text-align: left;'>" + personArchiveTitle.VacAnn + @"</td>" : "") + @"                    
        " + (!IsArchiveDateArchiveHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personArchiveTitle.DateArchive) + @"</td>" : "") + @"                    
        " + (!IsArchiveDateWhenHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personArchiveTitle.DateWhen) + @"</td>" : "") + @"                    
        " + (!IsArchiveMilitaryCommanderRankHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personArchiveTitle.MilitaryCommanderRankCode) ? personArchiveTitle.MilitaryCommanderRank.MilitaryCommanderRankName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsArchiveDRHidden ? @"<td style='text-align: left;'>" + (personArchiveTitle.IsDR ? "Да" : "Не") + @"</td>" : "") + @"                    

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the ArchiveTitles light-box
        private string GetArchiveTitleLightBox()
        {
            // Generates html for drop down list MilitaryRank
            List<MilitaryRank> listMilitaryRank = MilitaryRankUtil.GetAllMilitaryRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryRank = new List<IDropDownItem>();

            foreach (MilitaryRank militaryRank in listMilitaryRank)
            {
                ddiMilitaryRank.Add(militaryRank);
            }

            // Generates html for drop down list
            string PersonArchiveTitleMilitaryRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryRank, null, "ddPersonArchiveTitleMilitaryRank", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            // Generates html for drop down list MilitaryCommanderRank
            List<MilitaryCommanderRank> listMilitaryCommanderRank = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRank)
            {
                ddiMilitaryCommanderRank.Add(militaryCommanderRank);
            }

            // Generates html for drop down list
            string PersonArchiveTitleMilitaryCommanderRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonArchiveTitleMilitaryCommanderRank", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);

            //bool IsArchiveMilitaryRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_TITLE") == UIAccessLevel.Hidden;
            //bool IsArchiveVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_VACANN") == UIAccessLevel.Hidden;
            //bool IsArchiveDateArchiveHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEARCHIVE") == UIAccessLevel.Hidden;
            //bool IsArchiveDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEWHEN") == UIAccessLevel.Hidden;
            //bool IsArchiveMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_COMMANDERRANK") == UIAccessLevel.Hidden;

            string html = @"
<center>
    <input type=""hidden"" id=""hdnArchiveTitleID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditArchiveTitleTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonArchiveTitleMilitaryRankTitle"" class=""InputLabel"">Звание:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonArchiveTitleMilitaryRanksHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonArchiveTitleVacAnn"" class=""InputLabel"">№ на заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonArchiveTitleVacAnn"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonArchiveTitleDateArchive"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonArchiveTitleDateArchive"">
                <input type=""text"" id=""txtPersonArchiveTitleDateArchive"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonArchiveTitleDateWhen"" class=""InputLabel"">В сила от:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonArchiveTitleDateWhen"">
                <input type=""text"" id=""txtPersonArchiveTitleDateWhen"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";



            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonArchiveTitleMilitaryCommanderRank"" class=""InputLabel"">Подписал заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonArchiveTitleMilitaryCommanderRanksHTML + @"
            </td>
        </tr> ";

            html += @"<tr>
                        <td style=""text-align: right;"">
                           <input type=""checkbox"" id=""chkPersonArchiveTitleDR""/> 
                        </td>
                        <td style=""text-align: left;"">                           
                           <span id=""lblPersonArchiveTitleDR"" class=""InputLabel"">ДР</span>
                        </td>
                      </tr>";

            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditArchiveTitleLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditArchiveTitleLightBox"" style=""display: inline;"" onclick=""SaveAddEditArchiveTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditArchiveTitleLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditArchiveTitleLightBox"" style=""display: inline;"" onclick=""HideAddEditArchiveTitleLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditArchiveTitleLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the ArchiveTitle table
        public string GetArchiveTitleUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            bool IsArchiveMilitaryRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_TITLE") == UIAccessLevel.Hidden;
            bool IsArchiveVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_VACANN") == UIAccessLevel.Hidden;
            bool IsArchiveDateArchiveHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEARCHIVE") == UIAccessLevel.Hidden;
            bool IsArchiveDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEWHEN") == UIAccessLevel.Hidden;
            bool IsArchiveMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_COMMANDERRANK") == UIAccessLevel.Hidden;


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_TITLE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleMilitaryRankTitle");
                disabledClientControls.Add("ddPersonArchiveTitleMilitaryRank");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleMilitaryRankTitle");
                hiddenClientControls.Add("ddPersonArchiveTitleMilitaryRank");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_VACANN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleVacAnn");
                disabledClientControls.Add("txtPersonArchiveTitleVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleVacAnn");
                hiddenClientControls.Add("txtPersonArchiveTitleVacAnn");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEARCHIVE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleDateArchive");
                disabledClientControls.Add("txtPersonArchiveTitleDateArchive");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleDateArchive");
                hiddenClientControls.Add("spanPersonArchiveTitleDateArchive");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DATEWHEN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleDateWhen");
                disabledClientControls.Add("txtPersonArchiveTitleDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleDateWhen");
                hiddenClientControls.Add("spanPersonArchiveTitleDateWhen");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_COMMANDERRANK");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleMilitaryCommanderRank");
                disabledClientControls.Add("ddPersonArchiveTitleMilitaryCommanderRank");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleMilitaryCommanderRank");
                hiddenClientControls.Add("ddPersonArchiveTitleMilitaryCommanderRank");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE_DR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonArchiveTitleDR");
                disabledClientControls.Add("chkPersonArchiveTitleDR");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonArchiveTitleDR");
                hiddenClientControls.Add("chkPersonArchiveTitleDR");
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


        // Table RewardIncentiv
        //Load RewardIncentiv table and light-box (ajax call)
        private void JSLoadRewardIncentivs()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV") == UIAccessLevel.Hidden
                                    )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetRewardIncentivsTable(reservistId);
                string lightBoxHTML = GetRewardIncentivLightBox();

                string UIItems = GetRewardIncentivUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }

                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular RewardIncentiv (ajax call)
        private void JSLoadRewardIncentiv()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV") == UIAccessLevel.Hidden
                                    )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personRewardIncentivId = int.Parse(Request.Form["RewardIncentivId"]);

                PersonRewardIncentiv personRewardIncentiv = PersonRewardIncentivUtil.GetPersonRewardIncentiv(personRewardIncentivId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personRewardIncentiv>
                                        <RewardIncentivCode>" + AJAXTools.EncodeForXML(personRewardIncentiv.RewardIncentivCode) + @"</RewardIncentivCode>
                                        <RewardIncentivNumber>" + AJAXTools.EncodeForXML(personRewardIncentiv.RewardIncentivNumber.ToString()) + @"</RewardIncentivNumber>
                                        <RewardIncentivVacAnn>" + AJAXTools.EncodeForXML(personRewardIncentiv.VacAnn) + @"</RewardIncentivVacAnn>
                                        <RewardIncentivDateWhen>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personRewardIncentiv.DateWhen)) + @"</RewardIncentivDateWhen>
                                        <RewardIncentivMilitaryCommanderRankCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personRewardIncentiv.MilitaryCommanderRankCode) ? personRewardIncentiv.MilitaryCommanderRankCode : ListItems.GetOptionChooseOne().Value) + @"</RewardIncentivMilitaryCommanderRankCode>
                             </personRewardIncentiv>";

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

        //Save a particular RewardIncentiv (ajax call)
        private void JSSaveRewardIncentiv()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personRewardIncentivId = int.Parse(Request.Form["RewardIncentivId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string personRewardIncentivCode = Request.Form["RewardIncentivCode"];
                int personRewardIncentivNumber = int.Parse(Request.Form["RewardIncentivNumber"]);
                string personRewardIncentivVacAnn = Request.Form["RewardIncentivVacAnn"];
                DateTime personRewardIncentivDateWhen = CommonFunctions.ParseDate(Request.Form["RewardIncentivDateWhen"]).Value;
                string personRewardIncentivMilitaryCommanderRankCode = Request.Form["RewardIncentivMilitaryCommanderRankCode"];


                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonRewardIncentiv existingPersonRewardIncentiv = PersonRewardIncentivUtil.GetPersonRewardIncentiv(reservist.Person.IdentNumber, personRewardIncentivNumber, personRewardIncentivDateWhen, CurrentUser);

                if (existingPersonRewardIncentiv != null &&
                    existingPersonRewardIncentiv.PersonRewardIncentivId != personRewardIncentivId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>Избраният номер вече е въведен за тази дата</status>";
                }
                else
                {
                    PersonRewardIncentiv personRewardIncentiv = new PersonRewardIncentiv(CurrentUser);

                    personRewardIncentiv.PersonRewardIncentivId = personRewardIncentivId;
                    personRewardIncentiv.RewardIncentivCode = personRewardIncentivCode;
                    personRewardIncentiv.RewardIncentivNumber = personRewardIncentivNumber;
                    if (personRewardIncentivVacAnn != "")
                        personRewardIncentiv.VacAnn = personRewardIncentivVacAnn;
                    personRewardIncentiv.DateWhen = personRewardIncentivDateWhen;
                    if (personRewardIncentivMilitaryCommanderRankCode != "-1")
                        personRewardIncentiv.MilitaryCommanderRankCode = personRewardIncentivMilitaryCommanderRankCode;


                    PersonRewardIncentivUtil.SavePersonRewardIncentiv(personRewardIncentiv, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedRewardIncentivTable = GetRewardIncentivsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedRewardIncentivTable>" + AJAXTools.EncodeForXML(refreshedRewardIncentivTable) + @"</refreshedRewardIncentivTable>";
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

        //  Delete a particular RewardIncentiv (ajax call)
        private void JSDeleteRewardIncentiv()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personRewardIncentivId = int.Parse(Request.Form["RewardIncentivId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonRewardIncentivUtil.DeletePersonRewardIncentiv(personRewardIncentivId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedRewardIncentivTable = GetRewardIncentivsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedRewardIncentivTable>" + AJAXTools.EncodeForXML(refreshedRewardIncentivTable) + @"</refreshedRewardIncentivTable>";
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

        //  Render the RewardIncentivs table
        private string GetRewardIncentivsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsRewardIncentivTitleHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_TITLE") == UIAccessLevel.Hidden;
            bool IsRewardIncentivNumberHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_NUMBER") == UIAccessLevel.Hidden;
            bool IsRewardIncentivVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_VACANN") == UIAccessLevel.Hidden;
            bool IsRewardIncentivDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_DATEWHEN") == UIAccessLevel.Hidden;
            bool IsRewardIncentivCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_COMMANDERRANK") == UIAccessLevel.Hidden;

            if (IsRewardIncentivTitleHidden &&
                IsRewardIncentivNumberHidden &&
                IsRewardIncentivVacAnnHidden &&
                IsRewardIncentivDateWhenHidden &&
                IsRewardIncentivCommanderRankHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewRewardIncentiv();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsRewardIncentivTitleHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Награда</th>" : "") + @"
  " + (!IsRewardIncentivNumberHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Номер</th>" : "") + @"                    
  " + (!IsRewardIncentivVacAnnHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Заповед</th>" : "") + @"
  " + (!IsRewardIncentivDateWhenHidden ? @"<th style='width: 120px; vertical-align: bottom;'>Дата</th>" : "") + @"
  " + (!IsRewardIncentivCommanderRankHidden ? @"<th style='width: 250px; vertical-align: bottom;'>Награден от</th>" : "") + @"
<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonRewardIncentiv> listPersonRewardIncentiv = PersonRewardIncentivUtil.GetAllPersonRewardIncentivByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonRewardIncentiv personRewardIncentiv in listPersonRewardIncentiv)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personRewardIncentiv.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteRewardIncentiv(" + personRewardIncentiv.PersonRewardIncentivId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditRewardIncentiv(" + personRewardIncentiv.PersonRewardIncentivId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>

        " + (!IsRewardIncentivTitleHidden ? @"<td style='text-align: left;'>" + personRewardIncentiv.RewardIncentiv.RewardIncentivName + @"</td>" : "") + @"
        " + (!IsRewardIncentivNumberHidden ? @"<td style='text-align: left;'>" + personRewardIncentiv.RewardIncentivNumber + @"</td>" : "") + @"                    
        " + (!IsRewardIncentivVacAnnHidden ? @"<td style='text-align: left;'>" + personRewardIncentiv.VacAnn + @"</td>" : "") + @"                    
        " + (!IsRewardIncentivDateWhenHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personRewardIncentiv.DateWhen) + @"</td>" : "") + @"                    
        " + (!IsRewardIncentivCommanderRankHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personRewardIncentiv.MilitaryCommanderRankCode) ? personRewardIncentiv.MilitaryCommanderRank.MilitaryCommanderRankName.ToString() : "") + @"</td>" : "") + @"                    

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the RewardIncentivs light-box
        private string GetRewardIncentivLightBox()
        {
            // Generates html for drop down list MilitaryRank
            List<RewardIncentiv> listRewardIncentiv = RewardIncentivUtil.GetAllRewardIncentivs(CurrentUser);
            List<IDropDownItem> ddiRewardIncentiv = new List<IDropDownItem>();

            foreach (RewardIncentiv rewardIncentiv in listRewardIncentiv)
            {
                ddiRewardIncentiv.Add(rewardIncentiv);
            }

            // Generates html for drop down list
            string PersonRewardIncentivRewardIncentivsHTML = ListItems.GetDropDownHtml(ddiRewardIncentiv, null, "ddPersonRewardIncentivRewardIncentiv", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            // Generates html for drop down list MilitaryCommanderRank
            List<MilitaryCommanderRank> listMilitaryCommanderRank = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRank)
            {
                ddiMilitaryCommanderRank.Add(militaryCommanderRank);
            }

            // Generates html for drop down list
            string PersonRewardIncentivMilitaryCommanderRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonRewardIncentivMilitaryCommanderRank", true, null, "", "style='width: 200px;' UnsavedCheckSkipMe='true'", true);

            //bool IsRewardIncentivTitleHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_TITLE") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivNumberHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_NUMBER") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_VACANN") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_DATEWHEN") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_COMMANDERRANK") == UIAccessLevel.Hidden;

            string html = @"
<center>
    <input type=""hidden"" id=""hdnRewardIncentivID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditRewardIncentivTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonRewardIncentivMilitaryRankTitle"" class=""InputLabel"">Награда:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonRewardIncentivRewardIncentivsHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonRewardIncentivNumber"" class=""InputLabel"">Номер:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonRewardIncentivNumber"" UnsavedCheckSkipMe='true' maxlength=""1"" class=""RequiredInputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonRewardIncentivVacAnn"" class=""InputLabel"">Заповед:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonRewardIncentivVacAnn"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonRewardIncentivDateWhen"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;""><span id=""spanPersonRewardIncentivDateWhen"">
                <input type=""text"" id=""txtPersonRewardIncentivDateWhen"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /></span>
            </td>
        </tr> ";



            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonRewardIncentivMilitaryCommanderRank"" class=""InputLabel"">Награден от:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonRewardIncentivMilitaryCommanderRanksHTML + @"
            </td>
        </tr> ";

            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditRewardIncentivLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditRewardIncentivLightBox"" style=""display: inline;"" onclick=""SaveAddEditRewardIncentivLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditRewardIncentivLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditRewardIncentivLightBox"" style=""display: inline;"" onclick=""HideAddEditRewardIncentivLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditRewardIncentivLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the RewardIncentiv table
        public string GetRewardIncentivUIItems()
        {
            string UIItemsXML = "";

            //bool IsRewardIncentivTitleHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_TITLE") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivNumberHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_NUMBER") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_VACANN") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_DATEWHEN") == UIAccessLevel.Hidden;
            //bool IsRewardIncentivCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_COMMANDERRANK") == UIAccessLevel.Hidden;

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_TITLE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonRewardIncentivMilitaryRankTitle");
                disabledClientControls.Add("ddPersonRewardIncentivRewardIncentiv");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonRewardIncentivMilitaryRankTitle");
                hiddenClientControls.Add("ddPersonRewardIncentivRewardIncentiv");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_NUMBER");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonRewardIncentivNumber");
                disabledClientControls.Add("txtPersonRewardIncentivNumber");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonRewardIncentivNumber");
                hiddenClientControls.Add("txtPersonRewardIncentivNumber");
            }
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_VACANN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonRewardIncentivVacAnn");
                disabledClientControls.Add("txtPersonRewardIncentivVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonRewardIncentivVacAnn");
                hiddenClientControls.Add("txtPersonRewardIncentivVacAnn");
            }
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_DATEWHEN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonRewardIncentivDateWhen");
                disabledClientControls.Add("txtPersonRewardIncentivDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonRewardIncentivDateWhen");
                hiddenClientControls.Add("spanPersonRewardIncentivDateWhen");
            }
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV_COMMANDERRANK");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonRewardIncentivMilitaryCommanderRank");
                disabledClientControls.Add("ddPersonRewardIncentivMilitaryCommanderRank");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonRewardIncentivMilitaryCommanderRank");
                hiddenClientControls.Add("ddPersonRewardIncentivMilitaryCommanderRank");
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

        // Table Penalty
        //Load Penalty table and light-box (ajax call)
        private void JSLoadPenalties()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetPenaltysTable(reservistId);
                string lightBoxHTML = GetPenaltyLightBox();

                string UIItems = GetPenaltyUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }

                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular Penalty (ajax call)
        private void JSLoadPenalty()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personPenaltyId = int.Parse(Request.Form["PenaltyId"]);

                PersonPenalty personPenalty = PersonPenaltyUtil.GetPersonPenalty(personPenaltyId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personPenalty>
                                        <PenaltyCode>" + AJAXTools.EncodeForXML(personPenalty.PenaltyCode) + @"</PenaltyCode>
                                        <VacAnnImposed>" + AJAXTools.EncodeForXML(personPenalty.VacAnnImposed) + @"</VacAnnImposed>
                                        <DateImposed>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personPenalty.DateImposed)) + @"</DateImposed>
                                        <MilitaryCommanderRankCodeImposed>" + AJAXTools.EncodeForXML(personPenalty.MilitaryCommanderRankCodeImposed) + @"</MilitaryCommanderRankCodeImposed>
                                        <MilitaryCommanderRankCodeCanceled>" + AJAXTools.EncodeForXML(personPenalty.MilitaryCommanderRankCodeCanceled) + @"</MilitaryCommanderRankCodeCanceled>
                                        <VacAnnCanceled>" + AJAXTools.EncodeForXML(personPenalty.VacAnnCanceled) + @"</VacAnnCanceled>
                                        <DateCanceled>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personPenalty.DateCanceled)) + @"</DateCanceled>
                             </personPenalty>";

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

        //Save a particular Penalty (ajax call)
        private void JSSavePenalty()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPenaltyId = int.Parse(Request.Form["PenaltyId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);


                string penaltyCode = Request.Form["PenaltyCode"];
                string vacAnnImposed = Request.Form["VacAnnImposed"];
                DateTime dateImposed = CommonFunctions.ParseDate(Request.Form["DateImposed"]).Value;
                string militaryCommanderRankCodeImposed = Request.Form["MilitaryCommanderRankCodeImposed"];
                string militaryCommanderRankCodeCanceled = Request.Form["MilitaryCommanderRankCodeCanceled"];
                string vacAnnCanceled = Request.Form["VacAnnCanceled"];
                string dateCanceled = Request.Form["DateCanceled"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPenalty existingPersonPenalty = PersonPenaltyUtil.GetPersonPenalty(reservist.Person.IdentNumber, dateImposed, CurrentUser);

                if (existingPersonPenalty != null &&
                    existingPersonPenalty.PersonPenaltyId != personPenaltyId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>За тази дата вече има въведено наказание</status>";
                }
                else
                {
                    PersonPenalty personPenalty = new PersonPenalty(CurrentUser);

                    personPenalty.PersonPenaltyId = personPenaltyId;
                    personPenalty.PenaltyCode = penaltyCode;
                    personPenalty.VacAnnImposed = vacAnnImposed;
                    personPenalty.DateImposed = dateImposed;
                    personPenalty.MilitaryCommanderRankCodeImposed = militaryCommanderRankCodeImposed;

                    if (militaryCommanderRankCodeCanceled != "-1")
                        personPenalty.MilitaryCommanderRankCodeCanceled = militaryCommanderRankCodeCanceled;

                    if (vacAnnCanceled != "")
                        personPenalty.VacAnnCanceled = vacAnnCanceled;

                    if (!string.IsNullOrEmpty(dateCanceled))
                        personPenalty.DateCanceled = CommonFunctions.ParseDate(dateCanceled);

                    PersonPenaltyUtil.SavePersonPenalty(personPenalty, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedPenaltyTable = GetPenaltysTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedPenaltyTable>" + AJAXTools.EncodeForXML(refreshedPenaltyTable) + @"</refreshedPenaltyTable>";
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

        //  Delete a particular Penalty (ajax call)
        private void JSDeletePenalty()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPenaltyId = int.Parse(Request.Form["PenaltyId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPenaltyUtil.DeletePersonPenalty(personPenaltyId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedPenaltyTable = GetPenaltysTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPenaltyTable>" + AJAXTools.EncodeForXML(refreshedPenaltyTable) + @"</refreshedPenaltyTable>";
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

        //  Render the Penaltys table
        private string GetPenaltysTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsPenaltyTitleHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_TITLE") == UIAccessLevel.Hidden;

            bool IsPenaltyVacAnnImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNIMPOSED") == UIAccessLevel.Hidden;
            bool IsPenaltyDateImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATEIMPOSED") == UIAccessLevel.Hidden;
            bool IsPenaltyMilitaryCommanderRankCodeImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKIMPOSED") == UIAccessLevel.Hidden;

            bool IsPenaltyMilitaryCommanderRankCodeCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKCANCELED") == UIAccessLevel.Hidden;
            bool IsPenaltyVacAnnCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNCANCELED") == UIAccessLevel.Hidden;
            bool IsPenaltyDateCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATECANCELED") == UIAccessLevel.Hidden;

            if (IsPenaltyTitleHidden &&
                IsPenaltyVacAnnImposedHidden &&
                IsPenaltyDateImposedHidden &&
                IsPenaltyMilitaryCommanderRankCodeImposedHidden &&
                IsPenaltyMilitaryCommanderRankCodeCanceledHidden &&
                IsPenaltyVacAnnCanceledHidden &&
                IsPenaltyDateCanceledHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewPenalty();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsPenaltyTitleHidden ? @"<th style='width: 200px; vertical-align: bottom;'>Наказание</th>" : "") + @"
  " + (!IsPenaltyVacAnnImposedHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Заповед</th>" : "") + @"                    
  " + (!IsPenaltyDateImposedHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Дата</th>" : "") + @"
  " + (!IsPenaltyMilitaryCommanderRankCodeImposedHidden ? @"<th style='width: 150px; vertical-align: bottom;'>Наложил наказанието</th>" : "") + @"
  " + (!IsPenaltyMilitaryCommanderRankCodeCanceledHidden ? @"<th style='width: 150px; vertical-align: bottom;'>Отменил наказанието</th>" : "") + @"
  " + (!IsPenaltyVacAnnCanceledHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Заповед</th>" : "") + @"                    
  " + (!IsPenaltyDateCanceledHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Дата</th>" : "") + @"
<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonPenalty> listPersonPenalty = PersonPenaltyUtil.GetAllPersonPenaltyByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonPenalty personPenalty in listPersonPenalty)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personPenalty.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeletePenalty(" + personPenalty.PersonPenaltyId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditPenalty(" + personPenalty.PersonPenaltyId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsPenaltyTitleHidden ? @"<td style='text-align: left;'>" + personPenalty.Penalty.PenaltyName + @"</td>" : "") + @"
        " + (!IsPenaltyVacAnnImposedHidden ? @"<td style='text-align: left;'>" + personPenalty.VacAnnImposed + @"</td>" : "") + @"                    
        " + (!IsPenaltyDateImposedHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPenalty.DateImposed) + @"</td>" : "") + @"                    
        " + (!IsPenaltyMilitaryCommanderRankCodeImposedHidden ? @"<td style='text-align: left;'>" + personPenalty.MilitaryCommanderRankImposed.MilitaryCommanderRankName + @"</td>" : "") + @"                    
        " + (!IsPenaltyMilitaryCommanderRankCodeCanceledHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personPenalty.MilitaryCommanderRankCodeCanceled) ? personPenalty.MilitaryCommanderRankCanceled.MilitaryCommanderRankName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsPenaltyVacAnnCanceledHidden ? @"<td style='text-align: left;'>" + personPenalty.VacAnnCanceled + @"</td>" : "") + @"                    
        " + (!IsPenaltyDateCanceledHidden ? @"<td style='text-align: left;'>" + ((personPenalty.DateCanceled.HasValue) ? CommonFunctions.FormatDate(personPenalty.DateCanceled.Value.ToString()) : "") + @"</td>" : "") + @"                    

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Penaltys light-box
        private string GetPenaltyLightBox()
        {
            // Generates html for drop down list MilitaryRank
            List<Penalty> listPenalties = PenaltyUtil.GetAllPenalties(CurrentUser);
            List<IDropDownItem> ddiPenalty = new List<IDropDownItem>();

            foreach (Penalty penalty in listPenalties)
            {
                ddiPenalty.Add(penalty);
            }

            // Generates html for drop down list
            string PersonPenaltyPenaltyHTML = ListItems.GetDropDownHtml(ddiPenalty, null, "ddPersonPenaltyPenalty", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            // Generates html for drop down list MilitaryCommanderRank   used in 2 Dropdowns
            List<MilitaryCommanderRank> listMilitaryCommanderRank = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRank)
            {
                ddiMilitaryCommanderRank.Add(militaryCommanderRank);
            }

            // Generates html for drop down list - IMPOSED
            string PersonPenaltyMilitaryCommanderRankCodeImposedHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonPenaltyMilitaryCommanderRankCodeImposed", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);

            // Generates html for drop down list - CANCELED
            string PersonPenaltyMilitaryCommanderRankCodeCanceledHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonPenaltyMilitaryCommanderRankCodeCanceled", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);

            //bool IsPenaltyTitleHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_TITLE") == UIAccessLevel.Hidden;

            //bool IsPenaltyVacAnnImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNIMPOSED") == UIAccessLevel.Hidden;
            //bool IsPenaltyDateImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATEIMPOSED") == UIAccessLevel.Hidden;
            //bool IsPenaltyMilitaryCommanderRankCodeImposedHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKIMPOSED") == UIAccessLevel.Hidden;

            //bool IsPenaltyMilitaryCommanderRankCodeCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKCANCELED") == UIAccessLevel.Hidden;
            //bool IsPenaltyVacAnnCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNCANCELED") == UIAccessLevel.Hidden;
            //bool IsPenaltyDateCanceledHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATECANCELED") == UIAccessLevel.Hidden;

            string html = @"
<center>
    <input type=""hidden"" id=""hdnPenaltyID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditPenaltyTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyMilitaryRankTitle"" class=""InputLabel"">Наказание:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonPenaltyPenaltyHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyVacAnnImposed"" class=""InputLabel"">Заповед:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPenaltyVacAnnImposed"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyDateImposed"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;""><span id=""spanPersonPenaltyDateImposed"">
                <input type=""text"" id=""txtPersonPenaltyDateImposed"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /></span>
            </td>
        </tr> ";



            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyMilitaryCommanderRankImposed"" class=""InputLabel"">Наложил наказанието:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonPenaltyMilitaryCommanderRankCodeImposedHTML + @"
            </td>
        </tr> ";



            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyMilitaryCommanderRankCanceled"" class=""InputLabel"">Отменил наказанието:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonPenaltyMilitaryCommanderRankCodeCanceledHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyVacAnnCanceled"" class=""InputLabel"">Заповед:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPenaltyVacAnnCanceled"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPenaltyDateCanceled"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonPenaltyDateCanceled"">
                <input type=""text"" id=""txtPersonPenaltyDateCanceled"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /></span>
            </td>
        </tr> ";

            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditPenaltyLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditPenaltyLightBox"" style=""display: inline;"" onclick=""SaveAddEditPenaltyLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditPenaltyLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditPenaltyLightBox"" style=""display: inline;"" onclick=""HideAddEditPenaltyLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditPenaltyLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the Penalty table
        public string GetPenaltyUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_TITLE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyMilitaryRankTitle");
                disabledClientControls.Add("ddPersonPenaltyPenalty");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyMilitaryRankTitle");
                hiddenClientControls.Add("ddPersonPenaltyPenalty");
            }


            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNIMPOSED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyVacAnnImposed");
                disabledClientControls.Add("txtPersonPenaltyVacAnnImposed");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyVacAnnImposed");
                hiddenClientControls.Add("txtPersonPenaltyVacAnnImposed");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATEIMPOSED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyDateImposed");
                disabledClientControls.Add("txtPersonPenaltyDateImposed");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyDateImposed");
                hiddenClientControls.Add("spanPersonPenaltyDateImposed");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKIMPOSED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyMilitaryCommanderRankImposed");
                disabledClientControls.Add("ddPersonPenaltyMilitaryCommanderRankCodeImposed");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyMilitaryCommanderRankImposed");
                hiddenClientControls.Add("ddPersonPenaltyMilitaryCommanderRankCodeImposed");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_MILITARYCOMMANDERRANKCANCELED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyMilitaryCommanderRankCanceled");
                disabledClientControls.Add("ddPersonPenaltyMilitaryCommanderRankCodeCanceled");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyMilitaryCommanderRankCanceled");
                hiddenClientControls.Add("ddPersonPenaltyMilitaryCommanderRankCodeCanceled");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_VACANNCANCELED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyVacAnnCanceled");
                disabledClientControls.Add("txtPersonPenaltyVacAnnCanceled");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyVacAnnCanceled");
                hiddenClientControls.Add("txtPersonPenaltyVacAnnCanceled");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY_DATECANCELED");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPenaltyDateCanceled");
                disabledClientControls.Add("txtPersonPenaltyDateCanceled");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPenaltyDateCanceled");
                hiddenClientControls.Add("txtPersonPenaltyDateCanceled");
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

        // Table Contract
        //Load Contract table and light-box (ajax call)
        private void JSLoadContracts()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetContractsTable(reservistId);
                string lightBoxHTML = GetContractLightBox();

                string UIItems = GetContractUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }

                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular Contract (ajax call)
        private void JSLoadContract()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personContractId = int.Parse(Request.Form["ContractId"]);

                PersonContract personContract = PersonContractUtil.GetPersonContract(personContractId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personContract>
                                        <PersonContractDocumentTypeKey>" + AJAXTools.EncodeForXML(personContract.PersonContractDocumentTypeKey) + @"</PersonContractDocumentTypeKey>
                                        <PersonContractNumber>" + AJAXTools.EncodeForXML(personContract.PersonContractNumber) + @"</PersonContractNumber>
                                        <PersonContractDateWhen>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personContract.PersonContractDateWhen)) + @"</PersonContractDateWhen>
                                        <PersonContractDatePeriod>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personContract.PersonContractDatePeriod)) + @"</PersonContractDatePeriod>
                                        <PersonContractDurationKey>" + AJAXTools.EncodeForXML(personContract.PersonContractDurationKey) + @"</PersonContractDurationKey>
                                        <PersonMilitaryServiceTo>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personContract.PersonMilitaryServiceTo)) + @"</PersonMilitaryServiceTo>
                             </personContract>";

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

        //Save a particular Contract (ajax call)
        private void JSSaveContract()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personContractId = int.Parse(Request.Form["ContractId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string personContractDocumentTypeKey = Request.Form["PersonContractDocumentTypeKey"];
                string personContractNumber = Request.Form["PersonContractNumber"];
                DateTime personContractDateWhen = CommonFunctions.ParseDate(Request.Form["PersonContractDateWhen"]).Value;
                string personContractDatePeriod = Request.Form["PersonContractDatePeriod"];

                string personContractDurationKey = Request.Form["PersonContractDurationKey"]; ;
                string personMilitaryServiceTo = Request.Form["PersonMilitaryServiceTo"]; ;

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonContract existingPersonContract = PersonContractUtil.GetPersonContract(reservist.Person.IdentNumber, personContractNumber, personContractDateWhen, CurrentUser);

                if (existingPersonContract != null &&
                    existingPersonContract.PersonContractId != personContractId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>За тази дата вече има въведен номер документ</status>";
                }
                else
                {
                    PersonContract personContract = new PersonContract(CurrentUser);

                    personContract.PersonContractId = personContractId;
                    personContract.PersonContractDocumentTypeKey = personContractDocumentTypeKey;
                    personContract.PersonContractNumber = personContractNumber;
                    personContract.PersonContractDateWhen = personContractDateWhen;

                    if (!string.IsNullOrEmpty(personContractDatePeriod))
                        personContract.PersonContractDatePeriod = CommonFunctions.ParseDate(personContractDatePeriod);

                    personContract.PersonContractDurationKey = personContractDurationKey;

                    if (!string.IsNullOrEmpty(personMilitaryServiceTo))
                        personContract.PersonMilitaryServiceTo = CommonFunctions.ParseDate(personMilitaryServiceTo);
                    
                    PersonContractUtil.SavePersonContract(personContract, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedContractTable = GetContractsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedContractTable>" + AJAXTools.EncodeForXML(refreshedContractTable) + @"</refreshedContractTable>";
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

        //  Delete a particular Contract (ajax call)
        private void JSDeleteContract()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personContractId = int.Parse(Request.Form["ContractId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonContractUtil.DeletePersonContract(personContractId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedContractTable = GetContractsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedContractTable>" + AJAXTools.EncodeForXML(refreshedContractTable) + @"</refreshedContractTable>";
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

        //  Render the Contracts table
        private string GetContractsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsContractDocumentTypeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DOCUMENTTYPE") == UIAccessLevel.Hidden;
            bool IsContractNumberHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_NUMBER") == UIAccessLevel.Hidden;
            bool IsContractDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEWHEN") == UIAccessLevel.Hidden;
            bool IsContractDatePeriodHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEPERIOD") == UIAccessLevel.Hidden;
            bool IsPersonContractDurationHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_CONTRACTDURATION") == UIAccessLevel.Hidden;
            bool IsPersonMilitaryServiceToHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_MILITARYSERVICETO") == UIAccessLevel.Hidden;
            
            if (IsContractDocumentTypeHidden &&
                IsContractNumberHidden &&
                IsContractDateWhenHidden &&
                IsContractDatePeriodHidden &&
                IsPersonContractDurationHidden &&
                IsPersonMilitaryServiceToHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewContract();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsContractDocumentTypeHidden ? @"<th style='width: 150px; vertical-align: bottom;'>Вид документ</th>" : "") + @"
  " + (!IsContractNumberHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Номер</th>" : "") + @"                    
  " + (!IsContractDateWhenHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата</th>" : "") + @"
  " + (!IsContractDatePeriodHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Изтича на</th>" : "") + @"

  " + (!IsPersonContractDurationHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Срок на договора</th>" : "") + @"
  " + (!IsPersonMilitaryServiceToHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Военна служба до</th>" : "") + @"
<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonContract> listPersonContract = PersonContractUtil.GetAllPersonContractByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonContract personContract in listPersonContract)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personContract.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteContract(" + personContract.PersonContractId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditContract(" + personContract.PersonContractId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsContractDocumentTypeHidden ? @"<td style='text-align: left;'>" + personContract.PersonContractDocumentType.DocumentTypeName + @"</td>" : "") + @"
        " + (!IsContractNumberHidden ? @"<td style='text-align: left;'>" + personContract.PersonContractNumber + @"</td>" : "") + @"
        " + (!IsContractDateWhenHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personContract.PersonContractDateWhen) + @"</td>" : "") + @"                    
        " + (!IsContractDatePeriodHidden ? @"<td style='text-align: left;'>" + ((personContract.PersonContractDatePeriod.HasValue) ? CommonFunctions.FormatDate(personContract.PersonContractDatePeriod.Value.ToString()) : "") + @"</td>" : "") + @"                    
        " + (!IsPersonContractDurationHidden ? @"<td style='text-align: left;'>" + (personContract.PersonContractDuration != null ? personContract.PersonContractDuration.ContractDurationName : "") + @"</td>" : "") + @"
        " + (!IsPersonMilitaryServiceToHidden ? @"<td style='text-align: left;'>" + ((personContract.PersonMilitaryServiceTo.HasValue) ? CommonFunctions.FormatDate(personContract.PersonMilitaryServiceTo.Value.ToString()) : "") + @"</td>" : "") + @"                    
                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Contracts light-box
        private string GetContractLightBox()
        {
            // Generates html for drop down list DocumentType
            List<DocumentType> listDocumentTypes = DocumentTypeUtil.GetAllDocumentType(CurrentUser);
            List<IDropDownItem> ddiDocumentType = new List<IDropDownItem>();

            foreach (DocumentType documentType in listDocumentTypes)
            {
                ddiDocumentType.Add(documentType);
            }

            // Generates html for drop down list
            string PersonContractDocumentTypeHTML = ListItems.GetDropDownHtml(ddiDocumentType, null, "ddPersonContractDocumentType", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);

            List<ContractDuration> listContractDurations = ContractDurationUtil.GetAllContractDuration(CurrentUser);
            List<IDropDownItem> ddiContractDuration = new List<IDropDownItem>();

            foreach (ContractDuration contractDuration in listContractDurations)
                ddiContractDuration.Add(contractDuration);

            string PersonContractDurationHTML = ListItems.GetDropDownHtml(ddiContractDuration, null, "ddPersonContractDuration", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='InputField' ", true);
            //bool IsContractDocumentTypeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DOCUMENTTYPE") == UIAccessLevel.Hidden;
            //bool IsContractNumberHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_NUMBER") == UIAccessLevel.Hidden;
            //bool IsContractDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEWHEN") == UIAccessLevel.Hidden;
            //bool IsContractDatePeriodHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEPERIOD") == UIAccessLevel.Hidden;

            string html = @"
<center>
    <input type=""hidden"" id=""hdnContractID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditContractTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonContractDocumentType"" class=""InputLabel"">Вид документ:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonContractDocumentTypeHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonContractNumber"" class=""InputLabel"">Номер:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonContractNumber"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""RequiredInputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonContractDateWhen"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonContractDateWhen""> 
                <input type=""text"" id=""txtPersonContractDateWhen"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonContractDatePeriod"" class=""InputLabel"">Изтича на:</span>
            </td>
            <td style=""text-align: left;"">   <span id=""spanPersonContractDatePeriod""> 
                <input type=""text"" id=""txtPersonContractDatePeriod"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";

            html += @"  <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonContractDuration"" class=""InputLabel"">Срок на договора:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + PersonContractDurationHTML + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonMilitaryServiceTo"" class=""InputLabel"">Военна служба до:</span>
                            </td>
                            <td style=""text-align: left;"">   <span id=""spanPersonContractMilitaryServiceTo""> 
                                <input type=""text"" id=""txtPersonMilitaryServiceTo"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
                            </td>
                        </tr>";


                  html += @" </tr> 
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditContractLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr> 
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditContractLightBox"" style=""display: inline;"" onclick=""SaveAddEditContractLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditContractLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditContractLightBox"" style=""display: inline;"" onclick=""HideAddEditContractLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditContractLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the Contract table
        public string GetContractUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DOCUMENTTYPE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonContractDocumentType");
                disabledClientControls.Add("ddPersonContractDocumentType");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonContractDocumentType");
                hiddenClientControls.Add("ddPersonContractDocumentType");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_NUMBER");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonContractNumber");
                disabledClientControls.Add("txtPersonContractNumber");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonContractNumber");
                hiddenClientControls.Add("txtPersonContractNumber");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEWHEN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonContractDateWhen");
                disabledClientControls.Add("txtPersonContractDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonContractDateWhen");
                hiddenClientControls.Add("spanPersonContractDateWhen");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_DATEPERIOD");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonContractDatePeriod");
                disabledClientControls.Add("txtPersonContractDatePeriod");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonContractDatePeriod");
                hiddenClientControls.Add("spanPersonContractDatePeriod");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_CONTRACTDURATION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonContractDuration");
                disabledClientControls.Add("ddPersonContractDuration");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonContractDuration");
                hiddenClientControls.Add("ddPersonContractDuration");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT_MILITARYSERVICETO");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonMilitaryServiceTo");
                disabledClientControls.Add("txtPersonMilitaryServiceTo");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonMilitaryServiceTo");
                hiddenClientControls.Add("spanPersonContractMilitaryServiceTo");
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

        // Table PreviousPosition
        //Load PreviousPosition table and light-box (ajax call)
        private void JSLoadPreviousPositions()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetPreviousPositionsTable(reservistId);
                string lightBoxHTML = GetPreviousPositionLightBox();

                string UIItems = GetPreviousPositionUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }

                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular PreviousPosition (ajax call)
        private void JSLoadPreviousPosition()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personPreviousPositionId = int.Parse(Request.Form["PreviousPositionId"]);

                PersonPreviousPosition personPreviousPosition = PersonPreviousPositionUtil.GetPersonPreviousPosition(personPreviousPositionId, CurrentUser);

                //Bind MilitaryReportSpeciality obect to get MilitaryReportSpecialityType
                //int personPreviousPositionMilReportingSpecialityCode = 0;
                int personPreviousPositionMilReportingSpecialityTypeCode = 0;

                if (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode))
                {
                    //int.TryParse(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode, out personPreviousPositionMilReportingSpecialityCode);
                    //if (personPreviousPositionMilReportingSpecialityCode > 0)
                    //{
                    //    MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(personPreviousPositionMilReportingSpecialityCode, CurrentUser);
                    //    personPreviousPositionMilReportingSpecialityTypeCode = militaryReportSpeciality.MilReportSpecialityTypeId;
                    //}

                    MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialityByCode(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode, CurrentUser);
                    if (militaryReportSpeciality != null)
                        personPreviousPositionMilReportingSpecialityTypeCode = militaryReportSpeciality.MilReportSpecialityTypeId;
                }

                //Bind city object to get RegionId and MunicipalityId
                City city = CityUtil.GetCity(personPreviousPosition.PersonPreviousPositionGarrisonCityId, CurrentUser);


                //Bind MilitaryUnitId
                personPreviousPosition.PersonPreviousPositionMilitaryUnitId = MilitaryUnitUtil.GetMilitaryUnitsId(personPreviousPosition.PersonPreviousPositionMilitaryUnit.VPN, personPreviousPosition.PersonPreviousPositionMilitaryUnit.ShortName, CurrentUser);

                stat = AJAXTools.OK;

                int personPreviousPositionMilReportingSpecialityId = -1;
                if (!String.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode))
                {
                    MilitaryReportSpeciality milRepSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialityByCode(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode, CurrentUser);
                    if (milRepSpeciality != null)
                        personPreviousPositionMilReportingSpecialityId = milRepSpeciality.MilReportSpecialityId;
                }

                response = @"<personPreviousPosition>
<PersonPreviousPositionCode>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionCode) + @"</PersonPreviousPositionCode>
<PersonPreviousPositionPositionName>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionPositionName) + @"</PersonPreviousPositionPositionName>
<PersonPreviousPositionTypeKey>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionTypeKey) + @"</PersonPreviousPositionTypeKey>
                                  
<PersonPreviousPositionKindKey>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionKindKey) + @"</PersonPreviousPositionKindKey>
<PersonPreviousPositionMilitaryCategoryId>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMilitaryCategoryId) + @"</PersonPreviousPositionMilitaryCategoryId>
<PersonPreviousPositionMilReportingSpecialityId>" + AJAXTools.EncodeForXML(personPreviousPositionMilReportingSpecialityId.ToString()) + @"</PersonPreviousPositionMilReportingSpecialityId>

<PersonPreviousPositionKindKey>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionKindKey) + @"</PersonPreviousPositionKindKey>
<PersonPreviousPositionMilitaryCategoryId>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMilitaryCategoryId) + @"</PersonPreviousPositionMilitaryCategoryId>
  
<PersonPreviousPositionMission>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMission.ToString()) + @"</PersonPreviousPositionMission>
<PersonPreviousPositionVaccAnnNum>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionVaccAnnNum) + @"</PersonPreviousPositionVaccAnnNum>
<PersonPreviousPositionVaccAnnDateVacAnn>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn)) + @"</PersonPreviousPositionVaccAnnDateVacAnn>

<PersonPreviousPositionVaccAnnDateWhen>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen)) + @"</PersonPreviousPositionVaccAnnDateWhen>
<PersonPreviousPositionVaccAnnDateEnd>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd)) + @"</PersonPreviousPositionVaccAnnDateEnd>
<PersonPreviousPositionMilitaryCommanderRankCode>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode) + @"</PersonPreviousPositionMilitaryCommanderRankCode>
<PersonPreviousPositionMilitaryUnitId>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMilitaryUnitId.ToString()) + @"</PersonPreviousPositionMilitaryUnitId>

<PersonPreviousPositionMilitaryUnitVpnName>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection) + @"</PersonPreviousPositionMilitaryUnitVpnName>

<PersonPreviousPositionOrganisationUnit>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionOrganisationUnit) + @"</PersonPreviousPositionOrganisationUnit>
<PersonPreviousPositionGarrisonCityId>" + AJAXTools.EncodeForXML(personPreviousPosition.PersonPreviousPositionGarrisonCityId.ToString()) + @"</PersonPreviousPositionGarrisonCityId>

<PersonPreviousPositionMilReportingSpecialityTypeCode>" + AJAXTools.EncodeForXML(personPreviousPositionMilReportingSpecialityTypeCode > -1 ? personPreviousPositionMilReportingSpecialityTypeCode.ToString() : "") + @"</PersonPreviousPositionMilReportingSpecialityTypeCode>

<PersonPreviousPositionGarrisonRegionId>" + AJAXTools.EncodeForXML(city.RegionId.ToString()) + @"</PersonPreviousPositionGarrisonRegionId>
<PersonPreviousPositionGarrisonMunicipalityId>" + AJAXTools.EncodeForXML(city.MunicipalityId.ToString()) + @"</PersonPreviousPositionGarrisonMunicipalityId>


</personPreviousPosition>";

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

        // Save a particular PreviousPosition (ajax call)
        private void JSSavePreviousPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPreviousPositionId = int.Parse(Request.Form["PreviousPositionId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string PersonPreviousPositionCode = Request.Form["txtPersonPreviousPositionCode"];
                string PersonPreviousPositionPositionName = Request.Form["txtPersonPreviousPositionPositionName"];
                string PersonPreviousPositionTypeKey = Request.Form["ddPersonPreviousPositionType"];

                string PersonPreviousPositionKindKey = Request.Form["ddPersonPreviousPositionKind"];
                string PersonPreviousPositionMilitaryCategoryId = Request.Form["ddPersonPreviousPositionMilitaryCategory"];
                string PersonPreviousPositionMilReportingSpecialityId = Request.Form["ddPersonPreviousPositionMilitarySpecialities"];

                bool PersonPreviousPositionMission = Request.Form["chkboxPersonPreviousPositionMission"] == "true" ? true : false;
                string PersonPreviousPositionVaccAnnNum = Request.Form["txtPersonPreviousPositionVaccAnnNum"];
                DateTime PersonPreviousPositionVaccAnnDateVacAnn = CommonFunctions.ParseDate(Request.Form["txtPersonPreviousPositionVaccAnnDateVacAnn"]).Value;

                DateTime PersonPreviousPositionVaccAnnDateWhen = CommonFunctions.ParseDate(Request.Form["txtPersonPreviousPositionVaccAnnDateWhen"]).Value;
                DateTime? PersonPreviousPositionVaccAnnDateEnd = (!String.IsNullOrEmpty(Request.Form["txtPersonPreviousPositionVaccAnnDateEnd"]) ? CommonFunctions.ParseDate(Request.Form["txtPersonPreviousPositionVaccAnnDateEnd"]).Value : (DateTime?)null);
                string PersonPreviousPositionMilitaryCommanderRankCode = Request.Form["ddPersonPreviousPositionMilitaryCommanderRank"];

                int PersonPreviousPositionMilitaryUnitId = int.Parse(Request.Form["itmsPersonPreviousPositionMilitaryUnitId"]);

                string PersonPreviousPositionOrganisationUnit = Request.Form["txtPersonPreviousPositionOrganisationUnit"];

                int PersonPreviousPositionGarrisonCityId = int.Parse(Request.Form["PersonPreviousPositionCityId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPreviousPosition existingPersonPreviousPosition = PersonPreviousPositionUtil.GetPersonPreviousPosition(reservist.Person.IdentNumber, PersonPreviousPositionVaccAnnDateWhen, PersonPreviousPositionVaccAnnNum, CurrentUser);

                if (existingPersonPreviousPosition != null &&
                    existingPersonPreviousPosition.PersonPreviousPositionId != personPreviousPositionId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>За тази дата вече има въведен номер заповед</status>";
                }
                else
                {
                    PersonPreviousPosition personPreviousPosition = new PersonPreviousPosition(CurrentUser);

                    personPreviousPosition.PersonPreviousPositionId = personPreviousPositionId;

                    personPreviousPosition.PersonPreviousPositionCode = PersonPreviousPositionCode;
                    personPreviousPosition.PersonPreviousPositionPositionName = PersonPreviousPositionPositionName;
                    personPreviousPosition.PersonPreviousPositionTypeKey = PersonPreviousPositionTypeKey;

                    if (!string.IsNullOrEmpty(PersonPreviousPositionKindKey))
                        personPreviousPosition.PersonPreviousPositionKindKey = PersonPreviousPositionKindKey;


                    personPreviousPosition.PersonPreviousPositionMilitaryCategoryId = PersonPreviousPositionMilitaryCategoryId;

                    if (!string.IsNullOrEmpty(PersonPreviousPositionMilReportingSpecialityId) && PersonPreviousPositionMilReportingSpecialityId != "-1")
                    {
                        MilitaryReportSpeciality milRepSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(PersonPreviousPositionMilReportingSpecialityId), CurrentUser);
                        personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode = milRepSpeciality.MilReportingSpecialityCode;
                    }

                    personPreviousPosition.PersonPreviousPositionMission = PersonPreviousPositionMission;
                    personPreviousPosition.PersonPreviousPositionVaccAnnNum = PersonPreviousPositionVaccAnnNum;

                    personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn = PersonPreviousPositionVaccAnnDateVacAnn;

                    personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen = PersonPreviousPositionVaccAnnDateWhen;
                    personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd = PersonPreviousPositionVaccAnnDateEnd;

                    if (!string.IsNullOrEmpty(PersonPreviousPositionMilitaryCommanderRankCode))
                        personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode = PersonPreviousPositionMilitaryCommanderRankCode;

                    personPreviousPosition.PersonPreviousPositionMilitaryUnitId = PersonPreviousPositionMilitaryUnitId;
                    personPreviousPosition.PersonPreviousPositionOrganisationUnit = PersonPreviousPositionOrganisationUnit;

                    personPreviousPosition.PersonPreviousPositionGarrisonCityId = PersonPreviousPositionGarrisonCityId;

                    if (reservist.CurrResMilRepStatus != null)
                        personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId = reservist.CurrResMilRepStatus.SourceMilDepartmentId;

                    PersonPreviousPositionUtil.SavePersonPreviousPosition(personPreviousPosition, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedPreviousPositionTable = GetPreviousPositionsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedPreviousPositionTable>" + AJAXTools.EncodeForXML(refreshedPreviousPositionTable) + @"</refreshedPreviousPositionTable>";
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

        //  Delete a particular PreviousPosition (ajax call)
        private void JSDeletePreviousPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personPreviousPositionId = int.Parse(Request.Form["PreviousPositionId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonPreviousPositionUtil.DeletePersonPreviousPosition(personPreviousPositionId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedPreviousPositionTable = GetPreviousPositionsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPreviousPositionTable>" + AJAXTools.EncodeForXML(refreshedPreviousPositionTable) + @"</refreshedPreviousPositionTable>";
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

        //  Render the PreviousPositions table

        private string GetPreviousPositionsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsPersonPreviousPositionCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CODE") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionPositionNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_NAME") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionTypeKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_TYPEKEY") == UIAccessLevel.Hidden;

            bool IsPersonPreviousPositionKindKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_KINDKEY") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionMilitaryCategoryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CATEGORY") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionMilReportingSpecialityCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_SPECIALCODE") == UIAccessLevel.Hidden;

            bool IsPersonPreviousPositionMissionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MISSION") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionVaccAnnNumHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNNUM") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionVaccAnnDateVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATE") == UIAccessLevel.Hidden;

            bool IsPersonPreviousPositionVaccAnnDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATEWHEN") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionVaccAnnDateEndHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATEEND") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_COMMANDERRANK") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionMilitaryUnitVpnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITVPN") == UIAccessLevel.Hidden;

            bool IsPersonPreviousPositionMilitaryUnitNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITNAME") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionOrganisationUnitHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_ORGANISATIONUNIT") == UIAccessLevel.Hidden;
            bool IsPersonPreviousPositionGarrisonNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_GARRISONNAME") == UIAccessLevel.Hidden;


            if (IsPersonPreviousPositionCodeHidden &&
                IsPersonPreviousPositionPositionNameHidden &&
                IsPersonPreviousPositionTypeKeyHidden &&

                IsPersonPreviousPositionKindKeyHidden &&
                IsPersonPreviousPositionMilitaryCategoryHidden &&
                IsPersonPreviousPositionMilReportingSpecialityCodeHidden &&

                IsPersonPreviousPositionMissionHidden &&
                IsPersonPreviousPositionVaccAnnNumHidden &&
                IsPersonPreviousPositionVaccAnnDateVacAnnHidden &&

                IsPersonPreviousPositionVaccAnnDateWhenHidden &&
                IsPersonPreviousPositionVaccAnnDateEndHidden &&
                IsPersonPreviousPositionMilitaryCommanderRankHidden &&
                IsPersonPreviousPositionMilitaryUnitVpnHidden &&

                IsPersonPreviousPositionMilitaryUnitNameHidden &&
                IsPersonPreviousPositionOrganisationUnitHidden &&
                IsPersonPreviousPositionGarrisonNameHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewPreviousPosition();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsPersonPreviousPositionCodeHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Код длъжност</th>" : "") + @"
  " + (!IsPersonPreviousPositionPositionNameHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Длъжност</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionTypeKeyHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Заемал длъжността като</th>" : "") + @"

  " + (!IsPersonPreviousPositionKindKeyHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Вид</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionMilitaryCategoryHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Категория</th>" : "") + @"
  " + (!IsPersonPreviousPositionMilReportingSpecialityCodeHidden ? @"<th style='width: 80px; vertical-align: bottom;'>ВОС</th>" : "") + @"

  " + (!IsPersonPreviousPositionMissionHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Мисия</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionVaccAnnNumHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Заповед №</th>" : "") + @"
  " + (!IsPersonPreviousPositionVaccAnnDateVacAnnHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Заповед дата</th>" : "") + @"

  " + (!IsPersonPreviousPositionVaccAnnDateWhenHidden ? @"<th style='width: 80px; vertical-align: bottom;'>В сила от</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionVaccAnnDateEndHidden ? @"<th style='width: 80px; vertical-align: bottom;'>До дата</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionMilitaryCommanderRankHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Подписал заповедта</th>" : "") + @"
  " + (!IsPersonPreviousPositionMilitaryUnitVpnHidden ? @"<th style='width: 80px; vertical-align: bottom;'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @"</th>" : "") + @"

  " + (!IsPersonPreviousPositionMilitaryUnitNameHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Име на поделението</th>" : "") + @"                    
  " + (!IsPersonPreviousPositionOrganisationUnitHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Организационна единица</th>" : "") + @"
  " + (!IsPersonPreviousPositionGarrisonNameHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Гарнизон </th>" : "") + @"


<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonPreviousPosition> listPersonPreviousPosition = PersonPreviousPositionUtil.GetAllPersonPreviousPositionByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonPreviousPosition personPreviousPosition in listPersonPreviousPosition)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personPreviousPosition.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeletePreviousPosition(" + personPreviousPosition.PersonPreviousPositionId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditPreviousPosition(" + personPreviousPosition.PersonPreviousPositionId.ToString() + ");' />";


                //Add logic comes from PersonPreviousPositionMilitaryDepartmentId
                //if (personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId.HasValue)
                //{
                //    if (reservist.CurrResMilRepStatus == null)
                //    {
                //        deleteHTML = "";
                //        editHTML = "";
                //    }
                //    else
                //    {
                //        if (personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId.Value != reservist.CurrResMilRepStatus.SourceMilDepartmentId)
                //        {
                //            deleteHTML = "";
                //            editHTML = "";
                //        }
                //    }
                //}
                //else
                //{
                //    deleteHTML = "";
                //    editHTML = "";
                //}


                string cheked = "";
                if (personPreviousPosition.PersonPreviousPositionMission)
                {
                    cheked = "checked='checked'";
                }

                string milRepSpecialityCode = "";

                if (!String.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode))
                {
                    //MilitaryReportSpeciality milRepSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode), CurrentUser);
                    //milRepSpecialityCode = milRepSpeciality.MilReportingSpecialityCode;
                    milRepSpecialityCode = personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode;
                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>

" + (!IsPersonPreviousPositionCodeHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionCode + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionPositionNameHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionPositionName + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionTypeKeyHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionType.PreviousPositionTypeName + @"</td>" : "") + @"
    
" + (!IsPersonPreviousPositionKindKeyHidden ? @"<td style='text-align: left;'>" + (personPreviousPosition.PersonPreviousPositionKind != null ? personPreviousPosition.PersonPreviousPositionKind.PreviousPositionKindName : "") + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionMilitaryCategoryHidden ? @"<td style='text-align: left;'>" + (personPreviousPosition.PersonPreviousPositionMilitaryCategory != null ? personPreviousPosition.PersonPreviousPositionMilitaryCategory.CategoryName : "") + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionMilReportingSpecialityCodeHidden ? @"<td style='text-align: left;'>" + milRepSpecialityCode + @"</td>" : "") + @"

" + (!IsPersonPreviousPositionMissionHidden ? @"<td align='center'><input type='checkbox' disabled='disabled'" + cheked + @"</input></td>" : "") + @"                    
" + (!IsPersonPreviousPositionVaccAnnNumHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionVaccAnnNum + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionVaccAnnDateVacAnnHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn) + @"</td>" : "") + @"                    
 
" + (!IsPersonPreviousPositionVaccAnnDateWhenHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen) + @"</td>" : "") + @"                    
" + (!IsPersonPreviousPositionVaccAnnDateEndHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd) + @"</td>" : "") + @"                    
" + (!IsPersonPreviousPositionMilitaryCommanderRankHidden ? @"<td style='text-align: left;'>" + (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode) ? personPreviousPosition.PersonPreviousPositionMilitaryCommanderRank.MilitaryCommanderRankName : "") + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionMilitaryUnitVpnHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionMilitaryUnit.VPN + @"</td>" : "") + @"
   
" + (!IsPersonPreviousPositionMilitaryUnitNameHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionOrganisationUnitHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionOrganisationUnit + @"</td>" : "") + @"
" + (!IsPersonPreviousPositionGarrisonNameHidden ? @"<td style='text-align: left;'>" + personPreviousPosition.PersonPreviousPositionGarrisonName + @"</td>" : "") + @"



                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the PreviousPositions light-box
        private string GetPreviousPositionLightBox()
        {
            // Generates  drop down list PreviousPositionType
            List<PreviousPositionType> listPreviousPositionTypes = PreviousPositionTypeUtil.GetAllPreviousPositionType(CurrentUser);
            List<IDropDownItem> ddiPreviousPositionType = new List<IDropDownItem>();

            foreach (PreviousPositionType previousPositionType in listPreviousPositionTypes)
            {
                ddiPreviousPositionType.Add(previousPositionType);
            }

            // 1 Generates html for drop down list
            string PersonPreviousPositionTypeHTML = ListItems.GetDropDownHtml(ddiPreviousPositionType, null, "ddPersonPreviousPositionType", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);



            // Generates  drop down list PreviousPositionKind
            List<PreviousPositionKind> listPreviousPositionKinds = PreviousPositionKindUtil.GetAllPreviousPositionKind(CurrentUser);
            List<IDropDownItem> ddiPreviousPositionKind = new List<IDropDownItem>();

            foreach (PreviousPositionKind PreviousPositionKind in listPreviousPositionKinds)
            {
                ddiPreviousPositionKind.Add(PreviousPositionKind);
            }

            // 2 Generates html for drop down list
            string PersonPreviousPositionKindHTML = ListItems.GetDropDownHtml(ddiPreviousPositionKind, null, "ddPersonPreviousPositionKind", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);



            // Generates  drop down list DocumentType
            List<MilitaryCategory> listMilitaryCategorys = MilitaryCategoryUtil.GetAllMilitaryCategories(CurrentUser);
            List<IDropDownItem> ddiMilitaryCategory = new List<IDropDownItem>();

            foreach (MilitaryCategory militaryCategory in listMilitaryCategorys)
            {
                ddiMilitaryCategory.Add(militaryCategory);
            }

            // 3 Generates html for drop down list
            string PersonPreviousMilitaryCategoryHTML = ListItems.GetDropDownHtml(ddiMilitaryCategory, null, "ddPersonPreviousPositionMilitaryCategory", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);



            // Generates html for drop down list 
            List<MilitaryCommanderRank> listMilitaryCommanderRanks = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank MilitaryCommanderRank in listMilitaryCommanderRanks)
            {
                ddiMilitaryCommanderRank.Add(MilitaryCommanderRank);
            }

            // 4 Generates html for drop down list
            string PersonPreviousPositionMilitaryCommanderRankHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonPreviousPositionMilitaryCommanderRank", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);



            //VOS -  method ***
            List<MilitaryReportSpecialityType> listMilRepSpecTypes = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);
            List<IDropDownItem> ddiMilRepSpecType = new List<IDropDownItem>();

            foreach (MilitaryReportSpecialityType militaryReportSpecialityType in listMilRepSpecTypes)
            {
                ddiMilRepSpecType.Add(militaryReportSpecialityType);
            }

            // Generates html for drop down list
            string PersonPreviousPositionMilRepSpecTypeHTML = ListItems.GetDropDownHtml(ddiMilRepSpecType, null, "ddPersonPreviousPositionMilitaryReportSpecialityType", true, null, "ddPersonPreviousPositionMilitaryReportSpecialityTypeChanged();", "style='width: 180px;' UnsavedCheckSkipMe='true' ", true);


            //Region Method  ***
            List<Region> listRegions = RegionUtil.GetRegions(CurrentUser);
            List<IDropDownItem> ddiRegion = new List<IDropDownItem>();

            foreach (Region Region in listRegions)
            {
                ddiRegion.Add(Region);
            }

            // Generates html for drop down list
            string PersonPreviousPositionRegionHTML = ListItems.GetDropDownHtml(ddiRegion, null, "ddPersonPreviousPositionRegion", true, null, "ddPreviousPositionRegion_Changed()", "style='width: 130px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);
            //Municipality
            List<IDropDownItem> ddiMunicipality = new List<IDropDownItem>();
            string PersonPreviousPositionMunicipalityHTML = ListItems.GetDropDownHtml(ddiMunicipality, null, "ddPersonPreviousPositionMunicipality", true, null, "ddPreviousPositionMunicipality_Changed()", "style='width: 130px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);
            //City
            List<IDropDownItem> ddiCity = new List<IDropDownItem>();
            string PersonPreviousPositionCityHTML = ListItems.GetDropDownHtml(ddiCity, null, "ddPersonPreviousPositionCity", true, null, "", "style='width: 130px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);

            //Generate MilitaryUnitSelector
            MilitaryUnitSelector.MilitaryUnitSelector milUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            milUnitSelector.Page = this;
            milUnitSelector.DataSourceWebPage = "DataSource.aspx";
            milUnitSelector.DataSourceKey = "MilitaryUnit";
            milUnitSelector.ResultMaxCount = 1000;
            milUnitSelector.Style.Add("width", "90%");
            milUnitSelector.DivMainCss = "isDivMainClassRequired";
            milUnitSelector.DivListCss = "isDivListClass";
            milUnitSelector.DivFullListCss = "isDivFullListClass";
            milUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            milUnitSelector.IncludeOnlyActual = false;
            milUnitSelector.ID = "itmsPersonPreviousPositionMilUnitSelector";

            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITVPN") == UIAccessLevel.Disabled)
                milUnitSelector.Enabled = false;


            string militaryUnitIds = "";
            string positionName = "";

            if (Request.Params["MilitaryUnitID"] != null && Request.Params["PositionName"] != null && Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null && Request.Params["MaxPage"] != null)
            {
                if (Request.Params["MilitaryUnitID"] != ListItems.GetOptionAll().Value)
                    militaryUnitIds = Request.Params["MilitaryUnitID"];

                if (!string.IsNullOrEmpty(militaryUnitIds))
                {
                    MilitaryUnit unit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitIds), CurrentUser);
                    if (unit != null)
                    {
                        milUnitSelector.SelectedValue = militaryUnitIds;
                        milUnitSelector.SelectedText = unit.DisplayTextForSelection;
                    }
                }

                positionName = (Request.Params["PositionName"] != null ? Request.Params["PositionName"] : "");

            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            milUnitSelector.RenderControl(tw);


            //bool IsPersonPreviousPositionCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CODE") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionPositionNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_NAME") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionTypeKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_TYPEKEY") == UIAccessLevel.Hidden;

            //bool IsPersonPreviousPositionKindKeyHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_KINDKEY") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionMilitaryCategoryHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CATEGORY") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionMilReportingSpecialityCodeHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_SPECIALCODE") == UIAccessLevel.Hidden;

            //bool IsPersonPreviousPositionMissionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MISSION") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionVaccAnnNumHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNNUM") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionVaccAnnDateVacAnnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATE") == UIAccessLevel.Hidden;

            //bool IsPersonPreviousPositionVaccAnnDateWhenHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATEWHEN") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_COMMANDERRANK") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionMilitaryUnitVpnHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITVPN") == UIAccessLevel.Hidden;

            //bool IsPersonPreviousPositionMilitaryUnitNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITNAME") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionOrganisationUnitHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_ORGANISATIONUNIT") == UIAccessLevel.Hidden;
            //bool IsPersonPreviousPositionGarrisonNameHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_GARRISONNAME") == UIAccessLevel.Hidden;


            string html = @"
<center>
    <input type=""hidden"" id=""hdnPreviousPositionID"" />
    <table width=""95%"" style=""text-align: center;"">
        <colgroup style=""width: 30%"">
        </colgroup>
        <colgroup style=""width: 70%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditPreviousPositionTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionCode"" class=""InputLabel"">Код длъжност:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPreviousPositionCode"" UnsavedCheckSkipMe='true' maxlength=""13"" class=""RequiredInputField"" style=""width: 150px;"" />
            </td>
        </tr>";

            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionPositionName"" class=""InputLabel"">Длъжност:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPreviousPositionPositionName"" UnsavedCheckSkipMe='true' maxlength=""1024"" class=""RequiredInputField"" style=""width: 300px;"" />
            </td>
        </tr> ";



            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionType"" class=""InputLabel"">Заемал длъжността като:</span>
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousPositionTypeHTML + @"            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionKind"" class=""InputLabel"">Вид:</span>
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousPositionKindHTML + @"            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionCategory"" class=""InputLabel"">Категория:</span>
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousMilitaryCategoryHTML + @"            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionMilRepTypeVOS"" class=""InputLabel"">Тип ВОС:</span>
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousPositionMilRepSpecTypeHTML + @"            </td>
        </tr> 

        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionMilRepVOS"" class=""InputLabel"">ВОС:</span>
            </td>
            <td style=""text-align: left;"">
                <select id=""ddPersonPreviousPositionMilitarySpecialities"" style=""width: 250px"" UnsavedCheckSkipMe=""true""></select>
            </td>
        </tr>  ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionMission"" class=""InputLabel"">Мисия:</span>
            </td>
       <td style=""text-align: left;""><input type='checkbox' UnsavedCheckSkipMe='true' id='chkboxPersonPreviousPositionMission'></input>
            </td>
        </tr> ";

            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionVaccAnnNum"" class=""InputLabel"">Заповед номер:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPreviousPositionVaccAnnNum"" UnsavedCheckSkipMe='true' maxlength=""20"" class=""RequiredInputField"" style=""width: 170px;"" />
            </td>
        </tr> ";



            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionVaccAnnDateVacAnn"" class=""InputLabel"">Заповед дата:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonPreviousPositionVaccAnnDateVacAnn"">
                <input type=""text"" id=""txtPersonPreviousPositionVaccAnnDateVacAnn"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />  </span>
            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionVaccAnnDateWhen"" class=""InputLabel"">В сила от:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonPreviousPositionVaccAnnDateWhen"">
                <input type=""text"" id=""txtPersonPreviousPositionVaccAnnDateWhen"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />  </span>
            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionVaccAnnDateEnd"" class=""InputLabel"">До дата:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonPreviousPositionVaccAnnDateEnd"">
                <input type=""text"" id=""txtPersonPreviousPositionVaccAnnDateEnd"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />  </span>
            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionCommanderRank"" class=""InputLabel"">Подписал заповедта:</span>
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousPositionMilitaryCommanderRankHTML + @"            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">    
                    <td align='right'>
                                <span id=""lblPersonPreviousPositionMilitaryUnit"" class='InputLabel' style='padding-left: 10px'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>                                                                                                
                    </td>
                    <td align='left'> <span id=""spanPersonPreviousPositionMilitaryUnit"">

                                " + sw.ToString() + @" </span>

                   </td>
               <input type=""hidden"" id=""hdnPersonPreviousPositionMilitaryUnit"" />
               </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionOrganisationUnit"" class=""InputLabel"">Организационна единица:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonPreviousPositionOrganisationUnit"" UnsavedCheckSkipMe='true' maxlength=""25"" class=""RequiredInputField"" style=""width: 300px;"" />
            </td>
        </tr> ";


            html += @"      
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonPreviousPositionGarrison"" class=""InputLabel"">Гарнизон:</span>
            </td>
            <td style=""text-align: left;"">
 <span style='padding-left:40px;' id=""lblPersonPreviousPositionRegion"" class=""InputLabel"">Област:</span>
 <span style='padding-left:70px;' id=""lblPersonPreviousPositionRegionMunicipality"" class=""InputLabel"">Община:</span>
 <span style='padding-left:45px;' id=""lblPersonPreviousPositionCity"" class=""InputLabel"">Населено място:</span>
            </td>
        </tr> 
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
            </td>
            <td style=""text-align: left;""> "
        + PersonPreviousPositionRegionHTML + "&nbsp;" + PersonPreviousPositionMunicipalityHTML + "&nbsp;" + PersonPreviousPositionCityHTML + @" 
            </td>
       </tr> ";




            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditPreviousPositionLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditPreviousPositionLightBox"" style=""display: inline;"" onclick=""SaveAddEditPreviousPositionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditPreviousPositionLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditPreviousPositionLightBox"" style=""display: inline;"" onclick=""HideAddEditPreviousPositionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditPreviousPositionLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the PreviousPosition table
        public string GetPreviousPositionUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            //1
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CODE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionCode");
                disabledClientControls.Add("txtPersonPreviousPositionCode");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionCode");
                hiddenClientControls.Add("txtPersonPreviousPositionCode");
            }

            //2
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_NAME");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionPositionName");
                disabledClientControls.Add("txtPersonPreviousPositionPositionName");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionPositionName");
                hiddenClientControls.Add("txtPersonPreviousPositionPositionName");
            }

            //3
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_TYPEKEY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionType");
                disabledClientControls.Add("ddPersonPreviousPositionType");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionType");
                hiddenClientControls.Add("ddPersonPreviousPositionType");
            }

            //4
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_KINDKEY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionKind");
                disabledClientControls.Add("ddPersonPreviousPositionKind");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionKind");
                hiddenClientControls.Add("ddPersonPreviousPositionKind");
            }

            //5
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_CATEGORY");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionCategory");
                disabledClientControls.Add("ddPersonPreviousPositionMilitaryCategory");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionCategory");
                hiddenClientControls.Add("ddPersonPreviousPositionMilitaryCategory");
            }

            //6
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_SPECIALCODE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionMilRepTypeVOS");
                disabledClientControls.Add("lblPersonPreviousPositionMilRepVOS");
                disabledClientControls.Add("ddPersonPreviousPositionMilitaryReportSpecialityType");
                disabledClientControls.Add("ddPersonPreviousPositionMilitarySpecialities");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionMilRepTypeVOS");
                hiddenClientControls.Add("lblPersonPreviousPositionMilRepVOS");
                hiddenClientControls.Add("ddPersonPreviousPositionMilitaryReportSpecialityType");
                hiddenClientControls.Add("ddPersonPreviousPositionMilitarySpecialities");
            }

            //7
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MISSION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionMission");
                disabledClientControls.Add("chkboxPersonPreviousPositionMission");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionMission");
                hiddenClientControls.Add("chkboxPersonPreviousPositionMission");
            }

            //8
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNNUM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionVaccAnnNum");
                disabledClientControls.Add("txtPersonPreviousPositionVaccAnnNum");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionVaccAnnNum");
                hiddenClientControls.Add("txtPersonPreviousPositionVaccAnnNum");
            }

            //9
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionVaccAnnDateVacAnn");
                disabledClientControls.Add("txtPersonPreviousPositionVaccAnnDateVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionVaccAnnDateVacAnn");
                hiddenClientControls.Add("spanPersonPreviousPositionVaccAnnDateVacAnn");
            }

            //10
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATEWHEN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionVaccAnnDateWhen");
                disabledClientControls.Add("txtPersonPreviousPositionVaccAnnDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionVaccAnnDateWhen");
                hiddenClientControls.Add("spanPersonPreviousPositionVaccAnnDateWhen");
            }

            //11
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_COMMANDERRANK");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionCommanderRank");
                disabledClientControls.Add("ddPersonPreviousPositionMilitaryCommanderRank");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionCommanderRank");
                hiddenClientControls.Add("ddPersonPreviousPositionMilitaryCommanderRank");
            }

            //12
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_VACCANNDATEEND");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionVaccAnnDateEnd");
                disabledClientControls.Add("txtPersonPreviousPositionVaccAnnDateEnd");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionVaccAnnDateEnd");
                hiddenClientControls.Add("spanPersonPreviousPositionVaccAnnDateEnd");
            }

            //VPN
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_MILUNITVPN");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionMilitaryUnit");
                disabledClientControls.Add("lblPersonPreviousPositionMilitaryUnit");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionMilitaryUnit");
                hiddenClientControls.Add("spanPersonPreviousPositionMilitaryUnit");
            }


            //1
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_ORGANISATIONUNIT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonPreviousPositionOrganisationUnit");
                disabledClientControls.Add("txtPersonPreviousPositionOrganisationUnit");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionOrganisationUnit");
                hiddenClientControls.Add("txtPersonPreviousPositionOrganisationUnit");
            }

            //1
            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION_GARRISONNAME");
            if (l == UIAccessLevel.Disabled)
            {
                
                disabledClientControls.Add("lblPersonPreviousPositionGarrison");
                disabledClientControls.Add("lblPersonPreviousPositionRegion");
                disabledClientControls.Add("lblPersonPreviousPositionRegionMunicipality");
                disabledClientControls.Add("lblPersonPreviousPositionCity");

                disabledClientControls.Add("ddPersonPreviousPositionRegion");
                disabledClientControls.Add("ddPersonPreviousPositionMunicipality");
                disabledClientControls.Add("ddPersonPreviousPositionCity");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonPreviousPositionGarrison");

                hiddenClientControls.Add("lblPersonPreviousPositionRegion");
                hiddenClientControls.Add("lblPersonPreviousPositionRegionMunicipality");
                hiddenClientControls.Add("lblPersonPreviousPositionCity");

                hiddenClientControls.Add("ddPersonPreviousPositionRegion");
                hiddenClientControls.Add("ddPersonPreviousPositionMunicipality");
                hiddenClientControls.Add("ddPersonPreviousPositionCity");
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


        //Get the military report specialities for a type (ajax call)
        private void JSLoadMilRepSpecs()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int milRepSpecTypeID = -1;
                if(!String.IsNullOrEmpty(Request.Form["MilRepSpecTypeID"]))
                    milRepSpecTypeID = int.Parse(Request.Form["MilRepSpecTypeID"]);

                int selectedItem = 0;
                if (!String.IsNullOrEmpty(Request.Form["SelectedItem"]))
                    selectedItem = int.Parse(Request.Form["SelectedItem"]);

                bool selectedItemIsInList = false;

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

                    if (selectedItem > 0 && speciality.MilReportSpecialityId == selectedItem)
                        selectedItemIsInList = true;
                }

                if (selectedItem > 0 && !selectedItemIsInList)
                {
                    MilitaryReportSpeciality selectedMilRepSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(selectedItem, CurrentUser);

                    response += "<m>" +
                                "<id>" + selectedMilRepSpeciality.MilReportSpecialityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(selectedMilRepSpeciality.CodeAndName) + "</name>" +
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

        //Get the Municipalities for a particular Region (ajax call)
        private void JSRepopulateMunicipality()
        {
            //if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
            //    RedirectAjaxAccessDenied();

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


        // Table Conscription
        //Load Conscription table and light-box (ajax call)
        private void JSLoadConscriptions()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetConscriptionsTable(reservistId);
                string lightBoxHTML = GetConscriptionLightBox();

                string UIItems = GetConscriptionUIItems();

                stat = AJAXTools.OK;

                response = @"
                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                    ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }

                //                stat = AJAXTools.OK;

                //                response = @"
                //                    <tableHTML>" + AJAXTools.EncodeForXML(tableHTML) + @"</tableHTML>
                //                    <lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>
                //                    ";
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

        // Load a particular Conscription (ajax call)
        private void JSLoadConscription()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personConscriptionId = int.Parse(Request.Form["ConscriptionId"]);

                PersonConscription personConscription = PersonConscriptionUtil.GetPersonConscription(personConscriptionId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personConscription>
                                        <PersonConscriptionMilitaryUnit>" + AJAXTools.EncodeForXML(personConscription.MilitaryUnit) + @"</PersonConscriptionMilitaryUnit>
                                        <PersonConscriptionPosition>" + AJAXTools.EncodeForXML(personConscription.Position) + @"</PersonConscriptionPosition>
                                        <PersonConscriptionDateFrom>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personConscription.DateFrom)) + @"</PersonConscriptionDateFrom>
                                        <PersonConscriptionDateTo>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personConscription.DateTo)) + @"</PersonConscriptionDateTo>
                             </personConscription>";

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

        //Save a particular Conscription (ajax call)
        private void JSSaveConscription()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personConscriptionId = int.Parse(Request.Form["ConscriptionId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                string militaryUnit = Request.Form["MilitaryUnit"];
                string position = Request.Form["Position"];
                DateTime dateFrom = CommonFunctions.ParseDate(Request.Form["DateFrom"]).Value;
                DateTime dateTo = CommonFunctions.ParseDate(Request.Form["DateTo"]).Value;

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonConscription existingPersonConscription = PersonConscriptionUtil.GetPersonConscription(reservist.PersonId, dateFrom, CurrentUser);

                if (existingPersonConscription != null &&
                    existingPersonConscription.PersonConscriptionId != personConscriptionId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>За тази дата \"От\" вече има въведена наборна служба</status>";
                }
                else
                {
                    PersonConscription personConscription = new PersonConscription(CurrentUser);

                    personConscription.PersonConscriptionId = personConscriptionId;
                    personConscription.MilitaryUnit = militaryUnit;
                    personConscription.Position = position;
                    personConscription.DateFrom = dateFrom;
                    personConscription.DateTo = dateTo;

                    PersonConscriptionUtil.SavePersonConscription(personConscription, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedConscriptionTable = GetConscriptionsTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedConscriptionTable>" + AJAXTools.EncodeForXML(refreshedConscriptionTable) + @"</refreshedConscriptionTable>";
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

        //  Delete a particular Conscription (ajax call)
        private void JSDeleteConscription()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personConscriptionId = int.Parse(Request.Form["ConscriptionId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonConscriptionUtil.DeletePersonConscription(personConscriptionId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedConscriptionTable = GetConscriptionsTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedConscriptionTable>" + AJAXTools.EncodeForXML(refreshedConscriptionTable) + @"</refreshedConscriptionTable>";
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

        //  Render the Conscriptions table
        private string GetConscriptionsTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsConscriptionMilitaryUnitHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsConscriptionPositionHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_POSITION") == UIAccessLevel.Hidden;
            bool IsConscriptionDateFromHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_DATEFROM") == UIAccessLevel.Hidden;
            bool IsConscriptionDateToHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_DATETO") == UIAccessLevel.Hidden;

            if (IsConscriptionMilitaryUnitHidden &&
                IsConscriptionPositionHidden &&
                IsConscriptionDateFromHidden &&
                IsConscriptionDateToHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") == UIAccessLevel.Enabled &&
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") == UIAccessLevel.Enabled && !isPreview
                                        );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewConscription();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsConscriptionMilitaryUnitHidden ? @"<th style='width: 300px; vertical-align: bottom;'>Военно формирование</th>" : "") + @"
  " + (!IsConscriptionPositionHidden ? @"<th style='width: 150px; vertical-align: bottom;'>Длъжност</th>" : "") + @"                    
  " + (!IsConscriptionDateFromHidden ? @"<th style='width: 80px; vertical-align: bottom;'>От</th>" : "") + @"
  " + (!IsConscriptionDateToHidden ? @"<th style='width: 80px; vertical-align: bottom;'>До</th>" : "") + @"
<th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonConscription> listPersonConscription = PersonConscriptionUtil.GetAllPersonConscriptionByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonConscription personConscription in listPersonConscription)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personConscription.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteConscription(" + personConscription.PersonConscriptionId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditConscription(" + personConscription.PersonConscriptionId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsConscriptionMilitaryUnitHidden ? @"<td style='text-align: left;'>" + personConscription.MilitaryUnit + @"</td>" : "") + @"
        " + (!IsConscriptionPositionHidden ? @"<td style='text-align: left;'>" + personConscription.Position + @"</td>" : "") + @"
        " + (!IsConscriptionDateFromHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personConscription.DateFrom) + @"</td>" : "") + @"                    
        " + (!IsConscriptionDateToHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personConscription.DateTo) + @"</td>" : "") + @"                    

                                    <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Conscriptions light-box
        private string GetConscriptionLightBox()
        {
            string html = @"
<center>
    <input type=""hidden"" id=""hdnConscriptionID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditConscriptionTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonConscriptionMilitaryUnit"" class=""InputLabel"">Военно формирование:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonConscriptionMilitaryUnit"" UnsavedCheckSkipMe='true' maxlength=""100"" class=""RequiredInputField"" style=""width: 200px;"" />
            </td>
        </tr>";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonConscriptionPosition"" class=""InputLabel"">Длъжност:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonConscriptionPosition"" UnsavedCheckSkipMe='true' maxlength=""100"" class=""RequiredInputField"" style=""width: 200px;"" />
            </td>
        </tr>";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonConscriptionDateFrom"" class=""InputLabel"">От:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonConscriptionDateFrom""> 
                <input type=""text"" id=""txtPersonConscriptionDateFrom"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonConscriptionDateTo"" class=""InputLabel"">До:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonConscriptionDateTo""> 
                <input type=""text"" id=""txtPersonConscriptionDateTo"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";

            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditConscriptionLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditConscriptionLightBox"" style=""display: inline;"" onclick=""SaveAddEditConscriptionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditConscriptionLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditConscriptionLightBox"" style=""display: inline;"" onclick=""HideAddEditConscriptionLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditConscriptionLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the Conscription table
        public string GetConscriptionUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_MILITARYUNIT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonConscriptionMilitaryUnit");
                disabledClientControls.Add("txtPersonConscriptionMilitaryUnit");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonConscriptionMilitaryUnit");
                hiddenClientControls.Add("txtPersonConscriptionMilitaryUnit");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_POSITION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonConscriptionPosition");
                disabledClientControls.Add("txtPersonConscriptionPosition");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonConscriptionPosition");
                hiddenClientControls.Add("txtPersonConscriptionPosition");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_DATEFROM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonConscriptionDateFrom");
                disabledClientControls.Add("txtPersonConscriptionDateFrom");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonConscriptionDateFrom");
                hiddenClientControls.Add("spanPersonConscriptionDateFrom");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION_DATETO");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonConscriptionDateTo");
                disabledClientControls.Add("txtPersonConscriptionDateTo");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonConscriptionDateTo");
                hiddenClientControls.Add("spanPersonConscriptionDateTo");
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


        // Table Discharge
        //Load Discharge table and light-box (ajax call)
        private void JSLoadDischarges()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Hidden ||
                                   GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE") == UIAccessLevel.Hidden
                                  )
                RedirectAjaxAccessDenied();

            int reservistId = int.Parse(Request.Params["ReservistId"]);

            string stat = "";
            string response = "";

            try
            {
                string tableHTML = GetDischargesTable(reservistId);
                string lightBoxHTML = GetDischargeLightBox();

                string UIItems = GetDischargeUIItems();

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

        // Load a particular Discharge (ajax call)
        private void JSLoadDischarge()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE") == UIAccessLevel.Hidden
                                   )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int personDischargeId = int.Parse(Request.Form["DischargeId"]);

                PersonDischarge personDischarge = PersonDischargeUtil.GetPersonDischarge(personDischargeId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"<personDischarge>
                                <PersonDischargeYear>" + AJAXTools.EncodeForXML(personDischarge.Year.ToString()) + @"</PersonDischargeYear>
                                <PersonDischargeReasonCode>" + AJAXTools.EncodeForXML(personDischarge.DischargeReasonCode) + @"</PersonDischargeReasonCode>
                                <PersonDischargeReasonName>" + AJAXTools.EncodeForXML(personDischarge.DischargeReason.DischargeReasonName) + @"</PersonDischargeReasonName>
                                <PersonDischargeDestinationCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personDischarge.DischargeDestinationCode) ? personDischarge.DischargeDestinationCode : ListItems.GetOptionChooseOne().Value) + @"</PersonDischargeDestinationCode>
                                <PersonDischargeOrder>" + AJAXTools.EncodeForXML(personDischarge.Order) + @"</PersonDischargeOrder>
                                <PersonDischargeOrderDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personDischarge.OrderDate)) + @"</PersonDischargeOrderDate>
                                <PersonDischargeOrderEffectiveDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(personDischarge.OrderEffectiveDate)) + @"</PersonDischargeOrderEffectiveDate>
                                <PersonDischargeMilitaryCommanderRankCode>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(personDischarge.MilitaryCommanderRankCode) ? personDischarge.MilitaryCommanderRankCode : ListItems.GetOptionChooseOne().Value) + @"</PersonDischargeMilitaryCommanderRankCode>
                             </personDischarge>";
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

        //Save a particular Discharge (ajax call)
        private void JSSaveDischarge()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personDischargeId = int.Parse(Request.Form["DischargeId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                int year = int.Parse(Request.Form["Year"]);
                string dischargeReasonCode = Request.Form["DischargeReasonCode"];
                string dischargeDestinationCode = Request.Form["DischargeDestinationCode"];
                string order = Request.Form["Order"];
                DateTime orderDate = CommonFunctions.ParseDate(Request.Form["OrderDate"]).Value;
                DateTime? orderEffectiveDate = (!String.IsNullOrEmpty(Request.Form["OrderEffectiveDate"]) ? CommonFunctions.ParseDate(Request.Form["OrderEffectiveDate"]).Value : (DateTime?)null);
                string militaryCommanderRankCode = Request.Form["MilitaryCommanderRankCode"];

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonDischarge existingPersonDischarge = PersonDischargeUtil.GetPersonDischarge(reservist.PersonId, orderDate, dischargeReasonCode, CurrentUser);

                if (existingPersonDischarge != null &&
                    existingPersonDischarge.PersonDischargeId != personDischargeId)
                {
                    stat = AJAXTools.OK;
                    response = "<status>За тази дата (на заповедта) вече има въведена заповед поради същата причина</status>";
                }
                else
                {
                    PersonDischarge personDischarge = new PersonDischarge(CurrentUser);

                    personDischarge.PersonDischargeId = personDischargeId;
                    personDischarge.Year = year;
                    personDischarge.DischargeReasonCode = dischargeReasonCode;
                    personDischarge.DischargeDestinationCode = dischargeDestinationCode;
                    personDischarge.Order = order;
                    personDischarge.OrderDate = orderDate;
                    personDischarge.OrderEffectiveDate = orderEffectiveDate;
                    personDischarge.MilitaryCommanderRankCode = militaryCommanderRankCode;

                    PersonDischargeUtil.SavePersonDischarge(personDischarge, reservist.Person, CurrentUser, change);

                    change.WriteLog();

                    string refreshedDischargeTable = GetDischargesTable(reservistId);

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedDischargeTable>" + AJAXTools.EncodeForXML(refreshedDischargeTable) + @"</refreshedDischargeTable>";
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

        //  Delete a particular Discharge (ajax call)
        private void JSDeleteDischarge()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Reservists");

                int personDischargeId = int.Parse(Request.Form["DischargeId"]);
                int reservistId = int.Parse(Request.Form["ReservistId"]);

                Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                PersonDischargeUtil.DeletePersonDischarge(personDischargeId, reservist.Person, CurrentUser, change);

                change.WriteLog();

                string refreshedDischargeTable = GetDischargesTable(reservistId);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedDischargeTable>" + AJAXTools.EncodeForXML(refreshedDischargeTable) + @"</refreshedDischargeTable>";
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

        //  Render the Discharges table
        private string GetDischargesTable(int reservistId)
        {
            string tableHTML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool IsDischargeYearHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_YEAR") == UIAccessLevel.Hidden;
            bool IsDischargeDischargeReasonHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_DISCHARGEREASON") == UIAccessLevel.Hidden;
            bool IsDischargeDischargeDestinationHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_DISCHARGEDESTINATION") == UIAccessLevel.Hidden;
            bool IsDischargeOrderHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDER") == UIAccessLevel.Hidden;
            bool IsDischargeOrderDateHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDERDATE") == UIAccessLevel.Hidden;
            bool IsDischargeOrderEffectiveDateHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDEREFFECTIVEDATE") == UIAccessLevel.Hidden;
            bool IsDischargeMilitaryCommanderRankHidden = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_MILITARYCMMANDERRANK") == UIAccessLevel.Hidden;

            if (IsDischargeYearHidden &&
                IsDischargeDischargeReasonHidden &&
                IsDischargeDischargeDestinationHidden &&
                IsDischargeOrderHidden &&
                IsDischargeOrderDateHidden &&
                IsDischargeOrderEffectiveDateHidden &&
                IsDischargeMilitaryCommanderRankHidden)
            {
                return "";
            }

            bool notHideDelEditHTML = (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Enabled &&
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE") == UIAccessLevel.Enabled && !isPreview
                                      );

            Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

            StringBuilder html = new StringBuilder();

            string newHTML = "";

            if (notHideDelEditHTML)
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewDischarge();' />";

            html.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsDischargeYearHidden ? @"                 <th style='width: 60px; vertical-align: bottom;'>Година</th>" : "") + @"
  " + (!IsDischargeDischargeReasonHidden ? @"      <th style='width: 250px; vertical-align: bottom;'>Причина за освобождаване от ВС</th>" : "") + @"                    
  " + (!IsDischargeDischargeDestinationHidden ? @" <th style='width: 200px; vertical-align: bottom;'>Къде отива при снемане от отчет</th>" : "") + @"
  " + (!IsDischargeOrderHidden ? @"                <th style='width: 100px; vertical-align: bottom;'>Заповед</th>" : "") + @"
  " + (!IsDischargeOrderDateHidden ? @"            <th style='width: 80px; vertical-align: bottom;'>Дата</th>" : "") + @"
  " + (!IsDischargeOrderEffectiveDateHidden ? @"   <th style='width: 80px; vertical-align: bottom;'>Считано от</th>" : "") + @"
  " + (!IsDischargeMilitaryCommanderRankHidden ? @"<th style='width: 150px; vertical-align: bottom;'>Подписал заповедта</th>" : "") + @"
                                                       <th style='width: 50px;vertical-align: top;'>
                                                          <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                                       </th>
                                </tr>
                              </thead>");

            int counter = 0;

            List<PersonDischarge> listPersonDischarge = PersonDischargeUtil.GetAllPersonDischargeByPersonID(reservist.PersonId, CurrentUser);

            foreach (PersonDischarge personDischarge in listPersonDischarge)
            {
                counter++;

                string deleteHTML = "";
                string editHTML = "";

                if (personDischarge.CanDelete)
                {
                    if (notHideDelEditHTML)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteDischarge(" + personDischarge.PersonDischargeId.ToString() + ");' />";
                }

                if (notHideDelEditHTML)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditDischarge(" + personDischarge.PersonDischargeId.ToString() + ");' />";

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                                 <td style='text-align: center;'>" + counter.ToString() + @"</td>
" + (!IsDischargeYearHidden ? @"                 <td style='text-align: left;'>" + personDischarge.Year.ToString() + @"</td>" : "") + @"
" + (!IsDischargeDischargeReasonHidden ? @"      <td style='text-align: left;'>" + (personDischarge.DischargeReason != null ? personDischarge.DischargeReason.DischargeReasonName : "") + @"</td>" : "") + @"
" + (!IsDischargeDischargeDestinationHidden ? @" <td style='text-align: left;'>" + (personDischarge.DischargeDestination != null ? personDischarge.DischargeDestination.DischargeDestinationName : "") + @"</td>" : "") + @"
" + (!IsDischargeOrderHidden ? @"                <td style='text-align: left;'>" + personDischarge.Order + @"</td>" : "") + @"
" + (!IsDischargeOrderDateHidden ? @"            <td style='text-align: left;'>" + CommonFunctions.FormatDate(personDischarge.OrderDate) + @"</td>" : "") + @"
" + (!IsDischargeOrderEffectiveDateHidden ? @"   <td style='text-align: left;'>" + CommonFunctions.FormatDate(personDischarge.OrderEffectiveDate) + @"</td>" : "") + @"
" + (!IsDischargeMilitaryCommanderRankHidden ? @"<td style='text-align: left;'>" + (personDischarge.MilitaryCommanderRank != null ? personDischarge.MilitaryCommanderRank.MilitaryCommanderRankName : "") + @"</td>" : "") + @"
                                                 <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                              </tr>
                             ");
            }

            html.Append("</table>");

            tableHTML = html.ToString();

            return tableHTML;
        }

        //Render the Discharges light-box
        private string GetDischargeLightBox()
        {
            List<DischargeDestination> listDischargeDestination = DischargeDestinationUtil.GetAllDischargeDestinations(CurrentUser);
            List<IDropDownItem> ddiDischargeDestination = new List<IDropDownItem>();

            foreach (DischargeDestination dischargeDestination in listDischargeDestination)
            {
                ddiDischargeDestination.Add(dischargeDestination);
            }

            // Generates html for drop down list
            string dischargeDestinationHTML = ListItems.GetDropDownHtml(ddiDischargeDestination, null, "ddPersonDischargeDischargeDestination", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true' class='InputField' ");


            List<MilitaryCommanderRank> listMilitaryCommanderRank = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRank)
            {
                ddiMilitaryCommanderRank.Add(militaryCommanderRank);
            }

            // Generates html for drop down list
            string dischargeMilitaryCommanderRankHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonDischargeDischargeMilitaryCommanderRank", true, null, "", "style='width: 300px;' UnsavedCheckSkipMe='true' class='InputField' ");


            string html = @"
<center>
    <input type=""hidden"" id=""hdnDischargeID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditDischargeTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeYear"" class=""InputLabel"">Година:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonDischargeYear"" UnsavedCheckSkipMe='true' maxlength=""4"" class=""RequiredInputField"" style=""width: 50px;"" />
            </td>
        </tr>";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;vertical-align: top;"">
                <span id=""lblPersonDischargeDischargeReason"" class=""InputLabel"">Причина за освобождаване от ВС:</span>
            </td>
            
            <td style=""text-align: left;vertical-align: top;"">
                <input type=""hidden"" id=""hdnDischargeReasonCode"" />
                <input type=""hidden"" id=""hdnDischargeReasonName"" />
                <table>
                    <tr>
                        <td style=""text-align: bottom;"">
                            <div id=""txtDischargeReason"" class=""ReadOnlyValue"" style=""background-color:#FFFFCC;width: 300px;min-height:20px""/>
                        </td>
                        <td style=""vertical-align: top;"">
                            <input id=""btnSelectDischargeReason""
                                   onclick='dischargeReasonSelector.showDialog(""dischargeReasonSelectorForPerson"", DischargeReasonSelector_OnSelectedDischargeReason);' 
                                   type=""button"" value=""Търсене"" class=""OpenCompanySelectorButton"" style=""margin-top:0px""/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeDischargeDestination"" class=""InputLabel"">Къде отива при снемане от отчет:</span>
            </td>
            <td style=""text-align: left;"">
                " + dischargeDestinationHTML + @"
            </td>
        </tr>";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeOrder"" class=""InputLabel"">Заповед:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonDischargeOrder"" UnsavedCheckSkipMe='true' maxlength=""10"" class=""InputField"" style=""width: 100px;"" />
            </td>
        </tr>";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeOrderDate"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonDischargeOrderDate""> 
                <input type=""text"" id=""txtPersonDischargeOrderDate"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";

            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeEffectiveOrderDate"" class=""InputLabel"">Считано от:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonDischargeOrderEffectiveDate""> 
                <input type=""text"" id=""txtPersonDischargeOrderEffectiveDate"" class='InputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" /> </span>
            </td>
        </tr> ";

            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonDischargeDischargeMilitaryCommanderRank"" class=""InputLabel"">Подписал заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                " + dischargeMilitaryCommanderRankHTML + @"
            </td>
        </tr>";

           
            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditDischargeLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditDischargeLightBox"" style=""display: inline;"" onclick=""SaveAddEditDischargeLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditDischargeLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditDischargeLightBox"" style=""display: inline;"" onclick=""HideAddEditDischargeLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditDischargeLightBoxText"" style=""width: 70px;"">
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

        //Get the UIItems info for the Discharge table
        public string GetDischargeUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_YEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeYear");
                disabledClientControls.Add("txtPersonDischargeYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeYear");
                hiddenClientControls.Add("txtPersonDischargeYear");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_DISCHARGEREASON");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeDischargeReason");
                disabledClientControls.Add("txtDischargeReason");
                hiddenClientControls.Add("btnSelectDischargeReason");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeDischargeReason");
                hiddenClientControls.Add("txtDischargeReason");
                hiddenClientControls.Add("btnSelectDischargeReason");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_DISCHARGEDESTINATION");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeDischargeDestination");
                disabledClientControls.Add("ddPersonDischargeDischargeDestination");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeDischargeDestination");
                hiddenClientControls.Add("ddPersonDischargeDischargeDestination");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDER");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeOrder");
                disabledClientControls.Add("txtPersonDischargeOrder");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeOrder");
                hiddenClientControls.Add("txtPersonDischargeOrder");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDERDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeOrderDate");
                disabledClientControls.Add("txtPersonDischargeOrderDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeOrderDate");
                hiddenClientControls.Add("spanPersonDischargeOrderDate");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_ORDEREFFECTIVEDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeEffectiveOrderDate");
                disabledClientControls.Add("txtPersonDischargeOrderEffectiveDate");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeEffectiveOrderDate");
                hiddenClientControls.Add("spanPersonDischargeOrderEffectiveDate");
            }

            l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE_MILITARYCMMANDERRANK");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonDischargeDischargeMilitaryCommanderRank");
                disabledClientControls.Add("ddPersonDischargeDischargeMilitaryCommanderRank");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonDischargeDischargeMilitaryCommanderRank");
                hiddenClientControls.Add("ddPersonDischargeDischargeMilitaryCommanderRank");
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

    public static class AddEditReservist_MilitaryService_PageUtil
    {
        public static string GetTabContent(AddEditReservist page)
        {
            string html = @" <div style=""width: 900px; overflow-x: auto;"">";

            //Table PreviousPositions

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PREVIOUSPOSITION") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divPreviousPosition"">
                           <div style=""text-align: left;"">
                              <span id=""lnkPreviousPosition"" class=""DataTableExpandLink"" onclick=""lnkPreviousPosition_Click();"">Заемани длъжности</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingPreviousPosition"" />
                           </div>
                           <div id=""tblPreviousPosition"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlPreviousPositionMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessagePreviousPosition""></span>
                           </div>
                           <div id=""lboxPreviousPosition"" style=""display: none;"" class=""lboxPreviousPosition""></div>
                           <input type=""hidden"" id=""hdnPreviousPositionLoaded"" value=""0"" />
                        </div>";
            }


            //Table ArchiveTitles

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_ARCHIVETITLE") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divArchiveTitle"">
                           <div style=""text-align: left;"">
                              <span id=""lnkArchiveTitle"" class=""DataTableExpandLink"" onclick=""lnkArchiveTitle_Click();"">Архив звания</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingArchiveTitle"" />
                           </div>
                           <div id=""tblArchiveTitle"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlArchiveTitleMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageArchiveTitle""></span>
                           </div>
                           <div id=""lboxArchiveTitle"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnArchiveTitleLoaded"" value=""0"" />
                        </div>";
            }

            //Table RewardIncentiv

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_REWARDINCENTIV") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divRewardIncentiv"">
                           <div style=""text-align: left;"">
                              <span id=""lnkRewardIncentiv"" class=""DataTableExpandLink"" onclick=""lnkRewardIncentiv_Click();"">Награди и поощрения</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingRewardIncentiv"" />
                           </div>
                           <div id=""tblRewardIncentiv"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlRewardIncentivMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageRewardIncentiv""></span>
                           </div>
                           <div id=""lboxRewardIncentiv"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnRewardIncentivLoaded"" value=""0"" />
                        </div>";
            }

            //Table Penalties

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_PENALTY") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divPenalty"">
                           <div style=""text-align: left;"">
                              <span id=""lnkPenalty"" class=""DataTableExpandLink"" onclick=""lnkPenalty_Click();"">Наказания</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingPenalty"" />
                           </div>
                           <div id=""tblPenalty"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlPenaltyMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessagePenalty""></span>
                           </div>
                           <div id=""lboxPenalty"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnPenaltyLoaded"" value=""0"" />
                        </div>";
            }

            //Table Contracts

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONTRACT") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divContract"">
                           <div style=""text-align: left;"">
                              <span id=""lnkContract"" class=""DataTableExpandLink"" onclick=""lnkContract_Click();"">Договори</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingContract"" />
                           </div>
                           <div id=""tblContract"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlContractMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageContract""></span>
                           </div>
                           <div id=""lboxContract"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnContractLoaded"" value=""0"" />
                        </div>";
            }

            //Table Conscription

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_CONSCRIPTION") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divConscription"">
                           <div style=""text-align: left;"">
                              <span id=""lnkConscription"" class=""DataTableExpandLink"" onclick=""lnkConscription_Click();"">Наборна служба</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingConscription"" />
                           </div>
                           <div id=""tblConscription"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlConscriptionMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageConscription""></span>
                           </div>
                           <div id=""lboxConscription"" style=""display: none;"" class=""lboxCivilEducation""></div>
                           <input type=""hidden"" id=""hdnConscriptionLoaded"" value=""0"" />
                        </div>";
            }

            //Table Discharge

            if (page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV_DISCHARGE") != UIAccessLevel.Hidden)
            {
                html += @"
                        <div style=""width: 900px; overflow-x: auto;"">

                        <div style=""height: 10px;""></div>
                        <div id=""divDischarge"">
                           <div style=""text-align: left;"">
                              <span id=""lnkDischarge"" class=""DataTableExpandLink"" onclick=""lnkDischarge_Click();"">Заповеди за прекратяване на служба</span>
                              <img src=""../Images/data_table_loading.gif"" style=""Position: relative; top: 3px; visibility: hidden;"" id=""imgLoadingDischarge"" />
                           </div>
                           <div id=""tblDischarge"" style=""text-align: left; padding;left: 5px; padding-top: 8px;""></div>
                           <div id=""pnlDischargeMessage"" style=""text-align: left; padding;left: 5px; padding-top: 5px;"">
                              <span id=""lblMessageDischarge""></span>
                           </div>
                           <div id=""lboxDischarge"" style=""display: none;"" class=""lboxPersonDischarge""></div>
                           <input type=""hidden"" id=""hdnDischargeLoaded"" value=""0"" />
                        </div>";
            }

           

            html += @"</div>";

            return html;
        }
    }
}
