using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

using PMIS.Common;

namespace PMIS.PMISAdmin.Common
{
    //This class represents a particular row on the Audit Trail History screen
    //The properties of this class represent the data that is included on the Audit Trail History screen
    public class AuditTrailHistoryRec
    {
        private int changeId;
        private string username;
        private string firstName;
        private string middleName;
        private string lastName;
        private DateTime changeDate;
        private string moduleName;
        private string changeTypeName;
        private int changeEventId;
        private string changeEventTypeName;
        private string objectDesc;
        private string militaryUnitName;
        private string pFirstName;
        private string pMiddleName;
        private string pLastName;
        private string pIdentityNumber;
        private string changeDetailsHTML;
        private string ip;
        private int rowNumber;

        public int ChangeId
        {
            get
            {
                return changeId;
            }
            set
            {
                changeId = value;
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

        public string UserFullName
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }

        public DateTime ChangeDate
        {
            get
            {
                return changeDate;
            }
            set
            {
                changeDate = value;
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

        public string ChangeTypeName
        {
            get
            {
                return changeTypeName;
            }
            set
            {
                changeTypeName = value;
            }
        }

        public int ChangeEventId
        {
            get
            {
                return changeEventId;
            }
            set
            {
                changeEventId = value;
            }
        }

        public string ChangeEventTypeName
        {
            get
            {
                return changeEventTypeName;
            }
            set
            {
                changeEventTypeName = value;
            }
        }

        public string ObjectDesc
        {
            get
            {
                return objectDesc;
            }
            set
            {
                objectDesc = value;
            }
        }

        public string MilitaryUnitName
        {
            get
            {
                return militaryUnitName;
            }
            set
            {
                militaryUnitName = value;
            }
        }

        public string PFirstName
        {
            get
            {
                return pFirstName;
            }
            set
            {
                pFirstName = value;
            }
        }

        public string PMiddleName
        {
            get
            {
                return pMiddleName;
            }
            set
            {
                pMiddleName = value;
            }
        }

        public string PLastName
        {
            get
            {
                return pLastName;
            }
            set
            {
                pLastName = value;
            }
        }

        public string PersonFullName
        {
            get
            {
                return PFirstName + " " + PMiddleName + " " + PLastName;
            }
        }

        public string PIdentityNumber
        {
            get
            {
                return pIdentityNumber;
            }
            set
            {
                pIdentityNumber = value;
            }
        }

        public string ChangeDetailsHTML
        {
            get
            {
                return changeDetailsHTML;
            }
            set
            {
                changeDetailsHTML = value;
            }
        }

        public string IP
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }

        public int RowNumber
        {
            get
            {
                return rowNumber;
            }
            set
            {
                rowNumber = value;
            }
        }

        public AuditTrailHistoryRec()
        {
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class AuditTrailHistoryFilter
    {
        string users;
        string modules;
        string changeTypes;
        string changeEventTypes;
        string militaryUnits;
        string personIdentityNumber;
        string objectDesc;
        DateTime? dateFrom;
        DateTime? dateTo;
        string oldValue;
        string newValue;
        int? loginLogId;        
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

        public string ChangeTypes
        {
            get
            {
                return changeTypes;
            }
            set
            {
                changeTypes = value;
            }
        }

        public string ChangeEventTypes
        {
            get
            {
                return changeEventTypes;
            }
            set
            {
                changeEventTypes = value;
            }
        }

        public string MilitaryUnits
        {
            get
            {
                return militaryUnits;
            }
            set
            {
                militaryUnits = value;
            }
        }

        public string PersonIdentityNumber
        {
            get
            {
                return personIdentityNumber;
            }
            set
            {
                personIdentityNumber = value;
            }
        }

        public string ObjectDesc
        {
            get
            {
                return objectDesc;
            }
            set
            {
                objectDesc = value;
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

        public string OldValue
        {
            get
            {
                return oldValue;
            }
            set
            {
                oldValue = value;
            }
        }

        public string NewValue
        {
            get
            {
                return newValue;
            }
            set
            {
                newValue = value;
            }
        }

        public int? LoginLogId
        {
            get { return loginLogId; }
            set { loginLogId = value; }
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

    //This class stored methods for looking at the changes log history
    public static class AuditTrailHistoryUtil
    {
        //Get a list of all changes log records according to the provided filter
        public static List<AuditTrailHistoryRec> GetAuditTrailHistory(User currentUser, AuditTrailHistoryFilter filter, int rowsPerPage)
        {
            //Initialize an empty list
            List<AuditTrailHistoryRec> changes = new List<AuditTrailHistoryRec>();

            //Connec to the database
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Dinamically construc the WHERE clause according to the provided filter information
                string where = "";

                if (!String.IsNullOrEmpty(filter.Users))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.UserID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Users) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.ChangeTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ChangeTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.ChangeEventTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.ChangeEventTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ChangeEventTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.PersonIdentityNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(v.EGN) LIKE UPPER('%" + filter.PersonIdentityNumber.Replace("'", "''") + "%') ";
                }

                if (!String.IsNullOrEmpty(filter.ObjectDesc))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(b.ObjectDesc) LIKE UPPER('%" + filter.ObjectDesc.Replace("'", "''") + "%') ";
                }

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeDate >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeDate < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filter.OldValue))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(c.OldValue) = UPPER('" + filter.OldValue.Replace("'", "''") + "') ";
                }

                if (!String.IsNullOrEmpty(filter.NewValue))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(c.NewValue) = UPPER('" + filter.NewValue.Replace("'", "''") + "') ";
                }

                if (filter.LoginLogId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginLogID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.LoginLogId.Value.ToString()) + ") ";
                }

                //Paging (load the rows only for the target page)
                string pageWhere = "";

                if(filter.PageIdx > 0 && rowsPerPage > 0)
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
                        orderBySQL = "(ud.FirstName || ud.MiddleName || ud.LastName)";
                        break;
                    case 2:
                        orderBySQL = "a.ChangeDate";
                        break;
                    case 3:
                        orderBySQL = "m.ModuleName";
                        break;
                    case 4:
                        orderBySQL = "d.ChangeType";
                        break;
                    case 5:
                        orderBySQL = "e.ChangeEventType";
                        break;
                    case 6:
                        orderBySQL = "NVL(b.ObjectDesc, ' ')";
                        break;
                    case 7:
                        orderBySQL = "f.IMEES";
                        break;
                    case 8:
                        orderBySQL = "(v.IME || v.FAM)";
                        break;
                    case 9:
                        orderBySQL = "l.IP";
                        break;
                    default:
                        orderBySQL = "a.ChangeDate";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.ChangeDate ASC" + DBCommon.FixNullsOrder("ASC");

                //The SQL select clause for the data
                string SQL = @"SELECT * FROM (
                                  SELECT a.ChangeID, 
                                         u.Username, ud.FirstName, ud.MiddleName, ud.LastName,
                                         a.ChangeDate,
                                         m.ModuleName,
                                         d.ChangeType as ChangeTypeName,
                                         b.ChangeEventID, e.ChangeEventType as ChangeEventTypeName, e.ChangeEventTypeKey,
                                         e.ChangeType,
                                         b.ObjectDesc, f.VPN || ' ' || f.IMEES as MilitaryUnitName,
                                         v.IME as PFirstName, NULL as PMiddleName, v.FAM as PLastName,
                                         v.EGN as PIdentityNumber,
                                         c.DetailID, g.FieldName, g.FieldType,
                                         c.OldValue, c.NewValue, l.IP,
                                         DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", b.ChangeEventID) as RowNumber 
                                  FROM PMIS_ADM.ChangesLog a
                                  INNER JOIN PMIS_ADM.Users u ON a.UserID = u.UserID
                                  INNER JOIN PMIS_ADM.UserDetails ud ON u.UserID = ud.UserID
                                  INNER JOIN PMIS_ADM.ChangeTypes d ON a.ChangeTypeID = d.ChangeTypeID
                                  INNER JOIN PMIS_ADM.Modules m ON d.ModuleID = m.ModuleID
                                  INNER JOIN PMIS_ADM.ChangeEvents b ON a.ChangeID = b.ChangeID
                                  INNER JOIN PMIS_ADM.ChangeEventTypes e ON b.ChangeEventTypeID = e.ChangeEventTypeID
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR f ON b.MilitaryUnitID = f.KOD_MIR
                                  LEFT OUTER JOIN PMIS_ADM.ChangeEventDetails c ON b.ChangeEventID = c.ChangeEventID
                                  LEFT OUTER JOIN PMIS_ADM.Fields g ON c.FieldID = g.FieldID
                                  LEFT OUTER JOIN VS_OWNER.VS_LS v ON b.PersonID = v.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.LoginLog l ON a.LoginLogID = l.LoginLogID
                                  " + where + @"
                                  ORDER BY " + orderBySQL + @", b.ChangeEventID, g.Seq, g.FieldID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Execute the SQL command
                OracleDataReader dr = cmd.ExecuteReader();

                int prevChangeEventId = -1;
                

                AuditTrailHistoryRec change = null;

                //Iterate the data reader object and construct business objects that are being added to the result list
                while (dr.Read())
                {
                    int changeId = DBCommon.GetInt(dr["ChangeID"]);
                    string firstName = dr["FirstName"].ToString();
                    string middleName = dr["MiddleName"].ToString();
                    string lastName = dr["LastName"].ToString();
                    DateTime changeDate = (DateTime)dr["ChangeDate"];
                    string moduleName = dr["ModuleName"].ToString();
                    string changeTypeName = dr["ChangeTypeName"].ToString();
                    int changeEventId = DBCommon.GetInt(dr["ChangeEventID"]);
                    string changeEventTypeName = dr["ChangeEventTypeName"].ToString();
                    string objectDesc = dr["ObjectDesc"].ToString();
                    string militaryUnitName = dr["MilitaryUnitName"].ToString();
                    string pFirstName = dr["PFirstName"].ToString();
                    string pMiddleName = dr["PMiddleName"].ToString();
                    string pLastName = dr["PLastName"].ToString();
                    string pIdentityNumber = dr["PIdentityNumber"].ToString();
                    
                    int? detailId = null;
                    if (DBCommon.IsInt(dr["DetailID"]))
                        detailId = DBCommon.GetInt(dr["DetailID"]);

                    string fieldName = dr["FieldName"].ToString();
                    string fieldType = dr["FieldType"].ToString();
                    string oldValue = dr["OldValue"].ToString();
                    string newValue = dr["NewValue"].ToString();
                    string ip = dr["IP"].ToString();
                    int rowNumber = DBCommon.GetInt(dr["RowNumber"]);

                    int changeType = DBCommon.GetInt(dr["ChangeType"]);

                    if (fieldType == "bool")
                    {
                        oldValue = oldValue == "1" ? "Да" : "Не";
                        newValue = newValue == "1" ? "Да" : "Не";
                    }

                    //Add multiple change event details into a single row on the screen
                    if (prevChangeEventId != changeEventId)
                    {
                        if(change != null)
                            changes.Add(change);

                        change = new AuditTrailHistoryRec();

                        change.ChangeId = changeId;
                        change.FirstName = firstName;
                        change.MiddleName = middleName;
                        change.LastName = lastName;
                        change.ChangeDate = changeDate;
                        change.ModuleName = moduleName;
                        change.ChangeTypeName = changeTypeName;
                        change.ChangeEventId = changeEventId;
                        change.ChangeEventTypeName = changeEventTypeName;
                        change.ObjectDesc = objectDesc;
                        change.MilitaryUnitName = militaryUnitName;
                        change.PFirstName = pFirstName;
                        change.PMiddleName = pMiddleName;
                        change.PLastName = pLastName;
                        change.PIdentityNumber = pIdentityNumber;
                        change.RowNumber = rowNumber;
                        change.IP = ip;
                        
                        prevChangeEventId = changeEventId;
                    }

                    if (detailId != null)
                    {
                        //Edit
                        if (changeType == 1)
                        {
                            change.ChangeDetailsHTML += @"<div>" + fieldName + ": смяна от '" + oldValue + @"' на '" + newValue + @"'</div>";
                        }
                        //Insert
                        else if (changeType == 2)
                        {
                            change.ChangeDetailsHTML += @"<div>" + fieldName + ": '" + newValue + @"'</div>";
                        }
                        //Delete
                        else if (changeType == 3)
                        {
                            change.ChangeDetailsHTML += @"<div>" + fieldName + ": '" + oldValue + @"'</div>";
                        }
                    }
                }

                if (change != null)
                    changes.Add(change);

                dr.Close();
            }
            finally
            {
                conn.Close();
            }  

            return changes;
        }


        //Get a count of all changes log records according to the provided filter
        public static int GetAuditTrailHistoryRecCnt(User currentUser, AuditTrailHistoryFilter filter)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Construct the WHERE clause according to the filter
                string where = "";

                if (!String.IsNullOrEmpty(filter.Users))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.UserID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Users) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Modules))
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.ModuleID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Modules) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.ChangeTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ChangeTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.ChangeEventTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.ChangeEventTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ChangeEventTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.PersonIdentityNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(v.EGN) LIKE UPPER('%" + filter.PersonIdentityNumber.Replace("'", "''") + "%') ";
                }

