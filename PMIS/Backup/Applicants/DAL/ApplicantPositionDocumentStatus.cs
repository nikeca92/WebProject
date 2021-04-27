using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ApplicantPositionDocumentStatus
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
    }

    public class ApplicantPositionDocumentStatusUtil
    {
        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ApplicantPositionDocumentStatus ExtractApplicantPositionDocumentStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? statusID = null;

            if (DBCommon.IsInt(dr["StatusID"]))
                statusID = DBCommon.GetInt(dr["StatusID"]);

            string statusName = dr["StatusName"].ToString();
            string statusKey = dr["StatusKey"].ToString();

            ApplicantPositionDocumentStatus applicantPositionDocumentStatus = new ApplicantPositionDocumentStatus();

            if (statusID.HasValue)
            {
                applicantPositionDocumentStatus.StatusId = statusID.Value;
                applicantPositionDocumentStatus.StatusName = statusName;
                applicantPositionDocumentStatus.StatusKey = statusKey;
            }

            return applicantPositionDocumentStatus;
        }

        public static ApplicantPositionDocumentStatus GetApplicantPositionDocumentStatus(int statusId, User currentUser)
        {
            ApplicantPositionDocumentStatus applicantPositionDocumentStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantPositionDocStatus a
                                WHERE a.StatusId = :StatusId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusId", OracleType.Number).Value = statusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantPositionDocumentStatus = ExtractApplicantPositionDocumentStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositionDocumentStatus;
        }

        public static ApplicantPositionDocumentStatus GetApplicantPositionDocumentStatusByKey(string statusKey, User currentUser)
        {
            ApplicantPositionDocumentStatus applicantPositionDocumentStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantPositionDocStatus a
                                WHERE a.StatusKey = :StatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusKey", OracleType.VarChar).Value = statusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantPositionDocumentStatus = ExtractApplicantPositionDocumentStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositionDocumentStatus;
        }

        public static List<ApplicantPositionDocumentStatus> GetAllApplicantPositionDocumentStatus(User currentUser)
        {
            List<ApplicantPositionDocumentStatus> applicantPositionDocumentStatuses = new List<ApplicantPositionDocumentStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.StatusId, a.StatusName, a.StatusKey 
                                FROM PMIS_APPL.ApplicantPositionDocStatus a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    applicantPositionDocumentStatuses.Add(ExtractApplicantPositionDocumentStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return applicantPositionDocumentStatuses;
        }
    }
}
