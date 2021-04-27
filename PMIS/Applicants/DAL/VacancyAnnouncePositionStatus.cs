using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class VacancyAnnouncePosStatus
    {
        private int vacancyAnnouncePosStatusID;
        private string vacAnnPosStatusName;
        private string vacAnnPosStatusKey;

        public int VacancyAnnouncePosStatusID
        {
            get
            {
                return vacancyAnnouncePosStatusID;
            }
            set
            {
                vacancyAnnouncePosStatusID = value;
            }
        }

        public string VacAnnPosStatusName
        {
            get
            {
                return vacAnnPosStatusName;
            }
            set
            {
                vacAnnPosStatusName = value;
            }
        }

        public string VacAnnPosStatusKey
        {
            get
            {
                return vacAnnPosStatusKey;
            }
            set
            {
                vacAnnPosStatusKey = value;
            }
        }
    }

    public static class VacancyAnnouncePosStatusUtil
    {
        //This method creates and returns a VacancyAnnounceStatus object. It extracts the data from a DataReader.
        public static VacancyAnnouncePosStatus ExtractVacancyAnnouncePosStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            VacancyAnnouncePosStatus vacancyAnnouncePosStatus = new VacancyAnnouncePosStatus();

            vacancyAnnouncePosStatus.VacancyAnnouncePosStatusID = DBCommon.GetInt(dr["VacancyAnnouncePosStatusID"]);
            vacancyAnnouncePosStatus.VacAnnPosStatusName = dr["VacAnnPosStatusName"].ToString();
            vacancyAnnouncePosStatus.VacAnnPosStatusKey = dr["VacAnnPosStatusKey"].ToString();

            return vacancyAnnouncePosStatus;
        }

        public static VacancyAnnouncePosStatus GetVacancyAnnouncePosStatus(int vacancyAnnouncePosStatusId, User currentUser)
        {
            VacancyAnnouncePosStatus vacancyAnnouncePosStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePosStatusID, a.VacAnnPosStatusName, a.VacAnnPosStatusKey
                               FROM PMIS_APPL.VacancyAnnouncePosStatuses a                       
                               WHERE a.VacancyAnnouncePosStatusID = :VacancyAnnouncePosStatusID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncePosStatusID", OracleType.Number).Value = vacancyAnnouncePosStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnouncePosStatus = ExtractVacancyAnnouncePosStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePosStatus;
        }

        public static VacancyAnnouncePosStatus GetVacancyAnnouncePosStatusByKey(string vacAnnPosStatusKey, User currentUser)
        {
            VacancyAnnouncePosStatus vacancyAnnouncePosStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePosStatusID, a.VacAnnPosStatusName, a.VacAnnPosStatusKey
                               FROM PMIS_APPL.VacancyAnnouncePosStatuses a                       
                               WHERE a.VacAnnPosStatusKey = :VacAnnPosStatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacAnnPosStatusKey", OracleType.VarChar).Value = vacAnnPosStatusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnouncePosStatus = ExtractVacancyAnnouncePosStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePosStatus;
        }

        public static List<VacancyAnnouncePosStatus> GetAllVacancyAnnouncePosStatuses(User currentUser)
        {
            List<VacancyAnnouncePosStatus> vacancyAnnouncePosStatuses = new List<VacancyAnnouncePosStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePosStatusID, a.VacAnnPosStatusName, a.VacAnnPosStatusKey
                               FROM PMIS_APPL.VacancyAnnouncePosStatuses a ";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacancyAnnouncePosStatuses.Add(ExtractVacancyAnnouncePosStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePosStatuses;
        }
    }

}