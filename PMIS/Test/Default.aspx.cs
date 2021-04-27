using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIS.Common;

namespace Test
{
    public partial class Default : TestBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/SelectMilitaryUnit.aspx");
        }
    }
}
