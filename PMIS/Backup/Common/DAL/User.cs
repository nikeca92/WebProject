using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    //The User class represents the users of the system
    //For each User we keep some details like First Name, Last Name, Email, etc.
    //Also, we keep which DB (Oracle) user should be used when doing DB calls. In other words there is an ability 
    //to use different DB users when different users log into the system (better security strategy). So, each specific
    //user has its own connection string.
    public class User : BaseDbObject
    {
        private int userId;
        private string username;
        private string password;
        private string firstName;
        private string middleName;
        private string lastName;
        private string email;
        private string phone;
        private bool isActive;
        private bool isBlocked;
        private bool passwordDoesNotExpire;
        private string oracleUsername;
        private string oraclePassword;
        private string connectionString;
        private UserRole role;
        private List<UserRole> allRoles;
        private string militaryUnitIds = null;
        private string militaryDepartmentIds = null;
        private string militaryDepartmentIds_ByEquipmentResRequests = null;
        private string militaryDepartmentIds_ByEquipmentTechRequests = null;
        private string militaryDepartmentIds_ListOfValues = null;
        private DateTime? passwordUpdateDate = null;

        private int loginLogId; // not a real field of User object; it's an auxiliary property which is filled in BasePage with LoginLogID value        

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

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

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
            }
        }

        public string MiddleName
        {
            get
            {
                return middleName;
            }

            set
            {
                middleName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                phone = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }

        public bool IsBlocked
        {
            get
            {
                return isBlocked;
            }

            set
            {
                isBlocked = value;
            }
        }

        public bool PasswordDoesNotExpire
        {
            get
            {
                return passwordDoesNotExpire;
            }

            set
            {
                passwordDoesNotExpire = value;
            }
        }

        public string FullName
        {
            get
            {
                return this.firstName + " " + middleName + " " + this.lastName;
            }
        }

        public string OracleUsername
        {
            get
            {
                return oracleUsername;
            }

            set
            {
                oracleUsername = value;
            }
        }

        public string OraclePassword
        {
            get
            {
                return oraclePassword;
            }

            set
            {
                oraclePassword = value;
            }
        }

        public DateTime? PasswordUpdateDate
        {
            get
            {
                return passwordUpdateDate;
            }

            set
            {
                passwordUpdateDate = value;
            }
        }

        //The is the connection string of each specific user
        //To form it we use a connection string template (from web.config) and next we put in it the actual DB user id and password
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    //The is the connection string of each specific user
                    //To form it we use a connection string template (from web.config) and next we put in it the actual DB user id and password
                    connectionString = Config.GetWebSetting("UserConnectionString");
                    connectionString = connectionString.Replace("/USERID/", OracleUsername);
                    connectionString = connectionString.Replace("/PASSWORD/", OraclePassword);
                }

                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public UserRole Role
        {
            get
            {
                return role;
            }

            set
            {
                role = value;
            }
        }

        //Specifies if a particular record could be deleted
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        //This is a list of all Roles that are assigned to the particular user
        public List<UserRole> AllRoles
        {
            get
            {
                return allRoles;
            }

            set
            {
                allRoles = value;
            }
        }

        //A list of IDs for the visible MilitaryUnits for this user
        public string MilitaryUnitIDs
        {
            get
            {
                if (militaryUnitIds == null && CurrentUser != null)
                    militaryUnitIds = MilitaryUnitUtil.GetMilitaryUnitIDsPerUser(this);

                return militaryUnitIds;
            }
        }

        //A list of IDs for the visible MilitaryDepartments for this user
        public string MilitaryDepartmentIDs
        {
            get
            {
                if (militaryDepartmentIds == null && CurrentUser != null)
                    militaryDepartmentIds = MilitaryDepartmentUtil.GetMilitaryDepartmentIDsPerUser(this);

                return militaryDepartmentIds;
            }
        }

        //A list of IDs for the visible MilitaryDepartments for this user by Equipment Reservists Requests by taking into account the requests that the user can see
        public string MilitaryDepartmentIDs_ByEquipmentResRequests
        {
            get
            {
                if (militaryDepartmentIds_ByEquipmentResRequests == null && CurrentUser != null)
                    militaryDepartmentIds_ByEquipmentResRequests = MilitaryDepartmentUtil.GetAllMilitaryDepartmentIDsByEquipmentResRequestsPerUser(this);

                return militaryDepartmentIds_ByEquipmentResRequests;
            }
        }

        //A list of IDs for the visible MilitaryDepartments for this user by Equipment Technics Requests by taking into account the requests that the user can see
        public string MilitaryDepartmentIDs_ByEquipmentTechRequests
        {
            get
            {
                if (militaryDepartmentIds_ByEquipmentTechRequests == null && CurrentUser != null)
                    militaryDepartmentIds_ByEquipmentTechRequests = MilitaryDepartmentUtil.GetAllMilitaryDepartmentIDsByEquipmentTechRequestsPerUser(this);

                return militaryDepartmentIds_ByEquipmentTechRequests;
            }
        }

        //A list of IDs for the visible MilitaryDepartments for this user
        public string MilitaryDepartmentIDs_ListOfValues
        {
            get
            {
                if (militaryDepartmentIds_ListOfValues == null && CurrentUser != null)
                    militaryDepartmentIds_ListOfValues = MilitaryDepartmentUtil.GetMilitaryDepartmentIDsPerUser_ListOfValues(this);

                return militaryDepartmentIds_ListOfValues;
            }
        }

        // not a real field of User object; it's an auxiliary property which is filled in BasePage with LoginLogID value
        public int LoginLogId
        {
            get;
            set;
        }

        public User(User user)
            : base(user)
        {            
        }
    }

    //This is the role that a specific user could have for a specific module.
    public class UserRole : BaseDbObject
    {
        private int roleId;
        private string roleName;
        private Module module;
        private int usersCount;
        private Module moduleByRole;

        public int RoleId
        {
            get
            {
                return roleId;
            }

            set
            {
                roleId = value;
            }
        }

        public string RoleName
        {
            get
            {
                return roleName;
            }

            set
            {
                roleName = value;
            }
        }

        public Module ModuleByRole
        {
            get
            {
                if (moduleByRole == null)
                {
                    moduleByRole = ModuleUtil.GetModuleByRoleID(roleId, CurrentUser);
                }
                return moduleByRole;
            }

            set
            {
                moduleByRole = value;
            }
        }

        public Module Module
        {
            get
            {
                return module;
            }

            set
            {
                module = value;
            }
        }

        //This is the count of users that have this role
        public int UsersCount
        {
            get
            {
                return usersCount;
            }

            set
            {
                usersCount = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return UsersCount <= 0;
            }
        }

        public UserRole (User user) : base(user)
        {
        }
    }

    public class OldPassword
    {
        public int OldPasswordId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime ChangeDate { get; set; }
    }

    public static class OldPasswordUtil
    {
        public static bool WriteOldPassword(string oldPassword, int userId, User currentUser)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           INSERT INTO PMIS_ADM.OldPasswords (UserID, OldPassword, ChangeDate)
                           VALUES (:UserID, :OldPassword, :ChangeDate);
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OldPassword";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = Cryptography.Encrypt(oldPassword);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
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

        public static bool IsInLastNPasswords(string password, int userId, int numberOfLastPasswordsToCheck, User currentUser)
        {
            bool isInLastNPasswords = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"SELECT * FROM 
                        (
                           SELECT OldPassword,
                                  RANK() OVER (ORDER BY ChangeDate DESC, OldPasswordID DESC) as RowNumber 
                           FROM PMIS_ADM.OldPasswords a
                           WHERE UserID = :UserID
                           ORDER BY ChangeDate DESC, OldPasswordID DESC
                        ) tmp
                        WHERE RowNumber <= :NumberOfLastPasswordsToCheck
                        ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NumberOfLastPasswordsToCheck";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = numberOfLastPasswordsToCheck;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string oldPassword = Cryptography.Decrypt(dr["OldPassword"].ToString());

                    if (oldPassword == password)
                    {
                        isInLastNPasswords = true;
                        break;
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return isInLastNPasswords;
        }
    }

    //Some static method that help to work with the UserRole objects
    public static class UserRoleUtil
    {
        //Get a particular role by its ID
        public static UserRole GetUserRole(User currentUser, int roleId)
        {
            UserRole userRole = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RoleID, a.RoleName, 
                                      b.ModuleID, b.ModuleName, b.ModuleKey,
                                      NVL(c.UsersCount, 0) as UsersCount,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_ADM.Roles a
                               INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                               LEFT OUTER JOIN (SELECT COUNT(UserID) as UsersCount,
                                                       RoleID
                                                FROM PMIS_ADM.UserRoles 
                                                WHERE RoleID = :RoleID
                                                GROUP BY RoleID) c ON a.RoleID = c.RoleID
                               WHERE a.RoleID = :RoleID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RoleID", OracleType.Number).Value = roleId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string roleName = dr["RoleName"].ToString();
                    int moduleId = DBCommon.GetInt(dr["ModuleID"]);
                    string moduleName = dr["ModuleName"].ToString();
                    string moduleKey = dr["ModuleKey"].ToString();
                    int usersCount = DBCommon.GetInt(dr["UsersCount"]);

                    userRole = new UserRole(currentUser);

                    userRole.RoleId = roleId;
                    userRole.RoleName = roleName;
                    userRole.Module = new Module();
                    userRole.Module.ModuleId = moduleId;
                    userRole.Module.ModuleKey = moduleKey;
                    userRole.Module.ModuleName = moduleName;
                    userRole.UsersCount = usersCount;

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, userRole);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userRole;
        }

        //Get a list of all user roles
        public static List<UserRole> GetUserRoles(User currentUser, string roleNameFilter, string modules, int orderBy, int pageIdx, int rowsPerPage)
        {
            List<UserRole> userRoles = new List<UserRole>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Construc dynamically the WHERE clause according to the filter
                string where = "";

                if (!String.IsNullOrEmpty(roleNameFilter))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.RoleName) LIKE UPPER('%" + roleNameFilter.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(modules) + ") ";
                }

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                where = (where == "" ? "" : " WHERE ") + where;

                //Set the ORDER BY clause
                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "b.ModuleName";
                        break;
                    case 2:
                        orderBySQL = "a.RoleName";
                        break;
                    case 3:
                        orderBySQL = "NVL(c.UsersCount, 0)";
                        break;
                    default:
                        orderBySQL = "b.ModuleName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.RoleName ASC" + DBCommon.FixNullsOrder("ASC");

                string SQL = @"SELECT * FROM (
                                  SELECT a.RoleID, a.RoleName, 
                                         b.ModuleID, b.ModuleName, 
                                         NVL(c.UsersCount, 0) as UsersCount,
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.RoleID) as RowNumber 
                                  FROM PMIS_ADM.Roles a
                                  INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                                  LEFT OUTER JOIN (SELECT COUNT(UserID) as UsersCount,
                                                          RoleID
                                                   FROM PMIS_ADM.UserRoles 
                                                   GROUP BY RoleID) c ON a.RoleID = c.RoleID
                                  " + where + @"
                                  ORDER BY " + orderBySQL + @", a.RoleID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int roleId = DBCommon.GetInt(dr["RoleID"]);
                    string roleName = dr["RoleName"].ToString();
                    int moduleId = DBCommon.GetInt(dr["ModuleID"]);
                    string moduleName = dr["ModuleName"].ToString();
                    int usersCount = DBCommon.GetInt(dr["UsersCount"]);

                    UserRole role = new UserRole(currentUser);

                    role.RoleId = roleId;
                    role.RoleName = roleName;
                    role.Module = new Module();
                    role.Module.ModuleId = moduleId;
                    role.Module.ModuleName = moduleName;
                    role.UsersCount = usersCount;

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, role);

                    userRoles.Add(role);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userRoles;
        }

        //Get count of all user roles
        public static int GetUserRolesCnt(User currentUser, string roleNameFilter, string modules)
        {
            int userRolesCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Construct dynamically the WHERE clause according to the provided filter
                string where = "";

                if (!String.IsNullOrEmpty(roleNameFilter))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RoleName LIKE '%" + roleNameFilter.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(modules) + ") ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_ADM.Roles a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        userRolesCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userRolesCnt;
        }

        //Save a particular User role. Pass the Role as object
        public static bool SaveRole(User currentUser, UserRole role, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //If a new record then INSERT it
                if (role.RoleId == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.Roles (RoleName, ModuleID, 
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                             VALUES (:RoleName, :ModuleID, 
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                             SELECT PMIS_ADM.Roles_ID_SEQ.currval INTO :RoleID FROM dual;
                            ";

                    //Add this change to the Audit Trail
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Roles_AddRole", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_RoleName", "", role.RoleName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_Module", "", role.Module.ModuleName, currentUser));

                    changeEntry.AddEvent(changeEvent);
                }
                else //UPDATE an existing record
                {
                    SQL += @"UPDATE PMIS_ADM.Roles SET
                               RoleName = :RoleName, 
                               ModuleID = :ModuleID,
                               LastModifiedBy = :LastModifiedBy,
                               LastModifiedDate = :LastModifiedDate
                             WHERE RoleID = :RoleID;
                            ";

                    //If there are any actual changes then track them to the Audit Trail log
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Roles_EditRole", "", null, null, currentUser);

                    UserRole oldRole = GetUserRole(currentUser, role.RoleId);

                    if (oldRole.RoleName.Trim() != role.RoleName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_RoleName", oldRole.RoleName, role.RoleName, currentUser));

                    if (oldRole.Module.ModuleName.Trim() != role.Module.ModuleName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_Module", oldRole.Module.ModuleName, role.Module.ModuleName, currentUser));

                    if (changeEvent.ChangeEventDetails.Count > 0)
                        changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add the RoleID parameter. If it is a new record the pass it as an Output parameter to be ablet to return the new ID.
                OracleParameter paramRoleID = new OracleParameter();
                paramRoleID.ParameterName = "RoleID";
                paramRoleID.OracleType = OracleType.Number;

                if (role.RoleId != 0)
                {
                    paramRoleID.Direction = ParameterDirection.Input;
                    paramRoleID.Value = role.RoleId;
                }
                else
                {
                    paramRoleID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRoleID);

                //Add the other parameters to the query
                OracleParameter param = new OracleParameter();
                param.ParameterName = "RoleName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = role.RoleName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ModuleID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = role.Module.ModuleId;
                cmd.Parameters.Add(param);

                if (role.RoleId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (role.RoleId == 0)
                    role.RoleId = DBCommon.GetInt(paramRoleID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Delete a role
        public static bool DeleteRole(User currentUser, int roleId, Change changeEntry)
        {
            bool result = false;

            UserRole oldRole = GetUserRole(currentUser, roleId);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                  DELETE FROM PMIS_ADM.UIItemsPerRole WHERE RoleID = :RoleID;
                                  DELETE FROM PMIS_ADM.Roles WHERE RoleID = :RoleID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RoleID", OracleType.Number).Value = roleId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            //Save the operation to the changes log
            ChangeEvent changeEvent = new ChangeEvent("ADM_Roles_DeleteRole", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_RoleName", oldRole.RoleName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Role_Module", oldRole.Module.ModuleName, "", currentUser));

            changeEntry.AddEvent(changeEvent);

            return result;
        }
    }


    //Some static method that help to work with the User objects
    public static class UserUtil
    {
        //Get a specific User objet by its ID
        public static User GetUser(int userId, string moduleKey)
        {
            User user = new User(null);
            user.UserId = 0;

            //To find out the particular user DB credentials we have to access the Users table
            //At this point we use a specific DB account (connection string) that the application use to access only the Users table
            //This is the WebConnectionString config entry
            string connString = Config.GetWebSetting("WebConnectionString");
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.OracleUsername, a.OraclePassword
                        FROM PMIS_ADM.Users a
                        WHERE a.UserID = :UserID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string oracleUsername = dr["OracleUsername"].ToString();
                    string oraclePassword = dr["OraclePassword"].ToString();

                    oraclePassword = Cryptography.Decrypt(oraclePassword);

                    user.OracleUsername = oracleUsername;
                    user.OraclePassword = oraclePassword;
                    user.UserId = userId;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            //Once we get the DB credentials for the user then we use its own ConnectionString for all other DB calls.
            //The following query pulls the details for the specific User
            if (user.UserId > 0)
            {
                conn = new OracleConnection(user.ConnectionString);
                conn.Open();

                try
                {
                    SQL = @"SELECT a.Username, b.FirstName, b.MiddleName, b.LastName, 
                                   a.Active, a.Blocked, b.EmailAddress, b.Phone,
                                   c.RoleID, e.RoleName, a.PasswordUpdateDate,
                                   b.PasswordDoesNotExpire,
                                   b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                        INNER JOIN PMIS_ADM.UserRoles c ON a.UserID = c.UserID
                        INNER JOIN PMIS_ADM.Roles e ON e.RoleID = c.RoleID
                        INNER JOIN PMIS_ADM.Modules d ON e.ModuleID = d.ModuleID
                        WHERE a.UserID = :UserID AND d.ModuleKey = :ModuleKey";

                    OracleCommand cmd = new OracleCommand(SQL, conn);

                    cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;
                    cmd.Parameters.Add("ModuleKey", OracleType.VarChar).Value = moduleKey;

                    OracleDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string username = dr["Username"].ToString();
                        string firstName = dr["FirstName"].ToString();
                        string middleName = dr["MiddleName"].ToString();
                        string lastName = dr["LastName"].ToString();
                        string emailAddress = dr["EmailAddress"].ToString();
                        string phone = dr["Phone"].ToString();
                        int active = int.Parse(dr["Active"].ToString());
                        int blocked = int.Parse(dr["Blocked"].ToString());
                        int roleId = int.Parse(dr["RoleID"].ToString());
                        string roleName = dr["RoleName"].ToString();
                        DateTime? passwordUpdateDate = (dr["PasswordUpdateDate"] is DateTime ? (DateTime)dr["PasswordUpdateDate"] : (DateTime?)null);
                        int passwordDoesNotExpire = int.Parse(dr["PasswordDoesNotExpire"].ToString());

                        user.Username = username;
                        user.FirstName = firstName;
                        user.MiddleName = middleName;
                        user.LastName = lastName;
                        user.Email = emailAddress;
                        user.Phone = phone;
                        user.IsActive = (active == 1);
                        user.IsBlocked = (blocked == 1);
                        user.Role = new UserRole(user);
                        user.Role.RoleId = roleId;
                        user.Role.RoleName = roleName;
                        user.PasswordUpdateDate = passwordUpdateDate;
                        user.PasswordDoesNotExpire = (passwordDoesNotExpire == 1);

                        BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, user);

                        user.CurrentUser = user;
                    }

                    dr.Close();
                }
                finally
                {
                    conn.Close();
                }
            }

            return user;
        }

        //Get a specific User objet by LoginLogID
        public static User GetUserByLoginLogID(int loginLogId, string moduleKey)
        {
            User user = new User(null);
            user.UserId = 0;

            //To find out the particular user DB credentials we have to access the Users table
            //At this point we use a specific DB account (connection string) that the application use to access only the Users table
            //This is the WebConnectionString config entry
            string connString = Config.GetWebSetting("WebConnectionString");
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.UserID, a.OracleUsername, a.OraclePassword
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.LoginLog b ON a.UserID = b.UserID
                        WHERE b.LoginLogID = :LoginLogID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LoginLogID", OracleType.Number).Value = loginLogId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    int userId = DBCommon.GetInt(dr["UserID"]);
                    string oracleUsername = dr["OracleUsername"].ToString();
                    string oraclePassword = dr["OraclePassword"].ToString();

                    oraclePassword = Cryptography.Decrypt(oraclePassword);

                    user.OracleUsername = oracleUsername;
                    user.OraclePassword = oraclePassword;
                    user.UserId = userId;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            //Once we get the DB credentials for the user then we use its own ConnectionString for all other DB calls.
            //The following query pulls the details for the specific User
            if (user.UserId > 0)
            {
                conn = new OracleConnection(user.ConnectionString);
                conn.Open();

                try
                {
                    SQL = @"SELECT a.Username, b.FirstName, b.MiddleName, b.LastName, 
                                   a.Active, a.Blocked, b.EmailAddress, b.Phone,
                                   c.RoleID, e.RoleName, a.PasswordUpdateDate,
                                   b.PasswordDoesNotExpire,
                                   b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                        INNER JOIN PMIS_ADM.UserRoles c ON a.UserID = c.UserID
                        INNER JOIN PMIS_ADM.Roles e ON e.RoleID = c.RoleID
                        INNER JOIN PMIS_ADM.Modules d ON e.ModuleID = d.ModuleID
                        WHERE a.UserID = :UserID AND d.ModuleKey = :ModuleKey";

                    OracleCommand cmd = new OracleCommand(SQL, conn);

                    cmd.Parameters.Add("UserID", OracleType.Number).Value = user.UserId;
                    cmd.Parameters.Add("ModuleKey", OracleType.VarChar).Value = moduleKey;

                    OracleDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string username = dr["Username"].ToString();
                        string firstName = dr["FirstName"].ToString();
                        string middleName = dr["MiddleName"].ToString();
                        string lastName = dr["LastName"].ToString();
                        string emailAddress = dr["EmailAddress"].ToString();
                        string phone = dr["Phone"].ToString();
                        int active = int.Parse(dr["Active"].ToString());
                        int blocked = int.Parse(dr["Blocked"].ToString());
                        int roleId = int.Parse(dr["RoleID"].ToString());
                        string roleName = dr["RoleName"].ToString();
                        DateTime? passwordUpdateDate = (dr["PasswordUpdateDate"] is DateTime ? (DateTime)dr["PasswordUpdateDate"] : (DateTime?)null);
                        int passwordDoesNotExpire = int.Parse(dr["PasswordDoesNotExpire"].ToString());

                        user.Username = username;
                        user.FirstName = firstName;
                        user.MiddleName = middleName;
                        user.LastName = lastName;
                        user.Email = emailAddress;
                        user.Phone = phone;
                        user.IsActive = (active == 1);
                        user.IsBlocked = (blocked == 1);
                        user.Role = new UserRole(user);
                        user.Role.RoleId = roleId;
                        user.Role.RoleName = roleName;
                        user.PasswordUpdateDate = passwordUpdateDate;
                        user.PasswordDoesNotExpire = (passwordDoesNotExpire == 1);

                        BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, user);

                        user.CurrentUser = user;
                    }

                    dr.Close();
                }
                finally
                {
                    conn.Close();
                }
            }

            return user;
        }

        //Get a specific User objet by its ID
        public static User GetUser(User currentUser, int userId)
        {
            User user = new User(currentUser);
            user.UserId = 0;


            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
                conn.Open();

                string SQL = "";

                try
                {
                    SQL = @"SELECT a.OracleUsername, a.Username,
                                   b.FirstName, b.MiddleName, b.LastName, 
                                   b.EmailAddress, b.Phone, a.Active, a.Blocked,
                                   c.RoleID, e.RoleName, a.PasswordUpdateDate,
                                   d.ModuleID, d.ModuleName, d.ModuleKey,
                                   b.PasswordDoesNotExpire,
                                   b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate
                            FROM PMIS_ADM.Users a
                            INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                            LEFT OUTER JOIN PMIS_ADM.UserRoles c ON a.UserID = c.UserID
                            LEFT OUTER JOIN PMIS_ADM.Roles e ON e.RoleID = c.RoleID
                            LEFT OUTER JOIN PMIS_ADM.Modules d ON e.ModuleID = d.ModuleID
                            WHERE a.UserID = :UserID";

                    OracleCommand cmd = new OracleCommand(SQL, conn);

                    cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;

                    OracleDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string username = dr["Username"].ToString();
                        string firstName = dr["FirstName"].ToString();
                        string middleName = dr["MiddleName"].ToString();
                        string lastName = dr["LastName"].ToString();
                        string emailAddress = dr["EmailAddress"].ToString();
                        string phone = dr["Phone"].ToString();
                        int active = DBCommon.GetInt(dr["Active"]);
                        int blocked = DBCommon.GetInt(dr["Blocked"]);
                        string oracleUsername = dr["OracleUsername"].ToString();
                        DateTime? passwordUpdateDate = (dr["PasswordUpdateDate"] is DateTime ? (DateTime)dr["PasswordUpdateDate"] : (DateTime?)null);
                        int passwordDoesNotExpire = DBCommon.GetInt(dr["PasswordDoesNotExpire"]);

                        int? roleId = null;

                        if (DBCommon.IsInt(dr["RoleID"]))
                        {
                            roleId = DBCommon.GetInt(dr["RoleID"]);
                        }

                        string roleName = dr["RoleName"].ToString();
                        Module module = ModuleUtil.ExtractModuleFromDataReader(dr);

                        //Create the object once because the query returns multiple rows (for each assigned role)
                        if (user.UserId == 0)
                        {
                            user.UserId = userId;
                            user.Username = username;
                            user.OracleUsername = oracleUsername;
                            user.FirstName = firstName;
                            user.MiddleName = middleName;
                            user.LastName = lastName;
                            user.Email = emailAddress;
                            user.Phone = phone;
                            user.IsActive = (active == 1);
                            user.IsBlocked = (blocked == 1);
                            user.PasswordUpdateDate = passwordUpdateDate;
                            user.PasswordDoesNotExpire = (passwordDoesNotExpire == 1);

                            user.AllRoles = new List<UserRole>();

                            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, user);
                        }

                        //Populate the list of all roles assigned to the user
                        if (roleId.HasValue)
                        {
                            UserRole role = new UserRole(currentUser);
                            role.RoleId = roleId.Value;
                            role.RoleName = roleName;
                            role.Module = module;

                            user.AllRoles.Add(role);
                        }
                    }

                    dr.Close();
                }
                finally
                {
                    conn.Close();
                }

            return user;
        }

        public static int GetUserID(string username)
        {
            int userId = 0;


            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.UserID
                        FROM PMIS_ADM.Users a
                        WHERE UPPER(a.Username) = UPPER(:Username)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Username", OracleType.VarChar).Value = username;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    userId = DBCommon.GetInt(dr["UserID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userId;
        }

        //This method checks if a particular username and passowrd are valid for a specific module
        //It returns the UserId of the specific user if the provided credentials are valied
        //This method is used from the Loing page
        public static int GetValidUserLogin(string username, string password, string moduleKey)
        {
            int userId = 0;

            string connString = Config.GetWebSetting("WebConnectionString");
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.UserID as UserID, a.Password
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.UserRoles b ON a.UserID = b.UserID
                        INNER JOIN PMIS_ADM.Roles c ON b.RoleID = c.RoleID
                        INNER JOIN PMIS_ADM.Modules d ON c.ModuleID = d.ModuleID
                        WHERE UPPER(a.Username) = UPPER(:Username) AND d.ModuleKey = :ModuleKey AND
                              NVL(a.Active, 0) = 1 AND NVL(a.Blocked, 0) = 0";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Username", OracleType.VarChar).Value = username;
                cmd.Parameters.Add("ModuleKey", OracleType.VarChar).Value = moduleKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string encPass = dr["Password"].ToString();

                    if (Cryptography.Encrypt(password) == encPass)
                    {
                        userId = int.Parse(dr["UserID"].ToString());
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userId;
        }

        public static int GetSystemAccessUser(string username)
        {
            int userId = 0;

            string connString = Config.GetWebSetting("WebConnectionString");
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.UserID as UserID
                        FROM PMIS_ADM.Users a                        
                        WHERE UPPER(a.Username) = UPPER(:Username)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Username", OracleType.VarChar).Value = username;                

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    userId = int.Parse(dr["UserID"].ToString());
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return userId;
        }

        //Get a list of all users
        public static List<User> GetUsers(User currentUser, string fullNameFilt, string usernameFilt, int? show,
                                          int orderBy, int pageIdx, int rowsPerPage)
        {
            List<User> users = new List<User>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                //Set the ORDER BY clause
                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "b.FirstName";
                        break;
                    case 2:
                        orderBySQL = "b.MiddleName";
                        break;
                    case 3:
                        orderBySQL = "b.LastName";
                        break;
                    case 4:
                        orderBySQL = "a.Username";
                        break;
                    default:
                        orderBySQL = "b.FirstName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", b.FirstName ASC" + DBCommon.FixNullsOrder("ASC");

                SQL = @"SELECT * FROM (
                                        SELECT a.UserID, a.Username, b.FirstName, b.MiddleName, b.LastName, 
                                               b.EmailAddress, b.Phone, a.Active, a.Blocked, a.PasswordUpdateDate,
                                               b.PasswordDoesNotExpire,
                                               b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                               RANK() OVER (ORDER BY " + orderBySQL + @", a.UserID) as RowNumber 
                                        FROM PMIS_ADM.Users a
                                        INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                                        WHERE UPPER(b.FirstName || ' ' || b.MiddleName || ' ' || b.LastName) LIKE UPPER(:FullName) AND
                                              UPPER(a.Username) LIKE UPPER(:Username) AND
                                              (:Show = 0 OR (:Show = 1 AND NVL(a.Active, 0) = 1) OR (:Show = 2 AND NVL(a.Active, 0) = 0))
                                        ORDER BY " + orderBySQL + @", a.UserID
                                      ) tmp
                        " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FullName", OracleType.VarChar).Value = "%" + fullNameFilt + "%";
                cmd.Parameters.Add("Username", OracleType.VarChar).Value = "%" + usernameFilt + "%";

                if (!show.HasValue)
                    show = 0;

                cmd.Parameters.Add("Show", OracleType.Number).Value = show.Value;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int userId = DBCommon.GetInt(dr["UserID"]);
                    string username = dr["Username"].ToString();
                    string firstName = dr["FirstName"].ToString();
                    string middleName = dr["MiddleName"].ToString();
                    string lastName = dr["LastName"].ToString();
                    string emailAddress = dr["EmailAddress"].ToString();
                    string phone = dr["Phone"].ToString();
                    int active = DBCommon.GetInt(dr["Active"]);
                    int blocked = DBCommon.GetInt(dr["Blocked"]);
                    DateTime? passwordUpdateDate = (dr["PasswordUpdateDate"] is DateTime ? (DateTime)dr["PasswordUpdateDate"] : (DateTime?)null);
                    int passwordDoesNotExpire = DBCommon.GetInt(dr["PasswordDoesNotExpire"]);

                    User user = new User(currentUser);

                    user.UserId = userId;
                    user.Username = username;
                    user.FirstName = firstName;
                    user.MiddleName = middleName;
                    user.LastName = lastName;
                    user.Email = emailAddress;
                    user.Phone = phone;
                    user.IsActive = (active == 1);
                    user.IsBlocked = (blocked == 1);
                    user.PasswordUpdateDate = passwordUpdateDate;
                    user.PasswordDoesNotExpire = (passwordDoesNotExpire == 1);

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, user);

                    users.Add(user);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return users;
        }

        //Get a count of all users
        public static int GetUsersCnt(User currentUser, string fullNameFilt, string usernameFilt, int? show)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT COUNT(*) as Cnt
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                        WHERE UPPER(b.FirstName || ' ' || b.MiddleName || ' ' || b.LastName) LIKE UPPER(:FullName) AND
                              UPPER(a.Username) LIKE UPPER(:Username) AND
                              (:Show = 0 OR (:Show = 1 AND NVL(a.Active, 0) = 1) OR (:Show = 2 AND NVL(a.Active, 0) = 0))";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FullName", OracleType.VarChar).Value = "%" + fullNameFilt + "%";
                cmd.Parameters.Add("Username", OracleType.VarChar).Value = "%" + usernameFilt + "%";

                if (!show.HasValue)
                    show = 0;

                cmd.Parameters.Add("Show", OracleType.Number).Value = show.Value;

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

        //Get a list of all users by IDs
        public static List<User> GetUsersByIDs(User currentUser, string userIDs)
        {
            List<User> users = new List<User>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(userIDs))
                    where = @" WHERE a.UserID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(userIDs) + @") ";

                SQL = @"SELECT a.UserID, a.Username, b.FirstName, b.MiddleName, b.LastName, 
                               b.EmailAddress, b.Phone, a.Active, a.Blocked, a.PasswordUpdateDate,
                               b.PasswordDoesNotExpire,
                               b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate
                        FROM PMIS_ADM.Users a
                        INNER JOIN PMIS_ADM.UserDetails b ON a.UserID = b.UserID
                        " + where + @"
                        ORDER BY b.FirstName, b.MiddleName, b.LastName, a.UserID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int userId = DBCommon.GetInt(dr["UserID"]);
                    string username = dr["Username"].ToString();
                    string firstName = dr["FirstName"].ToString();
                    string middleName = dr["MiddleName"].ToString();
                    string lastName = dr["LastName"].ToString();
                    string emailAddress = dr["EmailAddress"].ToString();
                    string phone = dr["Phone"].ToString();
                    int active = DBCommon.GetInt(dr["Active"]);
                    int blocked = DBCommon.GetInt(dr["Blocked"]);
                    DateTime? passwordUpdateDate = (dr["PasswordUpdateDate"] is DateTime ? (DateTime)dr["PasswordUpdateDate"] : (DateTime?)null);
                    int passwordDoesNotExpire = DBCommon.GetInt(dr["PasswordDoesNotExpire"]);

                    User user = new User(currentUser);

                    user.UserId = userId;
                    user.Username = username;
                    user.FirstName = firstName;
                    user.MiddleName = middleName;
                    user.LastName = lastName;
                    user.Email = emailAddress;
                    user.Phone = phone;
                    user.IsActive = (active == 1);
                    user.IsBlocked = (blocked == 1);
                    user.PasswordUpdateDate = passwordUpdateDate;
                    user.PasswordDoesNotExpire = (passwordDoesNotExpire == 1);

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, user);

                    users.Add(user);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return users;
        }

        //Delete an user
        public static bool DeleteUser(User currentUser, int userId, Change changeEntry)
        {
            bool result = false;

            User oldUser = GetUser(currentUser, userId);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                  DELETE FROM PMIS_ADM.MilitaryUnitsPerUser WHERE UserID = :UserID;
                                  DELETE FROM PMIS_ADM.MilitaryDepartmentsPerUser WHERE UserID = :UserID;
                                  DELETE FROM PMIS_ADM.UserRoles WHERE UserID = :UserID;
                                  DELETE FROM PMIS_ADM.UserDetails WHERE UserID = :UserID;
                                  DELETE FROM PMIS_ADM.Users WHERE UserID = :UserID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            //Track the chagne into the Audit Trail history log
            ChangeEvent changeEvent = new ChangeEvent("ADM_Users_DeleteUser", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Username", oldUser.Username, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_OracleUsername", oldUser.OracleUsername, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Active", oldUser.IsActive ? "1" : "0", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Blocked", oldUser.IsBlocked ? "1" : "0", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_FirstName", oldUser.FirstName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_MiddleName", oldUser.MiddleName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_LastName", oldUser.LastName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Email", oldUser.Email, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Phone", oldUser.Phone, "", currentUser));

            foreach (UserRole role in oldUser.AllRoles)
            {
                changeEvent.AddDetail(new ChangeEventDetail("ADM_User_" + role.Module.ModuleKey + "Role", role.RoleName, "", currentUser));
            }

            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_PasswordDoesNotExpire", oldUser.PasswordDoesNotExpire ? "1" : "0", "", currentUser));

            changeEntry.AddEvent(changeEvent);

            return result;
        }

        //Save user
        public static bool SaveUser(User currentUser, User user, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //This is a new record. In this case INSERT the record.
                if (user.UserId == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.Users (Username, Password, OracleUsername, OraclePassword, Active, Blocked, PasswordUpdateDate)
                             VALUES (:Username, :Password, :OracleUsername, :OraclePassword, :Active, :Blocked, :PasswordUpdateDate);

                             SELECT PMIS_ADM.Users_ID_SEQ.currval INTO :UserID FROM dual;

                             INSERT INTO PMIS_ADM.UserDetails (UserID, PersonID, FirstName, MiddleName, LastName, 
                                EmailAddress, Phone, PasswordDoesNotExpire,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                             VALUES (:UserID, NULL, :FirstName, :MiddleName, :LastName, :EmailAddress, :Phone,
                                :PasswordDoesNotExpire,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            ";

                    //Track this change into the Audit Trail History log
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Users_AddUser", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Username", "", user.Username, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_OracleUsername", "", user.OracleUsername, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Active", "", user.IsActive ? "1" : "0", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Blocked", "", user.IsBlocked ? "1" : "0", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_FirstName", "", user.FirstName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_MiddleName", "", user.MiddleName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_LastName", "", user.LastName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Email", "", user.Email, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Phone", "", user.Phone, currentUser));

                    //Add each role as a detail
                    foreach (UserRole role in user.AllRoles)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_" + role.Module.ModuleKey + "Role", "", role.RoleName, currentUser));
                    }

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_PasswordDoesNotExpire", "", user.PasswordDoesNotExpire ? "1" : "0", currentUser));

                    changeEntry.AddEvent(changeEvent);
                }
                else //UPDATE and existing record
                {
                    SQL += @"UPDATE PMIS_ADM.Users SET
                               Username = :Username, 
                               " + (!String.IsNullOrEmpty(user.Password) ? "Password = :Password, PasswordUpdateDate = :PasswordUpdateDate," : "") + @"
                               OracleUsername = :OracleUsername, 
                               " + (!String.IsNullOrEmpty(user.OraclePassword) ? "OraclePassword = :OraclePassword," : "") + @"
                               Active = :Active,
                               Blocked = :Blocked
                             WHERE UserID = :UserID;

                             UPDATE PMIS_ADM.UserDetails SET
                               FirstName = :FirstName,
                               MiddleName = :MiddleName,
                               LastName = :LastName,
                               EmailAddress = :EmailAddress,
                               Phone = :Phone,
                               PasswordDoesNotExpire = :PasswordDoesNotExpire,
                               LastModifiedBy = :LastModifiedBy,
                               LastModifiedDate = :LastModifiedDate
                             WHERE UserID = :UserID;
                            ";

                    //Chech if there are any changes and if there is an actual change then add the change to the Audit Trail log
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Users_EditUser", "", null, null, currentUser);

                    User oldUser = GetUser(currentUser, user.UserId);

                    if (oldUser.Username != user.Username)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Username", oldUser.Username, user.Username, currentUser));

                    if (user.Password != "")
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Password", "", "", currentUser));
                        OldPasswordUtil.WriteOldPassword(user.Password, user.UserId, currentUser);
                    }

                    if (oldUser.OracleUsername != user.OracleUsername)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_OracleUsername", oldUser.OracleUsername, user.OracleUsername, currentUser));

                    if (user.OraclePassword != "")
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_OraclePassword", "", "", currentUser));

                    if (oldUser.IsActive != user.IsActive)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Active", oldUser.IsActive ? "1" : "0", user.IsActive ? "1" : "0", currentUser));

                    if (oldUser.IsBlocked != user.IsBlocked)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Blocked", oldUser.IsBlocked ? "1" : "0", user.IsBlocked ? "1" : "0", currentUser));

                        //When the Block flag for the user is removed then reset the counter of failed logins
                        if (!user.IsBlocked)
                        {
                            ResetFailedLogins(user.UserId, currentUser);
                        }
                    }

                    if (oldUser.FirstName != user.FirstName)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_FirstName", oldUser.FirstName, user.FirstName, currentUser));

                    if (oldUser.MiddleName != user.MiddleName)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_MiddleName", oldUser.MiddleName, user.MiddleName, currentUser));

                    if (oldUser.LastName != user.LastName)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_LastName", oldUser.LastName, user.LastName, currentUser));

                    if (oldUser.Email != user.Email)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Email", oldUser.Email, user.Email, currentUser));

                    if (oldUser.Phone != user.Phone)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Phone", oldUser.Phone, user.Phone, currentUser));

                    //Find all changes in the already set roles
                    foreach (UserRole oldRole in oldUser.AllRoles)
                    {
                        string oldRoleName = oldRole.RoleName;
                        string newRoleName = "";

                        foreach (UserRole role in user.AllRoles)
                        {
                            if (oldRole.Module.ModuleId == role.Module.ModuleId)
                            {
                                newRoleName = role.RoleName;
                                break;
                            }
                        }

                        if (oldRoleName != newRoleName)
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_" + oldRole.Module.ModuleKey + "Role", oldRoleName, newRoleName, currentUser));
                    }

                    //Find all changes related to new set roles
                    foreach (UserRole role in user.AllRoles)
                    {
                        bool foundInOld = false;

                        foreach (UserRole oldRole in oldUser.AllRoles)
                        {
                            if (oldRole.Module.ModuleId == role.Module.ModuleId)
                            {
                                foundInOld = true;
                                break;
                            }
                        }

                        if (!foundInOld)
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_User_" + role.Module.ModuleKey + "Role", "", role.RoleName, currentUser));
                    }

                    if (oldUser.PasswordDoesNotExpire != user.PasswordDoesNotExpire)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_User_PasswordDoesNotExpire", oldUser.PasswordDoesNotExpire ? "1" : "0", user.PasswordDoesNotExpire ? "1" : "0", currentUser));

                    if (changeEvent.ChangeEventDetails.Count > 0)
                        changeEntry.AddEvent(changeEvent);
                }

                //First delete all assigned Roles...
                SQL += @"DELETE FROM PMIS_ADM.UserRoles WHERE UserID = :UserID;
                        ";

                //... and next insert all updated roles
                foreach (UserRole role in user.AllRoles)
                {
                    SQL += @"INSERT INTO PMIS_ADM.UserRoles (UserID, RoleID)
                             VALUES (:UserID, " + role.RoleId.ToString() + @");
                        ";
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add the UserID parameter. It should be Output when adding a new record to be able to read the new ID later
                OracleParameter paramUserID = new OracleParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.OracleType = OracleType.Number;

                if (user.UserId != 0)
                {
                    paramUserID.Direction = ParameterDirection.Input;
                    paramUserID.Value = user.UserId;
                }
                else
                {
                    paramUserID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramUserID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "Username";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Username;
                cmd.Parameters.Add(param);

                //If the Password hasn't been changed then don't add it to the query
                if (!String.IsNullOrEmpty(user.Password))
                {
                    param = new OracleParameter();
                    param.ParameterName = "Password";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = Cryptography.Encrypt(user.Password);
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "PasswordUpdateDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "OracleUsername";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.OracleUsername;
                cmd.Parameters.Add(param);

                //If the Password hasn't been changed then don't add it to the query
                if (!String.IsNullOrEmpty(user.OraclePassword))
                {
                    param = new OracleParameter();
                    param.ParameterName = "OraclePassword";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = Cryptography.Encrypt(user.OraclePassword);
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.FirstName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MiddleName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.MiddleName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.LastName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EmailAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Email;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Phone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Phone;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Active";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (user.IsActive ? 1 : 0);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Blocked";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (user.IsBlocked ? 1 : 0);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PasswordDoesNotExpire";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (user.PasswordDoesNotExpire ? 1 : 0);
                cmd.Parameters.Add(param);

                if (user.UserId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (user.UserId == 0)
                    user.UserId = DBCommon.GetInt(paramUserID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Save user profile (when the user edits himself)
        public static bool SaveUserProfile(User currentUser, User user, Change changeEntry, string changetEventTypeKey)
        {
            bool result = false;

            //Use this object later when constructing the Audit Trail log
            User oldUser = GetUser(currentUser, user.UserId);

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_ADM.Users SET
                               Username = :Username
                               " + (!String.IsNullOrEmpty(user.Password) ? ",Password = :Password" : "") + @"
                           WHERE UserID = :UserID;

                           UPDATE PMIS_ADM.UserDetails SET
                               FirstName = :FirstName,
                               MiddleName = :MiddleName,
                               LastName = :LastName,
                               EmailAddress = :EmailAddress,
                               Phone = :Phone,
                               LastModifiedBy = :LastModifiedBy,
                               LastModifiedDate = :LastModifiedDate
                           WHERE UserID = :UserID;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add the UserID parameter. It should be Output when adding a new record to be able to read the new ID later
                OracleParameter paramUserID = new OracleParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.OracleType = OracleType.Number;

                if (user.UserId != 0)
                {
                    paramUserID.Direction = ParameterDirection.Input;
                    paramUserID.Value = user.UserId;
                }
                else
                {
                    paramUserID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramUserID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "Username";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Username;
                cmd.Parameters.Add(param);

                //If the Password hasn't been changed then don't add it to the query
                if (!String.IsNullOrEmpty(user.Password))
                {
                    param = new OracleParameter();
                    param.ParameterName = "Password";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = Cryptography.Encrypt(user.Password);
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.FirstName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MiddleName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.MiddleName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.LastName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EmailAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Email;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Phone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = user.Phone;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (user.UserId == 0)
                    user.UserId = DBCommon.GetInt(paramUserID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                //Chech if there are any changes and if there is an actual change then add the change to the Audit Trail log
                ChangeEvent changeEvent = new ChangeEvent(changetEventTypeKey, "", null, null, currentUser);

                if (oldUser.Username != user.Username)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Username", oldUser.Username, user.Username, currentUser));

                if (user.Password != "")
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Password", "", "", currentUser));

                if (oldUser.FirstName != user.FirstName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_FirstName", oldUser.FirstName, user.FirstName, currentUser));

                if (oldUser.MiddleName != user.MiddleName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_MiddleName", oldUser.MiddleName, user.MiddleName, currentUser));

                if (oldUser.LastName != user.LastName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_LastName", oldUser.LastName, user.LastName, currentUser));

                if (oldUser.Email != user.Email)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Email", oldUser.Email, user.Email, currentUser));

                if (oldUser.Phone != user.Phone)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_User_Phone", oldUser.Phone, user.Phone, currentUser));

                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Check if a particular username is already used
        public static bool IsExistingUsername(User currentUser, string username, int userId)
        {
            bool isExisting = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt 
                               FROM PMIS_ADM.Users 
                               WHERE UPPER(Username) = UPPER(:Username) AND UserID <> :UserID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;
                cmd.Parameters.Add("Username", OracleType.VarChar).Value = username;

                OracleDataReader dr = cmd.ExecuteReader();

                int cnt = 0;

                //Get the count of the other users having the same username
                if (dr.Read())
                {
                    cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();

                isExisting = cnt > 0;
            }
            finally
            {
                conn.Close();
            }

            return isExisting;
        }

        //Check if a particular password is valid
        public static bool IsValidPassword(User currentUser, string password, int userId)
        {
            bool isValid = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT Password
                               FROM PMIS_ADM.Users 
                               WHERE UserID = :UserID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = userId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string currentPassword = dr["Password"].ToString();

                    if (Cryptography.Decrypt(currentPassword) == password)
                        isValid = true;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return isValid;
        }

        public static int IncConsecutiveFailedLogins(int userId)
        {
            int consecutiveFailedLogins = 0;

            string SQL = "";

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           ConsFailedLogins number;
                        BEGIN
                           SELECT NVL(a.ConsecutiveFailedLogins, 0) INTO ConsFailedLogins
                           FROM PMIS_ADM.Users a
                           WHERE a.UserID = :UserID;

                           ConsFailedLogins := ConsFailedLogins + 1;

                           UPDATE PMIS_ADM.Users SET
                              ConsecutiveFailedLogins = ConsFailedLogins
                           WHERE UserID = :UserID;

                           :NewConsFailedLogins := ConsFailedLogins;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
                cmd.Parameters.Add(param);

                OracleParameter paramNewConsFailedLogins = new OracleParameter();
                paramNewConsFailedLogins.ParameterName = "NewConsFailedLogins";
                paramNewConsFailedLogins.OracleType = OracleType.Number;
                paramNewConsFailedLogins.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramNewConsFailedLogins);

                cmd.ExecuteNonQuery();

                consecutiveFailedLogins = int.Parse(paramNewConsFailedLogins.Value.ToString()); 
            }
            finally
            {
                conn.Close();
            }

            return consecutiveFailedLogins;
        }

        public static bool ResetFailedLogins(int userId, User currentUser)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_ADM.Users SET
                              ConsecutiveFailedLogins = 0
                           WHERE UserID = :UserID;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
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

        public static bool BlockUser(int userId)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_ADM.Users SET
                              Blocked = 1
                           WHERE UserID = :UserID;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
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

        public static bool IsBlocked(int userId)
        {
            bool isBlocked = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            try
            {
                SQL = @"SELECT a.Blocked FROM PMIS_ADM.Users a
                        WHERE a.UserID = :UserID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    isBlocked = DBCommon.IsInt(dr["Blocked"]) && DBCommon.GetInt(dr["Blocked"]) == 1;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return isBlocked;
        }


        //Save user
        public static bool ChangePassword(string password, int userId, User currentUser)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_ADM.Users SET
                               Password = :Password, 
                               PasswordUpdateDate = :PasswordUpdateDate
                            WHERE UserID = :UserID;
                         END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "UserID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = userId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Password";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = Cryptography.Encrypt(password);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PasswordUpdateDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            OldPasswordUtil.WriteOldPassword(password, userId, currentUser);

            return result;
        }
    }
}