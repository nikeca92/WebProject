using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single person education type from KLV_VVO.VVO_KOD table
    public class MilitaryEducationType : IDropDownItem
    {
        private string militaryEducationTypeCode;

        public string MilitaryEducationTypeCode
        {
            get { return militaryEducationTypeCode; }
            set { militaryEducationTypeCode = value; }
        }
        private string militaryEducationTypeName;

        public string MilitaryEducationTypeName
        {
            get { return militaryEducationTypeName; }
            set { militaryEducationTypeName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitaryEducationTypeName;
        }

        public string Value()
        {
            return MilitaryEducationTypeCode;
        }
    }

   public class MilitaryEducationTypeUtil
   {

       public static MilitaryEducationType GetMilitaryEducationType(string militaryEducationTypeCode, User currentUser)
       {
           MilitaryEducationType militaryEducationType = null;

           OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
           conn.Open();

           try
           {
               string SQL = @"SELECT a.VVO_KOD as MilitaryEducationType, a.VVO_IME as MilitaryEducationTypeName
                               FROM VS_OWNER.KLV_VVO a
                               WHERE a.VVO_KOD = :MilitaryEducationTypeCode";

               OracleCommand cmd = new OracleCommand(SQL, conn);

               cmd.Parameters.Add("MilitaryEducationTypeCode", OracleType.VarChar).Value = militaryEducationTypeCode;

               OracleDataReader dr = cmd.ExecuteReader();

               if (dr.Read())
               {
                   militaryEducationType = new MilitaryEducationType();
                   militaryEducationType.MilitaryEducationTypeCode = militaryEducationTypeCode;
                   militaryEducationType.MilitaryEducationTypeName = dr["MilitaryEducationTypeName"].ToString();
               }

               dr.Close();
           }
           finally
           {
               conn.Close();
           }

           return militaryEducationType;
       }

       public static List<MilitaryEducationType> GetAllMilitaryEducationTypes(User currentUser)
       {
           List<MilitaryEducationType> listMilitaryEducationType = new List<MilitaryEducationType>();

           OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
           try
           {
               conn.Open();
               string SQL = @"SELECT a.VVO_KOD as MilitaryEducationType, a.VVO_IME as MilitaryEducationTypeName
                               FROM VS_OWNER.KLV_VVO a
                               ORDER BY a.VVO_KOD";

               OracleCommand cmd = new OracleCommand(SQL, conn);

               OracleDataReader dr = cmd.ExecuteReader();

               while (dr.Read())
               {
                   MilitaryEducationType militaryEducationType = new MilitaryEducationType();
                   militaryEducationType.MilitaryEducationTypeCode = dr["MilitaryEducationType"].ToString();
                   militaryEducationType.MilitaryEducationTypeName = dr["MilitaryEducationTypeName"].ToString();
                   listMilitaryEducationType.Add(militaryEducationType);
               }

               dr.Close();
           }
           finally
           {
               conn.Close();
           }

           return listMilitaryEducationType;
       }
   }

}
