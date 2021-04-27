using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PreviousPositionKind : IDropDownItem
    {
        public string PreviousPositionKindKey { get; set; }
        public string PreviousPositionKindName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return PreviousPositionKindName;
        }

        public string Value()
        {
            return PreviousPositionKindKey;
        }
    }
    public class PreviousPositionKindUtil
    {
        public static PreviousPositionKind GetPreviousPositionKind(string PreviousPositionKindKey, User currentUser)
        {
            PreviousPositionKind PreviousPositionKind = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PreviousPositionKindKey as PreviousPositionKindKey, a.PreviousPositionKindName as PreviousPositionKindName
                               FROM PMIS_ADM.PreviousPositionKinds a
                               WHERE a.PreviousPositionKindKey = :PreviousPositionKindKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PreviousPositionKindKey", OracleType.VarChar).Value = PreviousPositionKindKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PreviousPositionKind = new PreviousPositionKind();
                    PreviousPositionKind.PreviousPositionKindKey = PreviousPositionKindKey;
                    PreviousPositionKind.PreviousPositionKindName = dr["PreviousPositionKindName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PreviousPositionKind;
        }

        public static List<PreviousPositionKind> GetAllPreviousPositionKind(User currentUser)
        {
            List<PreviousPositionKind> listPreviousPositionKind = new List<PreviousPositionKind>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PreviousPositionKindKey as PreviousPositionKindKey, a.PreviousPositionKindName as PreviousPositionKindName
                               FROM PMIS_ADM.PreviousPositionKinds a
                               ORDER BY a.PreviousPositionKindName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PreviousPositionKind PreviousPositionKind = new PreviousPositionKind();
                    PreviousPositionKind.PreviousPositionKindKey = dr["PreviousPositionKindKey"].ToString();
                    PreviousPositionKind.PreviousPositionKindName = dr["PreviousPositionKindName"].ToString();
                    listPreviousPositionKind.Add(PreviousPositionKind);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPreviousPositionKind;
        }
    }
}



