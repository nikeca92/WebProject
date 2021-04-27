using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportNormativeTechnics : RESPage
    {
        private string sessionResultsKey = "ReportNormativeTechnicsResult";

        const string All = "Всички";

        string militaryDepartmentId = "";
        bool isOwnershipAddress = true;
        string postCode = "";
        string regionId = "";
        string municipalityId = "";
        string cityId = "";
        string districtId = "";
        string address = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1820";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "655";
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
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTNormativeTechnics") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }



                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["IsOwnershipAddress"]))
                {
                    isOwnershipAddress = Request.Params["IsOwnershipAddress"] == "1";
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
            ReportNormativeTechnicsFilter filter = new ReportNormativeTechnicsFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                IsOwnershipAddress = isOwnershipAddress,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                PageSize = -1,
                PageIdx = -1
            };

            ReportNormativeTechnicsResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportNormativeTechnicsResult)Session[sessionResultsKey];
            else
                reportResult = ReportNormativeTechnicsUtil.GetReportNormativeTechnics(filter, CurrentUser);


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
            html.Append(@"<table style='padding: 5px; padding-left: 0px;'>
                          <tr>
                              <td align='center' style='padding-left: 100px;'>
                                  <span class='Label'>Военно окръжие:&nbsp;</span>
                                  <span class='ValueLabel'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;&nbsp;                                    
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
                                        <td align='left' colspan='4' >
                                            <span class='Label'>" + (isOwnershipAddress ? "Адрес на собственик" : "Адрес по местодомуване") + @"</span>
                                        </td>
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
                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header1Cells)
                {
                    cellIndex++;

                    string style = "";

                    if (cellIndex == 1)
                    {
                        style = "width: 50px;";
                    }
                    if (cellIndex == 2)
                    {
                        style = "width: 340px;";
                    }

                    style += " text-align: center;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow1 += "<th " + span + "><div style='" + style + "'>" + headerCell.Label + "</div></th>";
                }

                cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header2Cells)
                {
                    cellIndex++;

                    string style = "";

                    style = "word-wrap: break-word;";
                    style += " text-align: center;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow2 += "<th" + span + "><div style='" + style + "'>" + headerCell.Label + "</div></th>";
                }

                cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header3Cells)
                {
                    cellIndex++;

                    string style = "";

                    style = "word-wrap: break-word; width: 70px;";
                    style += " text-align: center;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow3 += "<th" + span + "><div style='" + style + "'>" + headerCell.Label + "</div></th>";
                }

                //Setup the header of the grid
                html.Append(@"<tr><td colspan='1' align='center'>
                              <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
                              <tbody>");

                ArrayList blocks = reportResult.Rows;

                int counter = 0;

                foreach (string[] row in blocks)
                {
                    counter++;

                    html.Append(@"<tr>
                                 ");

                    int dataCellIndex = 0;

                    foreach (string cell in row)
                    {
                        dataCellIndex++;

                        string cellValue = cell;
                        string style = "";

                        if (dataCellIndex != 2)
                        {
                            style = "text-align: right;";
                        }

                        html.Append("<td style='" + style + "'>" + cellValue + "</td>");
                    }

                    html.Append("</tr>");
                }

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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportNormativeTechnics.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {

            ReportNormativeTechnicsFilter filter = new ReportNormativeTechnicsFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                IsOwnershipAddress = isOwnershipAddress,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                PageSize = -1,
                PageIdx = -1
            };

            ReportNormativeTechnicsResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportNormativeTechnicsResult)Session[sessionResultsKey];
            else
                reportResult = ReportNormativeTechnicsUtil.GetReportNormativeTechnics(filter, CurrentUser);


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

            int headerColspan = 0;
            foreach (ReportTableHeaderCell cell in reportResult.Header1Cells)
                headerColspan += cell.ColSpan;

            int headerLeftColspan = headerColspan;

            if (headerColspan > 1)
                headerLeftColspan = headerColspan / 2;

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
                                  <thead>
                                    <tr><td align='center' colspan='" + headerColspan.ToString() + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + headerColspan.ToString() + @"' style='font-weight: bold; font-size: 2em;'>Отчет на ресурсите от резерва</td></tr>
                                    <tr><td colspan='" + headerColspan.ToString() + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + headerColspan.ToString() + @"' style='font-weight: bold; font-size: 1.3em;'>Отчетна ведомост за състоянието на техниката</td></tr>
                                    <tr><td colspan='" + headerColspan.ToString() + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + headerColspan.ToString() + @"'>
                                            <span style='font-weight: normal;'>Военно окръжие:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + headerColspan + @"'>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + headerColspan.ToString() + @"'>
                                            <table>
                                                <tr>
                                                    <td align='right' colspan='7'>
                                                        <span style='font-weight: normal;'>" + (isOwnershipAddress ? "Адрес на собственик" : "Адрес по местодомуване") + @"</span>
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
                                            </table>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan='" + headerColspan.ToString() + @"'>&nbsp;</td>
                                    </tr>
                                ");

            if (reportResult.Rows.Count > 0)
            {
                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header1Cells)
                {
                    cellIndex++;

                    string style = "";

                    if (cellIndex == 1)
                    {
                        style = "width: 50px;";
                    }
                    else if (cellIndex == 2)
                    {
                        style = "width: 340px;";
                    }
                    else 
                    {
                        style = "width: " + ((int)(100 * headerCell.ColSpan)).ToString() + "px;";
                    }

                    style += " text-align: center;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow1 += "<th class='TableHeaderCell' " + span + " style='" + style + "' >" + headerCell.Label + "</th>";
                }

                cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header2Cells)
                {
                    cellIndex++;

                    string style = "";

                    style = "word-wrap: break-word; text-align: center;";
                    style += " width: " + ((int)(100 * headerCell.ColSpan)).ToString() + "px;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow2 += "<th class='TableHeaderCell' " + span + " style='" + style + "'>" + headerCell.Label + "</th>";
                }

                cellIndex = 0;
                foreach (ReportTableHeaderCell headerCell in reportResult.Header3Cells)
                {
                    cellIndex++;

                    string style = "";

                    style = "word-wrap: break-word; width: 70px; text-align: center;";

                    string span = " colspan='" + headerCell.ColSpan + "' rowspan='" + headerCell.RowSpan + "' ";

                    headerRow3 += "<th class='TableHeaderCell' " + span + " style='" + style + "'>" + headerCell.Label + "</th>";
                }

                //Setup the header of the grid
                html.Append(@"<tr>
                              " + headerRow1.ToString() + @"
                              </tr> 
                              <tr>
                              " + headerRow2.ToString() + @"
                              </tr>                            
                              <tr>
                              " + headerRow3.ToString() + @"
                              </tr>
                              </thead>
                              <tbody>");

                ArrayList blocks = reportResult.Rows;

                int counter = 0;

                foreach (string[] row in blocks)
                {
                    counter++;

                    html.Append(@"<tr>
                                 ");

                    int dataCellIndex = 0;

                    foreach (string cell in row)
                    {
                        dataCellIndex++;

                        string cellValue = cell;
                        string style = "";

                        if (dataCellIndex != 2)
                        {
                            style = "text-align: right;";
                        }

                        html.Append("<td class='TableDataCell' style='" + style + "'>" + cellValue + "</td>");
                    }

                    html.Append("</tr>");
                }

                html.Append("</tbody>");
            }
            else
            {
                html.Append("</thead>");
            }


            html.Append("</table></body></html>");

            return html.ToString();
        }
    }
}
