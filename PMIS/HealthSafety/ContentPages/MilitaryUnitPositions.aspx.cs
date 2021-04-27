using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class MilitaryUnitPositions : HSPage
    {
        private bool screenDisabled = false;

        private bool addSubdevisionPermission = true;
        private bool editSubdevisionPermission = true;

        private bool addPositionPermission = true;
        private bool editPositionPermission = true;
        private bool hidePositionPermission = false;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_MILITARYUNITPOSITIONS";
            }
        }

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        //Getter/Setter of the ID of the displayed military unit
        private int MilitaryUnitId
        {
            get
            {
                int militaryUnitId = 0;
                //gets ProtocolID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfMilitaryUnitId.Value)
                    || this.hfMilitaryUnitId.Value == "0")
                {
                    if (Request.Params["MilitaryUnitId"] != null)
                        int.TryParse(Request.Params["MilitaryUnitId"].ToString(), out militaryUnitId);

                    //sets protocol ID in hidden field on the page in order to be accessible in javascript
                    this.hfMilitaryUnitId.Value = militaryUnitId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilitaryUnitId.Value, out militaryUnitId);
                }

                return militaryUnitId;
            }
            set { this.hfMilitaryUnitId.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HighlightMenuItems("RiskAssessments", "MilitaryUnitPositions");

            //Hide the navigation buttons
            //HideNavigationControls(btnBack);

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            if (MilitaryUnitId == 0)
            {
                List<MilitaryUnit> allMilUnits = MilitaryUnitUtil.GetAllMilitaryUnits(CurrentUser);

                MilitaryUnit milUnit = null;

                foreach (MilitaryUnit militaryUnit in allMilUnits)
                {
                    if (!String.IsNullOrEmpty(militaryUnit.VPN))
                    {
                        milUnit = militaryUnit;
                        break;
                    }
                }

                if (milUnit != null)
                {
                    MilitaryUnitId = milUnit.MilitaryUnitId;
                }
            }

            this.SetupPageUI(); //setup user interface elements according to rights of the user's role

            //Process the ajax request for saving indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveSubdivision")
            {
                JSSaveSubdivision();
                return;
            }

            //Process ajax request for deleting of indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteSubdivision")
            {
                JSDeleteSubdivision();
                return;
            }

            //Process the ajax request for saving indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePosition")
            {
                JSSavePosition();
                return;
            }

            //Process ajax request for deleting of indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePosition")
            {
                JSDeletePosition();
                return;
            }

            if (!IsPostBack)
            {
                MilitaryUnit selectedMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(MilitaryUnitId, CurrentUser);

                if (selectedMilitaryUnit != null)
                {
                    musMilitaryUnit.SelectedValue = selectedMilitaryUnit.MilitaryUnitId.ToString();
                    musMilitaryUnit.SelectedText = selectedMilitaryUnit.DisplayTextForSelection;

                    hdnOldMilitaryUnitID.Value = musMilitaryUnit.SelectedValue;
                }

                LoadTable();
            }

            lblMilitaryUnit.InnerHtml = MilitaryUnitLabel + ":";

            lblMessage.Text = ""; // clean message of protocol operations
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            bool screenHidden = (this.GetUIItemAccessLevel("HS_MILITARYUNITPOSITIONS") == UIAccessLevel.Hidden)
                                    || (this.GetUIItemAccessLevel("HS_SUBDEV") == UIAccessLevel.Hidden)
                                    || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);

            screenDisabled = (this.GetUIItemAccessLevel("HS_MILITARYUNITPOSITIONS") == UIAccessLevel.Disabled)
                                    || (this.GetUIItemAccessLevel("HS_SUBDEV") == UIAccessLevel.Disabled)
                                    || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Disabled);

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                editSubdevisionPermission = false;
                editPositionPermission = false;
            }

            UIAccessLevel l;

            // client controls to disable and hide when add mode of work
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            l = this.GetUIItemAccessLevel("HS_SUBDEV");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                addSubdevisionPermission = false;
                editSubdevisionPermission = false;

                disabledClientControls.Add("lblSubdivisionName");
                disabledClientControls.Add("txtSubdivisionName");

                disabledClientControls.Add("lblPositionName");
                disabledClientControls.Add("txtPositionName");
                disabledClientControls.Add("lblActivities");
                disabledClientControls.Add("txtActivities");
                disabledClientControls.Add("lblTotalPersonsCnt");
                disabledClientControls.Add("txtTotalPersonsCnt");
                disabledClientControls.Add("lblFemaleCnt");
                disabledClientControls.Add("txtFemaleCnt");
            }
            else
            {
                l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION");
                if (l == UIAccessLevel.Disabled)
                {
                    addPositionPermission = false;
                    editPositionPermission = false;

                    disabledClientControls.Add("lblPositionName");
                    disabledClientControls.Add("txtPositionName");
                    disabledClientControls.Add("lblActivities");
                    disabledClientControls.Add("txtActivities");
                    disabledClientControls.Add("lblTotalPersonsCnt");
                    disabledClientControls.Add("txtTotalPersonsCnt");
                    disabledClientControls.Add("lblFemaleCnt");
                    disabledClientControls.Add("txtFemaleCnt");
                }
                else
                {
                    l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSNAME");
                    if (l == UIAccessLevel.Disabled)
                    {
                        disabledClientControls.Add("lblPositionName");
                        disabledClientControls.Add("txtPositionName");
                    }

                    l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSACTIVITIES");
                    if (l == UIAccessLevel.Disabled)
                    {
                        disabledClientControls.Add("lblActivities");
                        disabledClientControls.Add("txtActivities");
                    }

                    l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSTOTALPERSONSCNT");
                    if (l == UIAccessLevel.Disabled)
                    {
                        disabledClientControls.Add("lblTotalPersonsCnt");
                        disabledClientControls.Add("txtTotalPersonsCnt");
                    }

                    l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSFEMALECNT");
                    if (l == UIAccessLevel.Disabled)
                    {
                        disabledClientControls.Add("lblFemaleCnt");
                        disabledClientControls.Add("txtFemaleCnt");
                    }
                }
            }

            l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION");
            if (l == UIAccessLevel.Hidden)
            {
                hidePositionPermission = true;
                addPositionPermission = false;
                editPositionPermission = false;

                hiddenClientControls.Add("lblPositionName");
                hiddenClientControls.Add("txtPositionName");
                hiddenClientControls.Add("lblActivities");
                hiddenClientControls.Add("txtActivities");
                hiddenClientControls.Add("lblTotalPersonsCnt");
                hiddenClientControls.Add("txtTotalPersonsCnt");
                hiddenClientControls.Add("lblFemaleCnt");
                hiddenClientControls.Add("txtFemaleCnt");
            }
            else
            {
                l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSNAME");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPositionName");
                    hiddenClientControls.Add("txtPositionName");
                }

                l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSACTIVITIES");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblActivities");
                    hiddenClientControls.Add("txtActivities");
                }

                l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSTOTALPERSONSCNT");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTotalPersonsCnt");
                    hiddenClientControls.Add("txtTotalPersonsCnt");
                }

                l = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSFEMALECNT");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFemaleCnt");
                    hiddenClientControls.Add("txtFemaleCnt");
                }
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        private void LoadTable()
        {
            divMilitaryUnitPositionsTable.InnerHtml = "";
            List<Subdivision> subdivisions = SubdivisionUtil.GetAllSubdivisionsByMilitaryUnitID(MilitaryUnitId, CurrentUser);
            divMilitaryUnitPositionsTable.InnerHtml = GenerateTableHTML(subdivisions);

            //Set the message if there is a need (e.g. a deleted, added or edited indicator type)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "SUBDIVISION_DELETED")
                {
                    lblMessage.Text = "Подразделението/обектът е изтрит успешно!";
                    lblMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "SUBDIVISION_EDITED")
                {
                    lblMessage.Text = "Подразделението/обектът е обновен успешно!";
                    lblMessage.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "SUBDIVISION_ADDED")
                {
                    lblMessage.Text = "Подразделението/обектът е добавен успешно!";
                    lblMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "POSITION_DELETED")
                {
                    lblMessage.Text = "Длъжността е изтрита успешно!";
                    lblMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "POSITION_EDITED")
                {
                    lblMessage.Text = "Длъжността е обновена успешно!";
                    lblMessage.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "POSITION_ADDED")
                {
                    lblMessage.Text = "Длъжността е добавена успешно!";
                    lblMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private string GenerateTableHTML(List<Subdivision> subdivisions)
        {
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSNAME") == UIAccessLevel.Hidden;
            bool IsActivitiesHidden = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSACTIVITIES") == UIAccessLevel.Hidden;
            bool IsTotalPersonsCntHidden = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSTOTALPERSONSCNT") == UIAccessLevel.Hidden;
            bool IsFemaleCntHidden = this.GetUIItemAccessLevel("HS_SUBDEV_POSITION_POSFEMALECNT") == UIAccessLevel.Hidden;
            bool IsRiskCardHidden = this.GetUIItemAccessLevel("HS_RISKCARD") == UIAccessLevel.Hidden;

            int columnNumber = 5;

            if (IsPositionNameHidden)
                columnNumber--;

            if (IsActivitiesHidden)
                columnNumber--;

            if (IsTotalPersonsCntHidden)
                columnNumber--;

            if (IsFemaleCntHidden)
                columnNumber--;

            StringBuilder sb = new StringBuilder();

            string headerStyle = "vertical-align: bottom;";
            string addNewSubdivisionHtml = "";

            if (!screenDisabled && addSubdevisionPermission)
                addNewSubdivisionHtml = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на ново подразделение/обект' class='GridActionIcon' onclick=""ShowSubdivisionLightBox(0, '');"" />";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
            " + (!IsPositionNameHidden ? @"<th style='width: 180px;" + headerStyle + @"'>Длъжност</th>" : "") + @"
            " + (!IsActivitiesHidden ? @"<th style='width: 180px;" + headerStyle + @"'>Извършвани дейности</th>" : "") + @"
            " + (!IsTotalPersonsCntHidden ? @"<th style='width: 80px;" + headerStyle + @"'>Общ брой лица</th>" : "") + @"
            " + (!IsFemaleCntHidden ? @"<th style='width: 80px;" + headerStyle + @"'>От тях жени</th>" : "") + @"
                               <th style='width: 60px;" + headerStyle + @"'>" + addNewSubdivisionHtml + @"</th>
                            </tr>
                         </thead>");

            int counter = 1;
            int positionCounter = 1;

            foreach (Subdivision subdivision in subdivisions)
            {
                string cellStyle = "vertical-align: top;";

                string addNewPositionHTML = "";

                if (!screenDisabled && editSubdevisionPermission && addPositionPermission)
                    addNewPositionHTML = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на длъжност' class='GridActionIcon' onclick=""ShowPositionLightBox(" + subdivision.SubdivisionId.ToString() + @", 0, '', '', '', '');"" />";

                string deleteSubdivisionHTML = "";

                if (!screenDisabled && editSubdevisionPermission && subdivision.CanDelete)
                {
                    deleteSubdivisionHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на подразделение/обект' class='GridActionIcon' onclick=""DeleteSubdivision(" + subdivision.SubdivisionId.ToString() + ", '" + CommonFunctions.HtmlEncoding(subdivision.SubdivisionName) + @"');"" />";
                }

                string editSubdivisionHTML = "";

                if (!screenDisabled && editSubdevisionPermission)
                    editSubdivisionHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowSubdivisionLightBox(" + subdivision.SubdivisionId.ToString() + ", '" + CommonFunctions.HtmlEncoding(subdivision.SubdivisionName) + @"');"" />";
                
                sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                                                 
                                                 <td style='" + cellStyle + @" text-align: center; font-size: 1.2em;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(subdivision.SubdivisionName) + @"</td>
                                                 <td style='" + cellStyle + @" text-align: center;'>" + editSubdivisionHTML + deleteSubdivisionHTML + addNewPositionHTML + @"</td>
                                              </tr>");

                if (!hidePositionPermission)
                {
                    foreach (Position position in subdivision.Positions)
                    {
                        string deletePositionHTML = "";

                        if (position.CanDelete)
                        {
                            if (!screenDisabled && editSubdevisionPermission && editPositionPermission)
                                deletePositionHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на длъжност' class='GridActionIcon' onclick=""DeletePosition(" + subdivision.SubdivisionId.ToString() + ", " + position.PositionId.ToString() + ", '" + position.PositionName + @"');"" />";
                        }

                        string editPositionHTML = "";

                        if (!screenDisabled && editSubdevisionPermission && editPositionPermission)
                            editPositionHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowPositionLightBox(" + subdivision.SubdivisionId.ToString() + ", " + position.PositionId.ToString() + ", '" + position.PositionName + "', '" + position.Activities + "', " + position.TotalPersonsCnt + ", " + position.FemaleCnt + @");"" />";

                        string goToRiskCardHTML = "";

                        if(!IsRiskCardHidden)
                            goToRiskCardHTML = @"<img src='../Images/list_edit.png' alt='Карта за оценка на риска за тази длъжност' title='Карта за оценка на риска за тази длъжност' class='GridActionIcon' onclick=""GoToRiskCard(" + position.PositionId.ToString() + @");"" />";


                        sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                                     <td style='" + cellStyle + @"'>" + positionCounter.ToString() + @"</td>
                        " + (!IsPositionNameHidden ? @"<td style='" + cellStyle + @"'>" + position.PositionName + @"</td>" : "") + @"
                        " + (!IsActivitiesHidden ? @"<td style='" + cellStyle + @"'>" + position.Activities + @"</td>" : "") + @"
                        " + (!IsTotalPersonsCntHidden ? @"<td style='" + cellStyle + @"'>" + position.TotalPersonsCnt.ToString() + @"</td>" : "") + @"
                        " + (!IsFemaleCntHidden ? @"<td style='" + cellStyle + @"'>" + position.FemaleCnt.ToString() + @"</td>" : "") + @"
                                                     <td style='" + cellStyle + @"'>" + editPositionHTML + goToRiskCardHTML + deletePositionHTML + @"</td>
                                                  </tr>");

                        positionCounter++;
                    }                    
                }

                counter++;
            }

            sb.Append(@"</table>");

            return sb.ToString();
        }

        // Saves Subdivision from ajax request
        private void JSSaveSubdivision()
        {
            string resultMsg = "";

            int subdivisionID = 0; 
            int.TryParse(Request.Form["SubdivisionID"], out subdivisionID);

            if (subdivisionID > 0 && !screenDisabled && !editSubdevisionPermission
                || !screenDisabled && !editSubdevisionPermission && !addPositionPermission)
            {
                RedirectAjaxAccessDenied();
            } 

            string subdivisionName = Request.Form["SubdivisionName"];

            Subdivision subdivision = new Subdivision(CurrentUser);
            subdivision.SubdivisionId = subdivisionID;
            subdivision.SubdivisionName = subdivisionName;
            subdivision.MilitaryUnitId = MilitaryUnitId;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_MilitaryUnitPositions");

            resultMsg = SubdivisionUtil.SaveSubdivision(subdivision, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes Subdivision by ajax request
        private void JSDeleteSubdivision()
        {
            int subdivisionID = 0; 
            int.TryParse(Request.Form["SubdivisionID"], out subdivisionID);

            if (subdivisionID == 0 || screenDisabled || !editSubdevisionPermission)
            {
                RedirectAjaxAccessDenied();
            }                

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_MilitaryUnitPositions");

                if (!SubdivisionUtil.DeleteSubdivision(subdivisionID, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на елемент!");
                }

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // Saves Position from ajax request
        private void JSSavePosition()
        {
            int positionID = 0; 
            int.TryParse(Request.Form["PositionID"], out positionID);

            if (positionID > 0 && !screenDisabled && !editSubdevisionPermission && !editPositionPermission
                || !screenDisabled && !editSubdevisionPermission && !addPositionPermission)
            {
                RedirectAjaxAccessDenied();   
            }

            string resultMsg = "";

            int subdivisionID = int.Parse(Request.Form["SubdivisionID"]);
            string positionName = Request.Form["PositionName"];
            string activities = Request.Form["Activities"];
            int? totalPersonsCnt = string.IsNullOrEmpty(Request.Form["TotalPersonsCnt"]) ? null : (int?)int.Parse(Request.Form["TotalPersonsCnt"]);
            int? femaleCnt = string.IsNullOrEmpty(Request.Form["FemaleCnt"]) ? null : (int?)int.Parse(Request.Form["FemaleCnt"]);


            Position position = new Position(CurrentUser);
            position.PositionId = positionID;
            position.PositionName = positionName;
            position.Activities = activities;
            position.TotalPersonsCnt = totalPersonsCnt;
            position.FemaleCnt = femaleCnt;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_MilitaryUnitPositions");

            resultMsg = PositionUtil.SavePosition(subdivisionID, position, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes Position by ajax request
        private void JSDeletePosition()
        {
            if (screenDisabled || !editSubdevisionPermission || !editPositionPermission)
            {
                RedirectAjaxAccessDenied();
            }

            int positionID = int.Parse(Request.Form["PositionID"]);
            int subdivisionID = int.Parse(Request.Form["SubdivisionID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_MilitaryUnitPositions");

                if (!PositionUtil.DeletePosition(subdivisionID, positionID, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на елемент!");
                }

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // hidden button for forced refresh of Subdivisions table
        protected void btnHdnRefreshSubdivisions_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        //Navigate back to the ManageProtocols screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }
    }
}
