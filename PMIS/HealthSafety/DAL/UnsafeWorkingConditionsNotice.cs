using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class UnsafeWorkingConditionsNotice : BaseDbObject
    {
        private int unsafeWConditionsNoticeId;
        private string noticeNumber;
        private DateTime noticeDate;
        private string reportingPersonName;
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;
        private string violationPlace;
        private string responsiblePersonName;
        private int? dangerDegreeId;
        private string descOfUnsafeCondition;
        private string listOfViolatedRequirements;
        private DateTime? riskReducingDueDate;
        private string tempProcedures;
        private string tempProceduresEstResult;
        private string finalProcedures;
        private string finalProceduresEstResult;
        private string additionalInfoContactPerson;
        private string additionalContactInfo;

        private GTableItem dangerDegree;

        public int UnsafeWConditionsNoticeId
        {
            get { return unsafeWConditionsNoticeId; }
            set { unsafeWConditionsNoticeId = value; }
        }

        public string NoticeNumber
        {
            get { return noticeNumber; }
            set { noticeNumber = value; }
        }

        public DateTime NoticeDate
        {
            get { return noticeDate; }
            set { noticeDate = value; }
        }

        public string ReportingPersonName
        {
            get { return reportingPersonName; }
            set { reportingPersonName = value; }
        }

        public int? MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                if (militaryUnit == null && militaryUnitId != null)
                {
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
                }
                return militaryUnit;
            }
            set
            {
                militaryUnit = value;
            }
        }

        public string ViolationPlace
        {
            get { return violationPlace; }
            set { violationPlace = value; }
        }

        public string ResponsiblePersonName
        {
            get { return responsiblePersonName; }
            set { responsiblePersonName = value; }
        }

        public int? DangerDegreeId
        {
            get { return dangerDegreeId; }
            set { dangerDegreeId = value; }
        }

        public string DescOfUnsafeCondition
        {
            get { return descOfUnsafeCondition; }
            set { descOfUnsafeCondition = value; }
        }

        public string ListOfViolatedRequirements
        {
            get { return listOfViolatedRequirements; }
            set { listOfViolatedRequirements = value; }
        }

        public DateTime? RiskReducingDueDate
        {
            get { return riskReducingDueDate; }
            set { riskReducingDueDate = value; }
        }

        public string TempProcedures
        {
            get { return tempProcedures; }
            set { tempProcedures = value; }
        }

        public string TempProceduresEstResult
        {
            get { return tempProceduresEstResult; }
            set { tempProceduresEstResult = value; }
        }

        public string FinalProcedures
        {
            get { return finalProcedures; }
            set { finalProcedures = value; }
        }

        public string FinalProceduresEstResult
        {
            get { return finalProceduresEstResult; }
            set { finalProceduresEstResult = value; }
        }

        public string AdditionalInfoContactPerson
        {
            get { return additionalInfoContactPerson; }
            set { additionalInfoContactPerson = value; }
        }

        public string AdditionalContactInfo
        {
            get { return additionalContactInfo; }
            set { additionalContactInfo = value; }
        }

        public GTableItem DangerDegree
        {
            get
            {
                if (dangerDegree == null && dangerDegreeId != null)
                {
                    dangerDegree = GTableItemUtil.GetTableItem("DegreeOfDanger", (int)dangerDegreeId, ModuleUtil.HS(), CurrentUser);
                }
                return dangerDegree;
            }
            set
            {
                dangerDegree = value;
            }
        }

        public UnsafeWorkingConditionsNotice(User user)
            : base(user)
        {

        }
    }

    public static class UnsafeWorkingConditionsNoticeUtil
    {
        public static UnsafeWorkingConditionsNotice GetUnsafeWorkingConditionsNotice(int unsafeWorkingConditionsNoticeId, User currentUser)
        {
            UnsafeWorkingConditionsNotice unsafeWConditionsNotice = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_UNSAFEWCONDNOTICE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.NoticeNumber, a.NoticeDate, a.ReportingPerson, a.MilitaryUnitID, a.ViolationPlace, 
                                a.ResponsiblePerson, a.DegreeOfDangerID, a.DescOfUnsafeCondition, a.ListOfViolatedRequirements, 
                                a.ProcedForReducingRiskEndDate, a.TempProcedures, a.TempProceduresEstResult, a.FinalProcedures, 
                                a.FinalProceduresEstResult, a.AdditionalInfoContactPerson, a.AdditionalInfoContactInfo,
                                a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_HS.UnsafeWorkingConditionsNotices a
                               WHERE a.UnsafeWConditionsNoticeID = :UnsafeWConditionsNoticeID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UnsafeWConditionsNoticeID", OracleType.Number).Value = unsafeWorkingConditionsNoticeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    unsafeWConditionsNotice = new UnsafeWorkingConditionsNotice(currentUser);
                    unsafeWConditionsNotice.UnsafeWConditionsNoticeId = unsafeWorkingConditionsNoticeId;

                    unsafeWConditionsNotice.NoticeNumber = dr["NoticeNumber"].ToString();

                    if (dr["NoticeDate"] is DateTime)
                        unsafeWConditionsNotice.NoticeDate = (DateTime)dr["NoticeDate"];

                    unsafeWConditionsNotice.ReportingPersonName = dr["ReportingPerson"].ToString();

                    if (dr["MilitaryUnitID"] != null)
                        unsafeWConditionsNotice.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    else
                        unsafeWConditionsNotice.MilitaryUnitId = null;

                    unsafeWConditionsNotice.ViolationPlace = dr["ViolationPlace"].ToString();

                    unsafeWConditionsNotice.ResponsiblePersonName = dr["ResponsiblePerson"].ToString();

                    if (dr["DegreeOfDangerID"] != null)
                        unsafeWConditionsNotice.DangerDegreeId = DBCommon.GetInt(dr["DegreeOfDangerID"]);
                    else
                        unsafeWConditionsNotice.DangerDegreeId = null;

                    unsafeWConditionsNotice.DescOfUnsafeCondition = dr["DescOfUnsafeCondition"].ToString();
                    unsafeWConditionsNotice.ListOfViolatedRequirements = dr["ListOfViolatedRequirements"].ToString();

                    if (dr["ProcedForReducingRiskEndDate"] is DateTime)
                        unsafeWConditionsNotice.RiskReducingDueDate = (DateTime)dr["ProcedForReducingRiskEndDate"];

                    unsafeWConditionsNotice.TempProcedures = dr["TempProcedures"].ToString();
                    unsafeWConditionsNotice.TempProceduresEstResult = dr["TempProceduresEstResult"].ToString();
                    unsafeWConditionsNotice.FinalProcedures = dr["FinalProcedures"].ToString();
                    unsafeWConditionsNotice.FinalProceduresEstResult = dr["FinalProceduresEstResult"].ToString();
                    unsafeWConditionsNotice.AdditionalInfoContactPerson = dr["AdditionalInfoContactPerson"].ToString();
                    unsafeWConditionsNotice.AdditionalContactInfo = dr["AdditionalInfoContactInfo"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, unsafeWConditionsNotice);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return unsafeWConditionsNotice;
        }

        public static List<UnsafeWorkingConditionsNotice> GetAllUnsafeWorkingConditionsNotices(string noticeNumber, int? militaryUnitId, DateTime? dateFrom, DateTime? dateTo, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<UnsafeWorkingConditionsNotice> notices = new List<UnsafeWorkingConditionsNotice>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_UNSAFEWCONDNOTICE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(noticeNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeNumber LIKE '%" + noticeNumber.Replace("'", "''") + @"%' ";
                }

                if (militaryUnitId != null)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID = " + militaryUnitId + " ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
                }

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
                        orderBySQL = "a.NoticeNumber";
                        break;
                    case 2:
                        orderBySQL = "a.NoticeDate";
                        break;
                    case 3:
                        orderBySQL = "a.MilitaryUnitID";
                        break;
                    default:
                        orderBySQL = "a.NoticeNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*  FROM (
                                  SELECT a.UnsafeWConditionsNoticeID, a.NoticeNumber, a.NoticeDate, a.ReportingPerson, a.MilitaryUnitID, 
                                         a.ViolationPlace, a.ResponsiblePerson, a.DegreeOfDangerID, a.DescOfUnsafeCondition, 
                                         a.ListOfViolatedRequirements, a.ProcedForReducingRiskEndDate, a.TempProcedures, 
                                         a.TempProceduresEstResult, a.FinalProcedures, a.FinalProceduresEstResult, 
                                         a.AdditionalInfoContactPerson, a.AdditionalInfoContactInfo, 
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.UnsafeWConditionsNoticeID) as RowNumber 
                                  FROM PMIS_HS.UnsafeWorkingConditionsNotices a
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR b ON a.MilitaryUnitID = b.KOD_MIR           
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.UnsafeWConditionsNoticeID                             
                               ) tmp
                               " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    UnsafeWorkingConditionsNotice notice = new UnsafeWorkingConditionsNotice(currentUser);
                    notice.UnsafeWConditionsNoticeId = DBCommon.GetInt(dr["UnsafeWConditionsNoticeID"]);

                    notice.NoticeNumber = dr["NoticeNumber"].ToString();

                    if (dr["NoticeDate"] is DateTime)
                        notice.NoticeDate = (DateTime)dr["NoticeDate"];

                    notice.ReportingPersonName = dr["ReportingPerson"].ToString();

                    if (dr["MilitaryUnitID"] != null)
                        notice.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    else
                        notice.MilitaryUnitId = null;

                    notice.ViolationPlace = dr["ViolationPlace"].ToString();

                    notice.ResponsiblePersonName = dr["ResponsiblePerson"].ToString();

                    if (dr["DegreeOfDangerID"] != null)
                        notice.DangerDegreeId = DBCommon.GetInt(dr["DegreeOfDangerID"]);
                    else
                        notice.DangerDegreeId = null;

                    notice.DescOfUnsafeCondition = dr["DescOfUnsafeCondition"].ToString();
                    notice.ListOfViolatedRequirements = dr["ListOfViolatedRequirements"].ToString();

                    if (dr["ProcedForReducingRiskEndDate"] is DateTime)
                        notice.RiskReducingDueDate = (DateTime)dr["ProcedForReducingRiskEndDate"];

                    notice.TempProcedures = dr["TempProcedures"].ToString();
                    notice.TempProceduresEstResult = dr["TempProceduresEstResult"].ToString();
                    notice.FinalProcedures = dr["FinalProcedures"].ToString();
                    notice.FinalProceduresEstResult = dr["FinalProceduresEstResult"].ToString();
                    notice.AdditionalInfoContactPerson = dr["AdditionalInfoContactPerson"].ToString();
                    notice.AdditionalContactInfo = dr["AdditionalInfoContactInfo"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, notice);

                    notices.Add(notice);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return notices;
        }

        public static int GetAllNoticesCount(string noticeNumber, int? militaryUnitId, DateTime? dateFrom, DateTime? dateTo, User currentUser)
        {
            int riskAssessmentsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_UNSAFEWCONDNOTICE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(noticeNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeNumber LIKE '%" + noticeNumber.Replace("'", "''") + @"%' ";
                }

                if (militaryUnitId != null)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID = " + militaryUnitId + " ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.NoticeDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
                }

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

                string SQL = @"SELECT COUNT(*) as Count
                               FROM PMIS_HS.UnsafeWorkingConditionsNotices a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Count"]))
                        riskAssessmentsCount = DBCommon.GetInt(dr["Count"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskAssessmentsCount;
        }

        public static bool SaveUnsafeWorkingConditionsNotice(UnsafeWorkingConditionsNotice unsafeWConditionsNotice, User currentUser, Change changeEntry)
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
                if (unsafeWConditionsNotice.UnsafeWConditionsNoticeId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.UnsafeWorkingConditionsNotices (NoticeNumber, NoticeDate, ReportingPerson, MilitaryUnitID, ViolationPlace, 
                                ResponsiblePerson, DegreeOfDangerID, DescOfUnsafeCondition, ListOfViolatedRequirements, 
                                ProcedForReducingRiskEndDate, TempProcedures, TempProceduresEstResult, FinalProcedures, 
                                FinalProceduresEstResult, AdditionalInfoContactPerson, AdditionalInfoContactInfo,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:NoticeNumber, :NoticeDate, :ReportingPerson, :MilitaryUnitID, :ViolationPlace, 
                                :ResponsiblePerson, :DegreeOfDangerID, :DescOfUnsafeCondition, :ListOfViolatedRequirements, 
                                :ProcedForReducingRiskEndDate, :TempProcedures, :TempProceduresEstResult, :FinalProcedures, 
                                :FinalProceduresEstResult, :AdditionalInfoContactPerson, :AdditionalInfoContactInfo,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.UnsafeWCondNotices_ID_SEQ.currval INTO :UnsafeWConditionsNoticeID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_UnsafeWCondNotices_AddNotice", "", unsafeWConditionsNotice.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeNumber", "", unsafeWConditionsNotice.NoticeNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeDate", "", CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ReportingPerson", "", unsafeWConditionsNotice.ReportingPersonName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_MilitaryUnit", "", unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ViolationPlace", "", String.IsNullOrEmpty(unsafeWConditionsNotice.ViolationPlace) ? "" : unsafeWConditionsNotice.ViolationPlace, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ResponsiblePerson", "", String.IsNullOrEmpty(unsafeWConditionsNotice.ResponsiblePersonName) ? "" : unsafeWConditionsNotice.ResponsiblePersonName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DangerDegree", "", unsafeWConditionsNotice.DangerDegree != null ? unsafeWConditionsNotice.DangerDegree.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DescOfUnsafeCondition", "", String.IsNullOrEmpty(unsafeWConditionsNotice.DescOfUnsafeCondition) ? "" : unsafeWConditionsNotice.DescOfUnsafeCondition, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ListOfViolatedRequirements", "", String.IsNullOrEmpty(unsafeWConditionsNotice.ListOfViolatedRequirements) ? "" : unsafeWConditionsNotice.ListOfViolatedRequirements, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_RiskReducingDueDate", "", unsafeWConditionsNotice.RiskReducingDueDate.HasValue ? unsafeWConditionsNotice.RiskReducingDueDate.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProcedures", "", String.IsNullOrEmpty(unsafeWConditionsNotice.TempProcedures) ? "" : unsafeWConditionsNotice.TempProcedures, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProcedures", "", String.IsNullOrEmpty(unsafeWConditionsNotice.FinalProcedures) ? "" : unsafeWConditionsNotice.FinalProcedures, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProceduresEstResult", "", String.IsNullOrEmpty(unsafeWConditionsNotice.TempProceduresEstResult) ? "" : unsafeWConditionsNotice.TempProceduresEstResult, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProceduresEstResult", "", String.IsNullOrEmpty(unsafeWConditionsNotice.FinalProceduresEstResult) ? "" : unsafeWConditionsNotice.FinalProceduresEstResult, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalInfoContactPerson", "", String.IsNullOrEmpty(unsafeWConditionsNotice.AdditionalInfoContactPerson) ? "" : unsafeWConditionsNotice.AdditionalInfoContactPerson, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalContactInfo", "", String.IsNullOrEmpty(unsafeWConditionsNotice.AdditionalContactInfo) ? "" : unsafeWConditionsNotice.AdditionalContactInfo, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.UnsafeWorkingConditionsNotices SET
                                NoticeNumber = :NoticeNumber, 
                                NoticeDate = :NoticeDate, 
                                ReportingPerson = :ReportingPerson, 
                                MilitaryUnitID = :MilitaryUnitID, 
                                ViolationPlace = :ViolationPlace, 
                                ResponsiblePerson = :ResponsiblePerson, 
                                DegreeOfDangerID = :DegreeOfDangerID, 
                                DescOfUnsafeCondition = :DescOfUnsafeCondition, 
                                ListOfViolatedRequirements = :ListOfViolatedRequirements, 
                                ProcedForReducingRiskEndDate = :ProcedForReducingRiskEndDate, 
                                TempProcedures = :TempProcedures, 
                                TempProceduresEstResult = :TempProceduresEstResult, 
                                FinalProcedures = :FinalProcedures, 
                                FinalProceduresEstResult = :FinalProceduresEstResult, 
                                AdditionalInfoContactPerson = :AdditionalInfoContactPerson, 
                                AdditionalInfoContactInfo = :AdditionalInfoContactInfo,
                                LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                      THEN :LastModifiedBy
                                                      ELSE LastModifiedBy
                                                 END, 
                                LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                        THEN :LastModifiedDate
                                                        ELSE LastModifiedDate
                                                   END
                             WHERE UnsafeWConditionsNoticeID = :UnsafeWConditionsNoticeID;
                            ";

                    changeEvent = new ChangeEvent("HS_UnsafeWCondNotices_EditNotice", "", unsafeWConditionsNotice.MilitaryUnit, null, currentUser);

                    UnsafeWorkingConditionsNotice oldUnsafeWConditionsNotice = UnsafeWorkingConditionsNoticeUtil.GetUnsafeWorkingConditionsNotice(unsafeWConditionsNotice.UnsafeWConditionsNoticeId, currentUser);

                    if (oldUnsafeWConditionsNotice.NoticeNumber.Trim() != unsafeWConditionsNotice.NoticeNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeNumber", oldUnsafeWConditionsNotice.NoticeNumber, unsafeWConditionsNotice.NoticeNumber, currentUser));

                    if (!CommonFunctions.IsEqual(oldUnsafeWConditionsNotice.NoticeDate, unsafeWConditionsNotice.NoticeDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeDate", CommonFunctions.FormatDate(oldUnsafeWConditionsNotice.NoticeDate), CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate), currentUser));

                    if (oldUnsafeWConditionsNotice.ReportingPersonName.Trim() != unsafeWConditionsNotice.ReportingPersonName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ReportingPerson", oldUnsafeWConditionsNotice.ReportingPersonName, unsafeWConditionsNotice.ReportingPersonName, currentUser));

                    if ((oldUnsafeWConditionsNotice.MilitaryUnit != null ? oldUnsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "") != (unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_MilitaryUnit", oldUnsafeWConditionsNotice.MilitaryUnit != null ? oldUnsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "", unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "", currentUser));

                    if (oldUnsafeWConditionsNotice.ViolationPlace.Trim() != unsafeWConditionsNotice.ViolationPlace.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ViolationPlace", oldUnsafeWConditionsNotice.ViolationPlace, unsafeWConditionsNotice.ViolationPlace, currentUser));

                    if (oldUnsafeWConditionsNotice.ResponsiblePersonName.Trim() != unsafeWConditionsNotice.ResponsiblePersonName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ResponsiblePerson", oldUnsafeWConditionsNotice.ResponsiblePersonName, unsafeWConditionsNotice.ResponsiblePersonName, currentUser));

                    if ((oldUnsafeWConditionsNotice.DangerDegree != null ? oldUnsafeWConditionsNotice.DangerDegree.TableValue : "") != (unsafeWConditionsNotice.DangerDegree != null ? unsafeWConditionsNotice.DangerDegree.TableValue : ""))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DangerDegree", oldUnsafeWConditionsNotice.DangerDegree != null ? oldUnsafeWConditionsNotice.DangerDegree.TableValue : "", unsafeWConditionsNotice.DangerDegree != null ? unsafeWConditionsNotice.DangerDegree.TableValue : "", currentUser));
                    }

                    if (oldUnsafeWConditionsNotice.DescOfUnsafeCondition.Trim() != unsafeWConditionsNotice.DescOfUnsafeCondition.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DescOfUnsafeCondition", oldUnsafeWConditionsNotice.DescOfUnsafeCondition, unsafeWConditionsNotice.DescOfUnsafeCondition, currentUser));

                    if (oldUnsafeWConditionsNotice.ListOfViolatedRequirements.Trim() != unsafeWConditionsNotice.ListOfViolatedRequirements.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ListOfViolatedRequirements", oldUnsafeWConditionsNotice.ListOfViolatedRequirements, unsafeWConditionsNotice.ListOfViolatedRequirements, currentUser));

                    if (!CommonFunctions.IsEqual(oldUnsafeWConditionsNotice.RiskReducingDueDate, unsafeWConditionsNotice.RiskReducingDueDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_RiskReducingDueDate", CommonFunctions.FormatDate(oldUnsafeWConditionsNotice.RiskReducingDueDate), CommonFunctions.FormatDate(unsafeWConditionsNotice.RiskReducingDueDate), currentUser));

                    if (oldUnsafeWConditionsNotice.TempProcedures.Trim() != unsafeWConditionsNotice.TempProcedures.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProcedures", oldUnsafeWConditionsNotice.TempProcedures, unsafeWConditionsNotice.TempProcedures, currentUser));

                    if (oldUnsafeWConditionsNotice.FinalProcedures.Trim() != unsafeWConditionsNotice.FinalProcedures.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProcedures", oldUnsafeWConditionsNotice.FinalProcedures, unsafeWConditionsNotice.FinalProcedures, currentUser));

                    if (oldUnsafeWConditionsNotice.TempProceduresEstResult.Trim() != unsafeWConditionsNotice.TempProceduresEstResult.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProceduresEstResult", oldUnsafeWConditionsNotice.TempProceduresEstResult, unsafeWConditionsNotice.TempProceduresEstResult, currentUser));

                    if (oldUnsafeWConditionsNotice.FinalProceduresEstResult.Trim() != unsafeWConditionsNotice.FinalProceduresEstResult.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProceduresEstResult", oldUnsafeWConditionsNotice.FinalProceduresEstResult, unsafeWConditionsNotice.FinalProceduresEstResult, currentUser));

                    if (oldUnsafeWConditionsNotice.AdditionalInfoContactPerson.Trim() != unsafeWConditionsNotice.AdditionalInfoContactPerson.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalInfoContactPerson", oldUnsafeWConditionsNotice.AdditionalInfoContactPerson, unsafeWConditionsNotice.AdditionalInfoContactPerson, currentUser));

                    if (oldUnsafeWConditionsNotice.AdditionalContactInfo.Trim() != unsafeWConditionsNotice.AdditionalContactInfo.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalContactInfo", oldUnsafeWConditionsNotice.AdditionalContactInfo, unsafeWConditionsNotice.AdditionalContactInfo, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramUnsafeWConditionsNoticeID = new OracleParameter();
                paramUnsafeWConditionsNoticeID.ParameterName = "UnsafeWConditionsNoticeID";
                paramUnsafeWConditionsNoticeID.OracleType = OracleType.Number;

                if (unsafeWConditionsNotice.UnsafeWConditionsNoticeId != 0)
                {
                    paramUnsafeWConditionsNoticeID.Direction = ParameterDirection.Input;
                    paramUnsafeWConditionsNoticeID.Value = unsafeWConditionsNotice.UnsafeWConditionsNoticeId;
                }
                else
                {
                    paramUnsafeWConditionsNoticeID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramUnsafeWConditionsNoticeID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "NoticeNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = unsafeWConditionsNotice.NoticeNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NoticeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = unsafeWConditionsNotice.NoticeDate;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReportingPerson";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.ReportingPersonName))
                    param.Value = unsafeWConditionsNotice.ReportingPersonName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;

                if (unsafeWConditionsNotice.MilitaryUnitId != null)
                    param.Value = unsafeWConditionsNotice.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ViolationPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.ViolationPlace))
                    param.Value = unsafeWConditionsNotice.ViolationPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResponsiblePerson";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.ResponsiblePersonName))
                    param.Value = unsafeWConditionsNotice.ResponsiblePersonName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DegreeOfDangerID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (unsafeWConditionsNotice.DangerDegreeId != null)
                    param.Value = unsafeWConditionsNotice.DangerDegreeId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DescOfUnsafeCondition";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.DescOfUnsafeCondition))
                    param.Value = unsafeWConditionsNotice.DescOfUnsafeCondition;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ListOfViolatedRequirements";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.ListOfViolatedRequirements))
                    param.Value = unsafeWConditionsNotice.ListOfViolatedRequirements;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProcedForReducingRiskEndDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (unsafeWConditionsNotice.RiskReducingDueDate.HasValue)
                    param.Value = unsafeWConditionsNotice.RiskReducingDueDate;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TempProcedures";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.TempProcedures))
                    param.Value = unsafeWConditionsNotice.TempProcedures;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TempProceduresEstResult";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.TempProceduresEstResult))
                    param.Value = unsafeWConditionsNotice.TempProceduresEstResult;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FinalProcedures";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.FinalProcedures))
                    param.Value = unsafeWConditionsNotice.FinalProcedures;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FinalProceduresEstResult";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.FinalProceduresEstResult))
                    param.Value = unsafeWConditionsNotice.FinalProceduresEstResult;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdditionalInfoContactPerson";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.AdditionalInfoContactPerson))
                    param.Value = unsafeWConditionsNotice.AdditionalInfoContactPerson;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdditionalInfoContactInfo";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(unsafeWConditionsNotice.AdditionalContactInfo))
                    param.Value = unsafeWConditionsNotice.AdditionalContactInfo;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (unsafeWConditionsNotice.UnsafeWConditionsNoticeId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (unsafeWConditionsNotice.UnsafeWConditionsNoticeId == 0)
                    unsafeWConditionsNotice.UnsafeWConditionsNoticeId = DBCommon.GetInt(paramUnsafeWConditionsNoticeID.Value);

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

        public static bool DeleteUnsafeWorkingConditionsNotice(int unsafeWConditionsNoticeId, User currentUser, Change changeEntry)
        {
            bool result = false;

            UnsafeWorkingConditionsNotice oldUnsafeWConditionsNotice = UnsafeWorkingConditionsNoticeUtil.GetUnsafeWorkingConditionsNotice(unsafeWConditionsNoticeId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_UnsafeWCondNotices_DeleteNotice", "", oldUnsafeWConditionsNotice.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeNumber", "", oldUnsafeWConditionsNotice.NoticeNumber, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_NoticeDate", "", CommonFunctions.FormatDate(oldUnsafeWConditionsNotice.NoticeDate), currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ReportingPerson", "", oldUnsafeWConditionsNotice.ReportingPersonName, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_MilitaryUnit", "", oldUnsafeWConditionsNotice.MilitaryUnit != null ? oldUnsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ViolationPlace", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.ViolationPlace) ? "" : oldUnsafeWConditionsNotice.ViolationPlace, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ResponsiblePerson", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.ResponsiblePersonName) ? "" : oldUnsafeWConditionsNotice.ResponsiblePersonName, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DangerDegree", "", oldUnsafeWConditionsNotice.DangerDegree != null ? oldUnsafeWConditionsNotice.DangerDegree.TableValue : "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_DescOfUnsafeCondition", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.DescOfUnsafeCondition) ? "" : oldUnsafeWConditionsNotice.DescOfUnsafeCondition, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_ListOfViolatedRequirements", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.ListOfViolatedRequirements) ? "" : oldUnsafeWConditionsNotice.ListOfViolatedRequirements, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_RiskReducingDueDate", "", oldUnsafeWConditionsNotice.RiskReducingDueDate.HasValue ? oldUnsafeWConditionsNotice.RiskReducingDueDate.Value.ToString() : "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProcedures", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.TempProcedures) ? "" : oldUnsafeWConditionsNotice.TempProcedures, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProcedures", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.FinalProcedures) ? "" : oldUnsafeWConditionsNotice.FinalProcedures, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_TempProceduresEstResult", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.TempProceduresEstResult) ? "" : oldUnsafeWConditionsNotice.TempProceduresEstResult, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_FinalProceduresEstResult", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.FinalProceduresEstResult) ? "" : oldUnsafeWConditionsNotice.FinalProceduresEstResult, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalInfoContactPerson", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.AdditionalInfoContactPerson) ? "" : oldUnsafeWConditionsNotice.AdditionalInfoContactPerson, currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_UnsafeWCondNotices_AdditionalContactInfo", "", String.IsNullOrEmpty(oldUnsafeWConditionsNotice.AdditionalContactInfo) ? "" : oldUnsafeWConditionsNotice.AdditionalContactInfo, currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"DELETE FROM PMIS_HS.UnsafeWorkingConditionsNotices WHERE UnsafeWConditionsNoticeID = :UnsafeWConditionsNoticeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UnsafeWConditionsNoticeID", OracleType.Number).Value = unsafeWConditionsNoticeId;

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
