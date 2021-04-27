using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Services.Protocols;

namespace PMIS.ExceptionHandler
{
    //This is the basic class that handles any exceptions
    public class Handler
    {
        //Initialize some private members that control the work of the entire class
        //Some of the settings are store in the web.config file
        private bool _blnLogToFile = Config.GetBoolean("LogToFile", true);
        private bool _blnLogToUI = Config.GetBoolean("LogToUI", true);
        private string _strLogFilePath = Config.GetPath("PathLogFile");
        private NameValueCollection _ResultCollection = new NameValueCollection();
        private string _strException = "";
        private string _strExceptionType = "";
        private string _strViewstate = "";
        private const string _strViewstateKey = "__VIEWSTATE";
        private const string _strRootException = "System.Web.HttpUnhandledException";
        private const string _strRootWsException = "System.Web.Services.Protocols.SoapException";
        private const string _strDefaultLogName = "PMISExceptionLog.txt";

        //Format a particular Stack Frame to string
        private string StackFrameToString(StackFrame sf)
        {
            StringBuilder sb = new StringBuilder();
            int intParam;
            MemberInfo mi = sf.GetMethod();
            sb.Append(" ");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);
            sb.Append("(");
            intParam = 0;

            foreach (ParameterInfo param in sf.GetMethod().GetParameters())
            {
                intParam += 1;

                if (intParam > 1)
                {
                    sb.Append(", ");
                }

                sb.Append(param.Name);
                sb.Append(" As ");
                sb.Append(param.ParameterType.Name);
            }

            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append(" ");

