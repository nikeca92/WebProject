using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class DocumentType : IDropDownItem
    {
        public string DocumentTypeKey{get;set;}
        public string DocumentTypeName{get;set;}

        //IDropDownItem Members
        public string Text()
        {
            return DocumentTypeName;
        }

        public string Value()
        {
            return DocumentTypeKey;
        }
    }
    public class DocumentTypeUtil
    {
        public static DocumentType GetDocumentType(string documentTypeKey, User currentUser)
        {
            DocumentType DocumentType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RV_Low_Value as DocumentTypeKey,
                                      a.RV_Meaning as DocumentTypeName
                               FROM VS_OWNER.CG_REF_CODES a
                               WHERE a.RV_Domain = 'VID_DOG' 
                                     AND a.RV_Low_Value = :DocumentTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DocumentTypeKey", OracleType.VarChar).Value = documentTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    DocumentType = new DocumentType();
                    DocumentType.DocumentTypeKey = documentTypeKey;
                    DocumentType.DocumentTypeName = dr["DocumentTypeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return DocumentType;
        }

        public static List<DocumentType> GetAllDocumentType(User currentUser)
        {
            List<DocumentType> listDocumentType = new List<DocumentType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
              
                string SQL = @"SELECT a.RV_Low_Value as DocumentTypeKey,
                                      a.RV_Meaning as DocumentTypeName
                               FROM VS_OWNER.CG_REF_CODES a
                               WHERE a.RV_Domain = 'VID_DOG' 
                               ORDER BY a.RV_Meaning ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DocumentType DocumentType = new DocumentType();
                    DocumentType.DocumentTypeKey = dr["DocumentTypeKey"].ToString();
                    DocumentType.DocumentTypeName = dr["DocumentTypeName"].ToString();
                    listDocumentType.Add(DocumentType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDocumentType;
        }
    }
}

