using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    //This class represents the Modules (e.g. HealthSafety, Applicants, etc.)
    public class Module
    {
        private int moduleId;
        private string moduleName;
        private string moduleKey;

        public int ModuleId
        {
            get
            {
                return moduleId;
            }

            set
            {
                moduleId = value;
            }
        }

        public string ModuleName
        {
            get
            {
                return moduleName;
            }

            set
            {
                moduleName = value;
            }
        }

        public string ModuleKey
        {
            get
            {
                return moduleKey;
            }

            set
            {
                moduleKey = value;
            }
        }

        public Module() { }
    }

    //The ModuleUtil static class has some method that help when working with Module objects
    public static class ModuleUtil
    {
        // Each constant presents the specified module abbreviation
        public static string APPL() { return "APPL"; }
        public static string HS() { return "HS"; }
        public static string ADM() { return "ADM"; }
        public static string RES() { return "RES"; }
        public static string REP() { return "REP"; }

        //This method creates and returns a Module object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ModuleID, for example.
        public static Module ExtractModuleFromDataReader(OracleDataReader dr)
        {
            int? moduleId = null;
            
            if(DBCommon.IsInt(dr["ModuleID"]))
                moduleId = DBCommon.GetInt(dr["ModuleID"]);

            string moduleName = dr["ModuleName"].ToString();
            string moduleKey = dr["ModuleKey"].ToString();

            Module module = new Module();

            if (moduleId.HasValue)
            {
                module.ModuleId = moduleId.Value;
                module.ModuleName = moduleName;
                module.ModuleKey = moduleKey;
            }

            return module;
        }

        //Get a list of all Modules
        public static List<Module> GetModules(User currentUser)
        {
            List<Module> modules = new List<Module>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        ORDER BY a.ModuleName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Module module = ExtractModuleFromDataReader(dr);

                    modules.Add(module);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return modules;
        }

        //Get a specific Module
        public static Module GetModule(User currentUser, int moduleId)
        {
            Module module = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        WHERE a.ModuleID = :ModuleID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ModuleID", OracleType.Number).Value = moduleId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    module = ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return module;
        }

        //Get a specific Module by key
        public static Module GetModule(User currentUser, string moduleKey)
        {
            Module module = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        WHERE a.ModuleKey = :ModuleKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ModuleKey", OracleType.VarChar).Value = moduleKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    module = ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return module;
        }

        public static Module GetModule(string moduleKey)
        {
            Module module = null;

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        WHERE a.ModuleKey = :ModuleKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ModuleKey", OracleType.VarChar).Value = moduleKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    module = ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return module;
        }

        //Get a list of all Modules by Role
        public static Module GetModuleByRoleID(int roleId, User currentUser)
        {
            Module module = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        INNER JOIN PMIS_ADM.Roles b ON a.ModuleID = b.ModuleID
                        WHERE b.RoleID = :RoleID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RoleID", OracleType.Number).Value = roleId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    module = ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return module;
        }

        //Get a list of all Modules
        public static List<Module> GetModulesByIDs(User currentUser, string moduleIDs)
        {
            List<Module> modules = new List<Module>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(moduleIDs))
                    where = @" WHERE a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(moduleIDs) + @") ";

                SQL = @"SELECT a.ModuleID, a.ModuleName, a.ModuleKey
                        FROM PMIS_ADM.Modules a
                        " + where + @"
                        ORDER BY a.ModuleName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Module module = ExtractModuleFromDataReader(dr);

                    modules.Add(module);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return modules;
        }
    }
}