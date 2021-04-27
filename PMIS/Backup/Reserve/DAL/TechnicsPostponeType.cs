using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TechnicsPostponeType : IDropDownItem
    {
        private int technicsPostponeTypeId;
        private string technicsPostponeTypeKey;
        private string technicsPostponeTypeName;

        public int TechnicsPostponeTypeId
        {
            get
            {
                return technicsPostponeTypeId;
            }
            set
            {
                technicsPostponeTypeId = value;
            }
        }

        public string TechnicsPostponeTypeKey
        {
            get
            {
                return technicsPostponeTypeKey;
            }
            set
            {
                technicsPostponeTypeKey = value;
            }
        }

        public string TechnicsPostponeTypeName
        {
            get
            {
                return technicsPostponeTypeName;
            }
            set
            {
                technicsPostponeTypeName = value;
            }
        }



        #region IDropDownItem Members

        public string Text()
        {
            return TechnicsPostponeTypeName;
        }

        public string Value()
        {
            return TechnicsPostponeTypeId.ToString();
        }

        #endregion
    }

    public static class TechnicsPostponeTypeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static TechnicsPostponeType ExtractTechnicsPostponeTypeFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsPostponeType technicsPostponeType = new TechnicsPostponeType();

            technicsPostponeType.TechnicsPostponeTypeId = DBCommon.GetInt(dr["TechnicsPostponeTypeID"]);
            technicsPostponeType.TechnicsPostponeTypeKey = dr["TechnicsPostponeTypeKey"].ToString();
            technicsPostponeType.TechnicsPostponeTypeName = dr["TechnicsPostponeTypeName"].ToString();

            return technicsPostponeType;
        }

        //Get a particular object by its ID
        public static TechnicsPostponeType GetTechnicsPostponeType(int technicsPostponeTypeId, User currentUser)
        {
            TechnicsPostponeType technicsPostponeType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsPostponeTypeID, a.TechnicsPostponeTypeKey, a.TechnicsPostponeTypeName
                               FROM PMIS_RES.TechnicsPostponeTypes a                       
                               WHERE a.TechnicsPostponeTypeID = :TechnicsPostponeTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsPostponeTypeID", OracleType.Number).Value = technicsPostponeTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsPostponeType = ExtractTechnicsPostponeTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsPostponeType;
        }

        //Get a particular object by its key
        public static TechnicsPostponeType GetTechnicsPostponeType(string technicsPostponeTypeKey, User currentUser)
        {
            TechnicsPostponeType technicsPostponeType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsPostponeTypeID, a.TechnicsPostponeTypeKey, a.TechnicsPostponeTypeName
                               FROM PMIS_RES.TechnicsPostponeTypes a                       
                               WHERE a.TechnicsPostponeTypeKey = :TechnicsPostponeTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsPostponeTypeKey", OracleType.VarChar).Value = technicsPostponeTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsPostponeType = ExtractTechnicsPostponeTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsPostponeType;
        }

        //Get a list of all types
        public static List<TechnicsPostponeType> GetAllTechnicsPostponeTypes(User currentUser)
        {
            List<TechnicsPostponeType> technicsPostponeTypes = new List<TechnicsPostponeType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsPostponeTypeID, a.TechnicsPostponeTypeKey, a.TechnicsPostponeTypeName
                               FROM PMIS_RES.TechnicsPostponeTypes a 
                               ORDER BY a.TechnicsPostponeTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsPostponeTypes.Add(ExtractTechnicsPostponeTypeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsPostponeTypes;
        }
    }

}