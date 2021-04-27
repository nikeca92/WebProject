using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web;
using System.Web.SessionState;

namespace PMIS.Common
{
    public class LoginLog : BaseDbObject
    {
        private int loginLogID;        
        private int moduleID;        
        private Module module;        
        private int userID;        
        private User user;        
        private string ip;       
        private string userAgent;        
        private string sessionID;        
        private string authID;
        private DateTime loginDateTime;        

        public int LoginLogID
        {
            get { return loginLogID; }
            set { loginLogID = value; }
        }

        public int ModuleID
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public Module Module
        {
            get 
            {
                if (module == null)
                    module = ModuleUtil.GetModule(CurrentUser, moduleID);

                return module; 
            }
            set { module = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public User User
        {
            get 
            {
                if (user == null)
                    user = UserUtil.GetUser(CurrentUser, userID);

                return user; 
            }
            set { user = value; }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        public string AuthID
        {
            get { return authID; }
            set { authID = value; }
        }

        public DateTime LoginDateTime
        {
            get { return loginDateTime; }
            set { loginDateTime = value; }
        }

        public LoginLog(User user)
            : base(user)
        {

        }
    }

    public class LoginLogFilter
    {
        string users;
        string modules;
        DateTime? dateFrom;
        DateTime? dateTo;
        int orderBy;
        int pageIdx;

        public string Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
            }
        }

        public string Modules
        {
            get
            {
                return modules;
            }
            set
            {
                modules = value;
            }
        }

        public DateTime? DateFrom
        {
            get
            {
                return dateFrom;
            }
            set
            {
                dateFrom = value;
            }
        }

        public DateTime? DateTo
        {
            get
            {
                return dateTo;
            }
            set
            {
                dateTo = value;
            }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public class LoginLogUtil
    {        
        private static LoginLog ExtractLoginLogFromDR(OracleDataReader dr, User currentUser)
        {
            LoginLog loginLog = new LoginLog(currentUser);

            loginLog.LoginLogID = DBCommon.GetInt(dr["LoginLogID"]);
            loginLog.ModuleID = DBCommon.GetInt(dr["ModuleID"]);
            loginLog.UserID = DBCommon.GetInt(dr["UserID"]);
            loginLog.IP = dr["IP"].ToString();
            loginLog.UserAgent = dr["UserAgent"].ToString();
            loginLog.SessionID = dr["SessionID"].ToString();
            loginLog.AuthID = dr["AuthID"].ToString();
            loginLog.LoginDateTime = (DateTime)dr["LoginDateTime"];

            return loginLog;
        }

        public static LoginLog GetLoginLog(int loginLogID, User currentUser)
        {
            LoginLog loginLog = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.LoginLogID, a.ModuleID, a.UserID, a.IP, 
                                      a.UserAgent, a.SessionID, a.AuthID, a.LoginDateTime
                               FROM PMIS_ADM.LoginLog a
                               WHERE a.LoginLogID = :LoginLogID ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LoginLogID", OracleType.Number).Value = loginLogID;                

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    loginLog = ExtractLoginLogFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return loginLog;
        }

        public static List<LoginLog> GetAllLoginLogs(LoginLogFilter filter, int rowsPerPage, User currentUser)
        {
            List<LoginLog> loginLogs = new List<LoginLog>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Users))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.UserID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Users) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }           

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginDateTime >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginDateTime < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
                }              

                //Paging (load the rows only for the target page)
                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                where = (where == "" ? "" : " WHERE ") + where;

                //Construct the ORDER BY clause according to the order column
                string orderBySQL = "";
                string orderByDir = "ASC";

                int orderBy = filter.OrderBy;

                //The DESCending order is specified by using column number + 100 (e.g. 101, 102, etc.)
                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                //Get the specific order by expression
                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "u.Username";
                        break;
                    case 2:
                        orderBySQL = "(d.FirstName || ' ' || d.MiddleName || ' ' || d.LastName)";
                        break;
                    case 3:
                        orderBySQL = "m.ModuleName";
                        break;
                    case 4:
                        orderBySQL = "a.IP";
                        break;
                    case 5:
                        orderBySQL = "a.LoginDateTime";
                        break;
                    default:
                        orderBySQL = "m.ModuleName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.LoginLogID ASC" + DBCommon.FixNullsOrder("ASC");

