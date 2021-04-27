using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryForceType
    {
        private int militaryForceTypeId;
        private string militaryForceTypeName;

        public int MilitaryForceTypeId
        {
            get { return militaryForceTypeId; }
            set { militaryForceTypeId = value; }
        }

        public string MilitaryForceTypeName
        {
            get { return militaryForceTypeName; }
            set { militaryForceTypeName = value; }
        }
    }

    public static class MilitaryForceTypeUtil
    {
        public static MilitaryForceType GetMilitaryForceType(int militaryForceTypeId, User currentUser)
        {
            MilitaryForceType militaryForceType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryForceTypeName
                               FROM PMIS_ADM.MilitaryForceTypes a
                               WHERE a.MilitaryForceTypeID = :MilitaryForceTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryForceTypeID", OracleType.Number).Value = militaryForceTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryForceType = new MilitaryForceType();
                    militaryForceType.MilitaryForceTypeId = militaryForceTypeId;
                    militaryForceType.MilitaryForceTypeName = dr["MilitaryForceTypeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryForceType;
        }

        public static List<MilitaryForceType> GetAllMilitaryForceTypes(User currentUser)
        {
            List<MilitaryForceType> militaryForceTypes = new List<MilitaryForceType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryForceTypeID, a.MilitaryForceTypeName
                               FROM PMIS_ADM.MilitaryForceTypes a
                               ORDER BY a.MilitaryForceTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryForceType militaryForceType = new MilitaryForceType();
                    militaryForceType.MilitaryForceTypeId = DBCommon.GetInt(dr["MilitaryForceTypeID"]);
                    militaryForceType.MilitaryForceTypeName = dr["MilitaryForceTypeName"].ToString();

                    militaryForceTypes.Add(militaryForceType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryForceTypes;
        }
    }
}
