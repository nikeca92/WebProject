using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TechnicsCategory : IDropDownItem
    {
        private int technicsCategoryId;
        private string categoryName;

        public int TechnicsCategoryId
        {
            get
            {
                return technicsCategoryId;
            }
            set
            {
                technicsCategoryId = value;
            }
        }

        public string CategoryName
        {
            get
            {
                return categoryName;
            }
            set
            {
                categoryName = value;
            }
        }

        //IDropDownItem
        public string Value()
        {
            return TechnicsCategoryId.ToString();
        }

        public string Text()
        {
            return CategoryName;
        }
        
    }

    public static class TechnicsCategoryUtil
    {
        //This method creates and returns a TechnicsCategory object. It extracts the data from a DataReader.
        public static TechnicsCategory ExtractTechnicsCategoryFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsCategory technicsCategory = new TechnicsCategory();

            technicsCategory.TechnicsCategoryId = DBCommon.GetInt(dr["TechnicsCategoryID"]);
            technicsCategory.CategoryName = dr["TechnicsCategoryName"].ToString();

            return technicsCategory;
        }

        //Get a particular object by its ID
        public static TechnicsCategory GetTechnicsCategory(int technicsCategoryId, User currentUser)
        {
            TechnicsCategory technicsCategory = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsCategoryID, a.TechnicsCategoryName
                               FROM PMIS_RES.TechnicsCategories a
                               WHERE a.TechnicsCategoryID = :TechnicsCategoryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsCategoryID", OracleType.Number).Value = technicsCategoryId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsCategory = ExtractTechnicsCategoryFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsCategory;
        }


        //Get a list of all types
        public static List<TechnicsCategory> GetAllTechnicsCategories(User currentUser)
        {
            List<TechnicsCategory> technicsCategories = new List<TechnicsCategory>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsCategoryID, a.TechnicsCategoryName
                               FROM PMIS_RES.TechnicsCategories a
                               ORDER BY a.TechnicsCategoryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsCategories.Add(ExtractTechnicsCategoryFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsCategories;
        }
    }

}