using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class AddApplicant_SelectPerson : APPLPage
    {
        public override string PageUIKey
        {
            get
            {
                return "APPL_APPL";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_ADDAPPL") != UIAccessLevel.Enabled)
            {
                RedirectAccessDenied();
            }

            //Hilight the correct item in the menu
            HighlightMenuItems("Applicants", "Applicants_Add");

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
            PopulateMilitaryDepartments();
        }

        //Populate the MilitaryDeprtments drop-down
        private void PopulateMilitaryDepartments()
        {
            ddMilitaryDepartments.Items.Clear();
            ddMilitaryDepartments.Items.Add(ListItems.GetOptionChooseOne());

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                ddMilitaryDepartments.Items.Add(new ListItem(militaryDepartment.MilitaryDepartmentName, militaryDepartment.MilitaryDepartmentId.ToString()));
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            int militaryDepartmentId = 0;
            Int32.TryParse(this.ddMilitaryDepartments.SelectedValue, out militaryDepartmentId);

            string identNumber = txtIdentNumber.Text;

            string redirect = "~/ContentPages/AddApplicant_PersonDetails.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&IdentNumber=" + identNumber;

            int? personId = PersonUtil.GetPersonIdByIdentNumber(identNumber, CurrentUser);

            if (personId.HasValue)
            {
                Person person = PersonUtil.GetPerson(personId.Value, CurrentUser);
                Applicant applicant = ApplicantUtil.GetApplicant(personId.Value, militaryDepartmentId, CurrentUser);

                if (applicant != null)
                {
                    redirect = "~/ContentPages/EditApplicant.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&PersonId=" + personId.Value + "&PageFrom=2";
                }
                else
                {
                    //If this person hasn't been registered as Applicant and it has a different PersonTypeCode then redirect to no access page
                    if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                        CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")))
                    {
                        redirect = "~/ContentPages/NoAccessToPerson.aspx?PersonId=" + personId.Value + "&PageFrom=1";
                    }
                }
            }

            Response.Redirect(redirect);
        }
    }
}
