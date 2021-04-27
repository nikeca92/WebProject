using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintAllApplicants : APPLPage
    {
        const string All = "Всички";

        int? vacancyAnnounceId = null;
        int? militaryDepartmentId = null;
        string idNumber = "";
        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "770";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "75";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]) 
                    && Request.Params["VacancyAnnounceID"] != ListItems.GetOptionAll().Value)
                {
                    int vAnnounceId = 0;
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vAnnounceId);
                    if (vAnnounceId > 0)
                    {
                        vacancyAnnounceId = vAnnounceId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentID"])
                    && Request.Params["MilitaryDepartmentID"] != ListItems.GetOptionAll().Value)
                {
                    int mDepartmentId = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentID"], out mDepartmentId);
                    if (mDepartmentId > 0)
                    {
                        militaryDepartmentId = mDepartmentId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                if (!String.IsNullOrEmpty(Request.Params["IdentityNumber"]))
                {
                    idNumber = Request.Params["IdentityNumber"];
                }

                this.divResults.InnerHtml = GeneratePageContent(false);
            }
            else
            {
                this.divResults.InnerHtml = "";
            }
        }

        // Generate the page content's html
        private string GeneratePageContent(bool isExport)
        {
            string contentPage = "";

            if (!isExport)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllApplicantsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllApplicantsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllApplicantsHtml()
        {
            ApplicantsSearchFilter filter = new ApplicantsSearchFilter() 
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryDepartmentId = militaryDepartmentId,
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of applicants according to the specified filters and order
            List<ApplicantSearch> applicantSearches = ApplicantSearchUtil.GetAllApplicantsBySearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = null;
            if (vacancyAnnounceId != null)
            {
                vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 130px;'>
                                    <span class='Label'>Заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + (vacancyAnnounce != null ? vacancyAnnounce.OrderNumOrderDate : All) + @"</span>
                                </td>
                                <td align='right' style='width: 160px;'>
                                    <span class='Label'>Място на регистрация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 265px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 160px;'>
                                    <span class='Label'>ЕГН:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 265px;'>
                                    <span class='ValueLabel'>" + (idNumber != "" ? idNumber : All) + @"</span>
                                </td>
                             </tr>";

            if (applicantSearches.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 150px;'>Трите имена</th>
                                <th style='width: 80px;'>ЕГН</th>
                                <th style='width: 150px;'>Заповед №</th>
                                <th style='width: 70px;'>от дата</th>
                                <th style='width: 150px;'>Място на регистрация</th>
                                <th style='width: 90px; border-right: 1px solid #000000;'>Последна актуализация</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (ApplicantSearch applicantSearch in applicantSearches)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + applicantSearch.PersonName + @"</td>
                            <td align='left'>" + applicantSearch.PersonIdentNumber + @"</td>
                            <td align='left'>" + applicantSearch.VacancyAnnounceOrderNumber + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(applicantSearch.VacancyAnnounceOrderDate) + @"</td>
                            <td align='left'>" + applicantSearch.MilitaryDepartmentName + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(applicantSearch.LastModifiedDate) + @"</td>
                          </tr>";
                counter++;
            }

            if (applicantSearches.Count() > 0)
            {
                html += "</tbody></table></td></tr>";
            }

            html += "</table>";

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=Applicants.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllApplicantsForExport()
        {
            ApplicantsSearchFilter filter = new ApplicantsSearchFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryDepartmentId = militaryDepartmentId,
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of applicants according to the specified filters and order
            List<ApplicantSearch> applicantSearches = ApplicantSearchUtil.GetAllApplicantsBySearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = null;
            if (vacancyAnnounceId != null)
            {
                vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='7'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 1.3em;'>Списък на кандидатите по обявен конкурс</td></tr>
                                    <tr><td colspan='7'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='7'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (vacancyAnnounce != null ? vacancyAnnounce.OrderNumOrderDate : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Място на регистрация:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>ЕГН:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (idNumber != "" ? idNumber : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 80px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 70px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 90px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (applicantSearches.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Трите имена</th>
                                <th style='width: 80px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Заповед №</th>
                                <th style='width: 70px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>от дата</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Място на регистрация</th>
                                <th style='width: 90px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Последна актуализация</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (ApplicantSearch applicantSearch in applicantSearches)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + applicantSearch.PersonName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + applicantSearch.PersonIdentNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + applicantSearch.VacancyAnnounceOrderNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(applicantSearch.VacancyAnnounceOrderDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + applicantSearch.MilitaryDepartmentName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(applicantSearch.LastModifiedDate) + @"</td>
                              </tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
