using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportDocumentsApplied : APPLPage
    {
        const string All = "Всички";
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        int vacancyAnnounceId = 0;
        int? militaryUnitId = null;
        string position = null;
        int? militaryDepartmentId = null;
        string status = null;

        UIAccessLevel l;

        int visibleColumnsCount = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_APPLIED") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]))
                {
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vacancyAnnounceId);
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitID"])
                    && Request.Params["MilitaryUnitID"] != ListItems.GetOptionAll().Value)
                {
                    int mUnitId = 0;
                    int.TryParse(Request.Params["MilitaryUnitID"], out mUnitId);
                    if (mUnitId > 0)
                    {
                        militaryUnitId = mUnitId;
                    }
                }

                position = Request.Params["Position"];

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

                status = Request.Params["Status"];

                this.GenerateExcel();
            }
        }

        protected void GenerateExcel()
        {
            string result = this.GenerateContentForExport();
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportDocumentsApplied.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportDocumentsAppliedFilter filter = new ReportDocumentsAppliedFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryDepartmentId = militaryDepartmentId                
            };

            //Get the list of records according to the specified filters and order
            List<ReportDocumentsAppliedBlock> listBlocks = ReportDocumentsAppliedUtil.GetReportDocumentsAppliedSearch(filter, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)filter.VacancyAnnounceId, CurrentUser);
            
            MilitaryDepartment militaryDepartment = null;
            if (filter.MilitaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)filter.MilitaryDepartmentId, CurrentUser);
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
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Сведение за подалите документи за военна служба</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='7'>
                                            <span style='font-weight: bold;'>" + (vacancyAnnounce != null ? vacancyAnnounce.OrderNumOrderDate : "") + @"</span>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Място на регистрация:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='7'>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
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
                               <th class='TableHeaderCell' style='width: 180px;' rowspan='2'>ВО</th>
                               <th class='TableHeaderCell' style='width: 120px;' colspan='2' rowspan='2'>кандидати, подали документи за кандидатстване</th>
                               <th class='TableHeaderCell' style='width: 480px;' colspan='7'>кандидати, годни за служба във въоръжените сили - ГСВС </th>                           
                            </tr>
                            <tr>
                               <th class='TableHeaderCell' style='width: 120px;' colspan='2'>кандидати ГСВС</th>
                               <th class='TableHeaderCell' style='width: 120px;' colspan='2'>от тях без военна подготовка</th>
                               <th class='TableHeaderCell' style='width: 180px;' colspan='3'>възраст на кандидатите</th>
                            </tr>
                            <tr>
                               <th class='TableHeaderCell' style='width: 60px;'></th>
                               <th class='TableHeaderCell' style='width: 60px;'>мъже</th>
                               <th class='TableHeaderCell' style='width: 60px;'>жени</th>
                               <th class='TableHeaderCell' style='width: 60px;'>мъже</th>
                               <th class='TableHeaderCell' style='width: 60px;'>жени</th>
                               <th class='TableHeaderCell' style='width: 60px;'>мъже</th>
                               <th class='TableHeaderCell' style='width: 60px;'>жени</th>
                               <th class='TableHeaderCell' style='width: 60px;'>&lt; 25г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>&lt; 30г.</th>
                               <th class='TableHeaderCell' style='width: 60px;'>&gt; 30г.</th>
                            </tr>         
                        </thead>
                    <tbody>";


                foreach (ReportDocumentsAppliedBlock block in listBlocks)
                {
                    string cellStyleTotal = block.RowType > 0 ? " font-weight: bold; " : "";
                    string cellStyleNumber = @" vertical-align: top; text-align: right; mso-number-format: ""0""; ";
                    string cellStyleText = "vertical-align: top; text-align: left;";
                    
                    html += @"
                            <tr>
                                <td class='TableDataCell' style='" + cellStyleText + cellStyleTotal + @"' x:num >" + block.MilitaryDepartment + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_DocumentsApplied_Male.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_DocumentsApplied_Female.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_Male.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_Female.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_NoMilitaryTraining_Male.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_NoMilitaryTraining_Female.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_Age_Under25.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_Age_Under30.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + cellStyleTotal + @"' x:num >" + block.CntBy_MedCertFit_Age_Over30.ToString() + @"</td>
                            </tr>";
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
