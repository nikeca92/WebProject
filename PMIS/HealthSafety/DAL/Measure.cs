using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Measure
    {
        private int measureId;
        private string measureName;
        private decimal treshold;        

        public int MeasureId
        {
            get { return measureId; }
            set { measureId = value; }
        }

        public string MeasureName
        {
            get { return measureName; }
            set { measureName = value; }
        }

        public decimal Treshold
        {
            get { return treshold; }
            set { treshold = value; }
        }
    }

    public static class MeasureUtil
    {
        public static Measure GetMeasure(int measureId, User currentUser)
        {
            Measure measure = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MeasureName, a.Threshold 
                               FROM PMIS_HS.Measures a
                               WHERE a.MeasureID = :MeasureID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MeasureID", OracleType.Number).Value = measureId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    measure = new Measure();
                    measure.MeasureId = measureId;
                    measure.MeasureName = dr["MeasureName"].ToString();
                    measure.Treshold = (dr["Threshold"] is decimal ? (decimal)dr["Threshold"] : 0);                   
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return measure;
        }

        public static List<Measure> GetAllMeasures(User currentUser)
        {
            List<Measure> measures = new List<Measure>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MeasureID as MeasureID
                               FROM PMIS_HS.Measures a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MeasureID"]))
                        measures.Add(MeasureUtil.GetMeasure(DBCommon.GetInt(dr["MeasureID"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return measures;
        }
    }

}