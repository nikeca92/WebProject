using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EquipWithTechRequestsStatus
    {
        private int equipWithTechRequestsStatusId;
        private string statusName;
        private string statusKey;

        public int EquipWithTechRequestsStatusId
        {
            get
            {
                return equipWithTechRequestsStatusId;
            }
            set
            {
                equipWithTechRequestsStatusId = value;
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

    public static class EquipWithTechRequestsStatusUtil
    {
        //This method creates and returns a EquipWithTechRequestsStatus object. It extracts the data from a DataReader.
        public static EquipWithTechRequestsStatus ExtractEquipWithTechRequestsStatusFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? equipWithTechRequestsStatusId = null;

            if (DBCommon.IsInt(dr["EquipWithTechRequestsStatusID"]))
                equipWithTechRequestsStatusId = DBCommon.GetInt(dr["EquipWithTechRequestsStatusID"]);

            string statusName = dr["StatusName"].ToString();
            string statusKey = dr["StatusKey"].ToString();

            EquipWithTechRequestsStatus equipWithTechRequestsStatus = new EquipWithTechRequestsStatus();

            if (equipWithTechRequestsStatusId.HasValue)
            {
                equipWithTechRequestsStatus.EquipWithTechRequestsStatusId = equipWithTechRequestsStatusId.Value;
                equipWithTechRequestsStatus.StatusName = statusName;
                equipWithTechRequestsStatus.StatusKey = statusKey;
            }

            return equipWithTechRequestsStatus;
        }

        //Get a particular object by its ID
        public static EquipWithTechRequestsStatus GetEquipWithTechRequestsStatus(int equipWithTechRequestsStatusId, User currentUser)
        {
            EquipWithTechRequestsStatus equipWithTechRequestsStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EquipWithTechRequestsStatusID, a.StatusName, a.StatusKey
                               FROM PMIS_RES.EquipWithTechRequestsStatuses a                       
                               WHERE a.EquipWithTechRequestsStatusID = :EquipWithTechRequestsStatusID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipWithTechRequestsStatusID", OracleType.Number).Value = equipWithTechRequestsStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipWithTechRequestsStatus = ExtractEquipWithTechRequestsStatusFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipWithTechRequestsStatus;
        }

        //Get a list of all statuses
        public static List<EquipWithTechRequestsStatus> GetAllEquipWithTechRequestsStatuses(User currentUser)
        {
            List<EquipWithTechRequestsStatus> equipWithTechRequestsStatuses = new List<EquipWithTechRequestsStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EquipWithTechRequestsStatusID, a.StatusName, a.StatusKey
                               FROM PMIS_RES.EquipWithTechRequestsStatuses a 
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    equipWithTechRequestsStatuses.Add(ExtractEquipWithTechRequestsStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipWithTechRequestsStatuses;
        }
    }

}