using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single discharge destination
    public class DischargeDestination : IDropDownItem
    {
        public string DischargeDestinationCode { get; set; }
        public string DischargeDestinationName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return DischargeDestinationName;
        }

        public string Value()
        {
            return DischargeDestinationCode;
        }

    }

    public static class DischargeDestinationUtil
    {
        public static DischargeDestination GetDischargeDestination(string DischargeDestinationCode, User currentUser)
        {
            DischargeDestination dischargeDestination = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PSO_KOD as DischargeDestinationCode, a.PSO_IME as DischargeDestinationName
                               FROM VS_OWNER.KLV_PSO a
                               WHERE a.PSO_KOD = :DischargeDestinationCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DischargeDestinationCode", OracleType.VarChar).Value = DischargeDestinationCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    dischargeDestination = new DischargeDestination();
                    dischargeDestination.DischargeDestinationCode = dr["DischargeDestinationCode"].ToString();
                    dischargeDestination.DischargeDestinationName = dr["DischargeDestinationName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return dischargeDestination;
        }

        public static List<DischargeDestination> GetAllDischargeDestinations(User currentUser)
        {
            List<DischargeDestination> listDischargeDestination = new List<DischargeDestination>();
            DischargeDestination dischargeDestination;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PSO_KOD as DischargeDestinationCode, a.PSO_IME as DischargeDestinationName
                               FROM VS_OWNER.KLV_PSO a
                               ORDER BY a.PSO_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    dischargeDestination = new DischargeDestination();
                    dischargeDestination.DischargeDestinationCode = dr["DischargeDestinationCode"].ToString();
                    dischargeDestination.DischargeDestinationName = dr["DischargeDestinationName"].ToString();

                    listDischargeDestination.Add(dischargeDestination);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDischargeDestination;
        }
    }
}
