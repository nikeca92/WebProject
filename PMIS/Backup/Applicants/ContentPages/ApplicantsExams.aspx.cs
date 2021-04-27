using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Text;
using System.Linq;

namespace PMIS.Applicants.ContentPages
{
    public partial class ApplicantsExams : APPLPage
    {       
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_APPL_EXAMMARKS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_APPL_EXAMMARKS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveExams")
            {
                JSSaveExams();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Applicants", "Applicants_Exams");

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
            if (this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS") != UIAccessLevel.Enabled)
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
            ddOrderNum.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForExams(CurrentUser);
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
                ddResponsibleMilitaryUnit.SelectedValue != "-1")
            {
                int vacancyAnnounceId = int.Parse(ddOrderNum.SelectedValue);
                int responsibleMilitaryUnit = int.Parse(ddResponsibleMilitaryUnit.SelectedValue);               

                pnlDataGrid.InnerHtml = GenerateTable(vacancyAnnounceId, responsibleMilitaryUnit);
            }
            else
                pnlDataGrid.InnerHtml = "";
        }

        protected void ddOrderNum_Changed(object sender, EventArgs e)
        {
            ddResponsibleMilitaryUnit.Items.Clear();            

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

        // Generates html table with application positions according to choosen parameters in filter section
        private string GenerateTable(int vacancyAnnounceId, int responsibleMilitaryUnit)
        {
            StringBuilder sb = new StringBuilder();

            List<ApplicantExamStatus> allApplicantExamStatuses = ApplicantExamStatusUtil.GetAllApplicantExamStatuses(CurrentUser);
            List<ApplicantPositionStatus> allApplicantPositionStatuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(CurrentUser);

            List<IDropDownItem> applicantExamStatuses = new List<IDropDownItem>();

            ApplicantExamStatus ratedExamStatus = (from s in allApplicantExamStatuses where s.StatusKey == "RATED" select s).FirstOrDefault();

            ApplicantPositionStatus allowedStatus = (from s in allApplicantPositionStatuses where s.StatusKey == "PARTICIPATIONALLOWED" select s).FirstOrDefault();
            DropDownItem chooseOne = new DropDownItem();
            chooseOne.Txt = allowedStatus.StatusName;
            chooseOne.Val = ListItems.GetOptionChooseOne().Value;

            applicantExamStatuses.Add(chooseOne);
            applicantExamStatuses.Add(ratedExamStatus);
            applicantExamStatuses.Add((from s in allApplicantExamStatuses where s.StatusKey == "NOTRATED" select s).FirstOrDefault());

            List<Exam> exams = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);

            List<ApplicantExamsBlock> blocks = ApplicantExamMarkUtil.GetAllApplicantExamsBlock(vacancyAnnounceId, responsibleMilitaryUnit, CurrentUser);

            bool IsMarkHidden = this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_MARK") == UIAccessLevel.Hidden;
            bool IsPointsHidden = this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_POINTS") == UIAccessLevel.Hidden;
            bool IsExamStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_EXAMSTATUS") == UIAccessLevel.Hidden;

            bool isScreenEnabled = this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS") == UIAccessLevel.Enabled;
            bool IsMarkEnabled = isScreenEnabled && (this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_MARK") == UIAccessLevel.Enabled);
            bool IsPointsEnabled = isScreenEnabled && (this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_POINTS") == UIAccessLevel.Enabled);
            bool IsExamStatusEnabled = isScreenEnabled && (this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS_EXAMSTATUS") == UIAccessLevel.Enabled);

            string markDisplayStyle = IsMarkHidden ? "display : none" : "";
            string pointsDisplayStyle = IsPointsHidden ? "display : none" : "";
            string examStatusDisplayStyle = IsExamStatusHidden ? "display : none" : "";

            sb.Append("<center>");
            sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 20px;\"></th>");
            sb.Append("<th style=\"width: 160px;\">Име</th>");
            sb.Append("<th style=\"width: 80px;\">ЕГН</th>");

            foreach (Exam exam in exams)
            {
                sb.Append("<th style=\"width: 80px; " + markDisplayStyle + "\">Оценка " + exam.ExamName + "</th>");
                sb.Append("<th style=\"width: 80px; " + pointsDisplayStyle + "\">Точки " + exam.ExamName + "</th>");
            }

            sb.Append("<th style=\"width: 120px; " + examStatusDisplayStyle + "\">Статус</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 0;

            if (blocks.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (ApplicantExamsBlock block in blocks)
            {               
                counter++;

                bool isReadOnly = block.HasRelatedApplPositionsWithHigherStatus;

                string statusHTML = "";
                if (IsExamStatusEnabled && !isReadOnly)
                    statusHTML = ListItems.GetDropDownHtml(applicantExamStatuses, null, "ddExamStatus" + counter.ToString(), false, (from s in applicantExamStatuses where block.ApplicantExamStatus.HasValue && (s.Value() == block.ApplicantExamStatus.Value.ToString()) select s).FirstOrDefault(), "", "");
                else
                {
                    statusHTML = "<div style='display: none'>" + ListItems.GetDropDownHtml(applicantExamStatuses, null, "ddExamStatus" + counter.ToString(), true, (from s in applicantExamStatuses where block.ApplicantExamStatus.HasValue && (s.Value() == block.ApplicantExamStatus.ToString()) select s).FirstOrDefault(), "", "") + "</div>";
                    statusHTML += (from s in applicantExamStatuses where block.ApplicantExamStatus.HasValue && (s.Value() == block.ApplicantExamStatus.ToString()) select s).FirstOrDefault().Text();
                }

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                sb.Append("<td>" + block.ApplicantName + "</td>");
                sb.Append("<td>" + block.ApplicantIdentNumber + "</td>");

                int marksCounter = 0;
                foreach (ApplicantExamMarkBlock mark in block.Marks)
                {
                    marksCounter++;

                    string markHTML = "";
                    if (IsMarkEnabled)
                        markHTML = "<input type='text' id='mark" + counter + "_" + marksCounter + "' style='width: 50px;' value='" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "'/>";
                    else
                    {
                        markHTML = "<input type='text' id='mark" + counter + "_" + marksCounter + "' style='width: 50px; display: none;' value='" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "'/>";
                        markHTML += mark.Mark.HasValue ? mark.Mark.Value.ToString() : "";
                    }

                    string pointsHTML = "";
                    if (IsPointsEnabled)
                        pointsHTML = "<input type='text' id='points" + counter + "_" + marksCounter + "' style='width: 50px;' value='" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "'/>";
                    else
                    {
                        pointsHTML = "<input type='text' id='points" + counter + "_" + marksCounter + "' style='width: 50px; display: none;' value='" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "'/>";
                        pointsHTML += mark.Points.HasValue ? mark.Points.Value.ToString() : "";
                    }

                    sb.Append("<td style='" + markDisplayStyle + "' align='center'>" + markHTML + "</td>");
                    sb.Append("<td style='" + pointsDisplayStyle + "' align='center'>" + pointsHTML + "</td>");
                    sb.Append("<input type='hidden' id='applicantExamMarkID" + counter + "_" + marksCounter + "' value='" + (mark.ApplicantExamMarkId.HasValue ? mark.ApplicantExamMarkId.Value.ToString() : "0") + "' />");
                    sb.Append("<input type='hidden' id='vacancyAnnounceExamID" + counter + "_" + marksCounter + "' value='" + mark.VacancyAnnounceExamId.ToString() + "' />");
                    sb.Append("<input type='hidden' id='examName" + counter + "_" + marksCounter + "' value='" + mark.ExamName + "' />");
                }

                sb.Append("<td style='" + examStatusDisplayStyle + "' >" + statusHTML + "</td>");

                sb.Append("<input type='hidden' id='applicantID" + counter + "' value='" + block.ApplicantId + "' />");

                sb.Append("</tr>");                
            }

            if (blocks.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("<input type='hidden' id='positionsCounter' value='" + counter + "'/>");
            sb.Append("<input type='hidden' id='examsCounter' value='" + exams.Count + "'/>");
            sb.Append("</table>");
            sb.Append("</center>");

            if (blocks.Count > 0)
            {
                int applicantsRated = 0;

                applicantsRated = (from b in blocks where b.ApplicantExamStatus == ratedExamStatus.StatusId select b).Count();

                sb.Append("<div style='height: 17px;'>&nbsp;</div>");
                sb.Append("<span class='InputLabel' style='margin-left: 500px'>Брой класирани кандидати: </span><span class='InputField'>" + applicantsRated.ToString() + "</span>");
            }

            return sb.ToString();
        }

        // saves all position by ajax request, if needed
        private void JSSaveExams()
        {
            string response = "";

            bool result = true;

            int positionsCount = int.Parse(Request.Params["PositionsCount"]);
            int examsCount = int.Parse(Request.Params["ExamsCount"]);

            int vacancyAnnounceID = int.Parse(Request.Params["VacancyAnnounceID"]);
            int responsibleMilitaryUnitID = int.Parse(Request.Params["ResponsibleMilitaryUnitID"]);
            string orderNumDate = Request.Params["OrderNumDate"].ToString();
            string responsibleMilitaryUnitName = Request.Params["ResponsibleMilitaryUnitName"].ToString();

            List<ApplicantExamsBlock> oldExams = ApplicantExamMarkUtil.GetAllApplicantExamsBlock(vacancyAnnounceID, responsibleMilitaryUnitID, CurrentUser);

            List<ApplicantExamsBlock> newExams = new List<ApplicantExamsBlock>();

            for (int i = 1; i <= positionsCount; i++)
            {
                int applicantID = int.Parse(Request.Params["ApplicantID" + i.ToString()]);
                int statusID = int.Parse(Request.Params["ExamStatusID" + i.ToString()]);

                ApplicantExamsBlock exam = new ApplicantExamsBlock(CurrentUser);
                exam.ApplicantId = applicantID;
                exam.ApplicantExamStatus = (statusID == int.Parse(ListItems.GetOptionChooseOne().Value) ? (int?)null : statusID);
                exam.Marks = new List<ApplicantExamMarkBlock>();

                for (int j = 1; j <= examsCount; j++)
                {
                    int applicantExamMarkID = int.Parse(Request.Params["ApplicantExamMarkID" + i.ToString() + "_" + j.ToString()]);
                    int vacancyAnnounceExamID = int.Parse(Request.Params["VacancyAnnounceExamID" + i.ToString() + "_" + j.ToString()]);
                    int? mark = !string.IsNullOrEmpty(Request.Params["Mark" + i.ToString() + "_" + j.ToString()]) ? (int?)Math.Round(decimal.Parse(Request.Params["Mark" + i.ToString() + "_" + j.ToString()])) : null;
                    int? points = !string.IsNullOrEmpty(Request.Params["Points" + i.ToString() + "_" + j.ToString()]) ? (int?)Math.Round(decimal.Parse(Request.Params["Points" + i.ToString() + "_" + j.ToString()])) : null;

                    ApplicantExamMarkBlock examMark = new ApplicantExamMarkBlock();
                    examMark.ApplicantExamMarkId = applicantExamMarkID;
                    examMark.VacancyAnnounceExamId = vacancyAnnounceExamID;
                    examMark.Mark = mark;
                    examMark.Points = points;

                    exam.Marks.Add(examMark);
                }

                newExams.Add(exam);
            }

            List<ApplicantExamsBlock> newExamsToSave = (from n in newExams
                                                        join o in oldExams on n.ApplicantId equals o.ApplicantId
                                                        where (n.ApplicantExamStatus != o.ApplicantExamStatus) || 
                                                              ((from m in n.Marks where m.ApplicantExamMarkId == 0 select m).Count() > 0) ||
                                                              ((from m1 in n.Marks join m2 in o.Marks on m1.ApplicantExamMarkId equals m2.ApplicantExamMarkId where (m1.Mark != m2.Mark) || (m1.Points != m2.Points) select m1).Count() > 0)
                                                        select n).ToList();

            List<ApplicantExamsBlock> oldExamsToSave = (from o in oldExams
                                                        join n in newExamsToSave on o.ApplicantId equals n.ApplicantId
                                                        select o).ToList();

            if (newExamsToSave.Count > 0)
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                result = ApplicantExamMarkUtil.SaveApplicantExamsBlocks(oldExamsToSave, newExamsToSave, orderNumDate, responsibleMilitaryUnitName, CurrentUser, change);

                change.WriteLog();
            }

            VacancyAnnounceUtil.SetVacancyAnnounceStatusFlow(vacancyAnnounceID, "EXAM", CurrentUser);

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

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
