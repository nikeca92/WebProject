using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TractorMake : BaseDbObject, IDropDownItem
    {
        private int tractorMakeId;
        private string tractorMakeName;
        private List<TractorModel> tractorModels;

        public int TractorMakeId
        {
            get
            {
                return tractorMakeId;
            }
            set
            {
                tractorMakeId = value;
            }
        }

        public string TractorMakeName
        {
            get
            {
                return tractorMakeName;
            }
            set
            {
                tractorMakeName = value;
            }
        }

        public List<TractorModel> TractorModels
        {
            get
            {
                //Lazy initialization
                if (tractorModels == null)
                    tractorModels = TractorModelUtil.GetAllTractorModels(TractorMakeId, CurrentUser);

                return tractorModels;
            }
            set
            {
                tractorModels = value;
            }
        }

        public TractorMake(User user)
            : base(user)
        {

        } 

        //IDropDownItem
        public string Value()
        {
            return TractorMakeId.ToString();
        }

        public string Text()
        {
            return TractorMakeName.ToString();
        }
    }

    public static class TractorMakeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static TractorMake ExtractTractorMakeFromDataReader(OracleDataReader dr, User currentUser)
        {
            TractorMake tractorMake = new TractorMake(currentUser);

            tractorMake.TractorMakeId = DBCommon.GetInt(dr["TractorMakeID"]);
            tractorMake.TractorMakeName = dr["TractorMakeName"].ToString();

            return tractorMake;
        }

        //Get a particular object by its ID
        public static TractorMake GetTractorMake(int tractorMakeId, User currentUser)
        {
            TractorMake tractorMake = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TractorMakeID, a.TractorMakeName
                               FROM PMIS_RES.TractorMakes a                       
                               WHERE a.TractorMakeID = :TractorMakeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorMakeID", OracleType.Number).Value = tractorMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractorMake = ExtractTractorMakeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorMake;
        }

        //Get a list of all types
        public static List<TractorMake> GetAllTractorMakes(User currentUser)
        {
            List<TractorMake> tractorMakes = new List<TractorMake>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TractorMakeID, a.TractorMakeName
                               FROM PMIS_RES.TractorMakes a 
                               ORDER BY a.TractorMakeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tractorMakes.Add(ExtractTractorMakeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorMakes;
        }
    }

}