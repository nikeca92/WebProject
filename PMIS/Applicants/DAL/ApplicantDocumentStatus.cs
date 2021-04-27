using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Represents single applicant document status from PMIS_APPL.ApplicantDocumentStatus table
    public class ApplicantDocumentStatus : IDropDownItem
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

        //IDropDownItem Members
        public string Text()
        {
            return statusName;
        }

        public string Value()
        {
            return statusId.ToString();
        }
    }

    public class ApplicantDocumentStatusUtil
    {
        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ApplicantDocumentStatus ExtractApplicantDocumentStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? statusID = null;

            if (DBCommon.IsInt(dr["StatusID"]))
                statusID = DBCommon.GetInt(dr["StatusID"]);

            string statusName = dr["StatusName"].ToString();
            string statusKey = dr["StatusKey"].ToString();

            ApplicantDocumentStatus applicantDocumentStatus = new ApplicantDocumentStatus();

            if (statusID.HasValue)
            {
                applicantDocumentStatus.StatusId = statusID.Value;
                applicantDocumentStatus.StatusName = statusName;
                applicantDocumentStatus.StatusKey = statusKey;
            }

            return applicantDocumentStatus;
        }

        public static ApplicantDocumentStatus GetApplicantDocumentStatus(int statusId, User currentUser)
        {
            ApplicantDocumentStatus applicantDocumentStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantDocumentStatus a
                                WHERE a.StatusId = :StatusId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusId", OracleType.Number).Value = statusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantDocumentStatus = ExtractApplicantDocumentStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantDocumentStatus;
        }

        public static ApplicantDocumentStatus GetApplicantDocumentStatusByKey(string statusKey, User currentUser)
        {
            ApplicantDocumentStatus applicantDocumentStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantDocumentStatus a
                                WHERE a.StatusKey = :StatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusKey", OracleType.NVarChar).Value = statusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantDocumentStatus = ExtractApplicantDocumentStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantDocumentStatus;
        }

        public static List<ApplicantDocumentStatus> GetAllApplicantDocumentStatus(User currentUser)
        {
            List<ApplicantDocumentStatus> applicantDocumentStatuses = new List<ApplicantDocumentStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.StatusId, a.StatusName, a.StatusKey 
                                FROM PMIS_APPL.ApplicantDocumentStatus a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    applicantDocumentStatuses.Add(ExtractApplicantDocumentStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return applicantDocumentStatuses;
        }
    }
}
