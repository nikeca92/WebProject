using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintAllCadets : APPLPage
    {
        const string All = "Всички";

        int? militarySchoolId = null;
        int? militaryDepartmentId = null;
        int? schoolYear = null;
        string idNumber = "";

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "730";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitarySchoolID"])
                    && Request.Params["MilitarySchoolID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mSchoolId = 0;
                    int.TryParse(Request.Params["MilitarySchoolID"], out mSchoolId);
                    if (mSchoolId > 0)
                    {
                        militarySchoolId = mSchoolId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentID"])
                    && Request.Params["MilitaryDepartmentID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mDepartmentId = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentID"], out mDepartmentId);
                    if (mDepartmentId > 0)
                    {
                        militaryDepartmentId = mDepartmentId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["SchoolYear"])
                    && Request.Params["SchoolYear"] != ListItems.GetOptionChooseOne().Value)
                {
                    int sYear = 0;
                    int.TryParse(Request.Params["SchoolYear"], out sYear);
                    if (sYear > 0)
                    {
                        schoolYear = sYear;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["IdentityNumber"])
                    && Request.Params["IdentityNumber"] != "")
                {
                    idNumber = Request.Params["IdentityNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllCadetsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllCadetsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllCadetsHtml()
        {
            CadetsSearchFilter filter = new CadetsSearchFilter()
            {
                MilitarySchoolId = militarySchoolId,
                MilitaryDepartmentId = militaryDepartmentId,
                SchoolYear = schoolYear,
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of cadets according to the specified filters and order
            List<CadetSearch> cadetSearches = CadetSearchUtil.GetAllCadetsBySearch(filter, 0, CurrentUser);

            MilitarySchool militarySchool = null;
            if (militarySchoolId != null)
            {
                militarySchool = MilitarySchoolUtil.GetMilitarySchool((int)militarySchoolId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 170px;'>
                                    <span class='Label'>Военно училище:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 180px;'>
                                   <span class='ValueLabel'>" + (militarySchool != null ? militarySchool.MilitarySchoolName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 180px;'>
                                    <span class='Label'>Място на регистрация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 180px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Учебна година:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (schoolYear != null ? schoolYear.ToString() : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>ЕГН:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (idNumber != "" ? idNumber : All) + @"</span>
                                </td>
                             </tr>";

            if (cadetSearches.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 140px;'>Трите имена</th>
                                <th style='width: 80px;'>ЕГН</th>
                                <th style='width: 150px;'>Военно училище</th>
                                <th style='width: 60px;'>Учебна година</th>
                                <th style='width: 130px;'>Място на регистрация</th>
                                <th style='width: 100px; border-right: 1px solid #000000;'>Последна актуализация</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (CadetSearch cadetSearch in cadetSearches)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + cadetSearch.PersonName + @"</td>
                            <td align='left'>" + cadetSearch.PersonIdentNumber + @"</td>
                            <td align='left'>" + cadetSearch.MilitarySchoolName + @"</td>
                            <td align='left'>" + cadetSearch.SchoolYear + @"</td>
                            <td align='left'>" + cadetSearch.MilitaryDepartmentName + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(cadetSearch.LastModifiedDate) + @"</td>
                          </tr>";
                counter++;
            }

            if (cadetSearches.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=Cadets.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllCadetsForExport()
        {
            CadetsSearchFilter filter = new CadetsSearchFilter()
            {
                MilitarySchoolId = militarySchoolId,
                MilitaryDepartmentId = militaryDepartmentId,
                SchoolYear = schoolYear,
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of cadets according to the specified filters and order
            List<CadetSearch> cadetSearches = CadetSearchUtil.GetAllCadetsBySearch(filter, 0, CurrentUser);

            MilitarySchool militarySchool = null;
            if (militarySchoolId != null)
            {
                militarySchool = MilitarySchoolUtil.GetMilitarySchool((int)militarySchoolId, CurrentUser);
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
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 1.3em;'>Списък на курсантите</td></tr>
                                    <tr><td colspan='7'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='7'>
                                            <span style='font-weight: normal;'>Военно училище:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militarySchool != null ? militarySchool.MilitarySchoolName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Място на регистрация:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='7'>
                                            <span style='font-weight: normal;'>Учебна година:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (schoolYear != null ? schoolYear.ToString() : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>ЕГН:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (idNumber != "" ? idNumber : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 200px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 90px;'>&nbsp;</td>
                                        <td style='width: 200px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (cadetSearches.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Трите имена</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Военно училище</th>
                                <th style='width: 90px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Учебна година</th>
                                <th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Място на регистрация</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Последна актуализация</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (CadetSearch cadetSearch in cadetSearches)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + cadetSearch.PersonName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + cadetSearch.PersonIdentNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + cadetSearch.MilitarySchoolName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + cadetSearch.SchoolYear + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + cadetSearch.MilitaryDepartmentName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(cadetSearch.LastModifiedDate) + @"</td>
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
