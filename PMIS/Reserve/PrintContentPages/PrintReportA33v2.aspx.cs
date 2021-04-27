﻿using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportA33v2 : RESPage
    {
        private string sessionResultsKey = "ReportA33v2Result";

        const string All = "Всички";

        string postCode = "";
        string regionId = "";
        string municipalityId = "";
        string cityId = "";
        string districtId = "";
        string address = "";
   
        string militaryDepartmentId = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1324";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "400";
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
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
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
            ReportA33v2Filter filter = new ReportA33v2Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                PostCode = postCode,
                Address = address,
                PageSize = -1,
                PageIdx = -1
            };

            ReportA33v2Result reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportA33v2Result)Session[sessionResultsKey];
            else
                reportResult = ReportA33v2Util.GetReportA33v2(filter, CurrentUser);


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
            html.Append(@"<table style='padding: 5px; padding-left: 0px; width: 1324px;'>
                             <tr>
                                <td align='center' style='padding-left: 100px;'>
                                    <span class='Label'>Военно окръжие:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;&nbsp;                                    
                                </td>
                             </tr>

                             <tr>
                                <td align='left'>&nbsp;</td>
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

            if (reportResult.Rows.Count > 0)
            {
                TechnicsType vehiclesTechnicsType = TechnicsTypeUtil.GetTechnicsType("VEHICLES", CurrentUser);
                int vehicleKindsCnt = GTableItemUtil.GetAllGTableItemsCountByTableName("VehicleKind", ModuleKey, CurrentUser);

                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in reportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 2)
                    {
                        headerRow1 += "<th style='width: " + (cellIndex == 2 ? "260" : "50") + @"px;' rowspan='3'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 3)
                        {
                            headerRow1 += "<th colspan='5'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            headerRow2 += "<th style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                        else if (cellIndex == 8)
                        {
                            int colspan = reportResult.HeaderCells.Count() - 5;
                            headerRow1 += "<th colspan='" + colspan.ToString() + "'>ТЕХНИКА</th>";
                        }


                        string headerCellNoWrap = "";
                        if (cellIndex >= 3 && cellIndex <= 6)
                            headerCellNoWrap = "nowrap='nowrap'";

                        if (cellIndex > 6 && cellIndex <= 6 + vehicleKindsCnt)
                        {
                            if (cellIndex == 7)
                                headerRow2 += "<th colspan='" + vehicleKindsCnt.ToString() + "'>" + vehiclesTechnicsType.TypeName + "</th>";

                            headerRow3 += "<th style='width: 60px;' " + headerCellNoWrap + ">" + headerCell + "</th>";
                        }
                        else
                        {
                            headerRow2 += "<th style='width: 60px;' " + headerCellNoWrap + " rowspan='2'>" + headerCell + "</th>";
                        }

                        if (cellIndex == reportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                    }
                }

                cellIndex++;
                cellIndex++;

                //Setup the header of the grid
                html.Append(@"<tr><td colspan='1' align='center'>
                         <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>
                         </thead>
                         <tbody>
                         ");
            }

            List<ReportA33v2Row> blocks = reportResult.Rows;

            foreach (ReportA33v2Row row in blocks)
            {
                html.Append(@"<tr>");

                int count = 0;
                int totalHR = 0;
                int totalTech = 0;

                bool emptyRowNumber = String.IsNullOrEmpty(row.ColumnValues[0]);

                foreach (string cell in row.ColumnValues)
                {
                    string cellValue = cell;

                    count++;

                    if (!row.IsEmptyRow)
                    {
                        if (count == 3 || count == 4 || count == 5 || count == 6)
                        {
                            totalHR += int.Parse(cellValue);
                        }
                        else if (count == 7)
                        {
                            html.Append("<td>" + totalHR.ToString() + "</td>");

                            totalHR = 0;
                        }

                        if (count >= 7 && count <= row.ColumnValues.Count())
                        {
                            totalTech += int.Parse(cellValue);
                        }
                    }

                    if (count == 1 && emptyRowNumber)
                    {
                    }
                    else if (count == 2 && emptyRowNumber)
                    {
                        html.Append("<td colspan='2'>" + cellValue + "</td>");
                    }
                    else if (!row.IsEmptyRow || count <= 2)
                    {
                        string style = "";
                        if (count == 1 && (cellValue.Split('.').Length - 1) <= 2)
                            style = "font-weight: bold;";

                        html.Append("<td style='" + style + "'>" + cellValue + "</td>");

                        if (count == row.ColumnValues.Count())
                        {
                            html.Append("<td>" + totalTech.ToString() + "</td>");
                            totalTech = 0;
                        }
                    }
                    else if (row.IsEmptyRow && count == 3)
                    {
                        html.Append("<td colspan='" + ((int)row.ColumnValues.Count() - 2 + 2).ToString() + "'></td>");
                    }
                }

                html.Append("</tr>");
            }

            if (reportResult.Rows.Count > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportA33v2.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {

            ReportA33v2Filter filter = new ReportA33v2Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                PostCode = postCode,
                Address = address,
                PageSize = -1,
                PageIdx = -1
            };

            ReportA33v2Result reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportA33v2Result)Session[sessionResultsKey];
            else
                reportResult = ReportA33v2Util.GetReportA33v2(filter, CurrentUser);


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
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 2em;'>Отчет на ресурсите от резерва</td></tr>
                                    <tr><td colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 1.3em;'>Сведение-анализ за състоянието на ресурсите от резерва</td></tr>
                                    <tr><td colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>
                                            <span style='font-weight: normal;'>Военно окръжие:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;                                           
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align='right' colspan='10'>
                                            <span style='font-weight: normal;'>Посоянен адрес&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='10'>
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
                                        <td align='right' colspan='10'>
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
                                        <td align='right' colspan='10'>
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

            if (reportResult.Rows.Count > 0)
            {
                TechnicsType vehiclesTechnicsType = TechnicsTypeUtil.GetTechnicsType("VEHICLES", CurrentUser);
                int vehicleKindsCnt = GTableItemUtil.GetAllGTableItemsCountByTableName("VehicleKind", ModuleKey, CurrentUser);

                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in reportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 2)
                    {
                        headerRow1 += "<th class='TableHeaderCell' style='width: " + (cellIndex == 2 ? "260" : "50") + @"px;' rowspan='3'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 3)
                        {
                            headerRow1 += "<th class='TableHeaderCell' colspan='5'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            headerRow2 += "<th class='TableHeaderCell' style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                        else if (cellIndex == 8)
                        {
                            int colspan = reportResult.HeaderCells.Count() - 5;
                            headerRow1 += "<th class='TableHeaderCell' colspan='" + colspan.ToString() + "'>ТЕХНИКА</th>";
                        }


                        string headerCellNoWrap = "";
                        if (cellIndex >= 3 && cellIndex <= 6)
                            headerCellNoWrap = "nowrap='nowrap'";

                        if (cellIndex > 6 && cellIndex <= 6 + vehicleKindsCnt)
                        {
                            if (cellIndex == 7)
                                headerRow2 += "<th class='TableHeaderCell' colspan='" + vehicleKindsCnt.ToString() + "'>" + vehiclesTechnicsType.TypeName + "</th>";

                            headerRow3 += "<th class='TableHeaderCell' style='width: 60px;' " + headerCellNoWrap + ">" + headerCell + "</th>";
                        }
                        else
                        {
                            headerRow2 += "<th class='TableHeaderCell' style='width: 60px;' " + headerCellNoWrap + " rowspan='2'>" + headerCell + "</th>";
                        }

                        if (cellIndex == reportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th class='TableHeaderCell' style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                    }
                }

                //Setup the header of the grid
                html.Append(@"<table>                        
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>
                         </thead>
                         <tbody>
                         ");
            }

            List<ReportA33v2Row> blocks = reportResult.Rows;

            foreach (ReportA33v2Row row in blocks)
            {
                html.Append(@"<tr>");

                int count = 0;
                int totalHR = 0;
                int totalTech = 0;

                bool emptyRowNumber = String.IsNullOrEmpty(row.ColumnValues[0]);

                foreach (string cell in row.ColumnValues)
                {
                    string cellValue = cell;

                    count++;

                    if (!row.IsEmptyRow)
                    {
                        if (count == 3 || count == 4 || count == 5 || count == 6)
                        {
                            totalHR += int.Parse(cellValue);
                        }
                        else if (count == 7)
                        {
                            html.Append("<td class='TableDataCell'>" + totalHR.ToString() + "</td>");

                            totalHR = 0;
                        }

                        if (count >= 7 && count <= row.ColumnValues.Count())
                        {
                            totalTech += int.Parse(cellValue);
                        }
                    }

                    if (count == 1 && emptyRowNumber)
                    {
                    }
                    else if (count == 2 && emptyRowNumber)
                    {
                        html.Append("<td class='TableDataCell' colspan='2'>" + cellValue + "</td>");
                    }
                    else if (!row.IsEmptyRow || count <= 2)
                    {
                        string style = "";
                        if (count == 1 && (cellValue.Split('.').Length - 1) <= 2)
                            style = "font-weight: bold;";

                        if (count == 1)
                            style += "mso-number-format:\"\\@\";";

                        html.Append("<td class='TableDataCell' style='" + style + "'>" + cellValue + "</td>");

                        if (count == row.ColumnValues.Count())
                        {
                            html.Append("<td class='TableDataCell'>" + totalTech.ToString() + "</td>");
                            totalTech = 0;
                        }
                    }
                    else if (row.IsEmptyRow && count == 3)
                    {
                        html.Append("<td class='TableDataCell' colspan='" + ((int)row.ColumnValues.Count() - 2 + 2).ToString() + "'></td>");
                    }
                }

                html.Append("</tr>");
            }

            if (reportResult.Rows.Count > 0)
                html.Append("</tbody></table>");

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
