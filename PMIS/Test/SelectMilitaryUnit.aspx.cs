using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIS.Common;
using System.Web.Configuration;

namespace Test
{
    public partial class SelectMilitaryUnit : TestBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ItemSelector2.SelectedValue = "11";
            //ItemSelector2.SelectedText = "Под. Младост";
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            lblSelectedID.Text = ItemSelector2.SelectedValue;
            lblSelectedName.Text = ItemSelector2.SelectedText;
        }
    }
}
