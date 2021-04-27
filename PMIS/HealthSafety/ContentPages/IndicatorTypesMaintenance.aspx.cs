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
    public partial class IndicatorTypesMaintenance : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LIST_INDICATORTYPES";
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
            if (this.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the correct menu item. It is specific for each maintenance record.
            HighlightMenuItems("Lists", "Lists_HS_IndicatorTypes");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Process the ajax request for saving indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveIndicatorType")
            {
                JSSaveIndicatorType();
                return;
            }

            //Process ajax request for deleting of indicator type
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteIndicatorType")
            {
                JSDeleteIndicatorType();
                return;
            }

            if (!IsPostBack)
            {
                //Set up page title
                lblHeader.Text = "Елементи на специфичните условия на труд";
                Page.Title = "Елементи на специфичните условия на труд";

                //Load indicator types table
                LoadTable();
            }

            lblStatus.Text = "";
        }

        private void LoadTable()
        {
            divIndicatorTypesTable.InnerHtml = "";
            List<WCondIndicatorType> indicatorTypes = WCondIndicatorTypeUtil.GetAllIndicatorTypes(CurrentUser);
            divIndicatorTypesTable.InnerHtml = GenerateTableHTML(indicatorTypes);

            //Set the message if there is a need (e.g. a deleted, added or edited indicator type)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblStatus.Text = "Елементът е изтрит успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "EDITED")
                {
                    lblStatus.Text = "Елементът е обновен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblStatus.Text = "Елементът е добавен успешно!";
                    lblStatus.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private string GenerateTableHTML(List<WCondIndicatorType> indicatorTypes)
        {
            StringBuilder sb = new StringBuilder();

            bool screenDisabled = this.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") == UIAccessLevel.Disabled;
            bool addDisabled = this.GetUIItemAccessLevel("HS_LIST_ADDINDICATORTYPE") != UIAccessLevel.Enabled;
            bool editDisabled = this.GetUIItemAccessLevel("HS_LIST_EDITINDICATORTYPE") != UIAccessLevel.Enabled;
            bool deleteDisabled = this.GetUIItemAccessLevel("HS_LIST_DELETEINDICATORTYPE") != UIAccessLevel.Enabled;

            string headerStyle = "vertical-align: bottom;";
            string addNewHtml = "";

            if (!screenDisabled && !addDisabled)
                addNewHtml = @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на нов елемент' class='GridActionIcon' onclick=""ShowIndicatorTypeLightBox(0, '', '');"" />";

            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 180px;" + headerStyle + @"'>Наименование</th>                               
                               <th style='width: 30px;" + headerStyle + @"'>Подредба</th>
                               <th style='width: 80px;" + headerStyle + @"'>" + (indicatorTypes.Count == 0 ? addNewHtml : "&nbsp;") + @"</th>
                            </tr>
                         </thead>");

            int counter = 1;

            foreach (WCondIndicatorType indicatorType in indicatorTypes)
            {
                string cellStyle = "vertical-align: top;";

                string deleteHTML = "";

                if (indicatorType.CanDelete)
                {
                    if (!screenDisabled && !deleteDisabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този елемент' class='GridActionIcon' onclick=""DeleteIndicatorType(" + indicatorType.IndicatorTypeId.ToString() + ", '" + indicatorType.IndicatorTypeName + @"');"" />";
                }
                
                string editHTML = "";

                if (!screenDisabled && !editDisabled)
                    editHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowIndicatorTypeLightBox(" + indicatorType.IndicatorTypeId.ToString() + ", '" + indicatorType.IndicatorTypeName + "', '" + indicatorType.Seq.ToString() + @"');"" />";

                string indicatorsHTML = "";
                
                if (this.GetUIItemAccessLevel("HS_LIST_INDICATORS") != UIAccessLevel.Hidden)
                    indicatorsHTML = @"<img src='../Images/list_edit.png' alt='Показатели' title='Показатели' class='GridActionIcon' onclick=""EditIndicators(" + indicatorType.IndicatorTypeId.ToString() + @")"" />";

                sb.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + indicatorType.IndicatorTypeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + indicatorType.Seq.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + indicatorsHTML + deleteHTML + (counter == indicatorTypes.Count ? addNewHtml : "") + @"</td>
                              </tr>");

                counter++;
            }

            sb.Append(@"</table>");

            return sb.ToString();
        }

        // Saves indicator type from ajax request
        private void JSSaveIndicatorType()
        {
            string resultMsg = "";

            int indicatorTypeID = int.Parse(Request.Form["IndicatorTypeID"]);
            int seq = int.Parse(Request.Form["Seq"]);
            string indicatorTypeName = Request.Form["IndicatorTypeName"];

            WCondIndicatorType indicatorType = new WCondIndicatorType(indicatorTypeID, indicatorTypeName, seq, CurrentUser);            

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_IndicatorTypes");            

            resultMsg = WCondIndicatorTypeUtil.SaveIndicatorType(indicatorType, CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes indicator type by ajax request
        private void JSDeleteIndicatorType()
        {
            int indicatorTypeId = int.Parse(Request.Form["IndicatorTypeID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_Lists_IndicatorTypes");

                if (!WCondIndicatorTypeUtil.DeleteIndicatorType(indicatorTypeId, CurrentUser, change))
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

        // hidden button for forced refresh of indicator types table
        protected void btnHdnRefreshIndicatorTypes_Click(object sender, EventArgs e)
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
