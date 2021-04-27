using System.Collections.Generic;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class LearningMethod : IDropDownItem
    {
        private string learningMethodKey;
        private string learningMethodName;

        public string LearningMethodKey
        {
            get
            {
                return learningMethodKey;
            }
            set
            {
                learningMethodKey = value;
            }
        }

        public string LearningMethodName
        {
            get
            {
                return learningMethodName;
            }
            set
            {
                learningMethodName = value;
            }
        }


        //IDropDownItem Members
        public string Text()
        {
            return learningMethodName;
        }

        public string Value()
        {
            return learningMethodKey;
        }
    }

    public static class LearningMethodUtil
    {
        //Get a single LearningMethod object by its key
        public static LearningMethod GetLearningMethod(User currentUser, string learningMethodKey)
        {
            LearningMethod learningMethod = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.LearningMethodKey, a.LearningMethodName
                               FROM PMIS_ADM.LearningMethods a
                               WHERE a.LearningMethodKey = :LearningMethodKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("LearningMethodKey", OracleType.VarChar).Value = learningMethodKey;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    learningMethod = new LearningMethod();
                    learningMethod.LearningMethodKey = dr["LearningMethodKey"].ToString();
                    learningMethod.LearningMethodName = dr["LearningMethodName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return learningMethod;
        }

        //Get a list of all Learning Methods
        public static List<LearningMethod> GetLearningMethods(User currentUser)
        {
            List<LearningMethod> listLearningMethods = new List<LearningMethod>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.LearningMethodKey, a.LearningMethodName
                               FROM PMIS_ADM.LearningMethods a
                               ORDER BY a.LearningMethodName ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    LearningMethod learningMethod = new LearningMethod();
                    learningMethod.LearningMethodKey = dr["LearningMethodKey"].ToString();
                    learningMethod.LearningMethodName = dr["LearningMethodName"].ToString();
                    listLearningMethods.Add(learningMethod);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listLearningMethods;
        }
    }
}
