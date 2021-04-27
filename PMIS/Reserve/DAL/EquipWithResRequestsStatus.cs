using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EquipWithResRequestsStatus
    {
        private int equipWithResRequestsStatusId;
        private string statusName;
        private string statusKey;

        public int EquipWithResRequestsStatusId
        {
            get
            {
                return equipWithResRequestsStatusId;
            }
            set
            {
                equipWithResRequestsStatusId = value;
            }
        }

        public string StatusName
        {
            get
            {
                return statusName;
            }
            set
            {
                statusName = value;
            }
        }

        public string StatusKey
        {
            get
            {
                return statusKey;
            }
            set
            {
                statusKey = value;
            }
        }
    }

    public static class EquipWithResRequestsStatusUtil
    {
        //This method creates and returns a EquipWithResRequestsStatus object. It extracts the data from a DataReader.
        public static EquipWithResRequestsStatus ExtractEquipWithResRequestsStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? equipWithResRequestsStatusId = null;

            if (DBCommon.IsInt(dr["EquipWithResRequestsStatusID"]))
                equipWithResRequestsStatusId = DBCommon.GetInt(dr["EquipWithResRequestsStatusID"]);

            string statusName = dr["StatusName"].ToString();
            string statusKey = dr["StatusKey"].ToString();

            EquipWithResRequestsStatus equipWithResRequestsStatus = new EquipWithResRequestsStatus();

            if (equipWithResRequestsStatusId.HasValue)
            {
                equipWithResRequestsStatus.EquipWithResRequestsStatusId = equipWithResRequestsStatusId.Value;
                equipWithResRequestsStatus.StatusName = statusName;
                equipWithResRequestsStatus.StatusKey = statusKey;
            }

            return equipWithResRequestsStatus;
        }

        //Get a particular object by its ID
        public static EquipWithResRequestsStatus GetEquipWithResRequestsStatus(int equipWithResRequestsStatusId, User currentUser)
        {
            EquipWithResRequestsStatus equipWithResRequestsStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EquipWithResRequestsStatusID, a.StatusName, a.StatusKey
                               FROM PMIS_RES.EquipWithResRequestsStatuses a                       
                               WHERE a.EquipWithResRequestsStatusID = :EquipWithResRequestsStatusID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipWithResRequestsStatusID", OracleType.Number).Value = equipWithResRequestsStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipWithResRequestsStatus = ExtractEquipWithResRequestsStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipWithResRequestsStatus;
        }

        //Get a list of all statuses
        public static List<EquipWithResRequestsStatus> GetAllEquipWithResRequestsStatuses(User currentUser)
        {
            List<EquipWithResRequestsStatus> equipWithResRequestsStatuses = new List<EquipWithResRequestsStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EquipWithResRequestsStatusID, a.StatusName, a.StatusKey
                               FROM PMIS_RES.EquipWithResRequestsStatuses a 
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    equipWithResRequestsStatuses.Add(ExtractEquipWithResRequestsStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipWithResRequestsStatuses;
        }
    }

}