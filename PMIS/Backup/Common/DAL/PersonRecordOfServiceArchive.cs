using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace PMIS.Common.DAL
{
    //Represents single person record of service
    public class PersonRecordOfServiceArchive : BaseDbObject
    {
        private int recordOfServiceId;
        private int personId;
        private string recordOfServiceSeries;
        private string recordOfServiceNumber;
        private DateTime? recordOfServiceDate;
        private bool recordOfServiceCopy;
        private string recordOfServiceComment;

        public int RecordOfServiceId
        {
            get
            {
                return recordOfServiceId;
            }
            set
            {
                recordOfServiceId = value;
            }
        }

        public int PersonId
        {
            get
            {
                return personId;
            }
            set
            {
                personId = value;
            }
        }

        public string RecordOfServiceSeries
        {
            get
            {
                return recordOfServiceSeries;
            }
            set
            {
                recordOfServiceSeries = value;
            }
        }

        public string RecordOfServiceNumber
        {
            get
            {
                return recordOfServiceNumber;
            }
            set
            {
                recordOfServiceNumber = value;
            }
        }

        public DateTime? RecordOfServiceDate
        {
            get
            {
                return recordOfServiceDate;
            }
            set
            {
                recordOfServiceDate = value;
            }
        }

        public bool RecordOfServiceCopy
        {
            get
            {
                return recordOfServiceCopy;
            }
            set
            {
                recordOfServiceCopy = value;
            }
        }

        public string RecordOfServiceComment
        {
            get
            {
                return recordOfServiceComment;
            }
            set
            {
                recordOfServiceComment = value;
            }
        }

        public PersonRecordOfServiceArchive(User user)
            : base(user)
        {

        }
    }

    public class PersonRecordOfServiceArchiveUtil
    {
        private static PersonRecordOfServiceArchive ExtractPersonRecordOfServiceArchiveFromDR(OracleDataReader dr, User currentUser)
        {
            PersonRecordOfServiceArchive personRecordOfServiceArchive = new PersonRecordOfServiceArchive(currentUser);

            personRecordOfServiceArchive.RecordOfServiceId = DBCommon.GetInt(dr["RecordOfServiceID"]);
            personRecordOfServiceArchive.RecordOfServiceSeries = dr["RecordOfServiceSeries"].ToString();
            personRecordOfServiceArchive.RecordOfServiceNumber = dr["RecordOfServiceNumber"].ToString();
            personRecordOfServiceArchive.RecordOfServiceDate = (dr["RecordOfServiceDate"] is DateTime) ? (DateTime)dr["RecordOfServiceDate"] : (DateTime?)null;
            personRecordOfServiceArchive.RecordOfServiceCopy = (DBCommon.IsInt(dr["RecordOfServiceCopy"]) && DBCommon.GetInt(dr["RecordOfServiceCopy"]) == 1);
            personRecordOfServiceArchive.RecordOfServiceComment = dr["RecordOfServiceComment"].ToString();

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, personRecordOfServiceArchive);

            return personRecordOfServiceArchive;
        }

        public static PersonRecordOfServiceArchive GetPersonRecordOfServiceArchive(int recordOfServiceId, User currentUser)
        {
            PersonRecordOfServiceArchive personRecordOfServiceArchive = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RecordOfServiceID as RecordOfServiceID, 
                                      a.RecordOfServiceSeries as RecordOfServiceSeries,
                                      a.RecordOfServiceNumber as RecordOfServiceNumber, 
                                      a.RecordOfServiceDate as RecordOfServiceDate,
                                      a.RecordOfServiceCopy as RecordOfServiceCopy,
                                      a.RecordOfServiceComment as RecordOfServiceComment,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate,
                                      a.LastModifiedBy as LastModifiedBy,
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_ADM.RecordOfServiceArchives a
                               WHERE a.RecordOfServiceID = :RecordOfServiceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RecordOfServiceID", OracleType.Number).Value = recordOfServiceId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personRecordOfServiceArchive = ExtractPersonRecordOfServiceArchiveFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personRecordOfServiceArchive;
        }

        public static List<PersonRecordOfServiceArchive> GetPersonRecordOfServiceArchiveByPersonID(int personId, User currentUser)
        {
            List<PersonRecordOfServiceArchive> personRecordOfServiceArchive = new List<PersonRecordOfServiceArchive>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.RecordOfServiceID as RecordOfServiceID, 
                                      a.RecordOfServiceSeries as RecordOfServiceSeries,
                                      a.RecordOfServiceNumber as RecordOfServiceNumber, 
                                      a.RecordOfServiceDate as RecordOfServiceDate, 
                                      a.RecordOfServiceCopy as RecordOfServiceCopy,
                                      a.RecordOfServiceComment as RecordOfServiceComment,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate,
                                      a.LastModifiedBy as LastModifiedBy,
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_ADM.RecordOfServiceArchives a
                               WHERE a.PersonID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    personRecordOfServiceArchive.Add(ExtractPersonRecordOfServiceArchiveFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personRecordOfServiceArchive;
        }

        public static bool SavePersonRecordOfServiceArchive(PersonRecordOfServiceArchive personRecordOfServiceArchive, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personRecordOfServiceArchive.RecordOfServiceId == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.RecordOfServiceArchives (PersonID, RecordOfServiceSeries, RecordOfServiceNumber, RecordOfServiceDate, RecordOfServiceCopy, RecordOfServiceComment, Createdby, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PersonID, :RecordOfServiceSeries, :RecordOfServiceNumber, :RecordOfServiceDate, :RecordOfServiceCopy, :RecordOfServiceComment, :Createdby, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_ADM.ROSA_ROSID_SEQ.currval INTO :RecordOfServiceID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddRecordOfServiceArchive", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceSeries", "", personRecordOfServiceArchive.RecordOfServiceSeries, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceNumber", "", personRecordOfServiceArchive.RecordOfServiceNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceDate", "", CommonFunctions.FormatDate(personRecordOfServiceArchive.RecordOfServiceDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceCopy", "", (personRecordOfServiceArchive.RecordOfServiceCopy ? "1" : "0"), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceComment", "", personRecordOfServiceArchive.RecordOfServiceComment, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.RecordOfServiceArchives SET
                               RecordOfServiceSeries = :RecordOfServiceSeries, 
                               RecordOfServiceNumber = :RecordOfServiceNumber, 
                               RecordOfServiceDate = :RecordOfServiceDate, 
                               RecordOfServiceCopy = :RecordOfServiceCopy, 
                               RecordOfServiceComment = :RecordOfServiceComment,
                               LastModifiedBy = :LastModifiedBy,
                               LastModifiedDate = :LastModifiedDate
                               
                            WHERE RecordOfServiceID = :RecordOfServiceID ;                       

                            ";

                    PersonRecordOfServiceArchive oldPersonRecordOfServiceArchive = GetPersonRecordOfServiceArchive(personRecordOfServiceArchive.RecordOfServiceId, currentUser);

                    changeEvent = new ChangeEvent("RES_Reservist_EditRecordOfServiceArchive", "", null, person, currentUser);

                    if (oldPersonRecordOfServiceArchive.RecordOfServiceSeries.Trim() != personRecordOfServiceArchive.RecordOfServiceSeries.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceSeries", oldPersonRecordOfServiceArchive.RecordOfServiceSeries, personRecordOfServiceArchive.RecordOfServiceSeries, currentUser));

                    if (oldPersonRecordOfServiceArchive.RecordOfServiceNumber.Trim() != personRecordOfServiceArchive.RecordOfServiceNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceNumber", oldPersonRecordOfServiceArchive.RecordOfServiceNumber, personRecordOfServiceArchive.RecordOfServiceNumber, currentUser));

                    string oldRecordOfServiceDate = CommonFunctions.FormatDate(oldPersonRecordOfServiceArchive.RecordOfServiceDate);
                    string newRecordOfServiceDate = CommonFunctions.FormatDate(personRecordOfServiceArchive.RecordOfServiceDate);
                    if (oldRecordOfServiceDate != newRecordOfServiceDate)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceDate", oldRecordOfServiceDate, newRecordOfServiceDate, currentUser));

                    if (!oldPersonRecordOfServiceArchive.RecordOfServiceCopy.Equals(personRecordOfServiceArchive.RecordOfServiceCopy))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceCopy", (oldPersonRecordOfServiceArchive.RecordOfServiceCopy ? "1" : "0"), (personRecordOfServiceArchive.RecordOfServiceCopy ? "1" : "0"), currentUser));

                    if (oldPersonRecordOfServiceArchive.RecordOfServiceComment.Trim() != personRecordOfServiceArchive.RecordOfServiceComment.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceComment", oldPersonRecordOfServiceArchive.RecordOfServiceComment, personRecordOfServiceArchive.RecordOfServiceComment, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRecordOfServiceID = new OracleParameter();
                paramRecordOfServiceID.ParameterName = "RecordOfServiceID";
                paramRecordOfServiceID.OracleType = OracleType.Number;

                if (personRecordOfServiceArchive.RecordOfServiceId != 0)
                {
                    paramRecordOfServiceID.Direction = ParameterDirection.Input;
                    paramRecordOfServiceID.Value = personRecordOfServiceArchive.RecordOfServiceId;
                }
                else
                {
                    paramRecordOfServiceID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRecordOfServiceID);

                OracleParameter param = null;

                if (personRecordOfServiceArchive.RecordOfServiceId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personRecordOfServiceArchive.PersonId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceSeries";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (personRecordOfServiceArchive.RecordOfServiceSeries != null)
                    param.Value = personRecordOfServiceArchive.RecordOfServiceSeries;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (personRecordOfServiceArchive.RecordOfServiceNumber != null)
                    param.Value = personRecordOfServiceArchive.RecordOfServiceNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personRecordOfServiceArchive.RecordOfServiceDate.HasValue)
                    param.Value = personRecordOfServiceArchive.RecordOfServiceDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceCopy";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personRecordOfServiceArchive.RecordOfServiceCopy)
                    param.Value = 1;
                else
                    param.Value = 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceComment";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (personRecordOfServiceArchive.RecordOfServiceComment != null)
                    param.Value = personRecordOfServiceArchive.RecordOfServiceComment;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (personRecordOfServiceArchive.RecordOfServiceId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (personRecordOfServiceArchive.RecordOfServiceId == 0)
                    personRecordOfServiceArchive.RecordOfServiceId = DBCommon.GetInt(paramRecordOfServiceID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    PersonUtil.SetPersonModified(personRecordOfServiceArchive.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonRecordOfServiceArchive(int recordOfServiceId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonRecordOfServiceArchive oldPersonRecordOfServiceArchive = GetPersonRecordOfServiceArchive(recordOfServiceId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteRecordOfServiceArchive", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceSeries", oldPersonRecordOfServiceArchive.RecordOfServiceSeries, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceNumber", oldPersonRecordOfServiceArchive.RecordOfServiceNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceDate", CommonFunctions.FormatDate(oldPersonRecordOfServiceArchive.RecordOfServiceDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceCopy", (oldPersonRecordOfServiceArchive.RecordOfServiceCopy ? "1" : "0"), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceComment", oldPersonRecordOfServiceArchive.RecordOfServiceComment, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.RecordOfServiceArchives WHERE RecordOfServiceID = :RecordOfServiceID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RecordOfServiceID", OracleType.Number).Value = recordOfServiceId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
                PersonUtil.SetPersonModified(person.PersonId, currentUser);
            }

            return result;
        }
    }
}
