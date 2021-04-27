using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Vehicle into the system
    public class Vehicle : BaseDbObject
    {
        private int vehicleId;
        private int technicsId;
        private Technics technics;
        private string regNumber;
        private string inventoryNumber;
        
        //private int? vehicleMakeId;
        //private VehicleMake vehicleMake;
        //private int? vehicleModelId;
        //private VehicleModel vehicleModel;
        
        private string vehicleMakeName;
        private string vehicleModelName;
        
        private int? vehicleKindId;
        private GTableItem vehicleKind;
        private int? vehicleRoadabilityId;
        private GTableItem vehicleRoadability;
        private decimal? carryingCapacity;
        private decimal? loadingCapacity;
        private DateTime? firstRegistrationDate;
        private DateTime? lastAnnualTechnicalReviewDate;
        private string seats;
        private decimal? mileage;
        private int? vehicleEngineTypeId;
        private GTableItem vehicleEngineType;
        private int? vehicleBodyTypeId;
        private GTableItem vehicleBodyType;
        
        public int VehicleId
        {
            get
            {
                return vehicleId;
            }
            set
            {
                vehicleId = value;
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

        //public int? VehicleMakeId
        //{
        //    get
        //    {
        //        return vehicleMakeId;
        //    }
        //    set
        //    {
        //        vehicleMakeId = value;
        //    }
        //}

        //public VehicleMake VehicleMake
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (vehicleMake == null && VehicleMakeId.HasValue)
        //            vehicleMake = VehicleMakeUtil.GetVehicleMake(VehicleMakeId.Value, CurrentUser);

        //        return vehicleMake;
        //    }
        //    set
        //    {
        //        vehicleMake = value;
        //    }
        //}

        //public int? VehicleModelId
        //{
        //    get
        //    {
        //        return vehicleModelId;
        //    }
        //    set
        //    {
        //        vehicleModelId = value;
        //    }
        //}

        //public VehicleModel VehicleModel
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (vehicleModel == null && VehicleModelId.HasValue)
        //            vehicleModel = VehicleModelUtil.GetVehicleModel(VehicleModelId.Value, CurrentUser);

        //        return vehicleModel;
        //    }
        //    set
        //    {
        //        vehicleModel = value;
        //    }
        //}

        public string VehicleMakeName
        {
            get
            {
                return vehicleMakeName;
            }
            set
            {
                vehicleMakeName = value;
            }
        }

        public string VehicleModelName
        {
            get
            {
                return vehicleModelName;
            }
            set
            {
                vehicleModelName = value;
            }
        }

        public int? VehicleKindId
        {
            get
            {
                return vehicleKindId;
            }
            set
            {
                vehicleKindId = value;
            }
        }

        public GTableItem VehicleKind
        {
            get
            {
                //Lazy initialization
                if (vehicleKind == null && VehicleKindId.HasValue)
                    vehicleKind = GTableItemUtil.GetTableItem("VehicleKind", VehicleKindId.Value, ModuleUtil.RES(), CurrentUser);

                return vehicleKind;
            }
            set
            {
                vehicleKind = value;
            }
        }

        public int? VehicleRoadabilityId
        {
            get
            {
                return vehicleRoadabilityId;
            }
            set
            {
                vehicleRoadabilityId = value;
            }
        }

        public GTableItem VehicleRoadability
        {
            get
            {
                //Lazy initialization
                if (vehicleRoadability == null && VehicleRoadabilityId.HasValue)
                    vehicleRoadability = GTableItemUtil.GetTableItem("VehicleRoadability", VehicleRoadabilityId.Value, ModuleUtil.RES(), CurrentUser);

                return vehicleRoadability;
            }
            set
            {
                vehicleRoadability = value;
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

        public decimal? LoadingCapacity
        {
            get
            {
                return loadingCapacity;
            }
            set
            {
                loadingCapacity = value;
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

        public string Seats
        {
            get
            {
                return seats;
            }
            set
            {
                seats = value;
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

        public int? VehicleEngineTypeId
        {
            get
            {
                return vehicleEngineTypeId;
            }
            set
            {
                vehicleEngineTypeId = value;
            }
        }

        public GTableItem VehicleEngineType
        {
            get
            {
                //Lazy initialization
                if (vehicleEngineType == null && VehicleEngineTypeId.HasValue)
                    vehicleEngineType = GTableItemUtil.GetTableItem("VehicleEngineType", VehicleEngineTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return vehicleEngineType;
            }
            set
            {
                vehicleEngineType = value;
            }
        }

        public int? VehicleBodyTypeId
        {
            get
            {
                return vehicleBodyTypeId;
            }
            set
            {
                vehicleBodyTypeId = value;
            }
        }

        public GTableItem VehicleBodyType
        {
            get
            {
                //Lazy initialization
                if (vehicleBodyType == null && VehicleBodyTypeId.HasValue)
                    vehicleBodyType = GTableItemUtil.GetTableItem("VehicleBodyType", VehicleBodyTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return vehicleBodyType;
            }
            set
            {
                vehicleBodyType = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public Vehicle(User user)
            : base(user)
        {

        }
    }

    public class VehicleRegNumber
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
    public class VehicleManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string vehicleKindId;

        //string vehicleMakeId;
        //string vehicleModelId;

        string vehicleMakeName;
        string vehicleModelName;

        string vehicleBodyTypeId;
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

        public string VehicleKindId
        {
            get { return vehicleKindId; }
            set { vehicleKindId = value; }
        }

        //public string VehicleMakeId
        //{
        //    get { return vehicleMakeId; }
        //    set { vehicleMakeId = value; }
        //}

        //public string VehicleModelId
        //{
        //    get { return vehicleModelId; }
        //    set { vehicleModelId = value; }
        //}

        public string VehicleMakeName
        {
            get { return vehicleMakeName; }
            set { vehicleMakeName = value; }
        }

        public string VehicleModelName
        {
            get { return vehicleModelName; }
            set { vehicleModelName = value; }
        }

        public string VehicleBodyTypeId
        {
            get { return vehicleBodyTypeId; }
            set { vehicleBodyTypeId = value; }
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

    public class VehicleManageBlock
    {
        private int technicsId;
        private int vehicleId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string vehicleKind;
        
        //string vehicleMake;
        //string vehicleModel;

        string vehicleMakeName;
        string vehicleModelName;

        string militaryReportStatus;
        string militaryDepartment;
        string ownership;
        string bodyType;
        string address;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int VehicleId
        {
            get { return vehicleId; }
            set { vehicleId = value; }
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

        public string VehicleKind
        {
            get { return vehicleKind; }
            set { vehicleKind = value; }
        }

        //public string VehicleMake
        //{
        //    get { return vehicleMake; }
        //    set { vehicleMake = value; }
        //}

        //public string VehicleModel
        //{
        //    get { return vehicleModel; }
        //    set { vehicleModel = value; }
        //}

        public string VehicleMakeName
        {
            get { return vehicleMakeName; }
            set { vehicleMakeName = value; }
        }

        public string VehicleModelName
        {
            get { return vehicleModelName; }
            set { vehicleModelName = value; }
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

        public string BodyType
        {
            get { return bodyType; }
            set { bodyType = value; }
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

    public class VehicleFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int vehicleID;        
        private Vehicle vehicle;
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

        public int VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }

        public Vehicle Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class VehicleSearchFilter
    {
        string regNumber;
        string inventoryNumber;        
        string technicsCategoryId;        
        string vehicleKindId;

        //string vehicleMakeId;
        //string vehicleModelId;

        string vehicleMakeName;
        string vehicleModelName;
        
        string vehicleBodyTypeId;
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

        public string VehicleKindId
        {
            get { return vehicleKindId; }
            set { vehicleKindId = value; }
        }

        //public string VehicleMakeId
        //{
        //    get { return vehicleMakeId; }
        //    set { vehicleMakeId = value; }
        //}

        //public string VehicleModelId
        //{
        //    get { return vehicleModelId; }
        //    set { vehicleModelId = value; }
        //}

        public string VehicleMakeName
        {
            get { return vehicleMakeName; }
            set { vehicleMakeName = value; }
        }

        public string VehicleModelName
        {
            get { return vehicleModelName; }
            set { vehicleModelName = value; }
        }

        public string VehicleBodyTypeId
        {
            get { return vehicleBodyTypeId; }
            set { vehicleBodyTypeId = value; }
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

    public class VehicleSearchBlock
    {
        private int technicsId;
        private int vehicleId;        
        private string regNumber;
        string inventoryNumber;              
        string technicsCategory;        
        string vehicleKind;        
        string vehicleMake;        
        string vehicleModel;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int VehicleId
        {
            get { return vehicleId; }
            set { vehicleId = value; }
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

        public string VehicleKind
        {
            get { return vehicleKind; }
            set { vehicleKind = value; }
        }

        public string VehicleMake
        {
            get { return vehicleMake; }
            set { vehicleMake = value; }
        }

        public string VehicleModel
        {
            get { return vehicleModel; }
            set { vehicleModel = value; }
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

    public static class VehicleUtil
    {
        //This method creates and returns a Vehicle object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Vehicle ExtractVehicle(OracleDataReader dr, User currentUser)
        {
            Vehicle vehicle = new Vehicle(currentUser);

            vehicle.VehicleId = DBCommon.GetInt(dr["VehicleID"]);
            vehicle.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            vehicle.RegNumber = dr["RegNumber"].ToString();
            vehicle.InventoryNumber = dr["InventoryNumber"].ToString();

            //vehicle.VehicleMakeId = (DBCommon.IsInt(dr["VehicleMakeID"]) ? DBCommon.GetInt(dr["VehicleMakeID"]) : (int?)null);
            //vehicle.VehicleModelId = (DBCommon.IsInt(dr["VehicleModelID"]) ? DBCommon.GetInt(dr["VehicleModelID"]) : (int?)null);
            
            vehicle.VehicleMakeName = dr["VehicleMakeName"].ToString();
            vehicle.VehicleModelName = dr["VehicleModelName"].ToString();

            vehicle.VehicleKindId = (DBCommon.IsInt(dr["VehicleKindID"]) ? DBCommon.GetInt(dr["VehicleKindID"]) : (int?)null);
            vehicle.VehicleRoadabilityId = (DBCommon.IsInt(dr["VehicleRoadabilityID"]) ? DBCommon.GetInt(dr["VehicleRoadabilityID"]) : (int?)null);
            vehicle.CarryingCapacity = (DBCommon.IsDecimal(dr["CarryingCapacity"]) ? DBCommon.GetDecimal(dr["CarryingCapacity"]) : (decimal?)null);
            vehicle.LoadingCapacity = (DBCommon.IsDecimal(dr["LoadingCapacity"]) ? DBCommon.GetDecimal(dr["LoadingCapacity"]) : (decimal?)null);
            vehicle.FirstRegistrationDate = (dr["FirstRegistrationDate"] is DateTime ? (DateTime)dr["FirstRegistrationDate"] : (DateTime?)null);
            vehicle.LastAnnualTechnicalReviewDate = (dr["LastAnnualTechnicalReviewDate"] is DateTime ? (DateTime)dr["LastAnnualTechnicalReviewDate"] : (DateTime?)null);
            vehicle.Seats = dr["Seats"].ToString();
            vehicle.Mileage = (DBCommon.IsDecimal(dr["Mileage"]) ? DBCommon.GetDecimal(dr["Mileage"]) : (decimal?)null);
            vehicle.VehicleEngineTypeId = (DBCommon.IsInt(dr["VehicleEngineTypeID"]) ? DBCommon.GetInt(dr["VehicleEngineTypeID"]) : (int?)null);
            vehicle.VehicleBodyTypeId = (DBCommon.IsInt(dr["VehicleBodyTypeID"]) ? DBCommon.GetInt(dr["VehicleBodyTypeID"]) : (int?)null);

            return vehicle;
        }

        //Get a particular object by its ID
        public static Vehicle GetVehicle(int vehicleId, User currentUser)
        {
            Vehicle vehicle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VehicleID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.VehicleMakeName, a.VehicleModelName, a.VehicleKindID, a.VehicleRoadabilityID,
                                  a.CarryingCapacity, a.LoadingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Seats, a.Mileage,
                                  a.VehicleEngineTypeID, a.VehicleBodyTypeID
                               FROM PMIS_RES.Vehicles a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.VehicleID = :VehicleID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleID", OracleType.Number).Value = vehicleId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vehicle = ExtractVehicle(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicle;
        }

        //Get a particular object by its ID
        public static Vehicle GetVehicleByTechnicsId(int technicsId, User currentUser)
        {
            Vehicle vehicle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VehicleID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.VehicleMakeName, a.VehicleModelName, a.VehicleKindID, a.VehicleRoadabilityID,
                                  a.CarryingCapacity, a.LoadingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Seats, a.Mileage,
                                  a.VehicleEngineTypeID, a.VehicleBodyTypeID
                               FROM PMIS_RES.Vehicles a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vehicle = ExtractVehicle(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicle;
        }

        //Get a particular object by its reg number
        public static Vehicle GetVehicleByRegNumber(string regNumber, User currentUser)
        {
            Vehicle vehicle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VehicleID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.VehicleMakeName, a.VehicleModelName, a.VehicleKindID, a.VehicleRoadabilityID,
                                  a.CarryingCapacity, a.LoadingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Seats, a.Mileage,
                                  a.VehicleEngineTypeID, a.VehicleBodyTypeID
                               FROM PMIS_RES.Vehicles a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RegNumber = :RegNumber 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RegNumber", OracleType.VarChar).Value = regNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vehicle = ExtractVehicle(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicle;
        }

        //Get all Vehicle objects
        public static List<Vehicle> GetAllVehicles(User currentUser)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VehicleID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.VehicleMakeName, a.VehicleModelName, a.VehicleKindID, a.VehicleRoadabilityID,
                                  a.CarryingCapacity, a.LoadingCapacity, 
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Seats, a.Mileage,
                                  a.VehicleEngineTypeID, a.VehicleBodyTypeID
                               FROM PMIS_RES.Vehicles a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vehicles.Add(ExtractVehicle(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicles;
        }

        public static List<VehicleFulfilmentBlock> GetAllVehicleFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VehicleFulfilmentBlock> vehicles = new List<VehicleFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND f.CreatedBy = " + currentUser.UserId.ToString();
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
                        orderBySQL = "b.VehicleMakeName || ' ' || b.VehicleModelName";
                        break;
                    case 3:
                        orderBySQL = "e.TableValue";
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
                        orderBySQL = "c.IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.VehicleID, 
                                      tmp.TechnicsID, 
                                      tmp.RegNumber, 
                                      tmp.InventoryNumber,
                                      tmp.VehicleMakeName, 
                                      tmp.VehicleModelName, 
                                      tmp.VehicleKindID, 
                                      tmp.VehicleRoadabilityID,
                                      tmp.CarryingCapacity,
                                      tmp.LoadingCapacity,
                                      tmp.FirstRegistrationDate,
                                      tmp.LastAnnualTechnicalReviewDate,
                                      tmp.Seats,
                                      tmp.Mileage,
                                      tmp.VehicleEngineTypeID,
                                      tmp.VehicleBodyTypeID,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.VehicleID, 
                                             b.TechnicsID, 
                                             b.RegNumber, 
                                             b.InventoryNumber,
                                             b.VehicleMakeName, 
                                             b.VehicleModelName, 
                                             b.VehicleKindID, 
                                             b.VehicleRoadabilityID,
                                             b.CarryingCapacity,
                                             b.LoadingCapacity,
                                             b.FirstRegistrationDate,
                                             b.LastAnnualTechnicalReviewDate,
                                             b.Seats,
                                             b.Mileage,
                                             b.VehicleEngineTypeID,
                                             b.VehicleBodyTypeID,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Vehicles b ON a.TechnicsID = b.TechnicsID";

                                      //LEFT OUTER JOIN PMIS_RES.VehicleMakes c ON b.VehicleMakeID = c.VehicleMakeID
                                      //LEFT OUTER JOIN PMIS_RES.VehicleModels d ON b.VehicleModelID = d.VehicleModelID
                SQL += @"
                                      LEFT OUTER JOIN PMIS_RES.GTable e ON b.VehicleKindID = e.TableKey AND e.TableName = 'VehicleKind'
                                      INNER JOIN PMIS_RES.Technics f ON b.TechnicsID = f.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON f.NormativeTechnicsID = n.NormativeTechnicsID
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
                    VehicleFulfilmentBlock block = new VehicleFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.VehicleID = DBCommon.GetInt(dr["VehicleID"]);
                    block.Vehicle = ExtractVehicle(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    vehicles.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicles;
        }

        public static int GetAllVehicleFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int vehicles = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Vehicles b ON a.TechnicsID = b.TechnicsID
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
                    vehicles = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicles;
        }

        //Save a particular object into the DB
        public static bool SaveVehicle(Vehicle vehicle, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + vehicle.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (vehicle.VehicleId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.Vehicles (TechnicsID, RegNumber, 
                                InventoryNumber, VehicleMakeName, VehicleModelName, 
                                VehicleKindID, VehicleRoadabilityID,
                                CarryingCapacity, LoadingCapacity, 
                                FirstRegistrationDate, LastAnnualTechnicalReviewDate,
                                Seats, Mileage, VehicleEngineTypeID, VehicleBodyTypeID)
                            VALUES (:TechnicsID, :RegNumber, 
                                :InventoryNumber, :VehicleMakeName, :VehicleModelName, 
                                :VehicleKindID, :VehicleRoadabilityID,
                                :CarryingCapacity, :LoadingCapacity, 
                                :FirstRegistrationDate, :LastAnnualTechnicalReviewDate,
                                :Seats, :Mileage, :VehicleEngineTypeID, :VehicleBodyTypeID);

                            SELECT PMIS_RES.Vehicles_ID_SEQ.currval INTO :VehicleID FROM dual;

                            INSERT INTO PMIS_RES.VehicleRegNumbers (VehicleID, 
                               RegNumber, ChangeDate)
                            VALUES (:VehicleID, 
                               :RegNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VEHICLES_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_RegNumber", "", vehicle.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_InventoryNumber", "", vehicle.InventoryNumber, currentUser));

                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleMake", "", vehicle.VehicleMakeId.HasValue ? vehicle.VehicleMake.VehicleMakeName : "", currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleModel", "", vehicle.VehicleModelId.HasValue ? vehicle.VehicleModel.VehicleModelName : "", currentUser));
                    
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleMake", "", vehicle.VehicleMakeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleModel", "", vehicle.VehicleModelName, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleKind", "", vehicle.VehicleKindId.HasValue ? vehicle.VehicleKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleRoadability", "", vehicle.VehicleRoadabilityId.HasValue ? vehicle.VehicleRoadability.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_CarryingCapacity", "", vehicle.CarryingCapacity.HasValue ? vehicle.CarryingCapacity.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_FirstRegistrationDate", "", CommonFunctions.FormatDate(vehicle.FirstRegistrationDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_LoadingCapacity", "", vehicle.LoadingCapacity.HasValue ? vehicle.LoadingCapacity.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_LastAnnualTechnicalReviewDate", "", CommonFunctions.FormatDate(vehicle.LastAnnualTechnicalReviewDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_Seats", "", vehicle.Seats, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_Mileage", "", vehicle.Mileage.HasValue ? vehicle.Mileage.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleEngineType", "", vehicle.VehicleEngineTypeId.HasValue ? vehicle.VehicleEngineType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleBodyType", "", vehicle.VehicleBodyTypeId.HasValue ? vehicle.VehicleBodyType.TableValue : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.Vehicles SET
                               RegNumber = :RegNumber,
                               InventoryNumber = :InventoryNumber,
                               VehicleMakeName = :VehicleMakeName,
                               VehicleModelName = :VehicleModelName,
                               VehicleKindID = :VehicleKindID,
                               VehicleRoadabilityID = :VehicleRoadabilityID,
                               CarryingCapacity = :CarryingCapacity,
                               LoadingCapacity = :LoadingCapacity,
                               FirstRegistrationDate = :FirstRegistrationDate,
                               LastAnnualTechnicalReviewDate = :LastAnnualTechnicalReviewDate,
                               Seats = :Seats,
                               Mileage = :Mileage,
                               VehicleEngineTypeID = :VehicleEngineTypeID,
                               VehicleBodyTypeID = :VehicleBodyTypeID
                             WHERE VehicleID = :VehicleID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VEHICLES_Edit", logDescription, null, null, currentUser);

                    Vehicle oldVehicle = GetVehicle(vehicle.VehicleId, currentUser);

                    if (oldVehicle.RegNumber.Trim() != vehicle.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_RegNumber", oldVehicle.RegNumber, vehicle.RegNumber, currentUser));

                    if (oldVehicle.InventoryNumber.Trim() != vehicle.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_InventoryNumber", oldVehicle.InventoryNumber, vehicle.InventoryNumber, currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleMakeId, vehicle.VehicleMakeId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleMake", oldVehicle.VehicleMakeId.HasValue ? oldVehicle.VehicleMake.VehicleMakeName : "", vehicle.VehicleMakeId.HasValue ? vehicle.VehicleMake.VehicleMakeName : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleModelId, vehicle.VehicleModelId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleModel", oldVehicle.VehicleModelId.HasValue ? oldVehicle.VehicleModel.VehicleModelName : "", vehicle.VehicleModelId.HasValue ? vehicle.VehicleModel.VehicleModelName : "", currentUser));
                    
                    if (oldVehicle.VehicleMakeName.Trim() != vehicle.VehicleMakeName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleMake", oldVehicle.VehicleMakeName, vehicle.VehicleMakeName, currentUser));

                    if (oldVehicle.VehicleModelName.Trim() != vehicle.VehicleModelName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleModel", oldVehicle.VehicleModelName, vehicle.VehicleModelName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleKindId, vehicle.VehicleKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleKind", oldVehicle.VehicleKindId.HasValue ? oldVehicle.VehicleKind.TableValue : "", vehicle.VehicleKindId.HasValue ? vehicle.VehicleKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleRoadabilityId, vehicle.VehicleRoadabilityId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleRoadability", oldVehicle.VehicleRoadabilityId.HasValue ? oldVehicle.VehicleRoadability.TableValue : "", vehicle.VehicleRoadabilityId.HasValue ? vehicle.VehicleRoadability.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVehicle.CarryingCapacity, vehicle.CarryingCapacity))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_CarryingCapacity", oldVehicle.CarryingCapacity.HasValue ? oldVehicle.CarryingCapacity.ToString() : "", vehicle.CarryingCapacity.HasValue ? vehicle.CarryingCapacity.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldVehicle.FirstRegistrationDate, vehicle.FirstRegistrationDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_FirstRegistrationDate", CommonFunctions.FormatDate(oldVehicle.FirstRegistrationDate), CommonFunctions.FormatDate(vehicle.FirstRegistrationDate), currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVehicle.LoadingCapacity, vehicle.LoadingCapacity))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_LoadingCapacity", oldVehicle.LoadingCapacity.HasValue ? oldVehicle.LoadingCapacity.ToString() : "", vehicle.LoadingCapacity.HasValue ? vehicle.LoadingCapacity.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldVehicle.LastAnnualTechnicalReviewDate, vehicle.LastAnnualTechnicalReviewDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_LastAnnualTechnicalReviewDate", CommonFunctions.FormatDate(oldVehicle.LastAnnualTechnicalReviewDate), CommonFunctions.FormatDate(vehicle.LastAnnualTechnicalReviewDate), currentUser));

                    if (oldVehicle.Seats.Trim() != vehicle.Seats.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_Seats", oldVehicle.Seats, vehicle.Seats, currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVehicle.Mileage, vehicle.Mileage))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_Mileage", oldVehicle.Mileage.HasValue ? oldVehicle.Mileage.ToString() : "", vehicle.Mileage.HasValue ? vehicle.Mileage.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleEngineTypeId, vehicle.VehicleEngineTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleEngineType", oldVehicle.VehicleEngineTypeId.HasValue ? oldVehicle.VehicleEngineType.TableValue : "", vehicle.VehicleEngineTypeId.HasValue ? vehicle.VehicleEngineType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVehicle.VehicleBodyTypeId, vehicle.VehicleBodyTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_VehicleBodyType", oldVehicle.VehicleBodyTypeId.HasValue ? oldVehicle.VehicleBodyType.TableValue : "", vehicle.VehicleBodyTypeId.HasValue ? vehicle.VehicleBodyType.TableValue : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(vehicle.Technics, currentUser, changeEvent);
                vehicle.TechnicsId = vehicle.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVehicleID = new OracleParameter();
                paramVehicleID.ParameterName = "VehicleID";
                paramVehicleID.OracleType = OracleType.Number;

                if (vehicle.VehicleId != 0)
                {
                    paramVehicleID.Direction = ParameterDirection.Input;
                    paramVehicleID.Value = vehicle.VehicleId;
                }
                else
                {
                    paramVehicleID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVehicleID);

                OracleParameter param = null;

                if (vehicle.VehicleId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = vehicle.TechnicsId;
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
                if (!string.IsNullOrEmpty(vehicle.RegNumber))
                    param.Value = vehicle.RegNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vehicle.InventoryNumber))
                    param.Value = vehicle.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "VehicleMakeID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (vehicle.VehicleMakeId.HasValue)
                //    param.Value = vehicle.VehicleMakeId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "VehicleModelID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (vehicle.VehicleModelId.HasValue)
                //    param.Value = vehicle.VehicleModelId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleMakeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vehicle.VehicleMakeName))
                    param.Value = vehicle.VehicleMakeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vehicle.VehicleModelName))
                    param.Value = vehicle.VehicleModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.VehicleKindId.HasValue)
                    param.Value = vehicle.VehicleKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleRoadabilityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.VehicleRoadabilityId.HasValue)
                    param.Value = vehicle.VehicleRoadabilityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CarryingCapacity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.CarryingCapacity.HasValue)
                    param.Value = vehicle.CarryingCapacity.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LoadingCapacity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.LoadingCapacity.HasValue)
                    param.Value = vehicle.LoadingCapacity.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FirstRegistrationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (vehicle.FirstRegistrationDate.HasValue)
                    param.Value = vehicle.FirstRegistrationDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastAnnualTechnicalReviewDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (vehicle.LastAnnualTechnicalReviewDate.HasValue)
                    param.Value = vehicle.LastAnnualTechnicalReviewDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seats";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vehicle.Seats))
                    param.Value = vehicle.Seats;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Mileage";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.Mileage.HasValue)
                    param.Value = vehicle.Mileage.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleEngineTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.VehicleEngineTypeId.HasValue)
                    param.Value = vehicle.VehicleEngineTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleBodyTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vehicle.VehicleBodyTypeId.HasValue)
                    param.Value = vehicle.VehicleBodyTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (vehicle.VehicleId == 0)
                    vehicle.VehicleId = DBCommon.GetInt(paramVehicleID.Value);

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

        public static List<VehicleManageBlock> GetAllVehicleManageBlocks(VehicleManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<VehicleManageBlock> vehicleManageBlocks = new List<VehicleManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
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
                        orderBySQL = "a.VehicleMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.VehicleModelName";
                        break;
                    case 7:
                        orderBySQL = "h.MilitaryDepartmentName";
                        break;
                    case 8:
                        orderBySQL = "g.TechMilitaryReportStatusName";
                        break;
                    case 9:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 10:
                        orderBySQL = "PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address)";
                        break;
                    case 11:
                        orderBySQL = "k.TableValue";
                        break;
                    case 12:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.VehicleID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as VehicleKind,
                                       a.VehicleMakeName,
                                       a.VehicleModelName,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       k.TableValue as BodyType,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Vehicles a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.VehicleKindId = d.TableKey AND d.TableName = 'VehicleKind'";
                                
                                //LEFT OUTER JOIN PMIS_RES.VehicleMakes e ON a.VehicleMakeId = e.VehicleMakeId
                                //LEFT OUTER JOIN PMIS_RES.VehicleModels f ON a.VehicleModelId = f.VehicleModelId

                SQL += @"
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.GTable k ON a.VehicleBodyTypeID = k.TableKey AND k.TableName = 'VehicleBodyType'
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
                    VehicleManageBlock vehicleManageBlock = new VehicleManageBlock();

                    vehicleManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    vehicleManageBlock.VehicleId = DBCommon.GetInt(dr["VehicleID"]);
                    vehicleManageBlock.RegNumber = dr["RegNumber"].ToString();
                    vehicleManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    vehicleManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    vehicleManageBlock.VehicleKind = dr["VehicleKind"].ToString();
                    vehicleManageBlock.BodyType = dr["BodyType"].ToString();
                    vehicleManageBlock.VehicleMakeName = dr["VehicleMakeName"].ToString();
                    vehicleManageBlock.VehicleModelName = dr["VehicleModelName"].ToString();
                    vehicleManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    vehicleManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    vehicleManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        vehicleManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        vehicleManageBlock.Ownership = "";
                    }

                    vehicleManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    vehicleManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    vehicleManageBlocks.Add(vehicleManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleManageBlocks;
        }

        public static int GetAllVehicleManageBlocksCount(VehicleManageFilter filter, User currentUser)
        {
            int vehicleManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
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
                                FROM PMIS_RES.Vehicles a
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
                        vehicleManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleManageBlocksCnt;
        }

        public static List<VehicleSearchBlock> GetAllVehicleSearchBlocks(VehicleSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<VehicleSearchBlock> vehicleSearchBlocks = new List<VehicleSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
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
                        orderBySQL = "a.VehicleMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.VehicleModelName";
                        break;
                    case 7:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 8:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.VehicleID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as VehicleKind,
                                       a.VehicleMakeName,
                                       a.VehicleModelName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Vehicles a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.VehicleKindId = d.TableKey AND d.TableName = 'VehicleKind'";

                                //LEFT OUTER JOIN PMIS_RES.VehicleMakes e ON a.VehicleMakeId = e.VehicleMakeId
                                //LEFT OUTER JOIN PMIS_RES.VehicleModels f ON a.VehicleModelId = f.VehicleModelId
                SQL += @"
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
                    VehicleSearchBlock vehicleSearchBlock = new VehicleSearchBlock();

                    vehicleSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    vehicleSearchBlock.VehicleId = DBCommon.GetInt(dr["VehicleID"]);
                    vehicleSearchBlock.RegNumber = dr["RegNumber"].ToString();
                    vehicleSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    vehicleSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    vehicleSearchBlock.VehicleKind = dr["VehicleKind"].ToString();
                    vehicleSearchBlock.VehicleMake = dr["VehicleMakeName"].ToString();
                    vehicleSearchBlock.VehicleModel = dr["VehicleModelName"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        vehicleSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        vehicleSearchBlock.Ownership = "";
                    }

                    vehicleSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    vehicleSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    vehicleSearchBlocks.Add(vehicleSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleSearchBlocks;
        }

        public static int GetAllVehicleSearchBlocksCount(VehicleSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int vehicleSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
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
                                FROM PMIS_RES.Vehicles a
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
                        vehicleSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeRegNumber(int vehicleId, string newRegNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Vehicle vehicle = GetVehicle(vehicleId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + vehicle.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.Vehicles SET
                              RegNumber = :NewRegNumber
                           WHERE VehicleID = :VehicleID;

                           INSERT INTO PMIS_RES.VehicleRegNumbers (VehicleID, 
                              RegNumber, ChangeDate)
                           VALUES (:VehicleID, 
                              :NewRegNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeRegNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VEHICLES_RegNumber", vehicle.RegNumber, newRegNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "VehicleID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vehicleId;
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

                TechnicsUtil.SetTechnicsModified(vehicle.TechnicsId, currentUser);

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

        //Get all reg number (history) by Vehicle with pagination
        public static List<VehicleRegNumber> GetAllVehicleRegNumbers(int vehicleId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VehicleRegNumber> vehicleRegNumbers = new List<VehicleRegNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.VehicleRegNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.RegNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.VehicleRegNumberID) as RowNumber
                                               FROM PMIS_RES.VehicleRegNumbers a
                                               WHERE a.VehicleID = :VehicleID
                                               ORDER BY " + orderBySQL + @", a.VehicleRegNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleID", OracleType.Number).Value = vehicleId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VehicleRegNumber regNumber = new VehicleRegNumber();
                    regNumber.RegNumber = dr["RegNumber"].ToString();
                    regNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    vehicleRegNumbers.Add(regNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleRegNumbers;
        }

        //Get all reg number (history) count by Vehicle for pagination
        public static int GetAllVehicleRegNumbersCount(int vehicleId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.VehicleRegNumbers a
                               WHERE a.VehicleID = :VehicleID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleID", OracleType.Number).Value = vehicleId;

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