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
    public partial class ReportStaffPositionsList : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = 30;

        //Get the config setting that says how many rows per page should be dispayed in the grid
        private string sessionResultsKey = "ReportStaffPositionsList";

        private ReportStaffPositionsListResult reportResult = null;
        private ReportStaffPositionsListResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportStaffPositionsListResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportStaffPositionsListFilter filter = CollectFilterData();
                        reportResult = ReportStaffPositionsListUtil.GetReportStaffPositionsList(filter, CurrentUser);
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
                return "RES_REPORTS_REPORTSTAFFPOSITIONLIST";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportStaffPositionsList");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                Session[sessionResultsKey] = null;

                lblMilitaryUnit.Text = CommonFunctions.GetLabelText("MilitaryUnit") + ":";

                //Populate any drop-downs and list-boxes
                PopulateLists();
            
                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryCommands();
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") == UIAccessLevel.Hidden)
                return;

            divNavigation.Visible = true;

            string html = "";

            if (ReportResult.Result != null && ReportResult.Result.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Щатно-длъжностен списък:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th> 
                               <th style='width: 400px;' colspan='4'>Данни по щат</th>
                               <th style='width: 160px;' rowspan='2'>Звание, име, презиме, фамилия</th>
                               <th style='width: 80px;' rowspan='2'>ВОС</th>
                               <th style='width: 100px;' rowspan='2'>ЕГН</th>
                               <th style='width: 180px;' rowspan='2'>Постоянен адрес</th>
                               <th style='width: 80px;' rowspan='2'>Образование</th>
                               <th style='width: 80px;' rowspan='2'>От кое военно окръжие е получил назначението</th>
                               <th style='width: 80px;' rowspan='2'>Полагащо се по щат оръжие</th>
                               <th style='width: 80px;' rowspan='2'>Дата на приемане</th>
                            </tr>
                            <tr>
                               <th style='width: 160px;'>Наименование на длъжността</th>
                               <th style='width: 80px;'>Звание</th>
                               <th style='width: 80px;'>ВОС</th>
                               <th style='width: 80px;'>Код на длъжността</th>
                            </tr>
                         </thead>";

                foreach (ReportStaffPositionsListBlock block in ReportResult.PagedResult)
                {
                    html += @"<tr class='" + (block.RowIndex % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.RowIndex.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PositionName + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryRankName + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.StaffMRS + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PositionCode + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PersonFullNameAndRank + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PersonMRS + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PersonIdentityNumber + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PersonPermAddress + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.PersonEducation + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryDepartment + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.StaffWeapon + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.FormatDate(block.AppointmentDate) + @"</td>                                 
                              </tr>";
                }

                html += "</table>";
            }
            else
            {
                divNavigation.Visible = false;
                html += @"<div style='text-align: center;'>Няма намерени данни</div>";
            }

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + ReportResult.MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";

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
                hdnPageIdx.Value = "1";
                ReportResult.Filter.PageIdx = 1;
                RefreshReport();
            }
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
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            divNavigation.Visible = false;
            pnlReportGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            PopulateMilitaryCommands();

            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        private ReportStaffPositionsListFilter CollectFilterData()
        {
            ReportStaffPositionsListFilter filter = new ReportStaffPositionsListFilter();

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 1;

            hdnPageIdx.Value = pageIdx.ToString();

            filter.PageIdx = pageIdx;
            filter.PageSize = pageLength;

            filter.MilitaryUnitId = String.IsNullOrEmpty(msMilitaryUnit.SelectedValue) ? 0 : int.Parse(msMilitaryUnit.SelectedValue);
            filter.MilitaryCommandId = ddMilitaryCommand.SelectedValue == ListItems.GetOptionChooseOne().Value ? 0 : int.Parse(ddMilitaryCommand.SelectedValue);
            filter.SubMilitaryCommandSuffix = ddSubMilitaryCommand.SelectedValue == ListItems.GetOptionAll().Value ? "" : ddSubMilitaryCommand.SelectedValue;

            return filter;
        }

        protected void hdnBtnReloadMilitaryCommands_Click(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
        }

        private void PopulateMilitaryCommands()
        {
            ddMilitaryCommand.Items.Clear();
            ddMilitaryCommand.Items.Add(ListItems.GetOptionChooseOne());

            int militaryUnitId = Int32.Parse(msMilitaryUnit.SelectedValue);
            List<MilitaryCommand> militaryCommands = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryUnitAndChildren(CurrentUser, militaryUnitId);

            foreach (MilitaryCommand militaryCommand in militaryCommands)
            {
                ddMilitaryCommand.Items.Add(new ListItem(militaryCommand.DisplayTextForSelection, militaryCommand.MilitaryCommandId.ToString()));
            }

            ddMilitaryCommand_Changed(hdnBtnReloadMilitaryCommands, new EventArgs());
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            var subMilitaryCommands = RequestCommandUtil.GetRequestCommandsForMilCommand(CurrentUser, int.Parse(ddMilitaryCommand.SelectedValue));

            ddSubMilitaryCommand.DataSource = subMilitaryCommands;
            ddSubMilitaryCommand.DataTextField = "Txt";
            ddSubMilitaryCommand.DataValueField = "Val";
            ddSubMilitaryCommand.DataBind();

            if (subMilitaryCommands.Count > 0)
                ddSubMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());
            else
                ddSubMilitaryCommand.Items.Insert(0, ListItems.GetOptionChooseOne());
        }
    }
}
