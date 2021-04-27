using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class NormativeTechnics : BaseDbObject, IDropDownItemWithTooltip
    {
        private int normativeTechnicsId;
        private string normativeCode;
        private string normativeName;
        public int? TechnicsSubTypeId { get; set; }

        public int NormativeTechnicsId
        {
            get
            {
                return normativeTechnicsId;
            }
            set
            {
                normativeTechnicsId = value;
            }
        }

        public string NormativeCode
        {
            get
            {
                return normativeCode;
            }
            set
            {
                normativeCode = value;
            }
        }

        public string NormativeName
        {
            get
            {
                return normativeName;
            }
            set
            {
                normativeName = value;
            }
        }

        public string CodeAndText
        {
            get
            {
                return NormativeCode + " " + NormativeName;
            }
        }

        private TechnicsSubType technicsSubType;
        public TechnicsSubType TechnicsSubType
        {
            get
            {
                //Lazy initialization
                if (technicsSubType == null && TechnicsSubTypeId.HasValue)
                    technicsSubType = TechnicsSubTypeUtil.GetTechnicsSubType(TechnicsSubTypeId.Value, CurrentUser);

                return technicsSubType;
            }
            set
            {
                technicsSubType = value;
            }
        }

        //IDropDownItem
        public string Value()
        {
            return normativeTechnicsId.ToString();
        }

        public string Text()
        {
            return normativeName;
        }

        public string Tooltip()
        {
            return "Код: " + normativeCode;
        }

        public NormativeTechnics(User user)
            : base(user)
        {

        }
    }

    public static class NormativeTechnicsUtil
    {
        //This method creates and returns a NormativeTechnics object. It extracts the data from a DataReader.
        public static NormativeTechnics ExtractNormativeTechnicsFromDataReader(OracleDataReader dr, User currentUser)
        {
            NormativeTechnics normativeTechnics = null;

            if (DBCommon.IsInt(dr["NormativeTechnicsID"]))
            {
                normativeTechnics = new NormativeTechnics(currentUser);
                normativeTechnics.NormativeTechnicsId = DBCommon.GetInt(dr["NormativeTechnicsID"]);
                normativeTechnics.NormativeCode = dr["NormativeCode"].ToString();
                normativeTechnics.NormativeName = dr["NormativeName"].ToString();
                normativeTechnics.TechnicsSubTypeId = DBCommon.IsInt(dr["TechnicsSubTypeID"]) ? DBCommon.GetInt(dr["TechnicsSubTypeID"]) : (int?)null;
            }

            return normativeTechnics;
        }

        public static NormativeTechnics GetNormativeTechnicsObj(User currentUser, int normativeTechnicsId)
        {
            NormativeTechnics normativeTechnics = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NormativeTechnicsID, a.NormativeCode, a.NormativeName, a.TechnicsSubTypeID
                               FROM PMIS_RES.NormativeTechnics a 
                               WHERE a.NormativeTechnicsID = :NormativeTechnicsID
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("NormativeTechnicsID", OracleType.Number).Value = normativeTechnicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    normativeTechnics = ExtractNormativeTechnicsFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return normativeTechnics;
        }

        public static NormativeTechnics GetNormativeTechnicsObjByCode(User currentUser, string normativeCode, string technicsTypeKey)
        {
            NormativeTechnics normativeTechnics = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NormativeTechnicsID, a.NormativeCode, a.NormativeName, a.TechnicsSubTypeID
                               FROM PMIS_RES.NormativeTechnics a 
                               WHERE a.TechnicsTypeID = :TechnicsTypeID AND
                                     a.NormativeCode = :NormativeCode
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, currentUser);
                cmd.Parameters.Add("TechnicsTypeID", OracleType.Number).Value = technicsType.TechnicsTypeId;
                cmd.Parameters.Add("NormativeCode", OracleType.VarChar).Value = normativeCode;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    normativeTechnics = ExtractNormativeTechnicsFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return normativeTechnics;
        }


        /*
        //Get a particular object by its ID
        public static TechnicsType GetTechnicsType(int technicsTypeId, User currentUser)
        {
            TechnicsType technicsType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsTypeID, a.TechnicsTypeKey, a.TechnicsTypeName
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
                string SQL = @"SELECT a.TechnicsTypeID, a.TechnicsTypeKey, a.TechnicsTypeName
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
        }*/

        //Get a list of all types
        public static List<NormativeTechnics> GetNormativeTechnics(User currentUser, string technicsTypeKey)
        {
            List<NormativeTechnics> normativeTechnics = new List<NormativeTechnics>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NormativeTechnicsID, a.NormativeCode, a.NormativeName, a.TechnicsSubTypeID
                               FROM PMIS_RES.NormativeTechnics a 
                               WHERE a.TechnicsTypeID = :TechnicsTypeID
                               ORDER BY a.Sort ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, currentUser);
                cmd.Parameters.Add("TechnicsTypeID", OracleType.Number).Value = technicsType.TechnicsTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    normativeTechnics.Add(ExtractNormativeTechnicsFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return normativeTechnics;
        }

        public static List<NormativeTechnics> GetNormativeTechnicsByVehicleKind(User currentUser, string technicsTypeKey, int? vehicleKindId)
        {
            List<NormativeTechnics> normativeTechnics = new List<NormativeTechnics>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NormativeTechnicsID, a.NormativeCode, a.NormativeName, a.TechnicsSubTypeID
                               FROM PMIS_RES.NormativeTechnics a 
                               WHERE a.TechnicsTypeID = :TechnicsTypeID AND (a.Veh_VehicleKindID = :Veh_VehicleKindID OR :Veh_VehicleKindID IS NULL)
                               ORDER BY a.Sort ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, currentUser);
                cmd.Parameters.Add("TechnicsTypeID", OracleType.Number).Value = technicsType.TechnicsTypeId;
                if(vehicleKindId.HasValue)
                    cmd.Parameters.Add("Veh_VehicleKindID", OracleType.Number).Value = vehicleKindId.Value;
                else
                    cmd.Parameters.Add("Veh_VehicleKindID", OracleType.Number).Value = DBNull.Value;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    normativeTechnics.Add(ExtractNormativeTechnicsFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return normativeTechnics;
        }
    }

}