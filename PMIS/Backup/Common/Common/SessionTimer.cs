using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace PMIS.Common
{
    public static class SessionTimer
    {
        public static void InitSession()
        {
                //keeps in the session the times of inital postback or the last postback time if the time difference is more than half asp session time
                if (HttpContext.Current.Session["InitialSessionTime"] == null || string.IsNullOrEmpty(HttpContext.Current.Session["InitialSessionTime"].ToString()))
                {
                    HttpContext.Current.Session["InitialSessionTime"] = DateTime.Now.Month + ":" + DateTime.Now.Day + ":" + DateTime.Now.Year + ":" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                }

                  else
                {
                    string[] dateArray = HttpContext.Current.Session["InitialSessionTime"].ToString().Split(':');
                    DateTime dateInitialPostBack = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[0]), int.Parse(dateArray[1]), int.Parse(dateArray[3]), int.Parse(dateArray[4]), int.Parse(dateArray[5]), int.Parse(dateArray[6]));
                    TimeSpan tsDiff = DateTime.Now - dateInitialPostBack;
                    int seconds = tsDiff.Minutes * 60 + tsDiff.Seconds;
                    double halfSessionTime = (double)(HttpContext.Current.Session.Timeout - 1) / 2;
                    if (seconds >= halfSessionTime * 60)
                        HttpContext.Current.Session["InitialSessionTime"] = DateTime.Now.Month + ":" + DateTime.Now.Day + ":" + DateTime.Now.Year + ":" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                }
        }
    }
}
