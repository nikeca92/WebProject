using System;
using System.Data.OracleClient;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.PMISAdmin.Common;
using System.IO;

namespace PMIS.PMISAdmin
{

    public partial class GenerateOfflineReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = UserUtil.GetSystemAccessUser("devel");

            if (userId > 0)
            {
                string path = Config.GetWebSetting("OfflineReportsPath");

                User user = UserUtil.GetUser(userId, ModuleUtil.ADM());

                OfflineReportsGenerator reportGenerator = new OfflineReportsGenerator(user);

                Dictionary<string, string> result = reportGenerator.GenerateOfflineReports();

                foreach (string militaryDepartmentName in result.Keys)
                {
                    StreamWriter tw = new StreamWriter(path + militaryDepartmentName + ".xls");
                    tw.Write(result[militaryDepartmentName]);
                    tw.Close();
                }
            }
        }       
    }
}
