using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace PMIS.Common
{
    //This class represents the possible Change Types in the system
    //The Change  Types usually are used to represend a particular screen (e.g. InvestigationProtocol)
    public class ChangeType
    {
        private int changeTypeId;
        private string changeTypeKey;
        private string changeType;
        private Module module;

        public int ChangeTypeId
        {
            get
            {
                return changeTypeId;
            }

            set
            {
                changeTypeId = value;
            }
        }

        public string ChangeTypeKey
        {
            get
            {
                return changeTypeKey;
            }

            set
            {
                changeTypeKey = value;
            }
        }

        public string ChangeTypeName
        {
            get
            {
                return changeType;
            }

            set
            {
                changeType = value;
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

        public ChangeType() { }
    }

    //These are some utility functions that work with ChangeType objects
    public static class ChangeTypeUtil
    {
        //Extract a new ChangeType object from a DataReader.
        private static ChangeType ExtractChangeTypeFromDataReader(OracleDataReader dr)
        {
            ChangeType changeType = new ChangeType();

            int changeTypeId = int.Parse(dr["ChangeTypeID"].ToString());
            string changeTypeName = dr["ChangeType"].ToString();
            string changeTypeKey = dr["ChangeTypeKey"].ToString();

            changeType.ChangeTypeId = changeTypeId;
            changeType.ChangeTypeName = changeTypeName;
            changeType.ChangeTypeKey = changeTypeKey;

            return changeType;
        }

        //Get a particular ChangeType by its key
        public static ChangeType GetChangeTypeByKey(string changeTypeKey, User currentUser)
        {
            ChangeType changeType = new ChangeType();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ChangeTypeID, a.ChangeType, a.ChangeTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        WHERE a.ChangeTypeKey = :ChangeTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ChangeTypeKey", OracleType.VarChar).Value = changeTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    changeType = ExtractChangeTypeFromDataReader(dr);
                    changeType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeType;
        }

        public static List<ChangeType> GetChangeTypes(User currentUser)
        {
            List<ChangeType> changeTypes = new List<ChangeType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ChangeTypeID, a.ChangeType, a.ChangeTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        ORDER BY a.ChangeType";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChangeType changeType = ExtractChangeTypeFromDataReader(dr);
                    changeType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);

                    changeTypes.Add(changeType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeTypes;
        }

        public static List<ChangeType> GetChangeTypesByIDs(User currentUser, string changeTypeIDs)
        {
            List<ChangeType> changeTypes = new List<ChangeType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(changeTypeIDs))
                    where = @" WHERE a.ChangeTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(changeTypeIDs) + @") ";

                SQL = @"SELECT a.ChangeTypeID, a.ChangeType, a.ChangeTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        " + where + @"
                        ORDER BY a.ChangeType";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChangeType changeType = ExtractChangeTypeFromDataReader(dr);
                    changeType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);

                    changeTypes.Add(changeType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeTypes;
        }
    }

    //============================================================================================

    //This class represents the possible Change Event Types in the system
    //The Change Event Types usually are used to represend a particular change on a screen (e.g. Edited InvestigationProtocol)
    public class ChangeEventType
    {
        private int changeEventTypeId;
        private string changeEventTypeKey;
        private string changeEventType;
        private Module module;
        private int changeType; /*1 = Edit; 2 = Insert; 3 = Delete;*/

        public int ChangeEventTypeId
        {
            get
            {
                return changeEventTypeId;
            }

            set
            {
                changeEventTypeId = value;
            }
        }

        public string ChangeEventTypeKey
        {
            get
            {
                return changeEventTypeKey;
            }

            set
            {
                changeEventTypeKey = value;
            }
        }

        public string ChangeEventTypeName
        {
            get
            {
                return changeEventType;
            }

            set
            {
                changeEventType = value;
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

        public int ChangeType
        {
            get
            {
                return changeType;
            }

            set
            {
                changeType = value;
            }
        }

        public ChangeEventType() { }
    }

    //This utility class has some functions for working with ChangeEventType objects
    public static class ChangeEventTypeUtil
    {
        private static ChangeEventType ExtractChangeEventTypeFromDataReader(OracleDataReader dr)
        {
            ChangeEventType changeEventType = new ChangeEventType();

            int changeEventTypeID = int.Parse(dr["ChangeEventTypeID"].ToString());
            string changeEventTypeName = dr["ChangeEventType"].ToString();
            string changeEventTypeKey = dr["ChangeEventTypeKey"].ToString();
            int changeType = int.Parse(dr["ChangeType"].ToString());

            changeEventType.ChangeEventTypeId = changeEventTypeID;
            changeEventType.ChangeEventTypeName = changeEventTypeName;
            changeEventType.ChangeEventTypeKey = changeEventTypeKey;

            return changeEventType;
        }

        //Get a particular ChangeEventType by its key (i.e. changeEventTypeKey)
        public static ChangeEventType GetChangeEventTypeByKey(string changeEventTypeKey, User currentUser)
        {
            ChangeEventType changeEventType = new ChangeEventType();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ChangeEventTypeID, a.ChangeEventType, a.ChangeType, a.ChangeEventTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeEventTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        WHERE a.ChangeEventTypeKey = :ChangeEventTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ChangeEventTypeKey", OracleType.VarChar).Value = changeEventTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    changeEventType = ExtractChangeEventTypeFromDataReader(dr);
                    changeEventType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeEventType;
        }

        //Get a list of all ChangeEventTypes
        public static List<ChangeEventType> GetChangeEventTypes(User currentUser)
        {
            List<ChangeEventType> changeEventTypes = new List<ChangeEventType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.ChangeEventTypeID, a.ChangeEventType, a.ChangeType, a.ChangeEventTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeEventTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        ORDER BY a.ChangeEventType";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChangeEventType changeEventType = ExtractChangeEventTypeFromDataReader(dr);
                    changeEventType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);

                    changeEventTypes.Add(changeEventType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeEventTypes;
        }

        //Get a list of all ChangeEventTypes
        public static List<ChangeEventType> GetChangeEventTypesByIDs(User currentUser, string changeEventTypeIDs)
        {
            List<ChangeEventType> changeEventTypes = new List<ChangeEventType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(changeEventTypeIDs))
                    where = @" WHERE a.ChangeEventTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(changeEventTypeIDs) + @") ";

                SQL = @"SELECT a.ChangeEventTypeID, a.ChangeEventType, a.ChangeType, a.ChangeEventTypeKey,
                               b.ModuleID, b.ModuleName, b.ModuleKey
                        FROM PMIS_ADM.ChangeEventTypes a
                        INNER JOIN PMIS_ADM.Modules b ON a.ModuleID = b.ModuleID
                        " + where + @"
                        ORDER BY a.ChangeEventType";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChangeEventType changeEventType = ExtractChangeEventTypeFromDataReader(dr);
                    changeEventType.Module = ModuleUtil.ExtractModuleFromDataReader(dr);

                    changeEventTypes.Add(changeEventType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return changeEventTypes;
        }
    }

    //===============================================================================

    //This class represents a particular Change Event in the system
    //It is linked to a particular Change (i.e. the Change property)
    //Each change has a type. This is the ChangeEventType property
    //Also, each change can hold information about related object like MilitaryUnit or Person
    //Each change has a list of details (i.e. objects with old and new value) that stores the actual changes within this change event. This is the ChangeEventDetails property.
    public class ChangeEvent
    {
        private int changeEventId;
        private Change change;
        private ChangeEventType changeEventType;
        private string objectDesc;
        private MilitaryUnit militaryUnit;
        private Person person;

        private List<ChangeEventDetail> lstEventDetails = new List<ChangeEventDetail>();

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

        public Change Change
        {
            get
            {
                return change;
            }

            set
            {
                change = value;
            }
        }

        public ChangeEventType ChangeEventType
        {
            get
            {
                return changeEventType;
            }

            set
            {
                changeEventType = value;
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

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                return militaryUnit;
            }

            set
            {
                militaryUnit = value;
            }
        }

        public Person Person
        {
            get
            {
                return person;
            }

            set
            {
                person = value;
            }
        }

        public List<ChangeEventDetail> ChangeEventDetails
        {
            get
            {
                return lstEventDetails;
            }

            set
            {
                lstEventDetails = value;
            }
        }

        public ChangeEvent(string changeEventTypeKey, string objectDesc, MilitaryUnit militaryUnit, Person person, User currentUser) 
        {
            this.ChangeEventType = ChangeEventTypeUtil.GetChangeEventTypeByKey(changeEventTypeKey, currentUser);
            this.ObjectDesc = objectDesc;
            this.MilitaryUnit = militaryUnit;
            this.Person = person;
        }

        //This method is used to add a particular detail (i.e. a specific change with its old and new values) to the list of details for the specific change event
        public void AddDetail(ChangeEventDetail detail)
        {
            detail.ChangeEvent = this;
            ChangeEventDetails.Add(detail);
        }
    }

    //========================================================================================

    //This class represents a specific field in the system that could be changed
    //Basicly there is such a field for each input in the system
    //We identify these fields in the code by their fieldKeys
    public class Field
    {
        private int fieldID;
        private string fieldType;
        private string fieldKey;
        private string fieldName;

        public int FieldID
        {
            get
            {
                return fieldID;
            }

            set
            {
                fieldID = value;
            }
        }

        public string FieldType
        {
            get
            {
                return fieldType;
            }

            set
            {
                fieldType = value;
            }
        }

        public string FieldKey
        {
            get
            {
                return fieldKey;
            }

            set
            {
                fieldKey = value;
            }
        }

        public string FieldName
        {
            get
            {
                return fieldName;
            }

            set
            {
                fieldName = value;
            }
        }

        public Field() { }
    }

    //This is an utillity class that has some basic methods for working with Fields
    public static class FieldUtil
    {
        //Get a specific Field by its key (i.e. the fieldKey)
        public static Field GetFieldByKey(string fieldKey, User currentUser)
        {
            Field field = new Field();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.FieldID, a.FieldType, a.FieldName
                        FROM PMIS_ADM.Fields a
                        WHERE a.FieldKey = :FieldKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FieldKey", OracleType.VarChar).Value = fieldKey;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int fieldID = int.Parse(dr["FieldID"].ToString());
                    string fieldType = dr["FieldType"].ToString();
                    string fieldName = dr["FieldName"].ToString();

                    field.FieldID = fieldID;
                    field.FieldType = fieldType;
                    field.FieldName = fieldName;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return field;
        }
    }

    //===================================================================================

    //The ChangeEventDetails class represents a particular changed field in the system
    //It has old and new vlues. The Field property holds information about which field exactly has been changed
    public class ChangeEventDetail
    {
        private int detailID;
        private ChangeEvent changeEvent;
        private Field field;
        private string oldValue;
        private string newValue;

        public int DetailID
        {
            get
            {
                return detailID;
            }

            set
            {
                detailID = value;
            }
        }

        public ChangeEvent ChangeEvent
        {
            get
            {
                return changeEvent;
            }

            set
            {
                changeEvent = value;
            }
        }

        public Field Field
        {
            get
            {
                return field;
            }

            set
            {
                field = value;
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

        public ChangeEventDetail(string fieldKey, string oldValue, string newValue, User currentUser) 
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.Field = FieldUtil.GetFieldByKey(fieldKey, currentUser);
        }
    }

    //This class represents a specific Change in the system
    //It stores information about the user that has made the change, the date and time of the change and the type of the change (i.e. the screen)
    //The details for the change are sotred as separate ChangeEvents in the lstChangeEvents class member
    public class Change
    {
        private User user;
        private DateTime changeDate;
        private ChangeType changeType;
        private List<ChangeEvent> lstChangeEvents = new List<ChangeEvent>();
        private bool logged;

        public bool HasEvents
        {
            get
            {
                return lstChangeEvents.Count > 0 ? true : false;
            }
        }

        //Use this constructor when loading the history
        public Change()
        {
            logged = true;
        }

        public Change(User user, string changeTypeKey)
        {
            this.user = user;
            this.changeDate = DateTime.Now;
            this.changeType = ChangeTypeUtil.GetChangeTypeByKey(changeTypeKey, this.user);
        }

        public void AddEvent(ChangeEvent changeEvent)
        {
            changeEvent.Change = this;
            lstChangeEvents.Add(changeEvent);
        }
                
        //This method is used when the change details should be stored into the database
        public void WriteLog()
        {
            if (lstChangeEvents == null || lstChangeEvents.Count == 0)
                return;

            if (logged)
                throw new Exception("This log entry alrady has been logged into the DB");

            if (user == null || user.UserId == 0)
                throw new Exception("The assigned user is invalid");

            //First we write a new ChangesLog record and we get the new ID of that change to use it for the "events" records
            string SQL = @"DECLARE
                              ChangeID PMIS_ADM.ChangesLog.ChangeID%TYPE;
                              ChangeEventID PMIS_ADM.ChangeEvents.ChangeID%TYPE;
                           BEGIN

                              INSERT INTO PMIS_ADM.ChangesLog (UserID, ChangeDate, ChangeTypeID, LoginLogID)
                              VALUES (" + this.user.UserId.ToString() + @", 
                                      " + DBCommon.DateTimeToDBCode(this.changeDate) + @", 
                                      " + this.changeType.ChangeTypeId.ToString() + @", 
                                      " + this.user.LoginLogId.ToString() + @");

                              SELECT PMIS_ADM.CHANGESLOG_ID_SEQ.currval INTO ChangeID FROM dual;

                          ";

            //Write each particular event within the change as a new record into the ChangeEvents table
            foreach (ChangeEvent evt in lstChangeEvents)
            {
                SQL += @"INSERT INTO PMIS_ADM.ChangeEvents (ChangeID, ChangeEventTypeID,
                            ObjectDesc, MilitaryUnitID, PersonID)
                         VALUES (ChangeID, 
                                 " + evt.ChangeEventType.ChangeEventTypeId.ToString() + @", 
                                 '" + (String.IsNullOrEmpty(evt.ObjectDesc) ? "" : evt.ObjectDesc.Replace("'", "''")) + @"', 
                                 " + (evt.MilitaryUnit == null || evt.MilitaryUnit.MilitaryUnitId == 0 ? "NULL" : evt.MilitaryUnit.MilitaryUnitId.ToString()) + @", 
                                 " + (evt.Person == null || evt.Person.PersonId == 0 ? "NULL" : evt.Person.PersonId.ToString()) + @"
                                );

                         SELECT PMIS_ADM.ChangeEvents_ID_SEQ.currval INTO ChangeEventID FROM dual;

                        ";

                //For each change event write the details (i.e. each actually changed field) into the ChangeEventDetails table
                foreach (ChangeEventDetail detail in evt.ChangeEventDetails)
                {
                    SQL += @"INSERT INTO PMIS_ADM.ChangeEventDetails (ChangeEventID, FieldID,
                                OldValue, NewValue)
                             VALUES (ChangeEventID,
                                     " + detail.Field.FieldID + @", 
                                     '" + (String.IsNullOrEmpty(detail.OldValue) ? "" : detail.OldValue.Replace("'", "''")) + @"', 
                                     '" + (String.IsNullOrEmpty(detail.NewValue) ? "" : detail.NewValue.Replace("'", "''")) + @"'
                                    );

                            ";
                }
            }

            SQL += @"END;";

            OracleConnection conn = new OracleConnection(this.user.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            this.logged = true;
        }
    }
}
