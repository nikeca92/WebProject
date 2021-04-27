using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class VehicleMake : BaseDbObject, IDropDownItem
    {
        private int vehicleMakeId;
        private string vehicleMakeName;
        private List<VehicleModel> vehicleModels;

        public int VehicleMakeId
        {
            get
            {
                return vehicleMakeId;
            }
            set
            {
                vehicleMakeId = value;
            }
        }

        public string VehicleMakeName
        {
            get
            {
                return vehicleMakeName;
            }
            set
            {
                vehicleMakeName = value;
            }
        }

        public List<VehicleModel> VehicleModels
        {
            get
            {
                //Lazy initialization
                if (vehicleModels == null)
                    vehicleModels = VehicleModelUtil.GetAllVehicleModels(VehicleMakeId, CurrentUser);

                return vehicleModels;
            }
            set
            {
                vehicleModels = value;
            }
        }

        public VehicleMake(User user)
            : base(user)
        {

        } 

        //IDropDownItem
        public string Value()
        {
            return VehicleMakeId.ToString();
        }

        public string Text()
        {
            return VehicleMakeName.ToString();
        }
    }

    public static class VehicleMakeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static VehicleMake ExtractVehicleMakeFromDataReader(OracleDataReader dr, User currentUser)
        {
            VehicleMake vehicleMake = new VehicleMake(currentUser);

            vehicleMake.VehicleMakeId = DBCommon.GetInt(dr["VehicleMakeID"]);
            vehicleMake.VehicleMakeName = dr["VehicleMakeName"].ToString();

            return vehicleMake;
        }

        //Get a particular object by its ID
        public static VehicleMake GetVehicleMake(int vehicleMakeId, User currentUser)
        {
            VehicleMake vehicleMake = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VehicleMakeID, a.VehicleMakeName
                               FROM PMIS_RES.VehicleMakes a                       
                               WHERE a.VehicleMakeID = :VehicleMakeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleMakeID", OracleType.Number).Value = vehicleMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vehicleMake = ExtractVehicleMakeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleMake;
        }

        //Get a list of all types
        public static List<VehicleMake> GetAllVehicleMakes(User currentUser)
        {
            List<VehicleMake> vehicleMakes = new List<VehicleMake>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VehicleMakeID, a.VehicleMakeName
                               FROM PMIS_RES.VehicleMakes a 
                               ORDER BY a.VehicleMakeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vehicleMakes.Add(ExtractVehicleMakeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleMakes;
        }
    }

}