                if (filter.DateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeDate >= " + DBCommon.DateToDBCode(filter.DateFrom.Value) + " ";
                }

                if (filter.DateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ChangeDate < " + DBCommon.DateToDBCode(filter.DateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filter.OldValue))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(c.OldValue) = UPPER('" + filter.OldValue.Replace("'", "''") + "') ";
                }

                if (!String.IsNullOrEmpty(filter.NewValue))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(c.NewValue) = UPPER('" + filter.NewValue.Replace("'", "''") + "') ";
                }

                if (filter.LoginLogId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LoginLogID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.LoginLogId.Value.ToString()) + ") ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                //Just get the count of the records (this is used for Paging purposes)
                string SQL = @"SELECT COUNT(DISTINCT b.ChangeEventID) as RecCnt
                               FROM PMIS_ADM.ChangesLog a
                               INNER JOIN PMIS_ADM.Users u ON a.UserID = u.UserID
                               INNER JOIN PMIS_ADM.UserDetails ud ON u.UserID = ud.UserID
                               INNER JOIN PMIS_ADM.ChangeTypes d ON a.ChangeTypeID = d.ChangeTypeID
                               INNER JOIN PMIS_ADM.Modules m ON d.ModuleID = m.ModuleID
                               INNER JOIN PMIS_ADM.ChangeEvents b ON a.ChangeID = b.ChangeID
                               INNER JOIN PMIS_ADM.ChangeEventTypes e ON b.ChangeEventTypeID = e.ChangeEventTypeID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR f ON b.MilitaryUnitID = f.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.ChangeEventDetails c ON b.ChangeEventID = c.ChangeEventID
                               LEFT OUTER JOIN PMIS_ADM.Fields g ON c.FieldID = g.FieldID
                               LEFT OUTER JOIN VS_OWNER.VS_LS v ON b.PersonID = v.PersonID
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cnt = DBCommon.GetInt(dr["RecCnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }
    }
}
