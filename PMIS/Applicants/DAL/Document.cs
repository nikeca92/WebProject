using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Each applicant is identitified as a particular Person and MilitaryDepartment combination
    public class Document : BaseDbObject
    {
        private int documentId;
        private string documentName;

        public int DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }

        public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }

        public Document(User user)
            : base(user)
        {

        }
    }

    public class DocumentUtil
    {
        public static Document GetDocument(int documentId, User currentUser)
        {
            Document document = null;
            if (documentId == 0)
            {
                return document;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.DocumentName as DocumentName FROM PMIS_APPL.Documents a
                               WHERE a.DocumentID = :DocumentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DocumentID", OracleType.Number).Value = documentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    document = new Document(currentUser);
                    document.DocumentId = documentId;
                    document.DocumentName = dr["DocumentName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return document;
        }

        public static List<Document> GetAllDocuments(User currentUser)
        {
            Document document;
            List<Document> lstDocument = new List<Document>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT  a.DocumentID as DocumentID, a.DocumentName as DocumentName FROM PMIS_APPL.Documents a                             
                                order by a.DocumentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    document = new Document(currentUser);
                    document.DocumentId = DBCommon.GetInt(dr["DocumentID"]);
                    document.DocumentName = dr["DocumentName"].ToString();
                    lstDocument.Add(document);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return lstDocument;
        }

        public static List<Document> GetDocumentsForVacancyAnnounce(int vacancyAnnounceId, User currentUser)
        {
            Document document;
            List<Document> lstDocument = new List<Document>();

            if (vacancyAnnounceId == 0)
            {
                return lstDocument;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT  a.DocumentID as DocumentID, a.DocumentName as DocumentName FROM PMIS_APPL.Documents a                             
                                WHERE a.DocumentID in (SELECT DocumentID FROM PMIS_APPL.VacancyAnnounceDocuments 
                                WHERE VacancyAnnounceID = :VacancyAnnounceID)
                                order by a.DocumentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    document = new Document(currentUser);
                    document.DocumentId = DBCommon.GetInt(dr["DocumentID"]);
                    document.DocumentName = dr["DocumentName"].ToString();
                    lstDocument.Add(document);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return lstDocument;
        }

        //Perform DELETE and return true/false
        public static bool DeleteDocument(int documentId, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isDeleted = false;

            //Create Old Exam obect using GetInvestigationProtocol method
            Document oldDocument = GetDocument(documentId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_APPL.Documents WHERE DocumentID = :DocumentID";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "DocumentID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = documentId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            return isDeleted;
        }

    }

}