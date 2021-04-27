using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ApplicantExamStatus : IDropDownItem
    {
        private int statusId;
        private string statusName;
        private string statusKey;

        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }

        public string StatusKey
        {
            get { return statusKey; }
            set { statusKey = value; }
        }       

        public string Text()
        {
            return StatusName;
        }

        public string Value()
        {
            return StatusId.ToString();
        }
       
    }

    public class ApplicantExamStatusUtil
    {
        //This method creates and returns a ApplicantExamStatus object. It extracts the data from a DataReader.
        public static ApplicantExamStatus ExtractApplicantExamStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? statusID = null;

            if (DBCommon.IsInt(dr["ApplicantExamStatusID"]))
                statusID = DBCommon.GetInt(dr["ApplicantExamStatusID"]);

            string statusName = dr["ApplicantExamStatusName"].ToString();
            string statusKey = dr["ApplicantExamStatusKey"].ToString();

            ApplicantExamStatus applicantExamStatus = new ApplicantExamStatus();

            if (statusID.HasValue)
            {
                applicantExamStatus.StatusId = statusID.Value;
                applicantExamStatus.StatusName = statusName;
                applicantExamStatus.StatusKey = statusKey;
            }

            return applicantExamStatus;
        }

        public static ApplicantExamStatus GetApplicantExamStatus(int statusId, User currentUser)
        {
            ApplicantExamStatus applicantExamStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.ApplicantExamStatusID, a.ApplicantExamStatusName, a.ApplicantExamStatusKey 
                                FROM PMIS_APPL.ApplicantExamStatuses a
                                WHERE a.ApplicantExamStatusID = :ApplicantExamStatusID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantExamStatusID", OracleType.Number).Value = statusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantExamStatus = ExtractApplicantExamStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantExamStatus;
        }

        public static ApplicantExamStatus GetApplicantExamStatusByKey(string statusKey, User currentUser)
        {
            ApplicantExamStatus applicantExamStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.ApplicantExamStatusID, a.ApplicantExamStatusName, a.ApplicantExamStatusKey 
                                FROM PMIS_APPL.ApplicantExamStatuses a
                                WHERE a.ApplicantExamStatusKey = :ApplicantExamStatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantExamStatusKey", OracleType.VarChar).Value = statusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantExamStatus = ExtractApplicantExamStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantExamStatus;
        }

        public static List<ApplicantExamStatus> GetAllApplicantExamStatuses(User currentUser)
        {
            List<ApplicantExamStatus> applicantExamStatuses = new List<ApplicantExamStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ApplicantExamStatusID, a.ApplicantExamStatusName, a.ApplicantExamStatusKey 
                                FROM PMIS_APPL.ApplicantExamStatuses a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    applicantExamStatuses.Add(ExtractApplicantExamStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return applicantExamStatuses;
        }
    }
}
