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
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Text;
using PMIS.Common;
using System.Collections.Generic;

namespace Test
{

    public partial class DataSource : TestBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "MIR")
            {
                int pageCount = int.Parse(Request.Params["PageCount"]);
                int pageIndex = int.Parse(Request.Params["PageIndex"]);
                string searchText = Request.Params["SearchText"] != null ? Request.Params["SearchText"] : "";
                bool includeItemsWithoutVPN = Request.Params["AllowPickUpNodesWithoutVPN"] != null && Request.Params["AllowPickUpNodesWithoutVPN"].ToUpper() == "TRUE";
                bool includeOnlyActual = Request.Params["IncludeOnlyActual"] == null || Request.Params["IncludeOnlyActual"].ToUpper() != "FALSE"; //Default is True

                string response = GenerateXMLForMIR(pageIndex, pageCount, searchText, includeItemsWithoutVPN, includeOnlyActual);

                AJAX a = new AJAX(response, Response);
                a.Write();
                Response.End();
                return;
            }         
        }

        private string GenerateXMLForMIR(int pageIndex, int pageCount, string searchText, bool includeItemsWithoutVPN, bool includeOnlyActual)
        {
            List<MilitaryUnit> militaryUnits = MilitaryUnitUtil.GetAllMilitaryUnits(CurrentUser, searchText, includeItemsWithoutVPN, includeOnlyActual);

            List<MilitaryUnitNode> treeList = null;
            if (pageIndex != -1)
                MilitaryUnitUtil.GenerateTreeList(militaryUnits, ref treeList, null, 0);
            else
            {
                treeList = MilitaryUnitNodeUtil.TransformMilitaryUnitsList(militaryUnits, null);
                pageIndex = 1;
            }

            string xml = MilitaryUnitUtil.GenerateXML(treeList, pageIndex, pageCount);

            return xml;
        }      
    }
}
