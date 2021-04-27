using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Common;

namespace PMIS.Common
{
    public class TestBasePage : BasePage
    {
        //The current module is HS (HealthSafety)
        public override string ModuleKey
        {
            get
            {
                return ModuleUtil.HS();
            }
        }
    }
}
