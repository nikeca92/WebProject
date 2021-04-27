using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class VacancyAnnounceStatus
    {
        private int vacancyAnnouncesStatusId;
        private string vacAnnStatusName;
        private string vacAnnStatusKey;

        public int VacancyAnnouncesStatusId
        {
            get
            {
                return vacancyAnnouncesStatusId;
            }
            set
            {
                vacancyAnnouncesStatusId = value;
            }
        }

        public string VacAnnStatusName
        {
            get
            {
                return vacAnnStatusName;
            }
            set
            {
                vacAnnStatusName = value;
            }
        }

        public string VacAnnStatusKey
        {
            get
            {
                return vacAnnStatusKey;
            }
            set
            {
                vacAnnStatusKey = value;
            }
        }
    }

    public static class VacancyAnnounceStatusUtil
    {
        //This method creates and returns a VacancyAnnounceStatus object. It extracts the data from a DataReader.
        public static VacancyAnnounceStatus ExtractVacancyAnnounceStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? vacancyAnnouncesStatusID = null;

            if (DBCommon.IsInt(dr["VacancyAnnouncesStatusID"]))
                vacancyAnnouncesStatusID = DBCommon.GetInt(dr["VacancyAnnouncesStatusID"]);

            string vacAnnStatusName = dr["VacAnnStatusName"].ToString();
            string vacAnnStatusKey = dr["VacAnnStatusKey"].ToString();

            VacancyAnnounceStatus vacancyAnnounceStatus = new VacancyAnnounceStatus();

            if (vacancyAnnouncesStatusID.HasValue)
            {
                vacancyAnnounceStatus.VacancyAnnouncesStatusId = vacancyAnnouncesStatusID.Value;
                vacancyAnnounceStatus.VacAnnStatusName = vacAnnStatusName;
                vacancyAnnounceStatus.VacAnnStatusKey = vacAnnStatusKey;
            }

            return vacancyAnnounceStatus;
        }

        public static VacancyAnnounceStatus GetVacancyAnnounceStatus(int vacancyAnnounceStatusId, User currentUser)
        {
            VacancyAnnounceStatus vacancyAnnounceStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncesStatusID, a.VacAnnStatusName, a.VacAnnStatusKey
                               FROM PMIS_APPL.VacancyAnnouncesStatuses a                       
                               WHERE a.VacancyAnnouncesStatusID = :VacancyAnnouncesStatusID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncesStatusID", OracleType.Number).Value = vacancyAnnounceStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnounceStatus = ExtractVacancyAnnounceStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounceStatus;
        }

        public static List<VacancyAnnounceStatus> GetAllVacancyAnnounceStatuses(User currentUser)
        {
            List<VacancyAnnounceStatus> vacancyAnnounceStatuses = new List<VacancyAnnounceStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncesStatusID, a.VacAnnStatusName, a.VacAnnStatusKey
                               FROM PMIS_APPL.VacancyAnnouncesStatuses a         ";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacancyAnnounceStatuses.Add(ExtractVacancyAnnounceStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounceStatuses;
        }
    }

}