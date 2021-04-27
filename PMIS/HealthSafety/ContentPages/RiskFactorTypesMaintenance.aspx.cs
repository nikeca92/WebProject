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
    public partial class RiskFactorTypesMaintenance : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LIST_RISKFACTORTYPES";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("HS_LIST_RISKFACTORTYPES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the correct menu item. It is specific for each maintenance record.
            HighlightMenuItems("Lists", "Lists_HS_RiskFactorTypes");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Process the ajax request for saving risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRiskFactorType")
            {
                JSSaveRiskFactorType();
                return;
            }

            //Process ajax request for deleting of risk factor type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRiskFactorType")
            {
                JSDeleteRiskFactorType();
                return;
            }

            if (!IsPostBack)
            {
                //Set up page title
                lblHeader.Text = "Видове фактори при оценка на риска";
                Page.Title = "Видове фактори при оценка на риска";

                //Load risk factor types table
                LoadTable();
            }

            lblStatus.Text = "";
        }

        private void LoadTable()
        {
            divRiskFactorTypesTable.InnerHtml = "";
            List<RiskFactorType> riskFactorTypes = RiskFactorTypeUtil.GetAllRiskFactorTypes(CurrentUser);
            divRiskFactorTypesTable.InnerHtml = GenerateTableHTML(riskFactorTypes);

            //Set the message if there is a need (e.g. a deleted, added or edited risk factor type)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblStatus.Text = "Видът фактор е изтрит успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "EDITED")
                {
                    lblStatus.Text = "Видът фактор е обновен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblStatus.Text = "Видът фактор е добавен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private string GenerateTableHTML(List<RiskFactorType> riskFactorTypes)
        {
            StringBuilder sb = new StringBuilder();

            bool screenDisabled = this.GetUIItemAccessLevel("HS_LIST_RISKFACTORTYPES") == UIAccessLevel.Disabled;
            bool addDisabled = this.GetUIItemAccessLevel("HS_LIST_ADDRISKFACTORTYPE") != UIAccessLevel.Enabled;
            bool editDisabled = this.GetUIItemAccessLevel("HS_LIST_EDITRISKFACTORTYPE") != UIAccessLevel.Enabled;
            bool deleteDisabled = this.GetUIItemAccessLevel("HS_LIST_DELETERISKFACTORTYPE") != UIAccessLevel.Enabled;

            string headerStyle = "vertical-align: bottom;";
            string addNewHtml = "";

            if (!screenDisabled && !addDisabled)
                addNewHtml = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на нов вид фактор' class='GridActionIcon' onclick=""ShowRiskFactorTypeLightBox(0, '', '');"" />";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 300px;" + headerStyle + @"'>Наименование</th>                               
                               <th style='width: 30px;" + headerStyle + @"'>Подредба</th>
                               <th style='width: 80px;" + headerStyle + @"'>" + (riskFactorTypes.Count == 0 ? addNewHtml : "&nbsp;") + @"</th>
                            </tr>
                         </thead>");

            int counter = 1;

            foreach (RiskFactorType riskFactorType in riskFactorTypes)
            {
                string cellStyle = "vertical-align: top;";

                string deleteHTML = "";

                if (riskFactorType.CanDelete)
                {
                    if (!screenDisabled && !deleteDisabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този вид фактор' class='GridActionIcon' onclick=""DeleteRiskFactorType(" + riskFactorType.RiskFactorTypeId.ToString() + ", '" + riskFactorType.RiskFactorTypeName + @"');"" />";
                }
                
                string editHTML = "";

                if (!screenDisabled && !editDisabled)
                    editHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowRiskFactorTypeLightBox(" + riskFactorType.RiskFactorTypeId.ToString() + ", '" + riskFactorType.RiskFactorTypeName + "', '" + riskFactorType.Seq.ToString() + @"');"" />";

                string riskFactorsHTML = "";

                if (this.GetUIItemAccessLevel("HS_LIST_RISKFACTORS") != UIAccessLevel.Hidden)
                    riskFactorsHTML = @"<img src='../Images/list_edit.png' alt='Фактори' title='Фактори' class='GridActionIcon' onclick=""EditRiskFactors(" + riskFactorType.RiskFactorTypeId.ToString() + @")"" />";

                sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + riskFactorType.RiskFactorTypeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + riskFactorType.Seq.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + riskFactorsHTML + deleteHTML + (counter == riskFactorTypes.Count ? addNewHtml : "") + @"</td>
                              </tr>");

                counter++;
            }

            sb.Append(@"</table>");

            return sb.ToString();
        }

        // Saves risk factor type from ajax request
        private void JSSaveRiskFactorType()
        {
            string resultMsg = "";

            int riskFactorTypeID = int.Parse(Request.Form["RiskFactorTypeID"]);
            int seq = int.Parse(Request.Form["Seq"]);
            string riskFactorTypeName = Request.Form["RiskFactorTypeName"];

            RiskFactorType riskFactorType = new RiskFactorType(riskFactorTypeID, riskFactorTypeName, seq, CurrentUser);            

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_RiskFactorTypes");

            resultMsg = RiskFactorTypeUtil.SaveRiskFactorType(riskFactorType, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes risk factor type by ajax request
        private void JSDeleteRiskFactorType()
        {
            int riskFactorTypeId = int.Parse(Request.Form["RiskFactorTypeID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_Lists_RiskFactorTypes");

                if (!RiskFactorTypeUtil.DeleteRiskFactorType(riskFactorTypeId, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на вид фактор!");
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

        // hidden button for forced refresh of risk factor types table
        protected void btnHdnRefreshRiskFactorTypes_Click(object sender, EventArgs e)
        {
            LoadTable();
        }
    
        //When clicking Cancel then redirect to the Home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }
    }
}
