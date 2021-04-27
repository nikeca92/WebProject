using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class AddPotencialApplicant_SelectPerson : APPLPage
    {
        public override string PageUIKey
        {
            get
            {
                return "APPL_POTENCIALAPPL";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the correct item in the menu
            HighlightMenuItems("Applicants", "PotencialApplicants_Add");

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

            string redirect = "~/ContentPages/AddPotencialApplicant_PersonDetails.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&IdentNumber=" + identNumber + "&PageFrom=2";

            int? personId = PersonUtil.GetPersonIdByIdentNumber(identNumber, CurrentUser);

            if (personId.HasValue)
            {
                Person person = PersonUtil.GetPerson(personId.Value, CurrentUser);
                PotencialApplicant pApplicant = PotencialApplicantUtil.GetPotencialApplicant(personId.Value, militaryDepartmentId, CurrentUser);

                if (pApplicant != null)
                {
                    redirect = "~/ContentPages/AddPotencialApplicant_PersonDetails.aspx?MilitaryDepartmentId=" + militaryDepartmentId + "&IdentNumber=" + identNumber + "&PageFrom=2";
                }
                else
                {
                    //If this person hasn't been registered as Applicant and it has a different PersonTypeCode then redirect to no access page
                    if (Config.GetWebSetting("KOD_KZV_Check_PotentialApplicant").ToLower() == "true" &&
                        CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_PotentialApplicant")))
                    {
                        redirect = "~/ContentPages/NoAccessToPerson.aspx?PersonId=" + personId.Value + "&PageFrom=2";
                    }
                }
            }

            Response.Redirect(redirect);
        }

    }
}
