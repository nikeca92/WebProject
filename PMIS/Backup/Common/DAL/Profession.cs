using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class Profession : IDropDownItem
    {
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return ProfessionName;
        }

        public string Value()
        {
            return ProfessionId.ToString();
        }
    }

  public class ProfessionUtil
  {
      public static Profession GetProfession(int professionId, User currentUser)
      {
          Profession profession = null;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          conn.Open();

          try
          {
              string SQL = @"SELECT a.ProfessionID, a.ProfessionName
                             FROM PMIS_ADM.Professions a
                             WHERE a.ProfessionID = :ProfessionID";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("ProfessionID", OracleType.Number).Value = professionId;

              OracleDataReader dr = cmd.ExecuteReader();

              if (dr.Read())
              {
                  profession = new Profession();
                  profession.ProfessionId = DBCommon.GetInt(dr["ProfessionID"]);
                  profession.ProfessionName = dr["ProfessionName"].ToString();
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return profession;
      }

      public static Profession GetProfession(string professionName, User currentUser)
      {
          Profession profession = null;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          conn.Open();

          try
          {
              string SQL = @"SELECT a.ProfessionID, a.ProfessionName
                             FROM PMIS_ADM.Professions a
                             WHERE UPPER(a.ProfessionName) = UPPER(:ProfessionName)";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("ProfessionName", OracleType.VarChar).Value = professionName;

              OracleDataReader dr = cmd.ExecuteReader();

              if (dr.Read())
              {
                  profession = new Profession();
                  profession.ProfessionId = DBCommon.GetInt(dr["ProfessionID"]);
                  profession.ProfessionName = dr["ProfessionName"].ToString();
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return profession;
      }

      public static List<Profession> GetAllProfessions(User currentUser)
      {
          List<Profession> listProfessions = new List<Profession>();

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          try
          {
              conn.Open();
              string SQL = @"SELECT a.ProfessionID, a.ProfessionName
                             FROM PMIS_ADM.Professions a
                             ORDER BY a.ProfessionName";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              OracleDataReader dr = cmd.ExecuteReader();

              while (dr.Read())
              {
                  Profession profession = new Profession();
                  profession.ProfessionId = DBCommon.GetInt(dr["ProfessionID"]);
                  profession.ProfessionName = dr["ProfessionName"].ToString();

                  listProfessions.Add(profession);
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return listProfessions;
      }
  }

}
