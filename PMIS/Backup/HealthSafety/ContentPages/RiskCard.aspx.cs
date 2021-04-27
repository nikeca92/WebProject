using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class RiskCard : HSPage
    {
        private bool screenDisabled = false;

        private bool addRiskCardPermission = true;
        private bool editRiskCardPermission = true;

        private bool addRiskCardItemPermission = true;
        private bool editRiskCardItemPermission = true;
        private bool hideRiskCardItemPermission = false;

        // client controls to disable and hide when add mode of work
        List<string> disabledClientControls = new List<string>();
        List<string> hiddenClientControls = new List<string>();

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_RISKCARD";
            }
        }

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");


        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetupPageUI(); //setup user interface elements according to rights of the user's role

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetRiskFactorsHtml")
            {
                this.GenerateRiskFactorsHtml();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetHazardsHtml")
            {
                this.GenerateHazardsHtml();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCoefficientsHtml")
            {
                this.GenerateCoefficientsHtml();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRiskCardItem")
            {
                this.SaveRiskCardItem();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCalculateHazardValue")
            {
                this.CalculateHazardValue();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRiskCardItem")
            {
                this.DeleteRiskCardItem();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveOtherHazard")
            {
                this.SaveOtherHazard();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetFactorName")
            {
                this.GetFactorName();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetHazardName")
            {
                this.GetHazardName();
                return;
            }

            HighlightMenuItems("RiskAssessments", "RiskCard");

            //Hide the navigation buttons
            //HideNavigationControls(btnBack);

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.Params["PositionId"]))
                {
                    int positionId = 0;
                    int.TryParse(Request.Params["PositionId"], out positionId);

                    if (positionId > 0)
                    {
                        Position position = PositionUtil.GetPosition(positionId, CurrentUser);

                        if (MilitaryUnitUtil.CanAccess(position.Subdivision.MilitaryUnitId.Value, CurrentUser))
                        {
                            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(position.Subdivision.MilitaryUnitId.Value, CurrentUser);

                            musMilitaryUnit.SelectedValue = militaryUnit.MilitaryUnitId.ToString();
                            musMilitaryUnit.SelectedText = militaryUnit.DisplayTextForSelection;

                            PopulateSubdivisions();
                            ddSubdivisions.SelectedValue = position.SubdivisionId.ToString();

                            PopulatePositions();
                            ddPositions.SelectedValue = positionId.ToString();
                        }
                    }
                }

                LoadTable();
            }

            lblMilitaryUnit.InnerHtml = MilitaryUnitLabel + ":";

            lblMessage.Text = ""; // clean message of protocol operations
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            bool screenHidden = (this.GetUIItemAccessLevel("HS_RISKCARD") == UIAccessLevel.Hidden)
                                    || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);

            screenDisabled = (this.GetUIItemAccessLevel("HS_RISKCARD") == UIAccessLevel.Disabled)
                                    || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Disabled);

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                editRiskCardPermission = false;
                editRiskCardItemPermission = false;
            }

            UIAccessLevel l;

            l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                addRiskCardPermission = false;
                editRiskCardPermission = false;

                disabledClientControls.Add("lblProbability");
                disabledClientControls.Add("ddProbabilities");
                disabledClientControls.Add("lblExposure");
                disabledClientControls.Add("ddExposures");
                disabledClientControls.Add("lblEffectWeight");
                disabledClientControls.Add("ddEffectWeights");
                disabledClientControls.Add("lblHazard");
                disabledClientControls.Add("lblHazardValue");
            }
            else
            {
                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_PROBABILITY");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblProbability");
                    disabledClientControls.Add("ddProbabilities");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EXPOSURE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblExposure");
                    disabledClientControls.Add("ddExposures");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EFFECTWEIGHT");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblEffectWeight");
                    disabledClientControls.Add("ddEffectWeights");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_HAZARD");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblHazard");
                    disabledClientControls.Add("lblHazardValue");
                }
            }

            l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD");
            if (l == UIAccessLevel.Hidden)
            {
                hideRiskCardItemPermission = true;
                addRiskCardItemPermission = false;
                editRiskCardItemPermission = false;

                hiddenClientControls.Add("lblProbability");
                hiddenClientControls.Add("ddProbabilities");
                hiddenClientControls.Add("lblExposure");
                hiddenClientControls.Add("ddExposures");
                hiddenClientControls.Add("lblEffectWeight");
                hiddenClientControls.Add("ddEffectWeights");
                hiddenClientControls.Add("lblHazard");
                hiddenClientControls.Add("lblHazardValue");
            }
            else
            {
                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_PROBABILITY");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblProbability");
                    hiddenClientControls.Add("ddProbabilities");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EXPOSURE");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblExposure");
                    hiddenClientControls.Add("ddExposures");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EFFECTWEIGHT");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEffectWeight");
                    hiddenClientControls.Add("ddEffectWeights");
                }

                l = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_HAZARD");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHazard");
                    hiddenClientControls.Add("lblHazardValue");
                }
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        //Get the UIItems info for the page
        public string GetPageUIItemsInfo()
        {
            string UIItemsXML = "";

            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach (string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach (string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }

        private void LoadTable()
        {
            this.divRiskCardItemsTable.InnerHtml = "";
            int selectedPositionId = 0;
            int.TryParse(this.ddPositions.SelectedValue, out selectedPositionId);

            if (selectedPositionId > 0)
            {
                this.hfSelectedPositionId.Value = selectedPositionId.ToString();

                Position position = PositionUtil.GetPosition(selectedPositionId, CurrentUser);

                List<RiskCardItem> riskCardItems = RiskCardItemUtil.GetAllRiskCardItemsByPosition(selectedPositionId, CurrentUser);
                this.divRiskCardItemsTable.InnerHtml = GenerateTableHTML(riskCardItems, position.PositionName);
                this.btnPrintRiskCard.Visible = true;
            }
            else
            {
                this.divRiskCardItemsTable.InnerHtml = "<span>За показване и редактиране на карта за оценка на риска, моля изберете длъжност</span>";
                this.btnPrintRiskCard.Visible = false;
            }
        }

        private string GenerateTableHTML(List<RiskCardItem> riskCardItems, string positionName)
        {
            bool IsProbabilityHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_PROBABILITY") == UIAccessLevel.Hidden;
            bool IsExposureHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EXPOSURE") == UIAccessLevel.Hidden;
            bool IsEffectWeightHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EFFECTWEIGHT") == UIAccessLevel.Hidden;
            bool IsHazardHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_HAZARD") == UIAccessLevel.Hidden;

            int maxRiskValue = 0;
            string maxRiskRank = "";

            int columnNumber = 8;

            if (IsProbabilityHidden)
                columnNumber--;

            if (IsExposureHidden)
                columnNumber--;

            if (IsEffectWeightHidden)
                columnNumber--;

            if (IsHazardHidden)
                columnNumber--;

            List<RiskFactorType> riskFactorTypes = RiskFactorTypeUtil.GetAllRiskFactorTypes(CurrentUser);

            StringBuilder sb = new StringBuilder();

            string headerStyle = "vertical-align: middle;";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: center;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;" + headerStyle + @"'>№ по ред</th>
                               <th style='width: 250px;" + headerStyle + @"'>Вид опасност</th>

            " + (!IsProbabilityHidden ? @"<th style='width: 100px;" + headerStyle + @"'>Вероятност<br />(B)</th>" : "") + @"
            " + (!IsExposureHidden ? @"<th style='width: 100px;" + headerStyle + @"'>Експозиция<br />(E)</th>" : "") + @"
            " + (!IsEffectWeightHidden ? @"<th style='width: 100px;" + headerStyle + @"'>Тежест<br />(T)</th>" : "") + @"
            " + (!IsHazardHidden ? @"<th style='width: 80px;" + headerStyle + @"'>Риск<br />(B*E*T)</th>" : "") + @"

                               <th style='width: 80px;" + headerStyle + @"'>Степен</th>
                               <th style='width: 120px;" + headerStyle + @"'>Класификация</th>
                               <th style='width: 50px;" + headerStyle + @"'></th>
                            </tr>
                         </thead>");

            int riskFactorTypeCounter = 1;
            
            foreach (RiskFactorType riskFactorType in riskFactorTypes)
            {
                var subRiskCardItems = riskCardItems.FindAll(a => a.RiskFactorTypeId == riskFactorType.RiskFactorTypeId).OrderBy(a => a.RiskFactorId).ThenBy(a => a.RiskFactorSeq).ThenBy(a => a.HazardSeq).ThenBy(a => a.RiskCardItemId);

                string cellStyle = "vertical-align: top;";

                string addNewRiskCardItemHTML = "";

                if (!screenDisabled && addRiskCardPermission && addRiskCardItemPermission)
                    addNewRiskCardItemHTML = @"<img src='../Images/addrow.gif' alt='Добавяне на нова опасност' title='Добавяне на нова опасност' class='GridActionIcon' onclick=""ShowRiskCardItemLightBoxContent(" + riskFactorType.RiskFactorTypeId.ToString() + ", '" + riskFactorType.RiskFactorTypeName + @"', true);"" />";

                sb.Append(@"<tr style='background-color: #F8F8F8;'>
                                <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + @".</td>                                                 
                                <td style='" + cellStyle + @" text-align: left; font-size: 1.25em;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactorType.RiskFactorTypeName) + @" " + addNewRiskCardItemHTML + @"</td>
                            </tr>");

                if (!hideRiskCardItemPermission)
                {
                    int riskFactorCounter = 0;
                    int riskFactorId = 0;
                    int riskCardItemCounter = 1;

                    foreach (RiskCardItem riskCardItem in subRiskCardItems)
                    {
                        if (maxRiskValue < riskCardItem.HazardValue)
                        {
                            maxRiskValue = riskCardItem.HazardValue;
                            maxRiskRank = riskCardItem.RiskRank;
                        }

                        string deleteRiskCardItemHTML = "";
                        string editCoefficiantsHTML = "";

                        if (!screenDisabled && editRiskCardPermission && editRiskCardItemPermission)
                        {
                            deleteRiskCardItemHTML = @"<img src='../Images/delete.png' alt='Изтриване на опасност' title='Изтриване на опасност' class='GridActionIcon' onclick=""DeleteRiskCardItem(" + riskCardItem.RiskCardItemId.ToString() + @");"" />";
                            editCoefficiantsHTML = @"<img src='../Images/edit.png' alt='Редактиране на коефициентите' title='Редактиране на коефициентите' class='GridActionIcon' onclick=""ShowCoefficientsView(" + (riskCardItem.HazardId.HasValue ? riskCardItem.HazardId.Value.ToString() : "0") + ", '" + riskCardItem.RiskCardItemId.ToString() + @"', '" + riskCardItem.RiskFactorId.ToString() + @"', '" + RiskFactorTypeUtil.GetRiskFactorType(riskCardItem.RiskFactorTypeId, CurrentUser).RiskFactorTypeName + @"');"" />";
                        }

                        if (riskFactorId != riskCardItem.RiskFactorId)
                        {
                            riskFactorCounter++;
                            riskCardItemCounter = 1;
                            riskFactorId = riskCardItem.RiskFactorId;

                            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskCardItem.RiskFactorId, CurrentUser);

                            sb.Append(@"<tr style='background-color: #F8F8F8;'>
                                        <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + "." + riskFactorCounter + @".</td>                                                 
                                        <td style='" + cellStyle + @" text-align: left; font-size: 1.0em;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactor.RiskFactorName) + @"</td>
                                    </tr>");
                        }

                        sb.Append(@"<tr>
                                        <td style='" + cellStyle + @"'>" + riskFactorTypeCounter + "." + riskFactorCounter + "." + riskCardItemCounter + @".</td>");

                        if (riskCardItem.HazardId.HasValue)
                        {
                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + riskCardItem.HazardName + @"</td>");
                        }
                        else
                        {
                            string editOtherHazardHTML = "";
                            if (!screenDisabled && editRiskCardPermission && editRiskCardItemPermission)
                            {
                                editOtherHazardHTML = @"<img src='../Images/edit.png' alt='Редактиране на друга опасност' title='Редактиране на друга опасност' class='GridActionIcon' onclick=""ShowOtherHazardLightBox(" + riskCardItem.RiskCardItemId.ToString() + ", '" + CommonFunctions.ReplaceNewLinesInString(CommonFunctions.HtmlEncoding(riskCardItem.OtherHazard).Replace("'", "\\'")) + @"');"" />";
                            }

                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.ReplaceNewLinesInString(riskCardItem.OtherHazard) + " " + editOtherHazardHTML + @"</td>");
                        }

                        sb.Append(@"                                    
                        " + (!IsProbabilityHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ProbabilityFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsExposureHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ExposureFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsEffectWeightHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.EffectWeightFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsHazardHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.HazardValue.ToString() + @"</td>" : "") + @"

                                    <td style='" + cellStyle + @"'>" + riskCardItem.RiskRank + @"</td>
                                    <td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.HtmlEncoding(riskCardItem.RiskRankName) + @"</td>
                                    <td style='" + cellStyle + @"'>" + editCoefficiantsHTML + " " + deleteRiskCardItemHTML + @"</td>
                                </tr>");

                        riskCardItemCounter++;
                    }   
                }

                riskFactorTypeCounter++;
            }

            sb.Append(@"</table><br />");

            sb.Append(@"<span id='lblMaxHazardValue' class='Label' style='float: right; margin-right: 15px;'>Максимална степен на риска за длъжност " + positionName + ": <b>" + maxRiskRank + "</b></span>");

            return sb.ToString();
        }

        // hidden button for forced refresh of Subdivisions table
        protected void btnHdnRefreshSubdivisions_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        //Navigate back to the ManageProtocols screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.Params["PositionId"]))
                Response.Redirect("~/ContentPages/MilitaryUnitPositions.aspx?MilitaryUnitId=" + musMilitaryUnit.SelectedValue);
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnSelectedMilitaryUnit_Click(object sender, EventArgs e)
        {
            PopulateSubdivisions();
            PopulatePositions();
            LoadTable();
        }

        protected void ddSubdivisions_Changed(object sender, EventArgs e)
        {
            PopulatePositions();
            LoadTable();
        }

        protected void ddPositions_Changed(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void PopulateSubdivisions()
        {
            int militaryUntiId = -1;

            try
            {
                militaryUntiId = int.Parse(musMilitaryUnit.SelectedValue);
            }
            catch
            {
                militaryUntiId = -1;
            }
            
            ddSubdivisions.DataSource = SubdivisionUtil.GetAllSubdivisionsByMilitaryUnitID(militaryUntiId, CurrentUser);
            ddSubdivisions.DataTextField = "SubdivisionName";
            ddSubdivisions.DataValueField = "SubdivisionId";
            ddSubdivisions.DataBind();
            ddSubdivisions.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        private void PopulatePositions()
        {
            int subdivisionId = -1;

            try
            {
                subdivisionId = int.Parse(ddSubdivisions.SelectedValue);
            }
            catch
            {
                subdivisionId = -1;
            }

            ddPositions.DataSource = PositionUtil.GetAllPositionsBySubdivisionId(subdivisionId, CurrentUser);
            ddPositions.DataTextField = "PositionName";
            ddPositions.DataValueField = "PositionId";
            ddPositions.DataBind();
            ddPositions.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Generate risk factors html content
        private void GenerateRiskFactorsHtml()
        {
            int riskFactorTypeId = 0;
            int.TryParse(Request.Params["RiskFactorTypeId"], out riskFactorTypeId);

            if (riskFactorTypeId == 0 || screenDisabled || !addRiskCardPermission
                || !editRiskCardPermission || !addRiskCardItemPermission || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            } 

            // Generates html for risk factors drop down list
            List<RiskFactor> riskFactors = RiskFactorUtil.GetAllRiskFactorsByType(riskFactorTypeId, CurrentUser);
            List<IDropDownItem> ddiRiskFactors = new List<IDropDownItem>();
            foreach (RiskFactor riskFactor in riskFactors)
            {
                ddiRiskFactors.Add(riskFactor);
            }

            string riskFactorsHTML = ListItems.GetDropDownHtml(ddiRiskFactors, null, "ddRiskFactors", true, null, "RiskFactorsDDChange(this)", "class='RequiredInputField' style='width: 870px;'");

            string html = @"<table style='width: 100%;'>
                                <tr><td align='left'><span id='lblRiskFactor' class='InputLabel'>Фактори:</span></td></tr>
                                <tr>
                                    <td align='left'>
                                        " + riskFactorsHTML + @"
                                    </td>
                                </tr>
                            </table>";

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(html);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Generate harzards html content
        private void GenerateHazardsHtml()
        {
            int positionId = 0;
            int.TryParse(Request.Params["PositionId"], out positionId);

            if (positionId == 0 || screenDisabled || !addRiskCardPermission
                || !editRiskCardPermission || !addRiskCardItemPermission || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskFactorId = 0;
            int.TryParse(Request.Params["RiskFactorId"], out riskFactorId);

            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskFactorId, CurrentUser);

            string html = "";
            html += "<table style='width: 875px;'>";
            html += "<tr><td align='left'><span class='InputLabel'>Изберете опасност от списъка:</span></td></tr>";
            html += "<tr><td align='center'>";

            //Generate hazards table
            if (riskFactorId > 0)
            {
                List<Hazard> hazards = HazardUtil.GetAllHazardsByPositionAndRiskFactor(positionId, riskFactorId, CurrentUser);

                //No data found
                if (hazards.Count == 0)
                {
                    html += "<span>Няма въведени опасности</span>";
                }
                //If there is data then generate dynamically the HTML for the data grid
                else
                {
                    //Setup the header of the grid
                    html += "<table class='CommonHeaderTable' style='width: 100%; margin: 0 auto; text-align: left;'>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (Hazard hazard in hazards)
                    {
                        html += @"<tr title='Избери' class='TableRow' onclick='HazardRowSelected(" + hazard.HazardId.ToString() + @");'>
                                 <td align='center' style='width: 30px;'>" + counter.ToString() + @"</td>
                                 <td>" + hazard.HazardName + @"</td>
                              </tr>";

                        counter++;
                    }

                    html += "</table>";
                }
            }

            html += "</td></tr>";

            //Generate hazard text area
            if (riskFactor != null && riskFactor.AllowAddManually)
            {
                html += "<tr style='height: 15px'></tr>";
                html += "<tr><td align='left'><span class='InputLabel'>или въведете друга опасност:</span></td></tr>";
                html += "<tr><td><textarea id='txtOtherHazard' rows='3' class='RequiredInputField' style='width: 875px;'></textarea></td></tr>";
                html += "<tr><td align='right'><div id='btnGoToCoefficientsView' style='display: inline;' onclick='BeforeGoToCoefficientsView();' class='Button'><i></i><div id='btnGoToCoefficientsViewText' style='width:180px;'>Избери въведената опасност</div><b></b></div></td></tr>";
            }

            html += "</table>";

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(html);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Generate coefficients html content
        private void GenerateCoefficientsHtml()
        {
            if (screenDisabled || !addRiskCardPermission
                || !editRiskCardPermission || !addRiskCardItemPermission || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskCardItemId = 0;
            int.TryParse(Request.Params["RiskCardItemId"], out riskCardItemId);

            string status = "";
            string response = "";

            try
            {
                RiskCardItem riskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, CurrentUser);

                string html = "<table style='width: 100%;'>";

                // Generates html for probabilities drop down list
                List<Probability> probabilities = ProbabilityUtil.GetAllProbabilities(CurrentUser);
                List<IDropDownItem> ddiProbabilities = new List<IDropDownItem>();
                foreach (Probability probability in probabilities)
                {
                    ddiProbabilities.Add(probability);
                }

                IDropDownItem selectedProbability = null;

                if (riskCardItem != null)
                {
                    selectedProbability = (ddiProbabilities.Count > 0 ? ddiProbabilities.Find(p => p.Value() == riskCardItem.ProbabilityId.ToString()) : null);
                }

                string probabilitiesHTML = ListItems.GetDropDownHtml(ddiProbabilities, null, "ddProbabilities", true, selectedProbability, "ProbabilitiesDDChange(this)", "class='RequiredInputField' style='width: 360px;'");

                html += @"
                        <tr>
                            <td align='right' style='width: 200px;'><span id='lblProbability' class='InputLabel'>Вероятност (B):</span></td>
                            <td align='left'>
                                " + probabilitiesHTML + @"
                            </td>
                        </tr>";

                // Generates html for exposures drop down list
                List<Exposure> exposures = ExposureUtil.GetAllExposures(CurrentUser);
                List<IDropDownItem> ddiExposures = new List<IDropDownItem>();
                foreach (Exposure exposure in exposures)
                {
                    ddiExposures.Add(exposure);
                }

                IDropDownItem selectedExposure = null;

                if (riskCardItem != null)
                {
                    selectedExposure = (ddiExposures.Count > 0 ? ddiExposures.Find(p => p.Value() == riskCardItem.ExposureId.ToString()) : null);
                }

                string exposuresHTML = ListItems.GetDropDownHtml(ddiExposures, null, "ddExposures", true, selectedExposure, "ExposuresDDChange(this)", "class='RequiredInputField' style='width: 360px;'");

                html += @"
                        <tr>
                            <td align='right' style='width: 200px;'><span id='lblExposure' class='InputLabel'>Експозиция (E):</span></td>
                            <td align='left'>
                                " + exposuresHTML + @"
                            </td>
                        </tr>";

                // Generates html for effect weights drop down list
                List<EffectWeight> effectWeights = EffectWeightUtil.GetAllEffectWeights(CurrentUser);
                List<IDropDownItem> ddiEffectWeights = new List<IDropDownItem>();
                foreach (EffectWeight effectWeight in effectWeights)
                {
                    ddiEffectWeights.Add(effectWeight);
                }

                IDropDownItem selectedEffectWeight = null;

                if (riskCardItem != null)
                {
                    selectedEffectWeight = (ddiEffectWeights.Count > 0 ? ddiEffectWeights.Find(p => p.Value() == riskCardItem.EffectWeightId.ToString()) : null);
                }

                string effectWeightsHTML = ListItems.GetDropDownHtml(ddiEffectWeights, null, "ddEffectWeights", true, selectedEffectWeight, "EffectWeightsDDChange(this)", "class='RequiredInputField' style='width: 360px;'");

                html += @"
                        <tr>
                            <td align='right' style='width: 200px;'><span id='lblEffectWeight' class='InputLabel'>Тежест (T):</span></td>
                            <td align='left'>
                                " + effectWeightsHTML + @"
                            </td>
                        </tr>";

                string hazardValue = "";

                if (riskCardItem != null)
                {
                    hazardValue = riskCardItem.HazardValue.ToString();
                }

                html += @"
                        <tr>
                            <td align='right' style='width: 200px;'><span id='lblHazard' class='InputLabel'>Риск (B*E*T):</span></td>
                            <td align='left'><span id='lblHazardValue' class='ReadOnlyValue'>" + hazardValue + @"</span></td>
                        </tr>
                    </table><br />";

                html += @"<table style='width: 100%;'>
                            <tr style='padding-top: 5px;'>
                                <td align='center'>
                                    <span id='lblCoefficientsMessage' class='ErrorText' style='display: none;'></span>
                                </td>
                            </tr>
                        </table><br />";

                html += @"<table style='width: 100%;'>
                            <tr>
                                <td align='center'>
                                    <div id='btnSaveRiskCardItemLightBox' onclick='SaveRiskCardItem(" + riskCardItemId + @");' class='" + (riskCardItem != null ? "Button" : "DisabledButton") + @"'><i></i><div id='btnSaveRiskCardItemLightBoxText' style='width:70px;'>Запис</div><b></b></div>&nbsp;&nbsp;
                                    <div id='btnCloseRiskCardItemLightBox2' onclick='HideRiskCardItemLightBox();' class='Button'><i></i><div id='btnCloseRiskCardItemLightBox2Text' style='width:70px;'>Отказ</div><b></b></div>
                                </td>
                            </tr>
                        </table>";

                string UIItems = GetPageUIItemsInfo();

                status = AJAXTools.OK;

                response = @"
                        <contentHTML>" + AJAXTools.EncodeForXML(html) + @"</contentHTML>
                        ";

                if (!String.IsNullOrEmpty(UIItems))
                {
                    response += "<UIItems>" + UIItems + "</UIItems>";
                }
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Calculate hazard value
        private void CalculateHazardValue()
        {
            if (screenDisabled || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int probabilityId = 0;
            int.TryParse(Request.Params["ProbabilityId"], out probabilityId);

            int exposureId = 0;
            int.TryParse(Request.Params["ExposureId"], out exposureId);

            int effectWeightId = 0;
            int.TryParse(Request.Params["EffectWeightId"], out effectWeightId);

            string result = RiskCardItemUtil.CalculateHazardValue(probabilityId, exposureId, effectWeightId, CurrentUser).ToString();

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(result);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Save risk card item
        private void SaveRiskCardItem()
        {
            int riskCardItemId = 0;
            int.TryParse(Request.Params["RiskCardItemId"], out riskCardItemId);

            int positionId = 0;
            int.TryParse(Request.Params["PositionId"], out positionId);

            if (positionId == 0 || screenDisabled || (!addRiskCardPermission && riskCardItemId == 0)
                || (!editRiskCardPermission && riskCardItemId != 0) || (!addRiskCardItemPermission && riskCardItemId == 0)
                || (!editRiskCardItemPermission && riskCardItemId != 0) || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskFactorTypeId = 0;
            int.TryParse(Request.Params["RiskFactorTypeId"], out riskFactorTypeId);

            int riskFactorId = 0;
            int.TryParse(Request.Params["RiskFactorId"], out riskFactorId);

            int hazardId = 0;
            int.TryParse(Request.Params["HazardId"], out hazardId);

            int probabilityId = 0;
            int.TryParse(Request.Params["ProbabilityId"], out probabilityId);

            int exposureId = 0;
            int.TryParse(Request.Params["ExposureId"], out exposureId);

            int effectWeightId = 0;
            int.TryParse(Request.Params["EffectWeightId"], out effectWeightId);

            string otherHazard = Request.Params["OtherHazard"].ToString();

            string status = "";
            string response = "";

            try
            {
                RiskCardItem riskCardItem = null;

                if (riskCardItemId > 0)
                {
                    riskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, CurrentUser);
                    riskCardItem.ProbabilityId = probabilityId;
                    riskCardItem.ExposureId = exposureId;
                    riskCardItem.EffectWeightId = effectWeightId;
                }
                else
                {
                    Common.RiskCard riskCard = RiskCardUtil.GetRiskCardsByPosition(positionId, CurrentUser);

                    if (riskCard == null)
                    {
                        riskCard = new Common.RiskCard(CurrentUser) { PositionId = positionId };
                        RiskCardUtil.SaveRiskCard(riskCard, CurrentUser);
                    }

                    riskCardItem = new RiskCardItem(CurrentUser)
                    {
                        RiskCardId = riskCard.RiskCardId,
                        RiskFactorTypeId = riskFactorTypeId,
                        RiskFactorId = riskFactorId,
                        HazardId = hazardId > 0 ? hazardId : (int?)null,
                        OtherHazard = String.IsNullOrEmpty(otherHazard) ? null : otherHazard,
                        ProbabilityId = probabilityId,
                        ExposureId = exposureId,
                        EffectWeightId = effectWeightId
                    };
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_RiskCard");

                Position position = PositionUtil.GetPosition(positionId, CurrentUser);

                RiskCardItemUtil.SaveRiskCardItem(riskCardItem, position, CurrentUser, change);

                change.WriteLog();

                status = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Delete risk card item
        private void DeleteRiskCardItem()
        {
            if (screenDisabled || !editRiskCardPermission || !editRiskCardItemPermission || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskCardItemId = 0;
            int.TryParse(Request.Params["RiskCardItemId"], out riskCardItemId);

            string status = "";
            string response = "";

            try
            {
                RiskCardItem riskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, CurrentUser);

                int riskCardId = 0;
                if (riskCardItem != null)
                {
                    riskCardId = riskCardItem.RiskCardId;
                }
                else
                {
                    throw new Exception();
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_RiskCard");

                if (RiskCardItemUtil.DeleteRiskCardItem(riskCardItemId, CurrentUser, change))
                {
                    Common.RiskCard riskCard = RiskCardUtil.GetRiskCard(riskCardId, CurrentUser);
                    if (riskCard.RiskCardItems.Count == 0)
                    {
                        RiskCardUtil.DeleteRiskCard(riskCardId, CurrentUser);
                    }
                }

                change.WriteLog();

                status = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Save other hazard
        private void SaveOtherHazard()
        {
            if (screenDisabled || !editRiskCardPermission || !editRiskCardItemPermission || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskCardItemId = 0;
            int.TryParse(Request.Params["RiskCardItemId"], out riskCardItemId);

            string otherHazard = Request.Params["OtherHazard"].ToString();

            string status = "";
            string response = "";

            try
            {
                RiskCardItem riskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, CurrentUser);

                if (riskCardItem != null)
                {
                    riskCardItem.OtherHazard = otherHazard;
                }
                else
                {
                    throw new Exception();
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_RiskCard");

                Common.RiskCard riskCard = RiskCardUtil.GetRiskCard(riskCardItem.RiskCardId, CurrentUser);

                RiskCardItemUtil.SaveRiskCardItem(riskCardItem, riskCard.Position, CurrentUser, change);

                change.WriteLog();

                status = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Get factor name
        private void GetFactorName()
        {
            if (screenDisabled || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskFactorId = 0;
            int.TryParse(Request.Params["RiskFactorId"], out riskFactorId);

            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskFactorId, CurrentUser);

            string result = AJAXTools.OK;

            if (riskFactor != null)
            {
                result = riskFactor.RiskFactorName;    
            }

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(result);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Get hazard name
        private void GetHazardName()
        {
            if (screenDisabled || hideRiskCardItemPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int riskCardItemId = 0;
            int.TryParse(Request.Params["RiskCardItemId"], out riskCardItemId);

            int hazardId = 0;
            int.TryParse(Request.Params["HazardId"], out hazardId);

            string result = AJAXTools.OK;

            if (riskCardItemId > 0)
            {
                RiskCardItem riskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, CurrentUser);
                if (riskCardItem != null)
                {
                    if (riskCardItem.HazardId.HasValue)
                    {
                        result = riskCardItem.HazardName;
                    }
                    else
                    {
                        result = CommonFunctions.ReplaceNewLinesInString(AJAXTools.EncodeForXML(riskCardItem.OtherHazard));
                    }
                }
            }
            else if (hazardId > 0)
            {
                Hazard hazard = HazardUtil.GetHazard(hazardId, CurrentUser);
                if (hazard != null)
                {
                    result = hazard.HazardName;
                }
            }

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(result);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }
    }
}
