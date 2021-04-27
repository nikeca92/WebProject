using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryForceSort
    {
        private int militaryForceSortId;
        private string militaryForceSortName;
        private bool active;

        public int MilitaryForceSortId
        {
            get { return militaryForceSortId; }
            set { militaryForceSortId = value; }
        }

        public string MilitaryForceSortName
        {
            get { return militaryForceSortName; }
            set { militaryForceSortName = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
    }

    public static class MilitaryForceSortUtil
    {
        public static MilitaryForceSort GetMilitaryForceSort(int militaryForceSortId, User currentUser)
        {
            MilitaryForceSort militaryForceSort = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ROD_IME as MilitaryForceSortName, a.Active
                               FROM VS_OWNER.KLV_ROD a
                               WHERE a.RODID = :MilitaryForceSortID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryForceSortID", OracleType.Number).Value = militaryForceSortId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryForceSort = new MilitaryForceSort();
                    militaryForceSort.MilitaryForceSortId = militaryForceSortId;
                    militaryForceSort.MilitaryForceSortName = dr["MilitaryForceSortName"].ToString();
                    militaryForceSort.Active = DBCommon.GetInt(dr["Active"]) == 1;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryForceSort;
        }

        public static List<MilitaryForceSort> GetAllMilitaryForceSorts(User currentUser)
        {
            List<MilitaryForceSort> militaryForceSorts = new List<MilitaryForceSort>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.RODID as MilitaryForceSortID,
                                      a.ROD_IME as MilitaryForceSortName,
                                      a.Active
                               FROM VS_OWNER.KLV_ROD a
                               ORDER BY a.ROD_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryForceSort militaryForceSort = new MilitaryForceSort();
                    militaryForceSort.MilitaryForceSortId = DBCommon.GetInt(dr["MilitaryForceSortID"]);
                    militaryForceSort.MilitaryForceSortName = dr["MilitaryForceSortName"].ToString();
                    militaryForceSort.Active = DBCommon.GetInt(dr["Active"]) == 1;

                    militaryForceSorts.Add(militaryForceSort);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryForceSorts;
        }
    }
}
