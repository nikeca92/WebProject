using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class AddCadet_SelectPerson : APPLPage
    {
        public override string PageUIKey
        {
            get
            {
                return "APPL_CADETS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the correct item in the menu
            HighlightMenuItems("Cadets", "Cadets_Add");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Initial load
            if (!IsPostBack)
            {
                //Populate any drop-downs on the screen
                PopulateDropDowns();
            }
        }

        //Populate any drop-downs on the screen
        private void PopulateDropDowns()
        {
            this.PopulateMilitaryDepartments();
        }

        //Populate the MilitaryDeprtments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();

            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            int militaryDepartmentId = 0;
            Int32.TryParse(this.ddlMilitaryDepartments.SelectedValue, out militaryDepartmentId);

            string identNumber = txtIdentNumber.Text.Trim();

            string redirect = "~/ContentPages/AddCadet_PersonDetails.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&IdentNumber=" + identNumber;

            int? personId = PersonUtil.GetPersonIdByIdentNumber(identNumber, CurrentUser);

            if (personId.HasValue)
            {
                Person person = PersonUtil.GetPerson(personId.Value, CurrentUser);
                Cadet cadet = CadetUtil.GetCadet(personId.Value, militaryDepartmentId, CurrentUser);

                if (cadet != null)
                {
                    redirect = "~/ContentPages/EditCadet.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&PersonId=" + personId.Value + "&PageFrom=2";
                }
                else
                {
                    //If this person hasn't been registered as Cadet and it has a different PersonTypeCode then redirect to no access page
                    if (Config.GetWebSetting("KOD_KZV_Check_Cadet").ToLower() == "true" &&
                        CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Cadet")))
                    {
                        redirect = "~/ContentPages/NoAccessToPerson.aspx?PersonId=" + personId.Value + "&PageFrom=3";
                    }
                }
            }

            Response.Redirect(redirect);
        }
    }
}
