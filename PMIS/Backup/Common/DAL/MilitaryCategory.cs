using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryCategory : IDropDownItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return CategoryName;
        }

        public string  Value()
        {
            return CategoryId.ToString();
        }
    }

    public static class MilitaryCategoryUtil
    {
        public static MilitaryCategory GetMilitaryCategory(int categoryId, User currentUser)
        {
            MilitaryCategory militaryCategory = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KAT_KOD as CategoryID, a.KAT_IME as CategoryName
                               FROM VS_OWNER.KLV_KAT a                               
                               WHERE a.KAT_KOD = :CategoryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CategoryID", OracleType.Number).Value = categoryId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryCategory = new MilitaryCategory();
                    militaryCategory.CategoryId = int.Parse(dr["CategoryID"].ToString());
                    militaryCategory.CategoryName = dr["CategoryName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCategory;
        }

        public static List<MilitaryCategory> GetAllMilitaryCategories(User currentUser)
        {
            List<MilitaryCategory> militaryCategories = new List<MilitaryCategory>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KAT_KOD as CategoryID, a.KAT_IME as CategoryName
                               FROM VS_OWNER.KLV_KAT a                                                              
                               ORDER BY a.KAT_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryCategory militaryCategory = new MilitaryCategory();
                    militaryCategory.CategoryId = int.Parse(dr["CategoryID"].ToString());
                    militaryCategory.CategoryName = dr["CategoryName"].ToString();

                    militaryCategories.Add(militaryCategory);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCategories;
        }
    }
}
