using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryMedicalConclusion : BaseDbObject, IDropDownItem
    {
        private int militaryMedicalConclusionId;        
        private string militaryMedicalConclusionKey;        
        private string militaryMedicalConclusionName;
        
        public int MilitaryMedicalConclusionId
        {
            get { return militaryMedicalConclusionId; }
            set { militaryMedicalConclusionId = value; }
        }
        
        public string MilitaryMedicalConclusionKey
        {
            get { return militaryMedicalConclusionKey; }
            set { militaryMedicalConclusionKey = value; }
        }
        
        public string MilitaryMedicalConclusionName
        {
            get { return militaryMedicalConclusionName; }
            set { militaryMedicalConclusionName = value; }
        }

        public MilitaryMedicalConclusion(User currentUser)
            : base(currentUser)
        {
        }
        
        //IDropDownItem Members
        public string Text()
        {
            return MilitaryMedicalConclusionName;
        }

        public string Value()
        {
            return MilitaryMedicalConclusionId.ToString();
        }
    }

    public static class MilitaryMedicalConclusionUtil
    {
        public static MilitaryMedicalConclusion GetMilitaryMedicalConclusion(int militaryMedicalConclusionId, User currentUser)
        {
            MilitaryMedicalConclusion militaryMedicalConclusion = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryMedicalConclusionID, 
                                      a.MilitaryMedicalConclusionKey,
                                      a.MilitaryMedicalConclusionName
                               FROM PMIS_ADM.MilitaryMedicalConclusions a 
                               WHERE a.MilitaryMedicalConclusionID = :MilitaryMedicalConclusionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryMedicalConclusionID", OracleType.Number).Value = militaryMedicalConclusionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryMedicalConclusion = new MilitaryMedicalConclusion(currentUser);
                    militaryMedicalConclusion.MilitaryMedicalConclusionId = DBCommon.GetInt(dr["MilitaryMedicalConclusionID"]);
                    militaryMedicalConclusion.MilitaryMedicalConclusionKey = dr["MilitaryMedicalConclusionKey"].ToString();
                    militaryMedicalConclusion.MilitaryMedicalConclusionName = dr["MilitaryMedicalConclusionName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryMedicalConclusion;
        }

        public static List<MilitaryMedicalConclusion> GetAllMilitaryMedicalConclusions(User currentUser)
        {
            List<MilitaryMedicalConclusion> militaryMedicalConclusions = new List<MilitaryMedicalConclusion>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryMedicalConclusionID, 
                                      a.MilitaryMedicalConclusionKey,
                                      a.MilitaryMedicalConclusionName
                               FROM PMIS_ADM.MilitaryMedicalConclusions a";

                OracleCommand cmd = new OracleCommand(SQL, conn);               

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryMedicalConclusion militaryMedicalConclusion = new MilitaryMedicalConclusion(currentUser);
                    militaryMedicalConclusion.MilitaryMedicalConclusionId = DBCommon.GetInt(dr["MilitaryMedicalConclusionID"]);
                    militaryMedicalConclusion.MilitaryMedicalConclusionKey = dr["MilitaryMedicalConclusionKey"].ToString();
                    militaryMedicalConclusion.MilitaryMedicalConclusionName = dr["MilitaryMedicalConclusionName"].ToString();

                    militaryMedicalConclusions.Add(militaryMedicalConclusion);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryMedicalConclusions;
        }              
    }
}
