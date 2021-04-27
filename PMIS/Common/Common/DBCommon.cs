using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace PMIS.Common
{
    //Here are some basic functions that help when working with the DB
    public static class DBCommon
    {
        //This is used to normalize the new lines to be able to execute a PL/SQL commant from .NET
        public static string FixNewLines(string sql)
        {
            return sql.Replace(Environment.NewLine, " ");
        }

        //Prepare a DateTime object to be stored in the DB (only the Date)
        public static string DateToDBCode(DateTime date)
        {
            string sql = "TO_DATE('" + date.ToString("yyyyMMdd") + "', 'yyyymmdd')";
            return sql;
        }

        //Prepare a DateTime object to be stored in the DB (both Date and Time)
        public static string DateTimeToDBCode(DateTime date)
        {
            string sql = "TO_DATE('" + date.ToString("yyyyMMddHHmmss") + "', 'yyyymmddhh24miss')";
            return sql;
        }

        //Check if a particular object (usualy a value from DataReader) is actually an Integer
        public static bool IsInt(object field)
        {
            bool result;

            int fake;
            result = int.TryParse(field.ToString(), out fake);

            return result;
        }

        //Cast a particular object (usually a value from DataReader) to an Interger
        public static int GetInt(object field)
        {
            if (field.ToString().Trim() == "")
                return -1;

            int result = int.Parse(field.ToString());
            return result;
        }

        //Check if a particular object (usualy a value from DataReader) is actually an Decimal
        public static bool IsDecimal(object field)
        {
            bool result;

            decimal fake;
            result = decimal.TryParse(field.ToString(), out fake);

            return result;
        }

        //Cast a particular object (usually a value from DataReader) to an Decimal
        public static decimal GetDecimal(object field)
        {
            if (field.ToString().Trim() == "")
                return 0;

            decimal result = decimal.Parse(field.ToString());
            return result;
        }

        //A global string that could be used for the NULLS FIRST setting in Oracle
        public static string FixNullsOrder(string orderByDir)
        {
            return orderByDir == "DESC" ? " NULLS LAST " : " NULLS FIRST ";
        }
    }
}
