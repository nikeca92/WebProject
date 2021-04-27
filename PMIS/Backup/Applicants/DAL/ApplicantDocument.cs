using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ApplicantDocument : BaseDbObject
    {
        private int? applicantDocumentId;

        private Document document;
        private ApplicantDocumentStatus applicantDocumentStatus;

        public int? ApplicantDocumentId
        {
            get { return applicantDocumentId; }
            set { applicantDocumentId = value; }
        }

        public Document Document
        {
            get { return document; }
            set { document = value; }
        }

        public ApplicantDocumentStatus ApplicantDocumentStatus
        {
            get { return applicantDocumentStatus; }
            set { applicantDocumentStatus = value; }
        }

        public ApplicantDocument(User user)
            : base(user)
        {

        }
    }

    public class ApplicantDocumentUtil
    {
        //This method creates and returns a ApplicantDocument object. It extracts the data from a DataReader.
        public static ApplicantDocument ExtractApplicantDocumentFromDataReader(OracleDataReader dr, User currentUser)
        {
            ApplicantDocument applicantDocument = new ApplicantDocument(currentUser);

            if (DBCommon.IsInt(dr["ApplicantDocumentID"]))
                applicantDocument.ApplicantDocumentId = DBCommon.GetInt(dr["ApplicantDocumentID"]);

            applicantDocument.Document = new Document(currentUser) 
            { 
                DocumentId = DBCommon.GetInt(dr["DocumentID"]), 
                DocumentName = dr["DocumentName"].ToString() 
            };

            if (DBCommon.IsInt(dr["StatusID"]))
            {
                applicantDocument.ApplicantDocumentStatus = new ApplicantDocumentStatus() 
                {
                    StatusId = DBCommon.GetInt(dr["StatusID"]),
                    StatusName = dr["StatusName"].ToString(),
                    StatusKey = dr["StatusKey"].ToString()
                };
            }

            return applicantDocument;
        }

        public static ApplicantDocument GetApplicantDocument(int applicantId, int vacancyAnnounceId, int documentId, User currentUser)
        {
            ApplicantDocument applicantDocument = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT d.ApplicantDocumentID, c.DocumentID, c.DocumentName, e.StatusID, e.StatusName, e.StatusKey
                                FROM PMIS_APPL.VacancyAnnounces a
                                LEFT JOIN PMIS_APPL.VacancyAnnounceDocuments b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                JOIN PMIS_APPL.Documents c ON b.DocumentID = c.DocumentID
                                LEFT OUTER JOIN PMIS_APPL.ApplicantDocuments d ON b.VacancyAnnounceDocumentID = d.VacancyAnnounceDocumentID AND d.ApplicantID = :ApplicantID
                                LEFT OUTER JOIN PMIS_APPL.ApplicantDocumentStatus e ON d.ApplicantDocumentStatusID = e.StatusID
                                WHERE a.VacancyAnnounceID = :VacancyAnnounceID AND c.DocumentID = :DocumentID
                                ORDER BY a.OrderNum, a.VacancyAnnounceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("DocumentID", OracleType.Number).Value = documentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantDocument = ExtractApplicantDocumentFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantDocument;
        }

        public static List<ApplicantDocument> GetApplicantDocumentsForVacancyAnnounce(int applicantId, int vacancyAnnounceId, User currentUser)
        {
            ApplicantDocument applicantDocument;
            List<ApplicantDocument> listApplicantDocuments = new List<ApplicantDocument>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT d.ApplicantDocumentID, c.DocumentID, c.DocumentName, e.StatusID, e.StatusName, e.StatusKey
                                FROM PMIS_APPL.VacancyAnnounces a
                                LEFT JOIN PMIS_APPL.VacancyAnnounceDocuments b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                JOIN PMIS_APPL.Documents c ON b.DocumentID = c.DocumentID
                                LEFT OUTER JOIN PMIS_APPL.ApplicantDocuments d ON b.VacancyAnnounceDocumentID = d.VacancyAnnounceDocumentID AND d.ApplicantID = :ApplicantID
                                LEFT OUTER JOIN PMIS_APPL.ApplicantDocumentStatus e ON d.ApplicantDocumentStatusID = e.StatusID
                                WHERE a.VacancyAnnounceID = :VacancyAnnounceID 
                                ORDER BY c.DocumentName, a.OrderNum, a.VacancyAnnounceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    applicantDocument = ExtractApplicantDocumentFromDataReader(dr, currentUser);
                    listApplicantDocuments.Add(applicantDocument);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listApplicantDocuments;
        }

        public static bool SaveApplicantDocument(ApplicantDocument applicantDocument, int applicantId, 
            int vacancyAnnounceId, User currentUser, Change changeEntry)
        {
            Applicant applicant = ApplicantUtil.GetApplicant(applicantId, currentUser);
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounceId, currentUser);

            if (String.IsNullOrEmpty(applicantDocument.ApplicantDocumentStatus.StatusName))
            {
                applicantDocument.ApplicantDocumentStatus = ApplicantDocumentStatusUtil.GetApplicantDocumentStatus(applicantDocument.ApplicantDocumentStatus.StatusId, currentUser);   
            }

            bool result = false;

            string SQL = "";
            
            string logDescription = "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);

            ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_EditDocument", logDescription, null, applicant.Person, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"
                        DECLARE VacancyAnnounceDocumentID number;

                        BEGIN
                            SELECT VacancyAnnounceDocumentID INTO VacancyAnnounceDocumentID FROM PMIS_APPL.VacancyAnnounceDocuments 
                            WHERE VacancyAnnounceID = :VacancyAnnounceID AND DocumentID = :DocumentID;
                        
                       ";
                if (applicantDocument.ApplicantDocumentId == 0)
                {
                    SQL += @"

                                INSERT INTO PMIS_APPL.ApplicantDocuments (ApplicantID, VacancyAnnounceDocumentID, ApplicantDocumentStatusID)
                                VALUES (:ApplicantID, VacancyAnnounceDocumentID, :ApplicantDocumentStatusID);

                                SELECT PMIS_APPL.ApplicantDocuments_ID_SEQ.currval INTO :ApplicantDocumentID FROM dual;

                            ";

                    

                    ApplicantDocumentStatus oldApplDocStatus = ApplicantDocumentStatusUtil.GetApplicantDocumentStatusByKey("UNKNOWN", currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_DocumentStatus", oldApplDocStatus.StatusName, applicantDocument.ApplicantDocumentStatus.StatusName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.ApplicantDocuments SET
                               ApplicantID = :ApplicantID, 
                               VacancyAnnounceDocumentID = VacancyAnnounceDocumentID, 
                               ApplicantDocumentStatusID = :ApplicantDocumentStatusID
                            WHERE ApplicantDocumentID = :ApplicantDocumentID;                       

                            ";

                    ApplicantDocument oldApplicantDocument = ApplicantDocumentUtil.GetApplicantDocument(applicantId, vacancyAnnounceId, applicantDocument.Document.DocumentId, currentUser);

                    if (oldApplicantDocument.ApplicantDocumentStatus.StatusName.Trim() != applicantDocument.ApplicantDocumentStatus.StatusName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_DocumentStatus", oldApplicantDocument.ApplicantDocumentStatus.StatusName, applicantDocument.ApplicantDocumentStatus.StatusName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramApplicantDocumentID = new OracleParameter();
                paramApplicantDocumentID.ParameterName = "ApplicantDocumentID";
                paramApplicantDocumentID.OracleType = OracleType.Number;

                if (applicantDocument.ApplicantDocumentId != 0)
                {
                    paramApplicantDocumentID.Direction = ParameterDirection.Input;
                    paramApplicantDocumentID.Value = applicantDocument.ApplicantDocumentId;
                }
                else
                {
                    paramApplicantDocumentID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramApplicantDocumentID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ApplicantID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DocumentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantDocument.Document.DocumentId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ApplicantDocumentStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantDocument.ApplicantDocumentStatus.StatusId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (applicantDocument.ApplicantDocumentId == 0)
                    applicantDocument.ApplicantDocumentId = DBCommon.GetInt(paramApplicantDocumentID.Value);

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
                    ApplicantUtil.SetApplicantModified(applicantId, currentUser);
                }
            }

            return result;
        }
    }
}
