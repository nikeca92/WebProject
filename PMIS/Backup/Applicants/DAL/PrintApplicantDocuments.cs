using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Applicants.DAL
{
    public class PrintApplicantDocumentsBlock
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string PermAddress { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardIssuedBy { get; set; }
        public string IDCardIssueDate { get; set; }
    }

    public static class PrintApplicantDocumentsUtil
    {
        public static PrintApplicantDocumentsBlock GetPrintApplicantDocumentsBlock(int personId, User currentUser)
        {
            PrintApplicantDocumentsBlock block = new PrintApplicantDocumentsBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT b.IME as FirstName,
                                      b.FAM as LastName,
                                      b.EGN as IdentNumber,
                                      RTRIM(CASE WHEN q.Ime_Obl IS NOT NULL THEN 'обл. ' || q.Ime_Obl || ', ' ELSE '' END || CASE WHEN p.Ime_Obs IS NOT NULL THEN 'общ. ' || p.Ime_Obs || ', ' ELSE '' END || CASE WHEN o.Ime_Nma IS NOT NULL THEN o1.Ime_S || ' ' || o.Ime_Nma || ', ' ELSE '' END || CASE WHEN b.ADRES IS NOT NULL THEN b.ADRES || ', ' ELSE '' END, ', ') as PermAddress,
                                      o.PK as PermPostCode,
                                      b.PermSecondPostCode,  
                                      c.IDCardNumber, 
                                      c.IDCardIssuedBy, 
                                      c.IDCardIssueDate                                                     
                                   
                               FROM VS_OWNER.VS_LS b 
                               LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = b.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM o1 ON o1.KOD_VNM = o.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl

                               WHERE b.PersonID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.PermAddress = dr["PermAddress"].ToString();

                    string permPostCode = dr["PermPostCode"].ToString();
                    string permSecondPostCode = dr["PermSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(permSecondPostCode))
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(permPostCode) && permPostCode != "0")
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permPostCode;
                    }

                    block.IDCardNumber = dr["IDCardNumber"].ToString();
                    block.IDCardIssuedBy = dr["IDCardIssuedBy"].ToString();

                    DateTime? idIssueDate = null;

                    if (dr["IDCardIssueDate"] is DateTime)
                        idIssueDate = (DateTime)dr["IDCardIssueDate"];

                    block.IDCardIssueDate = idIssueDate.HasValue ? idIssueDate.Value.ToString("d.MM.yyyy") : "";
                }

                dr.Close();

                return block;
            }
            finally
            {
                conn.Close();
            }
        }

    }

}
