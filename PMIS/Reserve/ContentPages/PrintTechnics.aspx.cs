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
using System.Collections;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;


namespace PMIS.Reserve.ContentPages
{
    public partial class PrintTechnics : RESPage
    {
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPageOnPrintScreens"));
        private PrintTechnicsResult Result;

        private int MaxPage
        {
            get { return pageLength == 0 ? 1 : Result.AllRecordsCount / pageLength + (Result.AllRecordsCount != 0 && Result.AllRecordsCount % pageLength == 0 ? 0 : 1); }
        }

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_PRINT_TECHNICS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_PRINT") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_PRINT_TECHNICS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            if (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_MK") == UIAccessLevel.Hidden)
            {
                btnPrintMK.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_PZ") == UIAccessLevel.Hidden)
            {
                btnPrintPZ.Visible = false;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Print", "PrintTechnics");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            Result = new PrintTechnicsResult(pageLength, CurrentUser);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //Set the default order
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            btnRefresh.Attributes.Add("onclick", "SetPickListsSelection();");

            btnPrintMKSrv.Style.Add("display", "none");
            btnPrintPZSrv.Style.Add("display", "none");
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();
            PopulateMilitaryCommands();
            PopulateReadiness();
        }

        private void PopulateMilitaryCommands()
        {
            ddMilitaryCommand.Items.Clear();

            ddMilitaryCommand.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryDepartmentForTechnics(CurrentUser, hdnMilitaryDepartmentSelected.Value);
            ddMilitaryCommand.DataTextField = "DisplayTextForSelection";
            ddMilitaryCommand.DataValueField = "MilitaryCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryCommand_Changed(Page, new EventArgs());
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


        private void PopulateReadiness()
        {
            int baseReadiness = 1;
            int additionalReadiness = 2;

            ddReadiness.Items.Clear();

            ddReadiness.Items.Insert(0, ListItems.GetOptionAll());
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(baseReadiness), baseReadiness.ToString()));
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(additionalReadiness), additionalReadiness.ToString()));
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
        private void RefreshResults()
        {
            if (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS") == UIAccessLevel.Hidden)
                return;

            pnlPaging.Visible = true;

            StringBuilder html = new StringBuilder();
            string selectedRecordsMessageHtml = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            hdnSelectedTechnicsIDs.Value = "";

            if (Result.OverallResult != null)
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                string cellStyle = "vertical-align: top;";

                html.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                                 <thead>
                                    <tr>
                                       <th style='width: 20px;" + headerStyle + @"'><input type='checkbox' id='chkAll' checked='checked' onclick='CheckAll();' title='Премахни всички' /></th>
                                       <th style='width: 20px;" + headerStyle + @"'>№</th>
                                       <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Вид техника" + arrOrderCol[0] + @"</th>
                                       <th style='width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Нормативна категория" + arrOrderCol[1] + @"</th>
                                       <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Рег. номер" + arrOrderCol[2] + @"</th>
                                    </tr> 
                                 </thead>");

                int counter = 0;

                foreach (PrintTechnicsResultBlock block in Result.OverallResult)
                {
                    counter++;

                    string onClickCheckbox = "onclick='Checkbox_Clicked(" + counter.ToString() + @");'";
                    string onClickRow = "onclick='Row_Clicked(" + counter.ToString() + @");'";

                    html.Append(@"<tr id='row" + counter.ToString() + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                                
                                     <td style='" + cellStyle + @" text-align: left;'>
                                        <input type='checkbox' id='chkRow" + counter.ToString() + @"' " + onClickCheckbox + @" checked='checked'  />
                                        <input type='hidden' id='hdnTechnicsId" + counter.ToString() + @"' value='" + block.TechnicsId.ToString() + @"'/> 
                                     </td>
                                     <td style='" + cellStyle + @" text-align: left; cursor: pointer;' " + onClickRow + @">" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: left; cursor: pointer;' " + onClickRow + @">" + block.TechnicsTypeName + @"</td>
                                     <td style='" + cellStyle + @" text-align: left; cursor: pointer;' " + onClickRow + @">" + block.NormativeTechnics + @"</td>
                                     <td style='" + cellStyle + @" text-align: left; cursor: pointer;' " + onClickRow + @">" + block.RegNumber + @"</td>
                                  </tr>");

                    hdnSelectedTechnicsIDs.Value += (String.IsNullOrEmpty(hdnSelectedTechnicsIDs.Value) ? "" : ",") + block.TechnicsId.ToString();
                }

                html.Append("</table>");
                html.Append("<input type='hidden' id='hdnRowsCount' value='" + counter.ToString() + "' />");

                selectedRecordsMessageHtml = "<div style='width: 700px; text-align: left; margin: 0 auto; padding-bottom: 10px;'>Избрани са <span id='lblSelectedRowsCount'>" + counter.ToString() + "</span> записа</div>";
            }

            this.pnlResultsGrid.InnerHtml = selectedRecordsMessageHtml + html.ToString();

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;
            if (ValidateData())
            {
                Result.Filter = CollectFilterData();

                if (sender != Page ||
                   (!String.IsNullOrEmpty(hdnPageIdx.Value) && int.Parse(hdnPageIdx.Value) > MaxPage))
                {
                    hdnPageIdx.Value = "1";
                    Result.Filter = CollectFilterData();
                }

                RefreshResults();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                Result.Filter = CollectFilterData();
                RefreshResults();
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

                    Result.Filter = CollectFilterData();
                    RefreshResults();
                }
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Result.Filter = CollectFilterData();
                int page = int.Parse(hdnPageIdx.Value);

                if (page < MaxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    Result.Filter = CollectFilterData();

                    RefreshResults();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Result.Filter = CollectFilterData();
                hdnPageIdx.Value = MaxPage.ToString();
                Result.Filter = CollectFilterData();

                RefreshResults();
            }
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                Result.Filter = CollectFilterData();
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= MaxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    Result.Filter = CollectFilterData();
                    RefreshResults();
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

            if (page == MaxPage)
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
            hdnMilitaryDepartmentSelected.Value = "";
            PopulateMilitaryCommands();
            txtRegNumber.Text = "";
        }

        private PrintTechnicsFilter CollectFilterData()
        {
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            PrintTechnicsFilter filter = new PrintTechnicsFilter();

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            filter.MilitaryCommandId = int.Parse(ddMilitaryCommand.SelectedValue);
            hdnMilitaryCommandId.Value = ddMilitaryCommand.SelectedValue;

            filter.MilitaryCommandSuffix = ddSubMilitaryCommand.SelectedValue;
            hdnSubMilitaryCommandId.Value = ddSubMilitaryCommand.SelectedValue;

            filter.RegNumber = txtRegNumber.Text;
            hdnIdentNumber.Value = txtRegNumber.Text;

            filter.Readiness = int.Parse(ddReadiness.SelectedValue);
            hdnReadiness.Value = ddReadiness.SelectedValue;

            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            ddSubMilitaryCommand.DataSource = TechnicsRequestCommandUtil.GetTechnicsRequestCommandsForMilCommandAndMilDept(CurrentUser, int.Parse(ddMilitaryCommand.SelectedValue), hdnMilitaryDepartmentSelected.Value);
            ddSubMilitaryCommand.DataTextField = "Txt";
            ddSubMilitaryCommand.DataValueField = "Val";
            ddSubMilitaryCommand.DataBind();
            ddSubMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void hdnBtnReloadMilitaryCommands_Click(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
        }

        protected void btnPrintMK_Click(object sender, EventArgs e)
        {
            string result = PrintMK();

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=MK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private string PrintMK()
        {
            List<int> technicsIDs = new List<int>();

            if (!String.IsNullOrEmpty(hdnSelectedTechnicsIDs.Value))
            {
                string[] seletedTechnicsIDs = hdnSelectedTechnicsIDs.Value.Split(new char[] { ',' });
                foreach (string technicsId in seletedTechnicsIDs)
                    technicsIDs.Add(int.Parse(technicsId));
            }

            string result = GeneratePrintTechnicsUtil.PrintMK(technicsIDs, CurrentUser);
            return result;
        }

        protected void btnPrintPZ_Click(object sender, EventArgs e)
        {
            string result = PrintPZ();

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=PZ.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private string PrintPZ()
        {
            List<int> technicsIDs = new List<int>();

            if (!String.IsNullOrEmpty(hdnSelectedTechnicsIDs.Value))
            {
                string[] seletedTechnicsIDs = hdnSelectedTechnicsIDs.Value.Split(new char[] { ',' });
                foreach (string technicsId in seletedTechnicsIDs)
                    technicsIDs.Add(int.Parse(technicsId));
            }

            string result = GeneratePrintTechnicsUtil.PrintPZ(technicsIDs, CurrentUser);
            return result;
        }
    }
}
