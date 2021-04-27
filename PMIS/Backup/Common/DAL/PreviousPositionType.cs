using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PreviousPositionType : IDropDownItem
    {
        public string PreviousPositionTypeKey { get; set; }
        public string PreviousPositionTypeName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return PreviousPositionTypeName;
        }

        public string Value()
        {
            return PreviousPositionTypeKey;
        }
    }
    public class PreviousPositionTypeUtil
    {
        public static PreviousPositionType GetPreviousPositionType(string PreviousPositionTypeKey, User currentUser)
        {
            PreviousPositionType PreviousPositionType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PreviousPositionTypeKey as PreviousPositionTypeKey, a.PreviousPositionTypeName as PreviousPositionTypeName
                               FROM PMIS_ADM.PreviousPositionTypes a
                               WHERE a.PreviousPositionTypeKey = :PreviousPositionTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PreviousPositionTypeKey", OracleType.VarChar).Value = PreviousPositionTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PreviousPositionType = new PreviousPositionType();
                    PreviousPositionType.PreviousPositionTypeKey = PreviousPositionTypeKey;
                    PreviousPositionType.PreviousPositionTypeName = dr["PreviousPositionTypeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PreviousPositionType;
        }

        public static List<PreviousPositionType> GetAllPreviousPositionType(User currentUser)
        {
            List<PreviousPositionType> listPreviousPositionType = new List<PreviousPositionType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PreviousPositionTypeKey as PreviousPositionTypeKey, a.PreviousPositionTypeName as PreviousPositionTypeName
                               FROM PMIS_ADM.PreviousPositionTypes a
                               ORDER BY a.PreviousPositionTypeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PreviousPositionType PreviousPositionType = new PreviousPositionType();
                    PreviousPositionType.PreviousPositionTypeKey = dr["PreviousPositionTypeKey"].ToString();
                    PreviousPositionType.PreviousPositionTypeName = dr["PreviousPositionTypeName"].ToString();
                    listPreviousPositionType.Add(PreviousPositionType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPreviousPositionType;
        }
    }
}


