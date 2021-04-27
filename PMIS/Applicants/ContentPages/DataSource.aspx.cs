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
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants
{

    public partial class DataSource : APPLPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "MilitaryUnit")
            {
                int pageCount = int.Parse(Request.Params["PageCount"]);
                int pageIndex = int.Parse(Request.Params["PageIndex"]);
                string searchText = Request.Params["SearchText"] != null ? Request.Params["SearchText"] : "";
                bool includeItemsWithoutVPN = Request.Params["AllowPickUpNodesWithoutVPN"] != null && Request.Params["AllowPickUpNodesWithoutVPN"].ToUpper() == "TRUE";
                bool includeOnlyActual = Request.Params["IncludeOnlyActual"] == null || Request.Params["IncludeOnlyActual"].ToUpper() != "FALSE"; //Default is True

                string response = GenerateXMLForMilitaryUnit(pageIndex, pageCount, searchText, includeItemsWithoutVPN, includeOnlyActual);

                AJAX a = new AJAX(response, Response);
                a.Write();
                Response.End();
                return;
            }         
        }

        private static MilitaryUnit ExtractMilitaryUnitFromDR(OracleDataReader dr)
        {
            MilitaryUnit militaryUnit = new MilitaryUnit(null);

            militaryUnit.MilitaryUnitId = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? DBCommon.GetInt(dr["MilitaryUnitID"]) : 0);
            militaryUnit.ParentId = (DBCommon.IsInt(dr["ParentID"]) ? (int?)DBCommon.GetInt(dr["ParentID"]) : null);
            militaryUnit.CityId = (DBCommon.IsInt(dr["CityID"]) ? DBCommon.GetInt(dr["CityID"]) : 0);
            militaryUnit.ShortName = dr["ShortName"].ToString();
            militaryUnit.LongName = dr["LongName"].ToString();
            militaryUnit.VPN = dr["VPN"].ToString();

            return militaryUnit;
        }

        private string GenerateXMLForMilitaryUnit(int pageIndex, int pageCount, string searchText, bool includeItemsWithoutVPN, bool includeOnlyActual)
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
