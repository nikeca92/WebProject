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
    public partial class RiskFactorsMaintenance : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LIST_RISKFACTORS";
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

        //Getter/Setter of the ID of the displayed protocol(0 - if new)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("HS_LIST_RISKFACTORS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the correct menu item. It is specific for each maintenance record.
            HighlightMenuItems("Lists", "Lists_HS_RiskFactorTypes");

            RiskFactorType riskFactorType = RiskFactorTypeUtil.GetRiskFactorType(RiskFactorTypeId, CurrentUser);

            // Set risk factor type name on the page
            lblRiskFactorTypeName.InnerHtml = riskFactorType.Seq.ToString() + ". " + riskFactorType.RiskFactorTypeName;

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Process the ajax request for saving risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRiskFactor")
            {
                JSSaveRiskFactor();
                return;
            }

            //Process ajax request for deleting of risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRiskFactor")
            {
                JSDeleteRiskFactor();
                return;
            }

            if (!IsPostBack)
            {
                //Set up page title
                lblHeader.Text = "Фактори при оценка на риска";
                Page.Title = "Фактори при оценка на риска";

                //Load risk factors table
                LoadTable();
            }

            lblStatus.Text = "";
        }

        private void LoadTable()
        {
            divRiskFactorsTable.InnerHtml = "";
            List<RiskFactor> riskFactors = RiskFactorUtil.GetAllRiskFactorsByType(RiskFactorTypeId, CurrentUser);
            divRiskFactorsTable.InnerHtml = GenerateTableHTML(riskFactors);

            //Set the message if there is a need (e.g. a deleted, added or edited risk factor type)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblStatus.Text = "Факторът е изтрит успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "EDITED")
                {
                    lblStatus.Text = "Факторът е обновен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblStatus.Text = "Факторът е добавен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private string GenerateTableHTML(List<RiskFactor> riskFactors)
        {
            StringBuilder sb = new StringBuilder();

            bool screenDisabled = this.GetUIItemAccessLevel("HS_LIST_RISKFACTORS") == UIAccessLevel.Disabled;
            bool addDisabled = this.GetUIItemAccessLevel("HS_LIST_ADDRISKFACTOR") != UIAccessLevel.Enabled;
            bool editDisabled = this.GetUIItemAccessLevel("HS_LIST_EDITRISKFACTOR") != UIAccessLevel.Enabled;
            bool deleteDisabled = this.GetUIItemAccessLevel("HS_LIST_DELETERISKFACTOR") != UIAccessLevel.Enabled;

            string headerStyle = "vertical-align: bottom;";
            string addNewHtml = "";

            if (!screenDisabled && !addDisabled)
                addNewHtml = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на нов фактор' class='GridActionIcon' onclick=""ShowRiskFactorLightBox(0, '', '', 0);"" />";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 400px;" + headerStyle + @"'>Наименование</th>                               
                               <th style='width: 30px;" + headerStyle + @"'>Подредба</th>
                               <th style='width: 80px;" + headerStyle + @"'>" + (riskFactors.Count == 0 ? addNewHtml : "&nbsp;") + @"</th>
                            </tr>
                         </thead>");

            int counter = 1;

            foreach (RiskFactor riskFactor in riskFactors)
            {
                string cellStyle = "vertical-align: top;";

                string deleteHTML = "";

                if (riskFactor.CanDelete)
                {
                    if (!screenDisabled && !deleteDisabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този фактор' class='GridActionIcon' onclick=""DeleteRiskFactor(" + riskFactor.RiskFactorId.ToString() + ", '" + riskFactor.RiskFactorName + @"');"" />";
                }
                
                string editHTML = "";

                if (!screenDisabled && !editDisabled)
                    editHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowRiskFactorLightBox(" + riskFactor.RiskFactorId.ToString() + ", '" + riskFactor.RiskFactorName + "', '" + riskFactor.Seq.ToString() + @"', " + (riskFactor.AllowAddManually ? "1" : "0") + @");"" />";

                string hazardsHTML = "";

                if (this.GetUIItemAccessLevel("HS_LIST_HAZARDS") != UIAccessLevel.Hidden)
                    hazardsHTML = @"<img src='../Images/list_edit.png' alt='Потенциални опасности' title='Потенциални опасности' class='GridActionIcon' onclick=""EditHazards(" + RiskFactorTypeId.ToString() + ", " + riskFactor.RiskFactorId.ToString() + @")"" />";

                sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + riskFactor.RiskFactorName + @"</td>
                                 <td style='" + cellStyle + @"'>" + riskFactor.Seq.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + hazardsHTML + deleteHTML + (counter == riskFactors.Count ? addNewHtml : "") + @"</td>
                              </tr>");

                counter++;
            }

            sb.Append(@"</table>");

            return sb.ToString();
        }

        // Saves risk factor from ajax request
        private void JSSaveRiskFactor()
        {
            string resultMsg = "";

            int riskFactorID = int.Parse(Request.Form["RiskFactorID"]);
            int seq = int.Parse(Request.Form["Seq"]);
            string riskFactorName = Request.Form["RiskFactorName"];
            bool allowAddManually = Request.Form["AllowAddManually"] == "true";

            RiskFactor riskFactor = new RiskFactor(riskFactorID, riskFactorName, seq, allowAddManually, CurrentUser);            

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_RiskFactors");

            resultMsg = RiskFactorUtil.SaveRiskFactor(RiskFactorTypeId, riskFactor, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes risk factor by ajax request
        private void JSDeleteRiskFactor()
        {
            int riskFactorId = int.Parse(Request.Form["RiskFactorID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_Lists_RiskFactors");

                if (!RiskFactorUtil.DeleteRiskFactor(RiskFactorTypeId, riskFactorId, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на фактор!");
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
        protected void btnHdnRefreshRiskFactors_Click(object sender, EventArgs e)
        {
            LoadTable();
        }
    
        //When clicking Cancel then redirect to the Home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/RiskFactorTypesMaintenance.aspx");
        }
    }
}