            if (sf.GetFileName() == null || sf.GetFileName().Length == 0)
            {
                sb.Append("(unknown file)");
                sb.Append(": N ");
                sb.Append(string.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(string.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(string.Format("{0:#00}", sf.GetFileColumnNumber()));
                if (sf.GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(string.Format("{0:#0000}", sf.GetILOffset()));
                }
            }

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        //Extract some details from a StackTrace
        private string EnhancedStackTrace(StackTrace st, string strSkipClassName)
        {
            int intFrame = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);

            for (intFrame = 0; intFrame <= st.FrameCount - 1; intFrame++)
            {
                StackFrame sf = st.GetFrame(intFrame);
                MemberInfo mi = sf.GetMethod();

                if (strSkipClassName != "" && mi.DeclaringType.Name.IndexOf(strSkipClassName) > -1)
                {
                }
                else
                {
                    sb.Append(StackFrameToString(sf));
                }
            }

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        private string EnhancedStackTrace(Exception ex)
        {
            return EnhancedStackTrace(new StackTrace(ex, true), "");
        }

        private string EnhancedStackTrace()
        {
            return EnhancedStackTrace(new StackTrace(true), "ASPUnhandledException");
        }

        //Handle web service exceptions
        public string HandleWebServiceException(System.Web.Services.Protocols.SoapMessage sm)
        {
            _blnLogToUI = false;
            HandleException(sm.Exception);
            XmlDocument doc = new XmlDocument();
            XmlNode DetailNode = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);
            XmlNode TypeNode = doc.CreateNode(XmlNodeType.Element, "ExceptionType", SoapException.DetailElementName.Namespace);
            TypeNode.InnerText = _strExceptionType;
            DetailNode.AppendChild(TypeNode);
            XmlNode MessageNode = doc.CreateNode(XmlNodeType.Element, "ExceptionMessage", SoapException.DetailElementName.Namespace);
            MessageNode.InnerText = sm.Exception.Message;
            DetailNode.AppendChild(MessageNode);
            XmlNode InfoNode = doc.CreateNode(XmlNodeType.Element, "ExceptionInfo", SoapException.DetailElementName.Namespace);
            InfoNode.InnerText = _strException;
            DetailNode.AppendChild(InfoNode);

            return DetailNode.OuterXml.ToString();
        }

        //Handle a particular exception
        public void HandleException(Exception ex)
        {
            //Ignore exceptions from the Error.aspx page because they cause an infinite loop
            if (HttpContext.Current.Request.Url != null &&
                HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith("/error.aspx"))
                return;

            try
            {
                //Get the exceptino as a string
                _strException = ExceptionToString(ex);
                _strExceptionType = ex.GetType().FullName;

                if (_strExceptionType == _strRootException | _strExceptionType == _strRootWsException)
                {
                    if (!(ex.InnerException == null))
                    {
                        _strExceptionType = ex.InnerException.GetType().FullName;
                    }
                }
            }
            catch (Exception e)
            {
                _strException = "Error '" + e.Message + "' while generating exception string";
            }
            
            try
            {
                //Check if the exception should be logged into a file
                if (_blnLogToFile)
                {
                    ExceptionToFile();
                }
            }
            catch
            {
            }

            //Check if the exceptino should be logged to the screen
            if (_blnLogToUI)
            {
                ExceptionToPage();
            }

        }


        public static void LogException(Exception exc)
        {
            string err = "Exception occurred in Application\n\n" +
                "\nError in: " + HttpContext.Current.Request.Url.ToString() +
                "\nError Occurred: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                "\nError Message:" + exc.Message.ToString() +
                "\nStack Trace:" + ((exc.StackTrace != null) ? exc.StackTrace : string.Empty);

            EventLog.WriteEntry("Riverside", err, EventLogEntryType.Error);
        }

        // Static method LogException used to log any exception raised from notification component to event log.
        public static void LogException(string errorPlace, Exception exception)
        {
            string err = "Exception occurred in Application\n\n" +
                "\nError in: " + errorPlace +
                "\nError Occurred: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                "\nError Message:" + exception.Message.ToString() +
                "\nStack Trace:" + ((exception.StackTrace != null) ? exception.StackTrace : string.Empty);
            EventLog.WriteEntry("Riverside", err, EventLogEntryType.Error);
        }
        
        // Static method LogException used to log any exception raised from notification component to event log.
        public static void LogException(string strErrorMessage, EventLogEntryType ExceptionEventLogEntryType)
        {
            string err = "Exception occurred in Application\n\n" +
                "\nError Occurred: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                "\nError Message:" + strErrorMessage + "\nStack Trace:";
            EventLog.WriteEntry("Riverside", err, ExceptionEventLogEntryType);
        }

        private void ExceptionToPage()
        {
            //HttpContext.Current.Server.Transfer("Error.aspx",true);
            string strErrorPage = Config.GetString("ErrorPage", "");
            System.Web.UI.Control p = new System.Web.UI.Control();

            HttpContext.Current.Response.Redirect(p.ResolveUrl("~") + strErrorPage);
            //HttpContext.Current.Server.Transfer(p.ResolveUrl("~") + strErrorPage);
        }

        //Log the particular exception message to a file
        internal bool ExceptionToFile(string Message)
        {

            if (Path.GetFileName(_strLogFilePath) == "")
            {
                _strLogFilePath = Path.Combine(_strLogFilePath, _strDefaultLogName);
            }

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(_strLogFilePath, true);
                sw.Write(Message);
                sw.WriteLine();
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                _ResultCollection.Add("LogToFile", ex.Message);
            }
            finally
            {
                if (!(sw == null))
                {
                    sw.Close();
                }
            }
            return false;
        }

        private bool ExceptionToFile()
        {

            if (Path.GetFileName(_strLogFilePath) == "")
            {
                _strLogFilePath = Path.Combine(_strLogFilePath, _strDefaultLogName);
            }

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(_strLogFilePath, true);
                sw.Write(_strException);
                sw.WriteLine();
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                _ResultCollection.Add("LogToFile", ex.Message);
            }
            finally
            {
                if (!(sw == null))
                {
                    sw.Close();
                }
            }
            return false;
        }

        //The following functions return specific system information as string that could be logged with the exception
        private string CurrentWindowsIdentity()
        {
            try
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch
            {
                return "";
            }
        }

        private string CurrentEnvironmentIdentity()
        {
            try
            {
                return System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            }
            catch
            {
                return "";
            }
        }

