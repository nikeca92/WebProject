using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{

    public class Penalty : IDropDownItem
    {
        public string PenaltyCode { get; set; }
        public string PenaltyName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return PenaltyName;
        }

        public string Value()
        {
            return PenaltyCode;
        }
    }

    public class PenaltyUtil
    {
        public static Penalty GetPenalty(string PenaltyCode, User currentUser)
        {
            Penalty Penalty = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NKZ_KOD as PenaltyCode, a.NKZ_IME as PenaltyName
                               FROM VS_OWNER.KLV_NKZ a
                               WHERE a.NKZ_KOD = :PenaltyCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PenaltyCode", OracleType.VarChar).Value = PenaltyCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Penalty = new Penalty();
                    Penalty.PenaltyCode = dr["PenaltyCode"].ToString();
                    Penalty.PenaltyName = dr["PenaltyName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Penalty;
        }

        public static List<Penalty> GetAllPenalties(User currentUser)
        {
            List<Penalty> listPenaltys = new List<Penalty>();
            Penalty Penalty;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.NKZ_KOD as PenaltyCode, a.NKZ_IME as PenaltyName
                               FROM VS_OWNER.KLV_NKZ a
                               ORDER BY a.NKZ_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Penalty = new Penalty();
                    Penalty.PenaltyCode = dr["PenaltyCode"].ToString();
                    Penalty.PenaltyName = dr["PenaltyName"].ToString();

                    listPenaltys.Add(Penalty);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPenaltys;
        }
    }
}

