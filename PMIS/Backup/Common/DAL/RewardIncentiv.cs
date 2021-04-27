using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
  public  class RewardIncentiv : IDropDownItem
    {

      public string RewardIncentivCode { get; set; }
      public string RewardIncentivName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return RewardIncentivName;
        }

        public string Value()
        {
            return RewardIncentivCode;
        }
    }

  public class RewardIncentivUtil
  {
      public static RewardIncentiv GetRewardIncentiv(string RewardIncentivCode, User currentUser)
      {
          RewardIncentiv rewardIncentiv = null;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          conn.Open();

          try
          {
              string SQL = @"SELECT a.NGR_KOD as RewardIncentivCode, a.NGR_IME as RewardIncentivName
                               FROM VS_OWNER.KLV_NGR a
                               WHERE a.NGR_KOD = :RewardIncentivCode";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("RewardIncentivCode", OracleType.VarChar).Value = RewardIncentivCode;

              OracleDataReader dr = cmd.ExecuteReader();

              if (dr.Read())
              {
                  rewardIncentiv = new RewardIncentiv();
                  rewardIncentiv.RewardIncentivCode = dr["RewardIncentivCode"].ToString();
                  rewardIncentiv.RewardIncentivName = dr["RewardIncentivName"].ToString();
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return rewardIncentiv;
      }

      public static List<RewardIncentiv> GetAllRewardIncentivs(User currentUser)
      {
          List<RewardIncentiv> listRewardIncentivs = new List<RewardIncentiv>();
          RewardIncentiv rewardIncentiv;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          try
          {
              conn.Open();
              string SQL = @"SELECT a.NGR_KOD as RewardIncentivCode, a.NGR_IME as RewardIncentivName
                               FROM VS_OWNER.KLV_NGR a
                               ORDER BY a.NGR_IME";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              OracleDataReader dr = cmd.ExecuteReader();

              while (dr.Read())
              {
                  rewardIncentiv = new RewardIncentiv();
                  rewardIncentiv.RewardIncentivCode = dr["RewardIncentivCode"].ToString();
                  rewardIncentiv.RewardIncentivName = dr["RewardIncentivName"].ToString();

                  listRewardIncentivs.Add(rewardIncentiv);
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return listRewardIncentivs;
      }
  }

}
