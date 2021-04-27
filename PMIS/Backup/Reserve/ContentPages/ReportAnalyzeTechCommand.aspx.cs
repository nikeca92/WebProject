using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportAnalyzeTechCommand : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private string sessionResultsKey = "ReportAnalyzeTechCommand";

        private ReportAnalyzeTechCommandResult reportResult = null;
        private ReportAnalyzeTechCommandResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportAnalyzeTechCommandResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportAnalyzeTechCommandFilter filter = CollectFilterData();
                        reportResult = ReportAnalyzeTechCommandUtil.GetReportAnalyzeTechCommand(filter, CurrentUser);
                        Session[sessionResultsKey] = reportResult;
                    }
                }

                return reportResult;
            }
        }

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_REPORTS_REPORTANALYZETECHCOMMAND";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZETECHCOMMAND") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportAnalyzeTechCommand");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                Session[sessionResultsKey] = null;

                //Populate any drop-downs and list-boxes
                PopulateLists();
            
                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            btnRefresh.Attributes.Add("onclick", "SetPickListsSelection();");
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();
            PopulateMilitaryReadiness();
            PopulateMilitaryCommands();
        }

        private void PopulateMilitaryCommands()
        {
            ddMilitaryCommand.Items.Clear();

            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddMilitaryCommand.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryReadinessForTechnics(CurrentUser, hdnMilitaryDepartmentSelected.Value, readinessID);
            ddMilitaryCommand.DataTextField = "DisplayTextForSelection";
            ddMilitaryCommand.DataValueField = "MilitaryCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryCommand_Changed(Page, new EventArgs());
        }

        private void PopulateMilitaryReadiness()
        {
            ddMilitaryReadiness.Items.Clear();

            ddMilitaryReadiness.DataSource = MilitaryReadinessUtil.GetAllMilitaryReadiness(CurrentUser);
            ddMilitaryReadiness.DataTextField = "MilReadinessName";
            ddMilitaryReadiness.DataValueField = "MilReadinessId";
            ddMilitaryReadiness.DataBind();
            ddMilitaryReadiness.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryReadiness.SelectedIndex = 0;
        }

        private void PopulateMilitaryDepartments()
        {
            string result = "";

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                string pickListItem = "{value : '" + militaryDepartment.MilitaryDepartmentId.ToString() + "' , label : '" + militaryDepartment.MilitaryDepartmentName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnMilitaryDepartmentJson.Value = result;
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblMilitaryDepartment.Style.Add("vertical-align", "middle");            
        }        

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";          

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshReport()
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZETECHCOMMAND") == UIAccessLevel.Hidden)
                return;

            string html = "";            

            if (ReportResult.OverallResult != null)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>                               
                               <th style='width: 480px;' colspan='2'>Заявка от формированието</th>
                               <th style='width: 200px;' colspan='2'>Фактическо изпълнение</th>
                               <th style='width: 100px;' rowspan='2'>Процент резерв с МН /ед.т./</th>                                                              
                            </tr> 
                            <tr>
                               <th style='width: 400px;'>Вид и тип на техниката</th>
                               <th style='width: 80px;'>Брой</th>
                               <th style='width: 80px;'>Брой</th>
                               <th style='width: 120px;'>От тях по заменки бр.</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportOverallAnalyzeTechCommandResultBlock block in ReportResult.OverallResult)
                {
                    counter++;
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                                
                                     <td style='" + cellStyle + @" text-align: left;'>" + block.NormativeName.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.RequestedPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.FilledPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.ReplacedPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.ReservePos.ToString() + @"</td>
                                  </tr>";
                }
                html += "</table>";
            }

            if (ReportResult.PositionDeliveryResult != null && ReportResult.PositionDeliveryResult.Count > 0)
            {
                var technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);
                var vehicleKinds = GTableItemUtil.GetAllGTableItemsByTableName("VehicleKind", ModuleKey, 1, 0, 0, CurrentUser).OrderBy(x => x.TableSeq).ToList();
                var readinessTypeColspan = technicsTypes.Count + vehicleKinds.Count;
                var readinessTypes = new int[] { 1, 2 };

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 40px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Мобилизационните ресурсите се доставят от:</div>
                         <table class='CommonHeaderTable'>
                         <thead>
                            <tr>
                               <th style='width: 240px;' rowspan='3'>Община</th>
                               <th style='width: 240px;' rowspan='3'>Район</th>
                               <th style='width: 120px;' colspan='" + readinessTypeColspan + @"'>Основно попълнение</th>
                               <th style='width: 120px;' colspan='" + readinessTypeColspan + @"'>Резерв</th>
                            </tr>
                            <tr>";

                foreach (var readiness in readinessTypes)
                {
                    foreach (var technicsType in technicsTypes)
                    {
                        if(technicsType.TypeKey == "VEHICLES")
                            html += "<th colspan='" + vehicleKinds.Count + @"'>" + technicsType.TypeName + "</th>";
                        else
                            html += "<th style='width: 120px;' rowspan='2'>" + technicsType.TypeName + "</th>";
                    }
                    html += "<th style='width: 120px;' rowspan='2'>Всичко</th>";
                }

                html += @"</tr>
                          <tr>";

                foreach (var readiness in readinessTypes)
                {
                    foreach (var vehicleKind in vehicleKinds)
                    {
                        html += "<th style='width: 120px;'>" + vehicleKind.TableValue + "</th>";
                    }
                }
                html += "</tr>";

                html += "</thead>";

                int counter = 0;
                int cellValue = 0;

                foreach (var block in ReportResult.PositionDeliveryResult.GroupBy(x => new { x.MuniciplaityName, x.DistrictName }))
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                <td style='" + cellStyle + @" text-align: left;'>" + block.Key.MuniciplaityName + @"</td>
                                <td style='" + cellStyle + @" text-align: left;'>" + block.Key.DistrictName + @"</td>
                              ";

                    foreach (var readiness in readinessTypes)
                    {
                        foreach (var technicsType in technicsTypes)
                        {
                            if (technicsType.TypeKey == "VEHICLES")
                            {
                                foreach (var vehicleKind in vehicleKinds)
                                {
                                    var vehichleKindRec = block.Where(x => x.VehicleKindID == vehicleKind.TableKey).SingleOrDefault();
                                    cellValue = (vehichleKindRec == null ? 0 : (readiness == 1 ? vehichleKindRec.Fulfiled : vehichleKindRec.Reserve));
                                    html += "<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>";
                                }
                            }
                            else
                            {
                                var technicsTypeRec = block.Where(x => x.TechnicsTypeID == technicsType.TechnicsTypeId).SingleOrDefault();
                                cellValue = (technicsTypeRec == null ? 0 : (readiness == 1 ? technicsTypeRec.Fulfiled : technicsTypeRec.Reserve));
                                html += "<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>";
                            }
                        }
                        cellValue = block.Sum(x => (readiness == 1 ? x.Fulfiled : x.Reserve));
                        html += "<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>";
                    }

                    html += @"</tr>";
                }

                html += "</table>";
            }

            this.pnlReportGrid.InnerHtml = html;
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (ValidateData())
            {                                
                RefreshReport();
            }
        }                 

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            hdnMilitaryDepartmentSelected.Value = "";
            PopulateMilitaryReadiness();
            PopulateMilitaryCommands();

            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        private ReportAnalyzeTechCommandFilter CollectFilterData()
        {
            ReportAnalyzeTechCommandFilter filter = new ReportAnalyzeTechCommandFilter();

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            filter.MilitaryReadinessID = int.Parse(ddMilitaryReadiness.SelectedValue);

            filter.MilitaryCommandId = int.Parse(ddMilitaryCommand.SelectedValue);
            hdnMilitaryCommandId.Value = ddMilitaryCommand.SelectedValue;

            filter.MilitaryCommandSuffix = ddSubMilitaryCommand.SelectedValue;
            hdnSubMilitaryCommandId.Value = ddSubMilitaryCommand.SelectedValue;

            return filter;
        }

        protected void ddMilitaryReadiness_Changed(object sender, EventArgs e)
        {
            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddMilitaryCommand.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryReadinessForTechnics(CurrentUser, hdnMilitaryDepartmentSelected.Value, readinessID);
            ddMilitaryCommand.DataTextField = "DisplayTextForSelection";
            ddMilitaryCommand.DataValueField = "MilitaryCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryCommand_Changed(Page, new EventArgs());
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddSubMilitaryCommand.DataSource = TechnicsRequestCommandUtil.GetTechnicsRequestCommandsForMilCommandAndMilDeptAndMilReadiness(CurrentUser, int.Parse(ddMilitaryCommand.SelectedValue), hdnMilitaryDepartmentSelected.Value, readinessID);
            ddSubMilitaryCommand.DataTextField = "Txt";
            ddSubMilitaryCommand.DataValueField = "Val";
            ddSubMilitaryCommand.DataBind();
            ddSubMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void hdnBtnReloadMilitaryCommands_Click(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
        }
    }
}
