using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MedicalRubric : BaseDbObject, IDropDownItem
    {
        private int medicalRubricID;        
        private string medicalRubricTitle;        
        private bool medicalRubricIsActive;
        
        public int MedicalRubricID
        {
            get { return medicalRubricID; }
            set { medicalRubricID = value; }
        }
        
        public string MedicalRubricTitle
        {
            get { return medicalRubricTitle; }
            set { medicalRubricTitle = value; }
        }
        
        public bool MedicalRubricIsActive
        {
            get { return medicalRubricIsActive; }
            set { medicalRubricIsActive = value; }
        }

        public MedicalRubric(User currentUser)
            : base(currentUser)
        {
        }
        
        //IDropDownItem Members
        public string Text()
        {
            return MedicalRubricTitle;
        }

        public string Value()
        {
            return medicalRubricID.ToString();
        }
    }

    public static class MedicalRubricUtil
    {
        public static MedicalRubric GetMedicalRubric(int medicalRubricID, User currentUser)
        {
            MedicalRubric medicalRubric = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MedRubricID, 
                                      a.MedRubricTitle,
                                      a.IsActive
                               FROM PMIS_ADM.MedRubrics a 
                               WHERE a.MedRubricID = :MedicalRubricID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MedicalRubricID", OracleType.Number).Value = medicalRubricID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    medicalRubric = new MedicalRubric(currentUser);
                    medicalRubric.MedicalRubricID = DBCommon.GetInt(dr["MedRubricID"]);
                    medicalRubric.MedicalRubricTitle = dr["MedRubricTitle"].ToString();

                    if (dr["IsActive"] is bool)
                        medicalRubric.MedicalRubricIsActive = (bool)dr["IsActive"];
                    else
                        medicalRubric.MedicalRubricIsActive = false;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return medicalRubric;
        }

        public static List<MedicalRubric> GetAllMedicalRubrics(User currentUser)
        {
            List<MedicalRubric> medicalRubrics = new List<MedicalRubric>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MedRubricID, 
                                      a.MedRubricTitle,
                                      a.IsActive
                               FROM PMIS_ADM.MedRubrics a 
                               WHERE IsActive = 1";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MedicalRubric medicalRubric = new MedicalRubric(currentUser);
                    medicalRubric.MedicalRubricID = DBCommon.GetInt(dr["MedRubricID"]);
                    medicalRubric.MedicalRubricTitle = dr["MedRubricTitle"].ToString();

                    if (dr["IsActive"] is bool)
                        medicalRubric.MedicalRubricIsActive = (bool)dr["IsActive"];
                    else
                        medicalRubric.MedicalRubricIsActive = false;

                    medicalRubrics.Add(medicalRubric);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return medicalRubrics;
        }
    }
}
