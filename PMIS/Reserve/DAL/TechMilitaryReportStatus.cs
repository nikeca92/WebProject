using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Tech Military Report Status
    public class TechMilitaryReportStatus : BaseDbObject
    {
        private int techMilitaryReportStatusId;
        private string techMilitaryReportStatusKey;
        private string techMilitaryReportStatusName;

        public int TechMilitaryReportStatusId
        {
            get
            {
                return techMilitaryReportStatusId;
            }
            set
            {
                techMilitaryReportStatusId = value;
            }
        }

        public string TechMilitaryReportStatusKey
        {
            get
            {
                return techMilitaryReportStatusKey;
            }
            set
            {
                techMilitaryReportStatusKey = value;
            }
        }

        public string TechMilitaryReportStatusName
        {
            get
            {
                return techMilitaryReportStatusName;
            }
            set
            {
                techMilitaryReportStatusName = value;
            }
        }



        public TechMilitaryReportStatus(User user)
            : base(user)
        {

        }
    }

    public static class TechMilitaryReportStatusUtil
    {
        //This method creates and returns a MilitaryReportStatus object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static TechMilitaryReportStatus ExtractTechMilitaryReportStatus(OracleDataReader dr, User currentUser)
        {
            TechMilitaryReportStatus militaryReportStatus = new TechMilitaryReportStatus(currentUser);

            militaryReportStatus.TechMilitaryReportStatusId = DBCommon.GetInt(dr["TechMilitaryReportStatusID"]);
            militaryReportStatus.TechMilitaryReportStatusKey = dr["TechMilitaryReportStatusKey"].ToString();
            militaryReportStatus.TechMilitaryReportStatusName = dr["TechMilitaryReportStatusName"].ToString();

            return militaryReportStatus;
        }

        //Get a particular object by its ID
        public static TechMilitaryReportStatus GetTechMilitaryReportStatus(int techMilitaryReportStatusId, User currentUser)
        {
            TechMilitaryReportStatus militaryReportStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechMilitaryReportStatusID, a.TechMilitaryReportStatusKey, a.TechMilitaryReportStatusName
                               FROM PMIS_RES.TechMilitaryReportStatuses a
                               WHERE a.TechMilitaryReportStatusID = :TechMilitaryReportStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechMilitaryReportStatusID", OracleType.Number).Value = techMilitaryReportStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportStatus = ExtractTechMilitaryReportStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportStatus;
        }

        //Get a particular object by its key
        public static TechMilitaryReportStatus GetTechMilitaryReportStatusByKey(string techMilitaryReportStatusKey, User currentUser)
        {
            TechMilitaryReportStatus militaryReportStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechMilitaryReportStatusID, a.TechMilitaryReportStatusKey, a.TechMilitaryReportStatusName
                               FROM PMIS_RES.TechMilitaryReportStatuses a
                               WHERE a.TechMilitaryReportStatusKey = :TechMilitaryReportStatusKey";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechMilitaryReportStatusKey", OracleType.VarChar).Value = techMilitaryReportStatusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportStatus = ExtractTechMilitaryReportStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportStatus;
        }

        //Get a list of all statuses
        public static List<TechMilitaryReportStatus> GetAllTechMilitaryReportStatuses(User currentUser)
        {
            List<TechMilitaryReportStatus> militaryReportStatuses = new List<TechMilitaryReportStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechMilitaryReportStatusID, a.TechMilitaryReportStatusKey, a.TechMilitaryReportStatusName
                               FROM PMIS_RES.TechMilitaryReportStatuses a
                               ORDER BY a.TechMilitaryReportStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportStatuses.Add(ExtractTechMilitaryReportStatus(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportStatuses;
        }

        public static string GetLabelWhenLackOfStatus()
        {
            return "Не се води на отчет";
        }

        
    }
}