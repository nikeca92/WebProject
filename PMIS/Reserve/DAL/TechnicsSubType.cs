using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TechnicsSubType : BaseDbObject
    {
        public int TechnicsSubTypeId { get; set; }
        public int TechnicsTypeId { get; set; }
        public string TechnicsSubTypeName { get; set; }
        public int? Seq { get; set; }
        public bool IsActive { get; set; }

        private TechnicsType technicsType;
        public TechnicsType TechnicsType
        {
            get
            {
                if (technicsType == null)
                    technicsType = TechnicsTypeUtil.GetTechnicsType(TechnicsTypeId, CurrentUser);

                return technicsType;
            }
            set { technicsType = value; }
        }

        public TechnicsSubType(User user)
            :base(user)
        {

        }
    }

    public static class TechnicsSubTypeUtil
    {
        public static TechnicsSubType ExtractTechnicsSubTypeFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsSubType technicsSubType = new TechnicsSubType(currentUser);

            technicsSubType.TechnicsSubTypeId = DBCommon.GetInt(dr["TechnicsSubTypeID"]);
            technicsSubType.TechnicsTypeId = DBCommon.GetInt(dr["TechnicsTypeID"]);
            technicsSubType.TechnicsSubTypeName = dr["TechnicsSubTypeName"].ToString();
            technicsSubType.Seq = (DBCommon.IsInt(dr["Seq"]) ? DBCommon.GetInt(dr["Seq"]) : (int?)null);
            technicsSubType.IsActive = DBCommon.IsInt(dr["IsActive"]) && (DBCommon.GetInt(dr["IsActive"]) == 1);

            return technicsSubType;
        }

        public static TechnicsSubType GetTechnicsSubType(int technicsSubTypeId, User currentUser)
        {
            TechnicsSubType technicsSubType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsSubTypeID,
                                      a.TechnicsTypeID,
                                      a.TechnicsSubTypeName,
                                      a.Seq,
                                      a.IsActive
                               FROM PMIS_RES.TechnicsSubTypes a                       
                               WHERE a.TechnicsSubTypeID = :TechnicsSubTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsSubTypeID", OracleType.Number).Value = technicsSubTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsSubType = ExtractTechnicsSubTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsSubType;
        }
    }

}