                string SQL = @"SELECT * 
                               FROM (
                                      SELECT a.LoginLogID, a.ModuleID, a.UserID, a.IP, 
                                             a.UserAgent, a.SessionID, a.AuthID, a.LoginDateTime,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.LoginLogID) as RowNumber
                                      FROM PMIS_ADM.LoginLog a     
                                      INNER JOIN PMIS_ADM.Users u ON a.UserID = u.UserID   
                                      INNER JOIN PMIS_ADM.UserDetails d ON u.UserID = d.UserID
                                      INNER JOIN PMIS_ADM.Modules m ON a.ModuleID = m.ModuleID                       
                                      " + where + @"
                                      ORDER BY " + orderBySQL + @", a.LoginLogID
                                     ) tmp
                               " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);
               
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    loginLogs.Add(ExtractLoginLogFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return loginLogs;
        }

        public static int GetAllLoginLogsCnt(LoginLogFilter filter, User currentUser)
        {
            int loginLogs = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Users))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.UserID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Users) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginDateTime >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginDateTime < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
                }
               
                where = (where == "" ? "" : " WHERE ") + where;
                            
                string SQL = @" SELECT COUNT(*) as Cnt                                            
                                      FROM PMIS_ADM.LoginLog a     
                                      INNER JOIN PMIS_ADM.Users u ON a.UserID = u.UserID   
                                      INNER JOIN PMIS_ADM.UserDetails d ON u.UserID = d.UserID
                                      INNER JOIN PMIS_ADM.Modules m ON a.ModuleID = m.ModuleID                       
                                      " + where;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    loginLogs = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return loginLogs;
        }

        public static bool SaveLoginLog(LoginLog loginLog, User currentUser)
        {
            bool result = false;

            string SQL = "";           

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (loginLog.LoginLogID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.LoginLog ( LoginLogID, ModuleID, UserID, IP, 
                                                             UserAgent, SessionID, AuthID, LoginDateTime)
                            VALUES (:LoginLogID, :ModuleID, :UserID, :IP, 
                                    :UserAgent, :SessionID, :AuthID, :LoginDateTime);

                            SELECT PMIS_ADM.LoginLog_ID_SEQ.currval INTO :LoginLogID FROM dual;

                            ";

                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.LoginLog SET
                               ModuleID = :ModuleID, 
                               UserID = :UserID, 
                               IP = :IP, 
                               UserAgent = :UserAgent, 
                               SessionID = :SessionID, 
                               AuthID = :AuthID, 
                               LoginDateTime = :LoginDateTime
                            WHERE LoginLogID = :LoginLogID ;                       

                            ";

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramLoginLogID = new OracleParameter();
                paramLoginLogID.ParameterName = "LoginLogID";
                paramLoginLogID.OracleType = OracleType.Number;

                if (loginLog.LoginLogID != 0)
                {
                    paramLoginLogID.Direction = ParameterDirection.Input;
                    paramLoginLogID.Value = loginLog.LoginLogID;
                }
                else
                {
                    paramLoginLogID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramLoginLogID);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "ModuleID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = loginLog.ModuleID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = loginLog.UserID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IP";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(loginLog.IP))
                    param.Value = loginLog.IP;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UserAgent";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(loginLog.UserAgent))
                    param.Value = loginLog.UserAgent;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SessionID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(loginLog.SessionID))
                    param.Value = loginLog.SessionID;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AuthID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(loginLog.AuthID))
                    param.Value = loginLog.AuthID;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);              
               
                param = new OracleParameter();
                param.ParameterName = "LoginDateTime";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = loginLog.LoginDateTime;
                cmd.Parameters.Add(param);               

                cmd.ExecuteNonQuery();

                if (loginLog.LoginLogID == 0)
                    loginLog.LoginLogID = DBCommon.GetInt(paramLoginLogID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }            

            return result;
        }

        public static bool DeleteLoginLog(int loginLogID, User currentUser)
        {
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.LoginLog WHERE LoginLogID = :LoginLogID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LoginLogID", OracleType.Number).Value = loginLogID;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }         

            return result;
        }

        public static LoginLog CreateInitialLoginLog(User user, Module module)
        {
            LoginLog loginLog = new LoginLog(user);

            loginLog.ModuleID = module.ModuleId;
            loginLog.UserID = user.UserId;            

            SaveLoginLog(loginLog, user);

            return loginLog;
        }

        public static void UpdateLoginLog(User user, Module module, HttpRequest request, HttpSessionState session, LoginLog loginLog)
        {
            loginLog.IP = request.UserHostAddress;
            loginLog.UserAgent = request.ServerVariables["HTTP_USER_AGENT"];
            loginLog.SessionID = session.SessionID;
            loginLog.AuthID = request.Cookies[".ASPXFORMSAUTH_PMIS_" + module.ModuleKey].Value;
            loginLog.LoginDateTime = DateTime.Now;

            SaveLoginLog(loginLog, user);

            UserUtil.ResetFailedLogins(user.UserId, user);
        }
    }
}
