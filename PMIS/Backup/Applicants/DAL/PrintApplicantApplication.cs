using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Applicants.DAL
{
    public class PrintApplicantApplicationBlock
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string PermAddress { get; set; }
        public string PermPostCode { get; set; }
        public string CurrAddress { get; set; }
        public string CurrPostCode { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPostCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Email { get; set; }
        public string MilitaryDepartmentTextUpper { get; set; }
        public string ResMilitaryUnitVPN { get; set; }
        public string ResMilitaryUnitNameUpper { get; set; }
        public string VacancyAnnounceNumber { get; set; }
        public string VacancyAnnounceDate { get; set; }
        public string RegNumber { get; set; }
        public string RegDate { get; set; }

        public List<PositionBlock> Positions { get; set; }

        public PrintApplicantApplicationBlock()
        {
            Positions = new List<PositionBlock>();
        }
    }

    public class PrintApplicantApplicationUtil
    {
        public static PrintApplicantApplicationBlock GetPrintApplicantApplicationBlock(int applicantId, int vacancyAnnounceId, int responsibleMilitaryUnitId, User currentUser)
        {
            string dateFormat = "d.MM.yyyy";

            PrintApplicantApplicationBlock block = new PrintApplicantApplicationBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IME as FirstName, 
                                      a.FAM as LastName, 
                                      a.EGN as IdentNumber,
                                      RTRIM(CASE WHEN q.Ime_Obl IS NOT NULL THEN 'обл. ' || q.Ime_Obl || ', ' ELSE '' END || CASE WHEN p.Ime_Obs IS NOT NULL THEN 'общ. ' || p.Ime_Obs || ', ' ELSE '' END || CASE WHEN o.Ime_Nma IS NOT NULL THEN o1.Ime_S || ' ' || o.Ime_Nma || ', ' ELSE '' END || CASE WHEN a.ADRES IS NOT NULL THEN a.ADRES || ', ' ELSE '' END, ', ') as PermAddress,
                                      NVL(a.PermSecondPostCode, o.PK) as PermPostCode,
                                      RTRIM(CASE WHEN z.Ime_Obl IS NOT NULL THEN 'обл. ' || z.Ime_Obl || ', ' ELSE '' END || CASE WHEN w.Ime_Obs IS NOT NULL THEN 'общ. ' || w.Ime_Obs || ', ' ELSE '' END || CASE WHEN v.Ime_Nma IS NOT NULL THEN v1.Ime_S || ' ' || v.Ime_Nma || ', ' ELSE '' END || CASE WHEN a.CurrAddress IS NOT NULL THEN a.CurrAddress || ', ' ELSE '' END, ', ') as CurrAddress,
                                      NVL(a.PresSecondPostCode, v.PK) as CurrPostCode,
                                      RTRIM(CASE WHEN n.Ime_Obl IS NOT NULL THEN 'обл. ' || n.Ime_Obl || ', ' ELSE '' END || CASE WHEN m.Ime_Obs IS NOT NULL THEN 'общ. ' || m.Ime_Obs || ', ' ELSE '' END || CASE WHEN l.Ime_Nma IS NOT NULL THEN l1.Ime_S || ' ' || l.Ime_Nma || ', ' ELSE '' END || CASE WHEN g.AddressText IS NOT NULL THEN g.AddressText || ', ' ELSE '' END, ', ') as ContactAddress,
                                      NVL(g.PostCode, l.PK) as ContactPostCode,                                      
                                      a.Tel as TelephoneNumber,
                                      b.MobilePhone as MobilePhoneNumber,
                                      b.Email as Email,
                                      e.MilitaryDepartmentName as MilitaryDepartmentText,
                                      r.VPN as ResponsibleMilitaryUnitVPN,
                                      r.Imees as ResponsibleMilitaryUnitName
                               FROM VS_OWNER.VS_LS a
                               LEFT OUTER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.PersonAddresses f ON a.PersonID = f.PersonID AND f.AddressType = 'ADR_CONTACT'
                               LEFT OUTER JOIN PMIS_ADM.Addresses g ON f.AddressID = g.AddressID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = a.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM o1 ON o1.KOD_VNM = o.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA v on v.kod_nma = a.CurrAddrCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM v1 ON v1.KOD_VNM = v.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS w on w.kod_obs = v.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL z on z.kod_obl = v.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA l ON l.kod_nma = g.CityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM l1 ON l1.KOD_VNM = l.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS m ON m.kod_obs = l.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL n ON n.kod_obl = l.kod_obl
                               LEFT OUTER JOIN PMIS_APPL.Applicants d ON a.PersonID = d.PersonID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments e ON d.MilitaryDepartmentID = e.MilitaryDepartmentID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR r ON r.KOD_MIR = :ResponsibleMilitaryUnitID
                               WHERE d.ApplicantID = :ApplicantID";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.PermAddress = dr["PermAddress"].ToString();
                    block.PermPostCode = dr["PermPostCode"].ToString();
                    block.CurrAddress = dr["CurrAddress"].ToString();
                    block.CurrPostCode = dr["CurrPostCode"].ToString();
                    block.ContactAddress = dr["ContactAddress"].ToString();
                    block.ContactPostCode = dr["ContactPostCode"].ToString();
                    block.TelephoneNumber = dr["TelephoneNumber"].ToString();
                    block.MobilePhoneNumber = dr["MobilePhoneNumber"].ToString();
                    block.Email = dr["Email"].ToString();
                    block.ResMilitaryUnitVPN = dr["ResponsibleMilitaryUnitVPN"].ToString();

                    string resmilitaryUnitName = dr["ResponsibleMilitaryUnitName"].ToString();
                    block.ResMilitaryUnitNameUpper = !String.IsNullOrEmpty(resmilitaryUnitName) ? resmilitaryUnitName.ToUpper() : "";

                    string militaryDepartmentText = dr["MilitaryDepartmentText"].ToString();
                    block.MilitaryDepartmentTextUpper = !String.IsNullOrEmpty(militaryDepartmentText) ? militaryDepartmentText.ToUpper() : "";
                }

                dr.Close();

                SQL = @"SELECT OrderNum as VacancyAnnounceNumber, 
                               OrderDate as VacancyAnnounceDate
                        FROM PMIS_APPL.VacancyAnnounces 
                        WHERE VacancyAnnounceID = :VacancyAnnounceID";

                cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.VacancyAnnounceNumber = dr["VacancyAnnounceNumber"].ToString();

                    DateTime? vacancyAnnounceDate = null;

                    if (dr["VacancyAnnounceDate"] is DateTime)
                        vacancyAnnounceDate = (DateTime)dr["VacancyAnnounceDate"];

                    block.VacancyAnnounceDate = vacancyAnnounceDate.HasValue ? vacancyAnnounceDate.Value.ToString(dateFormat) : "";                    
                }

                dr.Close();

                SQL = @"SELECT RegisterNumber as RegNumber, 
                               DocumentDate as RegDate
                        FROM PMIS_APPL.Register
                        WHERE ApplicantID = :ApplicantID 
                        AND VacancyAnnounceID = :VacancyAnnounceID 
                        AND ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID";

                cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.RegNumber = dr["RegNumber"].ToString();

                    DateTime? regDate = null;

                    if (dr["RegDate"] is DateTime)
                        regDate = (DateTime)dr["RegDate"];

                    block.RegDate = regDate.HasValue ? regDate.Value.ToString(dateFormat) : "";
                }

                dr.Close();

                string whereMilitaryUnit = " AND b.ResponsibleMilitaryUnitID ";

                if (responsibleMilitaryUnitId != 0)
                    whereMilitaryUnit += "= " + responsibleMilitaryUnitId.ToString();
                else
                    whereMilitaryUnit += "IS NULL ";

                SQL = @"SELECT b.PositionName,
                               g.VPN as MilitaryUnitVPN,
                               g.Imees as MilitaryUnitName,
                               a.Seq as Seq                                    
                        FROM PMIS_APPL.ApplicantPositions a
                        INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                        INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                        INNER JOIN PMIS_APPL.VacancyAnnounces d on d.VacancyAnnounceID = b.VacancyAnnounceID
                        INNER JOIN PMIS_APPL.VacancyAnnouncesStatuses f on f.vacancyannouncesstatusid = d.VacAnnStatusID
                        INNER JOIN UKAZ_OWNER.MIR g ON b.MilitaryUnitID = g.KOD_MIR
                        WHERE c.ApplicantID = :ApplicantID
                        AND d.VacancyAnnounceID = :VacancyAnnounceID
                        AND (a.ApplicantStatusID IS NULL OR a.ApplicantStatusID IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('DOCUMENTSAPPLIED', 'PARTICIPATIONALLOWED', 'RATED', 'APPOINTED', 'RESERVE')))
                        AND f.vacannstatuskey <>'FINISHED'" + whereMilitaryUnit
                        + @" ORDER BY b.VacancyAnnounceID, b.ResponsibleMilitaryUnitID, a.Seq";

                cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

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
