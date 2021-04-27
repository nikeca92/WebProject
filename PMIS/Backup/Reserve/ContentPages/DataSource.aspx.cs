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
using PMIS.Reserve.Common;
using System.Collections.Generic;

namespace PMIS.Reserve.ContentPages
{
    public partial class DataSource : RESPage
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

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "SchoolSubject")
            {
                int pageCount = int.Parse(Request.Params["PageCount"]);
                int pageIndex = int.Parse(Request.Params["PageIndex"]);
                string prefix = Request.Params["Prefix"] != null ? Request.Params["Prefix"] : "";

                string response = GetSchoolSubject(pageIndex, pageCount, prefix);

                AJAX a = new AJAX(response, Response);
                a.Write();
                Response.End();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "Companies")
            {
                int pageCount = int.Parse(Request.Params["PageCount"]);
                int pageIndex = int.Parse(Request.Params["PageIndex"]);
//                int maxRes = 20;
                    //int.Parse(Request.Params["ResultMaxCount"]);
                string prefix = Request.Params["Prefix"] != null ? Request.Params["Prefix"] : "";

                //string response = GetAllCompanies(maxRes, prefix);
                string response = GetAllCompanies(pageIndex, pageCount, prefix);

                AJAX a = new AJAX(response, Response);
                a.Write();
                Response.End();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "Companies_WithPickupItem")
            {
                int pageCount = int.Parse(Request.Params["PageCount"]);
                int pageIndex = int.Parse(Request.Params["PageIndex"]);
                string prefix = Request.Params["Prefix"] != null ? Request.Params["Prefix"] : "";

                string response = GetAllCompanies_WithPickupItem(pageIndex, pageCount, prefix);

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

        private string GetSchoolSubject(int pageIndex, int pageCount, string prefix)
        {
            string xml = PersonSchoolSubjectUtil.GetPersonSchoolSubjects_ItemSelector(pageIndex, pageCount, prefix, CurrentUser);
            return xml;
        }

        private string GetAllCompanies(int pageIndex, int pageCount, string prefix)
        {
            string xml = CompanyUtil.GetAllCompanies_ItemSelector(pageIndex,pageCount,prefix,CurrentUser);
            return xml;
        }

        private string GetAllCompanies_WithPickupItem(int pageIndex, int pageCount, string prefix)
        {
            string xml = CompanyUtil.GetAllCompanies_WithPickupItem_ItemSelector(pageIndex, pageCount, prefix, CurrentUser);
            return xml;
        }

    }
}
