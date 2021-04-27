using System.Collections.Generic;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //This class represents the VesselCrewCategory
    public class VesselCrewCategory : BaseDbObject, IDropDownItem
    {
        private int vesselCrewCategoryID;       
        private string vesselCrewCategoryKey;       
        private string vesselCrewCategoryName;

        public int VesselCrewCategoryID
        {
            get { return vesselCrewCategoryID; }
            set { vesselCrewCategoryID = value; }
        }

        public string VesselCrewCategoryKey
        {
            get { return vesselCrewCategoryKey; }
            set { vesselCrewCategoryKey = value; }
        }

        public string VesselCrewCategoryName
        {
            get { return vesselCrewCategoryName; }
            set { vesselCrewCategoryName = value; }
        }

        public VesselCrewCategory(User currentUser)
            : base(currentUser)
        {
        }

        //IDropDownItem Members
        public string Text()
        {
            return VesselCrewCategoryName;
        }

        public string Value()
        {
            return VesselCrewCategoryID.ToString();
        }
    }

    public static class VesselCrewCategoryUtil
    {
        //Exstract a particular VesselCrewCategory object from a data reader. This method should be reused when pulling records from the DB
        public static VesselCrewCategory ExtractVesselCrewCategoryFromDR(User currentUser, OracleDataReader dr)
        {
            VesselCrewCategory vesselCrewCategory = new VesselCrewCategory(currentUser);

            vesselCrewCategory.VesselCrewCategoryID = (DBCommon.IsInt(dr["VesselCrewCategoryID"]) ? DBCommon.GetInt(dr["VesselCrewCategoryID"]) : 0);
            vesselCrewCategory.VesselCrewCategoryKey = dr["VesselCrewCategoryKey"].ToString();
            vesselCrewCategory.VesselCrewCategoryName = dr["VesselCrewCategoryName"].ToString();

            return vesselCrewCategory;
        }

        //Get a single VesselCrewCategory object by its ID
        public static VesselCrewCategory GetVesselCrewCategory(User currentUser, int vesselCrewCategoryID)
        {
            VesselCrewCategory vesselCrewCategory = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.VesselCrewCategoryID, a.VesselCrewCategoryKey, a.VesselCrewCategoryName
                               FROM PMIS_ADM.VesselCrewCategories a
                               WHERE a.VesselCrewCategoryID = :VesselCrewCategoryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("VesselCrewCategoryID", OracleType.Number).Value = vesselCrewCategoryID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vesselCrewCategory = ExtractVesselCrewCategoryFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselCrewCategory;
        }

        //Get a list of all VesselCrewCategories
        public static List<VesselCrewCategory> GetVesselCrewCategories(User currentUser)
        {
            List<VesselCrewCategory> vesselCrewCategories = new List<VesselCrewCategory>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.VesselCrewCategoryID, a.VesselCrewCategoryKey, a.VesselCrewCategoryName
                               FROM PMIS_ADM.VesselCrewCategories a
                               ORDER BY a.VesselCrewCategoryID ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VesselCrewCategory vesselCrewCategory = ExtractVesselCrewCategoryFromDR(currentUser, dr);
                    vesselCrewCategories.Add(vesselCrewCategory);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselCrewCategories;
        }
    }
}