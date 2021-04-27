using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryRank : IDropDownItem
    {
        private string militaryRankId;        
        private string shortName;
        private string longName;
        private MilitaryCategory militaryCategory;

        public string MilitaryRankId
        {
            get { return militaryRankId; }
            set { militaryRankId = value; }
        }

        public string ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        public string LongName
        {
            get { return longName; }
            set { longName = value; }
        }

        public MilitaryCategory MilitaryCategory
        {
            get
            {
                return militaryCategory;
            }
            set
            {
                militaryCategory = value;
            }
        }      

        public string Text()
        {
            return LongName;
        }

        public string Value()
        {
            return MilitaryRankId.ToString();
        }

    }

    public static class MilitaryRankUtil
    {
        //Extract a particular MilitaryRank object from a data reader
        public static MilitaryRank ExtractMilitaryRankFromDR(User currentUser, OracleDataReader dr)
        {
            MilitaryRank militaryRank = new MilitaryRank();

            militaryRank.MilitaryRankId = dr["MilitaryRankID"].ToString();
            militaryRank.ShortName = dr["MilRankShortName"].ToString();
            militaryRank.LongName = dr["MilRankLongName"].ToString();
            militaryRank.MilitaryCategory = new MilitaryCategory();
            militaryRank.MilitaryCategory.CategoryName = dr["MilCategoryName"].ToString();

            return militaryRank;
        }

        public static MilitaryRank GetMilitaryRank(string militaryRankId, User currentUser)
        {
            MilitaryRank militaryRank = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ZVA_KOD as MilitaryRankID, a.ZVA_IMEES as MilRankShortName, 
                                      a.ZVA_IME as MilRankLongName,
                                      b.KAT_IME as MilCategoryName
                               FROM VS_OWNER.KLV_ZVA a
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT b ON a.ZVA_KAT_KOD = b.KAT_KOD
                               WHERE a.ZVA_KOD = :MilitaryRankID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryRankID", OracleType.VarChar).Value = (militaryRankId != null ? militaryRankId : "");

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryRank = ExtractMilitaryRankFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryRank;
        }

        public static List<MilitaryRank> GetAllMilitaryRanks(User currentUser)
        {
            List<MilitaryRank> listMilitaryRanks = new List<MilitaryRank>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.ZVA_KOD as MilitaryRankID, a.ZVA_IMEES as MilRankShortName, 
                                      a.ZVA_IME as MilRankLongName,
                                      b.KAT_IME as MilCategoryName
                               FROM VS_OWNER.KLV_ZVA a
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT b ON a.ZVA_KAT_KOD = b.KAT_KOD
                               ORDER BY a.ZVA_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryRankID"]))
                    {
                        MilitaryRank militaryRank = ExtractMilitaryRankFromDR(currentUser, dr);
                        listMilitaryRanks.Add(militaryRank);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryRanks;
        }

        public static List<MilitaryRank> GetAllMilitaryRanksByCategory(string militaryCategory, User currentUser)
        {
            List<MilitaryRank> listMilitaryRanks = new List<MilitaryRank>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.ZVA_KOD as MilitaryRankID, a.ZVA_IMEES as MilRankShortName, 
                                      a.ZVA_IME as MilRankLongName,
                                      b.KAT_IME as MilCategoryName
                               FROM VS_OWNER.KLV_ZVA a
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT b ON a.ZVA_KAT_KOD = b.KAT_KOD
                               WHERE b.KAT_KOD = :MilitaryCategoryID
                               ORDER BY a.ZVA_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCategoryID", OracleType.VarChar).Value = militaryCategory;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryRank militaryRank = ExtractMilitaryRankFromDR(currentUser, dr);
                    listMilitaryRanks.Add(militaryRank);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryRanks;
        }
    }
}
