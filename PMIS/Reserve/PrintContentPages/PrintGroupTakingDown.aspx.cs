using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintGroupTakingDown : RESPage
    {
        const string All = "Всички";

        string genderID = "";
        int? age = null;
        string toDate = "";
        string militaryCategoryId = "";
        string militaryRankId = "";
        string administrationID = "";
        string militaryDepartmentId = "";
        string milRepSpecTypeId = "";
        string milRepSpecId = "";
        bool isPrimaryMilRepSpec = false;

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "755";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "55";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["GenderID"]))
                {
                    genderID = Request.Params["GenderID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Age"]))
                {
                    age = int.Parse(Request.Params["Age"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["todate"]))
                {
                    toDate = Request.Params["todate"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCategoryId"]))
                {
                    militaryCategoryId = Request.Params["MilitaryCategoryId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryRankId"]))
                {
                    militaryRankId = Request.Params["MilitaryRankId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["AdministrationID"]))
                {
                    administrationID = Request.Params["AdministrationID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilRepSpecTypeId"]))
                {
                    milRepSpecTypeId = Request.Params["MilRepSpecTypeId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilRepSpecId"]))
                {
                    milRepSpecId = Request.Params["MilRepSpecId"];
                }

                isPrimaryMilRepSpec = (!String.IsNullOrEmpty(Request.Params["IsPrimaryMilRepSpec"]) && Request.Params["IsPrimaryMilRepSpec"] == "1");

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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllRecordsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllRecordsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllRecordsHtml()
        {
            ReservistGroupTakingDownFilter filter = new ReservistGroupTakingDownFilter()
            {
                Age = age,
                ToDate = toDate,
                Gender = genderID,
                MilitaryCategory = militaryCategoryId,
                MilitaryRank = militaryRankId,
                Administration = administrationID,
                MilitaryDepartment = militaryDepartmentId,
                MilRepSpecType = milRepSpecTypeId,
                MilRepSpec = milRepSpecId,
                IsPrimaryMilRepSpec = isPrimaryMilRepSpec,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReservistGroupTakingDownBlock> reservists = ReservistUtil.GetAllReservistGroupTakingDownBlocks(filter, 0, CurrentUser);          

            Gender gender = null;
            if (!String.IsNullOrEmpty(genderID))
            {
                gender = GenderUtil.GetGender(CurrentUser, int.Parse(genderID));
            }

            MilitaryCategory militaryCategory = null;
            if (!String.IsNullOrEmpty(militaryCategoryId))
            {
                militaryCategory = MilitaryCategoryUtil.GetMilitaryCategory(int.Parse(militaryCategoryId), CurrentUser);
            }

            MilitaryRank militaryRank = null;
            if (!String.IsNullOrEmpty(militaryRankId))
            {
                militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationID))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationID), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            MilitaryReportSpecialityType milRepSpecType = null;
            if (!String.IsNullOrEmpty(milRepSpecTypeId))
            {
                milRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milRepSpec = null;
            if (!String.IsNullOrEmpty(milRepSpecId))
            {
                milRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milRepSpecId), CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 260px;'>
                                    <span class='Label'>Пол:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 155px;'>
                                   <span class='ValueLabel'>" + (gender != null ? gender.GenderName : All) + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 110px;'>
                                    <span class='Label'>Възраст:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 205px;'>
                                    <span class='ValueLabel'>" + (age.HasValue ? age.Value.ToString() : All) + @"</span>
                                    <span class='Label'>към дата:&nbsp;</span>
                                    <span class='ValueLabel'>" + (string.IsNullOrEmpty(toDate) ? "" : toDate) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Категория:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (militaryCategory != null ? militaryCategory.CategoryName : All) + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>Категория:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militaryRank != null ? militaryRank.LongName : All) + @"</span>
                                </td> 
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Работил/служил в:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>На отчет в:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td> 
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Тип ВОС:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (milRepSpecType != null ? milRepSpecType.TypeName : All) + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (milRepSpec != null ? milRepSpec.CodeAndName : All) + @"</span>
                                </td> 
                             </tr>
                             <tr>
                                <td align='right'>
                                </td>
                                <td align='left'>                                   
                                </td>
                                <td align='right'>
                                    <span class='Label'>Основна ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (isPrimaryMilRepSpec ? "x" : "") + @"</span>
                                </td> 
                             </tr>
                             ");

            if (reservists.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                               <th style='width: 20px; border-left: 1px solid #000000;'>№</th>
                               <th style='width: 110px;'>Трите имена</th>
                               <th style='width: 110px;'>ЕГН</th>
                               <th style='width: 50px;'>Пол</th>
                               <th style='width: 150px;'>Работил/служил в</th>                               
                               <th style='width: 150px;'>Състояние по отчета</th>
                               <th style='width: 150px; border-right: 1px solid #000000;'>Команда</th>     
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReservistGroupTakingDownBlock reservistGroupTakingDownBlock in reservists)
            {
                html.Append(@"
                          <tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.FullName + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.IdentNumber + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.Gender + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.Administration + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.MilitaryReportStatus + @"</td>
                            <td align='left'>" + reservistGroupTakingDownBlock.MilitaryCommand + @"</td>
                          </tr>");
                counter++;
            }

            if (reservists.Count() > 0)
            {
                html.Append("</tbody></table></td></tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=GroupTakingDown.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReservistGroupTakingDownFilter filter = new ReservistGroupTakingDownFilter()
            {
                Age = age,
                ToDate = toDate,
                Gender = genderID,
                MilitaryCategory = militaryCategoryId,
                MilitaryRank = militaryRankId,
                Administration = administrationID,
                MilitaryDepartment = militaryDepartmentId,
                MilRepSpecType = milRepSpecTypeId,
                MilRepSpec = milRepSpecId,
                IsPrimaryMilRepSpec = isPrimaryMilRepSpec,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReservistGroupTakingDownBlock> reservists = ReservistUtil.GetAllReservistGroupTakingDownBlocks(filter, 0, CurrentUser);

            Gender gender = null;
            if (!String.IsNullOrEmpty(genderID))
            {
                gender = GenderUtil.GetGender(CurrentUser, int.Parse(genderID));
            }

            MilitaryCategory militaryCategory = null;
            if (!String.IsNullOrEmpty(militaryCategoryId))
            {
                militaryCategory = MilitaryCategoryUtil.GetMilitaryCategory(int.Parse(militaryCategoryId), CurrentUser);
            }

            MilitaryRank militaryRank = null;
            if (!String.IsNullOrEmpty(militaryRankId))
            {
                militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationID))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationID), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            MilitaryReportSpecialityType milRepSpecType = null;
            if (!String.IsNullOrEmpty(milRepSpecTypeId))
            {
                milRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milRepSpec = null;
            if (!String.IsNullOrEmpty(milRepSpecId))
            {
                milRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milRepSpecId), CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 2em;'>Резервисти</td></tr>
                                    <tr><td colspan='7'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='7' style='font-weight: bold; font-size: 1.3em;'>Групово снемане от отчет</td></tr>
                                    <tr><td colspan='7'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Пол:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (gender != null ? gender.GenderName : All) + @"</span>                                         
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Възраст:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='1'>
                                            <span style='font-weight: bold;'>" + (age.HasValue ? age.Value.ToString() : All) + @"</span>
                                            <span style='font-weight: normal;'>към дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (string.IsNullOrEmpty(toDate) ? "" : toDate) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Категория:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (militaryCategory != null ? militaryCategory.CategoryName : All) + @"</span>                                           
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Военно звание:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='1'>
                                            <span style='font-weight: bold;'>" + (militaryRank != null ? militaryRank.LongName : All) + @"</span>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Работил/служил в:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (administration != null ? administration.AdministrationName : All) + @"</span>                                           
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>На отчет в:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='1'>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Тип ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milRepSpecType != null ? milRepSpecType.TypeName : All) + @"</span>                                           
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='1'>
                                            <span style='font-weight: bold;'>" + (milRepSpec != null ? milRepSpec.CodeAndName : All) + @"</span>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                        </td>
                                        <td align='left' colspan='2'>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Основна ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='1'>
                                            <span style='font-weight: bold;'>" + (isPrimaryMilRepSpec ? "x" : "") + @"</span>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reservists.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Трите имена</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Пол</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Работил/служил в</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Състояние по отчета</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Команда</th>
                            </tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReservistGroupTakingDownBlock reservistGroupTakingDownBlock in reservists)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.FullName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.IdentNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.Gender + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.Administration + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.MilitaryReportStatus + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reservistGroupTakingDownBlock.MilitaryCommand + @"</td>
                              </tr>");
                    counter++;
                }

                html.Append("</tbody></table>");
            }

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
