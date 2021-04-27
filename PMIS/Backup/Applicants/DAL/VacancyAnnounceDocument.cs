using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class VacancyAnnounceDocument : BaseDbObject
    {
        private int vacancyAnnounceDocumentId;
        private int vacancyAnnounceId;
        private int documentId;

        private VacancyAnnounce vacancyAnnounce;
        private Document document;

        public int VacancyAnnounceDocumentId
        {
            get { return vacancyAnnounceDocumentId; }
            set { vacancyAnnounceDocumentId = value; }
        }

        public int VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }

        public int DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }

        public VacancyAnnounce VacancyAnnounce
        {
            get
            {
                if (vacancyAnnounce == null)
                    vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounceId, CurrentUser);
                return vacancyAnnounce;
            }
            set { vacancyAnnounce = value; }
        }

        public Document Document
        {
            get
            {
                if (document == null)
                    document = DocumentUtil.GetDocument(documentId, CurrentUser);
                return document;
            }
            set { document = value; }
        }

        public VacancyAnnounceDocument(User user)
            : base(user)
        {

        }
    }

    public class VacancyAnnounceDocumentUtil
    {
        private static VacancyAnnounceDocument ExtractVacancyAnnounceDocumentFromDR(OracleDataReader dr, User currentUser)
        {
            VacancyAnnounceDocument vacancyAnnounceDocument = new VacancyAnnounceDocument(currentUser);

            vacancyAnnounceDocument.VacancyAnnounceDocumentId = DBCommon.GetInt(dr["VacancyAnnounceDocumentID"]);
            vacancyAnnounceDocument.VacancyAnnounceId = DBCommon.GetInt(dr["VacancyAnnounceID"]);
            vacancyAnnounceDocument.DocumentId = DBCommon.GetInt(dr["DocumentID"]);

            return vacancyAnnounceDocument;
        }

        public static VacancyAnnounceDocument GetVacancyAnnounceDocument(int vacancyAnnounceDocumentId, User currentUser)
        {
            VacancyAnnounceDocument vacancyAnnounceDocument = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnounceDocumentID, a.VacancyAnnounceID, a.DocumentID 
                               FROM PMIS_APPL.VacancyAnnounceDocuments a
                               WHERE a.VacancyAnnounceDocumentID = :VacancyAnnounceDocumentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceDocumentID", OracleType.Number).Value = vacancyAnnounceDocumentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnounceDocument = ExtractVacancyAnnounceDocumentFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounceDocument;
        }

        //Perform ADD value in  Table VACANCYANNOUNCEDOCUMENTS and return true/false
        public static bool AddDocumentForVacancyAnnounce(VacancyAnnounce vacancyAnnounce, Document document, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isAdded = false;
            ChangeEvent changeEvent;

            if (vacancyAnnounce.VacancyAnnounceId == 0) return isAdded;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"INSERT INTO PMIS_APPL.VacancyAnnounceDocuments (
                                VacancyAnnounceID, 
                                DocumentID)
                        VALUES (
                                :VacancyAnnounceID, 
                                :DocumentID)";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = vacancyAnnounce.VacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DocumentID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = document.DocumentId;
                cmd.Parameters.Add(param);

                isAdded = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            if (isAdded)
            {
                //Create obect using log for Add records
                changeEvent = new ChangeEvent("APPL_VacAnn_AddDocument", "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate), null, null, currentUser);
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnDocument_Name", "", document.DocumentName, currentUser));
                //Add Event 
                changeEntry.AddEvent(changeEvent);
            }
            return isAdded;
        }

        //Perform DELETE value from Table VACANCYANNOUNCEDOCUMENTS and return true/false
        public static bool DeleteDocumentForVacancyAnnounce(VacancyAnnounce vacancyAnnounce, Document document, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isDeleted = false;

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_APPL.VacancyAnnounceDocuments WHERE VacancyAnnounceID = :VacancyAnnounceID
                                                                             AND DocumentID = :DocumentID ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = vacancyAnnounce.VacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DocumentID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = document.DocumentId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            if (isDeleted)
            {
                //Create obect using log for Add records
                changeEvent = new ChangeEvent("APPL_VacAnn_DeleteDocument", "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate), null, null, currentUser);
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnDocument_Name", document.DocumentName, "", currentUser));
                //Add Event 
                changeEntry.AddEvent(changeEvent);
            }
            return isDeleted;
        }
    }
}
