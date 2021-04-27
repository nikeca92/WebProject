using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public class TechParamsPageUtil
    {
        private RESPage page;
        private User user;
        private Dictionary<string, List<Maintenance>> maintenances = null;

        public TechParamsPageUtil(RESPage pPage, User pUser)
        {
            this.page = pPage;
            this.user = pUser;
            maintenances = new Dictionary<string, List<Maintenance>>();
            
            AddMaintenance("VEHICLES", "RES_VehicleKind");
            AddMaintenance("VEHICLES", "RES_VehicleRoadability");
            AddMaintenance("VEHICLES", "RES_VehicleEngineType");
            AddMaintenance("VEHICLES", "RES_VehicleBodyType");

            AddMaintenance("TRAILERS", "RES_TrailerKind");
            AddMaintenance("TRAILERS", "RES_TrailerType");
            AddMaintenance("TRAILERS", "RES_TrailerBodyKind");

            AddMaintenance("TRACTORS", "RES_TractorKind");
            AddMaintenance("TRACTORS", "RES_TractorType");

            AddMaintenance("ENG_EQUIP", "RES_EngEquipKind");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipType");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipBaseKind");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipBaseType");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipBaseEngineType");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipWorkingBodyKind");
            AddMaintenance("ENG_EQUIP", "RES_EngEquipWorkBodyEngineType");

            AddMaintenance("MOB_LIFT_EQUIP", "RES_MobileLiftingEquipKind");
            AddMaintenance("MOB_LIFT_EQUIP", "RES_MobileLiftingEquipType");
            
            AddMaintenance("AVIATION_EQUIP", "RES_AviationAirKind");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationAirType");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationOtherKind");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationOtherType");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationOtherBaseMachineKind");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationOtherBaseMachineType");
            AddMaintenance("AVIATION_EQUIP", "RES_AviationOtherEquipmentKind");

            AddMaintenance("RAILWAY_EQUIP", "RES_RailwayEquipKind");
            AddMaintenance("RAILWAY_EQUIP", "RES_RailwayEquipType");

            AddMaintenance("VESSELS", "RES_VesselKind");
            AddMaintenance("VESSELS", "RES_VesselType");

            AddMaintenance("FUEL_CONTAINERS", "RES_FuelContainerKind");
            AddMaintenance("FUEL_CONTAINERS", "RES_FuelContainerType");
        }

        private void AddMaintenance(string technicTypeKey, string maintenanceKey)
        {
            if(!maintenances.ContainsKey(technicTypeKey))
            {
                maintenances.Add(technicTypeKey, new List<Maintenance>());
            }
            
            Maintenance maintenance = MaintenanceUtil.GetMaintenance(user, maintenanceKey);
            if(page.GetUIItemAccessLevel(maintenance.UIKeyMaintenance) != UIAccessLevel.Hidden)
                maintenances[technicTypeKey].Add(maintenance);
            
        }

        public List<Maintenance> GetMaintenanceList(string technicsTypeKey)
        {
            return maintenances[technicsTypeKey];            
        }

        public bool CanUserAccessAnyMaintenanceListForTechnicsType(string technicsTypeKey)
        {
            return maintenances[technicsTypeKey].Count > 0;
        }

        public bool CanUserAccessAnyMaintenanceList()
        {
            return maintenances.Where(x => x.Value.Count > 0).Count() > 0;
        }
    }

    public partial class TechParams : RESPage
    {
        protected TechParamsPageUtil pageUtil;

        protected void Page_Load(object sender, EventArgs e)
        {
            HighlightMenuItems("Lists_RES_TechParams");

            pageUtil = new TechParamsPageUtil(this, CurrentUser);

            if (!Page.IsPostBack)
            {
                PopulateTechnicsTypes();
                LoadLinks();
            }
        }

        private void PopulateTechnicsTypes()
        {
            this.ddTechnicsTypes.DataSource = GetTechnicsTypes();
            this.ddTechnicsTypes.DataTextField = "TypeName";
            this.ddTechnicsTypes.DataValueField = "TypeKey";
            this.ddTechnicsTypes.DataBind();
        }

        private List<TechnicsType> GetTechnicsTypes()
        {
            List<TechnicsType> technicsTypes = new List<TechnicsType>();// = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);

            foreach (TechnicsType technicsType in TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser))
            {
                if(pageUtil.CanUserAccessAnyMaintenanceListForTechnicsType(technicsType.TypeKey))
                    technicsTypes.Add(technicsType);                
            }

            return technicsTypes;
        }

        protected void ddTechnicsTypes_Changed(object sender, EventArgs e)
        {
            LoadLinks();
        }

        private void LoadLinks()
        {
            StringBuilder linksHTML = new StringBuilder();

            foreach (Maintenance maintenance in pageUtil.GetMaintenanceList(ddTechnicsTypes.SelectedValue))
            {
                linksHTML.Append(@"<div class=""HomePageItem"" runat=""server"" id=""div_" + maintenance.MaintKey + @""">
                                       <span onclick=""JSRedirect('Maintenance.aspx?MaintKey=" + maintenance.MaintKey + @"&fm=1');"" class=""HomePageItemLink"">" + maintenance.HeaderTitle + @"</span>
                                    </div>
                                  ");                
            }

            pnlLinks.InnerHtml = linksHTML.ToString();
        }
    }
}
