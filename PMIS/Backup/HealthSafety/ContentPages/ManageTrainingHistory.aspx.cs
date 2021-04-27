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
    public partial class ManageTrainingHistory : HSPage
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
                return "HS_TRAININGHISTORY";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {    
            if (this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Committees", "Committees_TrainingHistory");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();                   

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string identNumber = "";
            string firstName = "";
            string lastName = "";

            identNumber = txtIdentNumber.Text;
            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;

            int allRows = PersonUtil.GetAllPersonsCount(identNumber, firstName, lastName, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {               
                //The default order is by identNumber
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }             

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblIdentNumber.Style.Add("vertical-align", "top");
            txtIdentNumber.Style.Add("vertical-align", "top");
            lblFirstName.Style.Add("vertical-align", "top");
            txtFirstName.Style.Add("vertical-align", "top");
            lblLastName.Style.Add("vertical-align", "top");
            txtLastName.Style.Add("vertical-align", "top");
        }      

        //Validate some field on the screen - for compatibility, if validation on filter controls is needed
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
        private void RefreshPersons()
        {
            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string identNumber = "";
            string firstName = "";
            string lastName = "";

            identNumber = txtIdentNumber.Text;
            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;

            //Get the list of Persons according to the specified filters, order and paging
            List<Person> persons = PersonUtil.GetAllPersons(identNumber, firstName, lastName, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (persons.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {               
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>ЕГН" + arrOrderCol[0] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Име" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Звание" + arrOrderCol[2] + @"</th>                               
                               <th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (Person person in persons)
                {
                    string cellStyle = "vertical-align: top;";
                   
                    string editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditTrainingHistory(" + person.PersonId.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + person.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + person.FullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + (person.MilitaryRank != null ? person.MilitaryRank.LongName : "") + @"</td>                                 
                                 <td style='" + cellStyle + @"'>" + editHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlPersonsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";    
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshPersons();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshPersons();
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

                    RefreshPersons();
                }
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < maxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    RefreshPersons();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshPersons();
            }
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    RefreshPersons();
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

            if (page == maxPage)
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
            txtIdentNumber.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }
    }
}
