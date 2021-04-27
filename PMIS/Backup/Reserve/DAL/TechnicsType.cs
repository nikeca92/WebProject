using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TechnicsType
    {
        private int technicsTypeId;
        private string typeKey;
        private string typeName;
        private bool active;

        public int TechnicsTypeId
        {
            get
            {
                return technicsTypeId;
            }
            set
            {
                technicsTypeId = value;
            }
        }

        public string TypeKey
        {
            get
            {
                return typeKey;
            }
            set
            {
                typeKey = value;
            }
        }

        public string TypeName
        {
            get
            {
                return typeName;
            }
            set
            {
                typeName = value;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }
    }

    public static class TechnicsTypeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static TechnicsType ExtractTechnicsTypeFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsType technicsType = new TechnicsType();

            technicsType.TechnicsTypeId = DBCommon.GetInt(dr["TechnicsTypeID"]);
            technicsType.TypeKey = dr["TechnicsTypeKey"].ToString();
            technicsType.TypeName = dr["TechnicsTypeName"].ToString();
            technicsType.Active = DBCommon.IsInt(dr["TechnicsTypeActive"]) && (DBCommon.GetInt(dr["TechnicsTypeActive"]) == 1);

            return technicsType;
        }

        //Get a particular object by its ID
        public static TechnicsType GetTechnicsType(int technicsTypeId, User currentUser)
        {
            TechnicsType technicsType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsTypeID, a.TechnicsTypeKey, a.TechnicsTypeName,
                                      a.Active as TechnicsTypeActive
                               FROM PMIS_RES.TechnicsTypes a                       
                               WHERE a.TechnicsTypeID = :TechnicsTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsTypeID", OracleType.Number).Value = technicsTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsType = ExtractTechnicsTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsType;
        }

        //Get a particular object by its key
        public static TechnicsType GetTechnicsType(string technicsTypeKey, User currentUser)
        {
            TechnicsType technicsType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsTypeID, a.TechnicsTypeKey, a.TechnicsTypeName,
                                      a.Active as TechnicsTypeActive
                               FROM PMIS_RES.TechnicsTypes a                       
                               WHERE a.TechnicsTypeKey = :TechnicsTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsTypeKey", OracleType.VarChar).Value = technicsTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsType = ExtractTechnicsTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsType;
        }

        //Get a list of all types
        public static List<TechnicsType> GetAllTechnicsTypes(User currentUser)
        {
            List<TechnicsType> technicsTypes = new List<TechnicsType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsTypeID, a.TechnicsTypeKey, a.TechnicsTypeName,
                                      a.Active as TechnicsTypeActive
                               FROM PMIS_RES.TechnicsTypes a 
                               WHERE NVL(a.Active, 0) = 1
                               ORDER BY a.Seq, a.TechnicsTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsTypes.Add(ExtractTechnicsTypeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsTypes;
        }
    }

}