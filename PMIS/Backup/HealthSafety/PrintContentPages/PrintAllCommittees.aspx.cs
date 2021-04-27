using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintAllCommittees : HSPage
    {
        const string All = "Всички";

        string committeeTypeId = "";
        string militaryForceTypeIds = "";
        string militaryUnitsIds = "";
        int sortBy = 1; // 1 - Default

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["CommitteeTypeIDs"]) && Request.Params["CommitteeTypeIDs"] != "-1")
                {
                    committeeTypeId = Request.Params["CommitteeTypeIDs"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryForceTypesIDs"]) && Request.Params["MilitaryForceTypesIDs"] != "-1")
                {
                    militaryForceTypeIds = Request.Params["MilitaryForceTypesIDs"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitIDs"]) && Request.Params["MilitaryUnitIDs"] != "-1")
                {
                    militaryUnitsIds = Request.Params["MilitaryUnitIDs"];
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
                sb.Append("<td rowspan=\"2\">" + GenerateAllCommitteesHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = GenerateAllCommitteesForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllCommitteesHtml()
        {
            //Get the list of Committees according to the specified filters, order and paging
            List<Committee> committees = CommitteeUtil.GetAllCommittees(committeeTypeId, militaryForceTypeIds, militaryUnitsIds, sortBy, 0, 0, CurrentUser);

            GTableItem committeeType = null;
            if (!string.IsNullOrEmpty(committeeTypeId))
            {
                committeeType = GTableItemUtil.GetTableItem("ComitteeTypes", int.Parse(committeeTypeId), ModuleKey, CurrentUser);
            }

            MilitaryForceType militaryForceType = null;
            if (!string.IsNullOrEmpty(militaryForceTypeIds))
            {
                militaryForceType = MilitaryForceTypeUtil.GetMilitaryForceType(int.Parse(militaryForceTypeIds), CurrentUser);
            }

            MilitaryUnit militaryUnit = null;
            if (!string.IsNullOrEmpty(militaryUnitsIds))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitsIds), CurrentUser);
            }

            string html = @"<table style='padding: 5px; width: 700px;'>                            
                             <tr>
                                <td style='text-align: center; vertical-align: top;' colspan='4'>
                                    <span class='ValueLabel'>" + (committeeType != null ? committeeType.TableValue : All) + @"&nbsp;</span>
                                </td>
                             </tr>
                             <tr>
                                <td style='vertical-align: top; text-align: right;'>
                                    <span class='Label'>Вид ВС:&nbsp;</span>
                                </td>
                                <td align='left' style='vertical-align: top;'>
                                   <span class='ValueLabel'>" + (militaryForceType != null ? militaryForceType.MilitaryForceTypeName : All) + @"</span>
                                </td>
                                <td style='vertical-align: top; text-align: right;'>
                                   <span class='Label'>" + this.MilitaryUnitLabel + @"</span>
                                </td>
                                <td align='left' style='vertical-align: top;'>
                                   <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                             </tr>";

            if (committees.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='protocolsTable' name='protocolsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 20px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 220px;'>Тип</th>
                                <th style='width: 130px;'>Вид ВС</th>
                                <th style='width: 130px; border-right: 1px solid #000000;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (Committee committee in committees)
            {
                html += @"<tr>
                                 <td align='left'>" + counter + @"</td>
                                 <td align='left'>" + (committee.CommitteeType != null ? committee.CommitteeType.TableValue : "&nbsp;") + @"</td>
                                 <td align='left'>" + (committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "&nbsp;") + @"</td>                                 
                                 <td align='left'>" + (committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</td>
                          </tr>";
                counter++;
            }

            if (committees.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=Committees.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllCommitteesForExport()
        {
            //Get the list of Committees according to the specified filters, order and paging
            List<Committee> committees = CommitteeUtil.GetAllCommittees(committeeTypeId, militaryForceTypeIds, militaryUnitsIds, sortBy, 0, 0, CurrentUser);

            GTableItem committeeType = null;
            if (!string.IsNullOrEmpty(committeeTypeId))
            {
                committeeType = GTableItemUtil.GetTableItem("ComitteeTypes", int.Parse(committeeTypeId), ModuleKey, CurrentUser);
            }

            MilitaryForceType militaryForceType = null;
            if (!string.IsNullOrEmpty(militaryForceTypeIds))
            {
                militaryForceType = MilitaryForceTypeUtil.GetMilitaryForceType(int.Parse(militaryForceTypeIds), CurrentUser);
            }

            MilitaryUnit militaryUnit = null;
            if (!string.IsNullOrEmpty(militaryUnitsIds))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitsIds), CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                            <table>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 2em;'>Безопасност на труда</td></tr>
                            <tr><td colspan='4'>&nbsp;</td></tr>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.3em;'>Комитети и групи по условията на труд</td></tr>
                             <tr>                          
                                <td align='center' colspan='4' style='font-weight: bold; vertical-align: top;'>
                                    <span style='font-weight: bold;'>" + (committeeType != null ? committeeType.TableValue : All) + @"&nbsp;</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='center' style='vertical-align: top;' colspan='4'>
                                   <span style='font-weight: normal;'>Вид ВС:&nbsp;</span>                               
                                   <span style='font-weight: bold;'>" + (militaryForceType != null ? militaryForceType.MilitaryForceTypeName : All) + @"</span>
                                   &nbsp;&nbsp;&nbsp;&nbsp;
                                   <span style='font-weight: normal;'>" + this.MilitaryUnitLabel + @":</span>                               
                                   <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>                            
                             </tr>";

            if (committees.Count() > 0)
            {
                html += @"
                    <tr>
                        <td align='center' style='width: 20px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</td>
                        <td align='center' style='width: 220px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Тип</td>
                        <td align='center' style='width: 130px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид ВС</td>
                        <td align='center' style='width: 130px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</td>
                    </tr>";
            }

            int counter = 1;

            foreach (Committee committee in committees)
            {
                html += @"<tr>
                                 <td align='left' style='border: 1px solid black;'>" + counter + @"</td>
                                 <td align='left' style='border: 1px solid black;'>" + (committee.CommitteeType != null ? committee.CommitteeType.TableValue : "&nbsp;") + @"</td>
                                 <td align='left' style='border: 1px solid black;'>" + (committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "&nbsp;") + @"</td>                                 
                                 <td align='left' style='border: 1px solid black;'>" + (committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</td>
                          </tr>";
                counter++;
            }

            html += "</table>";

            html += "</body></html>";

            return html;
        }
    }
}
