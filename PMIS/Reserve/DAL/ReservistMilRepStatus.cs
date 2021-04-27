using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular record from the ReservistMilRepStatuses table
    public class ReservistMilRepStatus : BaseDbObject
    {
        private int reservistMilRepStatusId;        
        private int reservistId;
        bool isCurrent;
        private int militaryReportStatusId;
        private MilitaryReportStatus militaryReportStatus;
        private DateTime? enrolDate;
        private DateTime? dischargeDate;
        private int? sourceMilDepartmentId;
        private MilitaryDepartment sourceMilDepartment;
        private int? destMilDepartmentId;
        private MilitaryDepartment destMilDepartment;
        private string voluntary_ContractNumber;
        private DateTime? voluntary_ContractDate;
        private DateTime? voluntary_ExpireDate;
        private int? voluntary_DurationMonths;       
        private int? voluntary_FulfilPlaceID;
        private MilitaryUnit voluntary_FulfilPlace;
        private string voluntary_MilitaryRankId;
        private MilitaryRank voluntary_MilitaryRank;
        private int? voluntary_MilRepSpecialityId;
        private MilitaryReportSpeciality voluntary_MilRepSpeciality;
        private string voluntary_MilitaryPosition;
        private DateTime? removed_Date;
        private int? removed_ReasonId;
        private GTableItem removed_Reason; //Gtable -> MilRepStat_RemovedReasons
        private string removed_Deceased_DeathCert;
        private DateTime? removed_Deceased_Date;
        private string removed_AgeLimit_Order;
        private DateTime? removed_AgeLimit_Date;
        private string removed_AgeLimit_SignedBy;
        private string removed_NotSuitable_Cert;
        private DateTime? removed_NotSuitable_Date;
        private string removed_NotSuitable_SignedBy;
        private int? milEmployed_AdministrationId;
        private Administration milEmployed_Administration;
        private DateTime? milEmployed_Date;
        private int? temporaryRemoved_ReasonId;
        private GTableItem temporaryRemoved_Reason; //Gtable -> MilRepStat_ТemporaryRemovedReasons
        private DateTime? temporaryRemoved_Date;
        private int? temporaryRemoved_Duration;
        private int? postpone_TypeId;
        private PostponeType postpone_Type;
        private int? postpone_Year;

        public int ReservistMilRepStatusId
        {
            get { return reservistMilRepStatusId; }
            set { reservistMilRepStatusId = value; }
        }

        public int ReservistId
        {
            get
            {
                return reservistId;
            }
            set
            {
                reservistId = value;
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

        public int MilitaryReportStatusId
        {
            get
            {
                return militaryReportStatusId;
            }
            set
            {
                militaryReportStatusId = value;
            }
        }

        public MilitaryReportStatus MilitaryReportStatus
        {
            get
            {
                //Lazy initialization
                if (militaryReportStatus == null)
                    militaryReportStatus = MilitaryReportStatusUtil.GetMilitaryReportStatus(militaryReportStatusId, CurrentUser);

                return militaryReportStatus;
            }
            set
            {
                militaryReportStatus = value;
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

        public int? DestMilDepartmentId
        {
            get
            {
                return destMilDepartmentId;
            }
            set
            {
                destMilDepartmentId = value;
            }
        }

        public MilitaryDepartment DestMilDepartment
        {
            get
            {
                //Lazy initialization
                if (destMilDepartment == null && destMilDepartmentId.HasValue)
                    destMilDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(destMilDepartmentId.Value, CurrentUser);

                return destMilDepartment;
            }
            set
            {
                destMilDepartment = value;
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

        public DateTime? Voluntary_ExpireDate
        {
            get
            {
                return voluntary_ExpireDate;
            }
            set
            {
                voluntary_ExpireDate = value;
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

        public string Voluntary_MilitaryRankId
        {
            get
            {
                return voluntary_MilitaryRankId;
            }
            set
            {
                voluntary_MilitaryRankId = value;
            }
        }

        public MilitaryRank Voluntary_MilitaryRank
        {
            get
            {
                //Lazy initialization
                if (voluntary_MilitaryRank == null && !String.IsNullOrEmpty(voluntary_MilitaryRankId))
                    voluntary_MilitaryRank = MilitaryRankUtil.GetMilitaryRank(voluntary_MilitaryRankId, CurrentUser);

                return voluntary_MilitaryRank;
            }
            set
            {
                voluntary_MilitaryRank = value;
            }
        }

        public int? Voluntary_MilRepSpecialityId
        {
            get
            {
                return voluntary_MilRepSpecialityId;
            }
            set
            {
                voluntary_MilRepSpecialityId = value;
            }
        }

        public MilitaryReportSpeciality Voluntary_MilRepSpeciality
        {
            get
            {
                //Lazy initialization
                if (voluntary_MilRepSpeciality == null && voluntary_MilRepSpecialityId.HasValue)
                    voluntary_MilRepSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(voluntary_MilRepSpecialityId.Value, CurrentUser);

                return voluntary_MilRepSpeciality;
            }
            set
            {
                voluntary_MilRepSpeciality = value;
            }
        }

        public string Voluntary_MilitaryPosition
        {
            get
            {
                return voluntary_MilitaryPosition;
            }
            set
            {
                voluntary_MilitaryPosition = value;
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
                    removed_Reason = GTableItemUtil.GetTableItem("MilRepStat_RemovedReasons", removed_ReasonId.Value, ModuleUtil.RES(), CurrentUser);

                return removed_Reason;
            }
            set
            {
                removed_Reason = value;
            }
        }

        public string Removed_Deceased_DeathCert
        {
            get
            {
                return removed_Deceased_DeathCert;
            }
            set
            {
                removed_Deceased_DeathCert = value;
            }
        }

        public DateTime? Removed_Deceased_Date
        {
            get
            {
                return removed_Deceased_Date;
            }
            set
            {
                removed_Deceased_Date = value;
            }
        }

        public string Removed_AgeLimit_Order
        {
            get
            {
                return removed_AgeLimit_Order;
            }
            set
            {
                removed_AgeLimit_Order = value;
            }
        }

        public DateTime? Removed_AgeLimit_Date
        {
            get
            {
                return removed_AgeLimit_Date;
            }
            set
            {
                removed_AgeLimit_Date = value;
            }
        }

        public string Removed_AgeLimit_SignedBy
        {
            get
            {
                return removed_AgeLimit_SignedBy;
            }
            set
            {
                removed_AgeLimit_SignedBy = value;
            }
        }

        public string Removed_NotSuitable_Cert
        {
            get
            {
                return removed_NotSuitable_Cert;
            }
            set
            {
                removed_NotSuitable_Cert = value;
            }
        }

        public DateTime? Removed_NotSuitable_Date
        {
            get
            {
                return removed_NotSuitable_Date;
            }
            set
            {
                removed_NotSuitable_Date = value;
            }
        }

        public string Removed_NotSuitable_SignedBy
        {
            get
            {
                return removed_NotSuitable_SignedBy;
            }
            set
            {
                removed_NotSuitable_SignedBy = value;
            }
        }

        public int? MilEmployed_AdministrationId
        {
            get
            {
                return milEmployed_AdministrationId;
            }
            set
            {
                milEmployed_AdministrationId = value;
            }
        }

        public Administration MilEmployed_Administration
        {
            get
            {
                //Lazy initialization
                if (milEmployed_Administration == null && milEmployed_AdministrationId.HasValue)
                    milEmployed_Administration = AdministrationUtil.GetAdministration(milEmployed_AdministrationId.Value, CurrentUser);

                return milEmployed_Administration;
            }
            set
            {
                milEmployed_Administration = value;
            }
        }


        public DateTime? MilEmployed_Date
        {
            get
            {
                return milEmployed_Date;
            }
            set
            {
                milEmployed_Date = value;
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
                    temporaryRemoved_Reason = GTableItemUtil.GetTableItem("MilRepStat_ТemporaryRemovedReasons", temporaryRemoved_ReasonId.Value, ModuleUtil.RES(), CurrentUser);

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

        public int? Postpone_TypeId
        {
            get
            {
                return postpone_TypeId;
            }
            set
            {
                postpone_TypeId = value;
            }
        }

        public PostponeType Postpone_Type
        {
            get
            {
                //Lazy initialization
                if (postpone_Type == null && postpone_TypeId.HasValue)
                    postpone_Type = PostponeTypeUtil.GetPostponeType(postpone_TypeId.Value, CurrentUser);

                return postpone_Type;
            }
            set
            {
                postpone_Type = value;
            }
        }

        public int? Postpone_Year
        {
            get
            {
                return postpone_Year;
            }
            set
            {
                postpone_Year = value;
            }
        }

        public ReservistMilRepStatus(User user)
            : base(user)
        {

        }
    }

    public static class ReservistMilRepStatusUtil
    {
        //This method creates and returns a ReservistMilRepStatus object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static ReservistMilRepStatus ExtractReservistMilRepStatus(OracleDataReader dr, User currentUser)
        {
            ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(currentUser);

            reservistMilRepStatus.ReservistMilRepStatusId = DBCommon.GetInt(dr["ReservistMilRepStatusID"]);
            reservistMilRepStatus.ReservistId = DBCommon.GetInt(dr["ReservistID"]);
            reservistMilRepStatus.IsCurrent = DBCommon.GetInt(dr["IsCurrent"]) == 1;
            reservistMilRepStatus.MilitaryReportStatusId = DBCommon.GetInt(dr["MilitaryReportStatusID"]);
            reservistMilRepStatus.EnrolDate = (dr["EnrolDate"] is DateTime) ? (DateTime)dr["EnrolDate"] : (DateTime?)null;
            reservistMilRepStatus.DischargeDate = (dr["DischargeDate"] is DateTime) ? (DateTime)dr["DischargeDate"] : (DateTime?)null;
            reservistMilRepStatus.SourceMilDepartmentId = (DBCommon.IsInt(dr["SourceMilDepartmentID"]) ? DBCommon.GetInt(dr["SourceMilDepartmentID"]) : (int?)null);
            reservistMilRepStatus.DestMilDepartmentId = (DBCommon.IsInt(dr["DestMilDepartmentID"]) ? DBCommon.GetInt(dr["DestMilDepartmentID"]) : (int?)null);
            reservistMilRepStatus.Voluntary_ContractNumber = dr["Voluntary_ContractNumber"].ToString();
            reservistMilRepStatus.Voluntary_ContractDate = (dr["Voluntary_ContractDate"] is DateTime) ? (DateTime)dr["Voluntary_ContractDate"] : (DateTime?)null;
            reservistMilRepStatus.Voluntary_ExpireDate = (dr["Voluntary_ExpireDate"] is DateTime) ? (DateTime)dr["Voluntary_ExpireDate"] : (DateTime?)null;
            reservistMilRepStatus.Voluntary_DurationMonths = (DBCommon.IsInt(dr["Voluntary_DurationMonths"]) ? DBCommon.GetInt(dr["Voluntary_DurationMonths"]) : (int?)null);
            reservistMilRepStatus.Voluntary_FulfilPlaceID = (DBCommon.IsInt(dr["Voluntary_FulfilPlaceID"]) ? DBCommon.GetInt(dr["Voluntary_FulfilPlaceID"]) : (int?)null);
            reservistMilRepStatus.Voluntary_MilitaryRankId = dr["Voluntary_MilitaryRankID"].ToString();
            reservistMilRepStatus.Voluntary_MilRepSpecialityId = (DBCommon.IsInt(dr["Voluntary_MilRepSpecialityID"]) ? DBCommon.GetInt(dr["Voluntary_MilRepSpecialityID"]) : (int?)null);
            reservistMilRepStatus.Voluntary_MilitaryPosition = dr["Voluntary_MilitaryPosition"].ToString();
            reservistMilRepStatus.Removed_Date = (dr["Removed_Date"] is DateTime) ? (DateTime)dr["Removed_Date"] : (DateTime?)null;
            reservistMilRepStatus.Removed_ReasonId = (DBCommon.IsInt(dr["Removed_ReasonID"]) ? DBCommon.GetInt(dr["Removed_ReasonID"]) : (int?)null);
            reservistMilRepStatus.Removed_Deceased_DeathCert = dr["Removed_Deceased_DeathCert"].ToString();
            reservistMilRepStatus.Removed_Deceased_Date = (dr["Removed_Deceased_Date"] is DateTime) ? (DateTime)dr["Removed_Deceased_Date"] : (DateTime?)null;
            reservistMilRepStatus.Removed_AgeLimit_Order = dr["Removed_AgeLimit_Order"].ToString();
            reservistMilRepStatus.Removed_AgeLimit_Date = (dr["Removed_AgeLimit_Date"] is DateTime) ? (DateTime)dr["Removed_AgeLimit_Date"] : (DateTime?)null;
            reservistMilRepStatus.Removed_AgeLimit_SignedBy = dr["Removed_AgeLimit_SignedBy"].ToString();
            reservistMilRepStatus.Removed_NotSuitable_Cert = dr["Removed_NotSuitable_Cert"].ToString();
            reservistMilRepStatus.Removed_NotSuitable_Date = (dr["Removed_NotSuitable_Date"] is DateTime) ? (DateTime)dr["Removed_NotSuitable_Date"] : (DateTime?)null;
            reservistMilRepStatus.Removed_NotSuitable_SignedBy = dr["Removed_NotSuitable_SignedBy"].ToString();
            reservistMilRepStatus.MilEmployed_AdministrationId = (DBCommon.IsInt(dr["MilEmployed_AdministrationID"]) ? DBCommon.GetInt(dr["MilEmployed_AdministrationID"]) : (int?)null);
            reservistMilRepStatus.MilEmployed_Date = (dr["MilEmployed_Date"] is DateTime) ? (DateTime)dr["MilEmployed_Date"] : (DateTime?)null;
            reservistMilRepStatus.TemporaryRemoved_ReasonId = (DBCommon.IsInt(dr["TemporaryRemoved_ReasonID"]) ? DBCommon.GetInt(dr["TemporaryRemoved_ReasonID"]) : (int?)null);
            reservistMilRepStatus.TemporaryRemoved_Date = (dr["TemporaryRemoved_Date"] is DateTime) ? (DateTime)dr["TemporaryRemoved_Date"] : (DateTime?)null;
            reservistMilRepStatus.TemporaryRemoved_Duration = (DBCommon.IsInt(dr["TemporaryRemoved_Duration"]) ? DBCommon.GetInt(dr["TemporaryRemoved_Duration"]) : (int?)null);
            reservistMilRepStatus.Postpone_TypeId = (DBCommon.IsInt(dr["Postpone_TypeID"]) ? DBCommon.GetInt(dr["Postpone_TypeID"]) : (int?)null);
            reservistMilRepStatus.Postpone_Year = (DBCommon.IsInt(dr["Postpone_Year"]) ? DBCommon.GetInt(dr["Postpone_Year"]) : (int?)null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, reservistMilRepStatus);

            return reservistMilRepStatus;
        }

        //Get a particular object by its ID
        public static ReservistMilRepStatus GetReservistMilRepStatus(int reservistMilRepStatusId, User currentUser)
        {
            ReservistMilRepStatus reservistMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistMilRepStatusID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                      
                                      a.MilitaryReportStatusID,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      a.DestMilDepartmentID,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_ExpireDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_FulfilPlaceID,
                                      a.Voluntary_MilitaryRankID,
                                      a.Voluntary_MilRepSpecialityID,
                                      a.Voluntary_MilitaryPosition,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID,
                                      a.Removed_Deceased_DeathCert, 
                                      a.Removed_Deceased_Date,
                                      a.Removed_AgeLimit_Order, 
                                      a.Removed_AgeLimit_Date, 
                                      a.Removed_AgeLimit_SignedBy, 
                                      a.Removed_NotSuitable_Cert,
                                      a.Removed_NotSuitable_Date,
                                      a.Removed_NotSuitable_SignedBy,
                                      
                                      a.MilEmployed_AdministrationID,
                                      a.MilEmployed_Date,
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.Postpone_TypeID,
                                      a.Postpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistMilRepStatuses a
                               WHERE a.ReservistMilRepStatusID = :ReservistMilRepStatusID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistMilRepStatusID", OracleType.Number).Value = reservistMilRepStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservistMilRepStatus = ExtractReservistMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatus;
        }

        //Get the current status by ReservistID
        public static ReservistMilRepStatus GetReservistMilRepCurrentStatusByReservistId(int reservistId, User currentUser)
        {
            ReservistMilRepStatus reservistMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistMilRepStatusID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                      
                                      a.MilitaryReportStatusID,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      a.DestMilDepartmentID,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_ExpireDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_FulfilPlaceID,
                                      a.Voluntary_MilitaryRankID,
                                      a.Voluntary_MilRepSpecialityID,
                                      a.Voluntary_MilitaryPosition,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      a.Removed_Deceased_DeathCert, 
                                      a.Removed_Deceased_Date,
                                      a.Removed_AgeLimit_Order, 
                                      a.Removed_AgeLimit_Date, 
                                      a.Removed_AgeLimit_SignedBy, 
                                      a.Removed_NotSuitable_Cert,
                                      a.Removed_NotSuitable_Date,
                                      a.Removed_NotSuitable_SignedBy,

                                      a.MilEmployed_AdministrationID,
                                      a.MilEmployed_Date,
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.Postpone_TypeID,
                                      a.Postpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistMilRepStatuses a
                               WHERE a.ReservistId = :ReservistId AND a.IsCurrent = 1 ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservistMilRepStatus = ExtractReservistMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatus;
        }

        //Get the first status by ReservistID
        public static ReservistMilRepStatus GetReservistMilRepFirstStatusByReservistId(int reservistId, User currentUser)
        {
            ReservistMilRepStatus reservistMilRepStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.ReservistMilRepStatusID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                      
                                      a.MilitaryReportStatusID,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      a.DestMilDepartmentID,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_ExpireDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_FulfilPlaceID,
                                      a.Voluntary_MilitaryRankID,
                                      a.Voluntary_MilRepSpecialityID,
                                      a.Voluntary_MilitaryPosition,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      a.Removed_Deceased_DeathCert, 
                                      a.Removed_Deceased_Date,
                                      a.Removed_AgeLimit_Order, 
                                      a.Removed_AgeLimit_Date, 
                                      a.Removed_AgeLimit_SignedBy, 
                                      a.Removed_NotSuitable_Cert,
                                      a.Removed_NotSuitable_Date,
                                      a.Removed_NotSuitable_SignedBy,

                                      a.MilEmployed_AdministrationID,
                                      a.MilEmployed_Date,
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.Postpone_TypeID,
                                      a.Postpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate,
                                      
                                      a.RowNumber
                                FROM (
                                    SELECT a.ReservistMilRepStatusID,
                                          a.ReservistID,
                                          a.IsCurrent,
                                          
                                          a.MilitaryReportStatusID,
                                          a.EnrolDate,
                                          a.DischargeDate,
                                          a.SourceMilDepartmentID,
                                          a.DestMilDepartmentID,
                                          
                                          a.Voluntary_ContractNumber,
                                          a.Voluntary_ContractDate,
                                          a.Voluntary_ExpireDate,
                                          a.Voluntary_DurationMonths,
                                          a.Voluntary_FulfilPlaceID,
                                          a.Voluntary_MilitaryRankID,
                                          a.Voluntary_MilRepSpecialityID,
                                          a.Voluntary_MilitaryPosition,
                                          
                                          a.Removed_Date,
                                          a.Removed_ReasonID, 
                                          a.Removed_Deceased_DeathCert, 
                                          a.Removed_Deceased_Date,
                                          a.Removed_AgeLimit_Order, 
                                          a.Removed_AgeLimit_Date, 
                                          a.Removed_AgeLimit_SignedBy, 
                                          a.Removed_NotSuitable_Cert,
                                          a.Removed_NotSuitable_Date,
                                          a.Removed_NotSuitable_SignedBy,

                                          a.MilEmployed_AdministrationID,
                                          a.MilEmployed_Date,
                                          
                                          a.TemporaryRemoved_ReasonID,
                                          a.TemporaryRemoved_Date,
                                          a.TemporaryRemoved_Duration,
                                          
                                          a.Postpone_TypeID,
                                          a.Postpone_Year,
                                          
                                          a.CreatedBy,
                                          a.CreatedDate,
                                          a.LastModifiedBy,
                                          a.LastModifiedDate,
                                          
                                          (RANK() OVER (ORDER BY a.ReservistMilRepStatusID)) as RowNumber
                                   FROM PMIS_RES.ReservistMilRepStatuses a
                                   WHERE a.ReservistId = :ReservistId
                                ) a
                                WHERE a.RowNumber = 1";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservistMilRepStatus = ExtractReservistMilRepStatus(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatus;
        }

        //Get all statuses (history) by Reservist
        public static List<ReservistMilRepStatus> GetAllReservistMilRepStatusByReservistId(int reservistId, User currentUser)
        {
            List<ReservistMilRepStatus> reservistMilRepStatuses = new List<ReservistMilRepStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistMilRepStatusID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                      
                                      a.MilitaryReportStatusID,
                                      a.EnrolDate,
                                      a.DischargeDate,
                                      a.SourceMilDepartmentID,
                                      a.DestMilDepartmentID,
                                      
                                      a.Voluntary_ContractNumber,
                                      a.Voluntary_ContractDate,
                                      a.Voluntary_ExpireDate,
                                      a.Voluntary_DurationMonths,
                                      a.Voluntary_FulfilPlaceID,
                                      a.Voluntary_MilitaryRankID,
                                      a.Voluntary_MilRepSpecialityID,
                                      a.Voluntary_MilitaryPosition,
                                      
                                      a.Removed_Date,
                                      a.Removed_ReasonID, 
                                      a.Removed_Deceased_DeathCert, 
                                      a.Removed_Deceased_Date,
                                      a.Removed_AgeLimit_Order, 
                                      a.Removed_AgeLimit_Date, 
                                      a.Removed_AgeLimit_SignedBy, 
                                      a.Removed_NotSuitable_Cert,
                                      a.Removed_NotSuitable_Date,
                                      a.Removed_NotSuitable_SignedBy,

                                      a.MilEmployed_AdministrationID,
                                      a.MilEmployed_Date,
                                      
                                      a.TemporaryRemoved_ReasonID,
                                      a.TemporaryRemoved_Date,
                                      a.TemporaryRemoved_Duration,
                                      
                                      a.Postpone_TypeID,
                                      a.Postpone_Year,
                                      
                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistMilRepStatuses a
                               WHERE a.ReservistId = :ReservistId 
                               ORDER BY a.ReservistMilRepStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reservistMilRepStatuses.Add(ExtractReservistMilRepStatus(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatuses;
        }

        //Get all statuses (history) by Reservist with pagination
        public static List<ReservistMilRepStatus> GetAllReservistMilRepStatusByReservistId(int reservistId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<ReservistMilRepStatus> reservistMilRepStatuses = new List<ReservistMilRepStatus>();

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
                                               SELECT a.ReservistMilRepStatusID,
                                                      a.ReservistID,
                                                      a.IsCurrent,
                                                      
                                                      a.MilitaryReportStatusID,
                                                      a.EnrolDate,
                                                      a.DischargeDate,
                                                      a.SourceMilDepartmentID,
                                                      a.DestMilDepartmentID,
                                                      
                                                      a.Voluntary_ContractNumber,
                                                      a.Voluntary_ContractDate,
                                                      a.Voluntary_ExpireDate,
                                                      a.Voluntary_DurationMonths,
                                                      a.Voluntary_FulfilPlaceID,
                                                      a.Voluntary_MilitaryRankID,
                                                      a.Voluntary_MilRepSpecialityID,
                                                      a.Voluntary_MilitaryPosition,
                                                      
                                                      a.Removed_Date,
                                                      a.Removed_ReasonID, 
                                                      a.Removed_Deceased_DeathCert, 
                                                      a.Removed_Deceased_Date,
                                                      a.Removed_AgeLimit_Order, 
                                                      a.Removed_AgeLimit_Date, 
                                                      a.Removed_AgeLimit_SignedBy, 
                                                      a.Removed_NotSuitable_Cert,
                                                      a.Removed_NotSuitable_Date,
                                                      a.Removed_NotSuitable_SignedBy,

                                                      a.MilEmployed_AdministrationID,
                                                      a.MilEmployed_Date,
                                                      
                                                      a.TemporaryRemoved_ReasonID,
                                                      a.TemporaryRemoved_Date,
                                                      a.TemporaryRemoved_Duration,
                                                      
                                                      a.Postpone_TypeID,
                                                      a.Postpone_Year,
                                                      
                                                      a.CreatedBy,
                                                      a.CreatedDate,
                                                      a.LastModifiedBy,
                                                      a.LastModifiedDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistMilRepStatusID) as RowNumber
                                               FROM PMIS_RES.ReservistMilRepStatuses a
                                               WHERE a.ReservistId = :ReservistId
                                               ORDER BY " + orderBySQL + @", a.ReservistMilRepStatusID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reservistMilRepStatuses.Add(ExtractReservistMilRepStatus(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatuses;
        }

        //Get all statuses (history) count by Reservist for pagination
        public static int GetAllReservistMilRepStatusByReservistIdCount(int reservistId, User currentUser)
        {
            int reservistMilRepStatuses = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {                                
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.ReservistMilRepStatuses a
                               WHERE a.ReservistId = :ReservistId
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {                    
                    if (DBCommon.IsInt(dr["Cnt"]))
                        reservistMilRepStatuses = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistMilRepStatuses;
        }

        public static void SetReservistMilRepStatusModified(int reservistMilRepStatusId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.ReservistMilRepStatuses SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE ReservistMilRepStatusID = :ReservistMilRepStatusID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistMilRepStatusID", OracleType.Number).Value = reservistMilRepStatusId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }     

        public static bool SaveReservistMilRepStatus(ReservistMilRepStatus reservistMilRepStatus, User currentUser, Change changeEntry)
        {
            bool result = false;

            string removedReason = reservistMilRepStatus.Removed_Reason != null ? reservistMilRepStatus.Removed_Reason.Text() : "";

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            Reservist reservist = ReservistUtil.GetReservist(reservistMilRepStatus.ReservistId, currentUser);
            Person person = reservist.Person;

            ReservistMilRepStatus oldReservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepStatus(reservistMilRepStatus.ReservistMilRepStatusId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + person.FullName;
            logDescription += "<br />ЕГН: " + person.IdentNumber;

            string oldMilRepStatusName = "";

            if (oldReservistMilRepStatus != null)
            {
                oldMilRepStatusName = oldReservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName;
            }
            else
            {
                ReservistMilRepStatus currReservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(reservistMilRepStatus.ReservistId, currentUser);

                if (currReservistMilRepStatus != null)
                    oldMilRepStatusName = currReservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName;
                else
                    oldMilRepStatusName = MilitaryReportStatusUtil.GetLabelWhenLackOfStatus();

            }

            logDescription += "<br />Състояние по отчета: " + oldMilRepStatusName;

            try
            {
                SQL = @"BEGIN
                                
                               ";
                if (reservistMilRepStatus.ReservistMilRepStatusId == 0)
                {
                    SQL += @"UPDATE PMIS_RES.ReservistMilRepStatuses SET
                                        IsCurrent = 0
                                     WHERE ReservistID = :ReservistID;
        
                                     INSERT INTO PMIS_RES.ReservistMilRepStatuses 
                                            (ReservistID,
                                             IsCurrent,
                                                  
                                             MilitaryReportStatusID,
                                             EnrolDate,
                                             DischargeDate,
                                             SourceMilDepartmentID,
                                             DestMilDepartmentID,
                                                  
                                             Voluntary_ContractNumber,
                                             Voluntary_ContractDate,
                                             Voluntary_ExpireDate,
                                             Voluntary_DurationMonths,
                                             Voluntary_FulfilPlaceID,
                                             Voluntary_MilitaryRankID,
                                             Voluntary_MilRepSpecialityID,
                                             Voluntary_MilitaryPosition,
                                                  
                                             Removed_Date,
                                             Removed_ReasonID, 
                                             Removed_Deceased_DeathCert, 
                                             Removed_Deceased_Date,
                                             Removed_AgeLimit_Order, 
                                             Removed_AgeLimit_Date, 
                                             Removed_AgeLimit_SignedBy, 
                                             Removed_NotSuitable_Cert,
                                             Removed_NotSuitable_Date,
                                             Removed_NotSuitable_SignedBy,
                                                  
                                             MilEmployed_AdministrationID,
                                             MilEmployed_Date,
                                                  
                                             TemporaryRemoved_ReasonID,
                                             TemporaryRemoved_Date,
                                             TemporaryRemoved_Duration,
                                                  
                                             Postpone_TypeID,
                                             Postpone_Year,
                                                  
                                             CreatedBy,
                                             CreatedDate,
                                             LastModifiedBy,
                                             LastModifiedDate)
                                    VALUES ( :ReservistID,
                                             :IsCurrent,
                                                  
                                             :MilitaryReportStatusID,
                                             :EnrolDate,
                                             :DischargeDate,
                                             :SourceMilDepartmentID,
                                             :DestMilDepartmentID,
                                                  
                                             :Voluntary_ContractNumber,
                                             :Voluntary_ContractDate,
                                             :Voluntary_ExpireDate,
                                             :Voluntary_DurationMonths,
                                             :Voluntary_FulfilPlaceID,
                                             :Voluntary_MilitaryRankID,
                                             :Voluntary_MilRepSpecialityID,
                                             :Voluntary_MilitaryPosition,
                                                  
                                             :Removed_Date,
                                             :Removed_ReasonID, 
                                             :Removed_Deceased_DeathCert, 
                                             :Removed_Deceased_Date,
                                             :Removed_AgeLimit_Order, 
                                             :Removed_AgeLimit_Date, 
                                             :Removed_AgeLimit_SignedBy, 
                                             :Removed_NotSuitable_Cert,
                                             :Removed_NotSuitable_Date,
                                             :Removed_NotSuitable_SignedBy,

                                             :MilEmployed_AdministrationID,
                                             :MilEmployed_Date,
                                                  
                                             :TemporaryRemoved_ReasonID,
                                             :TemporaryRemoved_Date,
                                             :TemporaryRemoved_Duration,
                                                  
                                             :Postpone_TypeID,
                                             :Postpone_Year,
                                                  
                                             :CreatedBy,
                                             :CreatedDate,
                                             :LastModifiedBy,
                                             :LastModifiedDate);
        
                                    SELECT PMIS_RES.ReservistMilRepStatuses_ID_SEQ.currval INTO :ReservistMilRepStatusID FROM dual;
        
                                    ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddMilRepStatus", logDescription, null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MilRepStatus", "", reservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_EnrolDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.EnrolDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_SourceMilDept", "", reservistMilRepStatus.SourceMilDepartment != null ? reservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", currentUser));

                    switch (reservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey)
                    {
                        case "VOLUNTARY_RESERVE":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryContractNumber", "", reservistMilRepStatus.Voluntary_ContractNumber, currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryContractDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ContractDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryExpireDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ExpireDate), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryDurationMonths", "", reservistMilRepStatus.Voluntary_DurationMonths.HasValue ? reservistMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryFulfilPlace", "", (reservistMilRepStatus.Voluntary_FulfilPlace != null ? reservistMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilitaryRank", "", reservistMilRepStatus.Voluntary_MilitaryRank != null ? reservistMilRepStatus.Voluntary_MilitaryRank.LongName : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilitaryPosition", "", reservistMilRepStatus.Voluntary_MilitaryPosition, currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilRepSpeciality", "", reservistMilRepStatus.Voluntary_MilRepSpeciality != null ? reservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportingSpecialityName : "", currentUser));
                            break;
                        case "REMOVED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Date), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedReason", "", reservistMilRepStatus.Removed_Reason != null ? reservistMilRepStatus.Removed_Reason.Text() : "", currentUser));

                            if (!string.IsNullOrEmpty(removedReason))
                            {
                                if (removedReason == "Починал")
                                {
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDeceasedDeathCert", "", reservistMilRepStatus.Removed_Deceased_DeathCert, currentUser));
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDeceasedDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Deceased_Date), currentUser));
                                }
                                else if (removedReason == "Пределна възраст")
                                {
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitOrder", "", reservistMilRepStatus.Removed_AgeLimit_Order, currentUser));
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Removed_AgeLimit_Date), currentUser));
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitSignedBy", "", reservistMilRepStatus.Removed_AgeLimit_SignedBy, currentUser));
                                }
                                else if (removedReason == "НГВС с изключване")
                                {
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableCert", "", reservistMilRepStatus.Removed_NotSuitable_Cert, currentUser));
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.Removed_NotSuitable_Date), currentUser));
                                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableSignedBy", "", reservistMilRepStatus.Removed_NotSuitable_SignedBy, currentUser));
                                }
                            }
                            break;
                        case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MilEmployedAdministration", "", reservistMilRepStatus.MilEmployed_Administration != null ? reservistMilRepStatus.MilEmployed_Administration.AdministrationName : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MilEmployedDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.MilEmployed_Date), currentUser));
                            break;
                        case "TEMPORARY_REMOVED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedReason", "", reservistMilRepStatus.TemporaryRemoved_Reason != null ? reservistMilRepStatus.TemporaryRemoved_Reason.Text() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.TemporaryRemoved_Date), currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedDuration", "", reservistMilRepStatus.TemporaryRemoved_Duration.HasValue ? reservistMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", currentUser));
                            break;
                        case "POSTPONED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PostponeType", "", reservistMilRepStatus.Postpone_Type != null ? reservistMilRepStatus.Postpone_Type.Text() : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PostponeYear", "", reservistMilRepStatus.Postpone_Year.HasValue ? reservistMilRepStatus.Postpone_Year.Value.ToString() : "", currentUser));
                            break;
                        case "DISCHARGED":
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_DestMilDepartment", "", reservistMilRepStatus.DestMilDepartment != null ? reservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : "", currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_DischargeDate", "", CommonFunctions.FormatDate(reservistMilRepStatus.DischargeDate), currentUser));
                            break;
                    }
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.ReservistMilRepStatuses SET
                                             ReservistID = :ReservistID,
                                             IsCurrent = :IsCurrent,
                                                  
                                             MilitaryReportStatusID = :MilitaryReportStatusID,
                                             EnrolDate = :EnrolDate,
                                             DischargeDate = :DischargeDate,
                                             SourceMilDepartmentID = :SourceMilDepartmentID,
                                             DestMilDepartmentID = :DestMilDepartmentID,
                                                  
                                             Voluntary_ContractNumber = :Voluntary_ContractNumber,
                                             Voluntary_ContractDate = :Voluntary_ContractDate,
                                             Voluntary_ExpireDate = :Voluntary_ExpireDate,
                                             Voluntary_DurationMonths = :Voluntary_DurationMonths,
                                             Voluntary_FulfilPlaceID = :Voluntary_FulfilPlaceID,
                                             Voluntary_MilitaryRankID = :Voluntary_MilitaryRankID,
                                             Voluntary_MilRepSpecialityID = :Voluntary_MilRepSpecialityID,
                                             Voluntary_MilitaryPosition = :Voluntary_MilitaryPosition,
                                                  
                                             Removed_Date = :Removed_Date,
                                             Removed_ReasonID = :Removed_ReasonID,
                                             Removed_Deceased_DeathCert = :Removed_Deceased_DeathCert,
                                             Removed_Deceased_Date = :Removed_Deceased_Date,
                                             Removed_AgeLimit_Order = :Removed_AgeLimit_Order,
                                             Removed_AgeLimit_Date = :Removed_AgeLimit_Date,
                                             Removed_AgeLimit_SignedBy = :Removed_AgeLimit_SignedBy, 
                                             Removed_NotSuitable_Cert = :Removed_NotSuitable_Cert,
                                             Removed_NotSuitable_Date = :Removed_NotSuitable_Date,
                                             Removed_NotSuitable_SignedBy = :Removed_NotSuitable_SignedBy,
                                                  
                                             MilEmployed_AdministrationID = :MilEmployed_AdministrationID,
                                             MilEmployed_Date = :MilEmployed_Date,
                                                  
                                             TemporaryRemoved_ReasonID = :TemporaryRemoved_ReasonID,
                                             TemporaryRemoved_Date = :TemporaryRemoved_Date,
                                             TemporaryRemoved_Duration = :TemporaryRemoved_Duration,
                                                  
                                             Postpone_TypeID = :Postpone_TypeID,
                                             Postpone_Year = :Postpone_Year,
                                       LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                             THEN :LastModifiedBy
                                                             ELSE LastModifiedBy
                                                        END, 
                                       LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                               THEN :LastModifiedDate
                                                               ELSE LastModifiedDate
                                                          END
                                    WHERE ReservistMilRepStatusID = :ReservistMilRepStatusID ;                       
        
                                    ";

                    changeEvent = new ChangeEvent("RES_Reservist_EditMilRepStatus", logDescription, null, person, currentUser);

                    if (oldReservistMilRepStatus.EnrolDate != reservistMilRepStatus.EnrolDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_EnrolDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.EnrolDate), CommonFunctions.FormatDate(reservistMilRepStatus.EnrolDate), currentUser));

                    if ((oldReservistMilRepStatus.SourceMilDepartment != null ? oldReservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "") != (reservistMilRepStatus.SourceMilDepartment != null ? reservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_SourceMilDept", oldReservistMilRepStatus.SourceMilDepartment != null ? oldReservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", reservistMilRepStatus.SourceMilDepartment != null ? reservistMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "", currentUser));

                    switch (reservistMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey)
                    {
                        case "VOLUNTARY_RESERVE":
                            if (oldReservistMilRepStatus.Voluntary_ContractNumber.Trim() != reservistMilRepStatus.Voluntary_ContractNumber.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryContractNumber", oldReservistMilRepStatus.Voluntary_ContractNumber, reservistMilRepStatus.Voluntary_ContractNumber, currentUser));

                            if (oldReservistMilRepStatus.Voluntary_ContractDate != reservistMilRepStatus.Voluntary_ContractDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryContractDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Voluntary_ContractDate), CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ContractDate), currentUser));

                            if (oldReservistMilRepStatus.Voluntary_ExpireDate != reservistMilRepStatus.Voluntary_ExpireDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryExpireDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Voluntary_ExpireDate), CommonFunctions.FormatDate(reservistMilRepStatus.Voluntary_ExpireDate), currentUser));
                            
                            if (oldReservistMilRepStatus.Voluntary_DurationMonths != reservistMilRepStatus.Voluntary_DurationMonths)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryDurationMonths", oldReservistMilRepStatus.Voluntary_DurationMonths.HasValue ? oldReservistMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", reservistMilRepStatus.Voluntary_DurationMonths.HasValue ? reservistMilRepStatus.Voluntary_DurationMonths.Value.ToString() : "", currentUser));
                          
                            if (oldReservistMilRepStatus.Voluntary_FulfilPlaceID != reservistMilRepStatus.Voluntary_FulfilPlaceID)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryFulfilPlace", (oldReservistMilRepStatus.Voluntary_FulfilPlace != null ? oldReservistMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), (reservistMilRepStatus.Voluntary_FulfilPlace != null ? reservistMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : ""), currentUser));
                          
                            if ((oldReservistMilRepStatus.Voluntary_MilitaryRank != null ? oldReservistMilRepStatus.Voluntary_MilitaryRank.LongName : "") != (reservistMilRepStatus.Voluntary_MilitaryRank != null ? reservistMilRepStatus.Voluntary_MilitaryRank.LongName : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilitaryRank", oldReservistMilRepStatus.Voluntary_MilitaryRank != null ? oldReservistMilRepStatus.Voluntary_MilitaryRank.LongName : "", reservistMilRepStatus.Voluntary_MilitaryRank != null ? reservistMilRepStatus.Voluntary_MilitaryRank.LongName : "", currentUser));

                            if (oldReservistMilRepStatus.Voluntary_MilitaryPosition.Trim() != reservistMilRepStatus.Voluntary_MilitaryPosition.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilitaryPosition", oldReservistMilRepStatus.Voluntary_MilitaryPosition, reservistMilRepStatus.Voluntary_MilitaryPosition, currentUser));

                            if ((oldReservistMilRepStatus.Voluntary_MilRepSpeciality != null ? oldReservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportingSpecialityName : "") != (reservistMilRepStatus.Voluntary_MilRepSpeciality != null ? reservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportingSpecialityName : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_VoluntaryMilRepSpeciality", oldReservistMilRepStatus.Voluntary_MilRepSpeciality != null ? oldReservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportingSpecialityName : "", reservistMilRepStatus.Voluntary_MilRepSpeciality != null ? reservistMilRepStatus.Voluntary_MilRepSpeciality.MilReportingSpecialityName : "", currentUser));
                            break;
                        case "REMOVED":
                            if (oldReservistMilRepStatus.Removed_Date != reservistMilRepStatus.Removed_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Removed_Date), CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Date), currentUser));
                            
                            if ((oldReservistMilRepStatus.Removed_Reason != null ? oldReservistMilRepStatus.Removed_Reason.Text() : "") != (reservistMilRepStatus.Removed_Reason != null ? reservistMilRepStatus.Removed_Reason.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedReason", oldReservistMilRepStatus.Removed_Reason != null ? oldReservistMilRepStatus.Removed_Reason.Text() : "", reservistMilRepStatus.Removed_Reason != null ? reservistMilRepStatus.Removed_Reason.Text() : "", currentUser));

                            if (oldReservistMilRepStatus.Removed_Deceased_DeathCert.Trim() != reservistMilRepStatus.Removed_Deceased_DeathCert.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDeceasedDeathCert", oldReservistMilRepStatus.Removed_Deceased_DeathCert, reservistMilRepStatus.Removed_Deceased_DeathCert, currentUser));

                            if (oldReservistMilRepStatus.Removed_Deceased_Date != reservistMilRepStatus.Removed_Deceased_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedDeceasedDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Removed_Deceased_Date), CommonFunctions.FormatDate(reservistMilRepStatus.Removed_Deceased_Date), currentUser));

                            if (oldReservistMilRepStatus.Removed_AgeLimit_Order.Trim() != reservistMilRepStatus.Removed_AgeLimit_Order.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitOrder", oldReservistMilRepStatus.Removed_AgeLimit_Order, reservistMilRepStatus.Removed_AgeLimit_Order, currentUser));

                            if (oldReservistMilRepStatus.Removed_AgeLimit_Date != reservistMilRepStatus.Removed_AgeLimit_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Removed_AgeLimit_Date), CommonFunctions.FormatDate(reservistMilRepStatus.Removed_AgeLimit_Date), currentUser));

                            if (oldReservistMilRepStatus.Removed_AgeLimit_SignedBy.Trim() != reservistMilRepStatus.Removed_AgeLimit_SignedBy.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedAgeLimitSignedBy", oldReservistMilRepStatus.Removed_AgeLimit_SignedBy, reservistMilRepStatus.Removed_AgeLimit_SignedBy, currentUser));

                            if (oldReservistMilRepStatus.Removed_NotSuitable_Cert.Trim() != reservistMilRepStatus.Removed_NotSuitable_Cert.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableCert", oldReservistMilRepStatus.Removed_NotSuitable_Cert, reservistMilRepStatus.Removed_NotSuitable_Cert, currentUser));

                            if (oldReservistMilRepStatus.Removed_NotSuitable_Date != reservistMilRepStatus.Removed_NotSuitable_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.Removed_NotSuitable_Date), CommonFunctions.FormatDate(reservistMilRepStatus.Removed_NotSuitable_Date), currentUser));

                            if (oldReservistMilRepStatus.Removed_NotSuitable_SignedBy.Trim() != reservistMilRepStatus.Removed_NotSuitable_SignedBy.Trim())
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_RemovedNotSuitableSignedBy", oldReservistMilRepStatus.Removed_NotSuitable_SignedBy, reservistMilRepStatus.Removed_NotSuitable_SignedBy, currentUser));
                            break;
                        case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                            if ((oldReservistMilRepStatus.MilEmployed_Administration != null ? oldReservistMilRepStatus.MilEmployed_Administration.AdministrationName : "") != (reservistMilRepStatus.MilEmployed_Administration != null ? reservistMilRepStatus.MilEmployed_Administration.AdministrationName : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MilEmployedAdministration", oldReservistMilRepStatus.MilEmployed_Administration != null ? oldReservistMilRepStatus.MilEmployed_Administration.AdministrationName : "", reservistMilRepStatus.MilEmployed_Administration != null ? reservistMilRepStatus.MilEmployed_Administration.AdministrationName : "", currentUser));

                            if (oldReservistMilRepStatus.MilEmployed_Date != reservistMilRepStatus.MilEmployed_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MilEmployedDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.MilEmployed_Date), CommonFunctions.FormatDate(reservistMilRepStatus.MilEmployed_Date), currentUser));
                            break;
                        case "TEMPORARY_REMOVED":
                            if ((oldReservistMilRepStatus.TemporaryRemoved_Reason != null ? oldReservistMilRepStatus.TemporaryRemoved_Reason.Text() : "") != (reservistMilRepStatus.TemporaryRemoved_Reason != null ? reservistMilRepStatus.TemporaryRemoved_Reason.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedReason", oldReservistMilRepStatus.TemporaryRemoved_Reason != null ? oldReservistMilRepStatus.TemporaryRemoved_Reason.Text() : "", reservistMilRepStatus.TemporaryRemoved_Reason != null ? reservistMilRepStatus.TemporaryRemoved_Reason.Text() : "", currentUser));

                            if (oldReservistMilRepStatus.TemporaryRemoved_Date != reservistMilRepStatus.TemporaryRemoved_Date)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.TemporaryRemoved_Date), CommonFunctions.FormatDate(reservistMilRepStatus.TemporaryRemoved_Date), currentUser));

                            if (oldReservistMilRepStatus.TemporaryRemoved_Duration != reservistMilRepStatus.TemporaryRemoved_Duration)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_TemporaryRemovedDuration", oldReservistMilRepStatus.TemporaryRemoved_Duration.HasValue ? oldReservistMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", reservistMilRepStatus.TemporaryRemoved_Duration.HasValue ? reservistMilRepStatus.TemporaryRemoved_Duration.Value.ToString() : "", currentUser));
                            break;
                        case "POSTPONED":
                            if ((oldReservistMilRepStatus.Postpone_Type != null ? oldReservistMilRepStatus.Postpone_Type.Text() : "") != (reservistMilRepStatus.Postpone_Type != null ? reservistMilRepStatus.Postpone_Type.Text() : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PostponeType", oldReservistMilRepStatus.Postpone_Type != null ? oldReservistMilRepStatus.Postpone_Type.Text() : "", reservistMilRepStatus.Postpone_Type != null ? reservistMilRepStatus.Postpone_Type.Text() : "", currentUser));

                            if (oldReservistMilRepStatus.Postpone_Year != reservistMilRepStatus.Postpone_Year)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PostponeYear", oldReservistMilRepStatus.Postpone_Year.HasValue ? oldReservistMilRepStatus.Postpone_Year.Value.ToString() : "", reservistMilRepStatus.Postpone_Year.HasValue ? reservistMilRepStatus.Postpone_Year.Value.ToString() : "", currentUser));
                            break;
                        case "DISCHARGED":
                            if ((oldReservistMilRepStatus.DestMilDepartment != null ? oldReservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : "") != (reservistMilRepStatus.DestMilDepartment != null ? reservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : ""))
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_DestMilDepartment", oldReservistMilRepStatus.DestMilDepartment != null ? oldReservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : "", reservistMilRepStatus.DestMilDepartment != null ? reservistMilRepStatus.DestMilDepartment.MilitaryDepartmentName : "", currentUser));

                            if (oldReservistMilRepStatus.DischargeDate != reservistMilRepStatus.DischargeDate)
                                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_DischargeDate", CommonFunctions.FormatDate(oldReservistMilRepStatus.DischargeDate), CommonFunctions.FormatDate(reservistMilRepStatus.DischargeDate), currentUser));
                            break;
                    }
                }

                SQL += @"UPDATE vs_owner.vs_ls p SET
                        pochinal = (SELECT CASE WHEN m.militaryreportstatuskey = 'REMOVED' AND g.tablevalue = 'Починал'
                                               THEN 'Y'
                                               ELSE 'N'
                                           END
                                    FROM pmis_res.reservists r
                                    INNER JOIN pmis_res.reservistmilrepstatuses rs ON r.reservistid = rs.reservistid AND rs.iscurrent = 1
                                    INNER JOIN pmis_res.militaryreportstatuses m ON rs.militaryreportstatusid = m.militaryreportstatusid
                                    LEFT OUTER JOIN pmis_res.gtable g ON g.tablename = 'MilRepStat_RemovedReasons' AND g.tablekey = rs.removed_reasonid
                                    WHERE r.reservistid = :ReservistID)
                        WHERE p.personid = (SELECT re.personid FROM pmis_res.reservists re WHERE re.reservistid = :ReservistID);          
                        
                        ";

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramReservistMilRepStatusID = new OracleParameter();
                paramReservistMilRepStatusID.ParameterName = "ReservistMilRepStatusID";
                paramReservistMilRepStatusID.OracleType = OracleType.Number;

                if (reservistMilRepStatus.ReservistMilRepStatusId != 0)
                {
                    paramReservistMilRepStatusID.Direction = ParameterDirection.Input;
                    paramReservistMilRepStatusID.Value = reservistMilRepStatus.ReservistMilRepStatusId;
                }
                else
                {
                    paramReservistMilRepStatusID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramReservistMilRepStatusID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistMilRepStatus.ReservistId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (reservistMilRepStatus.IsCurrent ? 1 : 0);
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryReportStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistMilRepStatus.MilitaryReportStatusId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EnrolDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.EnrolDate.HasValue)
                    param.Value = reservistMilRepStatus.EnrolDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DischargeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.DischargeDate.HasValue)
                    param.Value = reservistMilRepStatus.DischargeDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SourceMilDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.SourceMilDepartmentId.HasValue)
                    param.Value = reservistMilRepStatus.SourceMilDepartmentId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DestMilDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.DestMilDepartmentId.HasValue)
                    param.Value = reservistMilRepStatus.DestMilDepartmentId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ContractNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Voluntary_ContractNumber))
                    param.Value = reservistMilRepStatus.Voluntary_ContractNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ContractDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Voluntary_ContractDate.HasValue)
                    param.Value = reservistMilRepStatus.Voluntary_ContractDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_ExpireDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Voluntary_ExpireDate.HasValue)
                    param.Value = reservistMilRepStatus.Voluntary_ExpireDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_DurationMonths";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Voluntary_DurationMonths.HasValue)
                    param.Value = reservistMilRepStatus.Voluntary_DurationMonths.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_FulfilPlaceID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Voluntary_FulfilPlaceID != null)
                   param.Value = reservistMilRepStatus.Voluntary_FulfilPlaceID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_MilitaryRankID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistMilRepStatus.Voluntary_MilitaryRankId))
                    param.Value = reservistMilRepStatus.Voluntary_MilitaryRankId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_MilRepSpecialityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Voluntary_MilRepSpecialityId.HasValue)
                    param.Value = reservistMilRepStatus.Voluntary_MilRepSpecialityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Voluntary_MilitaryPosition";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Voluntary_MilitaryPosition))
                    param.Value = reservistMilRepStatus.Voluntary_MilitaryPosition;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Removed_Date.HasValue)
                    param.Value = reservistMilRepStatus.Removed_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_ReasonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Removed_ReasonId.HasValue)
                    param.Value = reservistMilRepStatus.Removed_ReasonId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_Deceased_DeathCert";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Removed_Deceased_DeathCert))
                    param.Value = reservistMilRepStatus.Removed_Deceased_DeathCert;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_Deceased_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Removed_Deceased_Date.HasValue)
                    param.Value = reservistMilRepStatus.Removed_Deceased_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_AgeLimit_Order";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Removed_AgeLimit_Order))
                    param.Value = reservistMilRepStatus.Removed_AgeLimit_Order;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_AgeLimit_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Removed_AgeLimit_Date.HasValue)
                    param.Value = reservistMilRepStatus.Removed_AgeLimit_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_AgeLimit_SignedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Removed_AgeLimit_SignedBy))
                    param.Value = reservistMilRepStatus.Removed_AgeLimit_SignedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_NotSuitable_Cert";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Removed_NotSuitable_Cert))
                    param.Value = reservistMilRepStatus.Removed_NotSuitable_Cert;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_NotSuitable_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Removed_NotSuitable_Date.HasValue)
                    param.Value = reservistMilRepStatus.Removed_NotSuitable_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Removed_NotSuitable_SignedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservistMilRepStatus.Removed_NotSuitable_SignedBy))
                    param.Value = reservistMilRepStatus.Removed_NotSuitable_SignedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilEmployed_AdministrationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.MilEmployed_AdministrationId.HasValue)
                    param.Value = reservistMilRepStatus.MilEmployed_AdministrationId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilEmployed_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.MilEmployed_Date.HasValue)
                    param.Value = reservistMilRepStatus.MilEmployed_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_ReasonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.TemporaryRemoved_ReasonId.HasValue)
                    param.Value = reservistMilRepStatus.TemporaryRemoved_ReasonId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_Date";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.TemporaryRemoved_Date.HasValue)
                    param.Value = reservistMilRepStatus.TemporaryRemoved_Date.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TemporaryRemoved_Duration";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.TemporaryRemoved_Duration.HasValue)
                    param.Value = reservistMilRepStatus.TemporaryRemoved_Duration.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Postpone_TypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Postpone_TypeId.HasValue)
                    param.Value = reservistMilRepStatus.Postpone_TypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Postpone_Year";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistMilRepStatus.Postpone_Year.HasValue)
                    param.Value = reservistMilRepStatus.Postpone_Year.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                if (reservistMilRepStatus.ReservistMilRepStatusId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);
                
                PersonUtil.SetPersonModified(person.PersonId, currentUser);
                ReservistUtil.SetReservistModified(reservist.ReservistId, currentUser);

                cmd.ExecuteNonQuery();

                if (reservistMilRepStatus.ReservistMilRepStatusId == 0)
                    reservistMilRepStatus.ReservistMilRepStatusId = DBCommon.GetInt(paramReservistMilRepStatusID.Value);

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
                    PersonUtil.SetPersonModified(person.PersonId, currentUser);
                    ReservistUtil.SetReservistModified(reservist.ReservistId, currentUser);
                }
            }

            return result;
        }

        //When doing a fulfilment for a particular Reservist into a particular Military Command then
        //use this method to change the current Military Reporting Status
        public static void SetMilRepStatusTo_COMPULSORY_RESERVE_MOB_APPOINTMENT(int reservistId, User currentUser, Change changeEntry)
        {
            ReservistMilRepStatus currentReservistMilRepStatus = GetReservistMilRepCurrentStatusByReservistId(reservistId, currentUser);
            ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(currentUser);

            reservistMilRepStatus.ReservistMilRepStatusId = 0;
            reservistMilRepStatus.ReservistId = reservistId;
            reservistMilRepStatus.IsCurrent = true;
            reservistMilRepStatus.MilitaryReportStatusId = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("COMPULSORY_RESERVE_MOB_APPOINTMENT", currentUser).MilitaryReportStatusId;
            reservistMilRepStatus.EnrolDate = DateTime.Now;
            reservistMilRepStatus.SourceMilDepartmentId = currentReservistMilRepStatus.SourceMilDepartmentId;

            SaveReservistMilRepStatus(reservistMilRepStatus, currentUser, changeEntry);
        }

        //When doing a fulfilment for a particular Reservist into a particular Military Command then
        //use this method to change the current Military Reporting Status
        public static void SetMilRepStatusTo_FREE(int reservistId, User currentUser, Change changeEntry)
        {
            ReservistMilRepStatus currentReservistMilRepStatus = GetReservistMilRepCurrentStatusByReservistId(reservistId, currentUser);
            ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(currentUser);

            reservistMilRepStatus.ReservistMilRepStatusId = 0;
            reservistMilRepStatus.ReservistId = reservistId;
            reservistMilRepStatus.IsCurrent = true;
            reservistMilRepStatus.MilitaryReportStatusId = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("FREE", currentUser).MilitaryReportStatusId;
            reservistMilRepStatus.EnrolDate = DateTime.Now;
            reservistMilRepStatus.SourceMilDepartmentId = currentReservistMilRepStatus.SourceMilDepartmentId;

            SaveReservistMilRepStatus(reservistMilRepStatus, currentUser, changeEntry);
        }

        public static void SetMilRepStatusTo_MILITARY_REPORT_PERSONS(int pReservistID, int? pMilitaryDepartmentID, User pCurrentUser, Change pChangeEntry)
        {
            ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(pCurrentUser);

            reservistMilRepStatus.ReservistMilRepStatusId = 0;
            reservistMilRepStatus.ReservistId = pReservistID;
            reservistMilRepStatus.IsCurrent = true;
            reservistMilRepStatus.MilitaryReportStatusId = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("MILITARY_REPORT_PERSONS", pCurrentUser).MilitaryReportStatusId;
            reservistMilRepStatus.EnrolDate = DateTime.Now;

            if (pMilitaryDepartmentID == null)
            {
                ReservistMilRepStatus currentReservistMilRepStatus = GetReservistMilRepCurrentStatusByReservistId(pReservistID, pCurrentUser);
                reservistMilRepStatus.SourceMilDepartmentId = currentReservistMilRepStatus.SourceMilDepartmentId;
            }
            else {
                reservistMilRepStatus.SourceMilDepartmentId = pMilitaryDepartmentID;
            }            

            SaveReservistMilRepStatus(reservistMilRepStatus, pCurrentUser, pChangeEntry);
        }
    
        public static void SetMilRepStatusTo_TEMPORARY_REMOVED(int reservistId, int reasonId, DateTime? date, int? duration, User currentUser, Change changeEntry)
        {
            ReservistMilRepStatus currentReservistMilRepStatus = GetReservistMilRepCurrentStatusByReservistId(reservistId, currentUser);
            ReservistMilRepStatus reservistMilRepStatus = new ReservistMilRepStatus(currentUser);

            reservistMilRepStatus.ReservistMilRepStatusId = 0;
            reservistMilRepStatus.ReservistId = reservistId;
            reservistMilRepStatus.IsCurrent = true;
            reservistMilRepStatus.MilitaryReportStatusId = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("TEMPORARY_REMOVED", currentUser).MilitaryReportStatusId;
            reservistMilRepStatus.EnrolDate = DateTime.Now;
            reservistMilRepStatus.SourceMilDepartmentId = currentReservistMilRepStatus != null ? currentReservistMilRepStatus.SourceMilDepartmentId : null;
            reservistMilRepStatus.TemporaryRemoved_ReasonId = reasonId;
            reservistMilRepStatus.TemporaryRemoved_Date = date;
            reservistMilRepStatus.TemporaryRemoved_Duration = duration;

            SaveReservistMilRepStatus(reservistMilRepStatus, currentUser, changeEntry);
        }
    }
}