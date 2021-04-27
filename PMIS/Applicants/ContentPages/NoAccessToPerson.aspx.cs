using System;
using System.Collections;
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
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class NoAccessToPerson : APPLPage
    {
        private string personDisplayInfo;
        public string PersonDisplayInfo
        {
            get
            {
                return personDisplayInfo;
            }

            set
            {
                personDisplayInfo = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnPersonId.Value = Request.Params["PersonId"];
                hdnPageFrom.Value = Request.Params["PageFrom"];

                if (String.IsNullOrEmpty(Request.Params["PageFrom"]) ||
                    (int.Parse(hdnPageFrom.Value) != 1 &&
                     int.Parse(hdnPageFrom.Value) != 2 &&
                     int.Parse(hdnPageFrom.Value) != 3))
                {
                    btnBack.Visible = false;
                }
            }

            int personId = int.Parse(hdnPersonId.Value);
            Person person = PersonUtil.GetPerson(personId, CurrentUser);

            PersonDisplayInfo = (person.MilitaryRank != null ? person.MilitaryRank.ShortName : "") + " " + person.FullName + " (ЕГН: " + person.IdentNumber + ")";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (int.Parse(hdnPageFrom.Value) == 1) //Try to add new Applicant
            {
                Response.Redirect("~/ContentPages/AddApplicant_SelectPerson.aspx");
            }
            else if (int.Parse(hdnPageFrom.Value) == 2) //Try to add new Potential Applicant
            {
                Response.Redirect("~/ContentPages/AddPotencialApplicant_SelectPerson.aspx");
            }
            else if (int.Parse(hdnPageFrom.Value) == 3) //Try to add new Cadet
            {
                Response.Redirect("~/ContentPages/AddCadet_SelectPerson.aspx");
            }
        }
    }
}
