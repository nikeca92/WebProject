using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Military Readiness
    public class MilitaryReadiness : BaseDbObject, IDropDownItem
    {
        private int milReadinessId;
        private string milReadinessName;

        public int MilReadinessId
        {
            get
            {
                return milReadinessId;
            }
            set
            {
                milReadinessId = value;
            }
        }

        public string MilReadinessName
        {
            get
            {
                return milReadinessName;
            }
            set
            {
                milReadinessName = value;
            }
        }


        public MilitaryReadiness(User user)
            : base(user)
        {

        }

        //IDropDownItem
        public string Value()
        {
            return milReadinessId.ToString();
        }

        public string Text()
        {
            return milReadinessName;
        }
    }

    public static class MilitaryReadinessUtil
    {
        //This method creates and returns a MilitaryReportStatus object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static MilitaryReadiness ExtractMilitaryReadiness(OracleDataReader dr, User currentUser)
        {
            MilitaryReadiness militaryReadiness = new MilitaryReadiness(currentUser);

            militaryReadiness.MilReadinessId = DBCommon.GetInt(dr["MilReadinessID"]);
            militaryReadiness.MilReadinessName = dr["MilReadinessName"].ToString();

            return militaryReadiness;
        }

        //Get a particular object by its ID
        public static MilitaryReadiness GetMilitaryReadiness(int milReadinessId, User currentUser)
        {
            MilitaryReadiness militaryReadiness = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReadinessID, a.MilReadinessName
                               FROM PMIS_RES.MilReadiness a
                               WHERE a.MilReadinessID = :MilReadinessID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilReadinessID", OracleType.Number).Value = milReadinessId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReadiness = ExtractMilitaryReadiness(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReadiness;
        }

        //Get a list of all readiness records
        public static List<MilitaryReadiness> GetAllMilitaryReadiness(User currentUser)
        {
            List<MilitaryReadiness> listMilitaryReadiness = new List<MilitaryReadiness>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReadinessID, a.MilReadinessName
                               FROM PMIS_RES.MilReadiness a
                               ORDER BY a.MilReadinessName";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listMilitaryReadiness.Add(ExtractMilitaryReadiness(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryReadiness;
        }
    }
}