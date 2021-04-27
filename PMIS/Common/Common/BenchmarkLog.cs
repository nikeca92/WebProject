using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace PMIS.Common
{
    public static class BenchmarkLog
    {
        public static void Write(string str, User currentUser, HttpRequest request)
        {
            if(Config.GetWebSetting("BenchmarkLog").ToLower() != "true")
                return;

            StreamWriter sw = new StreamWriter(Config.GetWebSetting("BenchmarkLogFile"), true, Encoding.UTF8);
            sw.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}", CommonFunctions.FormatDateTime(DateTime.Now), currentUser.Username, request.UserHostAddress, str));
            sw.Close();
        }

        public static DateTime WriteStart(string str, User currentUser, HttpRequest request)
        {
            DateTime startTime = DateTime.Now;
            Write(str, currentUser, request);
            return startTime;
        }

        public static void WriteEnd(string str, User currentUser, HttpRequest request, DateTime startTime)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan timeProcessed = endTime - startTime;

            str += String.Format(" (Време: {0} сек.)", timeProcessed.TotalSeconds.ToString("0.000000"));

            Write(str, currentUser, request);
        }
    }
}
