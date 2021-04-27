using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web;
using System.Web.SessionState;

namespace PMIS.Common
{
    public class FailedLogin : BaseDbObject
    {
        private int failedLoginID;        
        private int moduleID;        
        private Module module;        
        private string username;           
        private string ip;       
        private string userAgent;        
        private string sessionID;        
        private DateTime dateTime;        

        public int FailedLoginID
        {
            get { return failedLoginID; }
            set { failedLoginID = value; }
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

        public string Username
        {
            get { return username; }
            set { username = value; }
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

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public FailedLogin(User user)
            : base(user)
        {

        }
    }

    public class FailedLoginsFilter
    {
        string username;
        string modules;
        DateTime? dateFrom;
        DateTime? dateTo;
        int orderBy;
        int pageIdx;

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
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

    public class FailedLoginUtil
    {
        private static FailedLogin ExtractFailedLoginFromDR(OracleDataReader dr, User currentUser)
        {
            FailedLogin failedLogin = new FailedLogin(currentUser);

            failedLogin.FailedLoginID = DBCommon.GetInt(dr["FailedLoginID"]);
            failedLogin.ModuleID = DBCommon.GetInt(dr["ModuleID"]);
            failedLogin.Username = dr["Username"].ToString();
            failedLogin.IP = dr["IP"].ToString();
            failedLogin.UserAgent = dr["UserAgent"].ToString();
            failedLogin.SessionID = dr["SessionID"].ToString();
            failedLogin.DateTime = (DateTime)dr["DateTime"];

            return failedLogin;
        }

        public static FailedLogin GetFailedLogin(int failedLoginID, User currentUser)
        {
            FailedLogin failedLogin = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FailedLoginID, a.ModuleID, a.Username, a.IP, 
                                      a.UserAgent, a.SessionID, a.DateTime
                               FROM PMIS_ADM.FailedLogins a
                               WHERE a.FailedLoginID = :FailedLoginID ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FailedLoginID", OracleType.Number).Value = failedLoginID;                

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    failedLogin = ExtractFailedLoginFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return failedLogin;
        }

        public static List<FailedLogin> GetAllFailedLogins(FailedLoginsFilter filter, int rowsPerPage, User currentUser)
        {
            List<FailedLogin> failedLogins = new List<FailedLogin>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Username))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.Username LIKE '%" + filter.Username.Replace("'", "''") + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }           

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.DateTime >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.DateTime < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
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
                        orderBySQL = "a.Username";
                        break;
                    case 2:
                        orderBySQL = "m.ModuleName";
                        break;
                    case 3:
                        orderBySQL = "a.IP";
                        break;
                    case 4:
                        orderBySQL = "a.DateTime";
                        break;
                    default:
                        orderBySQL = "a.Username";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.FailedLoginID ASC" + DBCommon.FixNullsOrder("ASC");

                string SQL = @"SELECT * 
                               FROM (
                                      SELECT a.FailedLoginID, a.ModuleID, a.Username, a.IP, 
                                             a.UserAgent, a.SessionID, a.DateTime,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FailedLoginID) as RowNumber
                                      FROM PMIS_ADM.FailedLogins a     
                                      INNER JOIN PMIS_ADM.Modules m ON a.ModuleID = m.ModuleID                       
                                      " + where + @"
                                      ORDER BY " + orderBySQL + @", a.FailedLoginID
                                     ) tmp
                               " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);
               
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    failedLogins.Add(ExtractFailedLoginFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return failedLogins;
        }

        public static int GetAllLoginLogsCnt(FailedLoginsFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Username))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.Username LIKE '%" + filter.Username.Replace("'", "''") + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.DateTime >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.DateTime < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
                }
               
                where = (where == "" ? "" : " WHERE ") + where;
                            
                string SQL = @" SELECT COUNT(*) as Cnt                                            
                                FROM PMIS_ADM.FailedLogins a     
                                INNER JOIN PMIS_ADM.Modules m ON a.ModuleID = m.ModuleID                       
                              " + where;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }

        public static bool WriteFailedLoginToDB(FailedLogin failedLogin)
        {
            bool result = false;

            string SQL = "";           

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            try
            {
                SQL = @"BEGIN
                            INSERT INTO PMIS_ADM.FailedLogins ( ModuleID, Username, IP, 
                                     UserAgent, SessionID, DateTime)
                            VALUES (:ModuleID, :Username, :IP, 
                                    :UserAgent, :SessionID, :DateTime);
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "ModuleID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = failedLogin.ModuleID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Username";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = failedLogin.Username;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IP";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(failedLogin.IP))
                    param.Value = failedLogin.IP;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UserAgent";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(failedLogin.UserAgent))
                    param.Value = failedLogin.UserAgent;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SessionID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(failedLogin.SessionID))
                    param.Value = failedLogin.SessionID;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);          
               
                param = new OracleParameter();
                param.ParameterName = "DateTime";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = failedLogin.DateTime;
                cmd.Parameters.Add(param);               

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }            

            return result;
        }

        public static FailedLoginResult WriteFailedLogin(string username, Module module, HttpRequest request, HttpSessionState session)
        {
            FailedLoginResult result = FailedLoginResult.NoAction;

            FailedLogin failedLogin = new FailedLogin(null);

            failedLogin.ModuleID = module.ModuleId;
            failedLogin.Username = username;
            failedLogin.IP = request.UserHostAddress;
            failedLogin.UserAgent = request.ServerVariables["HTTP_USER_AGENT"];
            failedLogin.SessionID = session.SessionID;
            failedLogin.DateTime = DateTime.Now;

            WriteFailedLoginToDB(failedLogin);

            int userId = UserUtil.GetUserID(username);

            if (userId > 0)
            {
                int consecutiveFailedLogins = UserUtil.IncConsecutiveFailedLogins(userId);
                PasswordPolicy passwordPolicy = PasswordPolicyUtil.GetPasswordPolicy();

                if (passwordPolicy.BlockUserAfterFailedLogins.HasValue && passwordPolicy.BlockUserAfterFailedLogins.Value > 0 &&
                    consecutiveFailedLogins >= passwordPolicy.BlockUserAfterFailedLogins.Value &&
                    !UserUtil.IsBlocked(userId))
                {
                    UserUtil.BlockUser(userId);
                    result = FailedLoginResult.BlockedUser;
                }
            }

            return result;
        }
    }

    public enum FailedLoginResult { NoAction, BlockedUser }
}
