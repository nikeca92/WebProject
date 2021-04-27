using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace PMIS.Common
{
    //This is common class used for reading configuration values
    public static class Config
    {
        //Get a config value from the web.config file
        //If there isn't such a key then return empty string
        public static string GetWebSetting(string Key)
        {
            string val = "";

            try
            {
                val = WebConfigurationManager.AppSettings[Key];
            }
            catch
            {
                val = "";
            }

            if (val == null)
                val = "";

            return val;
        }
    }
}

