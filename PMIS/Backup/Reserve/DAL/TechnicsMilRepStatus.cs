using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular record from the TechnicsMilRepStatus table
    public class TechnicsMilRepStatus : BaseDbObject
    {
        private int technicsMilRepStatusId;        
        private int technicsId;
        bool isCurrent;
        private int techMilitaryReportStatusId;
        private TechMilitaryReportStatus techMilitaryReportStatus;
        private DateTime? enrolDate;
        private DateTime? dischargeDate;
        private int? sourceMilDepartmentId;
        private MilitaryDepartment sourceMilDepartment;        
        private string contract_ContractNumber;
        private DateTime? contract_ContractFromDate;
        private int? contract_MilitaryUnitID;
        private MilitaryUnit contract_MilitaryUnit;
        private DateTime? contract_ContractToDate;
        private string voluntary_ContractNumber;
        private DateTime? voluntary_ContractDate;
        private int? voluntary_DurationMonths;
        private DateTime? voluntary_ContractToDate;
        private int? voluntary_FulfilPlaceID;
        private MilitaryUnit voluntary_FulfilPlace;
        private DateTime? removed_Date;
        private int? removed_ReasonId;
        private GTableItem removed_Reason; //Gtable -> TechMilRepStat_RemovedReasons
        private int? temporaryRemoved_ReasonId;
        private GTableItem temporaryRemoved_Reason; //Gtable -> TechMilRepStat_ТemporaryRemovedReasons
        private DateTime? temporaryRemoved_Date;
        private int? temporaryRemoved_Duration;
        private int? technicsPostpone_TypeId;
        private TechnicsPostponeType technicsPostpone_Type;
        private int? technicsPostpone_Year;  

        public int TechnicsMilRepStatusId
        {
            get { return technicsMilRepStatusId; }
            set { technicsMilRepStatusId = value; }
        }

        public int TechnicsId
        {
            get
            {
                return technicsId;
            }
            set
            {
                technicsId = value;
            }
        }

        public bool IsCurrent
        {
            get
            {
                return isCurrent;
            }
            set
            {
                isCurrent = value;
            }
        }

        public int TechMilitaryReportStatusId
        {
            get
            {
                return techMilitaryReportStatusId;
            }
            set
            {
                techMilitaryReportStatusId = value;
            }
        }

        public TechMilitaryReportStatus TechMilitaryReportStatus
        {
            get
            {
                //Lazy initialization
                if (techMilitaryReportStatus == null)
                    techMilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatus(techMilitaryReportStatusId, CurrentUser);

                return techMilitaryReportStatus;
            }
            set
            {
                techMilitaryReportStatus = value;
            }
        }

        public DateTime? EnrolDate
        {
            get
            {
                return enrolDate;
            }
            set
            {
                enrolDate = value;
            }
        }

        public DateTime? DischargeDate
        {
            get
            {
                return dischargeDate;
            }
            set
            {
                dischargeDate = value;
            }
        }

        public int? SourceMilDepartmentId
        {
            get
            {
                return sourceMilDepartmentId;
            }
            set
            {
                sourceMilDepartmentId = value;
            }
        }

        public MilitaryDepartment SourceMilDepartment
        {
            get
            {
                //Lazy initialization
                if (sourceMilDepartment == null && sourceMilDepartmentId.HasValue)
                    sourceMilDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(sourceMilDepartmentId.Value, CurrentUser);

                return sourceMilDepartment;
            }
            set
            {
                sourceMilDepartment = value;
            }
        }

        public string Contract_ContractNumber
        {
            get
            {
                return contract_ContractNumber;
            }
            set
            {
                contract_ContractNumber = value;
            }
        }

        public DateTime? Contract_ContractFromDate
        {
            get
            {
                return contract_ContractFromDate;
            }
            set
            {
                contract_ContractFromDate = value;
            }
        }

        public int? Contract_MilitaryUnitID
        {
            get
            {
                return contract_MilitaryUnitID;
            }
            set
            {
                contract_MilitaryUnitID = value;
            }
        }

        public MilitaryUnit Contract_MilitaryUnit
        {
            get
            {
                if (contract_MilitaryUnit == null && contract_MilitaryUnitID.HasValue)
                    contract_MilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(contract_MilitaryUnitID.Value, CurrentUser);

                return contract_MilitaryUnit;
            }
            set { contract_MilitaryUnit = value; }
        }

        public DateTime? Contract_ContractToDate
        {
            get
            {
                return contract_ContractToDate;
            }
            set
            {
                contract_ContractToDate = value;
            }
        }

        public string Voluntary_ContractNumber
        {
            get
            {
                return voluntary_ContractNumber;
            }
            set
            {
                voluntary_ContractNumber = value;
            }
        }

        public DateTime? Voluntary_ContractDate
        {
            get
            {
                return voluntary_ContractDate;
            }
            set
            {
                voluntary_ContractDate = value;
            }
        }

        public int? Voluntary_DurationMonths
        {
            get
            {
                return voluntary_DurationMonths;
            }
            set
            {
                voluntary_DurationMonths = value;
            }
        }

        public DateTime? Voluntary_ContractToDate
        {
            get
            {
                return voluntary_ContractToDate;
            }
            set
            {
                voluntary_ContractToDate = value;
            }
        }

        public int? Voluntary_FulfilPlaceID
        {
            get { return voluntary_FulfilPlaceID; }
            set { voluntary_FulfilPlaceID = value; }
        }
        public MilitaryUnit Voluntary_FulfilPlace
        {
            get
            {
                if (voluntary_FulfilPlace == null && voluntary_FulfilPlaceID != null)
                {
                    voluntary_FulfilPlace = MilitaryUnitUtil.GetMilitaryUnit(voluntary_FulfilPlaceID.Value, CurrentUser);
                }
                return voluntary_FulfilPlace;
            }
            set
            {
                voluntary_FulfilPlace = value;
            }
        }

        public DateTime? Removed_Date
        {
            get
            {
                return removed_Date;
            }
            set
            {
                removed_Date = value;
            }
        }

        public int? Removed_ReasonId
        {
            get
            {
                return removed_ReasonId;
            }
            set
            {
                removed_ReasonId = value;
            }
        }

        public GTableItem Removed_Reason
        {
            get
            {
                //Lazy initialization
                if (removed_Reason == null && removed_ReasonId.HasValue)
                    removed_Reason = GTableItemUtil.GetTableItem("TechMilRepStat_RemovedReasons", removed_ReasonId.Value, ModuleUtil.RES(), CurrentUser);

                return removed_Reason;
            }
            set
            {
                removed_Reason = value;
            }
        }

        public int? TemporaryRemoved_ReasonId
        {
            get
            {
                return temporaryRemoved_ReasonId;
            }
            set
            {
                temporaryRemoved_ReasonId = value;
            }
        }

        public GTableItem TemporaryRemoved_Reason
        {
            get
            {
                //Lazy initialization
                if (temporaryRemoved_Reason == null && temporaryRemoved_ReasonId.HasValue)
                    temporaryRemoved_Reason = GTableItemUtil.GetTableItem("MilRepStat_ТTechMilRepStat_ТemporaryRemovedReasons", temporaryRemoved_ReasonId.Value, ModuleUtil.RES(), CurrentUser);

                return temporaryRemoved_Reason;
            }
            set
            {
                temporaryRemoved_Reason = value;
            }
        }

        public DateTime? TemporaryRemoved_Date
        {
            get
            {
                return temporaryRemoved_Date;
            }
            set
            {
                temporaryRemoved_Date = value;
            }
        }

        public int? TemporaryRemoved_Duration
        {
            get
            {
                return temporaryRemoved_Duration;
            }
            set
            {
                temporaryRemoved_Duration = value;
            }
        }

        public int? TechnicsPostpone_TypeId
        {
            get
            {
                return technicsPostpone_TypeId;
            }
            set
            {
                technicsPostpone_TypeId = value;
            }
        }

        public TechnicsPostponeType TechnicsPostpone_Type
        {
            get
            {
                //Lazy initialization
                if (technicsPostpone_Type == null && technicsPostpone_TypeId.HasValue)
                    technicsPostpone_Type = TechnicsPostponeTypeUtil.GetTechnicsPostponeType(technicsPostpone_TypeId.Value, CurrentUser);

                return technicsPostpone_Type;
            }
            set
            {
                technicsPostpone_Type = value;
            }
        }

        public int? TechnicsPostpone_Year
        {
            get
            {
                return technicsPostpone_Year;
            }
            set
            {
                technicsPostpone_Year = value;
            }
        }

        public TechnicsMilRepStatus(User user)
            : base(user)
        {

        }
    }

    public static class TechnicsMilRepStatusUtil
    {
        //This method creates and returns a TechnicsMilRepStatusUtil object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static TechnicsMilRepStatus ExtractTechnicsMilRepStatus(OracleDataReader dr, User currentUser)
        {
            TechnicsMilRepStatus technicsMilRepStatus = new TechnicsMilRepStatus(currentUser);

            technicsMilRepStatus.TechnicsMilRepStatusId = DBCommon.GetInt(dr["TechnicsMilRepStatusId"]);
            technicsMilRepStatus.TechnicsId = DBCommon.GetInt(dr["TechnicsId"]);
            technicsMilRepStatus.IsCurrent = DBCommon.GetInt(dr["IsCurrent"]) == 1;
            technicsMilRepStatus.TechMilitaryReportStatusId = DBCommon.GetInt(dr["TechMilitaryReportStatusId"]);
            technicsMilRepStatus.EnrolDate = (dr["EnrolDate"] is DateTime) ? (DateTime)dr["EnrolDate"] : (DateTime?)null;
            technicsMilRepStatus.DischargeDate = (dr["DischargeDate"] is DateTime) ? (DateTime)dr["DischargeDate"] : (DateTime?)null;
            technicsMilRepStatus.SourceMilDepartmentId = (DBCommon.IsInt(dr["SourceMilDepartmentID"]) ? DBCommon.GetInt(dr["SourceMilDepartmentID"]) : (int?)null);
            technicsMilRepStatus.Contract_ContractNumber = dr["Contract_ContractNumber"].ToString();
            technicsMilRepStatus.Contract_ContractFromDate = (dr["Contract_ContractFromDate"] is DateTime) ? (DateTime)dr["Contract_ContractFromDate"] : (DateTime?)null;
            technicsMilRepStatus.Contract_MilitaryUnitID = (DBCommon.IsInt(dr["Contract_MilitaryUnitID"]) ? DBCommon.GetInt(dr["Contract_MilitaryUnitID"]) : (int?)null);
            technicsMilRepStatus.Contract_ContractToDate = (dr["Contract_ContractToDate"] is DateTime) ? (DateTime)dr["Contract_ContractToDate"] : (DateTime?)null;

            technicsMilRepStatus.Voluntary_ContractNumber = dr["Voluntary_ContractNumber"].ToString();
            technicsMilRepStatus.Voluntary_ContractDate = (dr["Voluntary_ContractDate"] is DateTime) ? (DateTime)dr["Voluntary_ContractDate"] : (DateTime?)null;
            technicsMilRepStatus.Voluntary_DurationMonths = (DBCommon.IsInt(dr["Voluntary_DurationMonths"]) ? DBCommon.GetInt(dr["Voluntary_DurationMonths"]) : (int?)null);
            technicsMilRepStatus.Voluntary_ContractToDate = (dr["Voluntary_ContractToDate"] is DateTime) ? (DateTime)dr["Voluntary_ContractToDate"] : (DateTime?)null;
            technicsMilRepStatus.Voluntary_FulfilPlaceID = (DBCommon.IsInt(dr["Voluntary_FulfilPlaceID"]) ? DBCommon.GetInt(dr["Voluntary_FulfilPlaceID"]) : (int?)null);
            technicsMilRepStatus.Removed_Date = (dr["Removed_Date"] is DateTime) ? (DateTime)dr["Removed_Date"] : (DateTime?)null;
            technicsMilRepStatus.Removed_ReasonId = (DBCommon.IsInt(dr["Removed_ReasonID"]) ? DBCommon.GetInt(dr["Removed_ReasonID"]) : (int?)null);
            technicsMilRepStatus.TemporaryRemoved_ReasonId = (DBCommon.IsInt(dr["TemporaryRemoved_ReasonID"]) ? DBCommon.GetInt(dr["TemporaryRemoved_ReasonID"]) : (int?)null);
            technicsMilRepStatus.TemporaryRemoved_Date = (dr["TemporaryRemoved_Date"] is DateTime) ? (DateTime)dr["TemporaryRemoved_Date"] : (DateTime?)null;
            technicsMilRepStatus.TemporaryRemoved_Duration = (DBCommon.IsInt(dr["TemporaryRemoved_Duration"]) ? DBCommon.GetInt(dr["TemporaryRemoved_Duration"]) : (int?)null);
            technicsMilRepStatus.TechnicsPostpone_TypeId = (DBCommon.IsInt(dr["TechnicsPostpone_TypeID"]) ? DBCommon.GetInt(dr["TechnicsPostpone_TypeID"]) : (int?)null);
            technicsMilRepStatus.TechnicsPostpone_Year = (DBCommon.IsInt(dr["TechnicsPostpone_Year"]) ? DBCommon.GetInt(dr["TechnicsPostpone_Year"]) : (int?)null);            

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, technicsMilRepStatus);

            return technicsMilRepStatus;
        }

        //Get a particular object by its ID
        public static TechnicsMilRepStatus GetTechnicsMilRepStatus(int technicsMilRepStatusId, User currentUser)
        {
            TechnicsMilRepStatus technicsMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsMilRepStatusId,
                                      a.TechnicsId,
                                      a.IsCurrent,
                                      
                                      a.TechMilitaryReportStatusId,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      
                                      a.Contract_ContractNumber,
                                      a.Contract_ContractFromDate,
                                      a.Contract_MilitaryUnitID,
                                      a.Contract_ContractToDate,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_ContractToDate,
                                      a.Voluntary_FulfilPlaceID,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.TechnicsPostpone_TypeID,
                                      a.TechnicsPostpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsMilRepStatus a
                               WHERE a.TechnicsMilRepStatusId = :TechnicsMilRepStatusId ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsMilRepStatusId", OracleType.Number).Value = technicsMilRepStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsMilRepStatus = ExtractTechnicsMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatus;
        }

        //Get the current status by TechnicsId
        public static TechnicsMilRepStatus GetTechnicsMilRepCurrentStatusByTechnicsId(int technicsId, User currentUser)
        {
            TechnicsMilRepStatus technicsMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsMilRepStatusId,
                                      a.TechnicsId,
                                      a.IsCurrent,
                                      
                                      a.TechMilitaryReportStatusId,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      
                                      a.Contract_ContractNumber,
                                      a.Contract_ContractFromDate,
                                      a.Contract_MilitaryUnitID,
                                      a.Contract_ContractToDate,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_ContractToDate,
                                      a.Voluntary_FulfilPlaceID,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.TechnicsPostpone_TypeID,
                                      a.TechnicsPostpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsMilRepStatus a
                               WHERE a.TechnicsId = :TechnicsId AND a.IsCurrent = 1 ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsMilRepStatus = ExtractTechnicsMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatus;
        }

        //Get the first status by TechnicsId
        public static TechnicsMilRepStatus GetTechnicsMilRepFirstStatusByTechnicsId(int technicsId, User currentUser)
        {
            TechnicsMilRepStatus technicsMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.TechnicsMilRepStatusId,
                                      a.TechnicsId,
                                      a.IsCurrent,
                                      
                                      a.TechMilitaryReportStatusId,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      
                                      a.Contract_ContractNumber,
                                      a.Contract_ContractFromDate,
                                      a.Contract_MilitaryUnitID,
                                      a.Contract_ContractToDate,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_ContractToDate,
                                      a.Voluntary_FulfilPlaceID,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.TechnicsPostpone_TypeID,
                                      a.TechnicsPostpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate,
                                      
                                      a.RowNumber
                                FROM (
                                    SELECT a.TechnicsMilRepStatusId,
                                      a.TechnicsId,
                                      a.IsCurrent,
                                      
                                      a.TechMilitaryReportStatusId,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      
                                      a.Contract_ContractNumber,
                                      a.Contract_ContractFromDate,
                                      a.Contract_MilitaryUnitID,
                                      a.Contract_ContractToDate,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_ContractToDate,
                                      a.Voluntary_FulfilPlaceID,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.TechnicsPostpone_TypeID,
                                      a.TechnicsPostpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate,
                                          
                                          (RANK() OVER (ORDER BY a.TechnicsMilRepStatusId)) as RowNumber
                                   FROM PMIS_RES.TechnicsMilRepStatus a
                                   WHERE a.TechnicsId = :TechnicsId
                                ) a
                                WHERE a.RowNumber = 1";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsMilRepStatus = ExtractTechnicsMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatus;
        }

        //Get all statuses (history) by Technics
        public static List<TechnicsMilRepStatus> GetAllTechnicsMilRepStatusByTechnicsId(int technicsId, User currentUser)
        {
            List<TechnicsMilRepStatus> technicsMilRepStatus = new List<TechnicsMilRepStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsMilRepStatusId,
                                      a.TechnicsId,
                                      a.IsCurrent,
                                      
                                      a.TechMilitaryReportStatusId,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      
                                      a.Contract_ContractNumber,
                                      a.Contract_ContractFromDate,
                                      a.Contract_MilitaryUnitID,
                                      a.Contract_ContractToDate,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_ContractToDate,
                                      a.Voluntary_FulfilPlaceID,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.TechnicsPostpone_TypeID,
                                      a.TechnicsPostpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsMilRepStatus a
                               WHERE a.TechnicsId = :TechnicsId 
                               ORDER BY a.TechnicsMilRepStatusId";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsMilRepStatus.Add(ExtractTechnicsMilRepStatus(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatus;
        }

        //Get all statuses (history) by Technics with pagination
        public static List<TechnicsMilRepStatus> GetAllTechnicsMilRepStatusByTechnicsId(int technicsId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TechnicsMilRepStatus> technicsMilRepStatus = new List<TechnicsMilRepStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "NVL(a.EnrolDate, a.DischargeDate)";
                string orderByDir = "ASC";                             

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.TechnicsMilRepStatusId,
                                                      a.TechnicsId,
                                                      a.IsCurrent,
                                                      
                                                      a.TechMilitaryReportStatusId,
                                                      a.EnrolDate,
                                                      a.DischargeDate,
                                                      a.SourceMilDepartmentID,
                                                      
                                                      a.Contract_ContractNumber,
                                                      a.Contract_ContractFromDate,
                                                      a.Contract_MilitaryUnitID,
                                                      a.Contract_ContractToDate,
                                      
                                                      a.Voluntary_ContractNumber,
                                                      a.Voluntary_ContractDate,
                                                      a.Voluntary_DurationMonths,
                                                      a.Voluntary_ContractToDate,
                                                      a.Voluntary_FulfilPlaceID,
                                                      
                                                      a.Removed_Date,
                                                      a.Removed_ReasonID, 
                                                      
                                                      a.TemporaryRemoved_ReasonID,
                                                      a.TemporaryRemoved_Date,
                                                      a.TemporaryRemoved_Duration,
                                                      
                                                      a.TechnicsPostpone_TypeID,
                                                      a.TechnicsPostpone_Year,                                     
                                                      
                                                      a.CreatedBy,
                                                      a.CreatedDate,
                                                      a.LastModifiedBy,
                                                      a.LastModifiedDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsMilRepStatusId) as RowNumber
                                               FROM PMIS_RES.TechnicsMilRepStatus a
                                               WHERE a.TechnicsId = :TechnicsId
                                               ORDER BY " + orderBySQL + @", a.TechnicsMilRepStatusId
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsMilRepStatus.Add(ExtractTechnicsMilRepStatus(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatus;
        }

        //Get all statuses (history) count by Technics for pagination
        public static int GetAllTechnicsMilRepStatusByTechnicsIdCount(int technicsId, User currentUser)
        {
            int technicsMilRepStatuses = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {                                
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.TechnicsMilRepStatus a
                               WHERE a.TechnicsId = :TechnicsId
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {                    
                    if (DBCommon.IsInt(dr["Cnt"]))
                        technicsMilRepStatuses = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsMilRepStatuses;
        }

        public static void SetTechnicsMilRepStatusModified(int technicsMilRepStatusId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.TechnicsMilRepStatus SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE TechnicsMilRepStatusId = :TechnicsMilRepStatusId";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsMilRepStatusId", OracleType.Number).Value = technicsMilRepStatusId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool SaveTechnicsMilRepStatus(TechnicsMilRepStatus technicsMilRepStatus, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            TechnicsMilRepStatus oldTechnicsMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepStatus(technicsMilRepStatus.TechnicsMilRepStatusId, currentUser);

            string logDescription = TechnicsUtil.GetTechnicsLogDescription(technicsMilRepStatus.TechnicsId, currentUser);

            string oldMilRepStatusName = "";

            if (oldTechnicsMilRepStatus != null)
            {
                oldMilRepStatusName = oldTechnicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName;
            }
            else
            {
                TechnicsMilRepStatus currTechnicsMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(technicsMilRepStatus.TechnicsId, currentUser);

                if (currTechnicsMilRepStatus != null)
                    oldMilRepStatusName = currTechnicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName;
                else
                    oldMilRepStatusName = TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus();
            }

            logDescription += "<br />Състояние по отчета: " + oldMilRepStatusName;

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (technicsMilRepStatus.TechnicsMilRepStatusId == 0)
                {
                    SQL += @"UPDATE PMIS_RES.TechnicsMilRepStatus SET
                                IsCurrent = 0
                             WHERE TechnicsID = :TechnicsID;

                             INSERT INTO PMIS_RES.TechnicsMilRepStatus 
                                    (TechnicsID,
                                     IsCurrent,
                                          
                                     TechMilitaryReportStatusID,
                                     EnrolDate,
                                     DischargeDate,
                                     SourceMilDepartmentID,                                    
                                          
                                     Contract_ContractNumber,
                                     Contract_ContractFromDate,
                                     Contract_MilitaryUnitID,
                                     Contract_ContractToDate,
                                      
                                     Voluntary_ContractNumber,
                                     Voluntary_ContractDate,
                                     Voluntary_DurationMonths,
                                     Voluntary_ContractToDate,
                                     Voluntary_FulfilPlaceID,
                                     
                                     Removed_Date,
                                     Removed_ReasonID, 
                                     
                                     TemporaryRemoved_ReasonID,
                                     TemporaryRemoved_Date,
                                     TemporaryRemoved_Duration,
                                     
                                     TechnicsPostpone_TypeID,
                                     TechnicsPostpone_Year,
                                          
                                     CreatedBy,
                                     CreatedDate,
                                     LastModifiedBy,
                                     LastModifiedDate)
                            VALUES ( :TechnicsID,
                                     :IsCurrent,
                                          
                                     :TechMilitaryReportStatusID,
                                     :EnrolDate,
                                     :DischargeDate,
                                     :SourceMilDepartmentID,                                    
                                          
                                     :Contract_ContractNumber,
                                     :Contract_ContractFromDate,
                                     :Contract_MilitaryUnitID,
                                     :Contract_ContractToDate,
                                      
                                     :Voluntary_ContractNumber,
                                     :Voluntary_ContractDate,
                                     :Voluntary_DurationMonths,
                                     :Voluntary_ContractToDate,
                                     :Voluntary_FulfilPlaceID,
                                     
                                     :Removed_Date,
                                     :Removed_ReasonID, 
                                     
                                     :TemporaryRemoved_ReasonID,
                                     :TemporaryRemoved_Date,
                                     :TemporaryRemoved_Duration,
                                     
                                     :TechnicsPostpone_TypeID,
                                     :TechnicsPostpone_Year,                                                                
                                          
                                     :CreatedBy,
                                     :CreatedDate,
                                     :LastModifiedBy,
                                     :LastModifiedDate);

                            SELECT PMIS_RES.TechnicsMilRepStatus_ID_SEQ.currval INTO :TechnicsMilRepStatusID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_AddMilRepStatus", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_MilRepStatus", "", technicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_EnrolDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.EnrolDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_DischargeDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.EnrolDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_SourceMilDept", "", technicsMilRepStatus.SourceMilDepartment != null ? technicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", currentUser));

                    switch (technicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey)
                    {
                        case "CONTRACT":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractNumber", "", technicsMilRepStatus.Contract_ContractNumber, currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractFromDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractFromDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractToDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractToDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractMilitaryUnit", "", technicsMilRepStatus.Contract_MilitaryUnit != null ? technicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "", currentUser));
                            break;
                        case "VOLUNTARY_RESERVE":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractNumber", "", technicsMilRepStatus.Voluntary_ContractNumber, currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryDurationMonths", "", technicsMilRepStatus.Voluntary_DurationMonths.HasValue ? technicsMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractToDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractToDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryFulfilPlace", "", (technicsMilRepStatus.Voluntary_FulfilPlace != null ? technicsMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), currentUser));
                            break;
                        case "REMOVED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_RemovedDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.Removed_Date), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_RemovedReason", "", technicsMilRepStatus.Removed_Reason != null ? technicsMilRepStatus.Removed_Reason.Text() : "", currentUser));
                            break;
                        case "TEMPORARY_REMOVED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedReason", "", technicsMilRepStatus.TemporaryRemoved_Reason != null ? technicsMilRepStatus.TemporaryRemoved_Reason.Text() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedDate", "", CommonFunctions.FormatDate(technicsMilRepStatus.TemporaryRemoved_Date), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedDuration", "", technicsMilRepStatus.TemporaryRemoved_Duration.HasValue ? technicsMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", currentUser));
                            break;
                        case "POSTPONED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TechnicsPostponeType", "", technicsMilRepStatus.TechnicsPostpone_Type != null ? technicsMilRepStatus.TechnicsPostpone_Type.Text() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TechnicsPostponeYear", "", technicsMilRepStatus.TechnicsPostpone_Year.HasValue ? technicsMilRepStatus.TechnicsPostpone_Year.Value.ToString() : "", currentUser));                            
                            break;
                    }
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.TechnicsMilRepStatus SET
                                     TechnicsID = :TechnicsID,
                                     IsCurrent = :IsCurrent,
                                          
                                     TechMilitaryReportStatusID = :TechMilitaryReportStatusID,
                                     EnrolDate = :EnrolDate,
                                     DischargeDate = :DischargeDate,
                                     SourceMilDepartmentID = :SourceMilDepartmentID,
                                          
                                     Contract_ContractNumber = :Contract_ContractNumber,
                                     Contract_ContractFromDate = :Contract_ContractFromDate,
                                     Contract_MilitaryUnitID = :Contract_MilitaryUnitID,
                                     Contract_ContractToDate = :Contract_ContractToDate,
                                                  
                                     Voluntary_ContractNumber = :Voluntary_ContractNumber,
                                     Voluntary_ContractDate = :Voluntary_ContractDate,
                                     Voluntary_DurationMonths = :Voluntary_DurationMonths,
                                     Voluntary_ContractToDate = :Voluntary_ContractToDate,
                                     Voluntary_FulfilPlaceID = :Voluntary_FulfilPlaceID,
                                          
                                     Removed_Date = :Removed_Date,
                                     Removed_ReasonID = :Removed_ReasonID,                                                                                
                                          
                                     TemporaryRemoved_ReasonID = :TemporaryRemoved_ReasonID,
                                     TemporaryRemoved_Date = :TemporaryRemoved_Date,
                                     TemporaryRemoved_Duration = :TemporaryRemoved_Duration,
                                          
                                     TechnicsPostpone_TypeID = :TechnicsPostpone_TypeID,
                                     TechnicsPostpone_Year = :TechnicsPostpone_Year,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                            WHERE TechnicsMilRepStatusID = :TechnicsMilRepStatusID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_EditMilRepStatus", logDescription, null, null, currentUser);

                    if (oldTechnicsMilRepStatus.EnrolDate != technicsMilRepStatus.EnrolDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_EnrolDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.EnrolDate), CommonFunctions.FormatDate(technicsMilRepStatus.EnrolDate), currentUser));

                    if (oldTechnicsMilRepStatus.DischargeDate != technicsMilRepStatus.DischargeDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_DischargeDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.DischargeDate), CommonFunctions.FormatDate(technicsMilRepStatus.DischargeDate), currentUser));

                    if ((oldTechnicsMilRepStatus.SourceMilDepartment != null ? oldTechnicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "") != (technicsMilRepStatus.SourceMilDepartment != null ? technicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_SourceMilDept", oldTechnicsMilRepStatus.SourceMilDepartment != null ? oldTechnicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", technicsMilRepStatus.SourceMilDepartment != null ? technicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", currentUser));

                    switch (technicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey)
                    {
                        case "CONTRACT":
                            if (oldTechnicsMilRepStatus.Contract_ContractNumber.Trim() != technicsMilRepStatus.Contract_ContractNumber.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractNumber", oldTechnicsMilRepStatus.Contract_ContractNumber, technicsMilRepStatus.Contract_ContractNumber, currentUser));

                            if (oldTechnicsMilRepStatus.Contract_ContractFromDate != technicsMilRepStatus.Contract_ContractFromDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractFromDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.Contract_ContractFromDate), CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractFromDate), currentUser));

                            if ((oldTechnicsMilRepStatus.Contract_MilitaryUnit != null ? oldTechnicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "") != (technicsMilRepStatus.Contract_MilitaryUnit != null ? technicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractMilitaryUnit", oldTechnicsMilRepStatus.Contract_MilitaryUnit != null ? oldTechnicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "", technicsMilRepStatus.Contract_MilitaryUnit != null ? technicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "", currentUser));

                            if (oldTechnicsMilRepStatus.Contract_ContractToDate != technicsMilRepStatus.Contract_ContractToDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_ContractContractToDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.Contract_ContractToDate), CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractToDate), currentUser));
                            break;
                        case "VOLUNTARY_RESERVE":
                            if (oldTechnicsMilRepStatus.Voluntary_ContractNumber.Trim() != technicsMilRepStatus.Voluntary_ContractNumber.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractNumber", oldTechnicsMilRepStatus.Voluntary_ContractNumber, technicsMilRepStatus.Voluntary_ContractNumber, currentUser));

                            if (oldTechnicsMilRepStatus.Voluntary_ContractDate != technicsMilRepStatus.Voluntary_ContractDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.Voluntary_ContractDate), CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractDate), currentUser));

                            if (oldTechnicsMilRepStatus.Voluntary_DurationMonths != technicsMilRepStatus.Voluntary_DurationMonths)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryDurationMonths", oldTechnicsMilRepStatus.Voluntary_DurationMonths.HasValue ? oldTechnicsMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", technicsMilRepStatus.Voluntary_DurationMonths.HasValue ? technicsMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", currentUser));

                            if (oldTechnicsMilRepStatus.Voluntary_ContractToDate != technicsMilRepStatus.Voluntary_ContractToDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryContractToDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.Voluntary_ContractToDate), CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractToDate), currentUser));
                            /*
                            if (oldTechnicsMilRepStatus.Voluntary_FulfilPlace.Trim() != technicsMilRepStatus.Voluntary_FulfilPlace.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryFulfilPlace", oldTechnicsMilRepStatus.Voluntary_FulfilPlace, technicsMilRepStatus.Voluntary_FulfilPlace, currentUser));                                                                                    
                            */
                            if (oldTechnicsMilRepStatus.Voluntary_FulfilPlaceID != technicsMilRepStatus.Voluntary_FulfilPlaceID)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_VoluntaryFulfilPlace", (oldTechnicsMilRepStatus.Voluntary_FulfilPlace != null ? oldTechnicsMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), (technicsMilRepStatus.Voluntary_FulfilPlace != null ? technicsMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), currentUser));
                          
                            break;
                        case "REMOVED":
                            if (oldTechnicsMilRepStatus.Removed_Date != technicsMilRepStatus.Removed_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_RemovedDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.Removed_Date), CommonFunctions.FormatDate(technicsMilRepStatus.Removed_Date), currentUser));

                            if ((oldTechnicsMilRepStatus.Removed_Reason != null ? oldTechnicsMilRepStatus.Removed_Reason.Text() : "") != (technicsMilRepStatus.Removed_Reason != null ? technicsMilRepStatus.Removed_Reason.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_RemovedReason", oldTechnicsMilRepStatus.Removed_Reason != null ? oldTechnicsMilRepStatus.Removed_Reason.Text() : "", technicsMilRepStatus.Removed_Reason != null ? technicsMilRepStatus.Removed_Reason.Text() : "", currentUser));
                            break;                        
                        case "TEMPORARY_REMOVED":
                            if ((oldTechnicsMilRepStatus.TemporaryRemoved_Reason != null ? oldTechnicsMilRepStatus.TemporaryRemoved_Reason.Text() : "") != (technicsMilRepStatus.TemporaryRemoved_Reason != null ? technicsMilRepStatus.TemporaryRemoved_Reason.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedReason", oldTechnicsMilRepStatus.TemporaryRemoved_Reason != null ? oldTechnicsMilRepStatus.TemporaryRemoved_Reason.Text() : "", technicsMilRepStatus.TemporaryRemoved_Reason != null ? technicsMilRepStatus.TemporaryRemoved_Reason.Text() : "", currentUser));

                            if (oldTechnicsMilRepStatus.TemporaryRemoved_Date != technicsMilRepStatus.TemporaryRemoved_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedDate", CommonFunctions.FormatDate(oldTechnicsMilRepStatus.TemporaryRemoved_Date), CommonFunctions.FormatDate(technicsMilRepStatus.TemporaryRemoved_Date), currentUser));

                            if (oldTechnicsMilRepStatus.TemporaryRemoved_Duration != technicsMilRepStatus.TemporaryRemoved_Duration)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TemporaryRemovedDuration", oldTechnicsMilRepStatus.TemporaryRemoved_Duration.HasValue ? oldTechnicsMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", technicsMilRepStatus.TemporaryRemoved_Duration.HasValue ? technicsMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", currentUser));
                            break;
                        case "POSTPONED":
                            if ((oldTechnicsMilRepStatus.TechnicsPostpone_Type != null ? oldTechnicsMilRepStatus.TechnicsPostpone_Type.Text() : "") != (technicsMilRepStatus.TechnicsPostpone_Type != null ? technicsMilRepStatus.TechnicsPostpone_Type.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TechnicsPostponeType", oldTechnicsMilRepStatus.TechnicsPostpone_Type != null ? oldTechnicsMilRepStatus.TechnicsPostpone_Type.Text() : "", technicsMilRepStatus.TechnicsPostpone_Type != null ? technicsMilRepStatus.TechnicsPostpone_Type.Text() : "", currentUser));

                            if (oldTechnicsMilRepStatus.TechnicsPostpone_Year != technicsMilRepStatus.TechnicsPostpone_Year)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_TechnicsPostponeYear", oldTechnicsMilRepStatus.TechnicsPostpone_Year.HasValue ? oldTechnicsMilRepStatus.TechnicsPostpone_Year.Value.ToString() : "", technicsMilRepStatus.TechnicsPostpone_Year.HasValue ? technicsMilRepStatus.TechnicsPostpone_Year.Value.ToString() : "", currentUser));                            
                            break;
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTechnicsMilRepStatusID = new OracleParameter();
                paramTechnicsMilRepStatusID.ParameterName = "TechnicsMilRepStatusID";
                paramTechnicsMilRepStatusID.OracleType = OracleType.Number;

                if (technicsMilRepStatus.TechnicsMilRepStatusId != 0)
                {
                    paramTechnicsMilRepStatusID.Direction = ParameterDirection.Input;
                    paramTechnicsMilRepStatusID.Value = technicsMilRepStatus.TechnicsMilRepStatusId;
                }
                else
                {
                    paramTechnicsMilRepStatusID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTechnicsMilRepStatusID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsMilRepStatus.TechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (technicsMilRepStatus.IsCurrent ? 1 : 0);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechMilitaryReportStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsMilRepStatus.TechMilitaryReportStatusId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EnrolDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.EnrolDate.HasValue)
                    param.Value = technicsMilRepStatus.EnrolDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DischargeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.DischargeDate.HasValue)
                    param.Value = technicsMilRepStatus.DischargeDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SourceMilDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.SourceMilDepartmentId.HasValue)
                    param.Value = technicsMilRepStatus.SourceMilDepartmentId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Contract_ContractNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(technicsMilRepStatus.Contract_ContractNumber))
                    param.Value = technicsMilRepStatus.Contract_ContractNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Contract_ContractFromDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Contract_ContractFromDate.HasValue)
                    param.Value = technicsMilRepStatus.Contract_ContractFromDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Contract_MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Contract_MilitaryUnitID.HasValue)
                    param.Value = technicsMilRepStatus.Contract_MilitaryUnitID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Contract_ContractToDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Contract_ContractToDate.HasValue)
                    param.Value = technicsMilRepStatus.Contract_ContractToDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ContractNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(technicsMilRepStatus.Voluntary_ContractNumber))
                    param.Value = technicsMilRepStatus.Voluntary_ContractNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ContractDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Voluntary_ContractDate.HasValue)
                    param.Value = technicsMilRepStatus.Voluntary_ContractDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_DurationMonths";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Voluntary_DurationMonths.HasValue)
                    param.Value = technicsMilRepStatus.Voluntary_DurationMonths.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ContractToDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Voluntary_ContractToDate.HasValue)
                    param.Value = technicsMilRepStatus.Voluntary_ContractToDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_FulfilPlaceID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Voluntary_FulfilPlaceID != null)
                    param.Value = technicsMilRepStatus.Voluntary_FulfilPlaceID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                                              

                param = new OracleParameter();
                param.ParameterName = "Removed_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Removed_Date.HasValue)
                    param.Value = technicsMilRepStatus.Removed_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_ReasonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.Removed_ReasonId.HasValue)
                    param.Value = technicsMilRepStatus.Removed_ReasonId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                               

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_ReasonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.TemporaryRemoved_ReasonId.HasValue)
                    param.Value = technicsMilRepStatus.TemporaryRemoved_ReasonId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.TemporaryRemoved_Date.HasValue)
                    param.Value = technicsMilRepStatus.TemporaryRemoved_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_Duration";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.TemporaryRemoved_Duration.HasValue)
                    param.Value = technicsMilRepStatus.TemporaryRemoved_Duration.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsPostpone_TypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.TechnicsPostpone_TypeId.HasValue)
                    param.Value = technicsMilRepStatus.TechnicsPostpone_TypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsPostpone_Year";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsMilRepStatus.TechnicsPostpone_Year.HasValue)
                    param.Value = technicsMilRepStatus.TechnicsPostpone_Year.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                

                if (technicsMilRepStatus.TechnicsMilRepStatusId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);
                TechnicsUtil.SetTechnicsModified(technicsMilRepStatus.TechnicsId, currentUser);

                cmd.ExecuteNonQuery();

                if (technicsMilRepStatus.TechnicsMilRepStatusId == 0)
                    technicsMilRepStatus.TechnicsMilRepStatusId = DBCommon.GetInt(paramTechnicsMilRepStatusID.Value);

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

        //When doing a fulfilment for a particular Technics into a particular Military Command then
        //use this method to change the current Military Reporting Status
        public static void SetMilRepStatusTo_MOBILE_APPOINTMENT(int technicsId, User currentUser, Change changeEntry)
        {
            TechnicsMilRepStatus currentTechnicsMilRepStatus = GetTechnicsMilRepCurrentStatusByTechnicsId(technicsId, currentUser);
            TechnicsMilRepStatus technicsMilRepStatus = new TechnicsMilRepStatus(currentUser);

            technicsMilRepStatus.TechnicsMilRepStatusId = 0;
            technicsMilRepStatus.TechnicsId = technicsId;
            technicsMilRepStatus.IsCurrent = true;
            technicsMilRepStatus.TechMilitaryReportStatusId = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", currentUser).TechMilitaryReportStatusId;
            technicsMilRepStatus.EnrolDate = DateTime.Now;
            technicsMilRepStatus.SourceMilDepartmentId = currentTechnicsMilRepStatus.SourceMilDepartmentId;

            SaveTechnicsMilRepStatus(technicsMilRepStatus, currentUser, changeEntry);
        }

        //When doing a fulfilment for a particular Technics into a particular Military Command then
        //use this method to change the current Military Reporting Status
        public static void SetMilRepStatusTo_FREE(int technicsId, User currentUser, Change changeEntry)
        {
            TechnicsMilRepStatus currentTechnicsMilRepStatus = GetTechnicsMilRepCurrentStatusByTechnicsId(technicsId, currentUser);
            TechnicsMilRepStatus technicsMilRepStatus = new TechnicsMilRepStatus(currentUser);

            technicsMilRepStatus.TechnicsMilRepStatusId = 0;
            technicsMilRepStatus.TechnicsId = technicsId;
            technicsMilRepStatus.IsCurrent = true;
            technicsMilRepStatus.TechMilitaryReportStatusId = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("FREE", currentUser).TechMilitaryReportStatusId;
            technicsMilRepStatus.EnrolDate = DateTime.Now;
            technicsMilRepStatus.SourceMilDepartmentId = currentTechnicsMilRepStatus.SourceMilDepartmentId;

            SaveTechnicsMilRepStatus(technicsMilRepStatus, currentUser, changeEntry);
        }
    }
}