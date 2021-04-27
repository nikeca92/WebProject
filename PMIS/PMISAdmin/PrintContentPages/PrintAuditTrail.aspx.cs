using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.PMISAdmin.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.PMISAdmin.PrintContentPages
{
    public partial class PrintAuditTrail : AdmPage
    {
        //This get label name from resourse file
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        const string All = "Всички";

        string users = "";
        string modules = "";
        string changeTypes = "";
        string changeEventTypes = "";
        string militaryUnits = "";
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        string description = "";
        string identNumber = "";
        string oldValue = "";
        string newValue = "";
        
        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1005";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "195";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("ADM_AUDITTRAIL") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["Users"]))
                {
                    users = Request.Params["Users"];

                    string tmp = "," + users + ",";

                    if (tmp.Contains("," + ListItems.GetOptionAll().Value + ","))
                    {
                        users = "";
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["Modules"]))
                {
                    modules = Request.Params["Modules"];

                    string tmp = "," + modules + ",";

                    if (tmp.Contains("," + ListItems.GetOptionAll().Value + ","))
                    {
                        modules = "";
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["ChangeTypes"]))
                {
                    changeTypes = Request.Params["ChangeTypes"];

                    string tmp = "," + changeTypes + ",";

                    if (tmp.Contains("," + ListItems.GetOptionAll().Value + ","))
                    {
                        changeTypes = "";
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["ChangeEventTypes"]))
                {
                    changeEventTypes = Request.Params["ChangeEventTypes"];

                    string tmp = "," + changeEventTypes + ",";

                    if (tmp.Contains("," + ListItems.GetOptionAll().Value + ","))
                    {
                        changeEventTypes = "";
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["DateFrom"]))
                {
                    dateFrom = CommonFunctions.ParseDate(Request.Params["DateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["DateTo"]))
                {
                    dateTo = CommonFunctions.ParseDate(Request.Params["DateTo"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnits"]))
                {
                    militaryUnits = Request.Params["MilitaryUnits"];

                    string tmp = "," + militaryUnits + ",";

                    if (tmp.Contains("," + ListItems.GetOptionAll().Value + ","))
                    {
                        militaryUnits = "";
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["Description"]))
                {
                    description = Request.Params["Description"];
                }

                if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                {
                    identNumber = Request.Params["IdentNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["OldValue"]))
                {
                    oldValue = Request.Params["OldValue"];
                }

                if (!String.IsNullOrEmpty(Request.Params["NewValue"]))
                {
                    newValue = Request.Params["NewValue"];
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
            AuditTrailHistoryFilter filter = new AuditTrailHistoryFilter()
            {
                Users = users,
                Modules = modules,
                ChangeTypes = changeTypes,
                ChangeEventTypes = changeEventTypes,
                MilitaryUnits = militaryUnits,
                DateFrom = dateFrom,
                DateTo = dateTo,
                ObjectDesc = description,
                PersonIdentityNumber = identNumber,
                OldValue = oldValue,
                NewValue = newValue,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<AuditTrailHistoryRec> changes = AuditTrailHistoryUtil.GetAuditTrailHistory(CurrentUser, filter, 0);

            string usersText = All;

            if (!String.IsNullOrEmpty(users))
            {
                usersText = "";


                List<User> selectedUsers = UserUtil.GetUsersByIDs(CurrentUser, users);

                foreach (User user in selectedUsers)
                {
                    usersText += (String.IsNullOrEmpty(usersText) ? "" : ",<br/>") + user.FullName;
                }
            }


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

            string changeTypesText = All;

            if (!String.IsNullOrEmpty(changeTypes))
            {
                changeTypesText = "";

                List<ChangeType> selectedChangeTypes = ChangeTypeUtil.GetChangeTypesByIDs(CurrentUser, changeTypes);

                foreach (ChangeType selectedChangeType in selectedChangeTypes)
                {
                    changeTypesText += (String.IsNullOrEmpty(changeTypesText) ? "" : ",<br/>") + selectedChangeType.ChangeTypeName;
                }
            }

            string changeEventTypesText = All;

            if (!String.IsNullOrEmpty(changeEventTypes))
            {
                changeEventTypesText = "";

                List<ChangeEventType> selectedChangeEventTypes = ChangeEventTypeUtil.GetChangeEventTypesByIDs(CurrentUser, changeEventTypes);

                foreach (ChangeEventType selectedChangeEventType in selectedChangeEventTypes)
                {
                    changeEventTypesText += (String.IsNullOrEmpty(changeEventTypesText) ? "" : ",<br/>") + selectedChangeEventType.ChangeEventTypeName;
                }
            }

            string militaryUnitText = All;

            if (!String.IsNullOrEmpty(militaryUnits))
            {
                militaryUnitText = "";

                List<MilitaryUnit> selectedMilitaryUnits = MilitaryUnitUtil.GetMilitaryUnitsByIDsWithoutChilds(CurrentUser, militaryUnits);

                foreach (MilitaryUnit selectedMilitaryUnit in selectedMilitaryUnits)
                {
                    militaryUnitText += (String.IsNullOrEmpty(militaryUnitText) ? "" : ",<br/>") + selectedMilitaryUnit.DisplayTextForSelection;
                }
            }


            string html = @"<table style='padding: 5px; width: 1005px;'>
                             <tr>
                                <td align='right' style='width: 165px; vertical-align: top;'>
                                    <span class='Label'>Потребител:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 200px; vertical-align: top;'>
                                   <span class='ValueLabel'>" + usersText + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 130px; vertical-align: top;'>
                                    <span class='Label'>Модул:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px; vertical-align: top;'>
                                    <span class='ValueLabel'>" + modulesText + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 130px; vertical-align: top;'>
                                    <span class='Label'>Тип промяна:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px; vertical-align: top;'>
                                    <span class='ValueLabel'>" + changeTypesText + @"</span>
                                </td>
                                <td align='right' style='width: 130px; vertical-align: top;'>
                                    <span class='Label'>Събитие:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px; vertical-align: top;'>
                                    <span class='ValueLabel'>" + changeEventTypesText + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td style='text-align: right;'>
                                   <span class='Label'>" + this.MilitaryUnitLabel + @":</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + militaryUnitText + @"</span>&nbsp;&nbsp;
                                </td>
                                <td style='text-align: right;'>
                                   <span class='Label'>Дата от:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + CommonFunctions.FormatDate(dateFrom) + @"</span>&nbsp;&nbsp;
                                   <span class='Label'>до:</span>
                                   <span class='ValueLabel'>" + CommonFunctions.FormatDate(dateTo) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td style='text-align: right;'>
                                   <span class='Label'>ЕГН:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + identNumber + @"</span>&nbsp;&nbsp;
                                </td>
                                <td style='text-align: right;'>
                                   <span class='Label'>Описание:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + description + @"</span>&nbsp;&nbsp;
                                </td>
                             </tr>
                             <tr>
                                <td style='text-align: right;'>
                                   <span class='Label'>Стара стойност:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + oldValue + @"</span>&nbsp;&nbsp;
                                </td>
                                <td style='text-align: right;'>
                                   <span class='Label'>Нова стойност:</span>
                                </td>
                                <td style='text-align: left;'>
                                   <span class='ValueLabel'>" + newValue + @"</span>&nbsp;&nbsp;
                                </td>
                             </tr>
                             ";

            if (changes.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 130px;'>Потребител</th>
                                <th style='width: 70px;'>Дата</th>
                                <th style='width: 100px;'>IP адрес</th>
                                <th style='width: 120px;'>Модул</th>
                                <th style='width: 100px;'>Тип на промяната</th>
                                <th style='width: 100px;'>Събитие</th>
                                <th style='width: 100px;'>Описание</th>
                                <th style='width: 100px;'>" + this.MilitaryUnitLabel +  @"</th>
                                <th style='width: 130px;'>Човек</th>
                                <th style='width: 230px;'>Детайли на промяната</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (AuditTrailHistoryRec change in changes)
            {
                html += @"<tr>
                            <td align='center'>" + change.RowNumber.ToString() + @"</td>
                            <td align='left'>" + change.UserFullName + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDateTime(change.ChangeDate) + @"</td>
                            <td align='left'>" + change.IP + @"</td>
                            <td align='left'>" + change.ModuleName + @"</td>
                            <td align='left'>" + change.ChangeTypeName + @"</td>
                            <td align='left'>" + change.ChangeEventTypeName + @"</td>
                            <td align='left'>" + change.ObjectDesc + @"</td>
                            <td align='left'>" + change.MilitaryUnitName + @"</td>
                            <td align='left'>" + change.PersonFullName + (change.PIdentityNumber == "" ? "" : " (" + change.PIdentityNumber + ")") + @"</td>
                            <td align='left'>" + change.ChangeDetailsHTML + @"</td>
                          </tr>";
                counter++;
            }

            if (changes.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=AuditTrail.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            AuditTrailHistoryFilter filter = new AuditTrailHistoryFilter()
            {
                Users = users,
                Modules = modules,
                ChangeTypes = changeTypes,
                ChangeEventTypes = changeEventTypes,
                MilitaryUnits = militaryUnits,
                DateFrom = dateFrom,
                DateTo = dateTo,
                ObjectDesc = description,
                PersonIdentityNumber = identNumber,
                OldValue = oldValue,
                NewValue = newValue,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<AuditTrailHistoryRec> changes = AuditTrailHistoryUtil.GetAuditTrailHistory(CurrentUser, filter, 0);

            string usersText = All;

            if (!String.IsNullOrEmpty(users))
            {
                usersText = "";


                List<User> selectedUsers = UserUtil.GetUsersByIDs(CurrentUser, users);

                foreach (User user in selectedUsers)
                {
                    usersText += (String.IsNullOrEmpty(usersText) ? "" : ",<br/>") + user.FullName;
                }
            }


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

            string changeTypesText = All;

            if (!String.IsNullOrEmpty(changeTypes))
            {
                changeTypesText = "";

                List<ChangeType> selectedChangeTypes = ChangeTypeUtil.GetChangeTypesByIDs(CurrentUser, changeTypes);

                foreach (ChangeType selectedChangeType in selectedChangeTypes)
                {
                    changeTypesText += (String.IsNullOrEmpty(changeTypesText) ? "" : ",<br/>") + selectedChangeType.ChangeTypeName;
                }
            }

            string changeEventTypesText = All;

            if (!String.IsNullOrEmpty(changeEventTypes))
            {
                changeEventTypesText = "";

                List<ChangeEventType> selectedChangeEventTypes = ChangeEventTypeUtil.GetChangeEventTypesByIDs(CurrentUser, changeEventTypes);

                foreach (ChangeEventType selectedChangeEventType in selectedChangeEventTypes)
                {
                    changeEventTypesText += (String.IsNullOrEmpty(changeEventTypesText) ? "" : ",<br/>") + selectedChangeEventType.ChangeEventTypeName;
                }
            }

            string militaryUnitText = All;

            if (!String.IsNullOrEmpty(militaryUnits))
            {
                militaryUnitText = "";

                List<MilitaryUnit> selectedMilitaryUnits = MilitaryUnitUtil.GetMilitaryUnitsByIDsWithoutChilds(CurrentUser, militaryUnits);

                foreach (MilitaryUnit selectedMilitaryUnit in selectedMilitaryUnits)
                {
                    militaryUnitText += (String.IsNullOrEmpty(militaryUnitText) ? "" : ",<br/>") + selectedMilitaryUnit.DisplayTextForSelection;
                }
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 2em;'>Администратор</td></tr>
                                    <tr><td colspan='11'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 1.3em;'>Одитни записи</td></tr>
                                    <tr><td colspan='11'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='4' style='vertical-align: top;'>
                                            <span style='font-weight: normal;'>Потребител:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: bold;'>" + usersText + @"</span>
                                        </td>
                                        <td align='right' colspan='1' style='vertical-align: top;'>
                                            <span style='font-weight: normal;'>Модул:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: bold;'>" + modulesText + @"</span>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td align='right' colspan='4' style='vertical-align: top;'>
                                            <span>Тип промяна:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: bold;'>" + changeTypesText + @"</span>
                                        </td>
                                        <td align='right' colspan='1' style='vertical-align: top;'>
                                            <span>Събитие:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: bold;'>" + changeEventTypesText + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='text-align: right; vertical-align: top;' colspan='4'>
                                           <span>" + this.MilitaryUnitLabel + @":</span>
                                        </td>
                                        <td style='text-align: left; vertical-align: top;' colspan='2'>
                                           <span style='font-weight: bold;'>" + militaryUnitText + @"</span>&nbsp;&nbsp;
                                        </td>
                                        <td align='right' colspan='1' style='vertical-align: top;'>
                                            <span style='font-weight: normal;'>Дата от:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2' style='vertical-align: top;'>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateFrom) + @"</span>
                                            &nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateTo) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='text-align: right; vertical-align: top;' colspan='4'>
                                           <span>ЕГН:</span>
                                        </td>
                                        <td style='text-align: left; vertical-align: top;' colspan='2'>
                                           <span style='font-weight: bold;'>" + identNumber + @"</span>&nbsp;&nbsp;
                                        </td>
                                        <td style='text-align: right; vertical-align: top;' colspan='1'>
                                           <span>Описание:</span>
                                        </td>
                                        <td style='text-align: left; vertical-align: top;' colspan='2'>
                                           <span style='font-weight: bold;'>" + description + @"</span>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='text-align: right; vertical-align: top;' colspan='4'>
                                           <span>Стара стойност:</span>
                                        </td>
                                        <td style='text-align: left; vertical-align: top;' colspan='2'>
                                           <span style='font-weight: bold;'>" + oldValue + @"</span>&nbsp;&nbsp;
                                        </td>
                                        <td style='text-align: right; vertical-align: top;' colspan='1'>
                                           <span>Нова стойност:</span>
                                        </td>
                                        <td style='text-align: left; vertical-align-top;' colspan='2'>
                                           <span style='font-weight: bold;'>" + newValue + @"</span>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='11'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (changes.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Потребител</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>IP адрес</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Модул</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Тип на промяната</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Събитие</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Описание</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Човек</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Детайли на промяната</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (AuditTrailHistoryRec change in changes)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + change.RowNumber.ToString() + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.UserFullName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDateTime(change.ChangeDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.IP + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.ModuleName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.ChangeTypeName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.ChangeEventTypeName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.ObjectDesc + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.MilitaryUnitName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.PersonFullName + (change.PIdentityNumber == "" ? "" : " (" + change.PIdentityNumber + ")") + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + change.ChangeDetailsHTML + @"</td>
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

