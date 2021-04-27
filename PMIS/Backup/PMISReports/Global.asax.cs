using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Izenda.AdHoc;
using PMIS.Common;
using System.Globalization;

namespace PMISReports
{
    public class Global : System.Web.HttpApplication
    {
        protected void Session_Start(object sender, EventArgs e)
        {            
            //Izenda.AdHoc.AdHocSettings.LicenseKey = "TheIntellectionGroup +LANG|Enterprise|6.5|12/24/2010|8|0|0|M|61VE+AI";
            Izenda.AdHoc.AdHocSettings.AdHocConfig = new CustomAdHocConfig();

            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("bg-BG");
            //AdHocSettings.Culture = new CultureInfo("bg-BG");
            AdHocSettings.Language = AdHocLanguage.Bulgarian;
        }       
    }

    [Serializable]
    public class CustomAdHocConfig : Izenda.AdHoc.FileSystemAdHocConfig
    {
        // Configure settings
        //Add Custom Setting below license key and connection string setting
        public override void ConfigureSettings()
        {
            base.ConfigureSettings();
            //AdHocSettings.LicenseKey = "TheIntellectionGroup +LANG|Enterprise|6.5|12/24/2010|8|0|0|M|61VE+AI";            
            AdHocSettings.ReportsPath = Config.GetWebSetting("ReportsPath");
            AdHocSettings.OracleConnectionString = Config.GetWebSetting("WebConnectionString");
            //AdHocSettings.VisibleTables = new string[] { "INSERT_TABLE_OR_VIEW_NAME_HERE", "INSERT_SECOND_TABLE_OR_VIEW_NAME_HERE" };

        }

        // Control what reports are visible for the current user
        public override Izenda.AdHoc.ReportInfo[] ListReports()
        {
            return base.ListReports();
        }
        
        // Call Izenda.AdHoc.AdHocSettngs.AdHocConfig.PostLogin() from your login
        // page after authenitcation is successful
        public override void PostLogin()
        {            
            PMIS.Common.User user = PMIS.Common.UserUtil.GetUser(int.Parse(HttpContext.Current.Session["UserID"].ToString()), "REP");            

            UIItem uiItem = null;

            List<UIItem> lstUIItems = UIItemUtil.GetUIItems("REP_ADDEDITREPORT", user, false, user.Role.RoleId, null);

            if (lstUIItems.Count > 0)
                uiItem = lstUIItems[0];

            if (uiItem != null && uiItem.AccessLevel == UIAccessLevel.Enabled)
            {
                Izenda.AdHoc.AdHocSettings.CurrentUserIsAdmin = true;
                Izenda.AdHoc.AdHocSettings.ShowDesignLinks = true;
                Izenda.AdHoc.AdHocSettings.AllowDeletingReports = true;
            }
            else
            {
                Izenda.AdHoc.AdHocSettings.CurrentUserIsAdmin = false;
                Izenda.AdHoc.AdHocSettings.ShowDesignLinks = false;
                Izenda.AdHoc.AdHocSettings.AllowDeletingReports = false;                
            }

            lstUIItems = UIItemUtil.GetUIItems("REP_SETTINGS", user, false, user.Role.RoleId, null);

            if (lstUIItems.Count > 0)
                uiItem = lstUIItems[0];

            if (uiItem != null && uiItem.AccessLevel == UIAccessLevel.Enabled)
            {
                Izenda.AdHoc.AdHocSettings.ShowSettingsButton = true;
            }
            else
            {
                Izenda.AdHoc.AdHocSettings.ShowSettingsButton = false;
            }            

            Izenda.AdHoc.AdHocSettings.OracleConnectionString = user.ConnectionString;
            //Izenda.AdHoc.AdHocSettings.TableFilterRegex = @"^(?!.*\#)";            
        }
    }
}