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
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class NoAccessToPerson : RESPage
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

        public string PersonMilitaryEmployedAt { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnPersonId.Value = Request.Params["PersonId"];
                hdnPageFrom.Value = Request.Params["PageFrom"];

                if (String.IsNullOrEmpty(Request.Params["PageFrom"]) ||
                    (int.Parse(hdnPageFrom.Value) != 1))
                {
                    btnBack.Visible = false;
                }
            }

            int personId = int.Parse(hdnPersonId.Value);
            Person person = PersonUtil.GetPerson(personId, CurrentUser);

            PersonDisplayInfo = (person.MilitaryRank != null ? person.MilitaryRank.ShortName : "") + " " + person.FullName + " (ЕГН: " + person.IdentNumber + ")";

            if (person.MilitaryUnit != null)
            {
                PersonMilitaryEmployedAt = " Лицето е на кадрова военна служба във ВПН <b>" + person.MilitaryUnit.VPN + "</b>.";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (int.Parse(hdnPageFrom.Value) == 1) //Try to add new Reservist
            {
                Response.Redirect("~/ContentPages/AddEditReservist.aspx");
            }
        }
    }
}
