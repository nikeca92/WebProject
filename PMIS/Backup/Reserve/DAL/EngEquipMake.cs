using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EngEquipBaseMake : BaseDbObject, IDropDownItem
    {
        private int engEquipBaseMakeId;
        private string engEquipBaseMakeName;
        private List<EngEquipBaseModel> engEquipBaseModels;

        public int EngEquipBaseMakeId
        {
            get
            {
                return engEquipBaseMakeId;
            }
            set
            {
                engEquipBaseMakeId = value;
            }
        }

        public string EngEquipBaseMakeName
        {
            get
            {
                return engEquipBaseMakeName;
            }
            set
            {
                engEquipBaseMakeName = value;
            }
        }

        public List<EngEquipBaseModel> EngEquipBaseModels
        {
            get
            {
                //Lazy initialization
                if (engEquipBaseModels == null)
                    engEquipBaseModels = EngEquipBaseModelUtil.GetAllEngEquipBaseModels(EngEquipBaseMakeId, CurrentUser);

                return engEquipBaseModels;
            }
            set
            {
                engEquipBaseModels = value;
            }
        }

        public EngEquipBaseMake(User user)
            : base(user)
        {

        } 

        //IDropDownItem
        public string Value()
        {
            return EngEquipBaseMakeId.ToString();
        }

        public string Text()
        {
            return EngEquipBaseMakeName.ToString();
        }
    }

    public static class EngEquipBaseMakeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static EngEquipBaseMake ExtractEngEquipBaseMakeFromDataReader(OracleDataReader dr, User currentUser)
        {
            EngEquipBaseMake engEquipBaseMake = new EngEquipBaseMake(currentUser);

            engEquipBaseMake.EngEquipBaseMakeId = DBCommon.GetInt(dr["EngEquipBaseMakeID"]);
            engEquipBaseMake.EngEquipBaseMakeName = dr["EngEquipBaseMakeName"].ToString();

            return engEquipBaseMake;
        }

        //Get a particular object by its ID
        public static EngEquipBaseMake GetEngEquipBaseMake(int engEquipBaseMakeId, User currentUser)
        {
            EngEquipBaseMake engEquipBaseMake = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EngEquipBaseMakeID, a.EngEquipBaseMakeName
                               FROM PMIS_RES.EngEquipBaseMakes a                       
                               WHERE a.EngEquipBaseMakeID = :EngEquipBaseMakeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipBaseMakeID", OracleType.Number).Value = engEquipBaseMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    engEquipBaseMake = ExtractEngEquipBaseMakeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseMake;
        }

        //Get a list of all types
        public static List<EngEquipBaseMake> GetAllEngEquipBaseMakes(User currentUser)
        {
            List<EngEquipBaseMake> engEquipBaseMakes = new List<EngEquipBaseMake>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EngEquipBaseMakeID, a.EngEquipBaseMakeName
                               FROM PMIS_RES.EngEquipBaseMakes a 
                               ORDER BY a.EngEquipBaseMakeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    engEquipBaseMakes.Add(ExtractEngEquipBaseMakeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseMakes;
        }
    }

}