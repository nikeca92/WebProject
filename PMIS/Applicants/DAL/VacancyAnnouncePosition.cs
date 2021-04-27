using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.Common
{
    public class VacancyAnnouncePosition : BaseDbObject
    {
        private int vacancyAnnouncePositionID;
        private int vacancyAnnounceID;        
        private string mandatoryRequirements;        
        private string additionalRequirements;        
        private string specificRequirements;        
        private int? responsibleMilitaryUnitID;        
        private MilitaryUnit responsibleMilitaryUnit;        
        private string competitionPlaceAndDate;        
        private string contactPhone;        
        private int? positionStatusID;
        private VacancyAnnouncePosStatus positionStatus;        
        private int? militaryUnitID;        
        private MilitaryUnit militaryUnit;        
        private string positionName;        
        private string positionCode;        
        private int? educationId;
        private Education education;        
        private string clInformationAccLevelNATO;        
        private string clInformationAccLevelBG;        
        private string clInformationAccLevelEU;        
        private int positionsCnt;
        private List<MilitaryRank> militaryRanks;

        public int VacancyAnnouncePositionID
        {
            get { return vacancyAnnouncePositionID; }
            set { vacancyAnnouncePositionID = value; }
        }

        public int VacancyAnnounceID
        {
            get { return vacancyAnnounceID; }
            set { vacancyAnnounceID = value; }
        }

        public string MandatoryRequirements
        {
            get { return mandatoryRequirements; }
            set { mandatoryRequirements = value; }
        }

        public string AdditionalRequirements
        {
            get { return additionalRequirements; }
            set { additionalRequirements = value; }
        }

        public string SpecificRequirements
        {
            get { return specificRequirements; }
            set { specificRequirements = value; }
        }

        public int? ResponsibleMilitaryUnitID
        {
            get { return responsibleMilitaryUnitID; }
            set { responsibleMilitaryUnitID = value; }
        }

        public MilitaryUnit ResponsibleMilitaryUnit
        {
            get 
            {
                if (responsibleMilitaryUnit == null && responsibleMilitaryUnitID.HasValue)
                    responsibleMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(responsibleMilitaryUnitID.Value, CurrentUser);

                return responsibleMilitaryUnit; 
            }
            set { responsibleMilitaryUnit = value; }
        }

        public string CompetitionPlaceAndDate
        {
            get { return competitionPlaceAndDate; }
            set { competitionPlaceAndDate = value; }
        }

        public string ContactPhone
        {
            get { return contactPhone; }
            set { contactPhone = value; }
        }

        public int? PositionStatusID
        {
            get { return positionStatusID; }
            set { positionStatusID = value; }
        }

        public VacancyAnnouncePosStatus PositionStatus
        {
            get 
            {
                if (positionStatus == null && positionStatusID.HasValue)
                    positionStatus = VacancyAnnouncePosStatusUtil.GetVacancyAnnouncePosStatus(positionStatusID.Value, CurrentUser);

                return positionStatus; 
            }
            set { positionStatus = value; }
        }

        public int? MilitaryUnitID
        {
            get { return militaryUnitID; }
            set { militaryUnitID = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get 
            {
                if (militaryUnit == null && militaryUnitID.HasValue)
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitID.Value, CurrentUser);

                return militaryUnit; 
            }
            set { militaryUnit = value; }
        }

        public string PositionName
        {
            get { return positionName; }
            set { positionName = value; }
        }

        public string PositionCode
        {
            get { return positionCode; }
            set { positionCode = value; }
        }

        public int? EducationId
        {
            get { return educationId; }
            set { educationId = value; }
        }

        public Education Education
        {
            get 
            {
                if (education == null && educationId.HasValue)
                    education = EducationUtil.GetEducation(educationId.Value, CurrentUser);

                return education; 
            }
            set { education = value; }
        }

        public string ClInformationAccLevelNATO
        {
            get { return clInformationAccLevelNATO; }
            set { clInformationAccLevelNATO = value; }
        }

        public string ClInformationAccLevelBG
        {
            get { return clInformationAccLevelBG; }
            set { clInformationAccLevelBG = value; }
        }

        public string ClInformationAccLevelEU
        {
            get { return clInformationAccLevelEU; }
            set { clInformationAccLevelEU = value; }
        }

        public int PositionsCnt
        {
            get { return positionsCnt; }
            set { positionsCnt = value; }
        }

        public List<MilitaryRank> MilitaryRanks
        {
            get
            {   //Lazy initialization. Use it only when the list of specialities isn't already loaded
                //When loading the entire list of position we pull the specialities too
                if (militaryRanks == null)
                    militaryRanks = VacancyAnnouncePositionUtil.GetMilitaryRanksForPosition(VacancyAnnouncePositionID, CurrentUser);

                return militaryRanks;
            }
            set { militaryRanks = value; }
        }

        public string MilitaryRanksString
        {
            get
            {
                string militaryRanksString = "";

                foreach (MilitaryRank rank in MilitaryRanks)
                {
                    militaryRanksString += (militaryRanksString == "" ? "" : ", ") + rank.LongName;
                }

                return militaryRanksString;
            }
        }

        public VacancyAnnouncePosition(User user)
            : base(user)
        {
        }
    }

    public static class VacancyAnnouncePositionUtil
    {
        private static VacancyAnnouncePosition ExtractVacancyAnnouncePositionFromDR(OracleDataReader dr, User currentUser)
        {
            VacancyAnnouncePosition vacancyAnnouncePosition = new VacancyAnnouncePosition(currentUser);

            vacancyAnnouncePosition.VacancyAnnouncePositionID = DBCommon.GetInt(dr["VacancyAnnouncePositionID"]);
            vacancyAnnouncePosition.VacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
            vacancyAnnouncePosition.MandatoryRequirements = dr["MandatoryRequirements"].ToString();
            vacancyAnnouncePosition.AdditionalRequirements = dr["AdditionalRequirements"].ToString();
            vacancyAnnouncePosition.SpecificRequirements = dr["SpecificRequirements"].ToString();
            vacancyAnnouncePosition.ResponsibleMilitaryUnitID = (DBCommon.IsInt(dr["ResponsibleMilitaryUnitID"]) ? (int?)DBCommon.GetInt(dr["ResponsibleMilitaryUnitID"]) : null);
            vacancyAnnouncePosition.CompetitionPlaceAndDate = dr["CompetitionPlaceAndDate"].ToString();
            vacancyAnnouncePosition.ContactPhone = dr["ContactPhone"].ToString();
            vacancyAnnouncePosition.PositionStatusID = (DBCommon.IsInt(dr["PositionStatusID"]) ? (int?)DBCommon.GetInt(dr["PositionStatusID"]) : null);
            vacancyAnnouncePosition.MilitaryUnitID = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? (int?)DBCommon.GetInt(dr["MilitaryUnitID"]) : null);
            vacancyAnnouncePosition.PositionName = dr["PositionName"].ToString();
            vacancyAnnouncePosition.PositionCode = dr["PositionCode"].ToString();
            vacancyAnnouncePosition.EducationId = (DBCommon.IsInt(dr["EducationID"]) ? (int?)DBCommon.GetInt(dr["EducationID"]) : null);
            vacancyAnnouncePosition.ClInformationAccLevelNATO = dr["ClInformationAccLevelNATO"].ToString();
            vacancyAnnouncePosition.ClInformationAccLevelBG = dr["ClInformationAccLevelBG"].ToString();
            vacancyAnnouncePosition.ClInformationAccLevelEU = dr["ClInformationAccLevelEU"].ToString();
            vacancyAnnouncePosition.PositionsCnt = (DBCommon.IsInt(dr["PositionsCnt"]) ? DBCommon.GetInt(dr["PositionsCnt"]) : 0);
          
            return vacancyAnnouncePosition;
        }

        public static VacancyAnnouncePosition GetVacancyAnnouncePosition(int vacancyAnnouncePositionID, User currentUser)
        {
            VacancyAnnouncePosition vacancyAnnouncePosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePositionID,
                                      a.VacancyAnnounceID,
                                      a.MandatoryRequirements,
                                      a.AdditionalRequirements,
                                      a.SpecificRequirements,
                                      a.ResponsibleMilitaryUnitID,
                                      a.CompetitionPlaceAndDate,
                                      a.ContactPhone,
                                      a.PositionStatusID,
                                      a.MilitaryUnitID,
                                      a.PositionName,
                                      a.PositionCode,
                                      a.EducationID,
                                      a.ClInformationAccLevelNATO,
                                      a.ClInformationAccLevelBG,
                                      a.ClInformationAccLevelEU,
                                      a.PositionsCnt                                  
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnouncePositionID = :VacancyAnnouncePositionID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnouncePosition = ExtractVacancyAnnouncePositionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePosition;
        }

        public static VacancyAnnouncePosition GetVacancyAnnouncePositionForNomination(int vacancyAnnounceId, int respMilitaryUnitId, int vacancyAnnouncePositionId, int militaryUnitId, User currentUser)
        {
            VacancyAnnouncePosition vacancyAnnouncePosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePositionID,
                                      a.VacancyAnnounceID,
                                      a.MandatoryRequirements,
                                      a.AdditionalRequirements,
                                      a.SpecificRequirements,
                                      a.ResponsibleMilitaryUnitID,
                                      a.CompetitionPlaceAndDate,
                                      a.ContactPhone,
                                      a.PositionStatusID,
                                      a.MilitaryUnitID,
                                      a.PositionName,
                                      a.PositionCode,
                                      a.EducationID,
                                      a.ClInformationAccLevelNATO,
                                      a.ClInformationAccLevelBG,
                                      a.ClInformationAccLevelEU,
                                      a.PositionsCnt                                     
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID AND 
                                     a.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID AND 
                                     a.VacancyAnnouncePositionID = :VacancyAnnouncePositionID AND
                                     a.MilitaryUnitID = :MilitaryUnitID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = respMilitaryUnitId;
                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.VarChar).Value = vacancyAnnouncePositionId;
                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnouncePosition = ExtractVacancyAnnouncePositionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePosition;
        }

        public static List<VacancyAnnouncePosition> GetAllVacancyAnnouncePositionsByVacancyAnnounce(int vacancyAnnounceId, User currentUser)
        {
            List<VacancyAnnouncePosition> vacancyAnnouncePositions = new List<VacancyAnnouncePosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VacancyAnnouncePositionID,
                                      a.VacancyAnnounceID,
                                      a.MandatoryRequirements,
                                      a.AdditionalRequirements,
                                      a.SpecificRequirements,
                                      a.ResponsibleMilitaryUnitID,
                                      a.CompetitionPlaceAndDate,
                                      a.ContactPhone,
                                      a.PositionStatusID,
                                      a.MilitaryUnitID,
                                      a.PositionName,
                                      a.PositionCode,
                                      a.EducationID,
                                      a.ClInformationAccLevelNATO,
                                      a.ClInformationAccLevelBG,
                                      a.ClInformationAccLevelEU,
                                      a.PositionsCnt
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                SQL += " ORDER BY a.VacancyAnnouncePositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["VacancyAnnouncePositionID"]))
                        vacancyAnnouncePositions.Add(ExtractVacancyAnnouncePositionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePositions;
        }

        public static List<VacancyAnnouncePosition> GetAllVacancyAnnouncePositionsForApplicant(int applicantID, string respMilitaryUnitIDs, string militaryUnitIDs, string positionName, string orderNum, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VacancyAnnouncePosition> vacancyAnnouncePositions = new List<VacancyAnnouncePosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(positionName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.PositionName) LIKE UPPER('%" + positionName.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(orderNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(b.OrderNum) LIKE UPPER('%" + orderNum.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(respMilitaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ResponsibleMilitaryUnitID IN (" + respMilitaryUnitIDs + @")";
                }

                if (!String.IsNullOrEmpty(militaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + militaryUnitIDs + @")";
                }

                where += (where == "" ? "" : " AND ") +
                             " a.VacancyAnnouncePositionID NOT IN (SELECT VacancyAnnouncePositionID FROM PMIS_APPL.ApplicantPositions WHERE ApplicantID = " + applicantID.ToString() + " )";

                where += (where == "" ? "" : " AND ") +
                             @" (b.MaxPositions IS NULL OR b.MaxPositions > (SELECT COUNT(*) FROM PMIS_APPL.ApplicantPositions x
                                                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions y ON x.VacancyAnnouncePositionID = y.VacancyAnnouncePositionID
                                                                  INNER JOIN PMIS_APPL.VacancyAnnounces z ON y.VacancyAnnounceID = z.VacancyAnnounceID
                                                                  WHERE x.ApplicantID = " + applicantID.ToString() + " AND z.VacancyAnnounceID = b.VacancyAnnounceID))";


                //Allow to apply only for Vacancy Announce in status DOCUMENTS
                where += (where == "" ? "" : " AND ") +
                             " d.VacAnnStatusKey = 'DOCUMENTS'";

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "b.OrderNum";
                        break;
                    case 2:
                        orderBySQL = "c.IMEES";
                        break;
                    case 3:
                        orderBySQL = "a.PositionName";
                        break;     
                    case 4:
                        orderBySQL = "a.MandatoryRequirements";
                        break;
                    case 5:
                        orderBySQL = "a.AdditionalRequirements";
                        break;
                    case 6:
                        orderBySQL = "a.SpecificRequirements";
                        break;
                    default:
                        orderBySQL = "b.OrderNum";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @" SELECT tmp.VacancyAnnouncePositionID,
                                      tmp.VacancyAnnounceID,
                                      tmp.MandatoryRequirements,
                                      tmp.AdditionalRequirements,
                                      tmp.SpecificRequirements,
                                      tmp.ResponsibleMilitaryUnitID,
                                      tmp.CompetitionPlaceAndDate,
                                      tmp.ContactPhone,
                                      tmp.PositionStatusID,
                                      tmp.MilitaryUnitID,
                                      tmp.PositionName,
                                      tmp.PositionCode,
                                      tmp.EducationID,
                                      tmp.ClInformationAccLevelNATO,
                                      tmp.ClInformationAccLevelBG,
                                      tmp.ClInformationAccLevelEU,
                                      tmp.PositionsCnt                                       
                                 FROM (
                                        SELECT a.VacancyAnnouncePositionID,
                                               a.VacancyAnnounceID,
                                               a.MandatoryRequirements,
                                               a.AdditionalRequirements,
                                               a.SpecificRequirements,
                                               a.ResponsibleMilitaryUnitID,
                                               a.CompetitionPlaceAndDate,
                                               a.ContactPhone,
                                               a.PositionStatusID,
                                               a.MilitaryUnitID,
                                               a.PositionName,
                                               a.PositionCode,
                                               a.EducationID,
                                               a.ClInformationAccLevelNATO,
                                               a.ClInformationAccLevelBG,
                                               a.ClInformationAccLevelEU,
                                               a.PositionsCnt as PositionsCnt,
                                               RANK() OVER (ORDER BY " + orderBySQL + @", a.VacancyAnnouncePositionID) as RowNumber
                                     FROM PMIS_APPL.VacancyAnnouncePositions a
                                     INNER JOIN PMIS_APPL.VacancyAnnounces b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                     LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                                     INNER JOIN PMIS_APPL.VacancyAnnouncesStatuses d ON b.VacAnnStatusID = d.VacancyAnnouncesStatusID
                                           " + where + @"                                    
                                     ORDER BY " + orderBySQL + @", a.MilitaryUnitID, a.PositionCode                                                                                         
                                     ) tmp
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacancyAnnouncePositions.Add(ExtractVacancyAnnouncePositionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePositions;
        }

        public static List<ListItem> GetDistinctPositionsByVacancyAnnounceIDAndRespMilitaryUnitID(int vacancyAnnounceID, int respMilitaryUnit, User currentUser)
        {
            List<ListItem> positions = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PositionName, a.VacancyAnnouncePositionID                
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID AND a.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.ResponsibleMilitaryUnitID IS NULL OR a.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceID;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = respMilitaryUnit;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem li = new ListItem(dr["PositionName"].ToString(), dr["VacancyAnnouncePositionID"].ToString());

                    positions.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positions;
        }

        public static List<ListItem> GetDistinctMilitaryUnitsForPosition(int vacancyAnnounceID, int responsibleMilitaryUnitID, int vacancyAnnouncePositionID, User currentUser)
        {
            List<ListItem> militaryUnits = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT a.MilitaryUnitID, b.IMEES as ShortName, b.VPN
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               INNER JOIN UKAZ_OWNER.MIR b ON a.MilitaryUnitID = b.KOD_MIR
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID AND 
                                     a.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID AND
                                     a.VacancyAnnouncePositionID = :VacancyAnnouncePositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceID;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitID;
                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                    {
                        ListItem li = new ListItem();
                        li.Text = dr["VPN"].ToString() + " " + dr["ShortName"].ToString();
                        li.Value = DBCommon.GetInt(dr["MilitaryUnitID"]).ToString();
                        militaryUnits.Add(li);                        
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnits;
        }

        public static int GetAllVacancyAnnouncePositionsForApplicantCount(int applicantId, string respMilitaryUnitIDs, string militaryUnitIDs, string positionName, string orderNum, User currentUser)
        {
            int vacancyAnnouncePositions = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(positionName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.PositionName) LIKE UPPER('%" + positionName.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(orderNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(b.OrderNum) LIKE UPPER('%" + orderNum.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(respMilitaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ResponsibleMilitaryUnitID IN (" + respMilitaryUnitIDs + @")";
                }

                if (!String.IsNullOrEmpty(militaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + militaryUnitIDs + @")";
                }

                where += (where == "" ? "" : " AND ") +
                             " a.VacancyAnnouncePositionID NOT IN (SELECT VacancyAnnouncePositionID FROM PMIS_APPL.ApplicantPositions WHERE ApplicantID = " + applicantId.ToString() + " )";

                where += (where == "" ? "" : " AND ") +
                             @" (b.MaxPositions IS NULL OR b.MaxPositions > (SELECT COUNT(*) FROM PMIS_APPL.ApplicantPositions x
                                                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions y ON x.VacancyAnnouncePositionID = y.VacancyAnnouncePositionID
                                                                  INNER JOIN PMIS_APPL.VacancyAnnounces z ON y.VacancyAnnounceID = z.VacancyAnnounceID
                                                                  WHERE x.ApplicantID = " + applicantId.ToString() + " AND z.VacancyAnnounceID = b.VacancyAnnounceID))";

                //Allow to apply only for Vacancy Announce in status DOCUMENTS
                where += (where == "" ? "" : " AND ") +
                             " d.VacAnnStatusKey = 'DOCUMENTS'";

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
              

                string SQL = @" SELECT COUNT(*) as Cnt
                                FROM PMIS_APPL.VacancyAnnouncePositions a
                                INNER JOIN PMIS_APPL.VacancyAnnounces b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                INNER JOIN PMIS_APPL.VacancyAnnouncesStatuses d ON b.VacAnnStatusID = d.VacancyAnnouncesStatusID
                                LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                                " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        vacancyAnnouncePositions = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncePositions;
        }

        public static string GetPositionCodeByVacancyAnnouncePositionId(int vacancyAnnouncePositionId, User currentUser)
        {
            string positionCode = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PositionCode                
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnouncePositionID = :VacancyAnnouncePositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    positionCode = dr["PositionCode"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positionCode;
        }

        public static bool SaveVacancyAnnouncePositionManually(int vacancyAnnounceId, VacancyAnnouncePosition vacancyAnnouncePosition, User currentUser, Change changeEntry)
        {
            return SaveVacancyAnnouncePosition(vacancyAnnounceId, vacancyAnnouncePosition, currentUser, changeEntry, "APPL_VacAnn_AddPositionManually", "");
        }

        public static bool SaveVacancyAnnouncePosition(int vacancyAnnounceId, VacancyAnnouncePosition vacancyAnnouncePosition, User currentUser, Change changeEntry)
        {
            return SaveVacancyAnnouncePosition(vacancyAnnounceId, vacancyAnnouncePosition, currentUser, changeEntry, "APPL_VacAnn_AddPosition", "APPL_VacAnn_EditPosition");
        }

        public static bool SaveVacancyAnnouncePosition(int vacancyAnnounceId, VacancyAnnouncePosition vacancyAnnouncePosition, User currentUser, Change changeEntry, string addChangeEventKey, string editChagneEventKey)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounceId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                string logDescription = "";
                logDescription += "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);
                logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + ": " + vacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection;
                logDescription += "<br />Код на длъжността: " + vacancyAnnouncePosition.PositionCode;                

                if (vacancyAnnouncePosition.VacancyAnnouncePositionID == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.VacancyAnnouncePositions 
                                            (VacancyAnnounceID,
                                             MandatoryRequirements,
                                             AdditionalRequirements,
                                             SpecificRequirements,
                                             ResponsibleMilitaryUnitID,
                                             CompetitionPlaceAndDate,
                                             ContactPhone,
                                             PositionStatusID,
                                             MilitaryUnitID,
                                             PositionName,
                                             PositionCode,
                                             EducationID,
                                             ClInformationAccLevelNATO,
                                             ClInformationAccLevelBG,
                                             ClInformationAccLevelEU,
                                             PositionsCnt)
                            VALUES          (:VacancyAnnounceID,
                                             :MandatoryRequirements,
                                             :AdditionalRequirements,
                                             :SpecificRequirements,
                                             :ResponsibleMilitaryUnitID,
                                             :CompetitionPlaceAndDate,
                                             :ContactPhone,
                                             :PositionStatusID,
                                             :MilitaryUnitID,
                                             :PositionName,
                                             :PositionCode,
                                             :EducationID,
                                             :ClInformationAccLevelNATO,
                                             :ClInformationAccLevelBG,
                                             :ClInformationAccLevelEU,
                                             :PositionsCnt);

                            SELECT PMIS_APPL.VacAnnouncePositions_ID_SEQ.currval INTO :VacancyAnnouncePositionID FROM dual;

                            ";

                    changeEvent = new ChangeEvent(addChangeEventKey, logDescription, vacancyAnnouncePosition.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryUnit", "", vacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionName", "", vacancyAnnouncePosition.PositionName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionCode", "", vacancyAnnouncePosition.PositionCode, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_Education", "", vacancyAnnouncePosition.Education != null ? vacancyAnnouncePosition.Education.EducationName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelNATO", "", vacancyAnnouncePosition.ClInformationAccLevelNATO, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelBG", "", vacancyAnnouncePosition.ClInformationAccLevelBG, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelEU", "", vacancyAnnouncePosition.ClInformationAccLevelEU, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MandatoryReq", "", vacancyAnnouncePosition.MandatoryRequirements, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AdditionalReq", "", vacancyAnnouncePosition.AdditionalRequirements, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_SpecificReq", "", vacancyAnnouncePosition.SpecificRequirements, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_RespMilitaryUnit", "", vacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? vacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_CompPlaceAndDate", "", vacancyAnnouncePosition.CompetitionPlaceAndDate, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_ContactPhone", "", vacancyAnnouncePosition.ContactPhone, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionsCnt", "", vacancyAnnouncePosition.PositionsCnt.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryRank", "", vacancyAnnouncePosition.MilitaryRanksString, currentUser));                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.VacancyAnnouncePositions SET
                               VacancyAnnounceID = :VacancyAnnounceID,
                               MandatoryRequirements = :MandatoryRequirements,
                               AdditionalRequirements = :AdditionalRequirements,
                               SpecificRequirements = :SpecificRequirements,
                               ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID,
                               CompetitionPlaceAndDate = :CompetitionPlaceAndDate,
                               ContactPhone = :ContactPhone,
                               PositionStatusID = :PositionStatusID,
                               MilitaryUnitID = :MilitaryUnitID,
                               PositionName = :PositionName,
                               PositionCode = :PositionCode,
                               EducationID = :EducationID,
                               ClInformationAccLevelNATO = :ClInformationAccLevelNATO,
                               ClInformationAccLevelBG = :ClInformationAccLevelBG,
                               ClInformationAccLevelEU = :ClInformationAccLevelEU,
                               PositionsCnt = :PositionsCnt
                            WHERE VacancyAnnouncePositionID = :VacancyAnnouncePositionID ;                            

                            ";

                    changeEvent = new ChangeEvent(editChagneEventKey, logDescription, vacancyAnnouncePosition.MilitaryUnit, null, currentUser);

                    VacancyAnnouncePosition oldVacancyAnnouncePosition = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(vacancyAnnouncePosition.VacancyAnnouncePositionID, currentUser);

                    if (oldVacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection.Trim() != vacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryUnit", oldVacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection, vacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection, currentUser));

                    if (oldVacancyAnnouncePosition.PositionName.Trim() != vacancyAnnouncePosition.PositionName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionName", oldVacancyAnnouncePosition.PositionName, vacancyAnnouncePosition.PositionName, currentUser));

                    if (oldVacancyAnnouncePosition.PositionCode.Trim() != vacancyAnnouncePosition.PositionCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionCode", oldVacancyAnnouncePosition.PositionCode, vacancyAnnouncePosition.PositionCode, currentUser));

                    if ((oldVacancyAnnouncePosition.Education != null ? oldVacancyAnnouncePosition.Education.EducationName.Trim() : "") != (vacancyAnnouncePosition.Education != null ? vacancyAnnouncePosition.Education.EducationName.Trim() : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_Education", oldVacancyAnnouncePosition.Education != null ? oldVacancyAnnouncePosition.Education.EducationName : "" , vacancyAnnouncePosition.Education != null ? vacancyAnnouncePosition.Education.EducationName : "", currentUser));

                    if (oldVacancyAnnouncePosition.ClInformationAccLevelNATO.Trim() != vacancyAnnouncePosition.ClInformationAccLevelNATO.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelNATO", oldVacancyAnnouncePosition.ClInformationAccLevelNATO, vacancyAnnouncePosition.ClInformationAccLevelNATO, currentUser));

                    if (oldVacancyAnnouncePosition.ClInformationAccLevelBG.Trim() != vacancyAnnouncePosition.ClInformationAccLevelBG.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelBG", oldVacancyAnnouncePosition.ClInformationAccLevelBG, vacancyAnnouncePosition.ClInformationAccLevelBG, currentUser));

                    if (oldVacancyAnnouncePosition.ClInformationAccLevelEU.Trim() != vacancyAnnouncePosition.ClInformationAccLevelEU.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelEU", oldVacancyAnnouncePosition.ClInformationAccLevelEU, vacancyAnnouncePosition.ClInformationAccLevelEU, currentUser));

                    if (oldVacancyAnnouncePosition.MandatoryRequirements.Trim() != vacancyAnnouncePosition.MandatoryRequirements.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MandatoryReq", oldVacancyAnnouncePosition.MandatoryRequirements, vacancyAnnouncePosition.MandatoryRequirements, currentUser));

                    if (oldVacancyAnnouncePosition.AdditionalRequirements.Trim() != vacancyAnnouncePosition.AdditionalRequirements.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AdditionalReq", oldVacancyAnnouncePosition.AdditionalRequirements, vacancyAnnouncePosition.AdditionalRequirements, currentUser));

                    if (oldVacancyAnnouncePosition.SpecificRequirements.Trim() != vacancyAnnouncePosition.SpecificRequirements.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_SpecificReq", oldVacancyAnnouncePosition.SpecificRequirements, vacancyAnnouncePosition.SpecificRequirements, currentUser));

                    if ((oldVacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? oldVacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection.Trim() : "") != (vacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? vacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection.Trim() : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_RespMilitaryUnit", oldVacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? oldVacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "", vacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? vacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "", currentUser));

                    if (oldVacancyAnnouncePosition.CompetitionPlaceAndDate.Trim() != vacancyAnnouncePosition.CompetitionPlaceAndDate.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_CompPlaceAndDate", oldVacancyAnnouncePosition.CompetitionPlaceAndDate, vacancyAnnouncePosition.CompetitionPlaceAndDate, currentUser));

                    if (oldVacancyAnnouncePosition.ContactPhone.Trim() != vacancyAnnouncePosition.ContactPhone.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_ContactPhone", oldVacancyAnnouncePosition.ContactPhone, vacancyAnnouncePosition.ContactPhone, currentUser));

                    if (oldVacancyAnnouncePosition.PositionsCnt != vacancyAnnouncePosition.PositionsCnt)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionsCnt", oldVacancyAnnouncePosition.PositionsCnt.ToString(), vacancyAnnouncePosition.PositionsCnt.ToString(), currentUser));

                    if (oldVacancyAnnouncePosition.MilitaryRanksString.Trim() != vacancyAnnouncePosition.MilitaryRanksString.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryRank", oldVacancyAnnouncePosition.MilitaryRanksString, vacancyAnnouncePosition.MilitaryRanksString, currentUser));
                }

                SQL += @"DELETE FROM PMIS_APPL.PositionMilitaryRanks 
                         WHERE VacancyAnnouncePositionID = :VacancyAnnouncePositionID;

                        ";

                foreach (MilitaryRank militaryRank in vacancyAnnouncePosition.MilitaryRanks)
                {
                    SQL += @"INSERT INTO PMIS_APPL.PositionMilitaryRanks (VacancyAnnouncePositionID, MilitaryRankID)
                             VALUES (:VacancyAnnouncePositionID, " + militaryRank.MilitaryRankId.ToString() + @");
                            ";

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVacancyAnnouncePositionID = new OracleParameter();
                paramVacancyAnnouncePositionID.ParameterName = "VacancyAnnouncePositionID";
                paramVacancyAnnouncePositionID.OracleType = OracleType.Number;

                if (vacancyAnnouncePosition.VacancyAnnouncePositionID != 0)
                {
                    paramVacancyAnnouncePositionID.Direction = ParameterDirection.Input;
                    paramVacancyAnnouncePositionID.Value = vacancyAnnouncePosition.VacancyAnnouncePositionID;
                }
                else
                {
                    paramVacancyAnnouncePositionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVacancyAnnouncePositionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MandatoryRequirements";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.MandatoryRequirements))
                    param.Value = vacancyAnnouncePosition.MandatoryRequirements;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdditionalRequirements";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.AdditionalRequirements))
                    param.Value = vacancyAnnouncePosition.AdditionalRequirements;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SpecificRequirements";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.SpecificRequirements))
                    param.Value = vacancyAnnouncePosition.SpecificRequirements;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResponsibleMilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnouncePosition.ResponsibleMilitaryUnitID.HasValue)
                    param.Value = vacancyAnnouncePosition.ResponsibleMilitaryUnitID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CompetitionPlaceAndDate";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.CompetitionPlaceAndDate))
                    param.Value = vacancyAnnouncePosition.CompetitionPlaceAndDate;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactPhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.ContactPhone))
                    param.Value = vacancyAnnouncePosition.ContactPhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnouncePosition.PositionStatusID.HasValue)
                    param.Value = vacancyAnnouncePosition.PositionStatusID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnouncePosition.MilitaryUnitID.HasValue)
                    param.Value = vacancyAnnouncePosition.MilitaryUnitID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.PositionName))
                    param.Value = vacancyAnnouncePosition.PositionName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.PositionCode))
                    param.Value = vacancyAnnouncePosition.PositionCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EducationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnouncePosition.EducationId.HasValue)
                    param.Value = vacancyAnnouncePosition.EducationId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelNATO";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.ClInformationAccLevelNATO))
                    param.Value = vacancyAnnouncePosition.ClInformationAccLevelNATO;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelBG";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.ClInformationAccLevelBG))
                    param.Value = vacancyAnnouncePosition.ClInformationAccLevelBG;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelEU";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnouncePosition.ClInformationAccLevelEU))
                    param.Value = vacancyAnnouncePosition.ClInformationAccLevelEU;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionsCnt";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vacancyAnnouncePosition.PositionsCnt;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (vacancyAnnouncePosition.VacancyAnnouncePositionID == 0)
                {
                    vacancyAnnouncePosition.VacancyAnnouncePositionID = DBCommon.GetInt(paramVacancyAnnouncePositionID.Value);
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
                {
                    changeEntry.AddEvent(changeEvent);
                    VacancyAnnounceUtil.SetVacancyAnnounceModified(vacancyAnnounceId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteVacancyAnnouncePosition(int vacancyAnnouncePositionId, int vacancyAnnounceId, User currentUser, Change changeEntry)
        {
            bool result = false;

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounceId, currentUser);

            VacancyAnnouncePosition oldVacancyAnnouncePosition = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(vacancyAnnouncePositionId, currentUser);

            string logDescription = "";
            logDescription += "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);
            logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + ": " + oldVacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection;
            logDescription += "<br />Код на длъжността: " + oldVacancyAnnouncePosition.PositionCode;

            ChangeEvent changeEvent = new ChangeEvent("APPL_VacAnn_DeletePosition", logDescription, oldVacancyAnnouncePosition.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryUnit", oldVacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionName", oldVacancyAnnouncePosition.PositionName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionCode", oldVacancyAnnouncePosition.PositionCode, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_Education", oldVacancyAnnouncePosition.Education != null ? oldVacancyAnnouncePosition.Education.EducationName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelNATO", oldVacancyAnnouncePosition.ClInformationAccLevelNATO, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelBG", oldVacancyAnnouncePosition.ClInformationAccLevelBG, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AccLevelEU", oldVacancyAnnouncePosition.ClInformationAccLevelEU, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MandatoryReq", oldVacancyAnnouncePosition.MandatoryRequirements, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_AdditionalReq", oldVacancyAnnouncePosition.AdditionalRequirements, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_SpecificReq", oldVacancyAnnouncePosition.SpecificRequirements, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_RespMilitaryUnit", oldVacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? oldVacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_CompPlaceAndDate", oldVacancyAnnouncePosition.CompetitionPlaceAndDate, "",  currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_ContactPhone", oldVacancyAnnouncePosition.ContactPhone, "",  currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_PositionsCnt", oldVacancyAnnouncePosition.PositionsCnt.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnPos_MilitaryRank", oldVacancyAnnouncePosition.MilitaryRanksString, "", currentUser));                    
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                   DELETE FROM PMIS_APPL.PositionMilitaryRanks
                                   WHERE VacancyAnnouncePositionID = :VacancyAnnouncePositionID;

                                   DELETE FROM PMIS_APPL.VacancyAnnouncePositions
                                   WHERE VacancyAnnouncePositionID = :VacancyAnnouncePositionID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionId;

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

        //Get a list of all Military Ranks assigned to a particular vacancy announce position
        public static List<MilitaryRank> GetMilitaryRanksForPosition(int vacancyAnnouncePositionId, User currentUser)
        {
            List<MilitaryRank> militaryRanks = new List<MilitaryRank>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryRankID, b.ZVA_IMEES as MilRankShortName, b.ZVA_IME as MilRankLongName, c.KAT_IME as MilCategoryName
                               FROM PMIS_APPL.PositionMilitaryRanks a
                               INNER JOIN VS_OWNER.KLV_ZVA b ON a.MilitaryRankID = b.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT c ON b.ZVA_KAT_KOD = c.KAT_KOD
                               WHERE a.VacancyAnnouncePositionID = :VacancyAnnouncePositionID
                               ORDER BY b.ZVA_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryRank militaryRank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);

                    militaryRanks.Add(militaryRank);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryRanks;
        }
    }
}