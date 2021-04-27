using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class Education
    {
        private int educationId;        
        private string educationName;        
        private int educationCode;
        
        public int EducationId
        {
            get { return educationId; }
            set { educationId = value; }
        }

        public string EducationName
        {
            get { return educationName; }
            set { educationName = value; }
        }

        public int EducationCode
        {
            get { return educationCode; }
            set { educationCode = value; }
        }
    }

    public class EducationUtil
    {
        private static Education ExtractEducationFromDR(OracleDataReader dr)
        {
            Education education = new Education();

            education.EducationId = DBCommon.GetInt(dr["EducationID"]);
            education.EducationName = dr["EducationName"].ToString();
            education.EducationCode = (DBCommon.IsInt(dr["EducationCode"]) ? DBCommon.GetInt(dr["EducationCode"]) : 0);

            return education;
        }

        public static Education GetEducation(int educationId, User currentUser)
        {
            Education education = null;
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EducationID as EducationID,
                                      a.EducationName as EducationName,
                                      a.EducationCode as EducationCode 
                               FROM PMIS_APPL.Educations a
                               WHERE a.EducationID = :EducationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EducationID", OracleType.Number).Value = educationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    education = ExtractEducationFromDR(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return education;
        }

        public static Education GetEducationByCode(int educationCode, User currentUser)
        {
            Education education = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EducationID as EducationID,
                                      a.EducationName as EducationName,
                                      a.EducationCode as EducationCode 
                               FROM PMIS_APPL.Educations a
                               WHERE a.EducationCode = :EducationCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EducationCode", OracleType.Number).Value = educationCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    education = ExtractEducationFromDR(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return education;
        }

        public static List<Education> GetAllEducations(User currentUser)
        {
            List<Education> lstEducations = new List<Education>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT a.EducationID as EducationID,
                                      a.EducationName as EducationName,
                                      a.EducationCode as EducationCode 
                               FROM PMIS_APPL.Educations a
                               ORDER BY a.EducationCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lstEducations.Add(ExtractEducationFromDR(dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return lstEducations;
        }
      
    }

}