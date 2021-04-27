using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Common;
using System.Data.OracleClient;

namespace PMIS.Applicants.Common
{
    public class RegisterNumbers : BaseDbObject
    {
        private int militaryDepartmentId;
        private int year;
        private int lastNumber;

        public int MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public int LastNumber
        {
            get { return lastNumber; }
            set { lastNumber = value; }
        }

        public RegisterNumbers(User user)
            : base(user)
        {
        }
    }

    public static class RegisterNumbersUtil
    {
        public static RegisterNumbers GetRegisterNumbers(int militaryDepartmentId, int year, User currentUser)
        {
            RegisterNumbers registerNumbers = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT MilitaryDepartmentID, Year, LastNumber 
                                FROM PMIS_APPL.RegisterNumbers
                                WHERE MilitaryDepartmentID = :MilitaryDepartmentID AND Year = :Year";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;
                
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    registerNumbers = new RegisterNumbers(currentUser);

                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                        registerNumbers.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);

                    if (DBCommon.IsInt(dr["Year"]))
                        registerNumbers.Year = DBCommon.GetInt(dr["Year"]);

                    if (DBCommon.IsInt(dr["LastNumber"]))
                        registerNumbers.LastNumber = DBCommon.GetInt(dr["LastNumber"]);

                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return registerNumbers;
        }

        public static bool AddNewRegisterNumbers(int militaryDepartmentId, int year, User currentUser)
        {
            int startNumber = 1;
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"INSERT INTO PMIS_APPL.RegisterNumbers
                                (MilitaryDepartmentID, Year, LastNumber)
                                VALUES (:MilitaryDepartmentID, :Year, :StartNumber)";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;
                cmd.Parameters.Add("StartNumber", OracleType.Number).Value = startNumber;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool UpdateRegisterNumbers(int militaryDepartmentId, int year, int newNumber, User currentUser)
        {
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_APPL.RegisterNumbers
                                SET LastNumber = :NewNumber
                                WHERE MilitaryDepartmentID = :MilitaryDepartmentID AND Year = :Year";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;
                cmd.Parameters.Add("NewNumber", OracleType.Number).Value = newNumber;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
