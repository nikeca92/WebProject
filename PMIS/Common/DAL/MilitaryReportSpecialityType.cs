using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryReportSpecialityType : IDropDownItem
    {
        private int type;
        private string typeName;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }        

        public string Text()
        {
            return TypeName;
        }

        public string Value()
        {
            return Type.ToString();
        }
    }

    public static class MilitaryReportSpecialityTypeUtil
    {
        public static MilitaryReportSpecialityType GetMilitaryReportSpecialityType(int type, User currentUser)
        {
            MilitaryReportSpecialityType militaryReportSpecialityType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.Type, a.TypeName
                               FROM PMIS_ADM.MilitaryReportSpecialityTypes a
                               WHERE a.Type = :Type";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Type", OracleType.Number).Value = type;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportSpecialityType = new MilitaryReportSpecialityType();
                    militaryReportSpecialityType.Type = DBCommon.GetInt(dr["Type"]);
                    militaryReportSpecialityType.TypeName = dr["TypeName"].ToString();                   
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialityType;
        }

        public static List<MilitaryReportSpecialityType> GetAllMilitaryReportSpecialityTypes(User currentUser)
        {
            List<MilitaryReportSpecialityType> militaryReportSpecialityTypes = new List<MilitaryReportSpecialityType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.Type, a.TypeName
                               FROM PMIS_ADM.MilitaryReportSpecialityTypes a
                               ORDER BY a.Type";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryReportSpecialityType militaryReportSpecialityType = new MilitaryReportSpecialityType();
                    militaryReportSpecialityType.Type = DBCommon.GetInt(dr["Type"]);
                    militaryReportSpecialityType.TypeName = dr["TypeName"].ToString();

                    militaryReportSpecialityTypes.Add(militaryReportSpecialityType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialityTypes;
        }
    }
}
