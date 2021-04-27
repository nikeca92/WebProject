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
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class IndicatorsMaintenance : HSPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LIST_INDICATORS";
            }
        }

        //Getter/Setter of the ID of the displayed protocol(0 - if new)
        private int IndicatorTypeId
        {
            get
            {
                int indicatorTypeId = 0;
                //gets IndicatorTypeID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfIndicatorTypeID.Value)
                    || this.hfIndicatorTypeID.Value == "0")
                {
                    if (Request.Params["IndicatorTypeID"] != null)
                        int.TryParse(Request.Params["IndicatorTypeID"].ToString(), out indicatorTypeId);

                    //sets IndicatorTypeID in hidden field on the page in order to be accessible in javascript
                    this.hfIndicatorTypeID.Value = indicatorTypeId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfIndicatorTypeID.Value, out indicatorTypeId);
                }

                return indicatorTypeId;
            }
            set { this.hfIndicatorTypeID.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //Process the ajax request for saving indicator
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveIndicator")
            {
                JSSaveIndicator();
                return;
            }

            //Process ajax request for deleting of indicator
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteIndicator")
            {
                JSDeleteIndicator();
                return;
            }

            if (IndicatorTypeId == 0 ||
                this.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("HS_LIST_INDICATORS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            // Disable add button if the user has no rights
            SetBtnNew();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Lists", "Lists_HS_IndicatorTypes");

            // Set indicator type name on the page
            lblIndicatorTypeName.InnerHtml = WCondIndicatorTypeUtil.GetIndicatorType(IndicatorTypeId, CurrentUser).IndicatorTypeName;

            //Setup some basic styles on the screen
            SetupStyle();

            int allRows = WCondIndicatorUtil.GetAllIndicatorsByTypeCount(IndicatorTypeId, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {              
                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Initial load
                hdnPageIdx.Value = "1";
                RefreshIndicators();
            }

            lblGridMessage.Text = "";
        }            
     
        //Setup some styling on the page
        private void SetupStyle()
        {
            lblIndicatorTypeName.Style.Add("vertical-align", "top");            
        }                

        //Refresh the data grid
        private void RefreshIndicators()
        {
            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Get the list of indicators according to the specified filters, order and paging
            List<WCondIndicator> indicators = WCondIndicatorUtil.GetAllIndicatorsByType(IndicatorTypeId, orderBy, pageIdx, pageLength, CurrentUser);

            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "" };
            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 380px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Наименование" + arrOrderCol[0] + @"</th>                               
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (WCondIndicator indicator in indicators)
            {
                string cellStyle = "vertical-align: top;";

                string deleteHTML = "";

                if (indicator.CanDelete)
                {
                    if (this.GetUIItemAccessLevel("HS_LIST_DELETEINDICATOR") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_LIST_INDICATORS") == UIAccessLevel.Enabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този показател' class='GridActionIcon' onclick=""DeleteIndicator(" + indicator.IndicatorId.ToString() + ",'" + indicator.IndicatorName + @"');"" />";
                }

                string editHTML = "";

                if (this.GetUIItemAccessLevel("HS_LIST_EDITINDICATOR") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_LIST_INDICATORS") == UIAccessLevel.Enabled)
                    editHTML = @"<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowIndicatorLightBox(" + indicator.IndicatorId.ToString() + ",'" + indicator.IndicatorName + @"');"" />";


                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + indicator.IndicatorName + @"</td>                               
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                counter++;
            }

            html += "</table>";


            //Put the generated grid on the page
            pnlIndicatorsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted, added or edited indicator)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Показателят е изтрит успешно!";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "EDITED")
                {
                    lblGridMessage.Text = "Показателят е обновен успешно!";
                    lblGridMessage.CssClass = "SuccessText";
                }
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblGridMessage.Text = "Показателят е добавен успешно!";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = "1";
            RefreshIndicators();
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = "1";
            RefreshIndicators();
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hdnPageIdx.Value);

            if (page > 1)
            {
                page--;
                hdnPageIdx.Value = page.ToString();

                RefreshIndicators();
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hdnPageIdx.Value);

            if (page < maxPage)
            {
                page++;
                hdnPageIdx.Value = page.ToString();

                RefreshIndicators();
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = maxPage.ToString();
            RefreshIndicators();
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            int gotoPage;
            if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
            {
                hdnPageIdx.Value = gotoPage.ToString();
                RefreshIndicators();
            }
        }

        // hidden button for forced refresh of indicators table
        protected void btnHdnRefreshIndicators_Click(object sender, EventArgs e)
        {
            RefreshIndicators();
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            int page = int.Parse(hdnPageIdx.Value);

            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to indicator types maintenance page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/IndicatorTypesMaintenance.aspx");
        }

        // Saves indicator from ajax request
        private void JSSaveIndicator()
        {
            string resultMsg = "";

            int indicatorID = int.Parse(Request.Form["IndicatorID"]);
            int indicatorTypeID = int.Parse(Request.Form["IndicatorTypeID"]);
            string indicatorName = Request.Form["IndicatorName"];

            WCondIndicator indicator = new WCondIndicator(indicatorID, indicatorName);

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_Indicators");

            resultMsg = WCondIndicatorUtil.SaveIndicator(indicatorTypeID, new WCondIndicator(indicatorID, indicatorName), CurrentUser, change) ? AJAXTools.OK : "Неуспешна операция";

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes indicator by ajax request
        private void JSDeleteIndicator()
        {
            if (this.GetUIItemAccessLevel("HS_LIST_DELETEINDICATOR") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_LIST_INDICATORS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int indicatorId = int.Parse(Request.Form["IndicatorID"]);
            int indicatorTypeID = int.Parse(Request.Form["IndicatorTypeID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail
                Change change = new Change(CurrentUser, "HS_Lists_Indicators");

                if (!WCondIndicatorUtil.DeleteIndicator(indicatorTypeID, indicatorId, CurrentUser, change))
                {
                    throw new Exception("Неуспешно изтриване на показател!");
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

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("HS_LIST_ADDINDICATOR") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_LIST_INDICATORS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_LIST_ADDINDICATOR") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }
    }
}
