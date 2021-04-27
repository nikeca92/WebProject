using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportSV1 : RESPage
    {
        private string sessionResultsKey = "ReportSV1Result";

        const string All = "Всички";
        
        string postCode = "";
        string regionId = "";
        string municipalityId = "";
        string cityId = "";
        string districtId = "";
        string address = "";

        string militaryDepartmentId = "";
        string militaryForceSortId = "";
        string militaryReportSpecialityId = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1264";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "305";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryForceSortId"]))
                {
                    militaryForceSortId = Request.Params["MilitaryForceSortId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryReportSpecialityId"]))
                {
                    militaryReportSpecialityId = Request.Params["MilitaryReportSpecialityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["PostCode"]))
                {
                    postCode = Request.Params["PostCode"];
                }

                if (!String.IsNullOrEmpty(Request.Params["RegionId"]))
                {
                    regionId = Request.Params["RegionId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MunicipalityId"]))
                {
                    municipalityId = Request.Params["MunicipalityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["CityId"]))
                {
                    cityId = Request.Params["CityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["DistrictId"]))
                {
                    districtId = Request.Params["DistrictId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Address"]))
                {
                    address = Request.Params["Address"];
                }

                if (!IsPostBack)
                {
                    if (Request.Params["Export"] != null && Request.Params["Export"].ToLower() == "true")
                    {
                        btnGenerateExcel_Click(this, new EventArgs());
                    }
                    else
                    {
                        this.divResults.InnerHtml = GeneratePageContent(false);
                    }
                }               
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
            ReportSV1Filter filter = new ReportSV1Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryForceSortIds = militaryForceSortId,
                MilitaryReportSpecialityIds = militaryReportSpecialityId,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                PostCode = postCode,
                Address = address,
                PageSize = -1,
                PageIdx = -1
            };

            ReportSV1Result reportReusult = null;

            if (Session[sessionResultsKey] != null)
                reportReusult = (ReportSV1Result)Session[sessionResultsKey];
            else
                reportReusult = ReportSV1Util.GetReportSV1(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if(militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryForceSortName = All;
            int milForceSortId = 0;
            if (int.TryParse(militaryForceSortId, out milForceSortId))
            {
                MilitaryForceSort milForceSort = null;
                milForceSort = MilitaryForceSortUtil.GetMilitaryForceSort(milForceSortId, CurrentUser);

                if (milForceSort != null)
                    militaryForceSortName = milForceSort.MilitaryForceSortName;
            }

            string militaryReportSpecialityName = "";
            List<MilitaryReportSpeciality> militaryReportSepcialities = MilitaryReportSpecialityUtil.GetAllMilitaryReportSpecialitiesByIDs(CurrentUser, militaryReportSpecialityId);
            foreach (MilitaryReportSpeciality militaryReportSpeciality in militaryReportSepcialities)
            {
                militaryReportSpecialityName += ((militaryReportSpecialityName == "" ? "" : ", ") + militaryReportSpeciality.MilReportingSpecialityCode + " - " + militaryReportSpeciality.MilReportingSpecialityName);
            }

            if (militaryReportSpecialityName == "")
            {
                militaryReportSpecialityName = All;
            }

            Region region = null;
            if (!String.IsNullOrEmpty(regionId))
            {
                region = RegionUtil.GetRegion(int.Parse(regionId), CurrentUser);
            }

            Municipality municipality = null;
            if (!String.IsNullOrEmpty(municipalityId))
            {
                municipality = MunicipalityUtil.GetMunicipality(int.Parse(municipalityId), CurrentUser);
            }

            City city = null;
            if (!String.IsNullOrEmpty(cityId))
            {
                city = CityUtil.GetCity(int.Parse(cityId), CurrentUser);
            }

            District district = null;
            if (!String.IsNullOrEmpty(districtId))
            {
                district = DistrictUtil.GetDistrict(int.Parse(districtId), CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px; width: 1264px;'>
                             <tr>
                                <td align='center'>
                                    <span class='Label'>Военно окръжие:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span class='Label'>Род войски:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryForceSortName + @"</span>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span class='Label'>ВОС:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryReportSpecialityName + @"</span>
                                </td>
                             </tr>

                             <tr>
                                <td align='left'>
                                    &nbsp;
                                </td>
                          </tr>
                          <tr>
                            <td align='center' style='padding-left: 100px;'>
                                <table>
                                     <tr>
                                        <td align='right' style='width: 140px;' >Постоянен адрес&nbsp;</td>
                                        <td></td><td></td><td></td>
                                     </tr>
                                     <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span class='Label'>Пощенски код:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 165px;'>
                                            <span class='ValueLabel'>" + postCode + @"</span>
                                        </td>
                                        <td align='right' style='width: 230px;'>
                                            <span class='Label'>Област:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 185px;'>
                                            <span class='ValueLabel'>" + (region != null ? region.RegionName : All) + @"</span>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span class='Label'>Населено място:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 165px;'>
                                            <span class='ValueLabel'>" + (city != null ? city.CityName : All) + @"</span>
                                        </td>
                                        <td align='right' style='width: 230px;'>
                                            <span class='Label'>Община:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 185px;'>
                                            <span class='ValueLabel'>" + (municipality != null ? municipality.MunicipalityName : All) + @"</span>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span class='Label'>Адрес:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 165px;'>
                                            <span class='ValueLabel'>" + address + @"</span>
                                        </td>
                                        <td align='right' style='width: 230px;'>
                                            <span class='Label'>Район:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 185px;'>
                                            <span class='ValueLabel'>" + (district != null ? district.DistrictName : All) + @"</span>
                                        </td>
                                     </tr>
                                </table>
                            </td>
                          </tr>           
                            ");

            if (reportReusult.AllBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='1' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                               <th style='width: 60px; border-left: 1px solid #000000;' rowspan='2'></th>
                               <th style='width: 100px;' rowspan='2'>Показатели</th>
                               <th style='width: 280px;' colspan='4'>До 35 год.</th>
                               <th style='width: 280px;' colspan='4'>До 45 год.</th>
                               <th style='width: 280px;' colspan='4'>Над 45 год.</th>
                               <th style='width: 280px;' colspan='4'>ОБЩО</th>
                               <th style='width: 80px;' rowspan='2'>Всичко</th>
                            </tr> 
                            <tr>
                               <th style='width: 70px;' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' nowrap='nowrap'>В-ци</th>
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            int prevMilRepSpecialityId = -2;
            int prevMilForceSortId = -2;
            int prevMilForceTypeId = -2;
            int prevMilStructureId = -2;
            int milRepStatusCounter = -2;

            //Iterate through all items and add them into the grid
            foreach (ReportSV1Block reportBlock in reportReusult.AllBlocks)
            {
                //Initialize the milRepStatusCounter variable
                //It depends on if the Grand Total section is displayed on the page
                //because its total line is before the military reporting specialities
                if (milRepStatusCounter == -2)
                {
                    if (reportBlock.RowType == 0)
                        milRepStatusCounter = -1;
                    else
                        milRepStatusCounter = 0;
                }

                string cellStyle = "vertical-align: top; text-align: left;";

                milRepStatusCounter++;

                //When any of the "sections" is changed then reset the milRepStatusCounter
                if (prevMilRepSpecialityId != reportBlock.MilRepSpecialityID)
                {
                    if (prevMilRepSpecialityId != -2)
                        milRepStatusCounter = 1;

                    prevMilRepSpecialityId = reportBlock.MilRepSpecialityID;
                }

                if (prevMilForceSortId != reportBlock.MilitaryForceSortID)
                {
                    if (prevMilForceSortId != -2)
                        milRepStatusCounter = 1;

                    prevMilForceSortId = reportBlock.MilitaryForceSortID;
                }

                if (prevMilForceTypeId != reportBlock.MilitaryForceTypeID)
                {
                    if (prevMilForceTypeId != -2)
                        milRepStatusCounter = 1;

                    prevMilForceTypeId = reportBlock.MilitaryForceTypeID;
                }

                if (prevMilStructureId != reportBlock.MilitaryStructureID)
                {
                    if (prevMilStructureId != -2)
                        milRepStatusCounter = 1;

                    prevMilStructureId = reportBlock.MilitaryStructureID;
                }

                string milRepStatusCounterStr = milRepStatusCounter.ToString() + " ";

                if (milRepStatusCounter > 6)
                    milRepStatusCounterStr = " - ";

                string firstCols = "";

                if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 1)
                {
                    firstCols = @"<td style='" + cellStyle + @"'>" + (milRepStatusCounter == 1 ? reportBlock.MilRepSpecialityCode : "") + @"</td>
                                  <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                }
                else if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 2)
                {
                    firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за ВОС " + reportBlock.MilRepSpecialityCode + @"</td>";
                }
                else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 1)
                {
                    if (milRepStatusCounter == 1)
                    {
                        html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceSortName + @"</td>
                                  </tr>");

                        counter++;
                    }

                    firstCols = @"<td style='" + cellStyle + @"'></td>
                                  <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                }
                else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 2)
                {
                    firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за рода войски</td>";
                }
                else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 1)
                {
                    if (milRepStatusCounter == 1)
                    {
                        html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceTypeName + @"</td>
                                  </tr>");

                        counter++;
                    }

                    firstCols = @"<td style='" + cellStyle + @"'></td>
                                  <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                }
                else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 2)
                {
                    firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за вида войски</td>";
                }
                else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 1)
                {
                    if (milRepStatusCounter == 1)
                    {
                        html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryStructureName + @"</td>
                                  </tr>");

                        counter++;
                    }

                    firstCols = @"<td style='" + cellStyle + @"'></td>
                                  <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                }
                else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 2)
                {
                    firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за структурата</td>";
                }
                else if (reportBlock.RowType == 1)
                {
                    firstCols = @"<td style='" + cellStyle + @"'></td>
                                  <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                }
                else if (reportBlock.RowType == 0)
                {
                    firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Водят се на отчет</td>";
                }


                html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                             " + firstCols + @"
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Of_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_OfCand_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Ser_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Sol_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Of_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_OfCand_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Ser_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Sol_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Of_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_OfCand_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Ser_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Sol_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Of_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_OfCand_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Ser_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Sol_Cnt + @"</td>
                             <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.TotalCnt + @"</td>
                          </tr>");

                counter++;
            }

            if (reportReusult.AllBlocks.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportSV1.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReportSV1Filter filter = new ReportSV1Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryForceSortIds = militaryForceSortId,
                MilitaryReportSpecialityIds = militaryReportSpecialityId,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                PageSize = -1,
                PageIdx = 0
            };

            ReportSV1Result reportReusult = null;

            if (Session[sessionResultsKey] != null)
                reportReusult = (ReportSV1Result)Session[sessionResultsKey];
            else
                reportReusult = ReportSV1Util.GetReportSV1(filter, CurrentUser);

            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if (militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryForceSortName = All;
            int milForceSortId = 0;
            if (int.TryParse(militaryForceSortId, out milForceSortId))
            {
                MilitaryForceSort milForceSort = null;
                milForceSort = MilitaryForceSortUtil.GetMilitaryForceSort(milForceSortId, CurrentUser);

                if (milForceSort != null)
                    militaryForceSortName = milForceSort.MilitaryForceSortName;
            }

            string militaryReportSpecialityName = "";
            List<MilitaryReportSpeciality> militaryReportSepcialities = MilitaryReportSpecialityUtil.GetAllMilitaryReportSpecialitiesByIDs(CurrentUser, militaryReportSpecialityId);
            foreach (MilitaryReportSpeciality militaryReportSpeciality in militaryReportSepcialities)
            {
                militaryReportSpecialityName += ((militaryReportSpecialityName == "" ? "" : ", ") + militaryReportSpeciality.MilReportingSpecialityCode + " - " + militaryReportSpeciality.MilReportingSpecialityName);
            }

            if (militaryReportSpecialityName == "")
            {
                militaryReportSpecialityName = All;
            }

            Region region = null;
            if (!String.IsNullOrEmpty(regionId))
            {
                region = RegionUtil.GetRegion(int.Parse(regionId), CurrentUser);
            }

            Municipality municipality = null;
            if (!String.IsNullOrEmpty(municipalityId))
            {
                municipality = MunicipalityUtil.GetMunicipality(int.Parse(municipalityId), CurrentUser);
            }

            City city = null;
            if (!String.IsNullOrEmpty(cityId))
            {
                city = CityUtil.GetCity(int.Parse(cityId), CurrentUser);
            }

            District district = null;
            if (!String.IsNullOrEmpty(districtId))
            {
                district = DistrictUtil.GetDistrict(int.Parse(districtId), CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
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
                                    <tr><td align='center' colspan='19' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='19' style='font-weight: bold; font-size: 2em;'>Отчет на ресурсите от резерва</td></tr>
                                    <tr><td colspan='19'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='19' style='font-weight: bold; font-size: 1.3em;'>Отчетна ведомост за състоянието на ресурсите</td></tr>
                                    <tr><td colspan='19'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='19'>
                                            <span style='font-weight: normal;'>Военно окръжие:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Род войски:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryForceSortName + @"</span>&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>ВОС:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryReportSpecialityName + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='7'>
                                            <span style='font-weight: normal;'>Посоянен адрес&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='7'>
                                            <span style='font-weight: normal;'>Пощенски код:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + postCode + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Област:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (region != null ? region.RegionName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='7'>
                                            <span style='font-weight: normal;'>Населено място:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (city != null ? city.CityName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Община:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (municipality != null ? municipality.MunicipalityName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='7'>
                                            <span style='font-weight: normal;'>Адрес:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + address + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Район:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (district != null ? district.DistrictName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='19'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reportReusult.AllBlocks.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                               <th class='TableHeaderCell' style='width: 80px;' rowspan='2'></th>
                               <th class='TableHeaderCell' style='width: 80px;' rowspan='2'>Показатели</th>
                               <th class='TableHeaderCell' style='width: 280px;' colspan='4'>До 35 год.</th>
                               <th class='TableHeaderCell' style='width: 280px;' colspan='4'>До 45 год.</th>
                               <th class='TableHeaderCell' style='width: 280px;' colspan='4'>Над 45 год.</th>
                               <th class='TableHeaderCell' style='width: 280px;' colspan='4'>ОБЩО</th>
                               <th class='TableHeaderCell' style='width: 80px;' rowspan='2'>Всичко</th>
                            </tr> 
                            <tr>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф. к-ти</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Серж.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>В-ци</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф. к-ти</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Серж.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>В-ци</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф. к-ти</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Серж.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>В-ци</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Оф. к-ти</th>
                               <th class='TableHeaderCell' style='width: 70px;'>Серж.</th>
                               <th class='TableHeaderCell' style='width: 70px;'>В-ци</th>
                            </tr>
                        </thead><tbody>");

                int counter = 1;
                int prevMilRepSpecialityId = -2;
                int prevMilForceSortId = -2;
                int prevMilForceTypeId = -2;
                int prevMilStructureId = -2;
                int milRepStatusCounter = -2;

                //Iterate through all items and add them into the grid
                foreach (ReportSV1Block reportBlock in reportReusult.AllBlocks)
                {
                    //Initialize the milRepStatusCounter variable
                    //It depends on if the Grand Total section is displayed on the page
                    //because its total line is before the military reporting specialities
                    if (milRepStatusCounter == -2)
                    {
                        if (reportBlock.RowType == 0)
                            milRepStatusCounter = -1;
                        else
                            milRepStatusCounter = 0;
                    }

                    string cellStyle = "vertical-align: top;";

                    milRepStatusCounter++;

                    //When any of the "sections" is changed then reset the milRepStatusCounter
                    if (prevMilRepSpecialityId != reportBlock.MilRepSpecialityID)
                    {
                        if (prevMilRepSpecialityId != -2)
                            milRepStatusCounter = 1;

                        prevMilRepSpecialityId = reportBlock.MilRepSpecialityID;
                    }

                    if (prevMilForceSortId != reportBlock.MilitaryForceSortID)
                    {
                        if (prevMilForceSortId != -2)
                            milRepStatusCounter = 1;

                        prevMilForceSortId = reportBlock.MilitaryForceSortID;
                    }

                    if (prevMilForceTypeId != reportBlock.MilitaryForceTypeID)
                    {
                        if (prevMilForceTypeId != -2)
                            milRepStatusCounter = 1;

                        prevMilForceTypeId = reportBlock.MilitaryForceTypeID;
                    }

                    if (prevMilStructureId != reportBlock.MilitaryStructureID)
                    {
                        if (prevMilStructureId != -2)
                            milRepStatusCounter = 1;

                        prevMilStructureId = reportBlock.MilitaryStructureID;
                    }

                    string milRepStatusCounterStr = milRepStatusCounter.ToString() + " ";

                    if (milRepStatusCounter > 6)
                        milRepStatusCounterStr = " - ";

                    string firstCols = "";

                    if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 1)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"'>" + (milRepStatusCounter == 1 ? reportBlock.MilRepSpecialityCode : "") + @"</td>
                                      <td class='TableDataCell' style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"' colspan='2'>Всичко за ВОС " + reportBlock.MilRepSpecialityCode + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html.Append(@"<tr>
                                        <td class='TableDataCell' style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceSortName + @"</td>
                                      </tr>");

                            counter++;
                        }

                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"'></td>
                                      <td class='TableDataCell' style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"' colspan='2'>Всичко за рода войски</td>";
                    }
                    else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html.Append(@"<tr>
                                        <td class='TableDataCell' style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceTypeName + @"</td>
                                      </tr>");

                            counter++;
                        }

                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"'></td>
                                      <td class='TableDataCell' style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"' colspan='2'>Всичко за вида войски</td>";
                    }
                    else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html.Append(@"<tr>
                                        <td class='TableDataCell' style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryStructureName + @"</td>
                                      </tr>");

                            counter++;
                        }

                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"'></td>
                                      <td class='TableDataCell' style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"' colspan='2'>Всичко за структурата</td>";
                    }
                    else if (reportBlock.RowType == 1)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"'></td>
                                      <td class='TableDataCell' style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.RowType == 0)
                    {
                        firstCols = @"<td class='TableDataCell' style='" + cellStyle + @"' colspan='2'>Водят се на отчет</td>";
                    }


                    html.Append(@"<tr>
                                 " + firstCols + @"
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Of_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_OfCand_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Ser_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Sol_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Of_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_OfCand_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Ser_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Sol_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Of_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_OfCand_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Ser_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Sol_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Of_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_OfCand_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Ser_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Sol_Cnt + @"</td>
                                 <td class='TableDataCell' style='" + cellStyle + @" text-align: right;'>" + reportBlock.TotalCnt + @"</td>
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
