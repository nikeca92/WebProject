using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public static class ReadinessUtil
    {
        public static string ReadinessName(int readinessId)
        {
            string name = "";

            switch (readinessId)
            {
                case 1:
                    name = "Основно попълнение";
                    break;
                case 2:
                    name = "Допълващ резерв";
                    break;
            }

            return name;
        }
    }
}