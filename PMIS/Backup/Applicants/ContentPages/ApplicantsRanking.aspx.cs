using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Text;
using System.Linq;

namespace PMIS.Applicants.ContentPages
{
    public partial class ApplicantsRanking : APPLPage
    {       
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_APPL_RANKING";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_APPL_RANKING") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePositions")
            {
                JSSavePositions();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Applicants", "Applicants_Ranking");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            SetupPageUI();
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            //Set the message if there is a need (e.g. save data)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "SAVED")
                {
                    lblGridMessage.Text = "Данните са записани успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.GetUIItemAccessLevel("APPL_APPL_RANKING") != UIAccessLevel.Enabled)
            {
                 pageDisabledControls.Add(btnSave);
            }
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateVacancyAnnounces();
        }

        //Populate the VacancyAnnounce drop-down
        private void PopulateVacancyAnnounces()
        {
            ddOrderNum.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForRanking(CurrentUser);
            ddOrderNum.DataTextField = "Text";
            ddOrderNum.DataValueField = "Value";
            ddOrderNum.DataBind();           
            ddOrderNum.Items.Insert(0, ListItems.GetOptionChooseOne());
            ListItems.SetTextAsTooltip(ddOrderNum);                
        }     
        
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }                

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ddOrderNum.SelectedValue != "-1" &&
                ddResponsibleMilitaryUnit.SelectedValue != "-1" &&
                ddPositionName.SelectedValue != "-1" &&
                ddMilitaryUnit.SelectedValue != "-1")
            {
                int vacancyAnnounceId = int.Parse(ddOrderNum.SelectedValue);
                int responsibleMilitaryUnit = int.Parse(ddResponsibleMilitaryUnit.SelectedValue);
                int vacancyAnnouncePositionId = int.Parse(ddPositionName.SelectedValue);
                int militaryUnitId = int.Parse(ddMilitaryUnit.SelectedValue);

                pnlDataGrid.InnerHtml = GenerateTable(vacancyAnnounceId, responsibleMilitaryUnit, vacancyAnnouncePositionId, militaryUnitId);
            }
            else
                pnlDataGrid.InnerHtml = "";
        }

        protected void ddOrderNum_Changed(object sender, EventArgs e)
        {
            ddResponsibleMilitaryUnit.Items.Clear();
            ddPositionName.Items.Clear();
            ddMilitaryUnit.Items.Clear();

            if (ddOrderNum.SelectedValue != "-1")
            {
                int vacancyAnnounceID = int.Parse(ddOrderNum.SelectedValue);

                // populate drop down with responsible military units for this vacancy announce
                ddResponsibleMilitaryUnit.DataSource = VacancyAnnounceUtil.GetDistinctRespMilitaryUnitsForVacancyAnnounceID(vacancyAnnounceID, CurrentUser);
                ddResponsibleMilitaryUnit.DataTextField = "Text";
                ddResponsibleMilitaryUnit.DataValueField = "Value";
                ddResponsibleMilitaryUnit.DataBind();
                ddResponsibleMilitaryUnit.Items.Insert(0, ListItems.GetOptionChooseOne());
                ListItems.SetTextAsTooltip(ddResponsibleMilitaryUnit);                
            }          
        }

        protected void ddResponsibleMilitaryUnit_Changed(object sender, EventArgs e)
        {
            ddPositionName.Items.Clear();
            ddMilitaryUnit.Items.Clear();

            if (ddResponsibleMilitaryUnit.SelectedValue != "-1")
            {
                int vacancyAnnounceID = int.Parse(ddOrderNum.SelectedValue);
                int responsibleMilitaryUnitID = int.Parse(ddResponsibleMilitaryUnit.SelectedValue);

                // populate drop down with positions for this vacancy announce and responsible military unit
                ddPositionName.DataSource = VacancyAnnouncePositionUtil.GetDistinctPositionsByVacancyAnnounceIDAndRespMilitaryUnitID(vacancyAnnounceID, responsibleMilitaryUnitID, CurrentUser);
                ddPositionName.DataTextField = "Text";
                ddPositionName.DataValueField = "Value";
                ddPositionName.DataBind();
                ddPositionName.Items.Insert(0, ListItems.GetOptionChooseOne());
                ListItems.SetTextAsTooltip(ddPositionName);                
            }          
        }

        protected void ddPositionName_Changed(object sender, EventArgs e)
        {
            ddMilitaryUnit.Items.Clear();

            if (ddPositionName.SelectedValue != "-1")
            {
                int vacancyAnnounceID = int.Parse(ddOrderNum.SelectedValue);                
                int responsibleMilitaryUnitID = int.Parse(ddResponsibleMilitaryUnit.SelectedValue);
                int vacancyAnnouncePositionID = int.Parse(ddPositionName.SelectedValue);

                // populate drop down with military units for this vacancy announce and responsible military unit and position
                ddMilitaryUnit.DataSource = VacancyAnnouncePositionUtil.GetDistinctMilitaryUnitsForPosition(vacancyAnnounceID, responsibleMilitaryUnitID, vacancyAnnouncePositionID, CurrentUser);
                ddMilitaryUnit.DataTextField = "Text";
                ddMilitaryUnit.DataValueField = "Value";
                ddMilitaryUnit.DataBind();
                ddMilitaryUnit.Items.Insert(0, ListItems.GetOptionChooseOne());
                ListItems.SetTextAsTooltip(ddMilitaryUnit);                
            }                
        }

        // Generates html table with application positions according to choosen parameters in filter section
        private string GenerateTable(int vacancyAnnounceId, int responsibleMilitaryUnit, int vacancyAnnouncePositionId, int militaryUnitId)
        {
            StringBuilder sb = new StringBuilder();

            List<ApplicantPositionStatus> allApplicantPositionStatuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(CurrentUser);

            List<IDropDownItem> applicantPositionStatuses = new List<IDropDownItem>();

            ApplicantPositionStatus appointedStatus = (from s in allApplicantPositionStatuses where s.StatusKey == "APPOINTED" select s).FirstOrDefault();
            ApplicantPositionStatus allowedStatus = (from s in allApplicantPositionStatuses where s.StatusKey == "PARTICIPATIONALLOWED" select s).FirstOrDefault();
            allowedStatus.StatusName = ApplicantExamStatusUtil.GetApplicantExamStatusByKey("RATED", CurrentUser).StatusName;

            applicantPositionStatuses.Add(allowedStatus);
            applicantPositionStatuses.Add(appointedStatus);
            applicantPositionStatuses.Add((from s in allApplicantPositionStatuses where s.StatusKey == "RESERVE" select s).FirstOrDefault());

            List<IDropDownItem> allApplicantPositionStatusListItems = new List<IDropDownItem>();

            foreach (ApplicantPositionStatus status in allApplicantPositionStatuses)
            {
                allApplicantPositionStatusListItems.Add(status);
            }

            List<Exam> exams = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);

            List<RankPositionBlock> positions = ApplicantPositionUtil.GetAllApplicantPositionForRanking(vacancyAnnounceId, responsibleMilitaryUnit, militaryUnitId, vacancyAnnouncePositionId, CurrentUser);

            bool IsRatingHidden = this.GetUIItemAccessLevel("APPL_APPL_RANKING_RATING") == UIAccessLevel.Hidden;
            bool IsMarkHidden = this.GetUIItemAccessLevel("APPL_APPL_RANKING_MARK") == UIAccessLevel.Hidden;
            bool IsPointsHidden = this.GetUIItemAccessLevel("APPL_APPL_RANKING_POINTS") == UIAccessLevel.Hidden;
            bool IsStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_RANKING_STATUS") == UIAccessLevel.Hidden;

            bool isScreenEnabled = this.GetUIItemAccessLevel("APPL_APPL_RANKING") == UIAccessLevel.Enabled;
            bool IsStatusEnabled = isScreenEnabled && (this.GetUIItemAccessLevel("APPL_APPL_RANKING_STATUS") == UIAccessLevel.Enabled);

            string statusDisplayStyle = IsStatusHidden ? "display : none" : "";

            sb.Append("<center>");
            sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 20px;\"></th>");
            sb.Append("<th style=\"width: 150px;\">Име</th>");
            sb.Append("<th style=\"width: 65px;\">ЕГН</th>");
            if (!IsRatingHidden)
                sb.Append("<th style=\"width: 50px; \">Входящ бал</th>");

            foreach (Exam exam in exams)
            {
                if (!IsMarkHidden)
                    sb.Append("<th style=\"width: 50px; \">Оценка " + exam.ExamName + "</th>");

                if (!IsPointsHidden)
                    sb.Append("<th style=\"width: 50px; \">Точки " + exam.ExamName + "</th>");
            }

            if (!IsPointsHidden)
                sb.Append("<th style=\"width: 120px; \">Общо точки</th>");

            sb.Append("<th style=\"width: 120px; " + statusDisplayStyle + "\">Статус</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 0;

            if (positions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (RankPositionBlock position in positions)
            {
                counter++;

                decimal points = 0;

                bool isReadOnly = true;

                // When the applicant has status which is not allowed to change on this page, then show all in Read-only mode
                if ((from s in applicantPositionStatuses select s.Value()).Contains(position.StatusID.ToString()))
                {
                    isReadOnly = false;
                }                

                string statusHTML = "";
                if (IsStatusEnabled && !isReadOnly)
                    statusHTML = ListItems.GetDropDownHtml(applicantPositionStatuses, null, "ddStatus" + counter.ToString(), false, (from s in applicantPositionStatuses where s.Value() == position.StatusID.ToString() select s).First(), "", "");
                else
                {
                    statusHTML = "<div style='display: none'>" + ListItems.GetDropDownHtml(allApplicantPositionStatusListItems, null, "ddStatus" + counter.ToString(), false, (from s in allApplicantPositionStatusListItems where s.Value() == position.StatusID.ToString() select s).First(), "", "") + "</div>";
                    statusHTML += (from s in allApplicantPositionStatusListItems where s.Value() == position.StatusID.ToString() select s).First().Text();
                }

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                sb.Append("<td>" + position.ApplicantName + "</td>");
                sb.Append("<td>" + position.ApplicantIdentNumber + "</td>");
                if (!IsRatingHidden)
                    sb.Append("<td align='center'>" + (position.Rating.HasValue ? position.Rating.Value.ToString() : "") + "</td>");

                points += position.Rating.HasValue ? position.Rating.Value : 0;

                int marksCounter = 0;
                foreach (ApplicantExamMarkBlock mark in position.Marks)
                {
                    marksCounter++;

                    if (!IsMarkHidden)
                        sb.Append("<td align='center'>" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "</td>");
                    if (!IsPointsHidden)
                        sb.Append("<td align='center'>" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "</td>");
                   
                    points += mark.Points.HasValue ? mark.Points.Value : 0;
                }

                if (!IsPointsHidden)
                    sb.Append("<td align='center'>" + points + "</td>");

                sb.Append("<td style='" + statusDisplayStyle + "' >" + statusHTML + "</td>");
                
                sb.Append("<input type='hidden' id='positionID" + counter + "' value='" + position.ApplicantPositionID + "' />");

                sb.Append("</tr>");
            }
            if (positions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("<input type='hidden' id='positionsCounter' value='" + counter + "'/>");
            sb.Append("</table>");
            sb.Append("</center>");

            if (positions.Count > 0)
            {
                int positionsAppointed = 0;

                positionsAppointed = (from p in positions where p.StatusID == appointedStatus.StatusId select p).Count();

                VacancyAnnouncePosition vacancyAnnouncePosition = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(positions.First().VacancyAnnouncePositionID, CurrentUser);

                sb.Append("<div style='height: 17px;'>&nbsp;</div>");
                sb.Append("<span class='InputLabel' style='margin-left: 100px'>Брой за длъжността: </span><span class='InputField'>" + vacancyAnnouncePosition.PositionsCnt.ToString() + "</span>");
                sb.Append("<span class='InputLabel' style='margin-left: 300px'>Брой определени кандидати: </span><span class='InputField'>" + positionsAppointed.ToString() + "</span>");
            }

            return sb.ToString();
        }

        // saves all position by ajax request, if needed
        private void JSSavePositions()
        {
            string response = "";

            bool result = true;

            int positionsCount = int.Parse(Request.Params["PositionsCount"]);

            int vacancyAnnounceID = int.Parse(Request.Params["VacancyAnnounceID"]);
            int responsibleMilitaryUnitID = int.Parse(Request.Params["ResponsibleMilitaryUnitID"]);
            int vacancyAnnouncePositionID = int.Parse(Request.Params["VacancyAnnouncePositionID"]);
            int militaryUnitID = int.Parse(Request.Params["MilitaryUnitID"]);
            string orderNumDate = Request.Params["OrderNumDate"].ToString();
            string responsibleMilitaryUnitName = Request.Params["ResponsibleMilitaryUnitName"].ToString();

            List<RankPositionBlock> oldPositions = ApplicantPositionUtil.GetAllApplicantPositionForRanking(vacancyAnnounceID, responsibleMilitaryUnitID, militaryUnitID, vacancyAnnouncePositionID, CurrentUser);

            List<RankPositionBlock> newPositions = new List<RankPositionBlock>();

            for (int i = 1; i <= positionsCount; i++)
            {
                int applicantPositionID = int.Parse(Request.Params["ApplicantPositionID" + i.ToString()]);                
                int statusID = int.Parse(Request.Params["StatusID" + i.ToString()]);

                RankPositionBlock block = new RankPositionBlock();
                block.ApplicantPositionID = applicantPositionID;
                block.StatusID = statusID;

                newPositions.Add(block);
            }

            ApplicantPositionStatus appointedStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey("APPOINTED", CurrentUser);
            int positionsAppointed = 0;

            positionsAppointed = (from p in newPositions where p.StatusID == appointedStatus.StatusId select p).Count();

            VacancyAnnouncePosition vacancyAnnouncePosition = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(oldPositions.First().VacancyAnnouncePositionID, CurrentUser);

            if (positionsAppointed > vacancyAnnouncePosition.PositionsCnt)
            {
                response = "Броят на определените кандидати за длъжността не може да надвишава обявения брой за длъжността";
                result = false;
            }
            else
            {
                List<RankPositionBlock> newPositionsToSave = (from n in newPositions
                                                              join o in oldPositions on n.ApplicantPositionID equals o.ApplicantPositionID
                                                              where n.StatusID != o.StatusID
                                                              select n).ToList();

                List<RankPositionBlock> oldPositionsToSave = (from o in oldPositions
                                                              join n in newPositionsToSave on o.ApplicantPositionID equals n.ApplicantPositionID
                                                              select o).ToList();

                if (newPositionsToSave.Count > 0)
                {
                    Change change = new Change(CurrentUser, "APPL_Applicants");

                    var positionCode = VacancyAnnouncePositionUtil.GetPositionCodeByVacancyAnnouncePositionId(vacancyAnnouncePositionID, CurrentUser);
                    result = ApplicantPositionUtil.SaveRankPositionBlocks(oldPositionsToSave, newPositionsToSave, orderNumDate, responsibleMilitaryUnitName, positionCode, CurrentUser, change);

                    change.WriteLog();
                }

                VacancyAnnounceUtil.SetVacancyAnnounceStatusFlow(vacancyAnnounceID, "RANK", CurrentUser);

                response = result ? AJAXTools.OK : AJAXTools.ERROR;
            }            

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddOrderNum.SelectedValue = ListItems.GetOptionAll().Value;
            ddOrderNum_Changed(sender, e);
            btnRefresh_Click(sender, e);
        }
    }
}
