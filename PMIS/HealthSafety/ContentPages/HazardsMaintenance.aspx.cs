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

using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Text;
using System.Drawing;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class HazardsMaintenance : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LIST_HAZARDS";
            }
        }

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hfFromHome.Value)
                    || this.hfFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hfFromHome.Value = fh.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hfFromHome.Value = value.ToString();
            }
        }
        
        private int RiskFactorTypeId
        {
            get
            {
                int riskFactorTypeId = 0;
                //gets RiskFactorTypeID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfRiskFactorTypeID.Value)
                    || this.hfRiskFactorTypeID.Value == "0")
                {
                    if (Request.Params["RiskFactorTypeID"] != null)
                        int.TryParse(Request.Params["RiskFactorTypeID"].ToString(), out riskFactorTypeId);

                    //sets RiskFactorTypeID in hidden field on the page in order to be accessible in javascript
                    this.hfRiskFactorTypeID.Value = riskFactorTypeId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfRiskFactorTypeID.Value, out riskFactorTypeId);
                }

                return riskFactorTypeId;
            }
            set { this.hfRiskFactorTypeID.Value = value.ToString(); }
        }

        private int RiskFactorId
        {
            get
            {
                int riskFactorId = 0;
                //gets RiskFactorID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfRiskFactorID.Value)
                    || this.hfRiskFactorTypeID.Value == "0")
                {
                    if (Request.Params["RiskFactorID"] != null)
                        int.TryParse(Request.Params["RiskFactorID"].ToString(), out riskFactorId);

                    //sets RiskFactorID in hidden field on the page in order to be accessible in javascript
                    this.hfRiskFactorID.Value = riskFactorId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfRiskFactorID.Value, out riskFactorId);
                }

                return riskFactorId;
            }
            set { this.hfRiskFactorID.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("HS_LIST_HAZARDS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the correct menu item. It is specific for each maintenance record.
            HighlightMenuItems("Lists", "Lists_HS_RiskFactorTypes");

            RiskFactorType riskFactorType = RiskFactorTypeUtil.GetRiskFactorType(RiskFactorTypeId, CurrentUser);

            // Set risk factor type name on the page
            lblRiskFactorTypeName.InnerHtml = riskFactorType.Seq.ToString() + ". " + riskFactorType.RiskFactorTypeName;

            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(RiskFactorId, CurrentUser);

            // Set risk factor name on the page
            lblRiskFactorName.InnerHtml = riskFactorType.Seq.ToString() + "." + riskFactor.Seq.ToString() + ". " + riskFactor.RiskFactorName;

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Process the ajax request for saving risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveHazard")
            {
                JSSaveHazard();
                return;
            }

            //Process ajax request for deleting of risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteHazard")
            {
                JSDeleteHazard();
                return;
            }

            if (!IsPostBack)
            {
                //Set up page title
                lblHeader.Text = "Потенциални опасности при оценка на риска";
                Page.Title = "Потенциални опасности при оценка на риска";

                //Load risk factors table
                LoadTable();
            }

            lblStatus.Text = "";
        }

        private void LoadTable()
        {
            divHazardsTable.InnerHtml = "";
            List<Hazard> hazards = HazardUtil.GetAllHazardsByFactor(RiskFactorId, CurrentUser);
            divHazardsTable.InnerHtml = GenerateTableHTML(hazards);

            //Set the message if there is a need (e.g. a deleted, added or edited risk factor type)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblStatus.Text = "Потенциалната опасност е изтрита успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "EDITED")
                {
                    lblStatus.Text = "Потенциалната опасност е обновена успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblStatus.Text = "Потенциалната опасност е добавена успешно!";
                    lblStatus.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private string GenerateTableHTML(List<Hazard> hazards)
        {
            StringBuilder sb = new StringBuilder();

            bool screenDisabled = this.GetUIItemAccessLevel("HS_LIST_HAZARDS") == UIAccessLevel.Disabled;
            bool addDisabled = this.GetUIItemAccessLevel("HS_LIST_ADDHAZARD") != UIAccessLevel.Enabled;
            bool editDisabled = this.GetUIItemAccessLevel("HS_LIST_EDITHAZARD") != UIAccessLevel.Enabled;
            bool deleteDisabled = this.GetUIItemAccessLevel("HS_LIST_DELETEHAZARD") != UIAccessLevel.Enabled;

            string headerStyle = "vertical-align: bottom;";
            string addNewHtml = "";

            if (!screenDisabled && !addDisabled)
                addNewHtml = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на нова потенциална опасност' class='GridActionIcon' onclick=""ShowHazardLightBox(0, '', '');"" />";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 400px;" + headerStyle + @"'>Наименование</th>                               
                               <th style='width: 30px;" + headerStyle + @"'>Подредба</th>
                               <th style='width: 80px;" + headerStyle + @"'>" + (hazards.Count == 0 ? addNewHtml : "&nbsp;") + @"</th>
                            </tr>
                         </thead>");

            int counter = 1;

            foreach (Hazard hazard in hazards)
            {
                string cellStyle = "vertical-align: top;";

                string deleteHTML = "";

                if (hazard.CanDelete)
                {
                    if (!screenDisabled && !deleteDisabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази потенциална опасност' class='GridActionIcon' onclick=""DeleteHazard(" + hazard.HazardId.ToString() + ", '" + hazard.HazardName + @"');"" />";
                }
                
                string editHTML = "";

                if (!screenDisabled && !editDisabled)
                    editHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowHazardLightBox(" + hazard.HazardId.ToString() + ", '" + hazard.HazardName + "', " + hazard.Seq.ToString() + @");"" />";
                               
                sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + hazard.HazardName + @"</td>
                                 <td style='" + cellStyle + @"'>" + hazard.Seq.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + (counter == hazards.Count ? addNewHtml : "") + @"</td>
                              </tr>");

                counter++;
            }

            sb.Append(@"</table>");

            return sb.ToString();
        }

        // Saves Hazard from ajax request
        private void JSSaveHazard()
        {
            string resultMsg = "";

            int hazardID = int.Parse(Request.Form["HazardID"]);
            int seq = int.Parse(Request.Form["Seq"]);
            string hazardName = Request.Form["HazardName"];

            Hazard hazard = new Hazard(hazardID, hazardName, seq, CurrentUser);            

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_Hazards");

            resultMsg = HazardUtil.SaveHazard(RiskFactorTypeId, RiskFactorId, hazard, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes Hazard by ajax request
        private void JSDeleteHazard()
        {
            int hazardId = int.Parse(Request.Form["HazardID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_Lists_Hazards");

                if (!HazardUtil.DeleteHazard(RiskFactorTypeId, RiskFactorId, hazardId, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на потенциална опасност!");
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

        // hidden button for forced refresh of risk factors table
        protected void btnHdnRefreshHazards_Click(object sender, EventArgs e)
        {
            LoadTable();
        }
    
        //When clicking Cancel then redirect to the Home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/RiskFactorsMaintenance.aspx?RiskFactorTypeID=" + RiskFactorTypeId.ToString());
        }
    }
}
