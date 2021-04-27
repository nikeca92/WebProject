﻿using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.PMISAdmin.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.PMISAdmin.PrintContentPages
{
    public partial class PrintFailedLogins : AdmPage
    {
        const string All = "Всички";

        string user = "";
        string modules = "";
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        
        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "705";
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
            if (GetUIItemAccessLevel("ADM_FAILEDLOGINS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["Username"]))
                {
                    user = Request.Params["Username"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Modules"]))
                {
                    modules = Request.Params["Modules"];
                }

                if (!String.IsNullOrEmpty(Request.Params["DateFrom"]))
                {
                    dateFrom = CommonFunctions.ParseDate(Request.Params["DateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["DateTo"]))
                {
                    dateTo = CommonFunctions.ParseDate(Request.Params["DateTo"]);
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
            FailedLoginsFilter filter = new FailedLoginsFilter()
            {
                Username = user,
                Modules = modules,
                DateFrom = dateFrom,
                DateTo = dateTo,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<FailedLogin> failedLogins = FailedLoginUtil.GetAllFailedLogins(filter, 0, CurrentUser);

            string modulesText = All;

            if (!String.IsNullOrEmpty(modules))
            {
                modulesText = "";

                List<Module> selectedModules = ModuleUtil.GetModulesByIDs(CurrentUser, modules);

                foreach (Module module in selectedModules)
                {
                    modulesText += (String.IsNullOrEmpty(modulesText) ? "" : ",<br/>") + module.ModuleName;
                }
            }


            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 165px; vertical-align: top;'>
                                    <span class='Label'>Потребител:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 200px; vertical-align: top;'>
                                   <span class='ValueLabel'>" + user + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 130px; vertical-align: top;'>
                                    <span class='Label'>Модул:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px; vertical-align: top;'>
                                    <span class='ValueLabel'>" + modulesText + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td style='text-align: right;'>
                                   <span class='Label'>Дата от:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + CommonFunctions.FormatDate(dateFrom) + @"</span>&nbsp;&nbsp;
                                   <span class='Label'>до:</span>
                                   <span class='ValueLabel'>" + CommonFunctions.FormatDate(dateTo) + @"</span>
                                </td>
                             </tr>
                             ";

            if (failedLogins.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 140px;'>Потребител</th>
                                <th style='width: 160px;'>Модул</th>
                                <th style='width: 160px;'>IP адрес</th>
                                <th style='width: 140px;'>Дата</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (FailedLogin failedLogin in failedLogins)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + failedLogin.Username + @"</td>
                            <td align='left'>" + failedLogin.Module.ModuleName + @"</td>
                            <td align='left'>" + failedLogin.IP + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDateTime(failedLogin.DateTime) + @"</td>
                          </tr>";
                counter++;
            }

            if (failedLogins.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=FailedLogins.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            FailedLoginsFilter filter = new FailedLoginsFilter()
            {
                Username = user,
                Modules = modules,
                DateFrom = dateFrom,
                DateTo = dateTo,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<FailedLogin> failedLogins = FailedLoginUtil.GetAllFailedLogins(filter, 0, CurrentUser);

            string modulesText = All;

            if (!String.IsNullOrEmpty(modules))
            {
                modulesText = "";

                List<Module> selectedModules = ModuleUtil.GetModulesByIDs(CurrentUser, modules);

                foreach (Module module in selectedModules)
                {
                    modulesText += (String.IsNullOrEmpty(modulesText) ? "" : ",<br/>") + module.ModuleName;
                }
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 2em;'>Администратор</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.3em;'>Потребителски сесии</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: normal;'>Потребител:&nbsp;</span>
                                        </td>
                                        <td align='left'>
                                            <span style='font-weight: bold;'>" + user + @"</span>
                                        </td>
                                        <td align='right' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: normal;'>Модул:&nbsp;</span>
                                        </td>
                                        <td align='left'>
                                            <span style='font-weight: bold;'>" + modulesText + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td align='right'>
                                            <span style='font-weight: normal;'>Дата от:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='4'>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateFrom) + @"</span>
                                            &nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateTo) + @"</span>
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
                                </table>";


            if (failedLogins.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Потребител</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Модул</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>IP адрес</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (FailedLogin failedLogin in failedLogins)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + failedLogin.Username + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + failedLogin.Module.ModuleName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + failedLogin.IP + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDateTime(failedLogin.DateTime) + @"</td>
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
