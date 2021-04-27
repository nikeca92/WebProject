using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Common
{
    public class Training
    {
        private int trainingId;        
        private DateTime? trainingDate;        
        private int? trainingYear;        
        private string trainingDesc;        
        private string legalRef;

        public int TrainingId
        {
            get { return trainingId; }
            set { trainingId = value; }
        }

        public DateTime? TrainingDate
        {
            get { return trainingDate; }
            set { trainingDate = value; }
        }

        public int? TrainingYear
        {
            get { return trainingYear; }
            set { trainingYear = value; }
        }

        public string TrainingDesc
        {
            get { return trainingDesc; }
            set { trainingDesc = value; }
        }

        public string LegalRef
        {
            get { return legalRef; }
            set { legalRef = value; }
        }
    }

    public static class TrainingUtil
    {
        private static Training ExtractTrainingFromDR(OracleDataReader dr)
        {
            Training training = new Training();

            training.TrainingId = (DBCommon.IsInt(dr["TrainingHistoryID"]) ? DBCommon.GetInt(dr["TrainingHistoryID"]) : 0);
            training.TrainingDate = (dr["TrainingDate"] is DateTime) ? (DateTime?)dr["TrainingDate"] : null;
            training.TrainingYear = (DBCommon.IsInt(dr["TrainingYear"]) ? (int?)DBCommon.GetInt(dr["TrainingYear"]) : null);
            training.TrainingDesc = dr["TrainingDesc"].ToString();
            training.LegalRef = dr["LegalRef"].ToString();

            return training;
        }

        public static Training GetTraining(int trainingId, User currentUser)
        {
            Training training = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TrainingHistoryID as TrainingHistoryID, 
                                      a.TrainingDate as TrainingDate, 
                                      a.TrainingYear as TrainingYear, 
                                      a.TrainingDesc as TrainingDesc,
                                      a.LegalRef as LegalRef
                               FROM PMIS_HS.TrainingHistory a                       
                               WHERE a.TrainingHistoryID = :TrainingHistoryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TrainingHistoryID", OracleType.Number).Value = trainingId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    training = ExtractTrainingFromDR(dr);              
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return training;
        }

        public static Training GetLastTrainingByPerson(int personId, User currentUser)
        {
            Training training = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TrainingHistoryID as TrainingHistoryID, 
                                      a.TrainingDate as TrainingDate, 
                                      a.TrainingYear as TrainingYear, 
                                      a.TrainingDesc as TrainingDesc,
                                      a.LegalRef as LegalRef
                               FROM (
                                        SELECT a.TrainingHistoryID as TrainingHistoryID, 
                                        a.TrainingDate as TrainingDate, 
                                        a.TrainingYear as TrainingYear, 
                                        a.TrainingDesc as TrainingDesc,
                                        a.LegalRef as LegalRef,
                                        row_number() over (ORDER BY a.TrainingDate DESC) as RowNumber
                                        FROM PMIS_HS.TrainingHistory a                       
                                        WHERE a.PersonID = :PersonID
                                    ) a
                                WHERE a.RowNumber = 1";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    training = ExtractTrainingFromDR(dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return training;
        }
        
        public static List<Training> GetAllTrainingsByPerson(int personId, User currentUser)
        {
            List<Training> trainings = new List<Training>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TrainingHistoryID as TrainingHistoryID, 
                                      a.TrainingDate as TrainingDate, 
                                      a.TrainingYear as TrainingYear, 
                                      a.TrainingDesc as TrainingDesc,
                                      a.LegalRef as LegalRef
                               FROM PMIS_HS.TrainingHistory a                       
                               WHERE a.PersonID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["TrainingHistoryID"]))
                        trainings.Add(ExtractTrainingFromDR(dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trainings;
        }

        public static int GetAllTrainingsByPersonCount(int personId, User currentUser)
        {
            int personsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_HS.TrainingHistory a                       
                               WHERE a.PersonID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        personsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personsCnt;
        }

        public static List<Training> GetAllTrainingsByPerson(int personId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<Training> trainings = new List<Training>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "a.TrainingDate";
                        break;
                    case 2:
                        orderBySQL = "a.TrainingYear";
                        break;
                    case 3:
                        orderBySQL = "a.TrainingDesc";
                        break;
                    case 4:
                        orderBySQL = "a.LegalRef";
                        break;                   
                    default:
                        orderBySQL = "a.TrainingDate";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.TrainingHistoryID as TrainingHistoryID, 
                                      tmp.TrainingDate as TrainingDate, 
                                      tmp.TrainingYear as TrainingYear, 
                                      tmp.TrainingDesc as TrainingDesc,
                                      tmp.LegalRef as LegalRef,
                                      tmp.RowNumber as RowNumber  FROM (
                                              SELECT a.TrainingHistoryID as TrainingHistoryID, 
                                                  a.TrainingDate as TrainingDate, 
                                                  a.TrainingYear as TrainingYear, 
                                                  a.TrainingDesc as TrainingDesc,
                                                  a.LegalRef as LegalRef,                                          
                                                     RANK() OVER (ORDER BY " + orderBySQL + @", a.TrainingHistoryID) as RowNumber 
                                              FROM PMIS_HS.TrainingHistory a                                                     
                                              WHERE a.PersonID = :PersonID    
                                              ORDER BY " + orderBySQL + @", TrainingHistoryID                             
                                           ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["TrainingHistoryID"]))
                        trainings.Add(ExtractTrainingFromDR(dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trainings;
        }

        public static bool SaveTraining(int personId, Training training, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Person person = PersonUtil.GetPerson(personId, currentUser);

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (training.TrainingId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.TrainingHistory (PersonID, TrainingDate, TrainingYear, TrainingDesc, LegalRef)
                            VALUES (:PersonID, :TrainingDate, :TrainingYear, :TrainingDesc, :LegalRef);

                            SELECT PMIS_HS.TrainingHistory_ID_SEQ.currval INTO :TrainingID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_TrainingHistory_AddTraining", person.FullName, null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDate", "", CommonFunctions.FormatDate(training.TrainingDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingYear", "", training.TrainingYear.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDesc", "", training.TrainingDesc, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_LegalRef", "", training.LegalRef, currentUser));

                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.TrainingHistory SET
                               PersonID = :PersonID, 
                               TrainingDate = :TrainingDate, 
                               TrainingYear = :TrainingYear, 
                               TrainingDesc = :TrainingDesc, 
                               LegalRef = :LegalRef
                            WHERE TrainingHistoryID = :TrainingID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_TrainingHistory_EditTraining", person.FullName, null, person, currentUser);

                    Training oldTraining = TrainingUtil.GetTraining(training.TrainingId, currentUser);

                    if (oldTraining != null)
                    {
                        if (!CommonFunctions.IsEqual(oldTraining.TrainingDate, training.TrainingDate))
                            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDate", CommonFunctions.FormatDate(oldTraining.TrainingDate), CommonFunctions.FormatDate(training.TrainingDate), currentUser));

                        if (oldTraining.TrainingYear != training.TrainingYear)
                            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingYear", oldTraining.TrainingYear.ToString(), training.TrainingYear.ToString(), currentUser));

                        if (oldTraining.TrainingDesc.Trim() != training.TrainingDesc.Trim())
                            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDesc", oldTraining.TrainingDesc, training.TrainingDesc, currentUser));

                        if (oldTraining.LegalRef.Trim() != training.LegalRef.Trim())
                            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_LegalRef", oldTraining.LegalRef, training.LegalRef, currentUser));
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTrainingID = new OracleParameter();
                paramTrainingID.ParameterName = "TrainingID";
                paramTrainingID.OracleType = OracleType.Number;

                if (training.TrainingId!= 0)
                {
                    paramTrainingID.Direction = ParameterDirection.Input;
                    paramTrainingID.Value = training.TrainingId;
                }
                else
                {
                    paramTrainingID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTrainingID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TrainingDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (training.TrainingDate.HasValue)
                    param.Value = training.TrainingDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                

                param = new OracleParameter();
                param.ParameterName = "TrainingYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (training.TrainingYear.HasValue)
                    param.Value = training.TrainingYear.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                              

                param = new OracleParameter();
                param.ParameterName = "TrainingDesc";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(training.TrainingDesc))
                    param.Value = training.TrainingDesc;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LegalRef";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(training.LegalRef))
                    param.Value = training.LegalRef;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (training.TrainingId == 0)
                {
                    training.TrainingId = DBCommon.GetInt(paramTrainingID.Value);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteTraining(int trainingId, int personId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Person person = PersonUtil.GetPerson(personId, currentUser);

            Training oldTraining = TrainingUtil.GetTraining(trainingId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_TrainingHistory_DeleteTraining", person.FullName, null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDate", CommonFunctions.FormatDate(oldTraining.TrainingDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingYear", oldTraining.TrainingYear.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_TrainingDesc", oldTraining.TrainingDesc, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_TrainingHistory_LegalRef", oldTraining.LegalRef, "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.TrainingHistory WHERE TrainingHistoryID = :TrainingID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TrainingID", OracleType.Number).Value = trainingId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }
    }
}
