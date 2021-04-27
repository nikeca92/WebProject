using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportVacancyAnnounceApplicants : APPLPage
    {
        const string All = "Всички";
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        int vacancyAnnounceId = 0;
        int responsibleMilitaryUnitID = 0;
        
        UIAccessLevel l;

        int visibleColumnsCount = 11;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]))
                {
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vacancyAnnounceId);
                }

                if (!String.IsNullOrEmpty(Request.Params["ResponsibleMilitaryUnitID"])
                    && Request.Params["ResponsibleMilitaryUnitID"] != ListItems.GetOptionAll().Value)
                {
                    int mUnitId = 0;
                    int.TryParse(Request.Params["ResponsibleMilitaryUnitID"], out mUnitId);
                    if (mUnitId > 0)
                    {
                        responsibleMilitaryUnitID = mUnitId;
                    }
                }
                                
                this.GenerateExcel();               
            }
        }

        protected void GenerateExcel()
        {
            string result = this.GenerateContentForExport();
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportVacancyAnnounceApplicants.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportVacancyAnnounceApplicantsFilter filter = new ReportVacancyAnnounceApplicantsFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitID                
            };

            //Get the list of records according to the specified filters and order
            List<ReportVacancyAnnounceApplicantsBlock> listBlocks = ReportVacancyAnnounceApplicantsUtil.GetReportVacancyAnnounceApplicantsSearch(filter, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)filter.VacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (filter.ResponsibleMilitaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitID, CurrentUser);
            }
            
            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' xmlns:x='urn:schemas-microsoft-com:office:excel'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                                <style>
                                   .TableHeaderCell
                                   {
                                       border: solid thin #000000;
                                       vertical-align: bottom;
                                   }

                                   .TableDataCell
                                   {
                                       border: solid thin #000000;
                                   }
                                </style> 
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Кандидати за военна служба по обявен конкурс</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='8'>
                                            <span style='font-weight: bold;'>" + (vacancyAnnounce != null ? vacancyAnnounce.OrderNumOrderDate : "") + @"</span>
                                        </td>                         
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Отговорно поделение:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='8'>
                                            <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                        </td>      
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (listBlocks.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                               <th class='TableHeaderCell' style='width: 320px;' colspan='6'>Разпределение по възраст</th>
                               <th class='TableHeaderCell' style='width: 60px;' rowspan='3'>Били на<br/> военна <br/> служба</th>
                               <th class='TableHeaderCell' style='width: 240px;' colspan='4'>По образование</th>                           
                            </tr>
                            <tr>
                               <th class='TableHeaderCell' style='width: 180px;' colspan='3'>мъже</th>
                               <th class='TableHeaderCell' style='width: 180px;' colspan='3'>жени</th>
                               <th class='TableHeaderCell' style='width: 120px;' colspan='2'>мъже</th>
                               <th class='TableHeaderCell' style='width: 120px;' colspan='2'>жени</th>
                            </tr>
                            <tr>
                               <th class='TableHeaderCell' style='width: 60px;'>до 25г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>до 30г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>над 30г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>до 25г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>до 30г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>над 30г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>висше</th>
                               <th class='TableHeaderCell' style='width: 60px;'>средно</th>
                               <th class='TableHeaderCell' style='width: 60px;'>висше</th>
                               <th class='TableHeaderCell' style='width: 60px;'>средно</th>
                            </tr>
                        </thead>
                    <tbody>";


                foreach (ReportVacancyAnnounceApplicantsBlock block in listBlocks)
                {
                    string cellStyleText = "vertical-align: top;";
                    string cellStyleNumber = @"vertical-align: top; text-align: right; mso-number-format: ""0""";
                    
                    html += @"
                            <tr>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Male_Under25.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Male_Under30.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Male_Over35.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Female_Under25.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Female_Under30.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Age_Female_Over35.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_MilitaryService_Employed.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Education_Male_UniversityDegree.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Education_Male_HighSchoolDegree.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Education_Female_UniversityDegree.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.CntBy_Education_Female_HighSchoolDegree.ToString() + @"</td>
                            </tr>";
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
