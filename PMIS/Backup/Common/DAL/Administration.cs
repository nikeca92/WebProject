using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    public class Administration :IDropDownItem
    {
        private int administrationId;
        private string administrationName;

        public int AdministrationId
        {
            get
            {
                return administrationId;
            }
            set
            {
                administrationId = value;
            }
        }

        public string AdministrationName
        {
            get
            {
                return administrationName;
            }
            set
            {
                administrationName = value;
            }
        }
       
        public string Text()
        {
            return AdministrationName;
        }

        public string Value()
        {
            return AdministrationId.ToString();
        }
    }

    public static class AdministrationUtil
    {
        //This method creates and returns a Administration object. It extracts the data from a DataReader.
        public static Administration ExtractAdministrationFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? administrationId = null;

            if (DBCommon.IsInt(dr["AdministrationID"]))
                administrationId = DBCommon.GetInt(dr["AdministrationID"]);

            string administrationName = dr["AdministrationName"].ToString();

            Administration administration = new Administration();

            if (administrationId.HasValue)
            {
                administration.AdministrationId = administrationId.Value;
                administration.AdministrationName = administrationName;
            }

            return administration;
        }

        //Get a particular object by its ID
        public static Administration GetAdministration(int administrationId, User currentUser)
        {
            Administration administration = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AdministrationID, a.AdministrationName
                               FROM PMIS_ADM.Administrations a                       
                               WHERE a.AdministrationID = :AdministrationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AdministrationID", OracleType.Number).Value = administrationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    administration = ExtractAdministrationFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return administration;
        }

        //Get a list of all administrations
        public static List<Administration> GetAllAdministrations(User currentUser)
        {
            List<Administration> administrations = new List<Administration>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AdministrationID, a.AdministrationName
                               FROM PMIS_ADM.Administrations a 
                               ORDER BY a.AdministrationName";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    administrations.Add(ExtractAdministrationFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return administrations;
        }

        public static Administration GetMinistryOfDefence(User currentUser)
        {
            Administration administration = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AdministrationID, a.AdministrationName
                               FROM PMIS_ADM.Administrations a                       
                               WHERE NVL(a.IsMinistryOfDefence, 0) = 1";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    administration = ExtractAdministrationFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return administration;
        }
    }

}