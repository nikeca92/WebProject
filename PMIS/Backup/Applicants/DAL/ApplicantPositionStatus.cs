using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ApplicantPositionStatus : IDropDownItem
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

    public class ApplicantPositionStatusUtil
    {
        //This method creates and returns a ApplicantPositionStatus object. It extracts the data from a DataReader.
        public static ApplicantPositionStatus ExtractApplicantPositionStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? statusID = null;

            if (DBCommon.IsInt(dr["StatusID"]))
                statusID = DBCommon.GetInt(dr["StatusID"]);

            string statusName = dr["StatusName"].ToString();
            string statusKey = dr["StatusKey"].ToString();

            ApplicantPositionStatus applicantPositionStatus = new ApplicantPositionStatus();

            if (statusID.HasValue)
            {
                applicantPositionStatus.StatusId = statusID.Value;
                applicantPositionStatus.StatusName = statusName;
                applicantPositionStatus.StatusKey = statusKey;
            }

            return applicantPositionStatus;
        }

        public static ApplicantPositionStatus GetApplicantPositionStatus(int statusId, User currentUser)
        {
            ApplicantPositionStatus applicantPositionStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantPositionStatus a
                                WHERE a.StatusId = :StatusId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusId", OracleType.Number).Value = statusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantPositionStatus = ExtractApplicantPositionStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositionStatus;
        }

        public static ApplicantPositionStatus GetApplicantPositionStatusByKey(string statusKey, User currentUser)
        {
            ApplicantPositionStatus applicantPositionStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT StatusId, StatusName, StatusKey 
                                FROM PMIS_APPL.ApplicantPositionStatus a
                                WHERE a.StatusKey = :StatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("StatusKey", OracleType.VarChar).Value = statusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantPositionStatus = ExtractApplicantPositionStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositionStatus;
        }

        public static List<ApplicantPositionStatus> GetAllApplicantPositionStatus(User currentUser)
        {
            List<ApplicantPositionStatus> applicantPositionStatuses = new List<ApplicantPositionStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.StatusId, a.StatusName, a.StatusKey 
                                FROM PMIS_APPL.ApplicantPositionStatus a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    applicantPositionStatuses.Add(ExtractApplicantPositionStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return applicantPositionStatuses;
        }
    }
}
