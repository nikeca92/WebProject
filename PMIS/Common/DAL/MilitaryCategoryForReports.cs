using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryCategoryForReports
    {
        public string CategoryKey { get; set; }
        public string CategoryName { get; set; }
    }

    public static class MilitaryCategoryForReportsUtil
    {
        public static List<MilitaryCategoryForReports> GetAllMilitaryCategoriesForReports()
        {
            List<MilitaryCategoryForReports> militaryCategories = new List<MilitaryCategoryForReports>();

            militaryCategories.Add(new MilitaryCategoryForReports() { CategoryKey = "KEY_OFFICER", CategoryName = "Офицери" });
            militaryCategories.Add(new MilitaryCategoryForReports() { CategoryKey = "KEY_OFFICER_CANDIDATE", CategoryName = "Офицерски кандидати" });
            militaryCategories.Add(new MilitaryCategoryForReports() { CategoryKey = "KEY_SERGEANTS", CategoryName = "Сержанти" });
            militaryCategories.Add(new MilitaryCategoryForReports() { CategoryKey = "KEY_SOLDIERS", CategoryName = "Войници" });

            return militaryCategories;
        }

        public static MilitaryCategoryForReports GetMilitaryCategoryByKey(string key)
        {
            List<MilitaryCategoryForReports> militaryCategories = GetAllMilitaryCategoriesForReports();
            MilitaryCategoryForReports militaryCategory = militaryCategories.Where(x => x.CategoryKey == key).FirstOrDefault();
            return militaryCategory;
        }
    }
}
