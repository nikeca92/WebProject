using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
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
    /*THIS PAGE IS NOT USED BY THE USERS
      It is used only by developers by manually typing the URL in the browser. 
      It was created on 2016-12-07 (ticket #162) in order to check the execution time if we return all the reservists as a list in C#.
      If it was faster then we could re-write the report and count the reservists by their statuses in C#. However, it was still slow (even a bit slower).
      For example, about for 230,000 reservists the old SQL-only code was taking about 20 seconds. And this new page returned the full list of resevists in C# for about 25-30 seconds.
       
      Finally, we found a way how to optimize the original report (A33v2): We changed the inner "core" part of the SQL which was returning the full list of reservists to 
      group the records by the various "status" columns and return the COUNT(*) of reservists in each status. Then we changed the outer SQL query to SUM(ReservistsCnt) instead of SUM(1).
      Locally, it seems to be much faster (about 1-2 seconds for the same nubmer of rows).
     
      So, this page is probably not needed anymore. But we are keeping it for the time being... it looks handy. In the future we can freely delete it.*/
    public partial class ReportA33v2_BENCHMARK : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = 0;
        private string sessionResultsKey = "ReportA33v2Result_BENCHMARK";

        private ReportA33v2Result_BENCHMARK reportResult = null;
        private ReportA33v2Result_BENCHMARK ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        DateTime pullSessionStart = BenchmarkLog.WriteStart("\tНачало на извличане на справката от сесията", CurrentUser, Request);
                        reportResult = (ReportA33v2Result_BENCHMARK)Session[sessionResultsKey];
                        BenchmarkLog.WriteEnd("\tКрай на извличане на справката от сесията", CurrentUser, Request, pullSessionStart);
                    }
                    else
                    {
                        DateTime pullFilterStart = BenchmarkLog.WriteStart("\tНачало на извличане на филтъра", CurrentUser, Request);
                        ReportA33v2Filter filter = CollectFilterData();
                        BenchmarkLog.WriteEnd("\tКрай на извличане на филтъра", CurrentUser, Request, pullFilterStart);

                        DateTime pullReportStart = BenchmarkLog.WriteStart("\tНачало на извличане на данните", CurrentUser, Request);
                        reportResult = ReportA33v2Util_BENCHMARK.GetReportA33v2(filter, CurrentUser);
                        var t = 0;
                        for (int i = 0; i < reportResult.Rows.Count; i++)
                        {
                            t += reportResult.Rows[i].ReservistID;
                        }
                        BenchmarkLog.WriteEnd("\tКрай на извличане на данните", CurrentUser, Request, pullReportStart);

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
                return "RES_REPORTS_REPORTA33v2";
            }
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            DateTime? pageStart = null;
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request);
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportA33v2_BENCHMARK");

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
           
            ddRegion.Attributes.Add("onchange", "SetPickListsSelection();");
            ddMuniciplaity.Attributes.Add("onchange", "SetPickListsSelection();");
            ddCity.Attributes.Add("onchange", "SetPickListsSelection();");  
           
            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, pageStart.Value);
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();        
            PopulateRegions();
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

        //Populate ddRegion
        private void PopulateRegions()
        {
            ddRegion.DataSource = RegionUtil.GetRegions(CurrentUser);
            ddRegion.DataTextField = "RegionName";
            ddRegion.DataValueField = "RegionId";
            ddRegion.DataBind();
            ddRegion.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }
        }

        //Populate ddMuniciplaity
        private void PopulateMunicipalities(int regionID)
        {
            ddMuniciplaity.DataSource = MunicipalityUtil.GetMunicipalities(regionID, CurrentUser); ;
            ddMuniciplaity.DataTextField = "MunicipalityName";
            ddMuniciplaity.DataValueField = "MunicipalityId";
            ddMuniciplaity.DataBind();
            ddMuniciplaity.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddMuniciplaity_Changed(object sender, EventArgs e)
        {
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }
        }

        //Populate ddCity
        private void PopulateCities(int municipalityID)
        {
            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionAll());

            // Initialize ddDistrict with blank value
            ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddDistrict
        protected void ddCity_Changed(object sender, EventArgs e)
        {
            ddDistrict.Items.Clear();

            if (ddCity.SelectedValue != "-1")
            {
                int cityId = int.Parse(ddCity.SelectedValue);

                ddDistrict.DataSource = DistrictUtil.GetDistricts(cityId, CurrentUser);
                ddDistrict.DataTextField = "DistrictName";
                ddDistrict.DataValueField = "DistrictId";
                ddDistrict.DataBind();

                if (ddDistrict.Items.Count > 0)
                    ddDistrict.Items.Insert(0, ListItems.GetOptionAll());
                else
                    ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
            }
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden)
                return;

            DateTime renderStart = BenchmarkLog.WriteStart("\tНачало на генериране на изхода", CurrentUser, Request);

            pnlPaging.Visible = true;

            StringBuilder html = new StringBuilder();

            //Get the list of postpones according to the specified filters, order and paging
            List<ReportA33v2Row_BENCHMARK> reportBlocks = ReportResult.Rows;

            //No data found
            if (reportBlocks.Count == 0)
            {
                html.Append("<span>Няма намерени резултати</span>");
                this.btnPrintReport.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                html.Append("Редове: " + reportBlocks.Count.ToString());
            }

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + ReportResult.MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";
            
            this.pnlReportGrid.InnerHtml = html.ToString();

            BenchmarkLog.WriteEnd("\tКрай на генериране на изхода", CurrentUser, Request, renderStart);
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                ReportResult.Filter.PageIdx = 1;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                ReportResult.Filter.PageIdx = 1;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page > 1)
                {
                    page--;
                    hdnPageIdx.Value = page.ToString();
                    ReportResult.Filter.PageIdx = page;

                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < ReportResult.MaxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();
                    ReportResult.Filter.PageIdx = page;

                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = ReportResult.MaxPage.ToString();
                ReportResult.Filter.PageIdx = ReportResult.MaxPage;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= ReportResult.MaxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    ReportResult.Filter.PageIdx = gotoPage;
                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
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

            if (page == ReportResult.MaxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            hdnMilitaryDepartmentSelected.Value = "";            
            
            pnlPaging.Visible = false;
            pnlReportGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва - ТЕСТ'", CurrentUser, Request, postBackStart.Value);
        }

        private ReportA33v2Filter CollectFilterData()
        {
            ReportA33v2Filter filter = new ReportA33v2Filter();

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 1;

            hdnPageIdx.Value = pageIdx.ToString();

            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;
                this.hdnRegionId.Value = this.ddRegion.SelectedValue;
            }
            else
            {
                this.hdnRegionId.Value = "";
            }

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;
                this.hdnMunicipalityId.Value = this.ddMuniciplaity.SelectedValue;
            }
            else
            {
                this.hdnMunicipalityId.Value = "";
            }

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;
                this.hdnCityId.Value = this.ddCity.SelectedValue;
            }
            else
            {
                this.hdnCityId.Value = "";
            }

            string district = "";

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                district = ddDistrict.SelectedValue;
                this.hdnDistrictId.Value = this.ddDistrict.SelectedValue;
            }
            else
            {
                this.hdnDistrictId.Value = "";
            }

            this.hdnPostCode.Value = txtPostCode.Text;
            this.hdnAddress.Value = txtAddress.Text;
           
            filter.PageIdx = pageIdx;
            filter.PageSize = pageLength;

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.District = district;
            filter.Address = txtAddress.Text;
            filter.PostCode = txtPostCode.Text;
            
            return filter;
        }
    }
}
