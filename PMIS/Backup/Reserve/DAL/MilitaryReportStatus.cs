using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Military Report Status
    public class MilitaryReportStatus : BaseDbObject
    {
        private int militaryReportStatusId;
        private string militaryReportStatusKey;
        private string militaryReportStatusName;

        public int MilitaryReportStatusId
        {
            get
            {
                return militaryReportStatusId;
            }
            set
            {
                militaryReportStatusId = value;
            }
        }

        public string MilitaryReportStatusKey
        {
            get
            {
                return militaryReportStatusKey;
            }
            set
            {
                militaryReportStatusKey = value;
            }
        }

        public string MilitaryReportStatusName
        {
            get
            {
                return militaryReportStatusName;
            }
            set
            {
                militaryReportStatusName = value;
            }
        }



        public MilitaryReportStatus(User user)
            : base(user)
        {

        }
    }

    public static class MilitaryReportStatusUtil
    {
        //This method creates and returns a MilitaryReportStatus object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static MilitaryReportStatus ExtractMilitaryReportStatus(OracleDataReader dr, User currentUser)
        {
            MilitaryReportStatus militaryReportStatus = new MilitaryReportStatus(currentUser);

            militaryReportStatus.MilitaryReportStatusId = DBCommon.GetInt(dr["MilitaryReportStatusID"]);
            militaryReportStatus.MilitaryReportStatusKey = dr["MilitaryReportStatusKey"].ToString();
            militaryReportStatus.MilitaryReportStatusName = dr["MilitaryReportStatusName"].ToString();

            return militaryReportStatus;
        }

        //Get a particular object by its ID
        public static MilitaryReportStatus GetMilitaryReportStatus(int militaryReportStatusId, User currentUser)
        {
            MilitaryReportStatus militaryReportStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryReportStatusID, a.MilitaryReportStatusKey, a.MilitaryReportStatusName
                               FROM PMIS_RES.MilitaryReportStatuses a
                               WHERE a.MilitaryReportStatusID = :MilitaryReportStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryReportStatusID", OracleType.Number).Value = militaryReportStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportStatus = ExtractMilitaryReportStatus(dr, currentUser);
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
        public static MilitaryReportStatus GetMilitaryReportStatusByKey(string militaryReportStatusKey, User currentUser)
        {
            MilitaryReportStatus militaryReportStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryReportStatusID, a.MilitaryReportStatusKey, a.MilitaryReportStatusName
                               FROM PMIS_RES.MilitaryReportStatuses a
                               WHERE a.MilitaryReportStatusKey = :MilitaryReportStatusKey";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryReportStatusKey", OracleType.VarChar).Value = militaryReportStatusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportStatus = ExtractMilitaryReportStatus(dr, currentUser);
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
        public static List<MilitaryReportStatus> GetAllMilitaryReportStatuses(User currentUser)
        {
            List<MilitaryReportStatus> militaryReportStatuses = new List<MilitaryReportStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryReportStatusID, a.MilitaryReportStatusKey, a.MilitaryReportStatusName
                               FROM PMIS_RES.MilitaryReportStatuses a
                               ORDER BY a.MilitaryReportStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportStatuses.Add(ExtractMilitaryReportStatus(dr, currentUser));
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