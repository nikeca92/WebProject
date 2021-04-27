using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class ProtocolType
    {
        private int protocolTypeId;        
        private string protocolTypeName;
        
        public int ProtocolTypeId
        {
            get { return protocolTypeId; }
            set { protocolTypeId = value; }
        }

        public string ProtocolTypeName
        {
            get { return protocolTypeName; }
            set { protocolTypeName = value; }
        }
  
    }

    public static class ProtocolTypeUtil
    {
        public static ProtocolType GetProtocolType(int protocolTypeId, User currentUser)
        {
            ProtocolType protocolType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProtocolTypeID as ProtocolTypeID, a.ProtocolTypeName as ProtocolTypeName
                               FROM PMIS_HS.PROTOCOLTYPES a                       
                               WHERE a.ProtocolTypeID = :ProtocolTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolTypeID", OracleType.Number).Value = protocolTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    protocolType = new ProtocolType();
                    protocolType.ProtocolTypeId = protocolTypeId;
                    protocolType.ProtocolTypeName = dr["ProtocolTypeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocolType;
        }

        public static List<ProtocolType> GetAllProtocolTypes(User currentUser)
        {
            List<ProtocolType> protocolTypes = new List<ProtocolType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProtocolTypeID as ProtocolTypeID
                               FROM PMIS_HS.PROTOCOLTYPES a";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ProtocolTypeID"]))
                        protocolTypes.Add(ProtocolTypeUtil.GetProtocolType(DBCommon.GetInt(dr["ProtocolTypeID"]), currentUser));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocolTypes;
        }
    }

}