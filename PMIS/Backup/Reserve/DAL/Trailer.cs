using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Trailer into the system
    public class Trailer : BaseDbObject
    {
        private int trailerId;
        private int technicsId;
        private Technics technics;
        private string regNumber;
        private string inventoryNumber;
        private int? trailerKindId;
        private GTableItem trailerKind;
        private int? trailerTypeId;
        private GTableItem trailerType;
        private int? trailerBodyKindId;
        private GTableItem trailerBodyKind;
        private decimal? carryingCapacity;
        private DateTime? firstRegistrationDate;
        private DateTime? lastAnnualTechnicalReviewDate;
        private decimal? mileage;
        
        public int TrailerId
        {
            get 
            {
                return trailerId;
            }
            set
            {
                trailerId = value;
            }
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

        public Technics Technics
        {
            get
            {
                if(technics == null)
                   technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                return technics;
            }
            set
            {
                technics = value;
            }
        }

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get
            {
                return inventoryNumber;
            }
            set
            {
                inventoryNumber = value;
            }
        }

        public int? TrailerKindId
        {
            get
            {
                return trailerKindId;
            }
            set
            {
                trailerKindId = value;
            }
        }

        public GTableItem TrailerKind
        {
            get
            {
                //Lazy initialization
                if (trailerKind == null && TrailerKindId.HasValue)
                    trailerKind = GTableItemUtil.GetTableItem("TrailerKind", TrailerKindId.Value, ModuleUtil.RES(), CurrentUser);

                return trailerKind;
            }
            set
            {
                trailerKind = value;
            }
        }

        public int? TrailerTypeId
        {
            get
            {
                return trailerTypeId;
            }
            set
            {
                trailerTypeId = value;
            }
        }

        public GTableItem TrailerType
        {
            get
            {
                //Lazy initialization
                if (trailerType == null && TrailerTypeId.HasValue)
                    trailerType = GTableItemUtil.GetTableItem("TrailerType", TrailerTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return trailerType;
            }
            set
            {
                trailerType = value;
            }
        }

        public int? TrailerBodyKindId
        {
            get
            {
                return trailerBodyKindId;
            }
            set
            {
                trailerBodyKindId = value;
            }
        }

        public GTableItem TrailerBodyKind
        {
            get
            {
                //Lazy initialization
                if (trailerBodyKind == null && TrailerBodyKindId.HasValue)
                    trailerBodyKind = GTableItemUtil.GetTableItem("TrailerBodyKind", TrailerBodyKindId.Value, ModuleUtil.RES(), CurrentUser);

                return trailerBodyKind;
            }
            set
            {
                trailerBodyKind = value;
            }
        }

        public decimal? CarryingCapacity
        {
            get
            {
                return carryingCapacity;
            }
            set
            {
                carryingCapacity = value;
            }
        }

        public DateTime? FirstRegistrationDate
        {
            get
            {
                return firstRegistrationDate;
            }
            set
            {
                firstRegistrationDate = value;
            }
        }

        public DateTime? LastAnnualTechnicalReviewDate
        {
            get
            {
                return lastAnnualTechnicalReviewDate;
            }
            set
            {
                lastAnnualTechnicalReviewDate = value;
            }
        }

        public decimal? Mileage
        {
            get
            {
                return mileage;
            }
            set
            {
                mileage = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public Trailer(User user)
            : base(user)
        {

        }
    }

    public class TrailerRegNumber
    {
        private string regNumber;
        private DateTime changeDate;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public DateTime ChangeDate
        {
            get
            {
                return changeDate;
            }
            set
            {
                changeDate = value;
            }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class TrailerManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string trailerKindId;
        string trailerTypeId;
        string militaryReportStatus;
        string militaryDepartment;
        string ownershipNumber;
        string ownershipName;
        bool isOwnershipAddress;
        string region;
        string municipality;
        string city;
        string district;
        string postCode;
        string address;
        string normativeTechnics;
        string appointmentIsDelivered;
        string readiness;

        int orderBy;
        int pageIdx;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string TrailerKindId
        {
            get { return trailerKindId; }
            set { trailerKindId = value; }
        }

        public string TrailerTypeId
        {
            get { return trailerTypeId; }
            set { trailerTypeId = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string OwnershipNumber
        {
            get { return ownershipNumber; }
            set { ownershipNumber = value; }
        }

        public string OwnershipName
        {
            get { return ownershipName; }
            set { ownershipName = value; }
        }

        public bool IsOwnershipAddress
        {
            get { return isOwnershipAddress; }
            set { isOwnershipAddress = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Municipality
        {
            get { return municipality; }
            set { municipality = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string District
        {
            get { return district; }
            set { district = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string NormativeTechnics
        {
            get { return normativeTechnics; }
            set { normativeTechnics = value; }
        }

        public string AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }

        public string Readiness
        {
            get { return readiness; }
            set { readiness = value; }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public class TrailerManageBlock
    {
        private int technicsId;
        private int trailerId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string trailerKind;
        string trailerType;
        string bodyType;        
        string militaryReportStatus;
        string militaryDepartment;
        string ownership;
        string address;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int TrailerId
        {
            get { return trailerId; }
            set { trailerId = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string TrailerKind
        {
            get { return trailerKind; }
            set { trailerKind = value; }
        }

        public string TrailerType
        {
            get { return trailerType; }
            set { trailerType = value; }
        }

        public string BodyType
        {
            get { return bodyType; }
            set { bodyType = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string Ownership
        {
            get { return ownership; }
            set { ownership = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string NormativeTechnicsCode
        {
            get { return normativeTechnicsCode; }
            set { normativeTechnicsCode = value; }
        }

        public string NormativeTechnicsName
        {
            get { return normativeTechnicsName; }
            set { normativeTechnicsName = value; }
        }
    }

    public class TrailerFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int trailerID;        
        private Trailer trailer;
        private bool appointmentIsDelivered;

        public int FulfilTechnicsRequestID
        {
            get { return fulfilTechnicsRequestID; }
            set { fulfilTechnicsRequestID = value; }
        }

        public int TechnicReadinessID
        {
            get { return technicReadinessID; }
            set { technicReadinessID = value; }
        }

        public string TechnicReadiness
        {
            get
            {
                return ReadinessUtil.ReadinessName(TechnicReadinessID);
            }
        }

        public int TrailerID
        {
            get { return trailerID; }
            set { trailerID = value; }
        }

        public Trailer Trailer
        {
            get { return trailer; }
            set { trailer = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class TrailerSearchFilter
    {
        string regNumber;
        string inventoryNumber;        
        string technicsCategoryId;
        string trailerKindId;
        string trailerTypeId;
        string ownershipNumber;
        string ownershipName;
        bool isOwnershipAddress;
        string region;
        string municipality;
        string city;
        string district;
        string postCode;
        string address;
        string normativeTechnics;

        int orderBy;
        int pageIdx;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string TrailerKindId
        {
            get { return trailerKindId; }
            set { trailerKindId = value; }
        }

        public string TrailerTypeId
        {
            get { return trailerTypeId; }
            set { trailerTypeId = value; }
        }

        public string OwnershipNumber
        {
            get { return ownershipNumber; }
            set { ownershipNumber = value; }
        }

        public string OwnershipName
        {
            get { return ownershipName; }
            set { ownershipName = value; }
        }

        public bool IsOwnershipAddress
        {
            get { return isOwnershipAddress; }
            set { isOwnershipAddress = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Municipality
        {
            get { return municipality; }
            set { municipality = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string District
        {
            get { return district; }
            set { district = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string NormativeTechnics
        {
            get { return normativeTechnics; }
            set { normativeTechnics = value; }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public class TrailerSearchBlock
    {
        private int technicsId;
        private int trailerId;        
        private string regNumber;
        string inventoryNumber;              
        string technicsCategory;        
        string trailerKind;        
        string trailerType;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int TrailerId
        {
            get { return trailerId; }
            set { trailerId = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string TrailerKind
        {
            get { return trailerKind; }
            set { trailerKind = value; }
        }

        public string TrailerType
        {
            get { return trailerType; }
            set { trailerType = value; }
        }

        public string Ownership
        {
            get { return ownership; }
            set { ownership = value; }
        }

        public string NormativeTechnicsCode
        {
            get { return normativeTechnicsCode; }
            set { normativeTechnicsCode = value; }
        }

        public string NormativeTechnicsName
        {
            get { return normativeTechnicsName; }
            set { normativeTechnicsName = value; }
        }
    }

    public static class TrailerUtil
    {
        //This method creates and returns a Trailer object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Trailer ExtractTrailer(OracleDataReader dr, User currentUser)
        {
            Trailer trailer = new Trailer(currentUser);

            trailer.TrailerId = DBCommon.GetInt(dr["TrailerID"]);
            trailer.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            trailer.RegNumber = dr["RegNumber"].ToString();
            trailer.InventoryNumber = dr["InventoryNumber"].ToString();
            trailer.TrailerKindId = (DBCommon.IsInt(dr["TrailerKindID"]) ? DBCommon.GetInt(dr["TrailerKindID"]) : (int?)null);
            trailer.TrailerTypeId = (DBCommon.IsInt(dr["TrailerTypeID"]) ? DBCommon.GetInt(dr["TrailerTypeID"]) : (int?)null);
            trailer.TrailerBodyKindId = (DBCommon.IsInt(dr["TrailerBodyKindID"]) ? DBCommon.GetInt(dr["TrailerBodyKindID"]) : (int?)null);
            trailer.CarryingCapacity = (DBCommon.IsDecimal(dr["CarryingCapacity"]) ? DBCommon.GetDecimal(dr["CarryingCapacity"]) : (decimal?)null);
            trailer.FirstRegistrationDate = (dr["FirstRegistrationDate"] is DateTime ? (DateTime)dr["FirstRegistrationDate"] : (DateTime?)null);
            trailer.LastAnnualTechnicalReviewDate = (dr["LastAnnualTechnicalReviewDate"] is DateTime ? (DateTime)dr["LastAnnualTechnicalReviewDate"] : (DateTime?)null);
            trailer.Mileage = (DBCommon.IsDecimal(dr["Mileage"]) ? DBCommon.GetDecimal(dr["Mileage"]) : (decimal?)null);

            return trailer;
        }

        //Get a particular object by its ID
        public static Trailer GetTrailer(int trailerId, User currentUser)
        {
            Trailer trailer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TrailerID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TrailerKindID, a.TrailerTypeID, a.TrailerBodyKindID, a.CarryingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate, a.Mileage
                               FROM PMIS_RES.Trailers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TrailerID = :TrailerID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TrailerID", OracleType.Number).Value = trailerId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    trailer = ExtractTrailer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailer;
        }

        //Get a particular object by its ID
        public static Trailer GetTrailerByTechnicsId(int technicsId, User currentUser)
        {
            Trailer trailer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TrailerID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TrailerKindID, a.TrailerTypeID, a.TrailerBodyKindID, a.CarryingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate, a.Mileage
                               FROM PMIS_RES.Trailers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    trailer = ExtractTrailer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailer;
        }

        //Get a particular object by its reg number
        public static Trailer GetTrailerByRegNumber(string regNumber, User currentUser)
        {
            Trailer trailer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TrailerID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TrailerKindID, a.TrailerTypeID, a.TrailerBodyKindID, a.CarryingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate, a.Mileage
                               FROM PMIS_RES.Trailers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RegNumber = :RegNumber 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RegNumber", OracleType.VarChar).Value = regNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    trailer = ExtractTrailer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailer;
        }

        //Get all Trailer objects
        public static List<Trailer> GetAllTrailers(User currentUser)
        {
            List<Trailer> trailers = new List<Trailer>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TrailerID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TrailerKindID, a.TrailerTypeID, a.TrailerBodyKindID, a.CarryingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate, a.Mileage
                               FROM PMIS_RES.Trailers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    trailers.Add(ExtractTrailer(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailers;
        }

        public static List<TrailerFulfilmentBlock> GetAllTrailerFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TrailerFulfilmentBlock> trailers = new List<TrailerFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND e.CreatedBy = " + currentUser.UserId.ToString();
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
                        orderBySQL = "b.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "c.TableValue";
                        break;
                    case 3:
                        orderBySQL = "d.TableValue";
                        break;
                    case 4:
                        orderBySQL = "a.TechnicReadinessID";
                        break;
                    case 5:
                        orderBySQL = "n.NormativeCode";
                        break;
                    case 6:
                        orderBySQL = "a.AppointmentIsDelivered";
                        break;
                    default:
                        orderBySQL = "b.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.TrailerID, 
                                      tmp.TechnicsID, 
                                      tmp.RegNumber, 
                                      tmp.InventoryNumber,
                                      tmp.TrailerKindID,
                                      tmp.TrailerTypeID,
                                      tmp.TrailerBodyKindID,
                                      tmp.CarryingCapacity,
                                      tmp.FirstRegistrationDate,
                                      tmp.LastAnnualTechnicalReviewDate,
                                      tmp.Mileage,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.TrailerID, 
                                             b.TechnicsID, 
                                             b.RegNumber, 
                                             b.InventoryNumber,
                                             b.TrailerKindID, 
                                             b.TrailerTypeID, 
                                             b.TrailerBodyKindID,
                                             b.CarryingCapacity,
                                             b.FirstRegistrationDate,
                                             b.LastAnnualTechnicalReviewDate,
                                             b.Mileage,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Trailers b ON a.TechnicsID = b.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.GTable c ON b.TrailerKindID = c.TableKey AND c.TableName = 'TrailerKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable d ON b.TrailerTypeID = d.TableKey AND d.TableName = 'TrailerType'
                                      INNER JOIN PMIS_RES.Technics e ON b.TechnicsID = e.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON e.NormativeTechnicsID = n.NormativeTechnicsID
                                      WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID
                                      " + addWhere + @"
                                      ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID
                                    ) tmp " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TrailerFulfilmentBlock block = new TrailerFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.TrailerID = DBCommon.GetInt(dr["TrailerID"]);
                    block.Trailer = ExtractTrailer(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    trailers.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailers;
        }

        public static int GetAllTrailerFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int trailers = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Trailers b ON a.TechnicsID = b.TechnicsID
                                      INNER JOIN PMIS_RES.Technics c ON b.TechnicsID = c.TechnicsID
                                      WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID
                                      " + addWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    trailers = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailers;
        }

        //Save a particular object into the DB
        public static bool SaveTrailer(Trailer trailer, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + trailer.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (trailer.TrailerId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.Trailers (TechnicsID, RegNumber, 
                                InventoryNumber, TrailerKindID, TrailerTypeID, TrailerBodyKindID,
                                CarryingCapacity, FirstRegistrationDate, 
                                LastAnnualTechnicalReviewDate, Mileage)
                            VALUES (:TechnicsID, :RegNumber, 
                                :InventoryNumber, :TrailerKindID, :TrailerTypeID, :TrailerBodyKindID,
                                :CarryingCapacity, :FirstRegistrationDate, 
                                :LastAnnualTechnicalReviewDate, :Mileage);

                            SELECT PMIS_RES.Trailers_ID_SEQ.currval INTO :TrailerID FROM dual;

                            INSERT INTO PMIS_RES.TrailerRegNumbers (TrailerID, RegNumber, ChangeDate)
                            VALUES (:TrailerID, :RegNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_TRAILERS_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_RegNumber", "", trailer.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_InventoryNumber", "", trailer.InventoryNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerKind", "", trailer.TrailerKindId.HasValue ? trailer.TrailerKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerType", "", trailer.TrailerTypeId.HasValue ? trailer.TrailerType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerBodyKind", "", trailer.TrailerBodyKindId.HasValue ? trailer.TrailerBodyKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_CarryingCapacity", "", trailer.CarryingCapacity.HasValue ? trailer.CarryingCapacity.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_FirstRegistrationDate", "", CommonFunctions.FormatDate(trailer.FirstRegistrationDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_LastAnnualTechnicalReviewDate", "", CommonFunctions.FormatDate(trailer.LastAnnualTechnicalReviewDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_Mileage", "", trailer.Mileage.HasValue ? trailer.Mileage.Value.ToString() : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.Trailers SET
                               RegNumber = :RegNumber,
                               InventoryNumber = :InventoryNumber,
                               TrailerKindID = :TrailerKindID,
                               TrailerTypeID = :TrailerTypeID,
                               TrailerBodyKindID = :TrailerBodyKindID,
                               CarryingCapacity = :CarryingCapacity,
                               FirstRegistrationDate = :FirstRegistrationDate,
                               LastAnnualTechnicalReviewDate = :LastAnnualTechnicalReviewDate,
                               Mileage = :Mileage
                             WHERE TrailerID = :TrailerID ;

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_TRAILERS_Edit", logDescription, null, null, currentUser);

                    Trailer oldTrailer = GetTrailer(trailer.TrailerId, currentUser);

                    if (oldTrailer.RegNumber.Trim() != trailer.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_RegNumber", oldTrailer.RegNumber, trailer.RegNumber, currentUser));

                    if (oldTrailer.InventoryNumber.Trim() != trailer.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_InventoryNumber", oldTrailer.InventoryNumber, trailer.InventoryNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldTrailer.TrailerKindId, trailer.TrailerKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerKind", oldTrailer.TrailerKindId.HasValue ? oldTrailer.TrailerKind.TableValue : "", trailer.TrailerKindId.HasValue ? trailer.TrailerKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldTrailer.TrailerTypeId, trailer.TrailerTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerType", oldTrailer.TrailerTypeId.HasValue ? oldTrailer.TrailerType.TableValue : "", trailer.TrailerTypeId.HasValue ? trailer.TrailerType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldTrailer.TrailerBodyKindId, trailer.TrailerBodyKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_TrailerBodyKind", oldTrailer.TrailerBodyKindId.HasValue ? oldTrailer.TrailerBodyKind.TableValue : "", trailer.TrailerBodyKindId.HasValue ? trailer.TrailerBodyKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldTrailer.CarryingCapacity, trailer.CarryingCapacity))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_CarryingCapacity", oldTrailer.CarryingCapacity.HasValue ? oldTrailer.CarryingCapacity.ToString() : "", trailer.CarryingCapacity.HasValue ? trailer.CarryingCapacity.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldTrailer.FirstRegistrationDate, trailer.FirstRegistrationDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_FirstRegistrationDate", CommonFunctions.FormatDate(oldTrailer.FirstRegistrationDate), CommonFunctions.FormatDate(trailer.FirstRegistrationDate), currentUser));

                    if (!CommonFunctions.IsEqual(oldTrailer.LastAnnualTechnicalReviewDate, trailer.LastAnnualTechnicalReviewDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_LastAnnualTechnicalReviewDate", CommonFunctions.FormatDate(oldTrailer.LastAnnualTechnicalReviewDate), CommonFunctions.FormatDate(trailer.LastAnnualTechnicalReviewDate), currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldTrailer.Mileage, trailer.Mileage))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_Mileage", oldTrailer.Mileage.HasValue ? oldTrailer.Mileage.ToString() : "", trailer.Mileage.HasValue ? trailer.Mileage.ToString() : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(trailer.Technics, currentUser, changeEvent);
                trailer.TechnicsId = trailer.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTrailerID = new OracleParameter();
                paramTrailerID.ParameterName = "TrailerID";
                paramTrailerID.OracleType = OracleType.Number;

                if (trailer.TrailerId != 0)
                {
                    paramTrailerID.Direction = ParameterDirection.Input;
                    paramTrailerID.Value = trailer.TrailerId;
                }
                else
                {
                    paramTrailerID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTrailerID);

                OracleParameter param = null;

                if (trailer.TrailerId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = trailer.TechnicsId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "ChangeDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "RegNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(trailer.RegNumber))
                    param.Value = trailer.RegNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(trailer.InventoryNumber))
                    param.Value = trailer.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TrailerKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (trailer.TrailerKindId.HasValue)
                    param.Value = trailer.TrailerKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TrailerTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (trailer.TrailerTypeId.HasValue)
                    param.Value = trailer.TrailerTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TrailerBodyKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (trailer.TrailerBodyKindId.HasValue)
                    param.Value = trailer.TrailerBodyKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CarryingCapacity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (trailer.CarryingCapacity.HasValue)
                    param.Value = trailer.CarryingCapacity.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FirstRegistrationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (trailer.FirstRegistrationDate.HasValue)
                    param.Value = trailer.FirstRegistrationDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastAnnualTechnicalReviewDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (trailer.LastAnnualTechnicalReviewDate.HasValue)
                    param.Value = trailer.LastAnnualTechnicalReviewDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Mileage";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (trailer.Mileage.HasValue)
                    param.Value = trailer.Mileage.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (trailer.TrailerId == 0)
                    trailer.TrailerId = DBCommon.GetInt(paramTrailerID.Value);

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

        public static List<TrailerManageBlock> GetAllTrailerManageBlocks(TrailerManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<TrailerManageBlock> trailerManageBlocks = new List<TrailerManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerKindID IN (" + filter.TrailerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerTypeID IN (" + filter.TrailerTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    TechMilitaryReportStatus removed = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") + @" NVL(i.TechMilitaryReportStatusID,0) <> " + removed.TechMilitaryReportStatusId + " ";

                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND NVL(ft.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND ft.TechnicReadinessID = " + int.Parse(filter.Readiness).ToString() + ")) ";
                }


                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 3:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 4:
                        orderBySQL = "d.TableValue";
                        break;
                    case 5:
                        orderBySQL = "e.TableValue";
                        break;
                    case 6:
                        orderBySQL = "h.MilitaryDepartmentName";
                        break;
                    case 7:
                        orderBySQL = "g.TechMilitaryReportStatusName";
                        break;
                    case 8:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 9:
                        orderBySQL = "PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address)";
                        break;
                    case 10:
                        orderBySQL = "k.TableValue";
                        break;
                    case 11:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.TrailerID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as TrailerKind,
                                       e.TableValue as TrailerType,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       k.TableValue as BodyType,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Trailers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.TrailerKindId = d.TableKey AND d.TableName = 'TrailerKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.TrailerTypeId = e.TableKey AND e.TableName = 'TrailerType'                                
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.GTable k ON a.TrailerBodyKindID = k.TableKey AND k.TableName = 'TrailerBodyKind'
                                LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON b.NormativeTechnicsID = n.NormativeTechnicsID
                                LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON a.TechnicsID = ft.TechnicsID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.TechnicsID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TrailerManageBlock trailerManageBlock = new TrailerManageBlock();

                    trailerManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    trailerManageBlock.TrailerId = DBCommon.GetInt(dr["TrailerID"]);
                    trailerManageBlock.RegNumber = dr["RegNumber"].ToString();
                    trailerManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    trailerManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    trailerManageBlock.TrailerKind = dr["TrailerKind"].ToString();
                    trailerManageBlock.TrailerType = dr["TrailerType"].ToString();
                    trailerManageBlock.BodyType = dr["BodyType"].ToString();
                    trailerManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    trailerManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    trailerManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        trailerManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        trailerManageBlock.Ownership = "";
                    }

                    trailerManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    trailerManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    trailerManageBlocks.Add(trailerManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailerManageBlocks;
        }

        public static int GetAllTrailerManageBlocksCount(TrailerManageFilter filter, User currentUser)
        {
            int trailerManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerKindID IN (" + filter.TrailerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerTypeID IN (" + filter.TrailerTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    TechMilitaryReportStatus removed = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") + @" NVL(i.TechMilitaryReportStatusID,0) <> " + removed.TechMilitaryReportStatusId + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND NVL(ft.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND ft.TechnicReadinessID = " + int.Parse(filter.Readiness).ToString() + ")) ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Trailers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON a.TechnicsID = ft.TechnicsID
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        trailerManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailerManageBlocksCnt;
        }

        public static List<TrailerSearchBlock> GetAllTrailerSearchBlocks(TrailerSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<TrailerSearchBlock> trailerSearchBlocks = new List<TrailerSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerKindID IN (" + filter.TrailerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerTypeID IN (" + filter.TrailerTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                where += (where == "" ? "" : " AND ") +
                             @" a.TechnicsID NOT IN ( SELECT TechnicsID 
                                                       FROM PMIS_RES.FulfilTechnicsRequest 
                                                       WHERE TechnicsRequestCmdPositionID = " + requestCommandPositionID + @" AND
                                                             MilitaryDepartmentID = " + militaryDepartmentID + " ) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 3:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 4:
                        orderBySQL = "d.TableValue";
                        break;
                    case 5:
                        orderBySQL = "e.TableValue";
                        break;
                    case 6:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 7:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.TrailerID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as TrailerKind,
                                       d.TableValue as TrailerType,
                                       j.CompanyName, j.UnifiedIdentityCode, 
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Trailers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.TrailerKindId = d.TableKey AND d.TableName = 'TrailerKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.TrailerTypeId = e.TableKey AND e.TableName = 'TrailerType'
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON b.NormativeTechnicsID = n.NormativeTechnicsID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.TechnicsID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TrailerSearchBlock trailerSearchBlock = new TrailerSearchBlock();

                    trailerSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    trailerSearchBlock.TrailerId = DBCommon.GetInt(dr["TrailerID"]);
                    trailerSearchBlock.RegNumber = dr["RegNumber"].ToString();
                    trailerSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    trailerSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    trailerSearchBlock.TrailerKind = dr["TrailerKind"].ToString();
                    trailerSearchBlock.TrailerType = dr["TrailerType"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        trailerSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        trailerSearchBlock.Ownership = "";
                    }

                    trailerSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    trailerSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    trailerSearchBlocks.Add(trailerSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailerSearchBlocks;
        }

        public static int GetAllTrailerSearchBlocksCount(TrailerSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int trailerSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRAILERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerKindID IN (" + filter.TrailerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TrailerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TrailerTypeID IN (" + filter.TrailerTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                where += (where == "" ? "" : " AND ") +
                             @" a.TechnicsID NOT IN ( SELECT TechnicsID 
                                                       FROM PMIS_RES.FulfilTechnicsRequest 
                                                       WHERE TechnicsRequestCmdPositionID = " + requestCommandPositionID + @" AND
                                                             MilitaryDepartmentID = " + militaryDepartmentID + " ) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Trailers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        trailerSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailerSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeRegNumber(int trailerId, string newRegNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Trailer trailer = GetTrailer(trailerId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + trailer.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.Trailers SET
                              RegNumber = :NewRegNumber
                           WHERE TrailerID = :TrailerID;

                           INSERT INTO PMIS_RES.TrailerRegNumbers (TrailerID, RegNumber, ChangeDate)
                           VALUES (:TrailerID, :NewRegNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeRegNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRAILERS_RegNumber", trailer.RegNumber, newRegNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "TrailerID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = trailerId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NewRegNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = newRegNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(trailer.TechnicsId, currentUser);

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

        //Get all reg number (history) by Trailer with pagination
        public static List<TrailerRegNumber> GetAllTrailerRegNumbers(int trailerId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TrailerRegNumber> trailerRegNumbers = new List<TrailerRegNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.TrailerRegNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.RegNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.TrailerRegNumberID) as RowNumber
                                               FROM PMIS_RES.TrailerRegNumbers a
                                               WHERE a.TrailerID = :TrailerID
                                               ORDER BY " + orderBySQL + @", a.TrailerRegNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TrailerID", OracleType.Number).Value = trailerId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TrailerRegNumber regNumber = new TrailerRegNumber();
                    regNumber.RegNumber = dr["RegNumber"].ToString();
                    regNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    trailerRegNumbers.Add(regNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return trailerRegNumbers;
        }

        //Get all reg number (history) count by Trailer for pagination
        public static int GetAllTrailerRegNumbersCount(int trailerId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.TrailerRegNumbers a
                               WHERE a.TrailerID = :TrailerID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TrailerID", OracleType.Number).Value = trailerId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        count = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return count;
        }
    }
}