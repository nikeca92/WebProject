using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class AviationOtherBaseMachineMake : BaseDbObject, IDropDownItem
    {
        private int aviationOtherBaseMachineMakeId;
        private string aviationOtherBaseMachineMakeName;
        private List<AviationOtherBaseMachineModel> aviationOtherBaseMachineModels;

        public int AviationOtherBaseMachineMakeId
        {
            get
            {
                return aviationOtherBaseMachineMakeId;
            }
            set
            {
                aviationOtherBaseMachineMakeId = value;
            }
        }

        public string AviationOtherBaseMachineMakeName
        {
            get
            {
                return aviationOtherBaseMachineMakeName;
            }
            set
            {
                aviationOtherBaseMachineMakeName = value;
            }
        }

        public List<AviationOtherBaseMachineModel> AviationOtherBaseMachineModels
        {
            get
            {
                //Lazy initialization
                if (aviationOtherBaseMachineModels == null)
                    aviationOtherBaseMachineModels = AviationOtherBaseMachineModelUtil.GetAllAviationOtherBaseMachineModels(AviationOtherBaseMachineMakeId, CurrentUser);

                return aviationOtherBaseMachineModels;
            }
            set
            {
                aviationOtherBaseMachineModels = value;
            }
        }

        public AviationOtherBaseMachineMake(User user)
            : base(user)
        {

        } 

        //IDropDownItem
        public string Value()
        {
            return AviationOtherBaseMachineMakeId.ToString();
        }

        public string Text()
        {
            return AviationOtherBaseMachineMakeName.ToString();
        }
    }

    public static class AviationOtherBaseMachineMakeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static AviationOtherBaseMachineMake ExtractAviationOtherBaseMachineMakeFromDataReader(OracleDataReader dr, User currentUser)
        {
            AviationOtherBaseMachineMake aviationOtherBaseMachineMake = new AviationOtherBaseMachineMake(currentUser);

            aviationOtherBaseMachineMake.AviationOtherBaseMachineMakeId = DBCommon.GetInt(dr["AviationOtherBaseMachineMakeID"]);
            aviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName = dr["AviationOtherBaseMachineMake"].ToString();

            return aviationOtherBaseMachineMake;
        }

        //Get a particular object by its ID
        public static AviationOtherBaseMachineMake GetAviationOtherBaseMachineMake(int aviationOtherBaseMachineMakeId, User currentUser)
        {
            AviationOtherBaseMachineMake aviationOtherBaseMachineMake = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AviationOtherBaseMachineMakeID, a.AviationOtherBaseMachineMake
                               FROM PMIS_RES.AviationOtherBaseMachineMakes a                       
                               WHERE a.AviationOtherBaseMachineMakeID = :AviationOtherBaseMachineMakeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationOtherBaseMachineMakeID", OracleType.Number).Value = aviationOtherBaseMachineMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    aviationOtherBaseMachineMake = ExtractAviationOtherBaseMachineMakeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineMake;
        }

        //Get a list of all types
        public static List<AviationOtherBaseMachineMake> GetAllAviationOtherBaseMachineMakes(User currentUser)
        {
            List<AviationOtherBaseMachineMake> aviationOtherBaseMachineMakes = new List<AviationOtherBaseMachineMake>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AviationOtherBaseMachineMakeID, a.AviationOtherBaseMachineMake
                               FROM PMIS_RES.AviationOtherBaseMachineMakes a 
                               ORDER BY a.AviationOtherBaseMachineMake";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    aviationOtherBaseMachineMakes.Add(ExtractAviationOtherBaseMachineMakeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineMakes;
        }
    }

}