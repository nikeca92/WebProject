using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{

    public class MilitaryCommanderRank : IDropDownItem
    {
        public string MilitaryCommanderRankCode { get; set; }
        public string MilitaryCommanderRankName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return MilitaryCommanderRankName;
        }

        public string Value()
        {
            return MilitaryCommanderRankCode;
        }
    }

    public class MilitaryCommanderRankUtil
    {
        public static MilitaryCommanderRank GetMilitaryCommanderRank(string militaryCommanderRankCode, User currentUser)
        {
            MilitaryCommanderRank militaryCommanderRank = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SPZ_KOD as MilitaryCommanderRankCode, a.SPZ_IME as MilitaryCommanderRankName
                               FROM VS_OWNER.KLV_SPZ a
                               WHERE a.SPZ_KOD = :MilitaryCommanderRankCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommanderRankCode", OracleType.VarChar).Value = militaryCommanderRankCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryCommanderRank = new MilitaryCommanderRank();
                    militaryCommanderRank.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();
                    militaryCommanderRank.MilitaryCommanderRankName = dr["MilitaryCommanderRankName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommanderRank;
        }

        public static List<MilitaryCommanderRank> GetAllMilitaryCommanderRanks(User currentUser)
        {
            List<MilitaryCommanderRank> listMilitaryCommanderRanks = new List<MilitaryCommanderRank>();
            MilitaryCommanderRank militaryCommanderRank;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SPZ_KOD as MilitaryCommanderRankCode, a.SPZ_IME as MilitaryCommanderRankName
                               FROM VS_OWNER.KLV_SPZ a
                               ORDER BY a.SPZ_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommanderRank = new MilitaryCommanderRank();
                    militaryCommanderRank.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();
                    militaryCommanderRank.MilitaryCommanderRankName = dr["MilitaryCommanderRankName"].ToString();

                    listMilitaryCommanderRanks.Add(militaryCommanderRank);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryCommanderRanks;
        }
    }
}
