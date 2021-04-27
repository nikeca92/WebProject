using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class Speciality : IDropDownItem
    {
        public int SpecialityId { get; set; }
        public string SpecialityName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return SpecialityName;
        }

        public string Value()
        {
            return SpecialityId.ToString();
        }
    }

  public class SpecialityUtil
  {
      public static Speciality GetSpeciality(int specialityId, User currentUser)
      {
          Speciality speciality = null;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          conn.Open();

          try
          {
              string SQL = @"SELECT a.SpecialityID, a.SpecialityName
                             FROM PMIS_ADM.Specialities a
                             WHERE a.SpecialityID = :SpecialityID";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("SpecialityID", OracleType.Number).Value = specialityId;

              OracleDataReader dr = cmd.ExecuteReader();

              if (dr.Read())
              {
                  speciality = new Speciality();
                  speciality.SpecialityId = DBCommon.GetInt(dr["SpecialityID"]);
                  speciality.SpecialityName = dr["SpecialityName"].ToString();
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return speciality;
      }

      public static Speciality GetSpeciality(int professionId, string specialityName, User currentUser)
      {
          Speciality speciality = null;

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          conn.Open();

          try
          {
              string SQL = @"SELECT a.SpecialityID, a.SpecialityName
                             FROM PMIS_ADM.Specialities a
                             WHERE a.ProfessionID = :ProfessionID AND
                                   UPPER(a.SpecialityName) = UPPER(:SpecialityName)";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("ProfessionID", OracleType.Number).Value = professionId;
              cmd.Parameters.Add("SpecialityName", OracleType.VarChar).Value = specialityName;

              OracleDataReader dr = cmd.ExecuteReader();

              if (dr.Read())
              {
                  speciality = new Speciality();
                  speciality.SpecialityId = DBCommon.GetInt(dr["SpecialityID"]);
                  speciality.SpecialityName = dr["SpecialityName"].ToString();
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return speciality;
      }

      public static List<Speciality> GetSpecialities(int professionId, User currentUser)
      {
          List<Speciality> listSpecialities = new List<Speciality>();          

          OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
          try
          {
              conn.Open();
              string SQL = @"SELECT a.SpecialityID, a.SpecialityName
                             FROM PMIS_ADM.Specialities a
                             WHERE a.ProfessionID = :ProfessionID
                             ORDER BY a.SpecialityName";

              OracleCommand cmd = new OracleCommand(SQL, conn);

              cmd.Parameters.Add("ProfessionID", OracleType.Number).Value = professionId;

              OracleDataReader dr = cmd.ExecuteReader();

              while (dr.Read())
              {
                  Speciality speciality = new Speciality();
                  speciality.SpecialityId = DBCommon.GetInt(dr["SpecialityID"]);
                  speciality.SpecialityName = dr["SpecialityName"].ToString();

                  listSpecialities.Add(speciality);
              }

              dr.Close();
          }
          finally
          {
              conn.Close();
          }

          return listSpecialities;
      }
  }

}