        private string ProcessIdentity()
        {
            string strTemp = CurrentWindowsIdentity();
            if (strTemp == "")
            {
                return CurrentEnvironmentIdentity();
            }
            return strTemp;
        }

        private string WebCurrentUrl()
        {
            string strUrl;
            strUrl = "http://" + HttpContext.Current.Request.ServerVariables["server_name"];
            if (HttpContext.Current.Request.ServerVariables["server_port"] != "80")
            {
                strUrl += ":" + HttpContext.Current.Request.ServerVariables["server_port"];
            }
            strUrl += HttpContext.Current.Request.ServerVariables["url"];
            if (HttpContext.Current.Request.ServerVariables["query_string"].Length > 0)
            {
                strUrl += "?" + HttpContext.Current.Request.ServerVariables["query_string"];
            }
            return strUrl;
        }

        private string AllAssemblyDetailsToString()
        {
            StringBuilder sb = new StringBuilder();
            NameValueCollection nvc;
            const string strLineFormat = " {0, -30} {1, -15} {2}";
            sb.Append(Environment.NewLine);
            sb.Append(string.Format(strLineFormat, "Assembly", "Version", "BuildDate"));
            sb.Append(Environment.NewLine);
            sb.Append(string.Format(strLineFormat, "--------", "-------", "---------"));
            sb.Append(Environment.NewLine);
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                nvc = AssemblyAttribs(a);
                if (nvc["Version"] != "0.0.0.0")
                {
                    sb.Append(string.Format(strLineFormat, Path.GetFileName(nvc["CodeBase"]), nvc["Version"], nvc["BuildDate"]));
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        private string AssemblyDetailsToString(System.Reflection.Assembly a)
        {
            StringBuilder sb = new StringBuilder();
            NameValueCollection nvc = AssemblyAttribs(a);
            sb.Append("Assembly Codebase: ");
            try
            {
                sb.Append(nvc["CodeBase"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Assembly Full Name: ");
            try
            {
                sb.Append(nvc["FullName"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Assembly Version: ");
            try
            {
                sb.Append(nvc["Version"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Assembly Build Date: ");
            try
            {
                sb.Append(nvc["BuildDate"]);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string AssemblyInfoToString(Exception ex)
        {
            System.Reflection.Assembly a = GetAssemblyFromName(ex.Source);
            if (a == null)
            {
                return AllAssemblyDetailsToString();
            }
            else
            {
                return AssemblyDetailsToString(a);
            }
        }

        private string SysInfoToString(bool blnIncludeStackTrace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Date and Time: ");
            sb.Append(DateTime.Now);
            sb.Append(Environment.NewLine);
            sb.Append("Machine Name: ");
            try
            {
                sb.Append(Environment.MachineName);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Process User: ");
            sb.Append(ProcessIdentity());
            sb.Append(Environment.NewLine);
            sb.Append("Remote User: ");
            sb.Append(HttpContext.Current.Request.ServerVariables["REMOTE_USER"]);
            sb.Append(Environment.NewLine);
            sb.Append("Remote Address: ");
            sb.Append(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            sb.Append(Environment.NewLine);
            sb.Append("Remote Host: ");
            sb.Append(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
            sb.Append(Environment.NewLine);
            sb.Append("URL: ");
            sb.Append(WebCurrentUrl());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("NET Runtime version: ");
            sb.Append(System.Environment.Version.ToString());
            sb.Append(Environment.NewLine);
            sb.Append("Application Domain: ");
            try
            {
                sb.Append(System.AppDomain.CurrentDomain.FriendlyName);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            if (blnIncludeStackTrace)
            {
                sb.Append(EnhancedStackTrace());
            }
            return sb.ToString();
        }

        private string ExceptionToString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ExceptionToStringPrivate(ex, true));
            try
            {
                sb.Append(GetASPSettings());
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string ExceptionToStringPrivate(Exception ex, bool blnIncludeSysInfo)
        {
            StringBuilder sb = new StringBuilder();
            if (!((ex.InnerException == null)))
            {
                if (ex.GetType().ToString() == _strRootException | ex.GetType().ToString() == _strRootWsException)
                {
                    return ExceptionToStringPrivate(ex.InnerException, true);
                }
                else
                {
                    sb.Append(ExceptionToStringPrivate(ex.InnerException, false));
                    sb.Append(Environment.NewLine);
                    sb.Append("(Outer Exception)");
                    sb.Append(Environment.NewLine);
                }
            }
            if (blnIncludeSysInfo)
            {
                sb.Append(SysInfoToString(false));
                sb.Append(AssemblyInfoToString(ex));
                sb.Append(Environment.NewLine);
            }
            sb.Append("Exception Type: ");
            try
            {
                sb.Append(ex.GetType().FullName);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Exception Message: ");
            try
            {
                sb.Append(ex.Message);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Exception Source: ");
            try
            {
                sb.Append(ex.Source);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            sb.Append("Exception Target Site: ");
            try
            {
                sb.Append(ex.TargetSite.Name);
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            try
            {
                sb.Append(EnhancedStackTrace(ex));
            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private DateTime AssemblyLastWriteTime(System.Reflection.Assembly a)
        {
            try
            {
                return File.GetLastWriteTime(a.Location);
            }
            catch
            {
                return DateTime.MaxValue;
            }
        }

        private DateTime AssemblyBuildDate(Assembly a, bool blnForceFileDate)
        {
            System.Version v = a.GetName().Version;
            DateTime dt;
            if (blnForceFileDate)
            {
                dt = AssemblyLastWriteTime(a);
            }
            else
            {

                DateTime dt1 = new DateTime(2006, 1, 1);
                dt = dt1.AddDays(v.Build).AddSeconds(v.Revision * 2);
                if (TimeZone.IsDaylightSavingTime(dt, TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
                {
                    dt = dt.AddHours(1);
                }
                if (dt > DateTime.Now | v.Build < 730 | v.Revision == 0)
                {
                    dt = AssemblyLastWriteTime(a);
                }
            }
            return dt;
        }

        private NameValueCollection AssemblyAttribs(System.Reflection.Assembly a)
        {
            string Name;
            string Value;
            NameValueCollection nvc = new NameValueCollection();
            foreach (object attrib in a.GetCustomAttributes(false))
            {
                Name = attrib.GetType().ToString();
                Value = "";
                if (Name == "System.Diagnostics.DebuggableAttribute")
                {
                    Name = "Debuggable";
                    Value = ((System.Diagnostics.DebuggableAttribute)(attrib)).IsJITTrackingEnabled.ToString();
                }
                else if (Name == "System.CLSCompliantAttribute")
                {
                    Name = "CLSCompliant";
                    Value = ((System.CLSCompliantAttribute)(attrib)).IsCompliant.ToString();
                }
                else if (Name == "System.Runtime.InteropServices.GuidAttribute")
                {
                    Name = "GUID";
                    Value = ((System.Runtime.InteropServices.GuidAttribute)(attrib)).Value.ToString();
                }
                else if (Name == "System.Reflection.AssemblyTrademarkAttribute")
                {
                    Name = "Trademark";
                    Value = ((AssemblyTrademarkAttribute)(attrib)).Trademark.ToString();
                }
                else if (Name == "System.Reflection.AssemblyProductAttribute")
                {
                    Name = "Product";
                    Value = ((AssemblyProductAttribute)(attrib)).Product.ToString();
                }
                else if (Name == "System.Reflection.AssemblyCopyrightAttribute")
                {
                    Name = "Copyright";
                    Value = ((AssemblyCopyrightAttribute)(attrib)).Copyright.ToString();
                }
                else if (Name == "System.Reflection.AssemblyCompanyAttribute")
                {
                    Name = "Company";
                    Value = ((AssemblyCompanyAttribute)(attrib)).Company.ToString();
                }
                else if (Name == "System.Reflection.AssemblyTitleAttribute")
                {
                    Name = "Title";
                    Value = ((AssemblyTitleAttribute)(attrib)).Title.ToString();
                }
                else if (Name == "System.Reflection.AssemblyDescriptionAttribute")
                {
                    Name = "Description";
                    Value = ((AssemblyDescriptionAttribute)(attrib)).Description.ToString();
                }
                else
                {
                }
                if (Value != "")
                {
                    if (nvc[Name] == "")
                    {
                        nvc.Add(Name, Value);
                    }
                }
            }
            nvc.Add("CodeBase", a.CodeBase.Replace("file:///", ""));
            nvc.Add("BuildDate", AssemblyBuildDate(a, false).ToString());
            nvc.Add("Version", a.GetName().Version.ToString());
            nvc.Add("FullName", a.FullName);
            return nvc;
        }

        private Assembly GetAssemblyFromName(string strAssemblyName)
        {
            Assembly ar = null;
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetName().Name == strAssemblyName)
                {
                    ar = a;

                }
            }
            return ar;
        }

        private string GetASPSettings()
        {
            StringBuilder sb = new StringBuilder();
            const string strSuppressKeyPattern = "^ALL_HTTP|^ALL_RAW|VSDEBUGGER";
            sb.Append("---- ASP.NET Collections ----");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            //sb.Append(HttpVarsToString(HttpContext.Current.Request.QueryString[""]) ); 
            //sb.Append(HttpVarsToString( HttpContext.Current.Request.Form[""]. ) ); 
            sb.Append(HttpVarsToString(HttpContext.Current.Request.Cookies));
            sb.Append(HttpVarsToString(HttpContext.Current.Session));
            sb.Append(HttpVarsToString(HttpContext.Current.Cache));
            sb.Append(HttpVarsToString(HttpContext.Current.Application));
            sb.Append(HttpVarsToString(HttpContext.Current.Request.ServerVariables, "ServerVariables", true, strSuppressKeyPattern));
            return sb.ToString();
        }

        private string HttpVarsToString(HttpCookieCollection c)
        {
            if (c.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Cookies");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            foreach (string strKey in c)
            {
                AppendLine(sb, strKey, c[strKey].Value);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string HttpVarsToString(HttpApplicationState a)
        {
            if (a.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Application");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            foreach (string strKey in a)
            {
                AppendLine(sb, strKey, a[strKey]);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string HttpVarsToString(Cache c)
        {
            if (c.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Cache");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            foreach (DictionaryEntry de in c)
            {
                AppendLine(sb, de.Key.ToString(), de.Value);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string HttpVarsToString(HttpSessionState s)
        {
            if (s == null)
            {
                return "";
            }
            if (s.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Session");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            foreach (string strKey in s)
            {
                AppendLine(sb, strKey, s[strKey]);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string HttpVarsToString(NameValueCollection nvc, string strTitle, bool blnSuppressEmpty, string strSuppressKeyPattern)
        {
            if (!(nvc.HasKeys()))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(strTitle);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            bool blnDisplay;
            foreach (string strKey in nvc)
            {
                blnDisplay = true;
                if (blnSuppressEmpty)
                {
                    blnDisplay = nvc[strKey] != "";
                }
                if (strKey == _strViewstateKey)
                {
                    _strViewstate = nvc[strKey];
                    blnDisplay = false;
                }
                if (blnDisplay && strSuppressKeyPattern != "")
                {
                    blnDisplay = !(Regex.IsMatch(strKey, strSuppressKeyPattern));
                }
                if (blnDisplay)
                {
                    AppendLine(sb, strKey, nvc[strKey]);
                }
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private string AppendLine(StringBuilder sb, string Key, object Value)
        {
            string strValue;
            if (Value == null)
            {
                strValue = "(Nothing)";
            }
            else
            {
                try
                {
                    strValue = Value.ToString();
                }
                catch
                {
                    strValue = "(" + Value.GetType().ToString() + ")";
                }
            }
            return AppendLine(sb, Key, strValue);
        }

        private string AppendLine(StringBuilder sb, string Key, string strValue)
        {
            sb.Append(string.Format(" {0, -30}{1}", Key, strValue));
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
