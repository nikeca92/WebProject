using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.DAL
{
    public class PrintApplicantLetterBlock
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardIssuedBy { get; set; }
        public string IDCardIssueDate { get; set; }
        public string MilitaryDepartmentText { get; set; }
        public string MilitaryDepartmentTextUpper { get; set; }
        public List<PositionBlock> Positions { get; set; }

        public PrintApplicantLetterBlock()
        {
            Positions = new List<PositionBlock>();
        }
    }

    public class PositionBlock
    {        
        public string MilitaryUnitVPN { get; set; }
        public string MilitaryUnitName { get; set; }
        public string PositionName { get; set; }
    }

    public static class PrintApplicantLetterUtil
    {
        public static PrintApplicantLetterBlock GetPrintApplicantLetterBlock(int applicantId, User currentUser)
        {
            PrintApplicantLetterBlock block = new PrintApplicantLetterBlock();  

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IME as FirstName, 
                                      a.FAM as LastName, 
                                      a.EGN as IdentNumber,
                                      b.GenderID, c.GenderName,
                                      b.IDCardNumber, b.IDCardIssuedBy, b.IDCardIssueDate,
                                      e.MilitaryDepartmentName as MilitaryDepartmentText
                               FROM VS_OWNER.VS_LS a
                               LEFT OUTER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Gender c ON b.GenderID = c.GenderID
                               LEFT OUTER JOIN PMIS_APPL.Applicants d ON a.PersonID = d.PersonID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments e ON d.MilitaryDepartmentID = e.MilitaryDepartmentID
                               WHERE d.ApplicantID = :ApplicantID";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.IDCardNumber = dr["IDCardNumber"].ToString();
                    block.IDCardIssuedBy = dr["IDCardIssuedBy"].ToString();
                    block.MilitaryDepartmentText = dr["MilitaryDepartmentText"].ToString();

                    block.MilitaryDepartmentTextUpper = !String.IsNullOrEmpty(block.MilitaryDepartmentText) ? block.MilitaryDepartmentText.ToUpper() : "";

                    DateTime? idIssueDate = null;
                    if (dr["IDCardIssueDate"] is DateTime)
                        idIssueDate = (DateTime)dr["IDCardIssueDate"];
                    block.IDCardIssueDate = idIssueDate.HasValue ? idIssueDate.Value.ToString("d.MM.yyyy") : "";
                                        
                }

                dr.Close();

                SQL = @"SELECT b.PositionName,
                               g.VPN as MilitaryUnitVPN,
                               g.Imees as MilitaryUnitName,
                               a.Seq as Seq                                     
                        FROM PMIS_APPL.ApplicantPositions a
                        INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                        INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                        INNER JOIN PMIS_APPL.VacancyAnnounces d on d.VacancyAnnounceID = b.vacancyannounceid
                        INNER JOIN PMIS_APPL.VacancyAnnouncesStatuses f on f.vacancyannouncesstatusid = d.VacAnnStatusID
                        INNER JOIN UKAZ_OWNER.MIR g ON b.MilitaryUnitID = g.KOD_MIR
                        WHERE c.ApplicantID = :ApplicantID
                        AND (a.ApplicantStatusID IS NULL OR a.ApplicantStatusID IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('DOCUMENTSAPPLIED', 'PARTICIPATIONALLOWED', 'RATED', 'APPOINTED', 'RESERVE')))
                        AND f.vacannstatuskey <>'FINISHED'
                        ORDER BY b.VacancyAnnounceID, b.ResponsibleMilitaryUnitID, a.Seq";

                cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PositionBlock positionBlock = new PositionBlock();

                    positionBlock.PositionName = dr["PositionName"].ToString();
                    positionBlock.MilitaryUnitVPN = dr["MilitaryUnitVPN"].ToString();
                    positionBlock.MilitaryUnitName = dr["MilitaryUnitName"].ToString();

                    block.Positions.Add(positionBlock);
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
